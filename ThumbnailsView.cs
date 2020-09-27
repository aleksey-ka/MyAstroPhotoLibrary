using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using AstroPhoto.LibRaw;

namespace MyAstroPhotoLibrary
{
    public class ThumbnailsView : FlowLayoutPanel
    {
        BackgroundWorker backgroundWorker;
        Session session;
        string currentObjectPath = "";
        List<string> selectedImages = new List<string>();

        public IEnumerable<string> SelectedImages { get { return selectedImages; } }
        public string CurrentObjectPath { get { return currentObjectPath; } }

        public event EventHandler<EventArgs> LoadThumbnailsCompleted;
        public event EventHandler<EventArgs> SelectionChanged;

        public ContextMenuStrip ItemContextMenu;
        
        public ThumbnailsView()
        {
            backgroundWorker = new BackgroundWorker() 
            { 
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += new DoWorkEventHandler( backgroundWorker_DoWork );
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler( backgroundWorker_ProgressChanged );
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            this.Click += new EventHandler( thumbnailsView_Click );
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing ) {
                backgroundWorker.Dispose();
            }
            base.Dispose( disposing );
        }

        public void OpenSession( Session _session )
        {
            if( backgroundWorker.IsBusy ) {
                backgroundWorker.CancelAsync();
                while( backgroundWorker.IsBusy ) {
                    System.Threading.Thread.Sleep( 1000 );
                    Application.DoEvents();
                }
            }
            if( selectedImages.Count > 0 ) {
                currentObjectPath = "";
                selectedImages.Clear();
                SelectionChanged( this, EventArgs.Empty );
            }

            Controls.Clear();

            session = _session;
            backgroundWorker.RunWorkerAsync();
        }

        public void CreateStack( AstroPhoto.LibRaw.Instance libraw, string name )
        {
            string dir = session.CreateStack( name, selectedImages );

            SuspendLayout();
            for( int i = Controls.Count - 1; i >= 0; i-- ) {
                Control control = Controls[i];
                Panel panel = (Panel) control;
                string path = (string) panel.Tag;
                if( selectedImages.Contains( path ) ) {
                    if( path == currentObjectPath ) {
                        panel.Tag = dir;
                        ThumbnailData data = loadThumbnailData( libraw, dir );
                        panel.Height = buildThumbnail( (Panel) ( panel.Controls[0] ), data ).Height;
                    } else {
                        Controls.Remove( control );
                    }
                }
            }
            ResumeLayout();

            selectedImages.Clear();
            selectedImages.Add( dir );
            currentObjectPath = dir;
            updateThumbnails();
        }

        void addThumbnail( ThumbnailData data )
        {
            Panel exPanel = new Panel();
            exPanel.Padding = new Padding( 2, 2, 2, 2 );
            exPanel.Margin = new Padding( 1, 1, 1, 1 );
            exPanel.Tag = data.path;
            exPanel.Click += new EventHandler( thumbnail_Click );
            exPanel.ContextMenuStrip = ItemContextMenu;

            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.Black;//Color.FromArgb( 44, 44, 44 );
            panel.Tag = data.path;
            panel.Click += new EventHandler( thumbnail_Click );
            panel.ContextMenuStrip = ItemContextMenu;

            var size = buildThumbnail( panel, data );

            exPanel.Width = size.Width + 30;
            exPanel.Height = size.Height + 4;
            exPanel.Controls.Add( panel );
            Controls.Add( exPanel );
        }

