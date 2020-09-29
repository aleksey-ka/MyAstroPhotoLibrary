using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using AstroPhoto.LibRaw;

namespace MyAstroPhotoLibrary
{
    public partial class MainForm : Form
    {
        private void detectBG_Click( object sender, EventArgs e )
        {
            int BgR, BgG, BgB;
            if( stackedZoom != null ) {
                var rgbImage = stackedZoom;
                using( var lImage = rgbImage.CreateLImage() ) {
                    if( pictureBox.Image != null ) {
                        pictureBox.Image.Dispose();
                    }
                    lImage.MaskBackground( 0xFF, 3 );

                    rgbImage.BackgroundLevels( lImage, out BgR, out BgG, out BgB );
                    int Bg = Math.Min( BgR, Math.Min( BgG, BgB ) );
                    BgNumericUpDown.Value = (decimal) ( ( 1.0 * Bg ) / stackedZoomCount );
                    BgRnumericUpDown.Value = (decimal) ( ( 1.0 * ( BgR - Bg ) ) / stackedZoomCount );
                    BgGnumericUpDown.Value = (decimal) ( ( 1.0 * ( BgG - Bg ) ) / stackedZoomCount );
                    BgBnumericUpDown.Value = (decimal) ( ( 1.0 * ( BgB - Bg ) ) / stackedZoomCount );
                }
            } else {
                using( var rgbImage = currentImage.RawImage.ExtractRgbImage( currentZoomRect ) ) {
                    using( var lImage = rgbImage.CreateLImage() ) {
                        if( pictureBox.Image != null ) {
                            pictureBox.Image.Dispose();
                        }
                        lImage.MaskBackground( 0xFF, 3 );
                        rgbImage.BackgroundLevels( lImage, out BgR, out BgG, out BgB );
                        int Bg = Math.Min( BgR, Math.Min( BgG, BgB ) );
                        BgNumericUpDown.Value = Bg;
                        BgRnumericUpDown.Value = BgR - Bg;
                        BgGnumericUpDown.Value = BgG - Bg;
                        BgBnumericUpDown.Value = BgB - Bg;
                    }
                }
            }
            updateCurve();
        }

        private void bgMask_Click( object sender, EventArgs e )
        {
            using( var rgbImage = currentImage.RawImage.ExtractRgbImage( currentZoomRect ) ) {
                using( var lImage = rgbImage.CreateLImage() ) {
                    if( pictureBox.Image != null ) {
                        pictureBox.Image.Dispose();
                    }
                    lImage.MaskBackground( 0, 7 );
                    pictureBox.Image = lImage.RenderBitmap( 255 );
                }
            }
        }

        LImage[] layers;
        int currentLayer = -1;

        private void wavelets_Click( object sender, EventArgs e )
        {
            if( stackedZoom != null ) {
                using( var lImage = stackedZoom.CreateLImage() ) {
                    layers = lImage.DoWaveletTransform( 6 );
                    currentLayer = -1;
                    if( pictureBox.Image != null ) {
                        pictureBox.Image.Dispose();
                    }
                    pictureBox.Image = lImage.RenderBitmap( (uint) ( stackedZoomCount * MaxVNumericUpDown.Value ) );
                }
            } else {
                using( var rgbImage = currentImage.RawImage.ExtractRgbImage( currentZoomRect ) ) {
                    using( var lImage = rgbImage.CreateLImage() ) {
                        layers = lImage.DoWaveletTransform( 6 );
                        currentLayer = -1;
                        if( pictureBox.Image != null ) {
                            pictureBox.Image.Dispose();
                        }
                        pictureBox.Image = lImage.RenderBitmap( (uint) MaxVNumericUpDown.Value );
                    }
                }
            }
        }

        private void nextWavelet_Click( object sender, EventArgs e )
        {
            if( currentLayer < layers.Length - 1 ) {
                currentLayer++;
            } else {
                currentLayer = -1;
                ;
            }
            Image newImage = null;
            if( currentLayer >= 0 ) {
                newImage = layers[currentLayer].RenderBitmap( 255 );
            } else {
                int[] threshholds = new int[] { 150, 15, 3, 1 };
                using( var image = new LImage( layers, threshholds ) ) {
                    newImage = image.RenderBitmap( (uint) MaxVNumericUpDown.Value );
                }
            }
            if( pictureBox.Image != null ) {
                pictureBox.Image.Dispose();
            }
            pictureBox.Image = newImage;
        }

