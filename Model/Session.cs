using System;
using System.IO;
using System.Collections.Generic;

namespace MyAstroPhotoLibrary
{
    public class Session
    {
        string rootPath;
        ushort[] flat;
        ushort[] dark;

        public Session( string _rootPath )
        {
            rootPath = _rootPath;
            ApplyFlat = true;
            if( File.Exists( Path.Combine( rootPath, "FLAT" ) ) ) {
                using( var stream = File.Open( Path.Combine( rootPath, "FLAT" ), FileMode.Open, FileAccess.Read ) ) {
                    using( var reader = new BinaryReader( stream ) ) {
                        int length = reader.ReadInt32();
                        flat = new ushort[length];
                        for( int i = 0; i < length; i++ ) {
                            flat[i] = reader.ReadUInt16();
                        }
                    }
                }
            }
            if( File.Exists( Path.Combine( rootPath, "DARK" ) ) ) {
                using( var stream = File.Open( Path.Combine( rootPath, "DARK" ), FileMode.Open, FileAccess.Read ) ) {
                    using( var reader = new BinaryReader( stream ) ) {
                        int length = reader.ReadInt32();
                        dark = new ushort[length];
                        for( int i = 0; i < length; i++ ) {
                            dark[i] = reader.ReadUInt16();
                        }
                    }
                }
            }
        }

        public string RootPath { get { return rootPath; } }

        public string CreateStack( string name, IEnumerable<string> selectedImages )
        {
            string dir = Path.Combine( rootPath, name );
            string rawDir = Path.Combine( dir, "RAW" );
            Directory.CreateDirectory( dir );
            Directory.CreateDirectory( rawDir );
            foreach( var file in selectedImages ) {
                File.Move( file, Path.Combine( rawDir, Path.GetFileName( file ) ) );
            }
            return dir;
        }

        public bool ApplyFlat { get; set; }

        public ushort[] Flat { get { return flat; } }
        public ushort[] Dark { get { return dark; } }

        public void SetFlat( ushort[] _flat )
        {
            foreach( var value in _flat ) {
                System.Diagnostics.Trace.Assert( value > 0 );
            }
            flat = _flat;
            using( var stream = File.Open( Path.Combine( rootPath, "FLAT" ), FileMode.OpenOrCreate, FileAccess.Write ) ) {
                using( var writer = new BinaryWriter( stream ) ) {
                    writer.Write( flat.Length );
                    for( int i = 0; i < flat.Length; i++ ) {
                        writer.Write( flat[i] );
                    }
                }
            }
        }

        public void SetDark( ushort[] _dark )
        {
            dark = _dark;
            using( var stream = File.Open( Path.Combine( rootPath, "DARK" ), FileMode.OpenOrCreate, FileAccess.Write ) ) {
                using( var writer = new BinaryWriter( stream ) ) {
                    writer.Write( dark.Length );
                    for( int i = 0; i < dark.Length; i++ ) {
                        writer.Write( dark[i] );
                    }
                }
            }
        }
    }
}
