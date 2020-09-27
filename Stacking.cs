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
        private RgbImage stackImages3( Rectangle rect, out int count )
        {
            count = 0;
            uint[] pixels = null;
            int width = 0, height = 0;
            foreach( var item in currentStack ) {
                using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                    var _pixels = rawImage.GetRawPixels();
                    if( pixels == null ) {
                        width = rawImage.Width;
                        height = rawImage.Height;
                        pixels = new uint[width * height];
                    }
                    for( int i = 0; i < pixels.Length; i++ ) {
                        pixels[i] += _pixels[i];
                    }
                    count++;
                    GC.Collect();
                    Application.DoEvents();
                }
            }
            ushort[] __pixels = new ushort[width * height];
            for( int i = 0; i < pixels.Length; i++ ) {
                __pixels[i] = (ushort)( ( 32 * pixels[i] ) / count );
            }
            
            using( var raw = new RawImage( width, height, 0, __pixels ) ) {
                return raw.ExtractRgbImage( new Rectangle( 5, 5, width - 10, height - 10 ) );
            }
        }

        private RgbImage stackImages2( Rectangle rect, out int count )
        {
            //var start = DateTime.Now;
            var currentOffset = currentStack[currentStackIndex].Offset;
            rect.Offset( -currentOffset.X, -currentOffset.Y );

            count = 0;
            var p = new Int64[4][];
            foreach( var item in currentStack ) {
                if( !item.IsExcluded ) {
                    using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                        var _rect = rect;
                        _rect.Offset( item.Offset );
                        //rawImage.ApplyDark( currentSession.Dark );
                        if( currentSession.Flat != null ) {
                            //rawImage.ApplyFlat( currentSession.Flat, 32 );
                        }
                        int channel = rawImage.Channel( _rect.X, _rect.Y );
                        var _pixels = rawImage.GetRawPixelsApplyFlat( _rect, currentSession.Flat, 64 );
                        if( p[channel] == null ) {
                            var pixels = p[channel] = new Int64[rect.Width * rect.Height];
                            for( int i = 0; i < pixels.Length; i++ ) {
                                pixels[i] = _pixels[i];
                                //pixels[i] -= 128;
                            }
                        } else {
                            var pixels = p[channel];
                            for( int i = 0; i < pixels.Length; i++ ) {
                                pixels[i] += _pixels[i];
                                //pixels[i] -= 128;
                            }
                        }
                        count++;
                        GC.Collect();
                        Application.DoEvents();
                    }
                }
            }
            GC.Collect( 0 );
            //Application.DoEvents();

            var result = new RgbImage( rect.Width - 4, rect.Height - 4 );
            ushort[] upixels = new ushort[rect.Width * rect.Height];
            for( int ch = 0; ch < 4; ch++ ) {
                if( p[ch] != null ) {
                    var pixels = p[ch];
                    Array.Clear( upixels, 0, upixels.Length );
                    if( currentSession.Flat != null ) {
                        for( int i = 0; i < pixels.Length; i++ ) {
                            upixels[i] = (ushort) ( pixels[i] / 640 + 128 );
                        }
                    } else {
                        for( int i = 0; i < pixels.Length; i++ ) {
                            upixels[i] = (ushort) ( pixels[i] );
                        }
                    }
                    using( var raw = new RawImage( rect.Width, rect.Height, ch, upixels ) )
                    using( var rgbImage = raw.ExtractRgbImage( new Rectangle( 2, 2, rect.Width - 4, rect.Height - 4 ) ) ) {
                        result.Add( rgbImage );
                    }
                }
            }
            //MessageBox.Show( ( DateTime.Now - start ).TotalMilliseconds.ToString( "0.00" ) );
            return result;
        }

        private RgbImage stackImages( Rectangle rect, out int count )
        {
            //var start = DateTime.Now;

            var result = new RgbImage( rect.Width, rect.Height );
            var currentOffset = currentStack[currentStackIndex].Offset;
            rect.Offset( -currentOffset.X, -currentOffset.Y );

            count = 0;
            foreach( var item in currentStack ) {
                if( !item.IsExcluded ) {
                    using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                        var _rect = rect;
                        _rect.Offset( item.Offset );
                        //rawImage.ApplyDark( currentSession.Dark );
                        if( currentSession.Flat != null ) {
                            rawImage.ApplyFlat( currentSession.Flat, 1 );
                        }
                        using( var rgbImage = rawImage.ExtractRgbImage( _rect ) ) {
                            result.Add( rgbImage );
                            count++;
                        }
                    }
                }
            }
            //MessageBox.Show( ( DateTime.Now - start ).TotalMilliseconds.ToString( "0.00" ) );
            return result;
        }

        private void stackZoom_Click( object sender, EventArgs e )
        {
            stackedZoom = stackImages( currentZoomRect, out stackedZoomCount );
            recalcCurve();
            showZoomImage();
        }

        private void doStack_Click( object sender, EventArgs e )
        {
            var currentSize = currentImage.RawSize;
            var currentOffset = currentStack[currentStackIndex].Offset;
            var refRect = new Rectangle( 0, 0, currentSize.Width, currentSize.Height );
            foreach( var item in currentStack ) {
                if( !item.IsExcluded ) {
                    var offset = item.Offset;
                    var rect = new Rectangle( 0, 0, currentSize.Width, currentSize.Height );
                    rect.Inflate( -2, -2 );
                    rect.Offset( currentOffset.X - offset.X, currentOffset.Y - offset.Y );
                    refRect.Intersect( rect );
                }
            }

            stackedImage = stackImages( refRect, out stackedImageCount );
            recalcCurve();
            updateImageView();
        }
    }
}