        private void waveletUpDown_ValueChanged( object sender, EventArgs e )
        {
            int[] threshholds = new int[] { 
                (int)waveletUpDown1.Value, 
                (int)waveletUpDown2.Value,
                (int)waveletUpDown3.Value,
                (int)waveletUpDown4.Value,
                (int)waveletUpDown5.Value
            };
            Image newImage = null;
            using( var image = new LImage( layers, threshholds ) ) {
                newImage = image.RenderBitmap( (uint) MaxVNumericUpDown.Value );
            }
            if( pictureBox.Image != null ) {
                pictureBox.Image.Dispose();
            }
            pictureBox.Image = newImage;
        }

        private void calcStatistics_Click( object sender, EventArgs e )
        {
            VisualTask.Run( this, "Calculating statistics", log =>
            {
                for( int ch = 0; ch < 4; ch++ ) {
                    double mean, sigma;
                    currentImage.RawImage.CalcStatistics( ch, currentZoomRect.X, currentZoomRect.Y, currentZoomRect.Width, currentZoomRect.Height, out mean, out sigma );
                    var stat = string.Format( "{0}: M = {1:0.0} S = {2:0.0} G = {3:0.00}", currentImage.RawImage.Color( ch ),
                        mean, sigma, sigma * sigma / ( mean - 128 ) );
                    log.TraceBold( Color.LightBlue, stat );
                }

                /*for( int ch = 0; ch < 4; ch++ ) {
                        var hyst = diffHystogram( currentImage.RawImage, ch );
                        int total = 0;
                        for( int i = 0; i < hyst.Length; i++ ) {
                            total += hyst[i];
                        }
                        int sum = 0;
                        int p98 = -1;
                        for( int i = 0; i < hyst.Length; i++ ) {
                            sum += hyst[i];
                            if( 100 * sum / total > 98 ) {
                                p98 = i;
                                break;
                            }
                        }
                        var stat = string.Format( "{0}: MaxDiff = {1} 98% < {2}", currentImage.RawImage.Color( ch ),
                            hyst.Length - 1, p98 );
                        log.TraceBold( Color.LightBlue, stat );
                    }*/
            } );
        }

        private int[] diffHystogram( RawImage image, int channel )
        {
            var pixels = currentImage.RawImage.GetRawPixels();
            var size = currentImage.RawSize;
            var idata_filters = currentImage.RawImage.data_filters;

            int maxDiff = 0;
            int prev = -1;
            for( ushort y = 0; y < size.Height; y++ ) {
                int i0 = y * size.Width;
                for( ushort x = 0; x < size.Width; x++ ) {
                    uint cindex = ( idata_filters >> ( ( ( y << 1 & 14 ) | ( x & 1 ) ) << 1 ) & 3 );
                    if( cindex == channel ) {
                        ushort current = pixels[i0 + x];
                        if( prev >= 0 ) {
                            int diff = Math.Abs( (int) current - prev );
                            if( diff > maxDiff ) {
                                maxDiff = diff;
                            }
                        }
                        prev = current;
                    }
                }
            }

            var hyst = new int[maxDiff + 1];
            prev = -1;
            for( ushort y = 0; y < size.Height; y++ ) {
                int i0 = y * size.Width;
                for( ushort x = 0; x < size.Width; x++ ) {
                    uint cindex = ( idata_filters >> ( ( ( y << 1 & 14 ) | ( x & 1 ) ) << 1 ) & 3 );
                    if( cindex == channel ) {
                        ushort current = pixels[i0 + x];
                        if( prev >= 0 ) {
                            int diff = Math.Abs( (int) current - prev );
                            hyst[diff]++;
                        }
                        prev = current;
                    }
                }
            }

            return hyst;
        }
    }
}