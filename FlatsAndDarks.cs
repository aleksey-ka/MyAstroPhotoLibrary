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
        private void createFlat_Click( object sender, EventArgs e )
        {
            var log = new ProgressLog( this, "Processing FLATs" );
            try {
                createFlat();
                //createFlat2( log );
            } catch( Exception ex ) {
                log.TraceError( ex );
            }
            log.EndLog();
        }

        private void createFlat()
        {
            // Простой вариант

            int width = 0;
            int height = 0;
            int[] flat = null;
            foreach( var item in currentStack ) {
                if( !item.IsExcluded ) {
                    using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                        var pixels = rawImage.GetRawPixels();
                        if( flat == null ) {
                            flat = new int[pixels.Length];
                            width = rawImage.Width;
                            height = rawImage.Height;
                        }
                        for( int i = 0; i < pixels.Length; i++ ) {
                            flat[i] += pixels[i] - 128;
                        }
                    }
                }
            }
            int max11 = 0;
            int max12 = 0;
            int max21 = 0;
            int max22 = 0;
            for( int y = 0; y < height; y += 2 ) {
                int yw = y * width;
                for( int x = 0; x < width; x += 2 ) {
                    int i = yw + x;
                    if( flat[i] > max11 )
                        max11 = flat[i];
                    if( flat[i + 1] > max12 )
                        max12 = flat[i + 1];
                    if( flat[i + width] > max21 )
                        max21 = flat[i + width];
                    if( flat[i + width + 1] > max22 )
                        max22 = flat[i + width + 1];
                }
            }
            var _flat = new ushort[flat.Length];
            for( int y = 0; y < height; y += 2 ) {
                int yw = y * width;
                for( int x = 0; x < width; x += 2 ) {
                    int i = yw + x;
                    _flat[i] = (ushort) ( ( (Int64) flat[i] * ushort.MaxValue ) / max11 );
                    _flat[i + 1] = (ushort) ( ( (Int64) flat[i + 1] * ushort.MaxValue ) / max12 );
                    _flat[i + width] = (ushort) ( ( (Int64) flat[i + width] * ushort.MaxValue ) / max21 );
                    _flat[i + width + 1] = (ushort) ( ( (Int64) flat[i + width + 1] * ushort.MaxValue ) / max22 );
                }
            }
            currentSession.SetFlat( _flat );
        }

        private void createFlat2( ProgressLog log )
        {
            // Более сложный вариант

            log.TraceHeader( "Processing frames" );
            int width = 0;
            int height = 0;
            int[] flat = null;
            int[] sumsqr = null;
            const int LEN = 7;
            const int CENTER = LEN / 2;
            int count = 0;
            foreach( var item in currentStack ) {
                if( !item.IsExcluded ) {
                    log.TraceBold( Color.LightBlue, Path.GetFileName( item.FilePath ) );
                    using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                        var pixels = rawImage.GetRawPixels();
                        if( flat == null ) {
                            flat = new int[LEN * pixels.Length];
                            sumsqr = new int[pixels.Length];
                            width = rawImage.Width;
                            height = rawImage.Height;
                        }
                        if( count < LEN ) {
                            for( int i = 0; i < pixels.Length; i++ ) {
                                flat[LEN * i + count] = pixels[i];
                            }
                            if( count == LEN - 1 ) {
                                for( int i = 0; i < pixels.Length; i++ ) {
                                    Array.Sort( flat, LEN * i, LEN );
                                    int value = flat[LEN * i + CENTER];
                                    sumsqr[i] = value * value;
                                }
                            }
                        } else {
                            for( int i = 0; i < pixels.Length; i++ ) {
                                int value = pixels[i];
                                int i0 = LEN * i;
                                int pos = -1;
                                for( int j = 0; j < LEN; j++ ) {
                                    if( j < CENTER ) {
                                        if( flat[i0 + j] > value ) {
                                            pos = j;
                                            break;
                                        }
                                    } else if( j > CENTER ) {
                                        if( flat[i0 + j] > value ) {
                                            pos = j - 1;
                                            break;
                                        }
                                    }
                                }
                                if( pos == -1 ) {
                                    pos = LEN - 1;
                                }
                                if( pos < CENTER ) {
                                    int v = flat[i0 + CENTER - 1];
                                    flat[i0 + CENTER] += v;
                                    sumsqr[i] += v * v;

                                    for( int j = pos; j < CENTER - 1; j++ ) {
                                        flat[i0 + j + 1] = flat[i0 + j];
                                    }
                                    flat[i0 + pos] = value;
                                } else if( pos > CENTER ) {
                                    int v = flat[i0 + CENTER + 1];
                                    flat[i0 + CENTER] += v;
                                    sumsqr[i] += v * v;

                                    for( int j = CENTER + 1; j < pos; j++ ) {
                                        flat[i0 + j] = flat[i0 + j + 1];
                                    }
                                    flat[i0 + pos] = value;
                                } else {
                                    flat[i0 + CENTER] += value;
                                    sumsqr[i] += value * value;
                                }

                                //int avg = flat[i0 + CENTER] / ( count + 1 - ( LEN - 1 ) );
                                //System.Diagnostics.Trace.Assert( flat[i0 + 0] <= flat[i0 + 1] );
                                //System.Diagnostics.Trace.Assert( flat[i0 + 1] <= avg );
                                //System.Diagnostics.Trace.Assert( avg <= flat[i0 + 3] );
                                //System.Diagnostics.Trace.Assert( flat[i0 + 3] <= flat[i0 + 4] );
                            }
                        }
                        count++;
                    }
                }
            }

            double gain = 0.0;
            double maxDiviation = 0;
            byte[] m = new byte[width * height];
            int n = count - ( LEN - 1 );
            for( int i = 0; i < width * height; i++ ) {
                int mean = flat[LEN * i + CENTER] / n;
                double sigma = Math.Sqrt( sumsqr[i] / n - mean * mean );
                gain += sigma * sigma / ( mean - 128 );
                m[i] = (byte) ( mean < 255 ? mean : 255 );
                flat[LEN * i + CENTER] = mean;
                for( int j = 0; j < LEN - 1; j++ ) {
                    System.Diagnostics.Trace.Assert( flat[LEN * i + j] <= flat[LEN * i + j + 1] );
                }
                for( int j = 0; j < LEN; j++ ) {
                    int value = flat[LEN * i + j];
                    double dev = Math.Abs( value - mean ) / sigma;
                    if( dev > maxDiviation ) {
                        maxDiviation = dev;
                        log.Trace( "dev = {0:0.00}, mean = {1}, value = {2}, sigma {3:0.00}, gain = {4:0.00}", dev, mean, value, sigma, sigma * sigma / ( mean - 128 ) );
                    }
                }
            }
            log.Trace( "gain = {0:0.00}", gain / width / height );
            var newImage = new Bitmap( width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb );
            var bitmapData = newImage.LockBits( new Rectangle( 0, 0, width, height ),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb );
            try {
                unsafe {
                    byte* pixels = (byte*) ( bitmapData.Scan0.ToPointer() );
                    for( int y = 0; y < height; y++ ) {
                        for( int x = 0; x < width; x++ ) {
                            int i = y * width + x;
                            byte R = 0;
                            byte G = 0;
                            byte B = 0;
                            if( y % 2 == 0 ) {
                                if( x % 2 == 0 ) {
                                    R = m[i];
                                } else {
                                    G = (byte) ( m[i] / 2 );
                                }
                            } else {
                                if( x % 2 == 0 ) {
                                    G = (byte) ( m[i] / 2 );
                                } else {
                                    B = m[i];
                                }
                            }
                            pixels[3 * i + 0] = R;
                            pixels[3 * i + 1] = G;
                            pixels[3 * i + 2] = B;
                        }
                    }
                }
            } finally {
                newImage.UnlockBits( bitmapData );
            }
            pictureBox.Image = newImage;
            /*int max11 = 0;
            int max12 = 0;
            int max21 = 0;
            int max22 = 0;
            for( int y = 0; y < height; y += 2 ) {
                int yw = y * width;
                for( int x = 0; x < width; x += 2 ) {
                    int i = yw + x;
                    if( flat[i] > max11 )
                        max11 = flat[i];
                    if( flat[i + 1] > max12 )
                        max12 = flat[i + 1];
                    if( flat[i + width] > max21 )
                        max21 = flat[i + width];
                    if( flat[i + width + 1] > max22 )
                        max22 = flat[i + width + 1];
                }
            }
            var _flat = new ushort[flat.Length];
            for( int y = 0; y < height; y += 2 ) {
                int yw = y * width;
                for( int x = 0; x < width; x += 2 ) {
                    int i = yw + x;
                    _flat[i] = (ushort) ( ( (Int64) flat[i] * ushort.MaxValue ) / max11 );
                    _flat[i + 1] = (ushort) ( ( (Int64) flat[i + 1] * ushort.MaxValue ) / max12 );
                    _flat[i + width] = (ushort) ( ( (Int64) flat[i + width] * ushort.MaxValue ) / max21 );
                    _flat[i + width + 1] = (ushort) ( ( (Int64) flat[i + width + 1] * ushort.MaxValue ) / max22 );
                }
            }
            currentSession.SetFlat( _flat );*/
        }

        private void batchCorrelation()
        {
            int width = 0, height = 0;
            ushort[] pixels1 = null;
            ushort[] max1 = null;
            ushort[] pixels2 = null;
            ushort[] max2 = null;
            int count = 0;
            int batchSize1 = 40;
            int batchSize2 = 40;
            foreach( var item in currentStack ) {
                if( !item.IsExcluded ) {
                    using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                        var pixels = rawImage.GetRawPixels();
                        if( pixels1 == null ) {
                            pixels1 = pixels;
                            width = rawImage.Width;
                            height = rawImage.Height;
                            count = 1;
                            continue;
                        } else if( count == batchSize1 && pixels2 == null ) {
                            pixels2 = pixels;
                            if( batchSize2 == 1 ) {
                                break;
                            }
                            count = 1;
                            continue;
                        }
                        count++;
                        if( pixels2 == null ) {
                            /*if( max1 == null ) {
                                max1 = pixels;
                                for( int i = 0; i < pixels.Length; i++ ) {
                                    if( max1[i] < pixels1[i] ) {
                                        ushort tmp = pixels1[i];
                                        pixels1[i] = max1[i];
                                        max1[i] = tmp;
                                    }
                                }
                            } else for( int i = 0; i < pixels.Length; i++ ) {
                                if( pixels[i] > max1[i] ) {
                                    pixels1[i] += max1[i];
                                    max1[i] = pixels[i];
                                } else {
                                    pixels1[i] += pixels[i];
                                }
                            }*/
                            for( int i = 0; i < pixels.Length; i++ ) {
                                pixels1[i] += pixels[i];
                            }
                        } else {
                            /*if( max2 == null ) {
                                max2 = pixels;
                                for( int i = 0; i < pixels.Length; i++ ) {
                                    if( max2[i] < pixels2[i] ) {
                                        ushort tmp = pixels2[i];
                                        pixels2[i] = max2[i];
                                        max2[i] = tmp;
                                    }
                                }
                            } else for( int i = 0; i < pixels.Length; i++ ) {
                                if( pixels[i] > max2[i] ) {
                                    pixels2[i] += max2[i];
                                    max2[i] = pixels[i];
                                } else {
                                    pixels2[i] += pixels[i];
                                }
                            }*/
                            for( int i = 0; i < pixels.Length; i++ ) {
                                pixels2[i] += pixels[i];
                            }
                            if( count == batchSize2 ) {
                                break;
                            }
                        }
                    }
                }
            }
            //batchSize1 -= 1;
            //batchSize2 -= 1;
            /*for( int y = 1; y < height - 1; y++ ) {
                for( int x = 1; x < width - 1; x++ ) {
                    int i = y * width + x;
                    pixels2[i] = (ushort)( ( pixels2[i - width] + pixels2[i - 1] + pixels2[i] + pixels2[i + 1] + pixels2[i + width] ) / 5 );
                }
            }*/
            /*ushort[] temp = pixels2;
            pixels2 = pixels1;
            pixels1 = temp;*/
            int sz = 1024 / 3;
            var bm = new Bitmap( sz, sz );
            ushort[] newBuf = new ushort[width * height];

            var bitmap = new Bitmap( sz, sz );
            var rect = new Rectangle( 0, 0, bitmap.Width, bitmap.Height );
            var bitmapData = bitmap.LockBits( rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb );
            try {
                unsafe {
                    byte* src = (byte*) ( bitmapData.Scan0.ToPointer() );
                    for( int i = 0; i < pixels1.Length; i++ ) {
                    //for( int i = 2 + 2 * width; i < pixels1.Length - 2 - 2 * width; i++ ) {
                        int y = ( pixels2[i] ) / batchSize2;
                        int x = ( pixels1[i]) / batchSize1;

                        //int y = (pixels2[i] -
                         //   ( pixels2[i - 2] /*+ pixels2[i + 2] + pixels2[i - 2 * width] + pixels2[i + 2 * width]*/ ) / 1 ) / batchSize2 + sz / 2;
                        //int x = ( pixels1[i] -
                         //   ( pixels1[i - 2] /*+ pixels1[i + 2] /*+ pixels1[i - 2 * width] + pixels1[i + 2 * width]*/ ) / 1 ) / batchSize1 + sz / 2;
                        
                        int _ch = 2 * ( ( i / width ) % 2 ) + i % 2;
                        int ch = _ch == 0 ? ch = 2 : ( _ch == 3 ? ch = 0 : ch = 1 );

                        /*int delta = y - 128;
                        double k = 1;//1  - Math.Exp( - 2 * delta * delta );
                        int _x = pixels1[i] + (int)Math.Round( k * ( 128 * batchSize2 - pixels2[i] ) );
                        int x = _x / batchSize1;
                        //newBuf[i] = (ushort)(_x + 128 - 128 * batchSize2);*/

                        if( x < sz && y < sz && x >= 0 && y >= 0 ) {
                            int j = y * bitmapData.Stride + 3 * x + ch;
                            byte c = src[j];
                            if( c < 255 ) {
                                const byte d = 1;
                                const bool b = false;
                                if( b && y > 0 ) {
                                    if( x > 0 )
                                        src[j - bitmapData.Stride - 3] += d;
                                    src[j - bitmapData.Stride] += d;
                                    if( y < sz - 1 )
                                        src[j - bitmapData.Stride + 3] += d;
                                }
                                if( b && x > 0 )
                                    src[j - 3] += d;
                                src[j] += d;
                                if( b && x < sz - 1 )
                                    src[j + 3] += d;
                                if( b && y < sz - 1 ) {
                                    if( x > 0 )
                                        src[j + bitmapData.Stride - 3] += d;
                                    src[j + bitmapData.Stride] += d;
                                    if( y < sz - 1 )
                                        src[j + bitmapData.Stride + 3] += d;
                                }
                            }
                        }
                    }
                    int i0 = 128 * bitmapData.Stride + 3 * 128;
                    for( int x = 0; x < sz; x++ ) {
                        src[i0 + 0] = 0;
                        src[i0 + 1] = 0;
                        src[i0 + 2] = 255;
                    }
                }
            } finally {
                bitmap.UnlockBits( bitmapData );
            }
            pictureBox.Image = bitmap;
            /*using( var newRaw = new RawImage( width, height, 0, newBuf ) ) {
                newRaw.ApplyFlat( currentSession.Flat, 1 );
                stackedImage = newRaw.ExtractRgbImage( new Rectangle( 2, 2, width - 10, height - 10 ) );
            }
            stackedImageCount = batchSize1;
            recalcCurve();*/
        }

        private void mutualCorrelation()
        {
            int width = 0, height = 0;
            ushort[] pixels1 = null;
            ushort[] pixels2 = null;
            int count = 0;
            int batchSize1 = 3;
            int batchSize2 = 3;
            foreach( var item in currentStack ) {
                if( !item.IsExcluded ) {
                    using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                        var pixels = rawImage.GetRawPixels();
                        if( pixels1 == null ) {
                            pixels1 = pixels;
                            width = rawImage.Width;
                            height = rawImage.Height;
                            count = 1;
                            continue;
                        } else if( count == batchSize1 && pixels2 == null ) {
                            pixels2 = pixels;
                            if( batchSize2 == 1 ) {
                                showCorrelation( width, pixels1, pixels2, batchSize1, batchSize2 );
                                pixels1 = pixels2;
                                pixels2 = null;
                            }
                            count = 1;
                            continue;
                        }
                        count++;
                        if( pixels2 == null ) {
                            for( int i = 0; i < pixels.Length; i++ ) {
                                pixels1[i] += pixels[i];
                            }
                        } else {
                            for( int i = 0; i < pixels.Length; i++ ) {
                                pixels2[i] += pixels[i];
                            }
                            if( count == batchSize2 ) {
                                showCorrelation( width, pixels1, pixels2, batchSize1, batchSize2 );
                                pixels1 = pixels2;
                                pixels2 = null;
                                count = batchSize1;
                            }
                        }
                    }
                }
            }
        }

        private void showCorrelation( int width, ushort[] pixels1, ushort[] pixels2, int batchSize1, int batchSize2 )
        {
            int sz = 1024 / 2;
            var bm = new Bitmap( sz, sz );

            var bitmap = new Bitmap( sz, sz );
            var rect = new Rectangle( 0, 0, bitmap.Width, bitmap.Height );
            var bitmapData = bitmap.LockBits( rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb );
            try {
                unsafe {
                    byte* src = (byte*) ( bitmapData.Scan0.ToPointer() );
                    for( int i = 2; i < pixels1.Length; i++ ) {
                        //int y = pixels2[i] / batchSize2;
                        //int x = pixels1[i] / batchSize1;

                        int y = (pixels2[i] -
                           ( pixels2[i - 2] /*+ pixels2[i + 2] + pixels2[i - 2 * width] + pixels2[i + 2 * width]*/ ) / 1 ) / batchSize2 + sz / 2;
                        int x = ( pixels1[i] -
                           ( pixels1[i - 2] /*+ pixels1[i + 2] /*+ pixels1[i - 2 * width] + pixels1[i + 2 * width]*/ ) / 1 ) / batchSize1 + sz / 2;

                        int _ch = 2 * ( ( i / width ) % 2 ) + i % 2;
                        int ch = _ch == 0 ? ch = 2 : ( _ch == 3 ? ch = 0 : ch = 1 );

                        if( x < sz && y < sz && x >= 0 && y >= 0 ) {
                            int j = y * bitmapData.Stride + 3 * x + ch;
                            byte c = src[j];
                            if( c < 255 ) {
                                const byte d = 3;
                                const bool b = false;
                                if( b && y > 0 ) {
                                    if( x > 0 )
                                        src[j - bitmapData.Stride - 3] += d;
                                    src[j - bitmapData.Stride] += d;
                                    if( y < sz - 1 )
                                        src[j - bitmapData.Stride + 3] += d;
                                }
                                if( b && x > 0 )
                                    src[j - 3] += d;
                                src[j] += d;
                                if( b && x < sz - 1 )
                                    src[j + 3] += d;
                                if( b && y < sz - 1 ) {
                                    if( x > 0 )
                                        src[j + bitmapData.Stride - 3] += d;
                                    src[j + bitmapData.Stride] += d;
                                    if( y < sz - 1 )
                                        src[j + bitmapData.Stride + 3] += d;
                                }
                            }
                        }
                    }
                }
            } finally {
                bitmap.UnlockBits( bitmapData );
            }
            if( pictureBox.Image != null ) {
                pictureBox.Image.Dispose();
            }
            pictureBox.Image = bitmap;
            Application.DoEvents();
        }

        private void filterSum( ushort[] sum, ushort[] pixels, ushort[] medianPixels, ushort count )
        {
            const ushort delta = 210;
            ushort deltaSqrt2 = (ushort) Math.Round( ( delta - 128 ) * Math.Sqrt( 2 ) );
            for( int i = 0; i < pixels.Length; i++ ) {
                ushort p = pixels[i];
                if( p / count < delta ) {
                    // В пределах "кружка" ошибок считывания
                    sum[i] += p;
                    continue;
                }
                ushort refP = medianPixels[i];
                if( Math.Abs( refP - p ) / count < deltaSqrt2 ) {
                    // На расстоянии ошибки считывания от линии единичной корреляции
                    sum[i] += p;
                    continue;
                }
                double k = ( p / count - 128.0 ) / ( refP / count - 128 );
                if( k > 1 ) {
                    k = 1 / k;
                }
                if( k > 0.6 ) {
                    // В пределах конуса ошибок распределения пуассона вокруг линии единичной корреляции
                    sum[i] += p;
                    continue;
                }
                sum[i] += refP;
            }
        }

        private void filterSum2( uint[] sum, ushort[] pixels, ushort[] medianPixels, ushort count )
        {
            const ushort delta = 210;
            ushort deltaSqrt2 = (ushort) Math.Round( ( delta - 128 ) * Math.Sqrt( 2 ) );
            for( int i = 0; i < pixels.Length; i++ ) {
                ushort p = pixels[i];
                if( p / count < delta ) {
                    // В пределах "кружка" ошибок считывания
                    sum[i] += p;
                    continue;
                }
                ushort refP = medianPixels[i];
                if( Math.Abs( refP - p ) / count < deltaSqrt2 ) {
                    // На расстоянии ошибки считывания от линии единичной корреляции
                    sum[i] += p;
                    continue;
                }
                double k = ( p / count - 128.0 ) / ( refP / count - 128 );
                if( k > 1 ) {
                    k = 1 / k;
                }
                if( k > 0.6 ) {
                    // В пределах конуса ошибок распределения пуассона вокруг линии единичной корреляции
                    sum[i] += p;
                    continue;
                }
                sum[i] += refP;
            }
        }

        private ushort[] median( ushort[][] batch, int count )
        {
            ushort[] pixels = new ushort[batch[0].Length];
            ushort[] buf = new ushort[count];
            for( int i = 0; i < pixels.Length; i++ ) {
                for( int j = 0; j < count; j++ ) {
                    buf[j] = batch[j][i];
                }
                Array.Sort( buf );
                if( count % 2 == 1 ) {
                    pixels[i] = buf[count / 2];
                } else {
                    pixels[i] = (ushort)( ( buf[count / 2] + buf[count / 2 - 1] ) / 2 );
                }
            }
            return pixels;
        }

        private void prepareTempDir()
        {
            foreach( var file in tempFiles( "pixels" ) ) {
                // TODO
            }
        }

        private string[] tempFiles( string ext )
        {
            return Directory.GetFiles( currentSession.RootPath, string.Format( "*.{0}", ext ) );
        }

        private void writeToFile( ushort[] pixels, string ext )
        {
            var filePath = Path.Combine( currentSession.RootPath, string.Format( "{0}.{1}", DateTime.Now.Ticks, ext ) );
            using( var stream = File.Open( filePath, FileMode.Create, FileAccess.Write ) ) {
                    using( var writer = new BinaryWriter( stream ) ) {
                        writer.Write( (uint)pixels.Length );
                        foreach( var p in pixels ) {
                            writer.Write( p );
                        }
                    }
            }
        }

        private ushort[] readFromFile( string path )
        {
            using( var stream = File.Open( path, FileMode.Open, FileAccess.Read ) ) {
                using( var reader = new BinaryReader( stream ) ) {
                    uint length = reader.ReadUInt32();
                    ushort[] pixels = new ushort[length];
                    for( int i = 0; i < length; i++ ) {
                        pixels[i] = reader.ReadUInt16();
                    }
                    return pixels;
                }
            }
        }

        private void createDark( ProgressBarForm progress )
        {
            prepareTempDir();

            int width = 0;
            int height = 0;
            ushort[][] batch = null;
            int count = 0;
            ushort[] prevMedianPixels = null;
            foreach( var item in currentStack ) {
                if( batch == null ) {
                    GC.Collect();
                    GC.WaitForFullGCComplete();
                    batch = new ushort[9][];
                    count = 0;
                }
                using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                    width = rawImage.Width;
                    height = rawImage.Height;
                    batch[count++] = rawImage.GetRawPixels();
                    //break;
                }
                if( count == batch.Length ) {
                    ushort[] medianPixels = median( batch, count );
                    ushort[] sum = new ushort[medianPixels.Length];
                    foreach( var pixels in batch ) {
                        filterSum( sum, pixels, medianPixels, 1 );
                        //showCorrelation( width, pixels, medianPixels, 1, 1 );
                    }
                    for( int i = 0; i < sum.Length; i++ ) {
                        sum[i] = (ushort) ( sum[i] / count );
                    }
                    if( prevMedianPixels != null ) {
                        showCorrelation( width, prevMedianPixels, sum, 1, 1 );
                    }
                    writeToFile( sum, "0.uint16" );
                    prevMedianPixels = sum;
                    batch = null;
                }
            }

            /*prevMedianPixels = null;
            batch = null;

            foreach( var file in tempFiles( "1.uint16" ) ) {
                if( batch == null ) {
                    GC.Collect();
                    GC.WaitForFullGCComplete();
                    batch = new ushort[15][];
                    count = 0;
                }
                batch[count++] = readFromFile( file );
                if( count == batch.Length ) {
                    ushort[] medianPixels = median( batch, count );
                    uint[] sum = new uint[medianPixels.Length];
                    foreach( var pixels in batch ) {
                        filterSum2( sum, pixels, medianPixels, 9 );
                        showCorrelation( width, pixels, medianPixels, 9, 9 );
                    }
                    ushort[] _sum = new ushort[sum.Length];
                    for( int i = 0; i < sum.Length; i++ ) {
                        _sum[i] = (ushort) ( sum[i] / count );
                    }
                    if( prevMedianPixels != null ) {
                        showCorrelation( width, prevMedianPixels, _sum, 9, 9 );
                    }
                    writeToFile( _sum, "2.uint16" );
                    prevMedianPixels = medianPixels;
                    batch = null;
                }
            }*/

            /*prevMedianPixels = null;

            foreach( var file in tempFiles( "uint16" ) ) {
                var pixels = readFromFile( file );
                if( prevMedianPixels != null ) {
                    showCorrelation( width, prevMedianPixels, pixels, 9, 9 );
                }
                prevMedianPixels = pixels;
                pictureBox.Image.Save( file.Replace( ".unt16", ".png" ) );
                //writeToFile( _sum, "3.uint16" );
            }*/

            /*GC.Collect();
            GC.WaitForFullGCComplete();
            
            ushort[] sum = null;
            ushort[] counts = null;
            
            foreach( var item in currentStack ) {
                progress.ShowMessage( string.Format( "{0}", System.IO.Path.GetFileName( item.FilePath ) ) );

                ushort[] pixels;
                using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                    pixels = rawImage.GetRawPixels();
                }
                filterFrame( pixels, prevMedianPixels );
                showCorrelation( width, pixels, prevMedianPixels, 1, 1 );
                
                GC.Collect();
                GC.WaitForFullGCComplete();
            }*/
        }

        private ushort[] toUInt16( uint[] pixels, int scale )
        {
            ushort[] _pixels = new ushort[pixels.Length];
            for( int i = 0; i < pixels.Length; i++ ) {
                _pixels[i] += (ushort)( pixels[i] / scale );
            }
            return _pixels;
        }

        private void sum()
        {
            uint[] sum = null;
            foreach( var item in currentStack ) {
                ushort[] pixels;
                using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                    pixels = rawImage.GetRawPixels();
                }
                if( sum == null ) {
                    sum = new uint[pixels.Length];
                }
                for( int i = 0; i < pixels.Length; i++ ) {
                    sum[i] += pixels[i];
                }
            }
            
            writeToFile( toUInt16( sum, 30 ), "uint16" );
        }

        private void sum2()
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

            refRect.Offset( -currentOffset.X, -currentOffset.Y );

            var dark = readFromFile( Path.Combine( @"D:\Астрофото\2018\2018.08.14", "_DARK1+2.uint16" ) );
            for( int i = 0; i < dark.Length; i++ ) {
                dark[i] = (ushort)( 387 * ( dark[i] - 9 * 128 ) / 300 / 2 / 9 + 128 );
            }

            uint[] sum = null;

            ushort count = 0;
            foreach( var item in currentStack ) {
                long[] pixels;
                using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                    Rectangle r = refRect;
                    r.Offset( item.Offset );
                    switch( rawImage.Channel( r.X, r.Y ) ) {
                        case 0:
                            break;
                        case 1:
                            r.Offset( -1, 0 );
                            break;
                        case 3:
                            r.Offset( 0, -1 );
                            break;
                        case 2:
                            r.Offset( -1, -1 );
                            break;

                    }
                    pixels = rawImage.GetRawPixelsApplyFlatDark( r, currentSession.Flat, dark, 1 );
                }
                if( sum == null ) {
                    sum = new uint[pixels.Length];
                }
                for( int i = 0; i < pixels.Length; i++ ) {
                    sum[i] += (uint)pixels[i] + 128;
                }
                count++;
                /*if( count == 10 ) {
                    break;
                }*/
            }

            var light = toUInt16( sum, count );
            //writeToFile( light, "uint16" );

            using( var newRaw = new RawImage( refRect.Width, refRect.Height, 0, light ) ) {
                stackedImage = newRaw.ExtractRgbImage( new Rectangle( 2, 2, refRect.Width - 10, refRect.Height - 10 ) );
            }
            stackedImageCount = 1;
            recalcCurve();
            updateImageView();
        }

        private void correlation()
        {
            var light = readFromFile( Path.Combine( currentSession.RootPath, "_LIGHT2.uint16" ) );
            var dark = readFromFile( Path.Combine( currentSession.RootPath, "_DARK1+2.uint16" ) );
            /*for( int i = 0; i < light.Length; i++ ) {
                light[i] += 128;
                light[i] -= ( 128 * 279 ) / 30;
                light[i] -= (ushort) ( ( 387 * ( dark[i] - 9 * 128 ) ) / 300 / 2 );
            }*/
                         
            int width = 4736-200; //4928;
            int height = 3037-200; //3276;

            showCorrelation( width, light, dark, 9, 9 );
                        
            using( var newRaw = new RawImage( width, height, 0, light ) ) {
                newRaw.ApplyFlat( currentSession.Flat, 1 );
                stackedImage = newRaw.ExtractRgbImage( new Rectangle( 2, 2, width - 10, height - 10 ) );
            }
            stackedImageCount = 9;
            recalcCurve();
        }

        private void createDark_Click( object sender, EventArgs e )
        {
            //mutualCorrelation();
            //batchCorrelation();
            sum2();
            //correlation();

            /*var log = new ProgressBarForm( this );
            try {
                createDark( log );
            } catch( Exception ex ) {
                log.ShowMessage( ex.Message );
            }
            log.EndProgress();*/
        }

        private void createDarkT_Click( object sender, EventArgs e )
        {
            int width = 0, height = 0;
            int[] dark = null;
            int count = 0;
            foreach( var item in currentStack ) {
                if( !item.IsExcluded ) {
                    using( var rawImage = libraw.load_raw( item.FilePath ) ) {
                        var pixels = rawImage.GetRawPixels();
                        if( dark == null ) {
                            dark = new int[pixels.Length];
                            width = rawImage.Width;
                            height = rawImage.Height;
                        }
                        for( int i = 0; i < pixels.Length; i++ ) {
                            dark[i] += pixels[i];
                        }
                    }
                    count++;
                }
            }
            /*int max = 0;
            for( int x = 0; x < width; x += 2 ) {
                for( int y = 0; y < height; y += 2 ) {
                    int i = y * width + x;
                    int sum = dark[i] + dark[i + 1] + dark[i + width] + dark[i + width + 1];
                    dark[i] = sum;
                    dark[i + 1] = sum;
                    dark[i + width] = sum;
                    dark[i + width + 1] = sum;
                    if( sum > max ) {
                        max = sum;
                    }
                }
            }*/
            /*int a = 4;
            for( int y = 0; y < height; y += a ) {
                for( int x = 0; x < width; x += a ) {
                    var sum = 0;
                    for( int dy = 0; dy < a; dy++ ) {
                        int i = x + ( y + dy ) * width;
                        for( int dx = 0; dx < a; dx++ ) {
                            sum += dark[i + dx];
                        }

                    }
                    for( int dy = 0; dy < a; dy++ ) {
                        int i = x + ( y + dy ) * width;
                        for( int dx = 0; dx < a; dx++ ) {
                            dark[i + dx] = sum;
                        }
                    }
                }
            }
            count *= a * a;
            */
            var _dark = new ushort[dark.Length];
            for( int i = 0; i < _dark.Length; i++ ) {
                _dark[i] = (ushort) ( dark[i] / count );
            }
            currentSession.SetDark( _dark );
        }
        
        private void buildDarksMenuItem_Click( object sender, EventArgs e )
        {
            var log = new ProgressLog( this, "Processing DARKs" );
            try {

                log.TraceHeader( "Gathering statistics" );

                string _path = Path.Combine( currentSession.RootPath, "_DARK" );
                check( Directory.Exists( _path ), "Session DARKs not found" );
                string path = Path.Combine( _path, "RAW" );
                check( Directory.Exists( path ), "DARKs RAW subfolder not found" );

                int[] histogram = new int[4096];
                int totalPixels = 0;

                foreach( var file in Directory.GetFiles( path ) ) {

                    log.TraceBold( Color.LightBlue, Path.GetFileName( file ) );

                    using( var rawImage = libraw.load_raw( file ) ) {
                        ushort[] pixels = rawImage.GetRawPixels();

                        ushort min = ushort.MaxValue;
                        ushort max = ushort.MinValue;
                        int[] localHistogram = new int[4096];
                        foreach( ushort pixel in pixels ) {
                            histogram[pixel]++;
                            localHistogram[pixel]++;
                            if( pixel > max )
                                max = pixel;
                            if( pixel < min )
                                min = pixel;
                        }

                        totalPixels += pixels.Length;

                        int localSum = 0;
                        int localMedian = -1;
                        for( int i = 0; i < localHistogram.Length; i++ ) {
                            localSum += localHistogram[i];
                            if( localSum >= pixels.Length / 2 ) {
                                localMedian = i;
                                break;
                            }
                        }

                        log.Trace( "Median = {0} Min = {1} Max = {2}", localMedian, min, max );
                    }
                }

                log.TraceHeader( "Statistics" );

                int runningSum = 0;
                int median = -1;
                int maxHistogramCount = 0;
                int maxHistogram = -1;
                int percentileOneSigma = -1;
                int percentileTwoSigma = -1;
                int percentileThreeSigma = -1;
                for( int i = 0; i < histogram.Length; i++ ) {
                    runningSum += histogram[i];
                    if( histogram[i] > maxHistogramCount ) {
                        maxHistogramCount = histogram[i];
                        maxHistogram = i;
                    }
                    if( median < 0 && runningSum >= totalPixels / 2 ) {
                        median = i;
                    }
                    // Отсечение по уровню 68.3%
                    if( percentileOneSigma < 0 && runningSum >= totalPixels * 0.8415 ) {
                        percentileOneSigma = i;
                    }
                    // Отсечение по уровню 95.4%
                    if( percentileTwoSigma < 0 && runningSum >= totalPixels * 0.977 ) {
                        percentileTwoSigma = i;
                    }
                    // Отсечение по уровню 99.7%
                    if( percentileThreeSigma < 0 && runningSum >= totalPixels * 0.9985 ) {
                        percentileThreeSigma = i;
                    }
                }

                int histogramSigma = percentileOneSigma - median;
                int histogramTwoSigma = percentileTwoSigma - median;
                int histogramTheeSigma = percentileThreeSigma - median;

                log.Trace( "MaxHistogram = {0}\nMedian = {1}\nHistogramOneSigma={2}",
                    maxHistogram, median, histogramSigma );

                log.TraceHeader( "Building integrated DARK frame" );

                int hotThreshold = median + 32 * histogramSigma;
                double[] avg = null;
                double[] avgFull = null;
                int[] counts = null;
                int count = 0;

                foreach( var file in Directory.GetFiles( path ) ) {

                    log.TraceBold( Color.LightBlue, Path.GetFileName( file ) );

                    using( var rawImage = libraw.load_raw( file ) ) {
                        ushort[] pixels = rawImage.GetRawPixels();
                        if( avg == null ) {
                            avg = new double[pixels.Length];
                            avgFull = new double[pixels.Length];
                            counts = new int[pixels.Length];
                        }

                        double sum = 0;
                        int hot = 0;

                        for( int i = 0; i < pixels.Length; i++ ) {
                            ushort pixel = pixels[i];
                            avgFull[i] += pixel;
                            if( pixel <= hotThreshold ) {
                                sum += pixel;
                                avg[i] += pixel;
                                counts[i]++;
                            } else {
                                hot++;
                            }
                            
                        }

                        double mean = sum / pixels.Length;

                        double sumsqrdev = 0;
                        
                        foreach( int pixel in pixels ) {
                            if( pixel <= hotThreshold ) {
                                double dev = pixel - mean;
                                sumsqrdev += dev * dev;
                            }
                        }

                        double sigma = Math.Sqrt( sumsqrdev / ( pixels.Length - hot - 1 ) );

                        log.Trace( "Mean = {0:0.00} Sigma = {1:0.00} Hot = {2}",
                            mean, sigma, hot );

                        count++;

                    }
                }

                {
                    log.TraceHeader( "Integrated DARK statistics" );

                    for( int i = 0; i < avg.Length; i++ ) {
                        if( count - counts[i] < count / 3 ) {
                            avg[i] /= counts[i];
                        } else {
                            avg[i] = avgFull[i] / count;
                        }
                    }

                    double sum = 0;
                    int hot = 0;

                    foreach( int pixel in avg ) {
                        if( pixel <= hotThreshold ) {
                            sum += pixel;
                        } else {
                            hot++;
                        }
                    }

                    double mean = sum / avg.Length;

                    double sumsqrdev = 0;
                    foreach( int pixel in avg ) {
                        if( pixel <= hotThreshold ) {
                            double dev = pixel - mean;
                            sumsqrdev += dev * dev;
                        }
                    }

                    double sigma = Math.Sqrt( sumsqrdev / ( avg.Length - hot - 1 ) );

                    log.Trace( "Mean = {0:0.00} Sigma = {1:0.00} Hot = {2}", mean, sigma, hot );

                    var bufLength = 3 * avg.Length / 2; // 12 бит на пиксель
                    byte[] buf = new byte[bufLength]; 
                    int pos = 0;
                    for( int i = 0; i < avg.Length; i++ ) {
                        int value = (int)Math.Round( avg[i] );
                        if( i % 2 == 0 ) {
                            byte hi = (byte) ( ( value & 0xFF0 ) >> 4 );
                            byte lo = (byte) ( ( value & 0xF ) << 4 );
                            buf[pos + 0] = hi;
                            buf[pos + 1] = lo;
                            int _value = ( hi << 4 ) | ( lo >> 4 );
                            System.Diagnostics.Trace.Assert( _value == value );
                            pos += 1;
                        } else {
                            byte hi = (byte) ( ( value & 0xF00 ) / 0x100 );
                            byte lo = (byte) ( ( value & 0xFF ) );
                            buf[pos + 0] |= hi;
                            buf[pos + 1] = lo;
                            int _value = ( hi * 0x100 ) | lo;
                            System.Diagnostics.Trace.Assert( _value == value );
                            pos += 2;
                        }
                    }

                    File.WriteAllBytes( Path.Combine( _path, "DARK.pixels" ), buf );
                }

                log.TraceHeader( "Dark subtracted DARKs" );

                foreach( var file in Directory.GetFiles( path ) ) {

                    log.TraceBold( Color.LightBlue, Path.GetFileName( file ) );

                    using( var rawImage = libraw.load_raw( file ) ) {
                        ushort[] pixels = rawImage.GetRawPixels();

                        double sum = 0;
                        int hot = 0;
                        int extraHot = 0;
                        for( int i = 0; i < pixels.Length; i++ ) {
                            if( pixels[i] <= hotThreshold && avg[i] <= hotThreshold ) {
                                double pixel = pixels[i] - avg[i];
                                sum += pixel;
                            } else {
                                hot++;
                                if( avg[i] <= hotThreshold ) {
                                    extraHot++;
                                }
                            }
                        }

                        double mean = sum / pixels.Length;

                        double sumsqrdev = 0;
                        for( int i = 0; i < pixels.Length; i++ ) {
                            if( pixels[i] <= hotThreshold && avg[i] <= hotThreshold ) {
                                double pixel = pixels[i] - avg[i];
                                double dev = pixel - mean;
                                sumsqrdev += dev * dev;
                            }
                        }

                        double sigma = Math.Sqrt( sumsqrdev / ( pixels.Length - hot - 1 ) );

                        log.Trace( "Mean = {0:0.00} Sigma = {1:0.00} ExtraHot = {2}", mean, sigma, extraHot );

                        count++;
                    }
                }
                log.TraceFinished();
            } catch( Exception ex ) {
                log.TraceError( ex );
            }
            log.EndLog();
        }

        ushort[] loadDark()
        {
            // Переделано на 12 бит на пиксель
            throw new NotImplementedException();

            string path = Path.Combine( currentSession.RootPath, "_DARK" );
            if( Directory.Exists( path ) ) {
                string filePath = Path.Combine( path, "DARK.pixels" );
                if( File.Exists( filePath ) ) {
                    byte[] buf = File.ReadAllBytes( filePath );
                    ushort[] result = new ushort[buf.Length / 3];
                    for( int i = 0; i < result.Length; i++ ) {
                        result[i] = (ushort)( 0x10000 * buf[3 * i + 0] + 0x100 * buf[3 * i + 1] + buf[3 * i + 2] );
                    }
                    return result;
                }
            }
            return null;
        }
    }
}