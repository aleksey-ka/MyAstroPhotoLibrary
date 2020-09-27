// © ABBYY. 2012.
// SAMPLES code is property of ABBYY, exclusive rights are reserved. 
// DEVELOPER is allowed to incorporate SAMPLES into his own APPLICATION and modify it 
// under the terms of License Agreement between ABBYY and DEVELOPER.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

class PictureBoxOverlay
{
    PictureBox pictureBox;
    class Box
    {
        public Color Color;
        public Rectangle Rect;
        public bool IsSelected;
        public int Tag;

        public Box( int tag, int x, int y, int w, int h, bool isSelected, Color color )
        {
            Tag = tag;
            Rect.X = x;
            Rect.Y = y;
            Rect.Width = w;
            Rect.Height = h;
            IsSelected = isSelected;
            Color = color;
        }
    }
    List<Box> zBoxes = new List<Box>();
    List<Box> areaBoxes = new List<Box>();
    enum BoxIntersectionType { None, New, Inside, Left, Right, Top, Bottom, LeftTop, LeftBottom, RightTop, RightBottom };
    BoxIntersectionType currentBoxIntersection;
    Point prevMouse;
    Rectangle prevRect;
    double scale;
    int deltaY;
    int deltaX;
    Color newBoxColor;
    bool newBoxMode = false;
    bool deleteBoxMode = false;
    bool disableEdit = false;

    public PictureBoxOverlay( PictureBox _pictureBox )
    {
        pictureBox = _pictureBox;
        pictureBox.Paint += new PaintEventHandler( pictureBox_Paint );
        pictureBox.MouseMove += new MouseEventHandler( pictureBox_MouseMove );
        pictureBox.MouseDown += new MouseEventHandler( pictureBox_MouseDown );
        pictureBox.MouseUp += new MouseEventHandler( pictureBox_MouseUp );
        pictureBox.MouseLeave += new EventHandler( pictureBox_MouseLeave );
    }

    public void AddBox( int tag, Rectangle rect, Color color, bool select )
    {
        if( select ) {
            foreach( var box in zBoxes ) {
                box.IsSelected = false;
            }
        }

        zBoxes.Add( new Box( tag, rect.Left, rect.Top, rect.Width, rect.Height, select, color ) );
        pictureBox.Invalidate();
    }

    public void AddArea( Rectangle rect, Color color )
    {
        areaBoxes.Add( new Box( 0, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top, false, color ) );
        pictureBox.Invalidate();
    }

    public Rectangle GetClientRect( Rectangle _rect )
    {
        scale = Math.Min( ( 1.0 * pictureBox.Height ) / pictureBox.Image.Height,
            ( 1.0 * pictureBox.Width ) / pictureBox.Image.Width );
        deltaY = ( pictureBox.Height - ( int )Math.Round( scale * pictureBox.Image.Height ) ) / 2;
        deltaX = ( pictureBox.Width - ( int )Math.Round( scale * pictureBox.Image.Width ) ) / 2;

        return getClientRect( _rect );
    }

    public void EnableDrawNewBoxes( bool newValue, Color color )
    {
        newBoxMode = newValue;
        newBoxColor = color;
    }

    public void EnableDeleteBoxes( bool newValue )
    {
        deleteBoxMode = newValue;
    }

    public void EnableEditBoxes( bool newValue )
    {
        disableEdit = !newValue;
    }

    public int GetSelectedTag()
    {
        foreach( var box in zBoxes ) {
            if( box.IsSelected ) {
                return box.Tag;
            }
        }
        return -1;
    }

    public void SelectBox( int tag ) 
    {
        foreach( var box in zBoxes ) {
            if( box.Tag == tag ) {
                box.IsSelected = true;
            } else {
                box.IsSelected = false;
            }
        }
        pictureBox.Invalidate();
    }

    public void SetBox( int tag, Rectangle rect )
    {
        foreach( var box in zBoxes ) {
            if( box.Tag == tag ) {
                box.Rect = new Rectangle( rect.Left, rect.Top, rect.Width, rect.Height );
                break;
            }
        }

        pictureBox.Invalidate();
    }

