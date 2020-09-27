using System.Windows.Forms;
using System.Drawing;

namespace MyAstroPhotoLibrary
{   
    class ProgressBarForm : Form, IMessageFilter
	{
		public ProgressBarForm( Form owner )
		{
			initialize( owner, true );
		}

        public ProgressBarForm( Form owner, bool cancelEnabled )
		{
			initialize( owner, cancelEnabled );
		}

        protected void initialize( Form owner, bool cancelEnabled )
        {
            Text = "Please wait...";
            Height = 125;
            Width = 350;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Location = new Point( 10, 800 );

			label = new Label();
            label.Dock = DockStyle.Top;
            label.Height = 26;
            label.Padding = new Padding( 5 );
			Controls.Add( label );
            progressBar = new ProgressBar();
            progressBar.Top = label.Bottom + 5;
            progressBar.Left = 5;
            progressBar.Width = ClientSize.Width - 10;
			Controls.Add( progressBar );
            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Top = progressBar.Bottom + 8;
            cancelButton.Left = ClientSize.Width - 5 - cancelButton.Width;
            cancelButton.MouseClick +=  new MouseEventHandler( cancelButton_MouseClick );
            cancelButton.Enabled = cancelEnabled;
            Controls.Add( cancelButton );
            							
            Show( owner );
            //CenterToParent();
            
            Application.UseWaitCursor = true;
            Application.AddMessageFilter( this );
            
            ShowProgress( 0 );
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_NOCLOSE = 0x200;
                const int WS_EX_APPWINDOW = 0x40000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_NOCLOSE;
                cp.ExStyle &= ~WS_EX_APPWINDOW;
                return cp;
            }
        }

        private const int WM_MOUSEFIRST = 0x0200;
        private const int WM_MOUSELAST  = 0x020A;
        private const int WM_KEYFIRST = 0x0100;
        private const int WM_KEYLAST = 0x0109;

        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            if( m.Msg >= WM_MOUSEFIRST && m.Msg <= WM_MOUSELAST ) {
                if( m.HWnd == cancelButton.Handle && cancelButton.Enabled ) {
                    return false;
                }
                return true;
            }
            if( m.Msg >= WM_KEYFIRST && m.Msg <= WM_KEYLAST ) { 
                return true;
            }
            return false;
        }
        
		public void ShowProgress( int progress ) 
        { 
            progressBar.Value = progress;
            
            CenterToParent();
            Application.DoEvents();
        }

        public void ShowMessage( string message ) 
        { 
            label.Text = message;
            
            CenterToParent();
            Application.DoEvents();
        }

        public void EndProgress()
        {
            cancelButton.Enabled = false;
            Application.DoEvents();
            Application.UseWaitCursor = false;
            Application.RemoveMessageFilter( this );
            base.Close();
        }

        void cancelButton_MouseClick( object sender, MouseEventArgs e )
        {
            cancelButton.Enabled = false;
            ShowMessage( "Cancelling..." );
            throw new System.OperationCanceledException();
        }

        ProgressBar progressBar;
        Button cancelButton;
        Label label;
	}
}
