using System.Collections.Generic;

namespace MyAstroPhotoLibrary
{
    static class Export
    {
        static public void ToFITS( AstroPhoto.LibRaw.Instance libraw, string srcPath, string destPath )
        {
            // 2880 = 36 lines * 80 chars
            string card = "";
            card += "SIMPLE  =                    T                                                  ";
            card += "BITPIX  =                   16                                                  ";
            card += "NAXIS   =                    2                                                  ";
            card += "NAXIS1  =                 4928                                                  ";
            card += "NAXIS2  =                 3276                                                  ";
            card += "BZERO   =                  0.0                                                  ";
            card += "BSCALE  =                  1.0                                                  ";
            card += "END                                                                             ";
            for( int i = 8; i < 36; i++ ) {
                card += new string( ' ', 80 );
            }

            System.IO.File.WriteAllText( destPath, card );

            using( var rawImage = libraw.load_raw( srcPath ) ) {
                var bytes = rawImage.GetBytes();
                for( int i = 0; i < bytes.Length; i += 2 ) {
                    byte temp = bytes[i];
                    bytes[i] = bytes[i + 1];
                    bytes[i + 1] = temp;
                }

                using( var stream = System.IO.File.Open( destPath, System.IO.FileMode.Append ) ) {
                    stream.Write( bytes, 0, bytes.Length );
                    if( bytes.Length % 2880 != 0 ) {
                        int paddingLength = 2880 - bytes.Length % 2880;
                        var padding = new byte[paddingLength];
                        stream.Write( padding, 0, padding.Length );
                    }
                }
            }
        }

        static public void ToIris( AstroPhoto.LibRaw.Instance libraw, StackItem[] currentStack )
        {
            var dark = ImageTools.CreateDarkMedian( libraw, @"E:\Астрофото\2012-08-24\_DARK-ISO1600" );

            string irisRoot = "C:\\IRIS_TEMP\\";
            var script = new List<string>();
            int width = -1;
            int height = -1;
            int number = 1;
            for( int i = 0; i < currentStack.Length; i++ ) {
                if( currentStack[i].IsExcluded ) {
                    continue;
                }
                using( var rawImage = libraw.load_raw( currentStack[i].FilePath ) ) {
                    ushort[] pixels = rawImage.GetRawPixels();
                    byte[] bytes = new byte[pixels.Length * 2];
                    for( int j = 0; j < pixels.Length; j++ ) {
                        int pixDark = dark[j];
                        ushort pix = (ushort) ( pixels[j] + 127 - pixDark );
                        if( pix < 0 ) {
                            pix = 0;
                        }
                        bytes[2 * j + 0] = (byte) ( pix & 0xFF );
                        bytes[2 * j + 1] = (byte) ( ( pix & 0xFF00 ) >> 8 );
                    }
                    width = rawImage.Width;
                    height = rawImage.Height;
                    System.IO.File.WriteAllBytes( irisRoot + number.ToString() + ".bin", bytes );
                    script.Add( string.Format( "import {0}.bin {1} {2} 0 2 0", number, width, height ) );
                    script.Add( "cosme_cfa hot" );
                    script.Add( "cfa2rgb" );
                    script.Add( "save rgb" + number.ToString() );
                    number++;
                }
            }
            script.Add( "load rgb1" );
            System.IO.File.WriteAllLines( irisRoot + "import.pgm", script.ToArray() );

            var hot = new List<string>();
            int nHot = 0;
            for( int x = 0; x < width; x++ ) {
                for( int y = 0; y < height; y++ ) {
                    if( dark[width * y + x] > 200 ) {
                        int _x = x + 1;
                        int _y = height - y;
                        hot.Add( string.Format( "P {0} {1}", _x, _y ) );
                        nHot++;
                    }
                }
            }
            System.IO.File.WriteAllLines( irisRoot + "hot.lst", hot.ToArray() );
        }

        static public void Save16BitPng( string filePath, ushort[] pixels, int width, int height )
        {
            var rgb16 = new System.Windows.Media.Imaging.WriteableBitmap( width, height, 96.0, 96.0,
                System.Windows.Media.PixelFormats.Rgb48, null );

            rgb16.WritePixels( new System.Windows.Int32Rect( 0, 0, width, height ), pixels, width * 2 * 3, 0 );

            var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
            encoder.Frames.Add( System.Windows.Media.Imaging.BitmapFrame.Create( rgb16 ) );
            using( var stream = System.IO.File.OpenWrite( filePath ) ) {
                encoder.Save( stream );
            }
        }

        static public void Save16BitGrayPng( string filePath, ushort[] pixels, int width, int height )
        {
            var g16 = new System.Windows.Media.Imaging.WriteableBitmap( width, height, 96.0, 96.0,
                System.Windows.Media.PixelFormats.Gray16, null );

            g16.WritePixels( new System.Windows.Int32Rect( 0, 0, width, height ), pixels, width * 2, 0 );

            var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
            encoder.Frames.Add( System.Windows.Media.Imaging.BitmapFrame.Create( g16 ) );
            using( var stream = System.IO.File.OpenWrite( filePath ) ) {
                encoder.Save( stream );
            }
        }
    }
}
