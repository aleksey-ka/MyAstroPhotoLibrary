using System;
using System.Drawing;

using AstroPhoto.LibRaw;

namespace MyAstroPhotoLibrary
{
    interface IImage : IDisposable
    {
        string FilePath { get; }
        Image Preview { get; }
        Size RawSize { get; }
        RawImage RawImage { get; }
    }

    class PreviewOnlyImage : IImage
    {
        string filePath;
        Image image;

        public PreviewOnlyImage( string _filePath )
        {
            filePath = _filePath;
        }

        public string FilePath { get { return filePath; } }

        public Image Preview
        {
            get
            {
                if( image == null ) {
                    image = Image.FromFile( filePath );
                }
                return image;
            }
        }

        public Size RawSize { get { return Preview.Size; } }
        public RawImage RawImage { get { return null; }  }

        public void Dispose()
        {
            if( image != null ) {
                image.Dispose();
                image = null;
            }
        }
    }

    class RawImageWrapper : IImage
    {
        string filePath;
        RawImage rawImage;
        Image image;
        AstroPhoto.LibRaw.Instance libraw;
        Session session;

        public RawImageWrapper( AstroPhoto.LibRaw.Instance _libraw, string _filePath, Session _session )
        {
            filePath = _filePath;
            libraw = _libraw;
            session = _session;
        }
        public RawImageWrapper( string _filePath, Session _session )
        {
            filePath = _filePath;
            libraw = null;
            session = _session;
        }

        public string FilePath { get { return filePath; } }

        public Image Preview
        {
            get
            {
                if( image == null ) {
                    image = RawImage.Preview;
                }
                return image;
            }
        }

        public Size RawSize 
        { 
            get 
            {
                var rawImage = this.RawImage;
                return new Size( rawImage.Width, rawImage.Height ); 
            } 
        }

        public RawImage RawImage
        {
            get
            {
                if( rawImage == null ) {
                    if( libraw != null ) {
                        rawImage = libraw.load_raw_thumbnail( filePath );
                        /*if( session.ApplyFlat && session.Flat != null ) {
                            rawImage.ApplyFlat( session.Flat );
                        }*/
                    } else {
                        rawImage = new RawImage( filePath );
                    }
                }
                return rawImage;
            }
        }

        public void Dispose()
        {
            if( image != null ) {
                image.Dispose();
                image = null;
            }
            if( rawImage != null ) {
                rawImage.Dispose();
                rawImage = null;
            }
        }
    }
    
    class StackItem
    {
        public string FilePath;
        public bool IsExcluded;
        public Point Offset;
    }
    
    static class ImageTools
    {
        const int offset = 512;

        static public IImage LoadImage( AstroPhoto.LibRaw.Instance libraw, string path, Session session )
        {
            if( System.IO.Path.GetExtension( path ).ToLower() == ".jpg" ) {
                return new PreviewOnlyImage( path );
            } else {
                return new RawImageWrapper( libraw, path, session );
            }
        }

        static public float[] CreateDarkAvg( AstroPhoto.LibRaw.Instance libraw, string path, out int width, out int height )
        {
            var rawDir = System.IO.Path.Combine( path, "RAW" );
            var stackFiles = System.IO.Directory.GetFiles( rawDir, "*.arw" );

            float[] sum = null;
            int size = 0;
            width = 0;
            height = 0;
            for( int i = 0; i < stackFiles.Length; i++ ) {
                using( var rawImage = libraw.load_raw( stackFiles[i] ) ) {
                    if( sum == null ) {
                        width = rawImage.Width;
                        height = rawImage.Height;
                        size = width * height;
                        sum = new float[size];
                        for( int j = 0; j < size; j++ ) {
                            sum[j] = 0.0f;
                        }
                        
                    }
                    ushort[] _pixels = rawImage.GetRawPixels();
                    for( int j = 0; j < size; j++ ) {
                        sum[j] += _pixels[j];
                    }
                }
            }
            for( int j = 0; j < sum.Length; j++ ) {
                sum[j] /= stackFiles.Length;
            }
            return sum;
        }