        Size buildThumbnail( Panel panel, ThumbnailData data )
        {
            panel.Controls.Clear();

            int width = 120;
            int height = 120;
            var size = new Size( width, height );

            if( data.image != null ) {
                PictureBox pictureBox = new PictureBox();
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Dock = DockStyle.Left;
                pictureBox.Tag = data.path;
                pictureBox.Click += new EventHandler( thumbnail_Click );
                pictureBox.ContextMenuStrip = ItemContextMenu;
                panel.Controls.Add( pictureBox );

                Label label = new Label();
                label.Dock = DockStyle.Bottom;
                label.Width = width;
                label.Tag = data.path;
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.ForeColor = Color.Gray;
                label.Click += new EventHandler( thumbnail_Click );
                label.ContextMenuStrip = ItemContextMenu;
                panel.Controls.Add( label );

                pictureBox.Image = data.image;
                label.Text = data.label;
                label.Height = label.PreferredHeight;
                size.Height = data.image.Height + 4 + label.Height;
            } else {
                Label label = new Label();
                label.Dock = DockStyle.Left;
                label.Text = data.label;
                label.Width = width;
                label.Height = height;
                label.Tag = data.path;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.ForeColor = Color.LightGray;
                label.BackColor = Color.DimGray;
                label.Click += new EventHandler( thumbnail_Click );
                label.ContextMenuStrip = ItemContextMenu;
                panel.Controls.Add( label );
            }

            return size;
        }

        class ThumbnailData { public Image image; public string label; public string path; }
        Queue<ThumbnailData> thumbnailsData = new Queue<ThumbnailData>();

        private void backgroundWorker_DoWork( object sender, DoWorkEventArgs e )
        {
            using( var libraw = new AstroPhoto.LibRaw.Instance() ) {

                var dirs = System.IO.Directory.GetDirectories( session.RootPath );
                var files = System.IO.Directory.GetFiles( session.RootPath );

                var thumbnailsToLoad = new string[dirs.Length + files.Length];
                Array.Copy( dirs, 0, thumbnailsToLoad, 0, dirs.Length );
                Array.Copy( files, 0, thumbnailsToLoad, dirs.Length, files.Length );

                int count = 0;
                int delta = 1;
                int nextToReport = 0;
                foreach( var path in thumbnailsToLoad ) {
                    if( System.IO.Directory.Exists( path ) ||
                        ".arw;.cr2;.jpg".Contains( System.IO.Path.GetExtension( path ).ToLower() ) ) {
                        var thumbnailData = loadThumbnailData( libraw, path );
                        lock( thumbnailsData ) {
                            thumbnailsData.Enqueue( thumbnailData );
                        }
                        if( count == nextToReport ) {
                            backgroundWorker.ReportProgress( 100 * count / thumbnailsToLoad.Length );
                            if( delta < 5 ) {
                                delta = delta + 1;
                            }
                            nextToReport = count + delta;
                        }
                        count++;
                    }
                    if( backgroundWorker.CancellationPending ) {
                        return;
                    }
                }
            }
        }

        private static ThumbnailData loadThumbnailData( AstroPhoto.LibRaw.Instance libraw, string path )
        {
            var thumbnailData = new ThumbnailData();
            thumbnailData.path = path;

            string ext = "";
            string[] stackFiles = null;
            if( System.IO.Directory.Exists( path ) ) {
                var rawDir = System.IO.Path.Combine( path, "RAW" );
                if( System.IO.Directory.Exists( rawDir ) ) {
                    stackFiles = System.IO.Directory.GetFiles( rawDir, "*.arw" );
                    ext = ".arw";
                    if( stackFiles.Length == 0 ) {
                        stackFiles = System.IO.Directory.GetFiles( rawDir, "*.cr2" );
                        ext = ".cr2";
                    }
                }
            } else {
                ext = System.IO.Path.GetExtension( path ).ToLower();
            }

            if( ext.Length > 0 ) {
                if( ext == ".arw" || ext == ".cr2" ) {
                    string filePath = stackFiles == null ? path : stackFiles[0];
                    using( RawImage rawImage = libraw.load_thumbnail( filePath ) ) {
                        thumbnailData.image = createScaledImage( rawImage.Preview );
                        string shutter = rawImage.Shutter > 1 ? string.Format( "{0:0}s", rawImage.Shutter ) :
                           string.Format( "1/{0:0}s", 1 / rawImage.Shutter );
                        if( stackFiles == null ) {
                            thumbnailData.label = "ISO" + rawImage.IsoSpeed.ToString( "0" ) + " - " + shutter ;
                        } else {
                            thumbnailData.label = System.IO.Path.GetFileName( path ) +
                                "\nISO" + rawImage.IsoSpeed.ToString( "0" ) + " - " + shutter
                                 + ( stackFiles == null ? "" : " (" + stackFiles.Length + ")" );
                        }
                    }
                } else if( ext == ".jpg" ) {
                    using( Image preview = Image.FromFile( path ) ) {
                        thumbnailData.image = createScaledImage( preview );
                    }
                    thumbnailData.label = System.IO.Path.GetFileName( path );
                }
            } else {
                thumbnailData.label = System.IO.Path.GetFileName( path );
            }
            return thumbnailData;
        }

