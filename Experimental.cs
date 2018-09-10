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
        private void doExperiment_Click( object sender, EventArgs e )
        {
            /*using( var buffer = new RawBuffer32() ) {
                foreach( var item in currentStack ) {
                    if( !item.IsExcluded ) {
                        using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                            buffer.Add2( rawImage );
                        }
                    }
                }

                pictureBox.Image = buffer.GetResult().RenderBitmapHalfRes( currentCurve, 0 );
            }*/

            Rectangle rect = currentZoomRect;
            
            int pcount = rect.Width * rect.Height;
            
            int margin = 11;
            int window = 2 * margin + 1;
            var result = new ushort[pcount * window];
            var sumSqr = new uint[pcount * window];
            int count = 0;
            int channel = 0;
            foreach( var item in currentStack ) {
                if( !item.IsExcluded ) {
                    using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                        //rawImage.ApplyFlat( currentSession.Flat, 1 );
                        var pixels = rawImage.GetRawPixels( rect );
                        channel = rawImage.Channel( rect.X, rect.Y );
                        if( count < window ) {
                            for( int i = 0; i < pcount; i++ ) {
                                int i0 = window * i;
                                result[i0 + count] = pixels[i];
                                if( count + 1 == window ) {
                                    Array.Sort( result, i0, window );
                                    var p = result[i0 + margin];
                                    sumSqr[i] = (uint)( p * p );
                                }
                            }
                        } else {
                            for( int i = 0; i < pcount; i++ ) {
                                int i0 = window * i;
                                int center = i0 + margin;
                                var p = pixels[i];

                                if( p > result[center - 1] && p < result[center + 1] ) {
                                    result[center] += p;
                                    sumSqr[i] += (uint) ( p * p );
                                } else {
                                    int insertionPoint = i0;
                                    for( int j = i0; j < i0 + window; j++ ) {
                                        if( j == center ) {
                                            continue;
                                        }
                                        if( p <= result[j] ) {
                                            if( j < center ) {
                                                insertionPoint = j;
                                            }
                                            break;
                                        }
                                        insertionPoint = j;
                                    }

                                    if( insertionPoint < center ) {
                                        var _p = result[center - 1];
                                        result[center] += _p;
                                        sumSqr[i] += (uint) ( _p * _p );
                                        for( int j = center - 1; j > insertionPoint; j-- ) {
                                            result[j] = result[j - 1];
                                        }
                                    } else {
                                        var _p = result[center + 1];
                                        result[center] += _p;
                                        sumSqr[i] += (uint) ( _p * _p );
                                        for( int j = center + 1; j < insertionPoint; j++ ) {
                                            result[j] = result[j + 1];
                                        }
                                    }
                                    result[insertionPoint] = p;
                                }
                            }
                        }
                        count++;
                    }
                }
            }

            double k = 3.0;
            int n = 4;
            int sumCount = count - window + 1;
            var newpixels = new ushort[pcount];
            var newpixels2 = new ushort[pcount];
            int skipped = 0;
            for( int i = 0; i < pcount; i++ ) {
                int i0 = window * i;
                int center = i0 + margin;
                int sum = result[center];
                uint sqr = sumSqr[i];
                int _count = sumCount;
                while( true ) {
                    double mean = (double) sum / _count;
                    double sigma = Math.Sqrt( (double) sqr / _count - mean * mean );
                    double delta = Math.Max( 1.0, k * sigma );
                    int min = (int) Math.Round( mean - delta );
                    int max = (int) Math.Round( mean + delta );
                    bool finished = true;
                    for( int j = i0; j < i0 + window; j++ ) {
                        if( j != center ) {
                            var p = result[j];
                            if( p != 0 && p >= min && p <= max) {
                                sum += p;
                                sqr += (uint) ( p * p );
                                result[j] = 0;
                                _count++;
                                finished = false;
                            }
                        }
                    }
                    if( finished || count == _count) {
                        int sum2 = 0;
                        int count2 = 0;
                        for( int j = i0; j < i0 + window; j++ ) {
                            if( j != center ) {
                                var p = result[j];
                                if( p != 0 ) {
                                    sum2 += p;
                                    skipped++;
                                    count2++;
                                }
                            }
                        }
                        if( sum2 > 0 ) {
                            sum2 -= 128 * count2;
                            sum2 *= n;
                            sum2 /= count2;
                        }
                        newpixels2[i] = (ushort) ( sum2 + 128 );
                        break;
                    }
                    
                }
                sum -= 128 *_count;
                sum *= n;
                sum /= _count;
                newpixels[i] = (ushort)( sum + 128 );
            }

            MessageBox.Show( string.Format( "Skipped {0}", (double)skipped / ( pcount * count ) ) );

            var raw = new RawImage( rect.Width, rect.Height, channel, newpixels );

            stackedZoom = raw.ExtractRgbImage( new Rectangle( 3, 3, rect.Width - 6, rect.Height - 6 ) );
            stackedZoomCount = n;
            recalcCurve();
            showZoomImage();
        }
    }
}