    public bool DeleteSelectedIfAllowed()
    {
        if( deleteBoxMode ) {
            foreach( var box in zBoxes ) {
                if( box.IsSelected ) {
                    zBoxes.Remove( box );
                    BoxDeleted( this, new BoxChangedEventArgs( box.Tag, box.Rect ) );
                    pictureBox.Invalidate();
                    return true;
                }
            }
        }
        return false;
    }

    public void Clear()
    {
        zBoxes.Clear();
        areaBoxes.Clear();
        pictureBox.Invalidate();
    }

    private BoxIntersectionType findBox( Point p, out int boxIndex )
    {
        boxIndex = -1;
        if( disableEdit ) {
            return BoxIntersectionType.None;
        }

        int delta = 4;
        for( int i = 0; i < zBoxes.Count; i++ ) {
            Rectangle r = getBoxRect( zBoxes[i] );
            r.Inflate( delta, delta );
            if( r.Contains( p ) ) {
                boxIndex = i;
                break;
            }
        }
        if( boxIndex != -1 ) {
            Rectangle rect = getBoxRect( zBoxes[boxIndex] );
            if( rect.Contains( p ) ) {
                delta = 0;
            }
            if( Math.Abs( rect.Bottom - p.Y ) <= delta ) {
                if( Math.Abs( rect.Right - p.X ) <= delta ) {
                    return BoxIntersectionType.RightBottom;
                }
                if( Math.Abs( rect.Left - p.X ) <= delta ) {
                    return BoxIntersectionType.LeftBottom;
                }
                return BoxIntersectionType.Bottom;
            }
            if( Math.Abs( rect.Top - p.Y ) <= delta ) {
                if( Math.Abs( rect.Right - p.X ) <= delta ) {
                    return BoxIntersectionType.RightTop;
                }
                if( Math.Abs( rect.Left - p.X ) <= delta ) {
                    return BoxIntersectionType.LeftTop;
                }
                return BoxIntersectionType.Top;
            }
            if( Math.Abs( rect.Left - p.X ) <= delta ) {
                return BoxIntersectionType.Left;
            }
            if( Math.Abs( rect.Right - p.X ) <= delta ) {
                return BoxIntersectionType.Right;
            }
            return BoxIntersectionType.Inside;
        }
        if( newBoxMode && getImageRect().Contains( p ) ) {
            return BoxIntersectionType.New;
        }
        return BoxIntersectionType.None;
    }

    private Rectangle getBoxRect( Box box )
    {
        return getClientRect( box.Rect );
    }

    private Rectangle getImageRect()
    { 
        return new Rectangle( deltaX, deltaY, (int)( pictureBox.Image.Width * scale ), (int)( pictureBox.Image.Height * scale ) );
    }

    private Rectangle getClientRect( Rectangle _rect )
    {
        int x1 = ( int )Math.Round( scale * _rect.Left ) - 1;
        int y1 = ( int )Math.Round( scale * _rect.Top ) - 1;
        int x2 = ( int )Math.Round( scale * _rect.Right ) + 1;
        int y2 = ( int )Math.Round( scale * _rect.Bottom ) + 1;
        
        Rectangle rect = new Rectangle(  x1, y1, x2 - x1, y2 - y1 );
        rect.Offset( deltaX, deltaY );

        return rect;
    }

    private void setBoxRect( Box box, Rectangle newRect )
    {
        Rectangle rect = newRect;
        rect.Offset( -deltaX, -deltaY );
        rect.X = ( int )Math.Round( ( rect.Left + 1 ) / scale );
        rect.Y = ( int )Math.Round( ( rect.Top + 1 )/ scale );
        rect.Width = ( int )Math.Round( ( rect.Width - 2 ) / scale );
        rect.Height = ( int )Math.Round( ( rect.Height - 2) / scale );
        box.Rect = rect;
    }