        static public ushort[] CreateDarkMedian( AstroPhoto.LibRaw.Instance libraw, string path )
        {
            var rawDir = System.IO.Path.Combine( path, "RAW" );
            var stackFiles = System.IO.Directory.GetFiles( rawDir, "*.arw" );

            ushort[][] pixels = new ushort[stackFiles.Length][];
            int size = -1;
            for( int i = 0; i < stackFiles.Length; i++ ) {
                using( var rawImage = libraw.load_raw( stackFiles[i] ) ) {
                    size = rawImage.Height * rawImage.Width;
                    pixels[i] = rawImage.GetRawPixels();
                }
            }
            ushort[] median = new ushort[size];
            for( int i = 0; i < size; i++ ) {
                ushort[] line = new ushort[stackFiles.Length];
                for( int j = 0; j < stackFiles.Length; j++ ) {
                    line[j] = pixels[j][i];
                }
                Array.Sort( line );
                if( stackFiles.Length % 2 == 0 ) {
                    median[i] = (ushort) ( ( line[stackFiles.Length / 2 - 1] + line[stackFiles.Length / 2] ) / 2 );
                } else {
                    median[i] = line[stackFiles.Length / 2];
                }
            }
            return median;
        }

        public class StackResult
        {
            private readonly Size size;
            private readonly int[] addedPixels;
            private bool isCfa;
            private int minR;
            private int maxR;
            private int minG;
            private int maxG;
            private int minB;
            private int maxB;

            public StackResult( int width, int height, int[] pixels, int count, bool _isCfa )
            {
                size.Width = width;
                size.Height = height;
                addedPixels = pixels;
                minR = minG = minB = 0;
                maxR = maxG = maxB = 0xFF * count;
                isCfa = _isCfa;
            }

            public int MinR { get { return minR; } set { minR = value; } }
            public int MaxR { get { return maxR; } set { maxR = value; } }

            public int MinG { get { return minG; } set { minG = value; } }
            public int MaxG { get { return maxG; } set { maxG = value; } }

            public int MinB { get { return minB; } set { minB = value; } }
            public int MaxB { get { return maxB; } set { maxB = value; } }

            public Bitmap Render()
            {
                var _size = isCfa ? new Size( size.Width / 2, size.Height / 2 ) : size;
                var bitmap = new Bitmap( _size.Width, _size.Height );
                var bitmapData = bitmap.LockBits( new Rectangle( 0, 0, _size.Width, _size.Height ), 
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb );
                try {
                    unsafe {
                        byte* dest = (byte*) ( bitmapData.Scan0.ToPointer() );
                        if( isCfa ) {
                            for( int y = 0; y < size.Height; y += 2 ) {
                                for( int x = 0; x < size.Width; x += 2 ) {
                                    int i0 = y * size.Width + x;
                                    int i1 = ( y + 1 ) * size.Width + x;
                                    var R = addedPixels[i0 + 0];
                                    var G1 = addedPixels[i0 + 1];
                                    var G2 = addedPixels[i1 + 0];
                                    var B = addedPixels[i1 + 1];
                                    var G = G1 + G2;

                                    int i = 3 * ( y * _size.Width / 2  + x / 2 );
                                    dest[i + 0] = (byte) ( B <= maxB ? ( B <= minB ? 0 : ( ( B - minB ) * 0xFF / ( maxB - minB ) ) ) : 0xFF );
                                    dest[i + 1] = (byte) ( G <= maxG ? ( G <= minG ? 0 : ( ( G - minG ) * 0xFF / ( maxG - minG ) ) ) : 0xFF );
                                    dest[i + 2] = (byte) ( R <= maxR ? ( R <= minR ? 0 : ( ( R - minR ) * 0xFF / ( maxR - minR ) ) ) : 0xFF );
                                }
                            }
                        } else {
                            for( int y = 0; y < _size.Height; y ++ ) {
                                for( int x = 0; x < _size.Width; x ++ ) {
                                    int i = 3 * ( y * _size.Width + x );
                                    var B = addedPixels[i + 0];
                                    var G = addedPixels[i + 1];
                                    var R = addedPixels[i + 2];

                                    dest[i + 0] = (byte) ( B < maxB ? ( ( B - minB ) * 0xFF / ( maxB - minB ) ) : 0xFF );
                                    dest[i + 1] = (byte) ( G < maxG ? ( ( G - minG ) * 0xFF / ( maxG - minG ) ) : 0xFF );
                                    dest[i + 2] = (byte) ( R < maxR ? ( ( R - minR ) * 0xFF / ( maxR - minR ) ) : 0xFF );
                                }
                            }
                        }
                    }
                } finally {
                    bitmap.UnlockBits( bitmapData );
                }
                return bitmap;
            }

