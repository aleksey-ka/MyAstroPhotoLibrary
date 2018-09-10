using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MyAstroPhotoLibrary
{
	/// <summary>
	/// A simple input dialog.
	/// </summary>
	public class InputDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.Label label;
        private IContainer components;
        private ComboBox comboBox;
		private System.Windows.Forms.ErrorProvider errorProvider;

        public InputDialog( string caption, string prompt, string initialValue, InputValidationMethod validationMethod )
		{
			InitializeComponent();

			Text = caption;
			label.Text = prompt;
            textBox.Text = initialValue;
            textBox.Visible = true;
			validateInput = validationMethod;
		}

        public InputDialog( string caption, string prompt, string[] values, string initialValue )
		{
			InitializeComponent();

			Text = caption;
			label.Text = prompt;
            int selectedIndex = -1;
            for( int i = 0; i < values.Length; i++ ) {
                comboBox.Items.Add( values[i] );
                if( values[i] == initialValue ) {
                    selectedIndex  = i;
                }
            }
            
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.SelectedIndex = selectedIndex;
            comboBox.Visible = true;			
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.label = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider();
			this.comboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 16, 96 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 1;
            this.okButton.TabStop = false;
            this.okButton.Text = "OK";
            // 
            // cancelButton
            // 
            this.cancelButton.CausesValidation = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 104, 96 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 2;
            this.cancelButton.TabStop = false;
            this.cancelButton.Text = "Cancel";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point( 16, 48 );
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size( 160, 20 );
            this.textBox.TabIndex = 0;
            this.textBox.Visible = false;
            this.textBox.Validating += new System.ComponentModel.CancelEventHandler( this.InputDialog_Validating );
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point( 16, 26 );
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size( 160, 20 );
            this.label.TabIndex = 3;
            this.label.Text = "Prompt";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // comboBox
            //
            this.comboBox.Location = new System.Drawing.Point( 16, 49 );
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size( 160, 21 );
            this.comboBox.TabIndex = 4;
            this.comboBox.Visible = false;
            this.comboBox.SelectedIndexChanged += new System.EventHandler( this.comboBox_SelectedIndexChanged );
            // 
            // InputDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 194, 131 );
            this.Controls.Add( this.comboBox );
            this.Controls.Add( this.textBox );
            this.Controls.Add( this.label );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Caption";
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		public string InputText;

		public delegate bool InputValidationMethod( string text, out string errorMessage  );
		InputValidationMethod validateInput;
		
		private void InputDialog_Validating(object sender, System.ComponentModel.CancelEventArgs e) 
		{
			InputText = "";
			if( validateInput != null ) {
				bool valid = true;
				string errorMessage = "";
				try {
					valid = validateInput( textBox.Text, out errorMessage );
				} catch( Exception ex ) {
					valid = false;
					errorMessage = ex.Message;
				}
				if( !valid ) {
					// Cancel the event and select the text to be corrected by the user.
					e.Cancel = true;
					textBox.Select( 0, textBox.Text.Length );

					// Set the ErrorProvider error with the text to display. 
					errorProvider.SetError( textBox, errorMessage );

					return;
				}
			}
			InputText = textBox.Text;
		}

        private void comboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            InputText = comboBox.SelectedItem.ToString();
        }
	}
}