    private void pictureBox_Paint( object sender, PaintEventArgs e )
    {
        if( pictureBox.Image != null ) {
                   
            scale = Math.Min( ( 1.0 * pictureBox.Height ) / pictureBox.Image.Height,
                ( 1.0 * pictureBox.Width ) / pictureBox.Image.Width );
            deltaY = ( pictureBox.Height - ( int )Math.Round( scale * pictureBox.Image.Height ) ) / 2;
            deltaX = ( pictureBox.Width - ( int )Math.Round( scale * pictureBox.Image.Width ) ) / 2;

            Graphics g = e.Graphics;
            foreach( var box in zBoxes ) {
                Rectangle rect = getBoxRect( box );
                for( int i = box.IsSelected ? 2 : 0; ; i-- ) {
                    Rectangle r = rect;
                    r.Inflate( i, i );
                    Pen pen = new Pen( Color.FromArgb( box.Color.A - i * 60, box.Color ) );
                    g.DrawRectangle( pen, r );
                    if( i == 0 ) {
                        g.DrawRectangle( pen, new Rectangle( r.X - 2, r.Y - 2, 4, 4 ) );
                        g.FillRectangle( Brushes.White, new Rectangle( r.X - 1, r.Y - 1, 3, 3 ) );
                        g.DrawRectangle( pen, new Rectangle( r.X + r.Width - 2, r.Y - 2, 4, 4 ) );
                        g.FillRectangle( Brushes.White, new Rectangle( r.X + r.Width - 1, r.Y - 1, 3, 3 ) );
                        g.DrawRectangle( pen, new Rectangle( r.X - 2, r.Y + r.Height - 2, 4, 4 ) );
                        g.FillRectangle( Brushes.White, new Rectangle( r.X - 1, r.Y + r.Height - 1, 3, 3 ) );
                        g.DrawRectangle( pen, new Rectangle( r.X + r.Width - 2, r.Y + r.Height - 2, 4, 4 ) );
                        g.FillRectangle( Brushes.White, new Rectangle( r.X + r.Width - 1, r.Y + r.Height - 1, 3, 3 ) );
                        break;
                    }
                }
            }

            foreach( var box in areaBoxes ) {
                Rectangle rect = getBoxRect( box );
                Brush brush = new SolidBrush( Color.FromArgb( 20, box.Color ) );
                g.FillRectangle( brush, rect );
            }
        }
    }

    void pictureBox_MouseDown( object sender, MouseEventArgs e )
    {
        int boxIndex;
        currentBoxIntersection = findBox( e.Location, out boxIndex );
        foreach( var box in zBoxes ) {
            box.IsSelected = false;
        }
        if( currentBoxIntersection != BoxIntersectionType.None ) {
            pictureBox.Capture = true;
            prevMouse = e.Location;
            Box newTopBox;
            if( currentBoxIntersection == BoxIntersectionType.New ) {
                Rectangle newRect = new Rectangle( e.Location, new Size( 1, 1 ) );
                newTopBox = new Box( -1, 0, 0, 0, 0, true, newBoxColor );
                setBoxRect( newTopBox, newRect );
                zBoxes.Add( null );
                zBoxes[zBoxes.Count - 1] = zBoxes[0];
            } else {
                newTopBox = zBoxes[boxIndex];
                zBoxes[boxIndex] = zBoxes[0];
            }
            zBoxes[0] = newTopBox;
            newTopBox.IsSelected = true;
            prevRect = getBoxRect( newTopBox );
        }
        pictureBox.Invalidate();
    }