            public void Autolevels( double low, double hi )
            {
                if( isCfa ) {
                    maxR = 0;
                    maxG = 0;
                    maxB = 0;
                    for( int y = 0; y < size.Height; y += 2 ) {
                        for( int x = 0; x < size.Width; x += 2 ) {
                            int i0 = y * size.Width + x;
                            int i1 = ( y + 1 ) * size.Width + x;
                            var R = addedPixels[i0 + 0];
                            var G1 = addedPixels[i0 + 1];
                            var G2 = addedPixels[i1 + 0];
                            var B = addedPixels[i1 + 1];
                            var G = G1 + G2;
                            if( R > maxR ) {
                                maxR = R;
                            }
                            if( G > maxG ) {
                                maxG = G;
                            }
                            if( B > maxB ) {
                                maxB = B;
                            }
                        }
                    }

                    var r = new int[maxR + 128 + 1];
                    var g = new int[maxG + 128 + 1];
                    var b = new int[maxB + 128 + 1];
                    int count = 0;

                    for( int y = 0; y < size.Height; y += 2 ) {
                        for( int x = 0; x < size.Width; x += 2 ) {
                            int i0 = y * size.Width + x;
                            int i1 = ( y + 1 ) * size.Width + x;
                            var R = addedPixels[i0 + 0];
                            var G1 = addedPixels[i0 + 1];
                            var G2 = addedPixels[i1 + 0];
                            var B = addedPixels[i1 + 1];
                            var G = G1 + G2;
                            r[R + 128]++;
                            g[G + 128]++;
                            b[B + 128]++;
                            count++;
                        }
                    }

                    int runningSum = 0;
                    minR = -1;
                    maxR = -1;
                    for( int i = 0; i < r.Length; i++ ) {
                        runningSum += r[i];
                        if( minR == -1 && runningSum >= low * count ) {
                            minR = i - 128;
                        }
                        if( maxR == -1 && runningSum >= hi * count ) {
                            maxR = i - 128;
                        }
                    }
                    runningSum = 0;
                    minG = -1;
                    maxG = -1;
                    for( int i = 0; i < g.Length; i++ ) {
                        runningSum += g[i];
                        if( minG == -1 && runningSum >= low * count ) {
                            minG = i - 128;
                        }
                        if( maxG == -1 && runningSum >= hi * count ) {
                            maxG = i - 128;
                        }
                    }
                    runningSum = 0;
                    minB = -1;
                    maxB = -1;
                    for( int i = 0; i < b.Length; i++ ) {
                        runningSum += b[i];
                        if( minB == -1 && runningSum >= low * count ) {
                            minB = i - 128;
                        }
                        if( maxB == -1 && runningSum >= hi * count ) {
                            maxB = i - 128;
                        }
                    }
                }
            }
        }

