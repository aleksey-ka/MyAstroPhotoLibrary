using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using AstroPhoto.LibRaw;

namespace MyAstroPhotoLibrary
{
    public partial class MainForm : Form
    {
        static class Program
        {
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault( false );
                Application.Run( new MainForm() );
            }
        }
        
        Session currentSession;
        StackItem[] currentStack;
        int currentStackIndex;

        IEnumerable<StackItem> CurrentStack
        {
            get
            {
                foreach( var item in currentStack ) {
                    if( !item.IsExcluded ) {
                        yield return item;
                    }
                }
            }
        }
        
        IImage currentImage;
        int zoomCenterX = -1;
        int zoomCenterY = -1;
        Rectangle currentZoomRect;

        AstroPhoto.LibRaw.Instance libraw = new AstroPhoto.LibRaw.Instance();
        Curve currentCurve = new LinearCurve();

        RgbImage stackedZoom;
        int stackedZoomCount;
        Curve stackedZoomCurve = new LinearCurve();

        RgbImage stackedImage;
        int stackedImageCount;
        Curve stackedImageCurve = new LinearCurve();

        //PictureBoxOverlay pictureBoxOverlay;
        
        public MainForm()
        {
            InitializeComponent();
            
            pictureBox.Cursor = new Cursor( "CrossHair.cur" );
            zoomPictureBox.Cursor = pictureBox.Cursor;

            thumbnailsView.ItemContextMenu = thumbnailsContextMenu; 

            //pictureBoxOverlay = new PictureBoxOverlay( pictureBox );
            //pictureBoxOverlay.AddBox( 0, new Rectangle( 100, 100, 200, 200 ),
            //    Color.DodgerBlue, false );
        }

        private void MainForm_Shown( object sender, EventArgs e )
        {
            fullScreen( true );
            
            string rootPath = "";
            if( Properties.Settings.Default.LibraryPath.Length > 0 ) {
                rootPath = Properties.Settings.Default.LibraryPath;
            }
            if( !Directory.Exists( rootPath ) ) {
                if( openFolderDialog( ref rootPath ) != DialogResult.OK ) {
                    Close();
		            return;
                }
            }

            currentSession = new Session( rootPath );
            thumbnailsView.OpenSession( currentSession );
        }