    void pictureBox_MouseMove( object sender, MouseEventArgs e )
    {
        if( pictureBox.Capture == true ) {
            if( currentBoxIntersection != BoxIntersectionType.None ) {
                Point delta = e.Location;
                delta.Offset( -prevMouse.X, -prevMouse.Y );
                Rectangle newRect = prevRect;
                switch( currentBoxIntersection ) {
                    case BoxIntersectionType.Inside: newRect.Offset( delta ); break;
                    case BoxIntersectionType.Left: newRect.X += delta.X; newRect.Width -= delta.X; break;
                    case BoxIntersectionType.Right: newRect.Width += delta.X; break;
                    case BoxIntersectionType.Top: newRect.Y += delta.Y; newRect.Height -= delta.Y; break;
                    case BoxIntersectionType.Bottom: newRect.Height += delta.Y; break;
                    case BoxIntersectionType.LeftTop:
                        newRect.X += delta.X; newRect.Width -= delta.X;
                        newRect.Y += delta.Y; newRect.Height -= delta.Y;
                        break;
                    case BoxIntersectionType.RightTop:
                        newRect.Width += delta.X;
                        newRect.Y += delta.Y; newRect.Height -= delta.Y;
                        break;
                    case BoxIntersectionType.LeftBottom:
                        newRect.X += delta.X; newRect.Width -= delta.X;
                        newRect.Height += delta.Y; ;
                        break;
                    case BoxIntersectionType.RightBottom:
                        newRect.Width += delta.X;
                        newRect.Height += delta.Y;
                        break;
                    case BoxIntersectionType.New:
                        if( delta.X >= 0 ) {
                            newRect.Width += delta.X;
                        } else {
                            newRect.X = e.X;
                            newRect.Width = -delta.X;
                        }
                        if( delta.Y >= 0 ) {
                            newRect.Height += delta.Y;
                        } else {
                            newRect.Y = e.Y;
                            newRect.Height = -delta.Y;
                        }
                        break;
                }
                Rectangle imageRect = getImageRect();
                if( newRect.X <= imageRect.X ) {
                    newRect.X = imageRect.X;
                }
                if( newRect.Y <= imageRect.Y ) {
                    newRect.Y = imageRect.Y;
                }
                if( newRect.Right > imageRect.Right ) {
                    newRect.Width = imageRect.Right - newRect.X;
                }
                if( newRect.Bottom > imageRect.Bottom ) {
                    newRect.Height = imageRect.Bottom - newRect.Y;
                }
                if( newRect.Width <= 0 ) {
                    if( newRect.Right <= prevRect.Left ) {
                        newRect.X = prevRect.Left;
                    } else {
                        newRect.X = prevRect.Right;
                    }
                    newRect.Width = 0;
                }
                if( newRect.Height <= 0 ) {
                    if( newRect.Bottom <= prevRect.Top ) {
                        newRect.Y = prevRect.Top;
                    } else {
                        newRect.Y = prevRect.Bottom;
                    }
                    newRect.Height = 0;
                }
                setBoxRect( zBoxes[0], newRect );
                pictureBox.Invalidate();
            }
        } else {
            int _boxIndex;
            switch( findBox( e.Location, out _boxIndex ) ) {
                case BoxIntersectionType.Inside: pictureBox.Cursor = Cursors.SizeAll; break;
                case BoxIntersectionType.Left: pictureBox.Cursor = Cursors.SizeWE; break;
                case BoxIntersectionType.Right: pictureBox.Cursor = Cursors.SizeWE; break;
                case BoxIntersectionType.Top: pictureBox.Cursor = Cursors.SizeNS; break;
                case BoxIntersectionType.Bottom: pictureBox.Cursor = Cursors.SizeNS; break;
                case BoxIntersectionType.LeftTop: pictureBox.Cursor = Cursors.SizeNWSE; break;
                case BoxIntersectionType.RightTop: pictureBox.Cursor = Cursors.SizeNESW; break;
                case BoxIntersectionType.LeftBottom: pictureBox.Cursor = Cursors.SizeNESW; break;
                case BoxIntersectionType.RightBottom: pictureBox.Cursor = Cursors.SizeNWSE; break;
                case BoxIntersectionType.New: pictureBox.Cursor = Cursors.Cross; break;
                default: pictureBox.Cursor = Cursors.Default; break;
            }
        }
    }

    void pictureBox_MouseUp( object sender, MouseEventArgs e )
    {
        if( pictureBox.Capture == true ) {
            pictureBox.Capture = false;
            if( currentBoxIntersection != BoxIntersectionType.None ) {
                var args = new BoxChangedEventArgs( zBoxes[0].Tag, zBoxes[0].Rect );
                if( BoxChanged != null ) {
                    if( currentBoxIntersection != BoxIntersectionType.New ) {
                        BoxChanged( this, args );
                    } else {
                        BoxCreated( this, args );
                    }
                }
                zBoxes[0].Tag = args.Tag;
            }
        }
    }

    public class BoxChangedEventArgs : EventArgs
    {
        public int Tag;
        public Rectangle Rect;

        public BoxChangedEventArgs( int tag, Rectangle rect ) { Tag = tag; Rect = rect; }
    }

    public event EventHandler<BoxChangedEventArgs> BoxChanged;
    public event EventHandler<BoxChangedEventArgs> BoxCreated;
    public event EventHandler<BoxChangedEventArgs> BoxDeleted;

    private void pictureBox_MouseLeave( object sender, EventArgs e )
    {
        pictureBox.Cursor = Cursors.Default;
    }
}