        /*static public StackResult AddStackImages( string filePath, StackItem[] currentStack, short[] darkPixels, bool useJpg )
        {
            int[] addedPixels = null;
            int width = 0;
            int height = 0;
            int N = 0;
            int dX0 = 0;
            int dY0 = 0;
            for( int i = 0; i < currentStack.Length; i++ ) {
                if( currentStack[i].IsExcluded ) {
                    continue;
                }
                Color[] pixels = null;
                short[] cfaPixels = null;
                if( useJpg ) {
                    throw new NotImplementedException();
                    using( var image = currentStack[i].Image.LoadThumbnail() ) {
                        pixels = currentStack[i].Image.GetThumbnailRgbPixels();
                        width = image.Width;
                        height = image.Height;
                    }
                } else {
                    throw new NotImplementedException();
                    cfaPixels = currentStack[i].Image.GetCfaPixels();
                    width = currentStack[i].Image.Width;
                    height = currentStack[i].Image.Height;
                    if( darkPixels != null ) {
                        for( int j = 0; j < cfaPixels.Length; j++ ) {
                            cfaPixels[j] -= darkPixels[j];
                        }
                        for( int y = 2; y < height - 2; y++ ) {
                            for( int x = 2; x < width - 2; x++ ) {
                                int j = y * width + x;
                                if( darkPixels[j] > 350 ) {
                                    cfaPixels[j] = (short)( ( cfaPixels[j - 2] + cfaPixels[j + 2] +
                                        cfaPixels[j - width] + cfaPixels[j + width] ) / 4 );
                                }
                            }
                        }
                    }
                }
                
                if( addedPixels == null ) {
                    dX0 = currentStack[i].Offset.X;
                    dY0 = currentStack[i].Offset.Y;
                    if( useJpg ) {
                        addedPixels = new int[3 *width * height];
                        for( int y = 0; y < height; y++ ) {
                            for( int x = 0; x < width; x++ ) {
                                Color c = pixels[y * width + x];
                                addedPixels[3 * ( y * width + x ) + 0] = c.R;
                                addedPixels[3 * ( y * width + x ) + 1] = c.G;
                                addedPixels[3 * ( y * width + x ) + 2] = c.B;
                            }
                        }
                    } else {
                        addedPixels = new int[width * height];
                        for( int y = 0; y < height; y++ ) {
                            for( int x = 0; x < width; x++ ) {
                                addedPixels[y * width + x] = cfaPixels[y * width + x];
                            }
                        }
      
                    }
                } else {
                    int dX = currentStack[i].Offset.X - dX0;
                    int dY = currentStack[i].Offset.Y - dY0;
                    if( !useJpg ) {
                        dX *= 2;
                        dY *= 2;
                    }
                    for( int y = 0; y < height; y++ ) {
                        for( int x = 0; x < width; x++ ) {
                            if( x + dX > 0 && x + dX < width && y + dY > 0 && y + dY < height ) {
                                if( useJpg ) {
                                    Color c = pixels[( y + dY ) * width + x + dX];
                                    addedPixels[3 * ( y * width + x ) + 0] += c.R;
                                    addedPixels[3 * ( y * width + x ) + 1] += c.G;
                                    addedPixels[3 * ( y * width + x ) + 2] += c.B;
                                } else {
                                    addedPixels[y * width + x] += cfaPixels[( y + dY ) * width + x + dX];
                                }
                            }
                        }
                    }
                }
                N++;
            }
            return new StackResult( width, height, addedPixels, N, !useJpg );
        }*/