        private static Image createScaledImage( Image original )
        {
            int width = 120;
            int height = 120;
            
            float scaleHeight = (float) original.Height / height;
            float scaleWidth = (float) original.Width / width;
            if( scaleHeight > scaleWidth ) {
                width = (int) ( original.Width / scaleHeight );
            } else {
                height = (int) ( original.Height / scaleWidth );
            }
            Image image = new Bitmap( width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
            using( Graphics g = Graphics.FromImage( image ) ) {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage( original, 0, 0, image.Width, image.Height );
            }
            return image;
        }

        private void backgroundWorker_ProgressChanged( object sender, ProgressChangedEventArgs e )
        {
            SuspendLayout();
            while( true ) {
                ThumbnailData data = null;
                lock( thumbnailsData ) {
                    if( thumbnailsData.Count > 0 ) {
                        data = thumbnailsData.Dequeue();
                    }
                }
                if( data == null ) {
                    break;
                }
                addThumbnail( data );
            }
            ResumeLayout();
        }

        private void backgroundWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            backgroundWorker_ProgressChanged( sender, null );
            LoadThumbnailsCompleted( sender, EventArgs.Empty );
        }

        private void updateThumbnails()
        {
            foreach( var control in Controls ) {
                Panel panel = (Panel) control;
                string path = (string) panel.Tag;

                if( path == currentObjectPath && selectedImages.Contains( path ) ) {
                    panel.BackColor = Color.DodgerBlue;
                } else if( selectedImages.Contains( path ) ) {
                    panel.BackColor = Color.Gray;
                } else {
                    panel.BackColor = panel.Parent.BackColor;
                }
            }
        }

        private void thumbnail_Click( object sender, EventArgs e )
        {
            string prevPath = currentObjectPath;
            currentObjectPath = (string) ( (Control) sender ).Tag;
            if( ( Control.ModifierKeys & Keys.Control ) == 0 ) {
                selectedImages.Clear();
            }
            if( ( Control.ModifierKeys & Keys.Shift ) != 0 && prevPath.Length > 0 ) {
                bool adding = false;
                foreach( var control in Controls ) {
                    string path = (string) ( (Control) control ).Tag;
                    if( path == currentObjectPath || path == prevPath ) {
                        selectedImages.Add( path );
                        adding = !adding;
                    }
                    if( adding & !selectedImages.Contains( path ) ) {
                        selectedImages.Add( path );
                    }
                }
            } else if( ( Control.ModifierKeys & Keys.Control ) != 0 ) {
                if( selectedImages.Contains( currentObjectPath ) ) {
                    selectedImages.Remove( currentObjectPath );
                } else {
                    selectedImages.Add( currentObjectPath );
                }
            } else {
                selectedImages.Add( currentObjectPath );
            }
            updateThumbnails();
            SelectionChanged( sender, e );
        }

        private void thumbnailsView_Click( object sender, EventArgs e )
        {
            selectedImages.Clear();
            currentObjectPath = "";
            updateThumbnails();
            SelectionChanged( sender, e );
        }
    }
}