        private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            Properties.Settings.Default.LibraryPath = currentSession.RootPath;
            Properties.Settings.Default.Save();
        }

        private DialogResult openFolderDialog( ref string rootPath )
        {
            using( var dialog = new OpenFolderDialog() ) {
                dialog.InitialFolder = rootPath;
                var result = dialog.ShowDialog( this );
                if( result == DialogResult.OK ) {
                    rootPath = dialog.Folder;
                } else {
                    rootPath = "";
                }
                return result;
            }
        }

        private void openFolder_Click( object sender, EventArgs e )
        {
            string rootPath = currentSession.RootPath;
            if( openFolderDialog( ref rootPath ) == DialogResult.OK ) {
                refreshButton.Enabled = false;
                currentSession = new Session( rootPath );
                thumbnailsView.OpenSession( currentSession );
            }
        }

        private void refreshButton_Click( object sender, EventArgs e )
        {
            refreshButton.Enabled = false;
            thumbnailsView.OpenSession( currentSession );
        }

        void thumbnailsView_LoadThumbnailsCompleted( object sender, EventArgs e )
        {
            refreshButton.Enabled = true;
        }

        private void createStack_Click( object sender, EventArgs e )
        {
            using( var inputDialog = new InputDialog( "New Stack", "Name:", "", null ) ) {
                if( inputDialog.ShowDialog() == DialogResult.OK ) {
                    thumbnailsView.CreateStack( libraw, inputDialog.InputText );
                }
            }
        }

        void thumbnailsView_SelectionChanged( object sender, EventArgs e )
        {
            if( currentImage != null ) {
                currentImage.Dispose();
                currentImage = null;
            }
            if( stackedImage != null ) {
                stackedImage.Dispose();
                stackedImage = null;
            }
            if( stackedZoom != null ) {
                stackedZoom.Dispose();
                stackedZoom = null;
            }
            currentStack = null;
            currentStackIndex = 0;
            zoomCenterX = -1;
            zoomCenterY = -1;

            BgRnumericUpDown.Value = 0;
            BgGnumericUpDown.Value = 0;
            BgBnumericUpDown.Value = 0;
            MaxVNumericUpDown.Value = 1024;
            BgNumericUpDown.Value = 0;
            KgNumericUpDown.Value = 135;
            KbNumericUpDown.Value = 245;
            gammaNumericUpDown.Value = 0;
            satNumericUpDown.Value = 0;
            recalcCurve();

            hideStackPanel();
            var start = DateTime.Now;
            updateImageView();
            System.Diagnostics.Trace.WriteLine( string.Format( "TIMER: {0} ms", ( DateTime.Now - start ).TotalMilliseconds ) );

            stackButton.Enabled = thumbnailsView.SelectedImages.Any();
        }

        private void viewMode_Click( object sender, EventArgs e )
        {
            updateImageView();
        }

        private void prepareStack( string rawPath )
        {
            if( currentStack == null ) {
                string stackFile = rawPath + "\\stack.txt";
                if( File.Exists( stackFile ) ) {
                    var lines = File.ReadAllLines( stackFile );
                    int linesPerRecord = 0;
                    for( int i = 0; i < lines.Length; i++ ) {
                        if( lines[i].StartsWith( "#" ) ) {
                            linesPerRecord++;
                            continue;
                        }
                        currentStack = new StackItem[( lines.Length - i ) / linesPerRecord];
                        break;
                    }
                    for( int i = 0; i < currentStack.Length; i++ ) {
                        string fileName = lines[( i + 1 ) * linesPerRecord + 0];
                        string[] offset = lines[( i + 1 ) * linesPerRecord + 1].Split( ',' );
                        currentStack[i] = new StackItem()
                        {
                            FilePath = Path.Combine( rawPath, fileName ),
                            Offset = new Point( int.Parse( offset[0] ), int.Parse( offset[1] ) )
                        };
                    }
                    zoomCenterX = currentStack[0].Offset.X;
                    zoomCenterY = currentStack[0].Offset.Y;
                } else {
                    var files = Directory.GetFiles( rawPath );
                    currentStack = new StackItem[files.Length];
                    for( int i = 0; i < files.Length; i++ ) {
                        currentStack[i] = new StackItem() { FilePath = files[i] };
                    }
                }
            }
        }

        private void updateImageView()
        {
            if( stackedImage == null ) {
                if( currentImage == null ) {
                    var path = thumbnailsView.CurrentObjectPath;
                    if( path.Length > 0 ) {
                        if( File.Exists( path ) ) {
                            currentImage = ImageTools.LoadImage( libraw, path, currentSession );
                            currentStack = null;
                            positionLabel.Text = "...";
                        } else if( Directory.Exists( path ) ) {
                            string cfaPath = path + "\\CFA";
                            if( Directory.Exists( cfaPath ) ) {
                                prepareStack( cfaPath );
                                currentImage = new RawImageWrapper( currentStack[currentStackIndex].FilePath, currentSession );
                                positionLabel.Text = string.Format( "{0} of {1}", currentStackIndex + 1, currentStack.Length );
                                stackCheckBox.Visible = true;
                                stackSelectedCheckBox.Checked = !currentStack[currentStackIndex].IsExcluded;
                                stackSelectedCheckBox.Enabled = true;
                            } else {
                                string rawPath = path + "\\RAW";
                                if( Directory.Exists( rawPath ) ) {
                                    prepareStack( rawPath );
                                    currentImage = new RawImageWrapper( libraw, currentStack[currentStackIndex].FilePath, currentSession );
                                    positionLabel.Text = string.Format( "{0} of {1}", currentStackIndex + 1, currentStack.Length );
                                    stackCheckBox.Visible = true;
                                    stackSelectedCheckBox.Checked = !currentStack[currentStackIndex].IsExcluded;
                                    stackSelectedCheckBox.Enabled = true;
                                }
                            }
                        }
                    } else {
                        stackSelectedCheckBox.Enabled = true;
                    }
                }

                if( currentImage != null ) {
                    if( quickPreviewRadioButton.Checked ) {
                        pictureBox.Image = currentImage.Preview;
                        showZoomImage();
                    } else if( rawPreviewRadioButton.Checked ) {
                        RawImage rawImage = currentImage.RawImage;
                        if( rawImage != null ) {
                            int saturation = (int) satNumericUpDown.Value;
                            pictureBox.Image = rawImage.RenderBitmapHalfRes( currentCurve, saturation );
                            showZoomImage();
                        } else {
                            pictureBox.Image = null;
                        }
                    }
                    if( infoRadioButton.Checked ) {
                        RawImage rawImage = currentImage.RawImage;
                        if( rawImage != null ) {
                            pictureBox.Image = rawImage.GetHistogram();
                            showZoomImage();
                        } else {
                            pictureBox.Image = null;
                        }
                    }
                } else {
                    pictureBox.Image = null;
                }

                updateStackView();
                stackCheckBox.Visible = currentStack != null;
                prevButton.Enabled = ( currentStack != null && currentStackIndex > 0 );
                nextButton.Enabled = ( currentStack != null && currentStackIndex < currentStack.Length - 1 );
            } else {
                if( pictureBox.Image != null ) {
                    pictureBox.Image.Dispose();
                }
                int saturation = (int) satNumericUpDown.Value;
                pictureBox.Image = stackedImage.RenderBitmap( stackedImageCurve, saturation );
                showZoomImage();
            }
        }

        private void showZoomImage()
        {
            if( noZoomRadioButton.Checked ) {
                zoomPanel.Visible = false;
            } else if( currentImage != null ) {
                Size rawSize = currentImage.RawSize;
                int scale = 1;
                if( zoom2xRadioButton.Checked ) scale = 2;
                if( zoom4xRadioButton.Checked ) scale = 4;
                if( zoomCFARadioButton.Checked ) scale = 4;

                int X = zoomCenterX;
                int Y = zoomCenterY;
                if( X == -1 ) {
                    X = rawSize.Width / 2;
                }
                if( Y == -1 ) {
                    Y = rawSize.Height / 2;
                }
                
                int width = zoomPictureBox.ClientSize.Width;
                int height = zoomPictureBox.ClientSize.Height;

                int W = width / scale;
                int H = height / scale;

                if( W % 2 == 1 ) W += 1;
                if( H % 2 == 1 ) H += 1;
                 
                currentZoomRect =  new Rectangle( X -  W / 2, Y - H / 2, W, H );

                width = W * scale;
                height = H * scale;

                var image = new Bitmap( width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
                using( Graphics g = Graphics.FromImage( image ) ) {
                    if( stackedZoom != null ) {
                        int saturation = (int) satNumericUpDown.Value;
                        using( var renderedImage = stackedZoom.RenderBitmap( stackedZoomCurve, saturation ) ) {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            g.DrawImage( renderedImage,
                                new Rectangle( 0, 0, width, height ),
                                new Rectangle( 0, 0, renderedImage.Width, renderedImage.Height ),
                                GraphicsUnit.Pixel );
                        }
                    } else if( zoomCFARadioButton.Checked ) {
                        var rawImage = currentImage.RawImage;
                        using( var extractedImage = rawImage.RenderCFA( currentZoomRect, currentCurve ) ) {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            g.DrawImage( extractedImage,
                                new Rectangle( 0, 0, width, height ),
                                new Rectangle( 0, 0, extractedImage.Width, extractedImage.Height ),
                                GraphicsUnit.Pixel );
                        }
                        
                    } else if( rawPreviewRadioButton.Checked ) {
                        var rawImage = currentImage.RawImage;
                        int saturation = (int)satNumericUpDown.Value;
                        using( var extractedImage = rawImage.RenderBitmap( currentZoomRect, currentCurve, saturation ) ) {
                            //using( var gx = Graphics.FromImage( extractedImage ) ) {
                            /*gx.DrawLine( Pens.Magenta,
                                1,
                                1,
                                currentZoomRect.Width - 1,
                                currentZoomRect.Height - 1);
                            gx.DrawLine( Pens.Magenta,
                                currentZoomRect.Width - 1,
                                1,
                                1,
                                currentZoomRect.Height - 1);*/
                            /*gx.DrawLine( Pens.Lime,
                                currentZoomRect.Width / 2 - 1,
                                currentZoomRect.Height / 2,
                                currentZoomRect.Width / 2 + 1,
                                currentZoomRect.Height / 2);
                            gx.DrawLine( Pens.Lime,
                                currentZoomRect.Width / 2,
                                currentZoomRect.Height / 2 - 1,
                                currentZoomRect.Width / 2,
                                currentZoomRect.Height / 2 + 1 );*/
                            /*gx.FillRectangle( Brushes.Lime,
                                currentZoomRect.Width / 2,
                                currentZoomRect.Height / 2,
                                1,
                                1 );*/
                            //}
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            g.DrawImage( extractedImage,
                                new Rectangle( 0, 0, width, height ),
                                new Rectangle( 0, 0, extractedImage.Width, extractedImage.Height ),
                                GraphicsUnit.Pixel );
                        }
                    } else {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        var srcImage = currentImage.Preview;
                        var srcSize = srcImage.Size;
                        X = ( X * srcSize.Width ) / rawSize.Width;
                        Y = ( Y * srcSize.Width ) / rawSize.Width;
                        W = ( W * srcSize.Width ) / rawSize.Width;
                        H = ( H * srcSize.Width ) / rawSize.Width;
                        var srcRect = new Rectangle( X - W / 2, Y - H / 2, W, H );
                        g.DrawImage( srcImage, new Rectangle( 0, 0, width, height ), srcRect, GraphicsUnit.Pixel );
                    }
                    
                }
                if( zoomPictureBox.Image != null ) {
                    zoomPictureBox.Image.Dispose();
                }
                zoomPictureBox.Image = image;
                zoomPanel.Visible = true;
            }
        }

        private void showZoomAt( Point e )
        {
            if( stackedZoom != null ) {
                stackedZoom.Dispose();
                stackedZoom = null;
            }

            if( pictureBox.Image == null ) {
                return;
            }

            if( noZoomRadioButton.Checked ) {
                zoom1xRadioButton.Checked = true;
            }

            var clientSize = pictureBox.ClientSize;
            var rawSize = currentImage.RawSize;
            int dX = 0;
            int dY = 0;
            float scaleX = ( 1.0f * rawSize.Width ) / clientSize.Width;
            float scaleY = ( 1.0f * rawSize.Height ) / clientSize.Height;
            float scale = 1;
            if( scaleX > scaleY ) {
                scale = scaleX;
                dY = ( clientSize.Height - (int) ( rawSize.Height / scale ) ) / 2;
            } else {
                scale = scaleY;
                dX = ( clientSize.Width - (int) ( rawSize.Width / scale ) ) / 2;
            }
            zoomCenterX = (int)( ( e.X - dX ) * scale );
            zoomCenterY = (int)( ( e.Y - dY ) * scale );
            showZoomImage();
        }

        private void zoomPictureBox_Click( object sender, EventArgs e )
        {
            if( stackedZoom != null ) {
                stackedZoom.Dispose();
                stackedZoom = null;
            }
            
            var p = zoomPictureBox.PointToClient( Control.MousePosition );
            var clientSize = zoomPictureBox.ClientSize;
            var imageSize = zoomPictureBox.Image.Size;
            p.X += ( clientSize.Width - imageSize.Width ) / 2;
            p.Y += ( clientSize.Height - imageSize.Height ) / 2;
            int dX = ( ( p.X - imageSize.Width / 2 ) * currentZoomRect.Width ) / imageSize.Width;
            int dY = ( ( p.Y - imageSize.Height / 2 ) * currentZoomRect.Height ) / imageSize.Height;
            zoomCenterX += dX;
            zoomCenterY += dY;

            /*if( quickPreviewRadioButton.Checked ) {
                zoomCenterX = (int) System.Math.Round( newCenter.X );
                zoomCenterY = (int) System.Math.Round( newCenter.Y );
                label1.Text = newCenter.ToString() + " - " + ImageTools.StarSigma( currentImage, newCenter ).ToString();
            } else */

            bool refineCenter = ( Control.ModifierKeys & Keys.Shift ) != 0 && rawPreviewRadioButton.Checked;
            showZoomImage( refineCenter, true );
        }
        
        void showZoomImage( bool refineCenter, bool centerCursor )
        {   
            if( refineCenter ) {
                Point newCenter = ImageTools.RefineCenter( currentImage.RawImage, 
                    new Point( zoomCenterX, zoomCenterY ), currentZoomRect.Size );
                zoomCenterX = newCenter.X;
                zoomCenterY = newCenter.Y;
            }
            showZoomImage();
            if( centerCursor ) {
                var clientSize = zoomPictureBox.ClientSize;
                Cursor.Position = zoomPictureBox.PointToScreen( new Point( clientSize.Width / 2, clientSize.Width / 2 ) );
            }
        }

        private void zoomRadioButton_CheckedChanged( object sender, EventArgs e )
        {
            showZoomImage();
        }

        private void nextButton_Click( object sender, EventArgs e )
        {
            if( currentStackIndex < currentStack.Length - 1 ) {
                currentStackIndex++;
                if( currentImage != null ) {
                    currentImage.Dispose();
                    currentImage = null;
                }
                if( zoomCenterX != -1 && currentStack[currentStackIndex].Offset.X != 0 ) {
                    zoomCenterX += currentStack[currentStackIndex].Offset.X - currentStack[currentStackIndex - 1].Offset.X;
                    zoomCenterY += currentStack[currentStackIndex].Offset.Y - currentStack[currentStackIndex - 1].Offset.Y;
                }
                updateImageView();
            }
        }

        private void prevButton_Click( object sender, EventArgs e )
        {
            if( currentStackIndex > 0 ) {
                currentStackIndex--;
                if( currentImage != null ) {
                    currentImage.Dispose();
                    currentImage = null;
                }
                if( zoomCenterX != -1 ) {
                    zoomCenterX += currentStack[currentStackIndex].Offset.X - currentStack[currentStackIndex + 1].Offset.X;
                    zoomCenterY += currentStack[currentStackIndex].Offset.Y - currentStack[currentStackIndex + 1].Offset.Y;
                }
                updateImageView();
            }
        }

        private void stackSelectedCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            if( stackSelectedCheckBox.Enabled ) {
                currentStack[currentStackIndex].IsExcluded = !stackSelectedCheckBox.Checked;
            }
        }

        private void pictureBox_MouseDown( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Left ) {
                var start = DateTime.Now;
                showZoomAt( pictureBox.PointToClient( Control.MousePosition ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "TIMER ZOOM: {0} ms", ( DateTime.Now - start ).TotalMilliseconds ) );
                if( ( Control.ModifierKeys & Keys.Shift ) != 0 ) {
                    var size = zoomPictureBox.ClientSize;
                    Cursor.Position = zoomPictureBox.PointToScreen( new Point( size.Width / 2, size.Height / 2 ) );
                }
            }
        }

        void updateStackView()
        {
            if( currentStack != null  ) {
                if( stackPanel.Visible ) {
                    if( stackFlowPanel.Controls.Count == 0 ) {
                        for( int i = 0; i < currentStack.Length; i++ ) {
                            var exPanel = new Panel()
                            {
                                Height = 20,
                                Width = 253,
                                Padding = new Padding( 1 ),
                                Margin = new Padding( 0 ),
                                BackColor = stackFlowPanel.BackColor,
                                Tag = i
                            };
                            exPanel.Click += new EventHandler( stackItem_Click );
                            var panel = new Panel()
                            {
                                Dock = DockStyle.Fill,
                                BackColor = stackFlowPanel.BackColor,
                                Tag = i
                            };
                            panel.Click += new EventHandler( stackItem_Click );
                            var label = new Label()
                            {
                                Text = Path.GetFileName( currentStack[i].FilePath ),
                                AutoSize = true,
                                Tag = i
                            };
                            label.Click += new EventHandler( stackItem_Click );
                            panel.Controls.Add( label );
                            exPanel.Controls.Add( panel );
                            stackFlowPanel.Controls.Add( exPanel );
                        }
                    }
                    foreach( Control control in stackFlowPanel.Controls ) {
                        int index = (int) ( control.Tag );
                        if( index == currentStackIndex ) {
                            control.BackColor = Color.DodgerBlue;
                        } else {
                            control.BackColor = stackFlowPanel.BackColor;
                        }
                    }
                }
            } else {
                hideStackPanel();
            }
        }

        void hideStackPanel()
        {
            stackFlowPanel.Controls.Clear();
            stackCheckBox.Checked = false;
            stackPanel.Visible = false;
        }

        void stackItem_Click( object sender, EventArgs e )
        {
            currentStackIndex = (int)( ( (Control)sender ).Tag );
            updateStackView();
            if( currentImage != null ) {
                currentImage.Dispose();
                currentImage = null;
            }
            viewMode_Click( null, EventArgs.Empty );
        }

        private void stackCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            stackPanel.Visible = stackCheckBox.Checked;
            if( stackPanel.Visible ) {
                updateStackView();
            } else {
                stackFlowPanel.Controls.Clear();
            }
        }

        private void saveCurrentStack()
        {
            var lines = new System.Collections.Generic.List<string>();
            lines.Add( "#FILENAME" );
            lines.Add( "#OFFSET" );
            foreach( var stackItem in currentStack ) {
                lines.Add( Path.GetFileName( stackItem.FilePath ) );
                lines.Add( string.Format( "{0}, {1}", stackItem.Offset.X, stackItem.Offset.Y ) );
            }

            string rawPath = Path.GetDirectoryName( currentStack[0].FilePath );
            string stackFile = rawPath + "\\stack.txt";
            File.WriteAllLines( stackFile, lines.ToArray() );
        }

        private void imageInfo_Click( object sender, EventArgs e )
        {
            MessageBox.Show( currentImage.RawImage.ImageInfo );
        }

        private void exportToFITS_Click( object sender, EventArgs e )
        {
            foreach( var item in currentStack ) {
                var fileName = Path.GetFileNameWithoutExtension( item.FilePath );
                Export.ToFITS( libraw, item.FilePath, string.Format( "C:\\IRIS_TEMP\\{0}.fits", fileName ) );
            }
        }

        private void exportToIRIS_Click( object sender, EventArgs e )
        {
            Export.ToIris( libraw, currentStack );
        }

        private void MainForm_KeyDown( object sender, KeyEventArgs e )
        {
            if( e.Control ) {
                switch( e.KeyCode ) {
                    case Keys.A:
                        if( currentStack != null ) {
                            currentStack[currentStackIndex].Offset = new Point( zoomCenterX, zoomCenterY );
                            saveCurrentStack();
                        }
                        while( nextButton.Enabled ) {
                            var prevRaw = currentImage.RawImage.GetRawPixels();
                            var prevCenter = new Point( ( ( zoomCenterX ) / 2 ) * 2, ( ( zoomCenterY ) / 2 ) * 2 );
                            nextButton_Click( sender, e );
                            showZoomImage( true, true );
                            Application.DoEvents();
                            if( e.Shift ) {
                                double correlation = ImageTools.CalcCorrelation( currentImage.RawImage, prevRaw,
                                    new Point( ( zoomCenterX / 2 ) * 2, ( zoomCenterY / 2 ) * 2 ), prevCenter, currentZoomRect.Size );
                                System.Diagnostics.Trace.WriteLine( correlation.ToString() );
                                if( correlation < 0.95 ) {
                                    break;
                                } else {
                                    currentStack[currentStackIndex].Offset = new Point( zoomCenterX, zoomCenterY );
                                    saveCurrentStack();
                                }
                            } else {
                                break;
                            }
                        }
                        e.Handled = true;
                        break;
                    case Keys.X:
                        stackSelectedCheckBox.Checked = false;
                        if( nextButton.Enabled ) {
                            nextButton_Click( sender, e );
                        }
                        e.Handled = true;
                        break;
                    case Keys.Z:
                        {
                            var width = zoomPanel.Width;
                            var height = zoomPanel.Height;
                            if( e.Shift ) {
                                height /= 2;
                                width /= 2;
                                if( height < 448 ) {
                                    noZoomRadioButton.Checked = true;
                                } else {
                                    zoomPanel.Top += height;
                                    zoomPanel.Height -= height;
                                    zoomPanel.Left += width;
                                    zoomPanel.Width -= width;
                                }
                            } else {
                                if( currentImage != null && !zoomPanel.Visible ) {
                                    zoom1xRadioButton.Checked = true;
                                } else {
                                    if( height < 448 * 2 ) {
                                        zoomPanel.Top -= height;
                                        zoomPanel.Height += height;
                                        zoomPanel.Left -= width;
                                        zoomPanel.Width += width;
                                    }
                                }
                            }
                            showZoomImage();
                        }
                        e.Handled = true;
                        break;
                    case Keys.Right:
                        if( nextButton.Enabled ) {
                            nextButton_Click( sender, e );
                        }
                        e.Handled = true;
                        break;
                    case Keys.Left:
                        if( prevButton.Enabled ) {
                            prevButton_Click( sender, e );
                        }
                        e.Handled = true;
                        break;
                    case Keys.F:
                        mainPictureFullScreen( !exitFullScreenButton.Visible );
                        e.Handled = true;
                        break;
                    case Keys.Delete:
                        if( currentImage != null ) {
                            if( currentStack != null ) {
                                if( currentStackIndex >= 0 ) {
                                    // Удаляем картинку из стека
                                    removeFileFromStack();
                                }
                                if( currentStackIndex == -1 ) {
                                    // Удаляем стек
                                }
                            } else {
                                // Удаляем несгруппированную картинку
                            }
                        }
                        break;
                }
            }
        }

        private void removeFileFromStack()
        {
            int deleteIndex = currentStackIndex;
            if( currentStackIndex < currentStack.Length - 1 ) {
                nextButton_Click( null, EventArgs.Empty );
            } else {
                prevButton_Click( null, EventArgs.Empty );
            }

            var filePath = currentStack[deleteIndex].FilePath;
            var fileName = Path.GetFileName( filePath );
            var dirName = Path.GetDirectoryName( filePath );
            var _dirName = Path.GetDirectoryName( dirName );
            var trashDirName = _dirName + "\\TRASH";
            if( !Directory.Exists( trashDirName ) ) {
                Directory.CreateDirectory( trashDirName );
            }
            File.Move( filePath, Path.Combine( trashDirName, fileName ) );

            var newStack = new StackItem[currentStack.Length - 1];
            int j = 0;
            for( int i = 0; i < currentStack.Length; i++ ) {
                if( i != deleteIndex ) {
                    newStack[j] = currentStack[i];
                    j++;
                }
            }
            currentStack = newStack;
            if( currentStack.Length > 0 ) {
                if( currentStackIndex >= deleteIndex ) {
                    currentStackIndex--;
                }
                saveCurrentStack();
            } else {
                currentStack = null;
                currentStackIndex = -1;
                currentImage.Dispose();
                currentImage = null;
                Directory.Move( dirName, Path.Combine( trashDirName, Path.GetFileName( dirName ) ) );
                positionLabel.Text = "...";
            }
            updateImageView();
        }

        void check( bool condition, string message )
        {
            if( !condition ) {
                throw new Exception( message );
            }
        }

        private void recalcCurve()
        {
            ( (LinearCurve) currentCurve ).SetKg( (int) KgNumericUpDown.Value );
            ( (LinearCurve) currentCurve ).SetKb( (int) KbNumericUpDown.Value );
            ( (LinearCurve) currentCurve ).SetBg( (int) BgNumericUpDown.Value );
            ( (LinearCurve) currentCurve ).SetBgRGB( (int) BgRnumericUpDown.Value,
                (int) BgGnumericUpDown.Value, (int) BgBnumericUpDown.Value );
            ( (LinearCurve) currentCurve ).SetMaxV( (int) MaxVNumericUpDown.Value );
            ( (LinearCurve) currentCurve ).SetGamma( (int) gammaNumericUpDown.Value );

            ( (LinearCurve) stackedZoomCurve ).SetKg( (int) KgNumericUpDown.Value );
            ( (LinearCurve) stackedZoomCurve ).SetKb( (int) KbNumericUpDown.Value );
            ( (LinearCurve) stackedZoomCurve ).SetBg( (int) ( stackedZoomCount * BgNumericUpDown.Value ) );
            ( (LinearCurve) stackedZoomCurve ).SetBgRGB( (int) ( stackedZoomCount * BgRnumericUpDown.Value ),
                (int) ( stackedZoomCount * BgGnumericUpDown.Value), (int) ( stackedZoomCount * BgBnumericUpDown.Value ) );
            ( (LinearCurve) stackedZoomCurve ).SetMaxV( stackedZoomCount * (int) MaxVNumericUpDown.Value );
            ( (LinearCurve) stackedZoomCurve ).SetGamma( (int) gammaNumericUpDown.Value );

            ( (LinearCurve) stackedImageCurve ).SetKg( (int) KgNumericUpDown.Value );
            ( (LinearCurve) stackedImageCurve ).SetKb( (int) KbNumericUpDown.Value );
            ( (LinearCurve) stackedImageCurve ).SetBg( (int) ( stackedImageCount * BgNumericUpDown.Value ) );
            ( (LinearCurve) stackedImageCurve ).SetBgRGB( (int) ( stackedImageCount * BgRnumericUpDown.Value ),
                (int) ( stackedImageCount * BgGnumericUpDown.Value ), (int) ( stackedImageCount * BgBnumericUpDown.Value ) );
            ( (LinearCurve) stackedImageCurve ).SetMaxV( stackedImageCount * (int) MaxVNumericUpDown.Value );
            ( (LinearCurve) stackedImageCurve ).SetGamma( (int) gammaNumericUpDown.Value );
        }

        private void updateCurve()
        {
            recalcCurve();
            updateImageView();
        }

        private void curve_Changed( object sender, EventArgs e )
        {
            MaxVNumericUpDown.Increment = MaxVNumericUpDown.Value / 5;
            updateCurve();
        }

        private void satNumericUpDown_ValueChanged( object sender, EventArgs e )
        {
            updateImageView();
        }

        private void fullScreen( bool fullScreen )
        {
            if( fullScreen ) {
                //this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            } else {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                //this.TopMost = false;
            }
        }

        private void goFullScreen_Click( object sender, EventArgs e )
        {
            if( this.FormBorderStyle != FormBorderStyle.None ) {
                fullScreen( true );
            } else {
                fullScreen( false );
            }
        }

        private void close_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void toolsMenu_Click( object sender, EventArgs e )
        {
            toolsContextMenu.Show( toolsMenuButton, 0, toolsMenuButton.Height );
        }

        private void mainPictureViewSaveAs_Click( object sender, EventArgs e )
        {
            saveImageOpenFolder( pictureBox.Image, "Color.png" );
        }

        private void zoomSaveAs_Click( object sender, EventArgs e )
        {
            saveImageOpenFolder( zoomPictureBox.Image, "Zoom.png" );
        }

        private System.Drawing.Imaging.ImageCodecInfo getEncoder( System.Drawing.Imaging.ImageFormat format )
        {
            foreach( var codec in System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders() ) {
                if( codec.FormatID == format.Guid ) {
                    return codec;
                }
            }
            return null;
        }

        private void saveImageOpenFolder( Image image, string fileName )
        {
            using( var saveDialog = new SaveFileDialog() ) {
                saveDialog.Filter = "PNG|*.png|BMP - 24bit|*.bmp|JPEG|*.jpg";
                saveDialog.FileName = fileName;
                saveDialog.InitialDirectory = thumbnailsView.CurrentObjectPath;
                if( saveDialog.ShowDialog() == DialogResult.OK ) {
                    fullScreen( false );
                    Application.DoEvents();

                    switch( Path.GetExtension( saveDialog.FileName ).ToLower() ) {
                        case ".bmp": {
                                var encoderParams = new System.Drawing.Imaging.EncoderParameters( 1 );
                                encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(
                                    System.Drawing.Imaging.Encoder.ColorDepth, 24 );

                                image.Save( saveDialog.FileName, 
                                    getEncoder( System.Drawing.Imaging.ImageFormat.Bmp ), encoderParams );
                            }
                            break;
                        case ".png":
                            image.Save( saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png );
                            break;
                        case ".jpg": {
                                var encoderParams = new System.Drawing.Imaging.EncoderParameters( 1 );
                                encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(
                                    System.Drawing.Imaging.Encoder.Quality, 99L );

                                image.Save( saveDialog.FileName, 
                                    getEncoder( System.Drawing.Imaging.ImageFormat.Jpeg ), encoderParams );
                            };
                            break;

                    }

                    

                    System.Diagnostics.Process.Start( thumbnailsView.CurrentObjectPath );
                }
            }
        }

        private void mainPictureFullScreen( bool fullScreen )
        {
            if( fullScreen ) {
                splitContainer.Panel1Collapsed = true;
                previewToolsPanel.Visible = false;
                mainToolsPanel.Visible = false;
                exitFullScreenButton.Visible = true;
                fillPanel.BorderStyle = BorderStyle.None;
            } else {
                exitFullScreenButton.Visible = false;
                mainToolsPanel.Visible = true;
                previewToolsPanel.Visible = true;
                splitContainer.Panel1Collapsed = false;
                fillPanel.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void enterFullScreen_Click( object sender, EventArgs e )
        {
            mainPictureFullScreen( true );
        }

        private void exitFullScreen_Click( object sender, EventArgs e )
        {
            mainPictureFullScreen( false );
        }

        struct RawRegion {
            public RawImage RawImage;
            public Rectangle Rect;
            public string FilePath;
        }

        private IEnumerable<RawRegion> EnumerateRaw( Rectangle rect )
        {
            var currentOffset = currentStack[currentStackIndex].Offset;
            rect.Offset( -currentOffset.X, -currentOffset.Y );

            foreach( var item in currentStack ) {
                if( !item.IsExcluded ) {
                    using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                        var _rect = rect;
                        _rect.Offset( item.Offset );
                        //rawImage.ApplyDark( currentSession.Dark );
                        /*if( currentSession.Flat != null ) {
                            rawImage.ApplyFlat( currentSession.Flat, 1 );
                        }*/
                        yield return new RawRegion(){
                            RawImage = rawImage,
                            Rect = _rect,
                            FilePath = item.FilePath
                        };
                    }
                }
            }
        }

        struct ImageSaveAs {
            public Image Image;
            public string FilePath;
        }

        private IEnumerable<ImageSaveAs> EnumerateImageSaveAs( Rectangle rect )
        {
            int saturation = (int) satNumericUpDown.Value;
            foreach( var item in EnumerateRaw( rect ) ) {
                using( var rgbImage = item.RawImage.ExtractRgbImage( item.Rect ) ) {
                    using( var image = rgbImage.RenderBitmap( currentCurve, saturation ) ) {             
                        yield return new ImageSaveAs() { Image = image, FilePath = item.FilePath };
                    }
                }
            }
        }

        private string prepareFilePath( string srcFilePath, string folderName, string fileExtention )
        {
            var filePath = srcFilePath.Replace( ".ARW", fileExtention ).
                            Replace( "\\RAW\\", string.Format( "\\{0}\\", folderName ) );
            
            var dirName = filePath.Substring( 0, filePath.LastIndexOf( "\\" ) );
            if( !Directory.Exists( dirName ) ) {
                Directory.CreateDirectory( dirName );
            }

            return filePath;
        }

        private void saveStackToPng_Click( object sender, EventArgs e )
        {
            foreach( var item in EnumerateImageSaveAs( currentZoomRect ) ) {
                item.Image.Save( prepareFilePath( item.FilePath, "PNG", ".png" ),
                    System.Drawing.Imaging.ImageFormat.Png );
            }
        }

        private void saveStackToCFA_Click( object sender, EventArgs e )
        {
            foreach( var item in EnumerateRaw( currentZoomRect ) ) {
                Rectangle r = item.Rect;
                r.Inflate( 10, 10 );
                using( var zoomed = item.RawImage.ExtractRawImage( r ) ) {
                    zoomed.SaveCFA( prepareFilePath( item.FilePath, "CFA", ".cfa" ) );
                }
            }
        }
    }
}