        /*static public Color LocalSky( IImage image, Point p )
        {
            int width = image.LoadThumbnail().Width;
            Color[] pixels = image.GetThumbnailRgbPixels();
            uint[] countsR = new uint[256];
            uint[] countsG = new uint[256];
            uint[] countsB = new uint[256];
            uint count = 0;
            int N = 25;
            for( int dy = -N; dy <= N; dy++ ) {
                for( int dx = -N; dx <= N; dx++ ) {
                    int offset = ( p.Y + dy ) * width + p.X + dx;
                    countsR[pixels[offset].R]++;
                    countsG[pixels[offset].G]++;
                    countsB[pixels[offset].B]++;
                    count++;
                }
            }
            int resultR = 0;
            uint total = 0;
            for( int i = 0; i < countsR.Length; i++ ) {
                total += countsR[i];
                if( 2 * total >= count ) {
                    resultR = i;
                    break;
                }
            }
            int resultG = 0;
            total = 0;
            for( int i = 0; i < countsG.Length; i++ ) {
                total += countsG[i];
                if( 2 * total >= count ) {
                    resultG = i;
                    break;
                }
            }
            int resultB = 0;
            total = 0;
            for( int i = 0; i < countsB.Length; i++ ) {
                total += countsB[i];
                if( 2 * total >= count ) {
                    resultB = i;
                    break;
                }
            }
            return Color.FromArgb( resultR, resultG, resultB );
        }*/
        
        static public Point RefineCenter( RawImage rawImage, Point center, Size size )
        {
            var pixels = rawImage.GetRawPixels();
            int width = rawImage.Width;
            /*PointF current = new PointF( center.X, center.Y );
            while( true ) {
                double ddx = center.X - current.X;
                double ddy = center.Y - current.Y;
                double sumX = 0.0;
                double sumY = 0.0;
                double sum = 0.0;
                int N = 17;
                for( int dy = -N; dy <= N; dy++ ) {
                    for( int dx = -N; dx <= N; dx++ ) {
                        double s = pixels[( center.Y + dy ) * width + center.X + dx];
                        if( dy == -N ) {
                            s *= 0.5 + ddy;
                        }
                        if( dy == N ) {
                            s *= 0.5 - ddy;
                        }
                        if( dx == -N ) {
                            s *= 0.5 + ddx;
                        }
                        if( dx == N ) {
                            s *= 0.5 - ddx;
                        }
                        sumX += ( dx + ddx ) * s;
                        sumY += ( dy + ddy ) * s;
                        sum += s;
                    }
                }
                double dX = sumX / sum;
                double dY = sumY / sum;
                current.X += (float) dX;
                current.Y += (float) dY;
                int X = (int) Math.Round( current.X );
                int Y = (int) Math.Round( current.Y );
                if( Math.Abs( dX ) < 0.001 && Math.Abs( dY ) < 0.001 ) {
                    return new Point( X, Y );
                }
                center.X = X;
                center.Y = Y;
            }
            return new Point();*/

            int WX = size.Width / 2;
            int WY = size.Height / 2;

            Point current = new Point( center.X, center.Y );

            int run = 0;
            while( true ) {
                double median = Median( pixels, width, current, size );

                double sumLdX = 0;
                double sumLdY = 0;
                double sumL = 0;
                for( int dy = -WY; dy <= WY; dy++ ) {
                    int y = current.Y + dy;
                    for( int dx = -WX; dx <= WX; dx++ ) {
                        int x = current.X + dx;
                        double L = pixels[y * width + x] - median;
                        if( L > 0 ) {
                            sumLdX += L * dx;
                            sumLdY += L * dy;
                            sumL += L;
                        }
                    }
                }
                double dX = sumLdX / sumL;
                double dY = sumLdY / sumL;
                current.X += (int)Math.Round( dX );
                current.Y += (int)Math.Round( dY );
                if( run++ > 1 ) {
                    break;
                }
            }
            return current;
        }

        static public double Median( ushort[] pixels, int width, Point center, Size size )
        {
            int WX = size.Width / 2;
            int WY = size.Height / 2;

            // Считаем медиану
            int count = 0;
            var counts = new int[16597];
            for( int i = 0; i < counts.Length; i++ ) {
                counts[i] = 0;
            }
            for( int dy = -WY; dy <= WY; dy++ ) {
                int y = center.Y + dy;
                for( int dx = -WX; dx <= WX; dx++ ) {
                    int x = center.X + dx;
                    int L = pixels[y * width + x];
                    counts[L]++;
                    count++;
                }
            }
            double median = 0;
            int sum = 0;
            for( int i = 0; i < counts.Length; i++ ) {
                sum += counts[i];
                if( sum >= count / 2 ) {
                    median = i;
                    break;
                }
            }
            return median;
        }

