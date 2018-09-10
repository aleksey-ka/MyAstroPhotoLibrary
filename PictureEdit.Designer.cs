namespace MyAstroPhotoLibrary
{
    partial class PictureEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.minRUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxRUpDown = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.maxGUpDown = new System.Windows.Forms.NumericUpDown();
            this.minGUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.maxBUpDown = new System.Windows.Forms.NumericUpDown();
            this.minBUpDown = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.autoLevelsButton = new System.Windows.Forms.Button();
            this.lowTextBox = new System.Windows.Forms.TextBox();
            this.hiTextBox = new System.Windows.Forms.TextBox();
            ( (System.ComponentModel.ISupportInitialize) ( this.pictureBox ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.minRUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.maxRUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.maxGUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.minGUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.maxBUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.minBUpDown ) ).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.pictureBox.Location = new System.Drawing.Point( 12, 12 );
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size( 826, 498 );
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // minRUpDown
            // 
            this.minRUpDown.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.minRUpDown.Increment = new decimal( new int[] {
            10,
            0,
            0,
            0} );
            this.minRUpDown.Location = new System.Drawing.Point( 36, 516 );
            this.minRUpDown.Maximum = new decimal( new int[] {
            65535,
            0,
            0,
            0} );
            this.minRUpDown.Name = "minRUpDown";
            this.minRUpDown.Size = new System.Drawing.Size( 120, 22 );
            this.minRUpDown.TabIndex = 1;
            // 
            // maxRUpDown
            // 
            this.maxRUpDown.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.maxRUpDown.Increment = new decimal( new int[] {
            10,
            0,
            0,
            0} );
            this.maxRUpDown.Location = new System.Drawing.Point( 162, 516 );
            this.maxRUpDown.Maximum = new decimal( new int[] {
            65535,
            0,
            0,
            0} );
            this.maxRUpDown.Name = "maxRUpDown";
            this.maxRUpDown.Size = new System.Drawing.Size( 120, 22 );
            this.maxRUpDown.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.button1.Location = new System.Drawing.Point( 309, 518 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 75, 23 );
            this.button1.TabIndex = 3;
            this.button1.Text = "Render";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler( this.render_Click );
            // 
            // label1
            // 
            this.label1.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 518 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 18, 17 );
            this.label1.TabIndex = 4;
            this.label1.Text = "R";
            // 
            // label2
            // 
            this.label2.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 12, 546 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 19, 17 );
            this.label2.TabIndex = 7;
            this.label2.Text = "G";
            // 
            // maxGUpDown
            // 
            this.maxGUpDown.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.maxGUpDown.Increment = new decimal( new int[] {
            10,
            0,
            0,
            0} );
            this.maxGUpDown.Location = new System.Drawing.Point( 162, 544 );
            this.maxGUpDown.Maximum = new decimal( new int[] {
            65535,
            0,
            0,
            0} );
            this.maxGUpDown.Name = "maxGUpDown";
            this.maxGUpDown.Size = new System.Drawing.Size( 120, 22 );
            this.maxGUpDown.TabIndex = 6;
            // 
            // minGUpDown
            // 
            this.minGUpDown.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.minGUpDown.Increment = new decimal( new int[] {
            10,
            0,
            0,
            0} );
            this.minGUpDown.Location = new System.Drawing.Point( 36, 544 );
            this.minGUpDown.Maximum = new decimal( new int[] {
            65535,
            0,
            0,
            0} );
            this.minGUpDown.Name = "minGUpDown";
            this.minGUpDown.Size = new System.Drawing.Size( 120, 22 );
            this.minGUpDown.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 12, 574 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 17, 17 );
            this.label3.TabIndex = 10;
            this.label3.Text = "B";
            // 
            // maxBUpDown
            // 
            this.maxBUpDown.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.maxBUpDown.Increment = new decimal( new int[] {
            10,
            0,
            0,
            0} );
            this.maxBUpDown.Location = new System.Drawing.Point( 162, 572 );
            this.maxBUpDown.Maximum = new decimal( new int[] {
            65535,
            0,
            0,
            0} );
            this.maxBUpDown.Name = "maxBUpDown";
            this.maxBUpDown.Size = new System.Drawing.Size( 120, 22 );
            this.maxBUpDown.TabIndex = 9;
            // 
            // minBUpDown
            // 
            this.minBUpDown.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.minBUpDown.Increment = new decimal( new int[] {
            10,
            0,
            0,
            0} );
            this.minBUpDown.Location = new System.Drawing.Point( 36, 572 );
            this.minBUpDown.Maximum = new decimal( new int[] {
            65535,
            0,
            0,
            0} );
            this.minBUpDown.Name = "minBUpDown";
            this.minBUpDown.Size = new System.Drawing.Size( 120, 22 );
            this.minBUpDown.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.button2.Location = new System.Drawing.Point( 763, 571 );
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size( 75, 23 );
            this.button2.TabIndex = 11;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler( this.save_Click );
            // 
            // autoLevelsButton
            // 
            this.autoLevelsButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.autoLevelsButton.Location = new System.Drawing.Point( 309, 571 );
            this.autoLevelsButton.Name = "autoLevelsButton";
            this.autoLevelsButton.Size = new System.Drawing.Size( 129, 23 );
            this.autoLevelsButton.TabIndex = 12;
            this.autoLevelsButton.Text = "AutoLevels";
            this.autoLevelsButton.UseVisualStyleBackColor = true;
            this.autoLevelsButton.Click += new System.EventHandler( this.autoLevelsButton_Click );
            // 
            // lowTextBox
            // 
            this.lowTextBox.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.lowTextBox.Location = new System.Drawing.Point( 309, 547 );
            this.lowTextBox.Name = "lowTextBox";
            this.lowTextBox.Size = new System.Drawing.Size( 100, 22 );
            this.lowTextBox.TabIndex = 13;
            this.lowTextBox.Text = "0,01";
            // 
            // hiTextBox
            // 
            this.hiTextBox.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.hiTextBox.Location = new System.Drawing.Point( 415, 546 );
            this.hiTextBox.Name = "hiTextBox";
            this.hiTextBox.Size = new System.Drawing.Size( 100, 22 );
            this.hiTextBox.TabIndex = 13;
            this.hiTextBox.Text = "0,9998";
            // 
            // PictureEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 8F, 16F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 850, 606 );
            this.Controls.Add( this.hiTextBox );
            this.Controls.Add( this.lowTextBox );
            this.Controls.Add( this.autoLevelsButton );
            this.Controls.Add( this.button2 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.maxBUpDown );
            this.Controls.Add( this.minBUpDown );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.maxGUpDown );
            this.Controls.Add( this.minGUpDown );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.button1 );
            this.Controls.Add( this.maxRUpDown );
            this.Controls.Add( this.minRUpDown );
            this.Controls.Add( this.pictureBox );
            this.Name = "PictureEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PictureEdit";
            ( (System.ComponentModel.ISupportInitialize) ( this.pictureBox ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.minRUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.maxRUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.maxGUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.minGUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.maxBUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.minBUpDown ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.NumericUpDown minRUpDown;
        private System.Windows.Forms.NumericUpDown maxRUpDown;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown maxGUpDown;
        private System.Windows.Forms.NumericUpDown minGUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown maxBUpDown;
        private System.Windows.Forms.NumericUpDown minBUpDown;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button autoLevelsButton;
        private System.Windows.Forms.TextBox lowTextBox;
        private System.Windows.Forms.TextBox hiTextBox;
    }
}