        static public double CalcCorrelation( RawImage rawImage, ushort[] prevPixels, 
            Point center, Point prevCenter, Size size )
        {
            var pixels = rawImage.GetRawPixels();
            int width = rawImage.Width;

            int WX = size.Width / 2;
            int WY = size.Height / 2;

            double median = Median( pixels, width, center, size );
            double prevMedian = Median( prevPixels, width, prevCenter, size );

            int n = 0;
            double sumXY = 0;
            double sumXX = 0;
            double sumYY = 0;
            double sumX = 0;
            double sumY = 0;
            for( int dy = -WY; dy <= WY; dy++ ) {
                int y1 = center.Y + dy;
                int y2 = prevCenter.Y + dy;
                for( int dx = -WX; dx <= WX; dx++ ) {
                    int x1 = center.X + dx;
                    int x2 = prevCenter.X + dx;
                    double X = pixels[y1 * width + x1] - median;
                    double Y = prevPixels[y2 * width + x2] - prevMedian;
                    sumXY += X * Y;
                    sumXX += X * X;
                    sumYY += Y * Y;
                    sumX += X;
                    sumY += Y;
                    n++;
                }
            }
            double result = ( n * sumXY - sumX * sumY ) /
                Math.Sqrt( ( n * sumXX - sumX * sumX ) * ( n * sumYY - sumY * sumY ) );
            return result;
        }

        /*static public double StarSigma( IImage image, PointF current )
        {
            int width = image.LoadThumbnail().Width;
            Color[] pixels = image.GetThumbnailRgbPixels();
            Point center = new Point( (int) Math.Round( current.X ), (int) Math.Round( current.Y ) );
            double ddx = center.X - current.X;
            double ddy = center.Y - current.Y;
            double sqrdev = 0.0;
            double sum = 0.0;
            int N = 7;
            Color sky = LocalSky( image, center );
            for( int dy = -N; dy <= N; dy++ ) {
                for( int dx = -N; dx <= N; dx++ ) {
                    int offset = ( center.Y + dy ) * width + center.X + dx;
                    int r = pixels[offset].R;
                    int g = pixels[offset].G;
                    int b = pixels[offset].B;
                    double s = ( r + g + b ) - sky.R - sky.G - sky.B;
                    if( s < 0 ) {
                        continue;
                    }
                    if( dy == -N ) {
                        s *= 0.5 + ddy;
                    }
                    if( dy == N ) {
                        s *= 0.5 - ddy;
                    }
                    if( dx == -N ) {
                        s *= 0.5 + ddx;
                    }
                    if( dx == N ) {
                        s *= 0.5 - ddx;
                    }
                    double rx = dx + ddx;
                    double ry = dy + ddy;
                    sqrdev += ( rx * rx + ry * ry ) * s ;
                    sum += s;
                }
            }
            return Math.Sqrt( sqrdev / sum );
        }*/

        static public void CalcStackStatistics( AstroPhoto.LibRaw.Instance libraw, string currentImagePath )
        {
            /*int width, height;
            float[] stack = ImageTools.CreateDarkAvg( libraw, currentImagePath, out width, out height );
            string msg = "";
            for( int ch = 0; ch < 4; ch++ ) {
                double mean, sigma;
                libraw.CalcStatistics( width, height, stack, ch, currentZoomRect.X, currentZoomRect.Y, currentZoomRect.Width, currentZoomRect.Height, out mean, out sigma );
                msg += string.Format( "{2}: Mean={0:0.00} Sigma={1:0.00}\n", mean, sigma, ch );
            }
            System.Windows.Forms.MessageBox.Show( msg, "Avg Dark" );
            string path = currentImagePath;
            if( !System.IO.File.Exists( path ) ) {
                var stackFiles = System.IO.Directory.GetFiles( currentImagePath + "\\RAW" );
                path = stackFiles[currentStackIndex];
            }
            libraw.open_file( path );
            short[] pixels  = libraw.GetPixels();
            for( int i = 0; i < pixels.Length; i++ ) {
                stack[i] = pixels[i] - stack[i];
            }
            msg = "";
            for( int ch = 0; ch < 4; ch++ ) {
                double mean, sigma;
                libraw.CalcStatistics( width, height, stack, ch, currentZoomRect.X, currentZoomRect.Y, currentZoomRect.Width, currentZoomRect.Height, out mean, out sigma );
                msg += string.Format( "{2}: Mean={0:0.00} Sigma={1:0.00}\n", mean, sigma, ch );
            }
            
            MessageBox.Show( msg, "Dark Subtracted Avg Dark" );*/

            throw new NotImplementedException();
        }

        /*private void button1_Click( object sender, EventArgs e )
        {
            var ext = System.IO.Path.GetExtension( thumbnailsView.CurrentImagePath ).ToLower();

            string[] stackFiles = null;
            if( ext.Length == 0 ) {
                var rawDir = System.IO.Path.Combine( thumbnailsView.CurrentImagePath, "RAW" );
                if( System.IO.Directory.Exists( rawDir ) ) {
                    ext = ".arw";
                    stackFiles = System.IO.Directory.GetFiles( rawDir, "*.arw" );
                }
            }

            libraw.open_file( stackFiles[0] );
            short[] pixels = libraw.GetPixels();
            int height = libraw.raw_height;
            int width = libraw.raw_width;
            libraw.recycle();

            long sum = 0;
            int N = 0;
            short min = short.MaxValue;
            short max = 0;
            for( int x = 0; x < width; x++ ) {
                for( int y = 0; y < height; y++ ) {
                    if( ( x + y ) % 2 == 1 ) {
                        var pix = pixels[y * width + x];
                        sum += pix;
                        if( pix < min ) {
                            min = pix;
                        }
                        if( pix > max ) {
                            max = pix;
                        }
                        N++;
                    }
                }
            }
            int black = libraw.GetBlackLevel();
            int cblack = libraw.GetCBlackLevel( 1 );
            double mean = sum / N - black - cblack;

            double devsum = 0;
            for( int x = 0; x < width; x++ ) {
                for( int y = 0; y < height; y++ ) {
                    if( ( x + y ) % 2 == 1 ) {
                        var pix = pixels[y * width + x];
                        double dev = 1.0 * ( pix - black ) - mean;
                        devsum += dev * dev;
                    }
                }
            }
            double sigma = Math.Sqrt( devsum / N );

            MessageBox.Show( string.Format( "Mean={0} Sigma = {1:0.00}\nMin = {2} Max = {3} Black = {4} CBlack = {5}", mean, sigma, min, max, black, cblack ) );
        }

        private void button2_Click( object sender, EventArgs e )
        {
            var ext = System.IO.Path.GetExtension( thumbnailsView.CurrentImagePath ).ToLower();

            string[] stackFiles = null;
            if( ext.Length == 0 ) {
                var rawDir = System.IO.Path.Combine( thumbnailsView.CurrentImagePath, "RAW" );
                if( System.IO.Directory.Exists( rawDir ) ) {
                    ext = ".arw";
                    stackFiles = System.IO.Directory.GetFiles( rawDir, "*.arw" );
                }
            }

            double[] sum = null;
            for( int i = 0; i < stackFiles.Length; i++ ) {
                libraw.open_file( stackFiles[i] );
                int size = libraw.raw_height * libraw.raw_width;
                if( sum == null ) {
                    sum = new double[size];
                    for( int j = 0; j < size; j++ ) {
                        sum[j] = 0.0;
                    }
                }
                short[] _pixels = libraw.GetPixels();
                for( int j = 0; j < size; j++ ) {
                    sum[j] += _pixels[j];
                }
                libraw.recycle();
            }
            double max = 0.0;
            for( int j = 0; j < sum.Length; j++ ) {
                if( sum[j] > max ) {
                    max = sum[j];
                }
            }
            for( int j = 0; j < sum.Length; j++ ) {
                sum[j] /= max;
            }

            libraw.open_file( stackFiles[0] );
            short[] pixels = libraw.GetPixels();
            int height = libraw.raw_height;
            int width = libraw.raw_width;
            libraw.recycle();

            double[] normalized = new double[width * height];
            for( int i = 0; i < normalized.Length; i++ ) {
                normalized[i] = pixels[i] / sum[i];
            }

            double Sum = 0;
            int N = 0;
            for( int x = 0; x < width; x++ ) {
                for( int y = 0; y < height; y++ ) {
                    if( ( x + y ) % 2 == 1 ) {
                        var pix = normalized[y * width + x];
                        Sum += pix;
                        N++;
                    }
                }
            }
            double mean = Sum / N;

            double devsum = 0;
            double maxdev = 0;
            for( int x = 0; x < width; x++ ) {
                for( int y = 0; y < height; y++ ) {
                    if( ( x + y ) % 2 == 1 ) {
                        var pix = normalized[y * width + x];
                        double dev = Math.Abs( 1.0 * ( pix ) - mean );
                        devsum += dev * dev;
                        if( dev > maxdev )
                            maxdev = dev;
                    }
                }
            }
            double sigma = Math.Sqrt( devsum / N );

            MessageBox.Show( string.Format( "Mean={0:0.00} Sigma = {1:0.00}\n", mean, sigma ) );
        }

        private void button3_Click( object sender, EventArgs e )
        {
            string path = currentStack != null ? currentStack[currentStackIndex] : thumbnailsView.CurrentImagePath;
            libraw.open_file( path );
            libraw.Process( thumbnailsView.CurrentImagePath + "\\render_out.tif" );
            libraw.recycle();
        }

        private void button4_Click( object sender, EventArgs e )
        {
            var stackFiles = System.IO.Directory.GetFiles( thumbnailsView.CurrentImagePath + "\\RAW", "*.arw" );

            var sum = ImageTools.CreateDarkMedian( libraw, thumbnailsView.CurrentImagePath );

            libraw.open_file( stackFiles[0] );
            short[] pixels = libraw.GetPixels();
            int height = libraw.raw_height;
            int width = libraw.raw_width;
            libraw.recycle();

            double[] normalized = new double[width * height];
            for( int i = 0; i < normalized.Length; i++ ) {
                normalized[i] = pixels[i] - sum[i];
            }

            double Sum = 0;
            int N = 0;
            for( int x = 0; x < width; x++ ) {
                for( int y = 0; y < height; y++ ) {
                    if( ( x + y ) % 2 == 1 ) {
                        var pix = normalized[y * width + x];
                        Sum += pix;
                        N++;
                    }
                }
            }
            double mean = Sum / N;

            double devsum = 0;
            double maxdev = 0;
            int nHot = 0;
            for( int x = 0; x < width; x++ ) {
                for( int y = 0; y < height; y++ ) {
                    if( ( x + y ) % 2 == 1 ) {
                        var pix = normalized[y * width + x];
                        double dev = Math.Abs( 1.0 * ( pix ) - mean );
                        devsum += dev * dev;
                        if( dev > maxdev )
                            maxdev = dev;
                        if( sum[y * width + x] > 200 ) {
                            nHot++;
                        }
                    }
                }
            }
            double sigma = Math.Sqrt( devsum / N );

            MessageBox.Show( string.Format( "Mean={0:0.00} Sigma = {1:0.00} MaxDev = {2:0.00} nHot = {3}\n", mean, sigma, maxdev, nHot ) );
        }*/
    }
}
