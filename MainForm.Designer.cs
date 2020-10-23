namespace MyAstroPhotoLibrary
{
    partial class MainForm
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
            libraw.Dispose();
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.mainPictureContextMenu = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.fullScreenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doStackMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPictureSaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fillPanel = new System.Windows.Forms.Panel();
            this.exitFullScreenButton = new System.Windows.Forms.Button();
            this.pictureBoxPanel = new System.Windows.Forms.Panel();
            this.stackPanel = new System.Windows.Forms.Panel();
            this.stackFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.stackHeaderPanel = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.stackToolsPanel = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.stackSelectedCheckBox = new System.Windows.Forms.CheckBox();
            this.zoomPanel = new System.Windows.Forms.Panel();
            this.zoomPictureBox = new System.Windows.Forms.PictureBox();
            this.zoomContextMenu = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.stackZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.imageInfoToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calcStatisticsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.zoomSaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waveletsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextWaveletMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bgMaskMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detectBGMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doExperimentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveStackToCFAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alignOnCenterOfMassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.saveStackTo16bitPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveStackTo160bitTIFF1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewToolsPanel = new System.Windows.Forms.Panel();
            this.waveletUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.waveletUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.waveletUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.waveletUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.waveletUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.satNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.gammaNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.KbNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.KgNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.BgBnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.BgGnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.BgRnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.BgNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.MaxVNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.positionLabel = new System.Windows.Forms.Label();
            this.stackCheckBox = new System.Windows.Forms.CheckBox();
            this.noZoomRadioButton = new System.Windows.Forms.RadioButton();
            this.zoom1xRadioButton = new System.Windows.Forms.RadioButton();
            this.zoom2xRadioButton = new System.Windows.Forms.RadioButton();
            this.zoom4xRadioButton = new System.Windows.Forms.RadioButton();
            this.zoomCFARadioButton = new System.Windows.Forms.RadioButton();
            this.nextButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.mainToolsPanel = new System.Windows.Forms.Panel();
            this.exportToFITSButton = new System.Windows.Forms.Button();
            this.toolsMenuButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.goFullScreenButton = new System.Windows.Forms.Button();
            this.exportToIRISButton = new System.Windows.Forms.Button();
            this.openFolderButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.stackButton = new System.Windows.Forms.Button();
            this.infoRadioButton = new System.Windows.Forms.RadioButton();
            this.rawPreviewRadioButton = new System.Windows.Forms.RadioButton();
            this.quickPreviewRadioButton = new System.Windows.Forms.RadioButton();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.toolsContextMenu = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.buildDarksMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thumbnailsContextMenu = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.createFlatMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thumbnailsView = new MyAstroPhotoLibrary.ThumbnailsView();
            ( (System.ComponentModel.ISupportInitialize) ( this.pictureBox ) ).BeginInit();
            this.mainPictureContextMenu.SuspendLayout();
            this.fillPanel.SuspendLayout();
            this.pictureBoxPanel.SuspendLayout();
            this.stackPanel.SuspendLayout();
            this.stackHeaderPanel.SuspendLayout();
            this.stackToolsPanel.SuspendLayout();
            this.zoomPanel.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize) ( this.zoomPictureBox ) ).BeginInit();
            this.zoomContextMenu.SuspendLayout();
            this.previewToolsPanel.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize) ( this.waveletUpDown5 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.waveletUpDown4 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.waveletUpDown3 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.waveletUpDown2 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.waveletUpDown1 ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.satNumericUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.gammaNumericUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.KbNumericUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.KgNumericUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.BgBnumericUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.BgGnumericUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.BgRnumericUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.BgNumericUpDown ) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.MaxVNumericUpDown ) ).BeginInit();
            this.mainToolsPanel.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.toolsContextMenu.SuspendLayout();
            this.thumbnailsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.ContextMenuStrip = this.mainPictureContextMenu;
            this.pictureBox.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point( 0, 0 );
            this.pictureBox.Margin = new System.Windows.Forms.Padding( 4 );
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size( 1251, 628 );
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictureBox_MouseDown );
            // 
            // mainPictureContextMenu
            // 
            this.mainPictureContextMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fullScreenMenuItem,
            this.doStackMenuItem,
            this.mainPictureSaveAsMenuItem} );
            this.mainPictureContextMenu.Name = "mainPictureViewContextMenu";
            this.mainPictureContextMenu.Size = new System.Drawing.Size( 150, 76 );
            // 
            // fullScreenMenuItem
            // 
            this.fullScreenMenuItem.Name = "fullScreenMenuItem";
            this.fullScreenMenuItem.Size = new System.Drawing.Size( 149, 24 );
            this.fullScreenMenuItem.Text = "Full Screen";
            this.fullScreenMenuItem.Click += new System.EventHandler( this.enterFullScreen_Click );
            // 
            // doStackMenuItem
            // 
            this.doStackMenuItem.Name = "doStackMenuItem";
            this.doStackMenuItem.Size = new System.Drawing.Size( 149, 24 );
            this.doStackMenuItem.Text = "Do Stack";
            this.doStackMenuItem.Click += new System.EventHandler( this.doStack_Click );
            // 
            // mainPictureSaveAsMenuItem
            // 
            this.mainPictureSaveAsMenuItem.Name = "mainPictureSaveAsMenuItem";
            this.mainPictureSaveAsMenuItem.Size = new System.Drawing.Size( 149, 24 );
            this.mainPictureSaveAsMenuItem.Text = "Save As ...";
            this.mainPictureSaveAsMenuItem.Click += new System.EventHandler( this.mainPictureViewSaveAs_Click );
            // 
            // fillPanel
            // 
            this.fillPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fillPanel.Controls.Add( this.exitFullScreenButton );
            this.fillPanel.Controls.Add( this.pictureBoxPanel );
            this.fillPanel.Controls.Add( this.previewToolsPanel );
            this.fillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillPanel.Location = new System.Drawing.Point( 0, 0 );
            this.fillPanel.Margin = new System.Windows.Forms.Padding( 4 );
            this.fillPanel.Name = "fillPanel";
            this.fillPanel.Padding = new System.Windows.Forms.Padding( 1 );
            this.fillPanel.Size = new System.Drawing.Size( 1255, 672 );
            this.fillPanel.TabIndex = 3;
            // 
            // exitFullScreenButton
            // 
            this.exitFullScreenButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.exitFullScreenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitFullScreenButton.Location = new System.Drawing.Point( 1213, 4 );
            this.exitFullScreenButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.exitFullScreenButton.Name = "exitFullScreenButton";
            this.exitFullScreenButton.Size = new System.Drawing.Size( 37, 28 );
            this.exitFullScreenButton.TabIndex = 15;
            this.exitFullScreenButton.Text = "X";
            this.exitFullScreenButton.UseVisualStyleBackColor = true;
            this.exitFullScreenButton.Visible = false;
            this.exitFullScreenButton.Click += new System.EventHandler( this.exitFullScreen_Click );
            // 
            // pictureBoxPanel
            // 
            this.pictureBoxPanel.Controls.Add( this.stackPanel );
            this.pictureBoxPanel.Controls.Add( this.zoomPanel );
            this.pictureBoxPanel.Controls.Add( this.pictureBox );
            this.pictureBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPanel.Location = new System.Drawing.Point( 1, 1 );
            this.pictureBoxPanel.Name = "pictureBoxPanel";
            this.pictureBoxPanel.Size = new System.Drawing.Size( 1251, 628 );
            this.pictureBoxPanel.TabIndex = 16;
            // 
            // stackPanel
            // 
            this.stackPanel.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.stackPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stackPanel.Controls.Add( this.stackFlowPanel );
            this.stackPanel.Controls.Add( this.stackHeaderPanel );
            this.stackPanel.Controls.Add( this.stackToolsPanel );
            this.stackPanel.Location = new System.Drawing.Point( 1, 179 );
            this.stackPanel.Margin = new System.Windows.Forms.Padding( 4 );
            this.stackPanel.Name = "stackPanel";
            this.stackPanel.Size = new System.Drawing.Size( 429, 449 );
            this.stackPanel.TabIndex = 5;
            this.stackPanel.Visible = false;
            // 
            // stackFlowPanel
            // 
            this.stackFlowPanel.AutoScroll = true;
            this.stackFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.stackFlowPanel.Location = new System.Drawing.Point( 0, 26 );
            this.stackFlowPanel.Margin = new System.Windows.Forms.Padding( 4 );
            this.stackFlowPanel.Name = "stackFlowPanel";
            this.stackFlowPanel.Size = new System.Drawing.Size( 427, 381 );
            this.stackFlowPanel.TabIndex = 1;
            this.stackFlowPanel.WrapContents = false;
            // 
            // stackHeaderPanel
            // 
            this.stackHeaderPanel.Controls.Add( this.label7 );
            this.stackHeaderPanel.Controls.Add( this.label1 );
            this.stackHeaderPanel.Controls.Add( this.label6 );
            this.stackHeaderPanel.Controls.Add( this.label5 );
            this.stackHeaderPanel.Controls.Add( this.label4 );
            this.stackHeaderPanel.Controls.Add( this.label8 );
            this.stackHeaderPanel.Controls.Add( this.label3 );
            this.stackHeaderPanel.Controls.Add( this.label2 );
            this.stackHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.stackHeaderPanel.Location = new System.Drawing.Point( 0, 0 );
            this.stackHeaderPanel.Margin = new System.Windows.Forms.Padding( 4 );
            this.stackHeaderPanel.Name = "stackHeaderPanel";
            this.stackHeaderPanel.Size = new System.Drawing.Size( 427, 26 );
            this.stackHeaderPanel.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point( 386, 1 );
            this.label7.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 26, 20 );
            this.label7.TabIndex = 0;
            this.label7.Text = "!";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point( 321, 1 );
            this.label1.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 45, 20 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Wide";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point( 264, 1 );
            this.label6.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 55, 20 );
            this.label6.TabIndex = 0;
            this.label6.Text = "LowBG";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point( 212, 1 );
            this.label5.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 50, 20 );
            this.label5.TabIndex = 0;
            this.label5.Text = "HiRes";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point( 153, 1 );
            this.label4.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 57, 20 );
            this.label4.TabIndex = 0;
            this.label4.Text = "FWHM";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point( 41, 1 );
            this.label8.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 58, 20 );
            this.label8.TabIndex = 0;
            this.label8.Text = "Aligned";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point( 101, 1 );
            this.label3.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 50, 20 );
            this.label3.TabIndex = 0;
            this.label3.Text = "BG";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point( 1, 1 );
            this.label2.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 38, 20 );
            this.label2.TabIndex = 0;
            this.label2.Text = "#";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stackToolsPanel
            // 
            this.stackToolsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stackToolsPanel.Controls.Add( this.button8 );
            this.stackToolsPanel.Controls.Add( this.button7 );
            this.stackToolsPanel.Controls.Add( this.button6 );
            this.stackToolsPanel.Controls.Add( this.stackSelectedCheckBox );
            this.stackToolsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stackToolsPanel.Location = new System.Drawing.Point( 0, 407 );
            this.stackToolsPanel.Margin = new System.Windows.Forms.Padding( 4 );
            this.stackToolsPanel.Name = "stackToolsPanel";
            this.stackToolsPanel.Size = new System.Drawing.Size( 427, 40 );
            this.stackToolsPanel.TabIndex = 0;
            // 
            // button8
            // 
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Location = new System.Drawing.Point( 164, 4 );
            this.button8.Margin = new System.Windows.Forms.Padding( 4 );
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size( 76, 31 );
            this.button8.TabIndex = 4;
            this.button8.Text = "Process";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point( 81, 4 );
            this.button7.Margin = new System.Windows.Forms.Padding( 4 );
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size( 76, 31 );
            this.button7.TabIndex = 4;
            this.button7.Text = "Analyze";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point( 3, 4 );
            this.button6.Margin = new System.Windows.Forms.Padding( 4 );
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size( 76, 31 );
            this.button6.TabIndex = 4;
            this.button6.Text = "Align";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // stackSelectedCheckBox
            // 
            this.stackSelectedCheckBox.AutoSize = true;
            this.stackSelectedCheckBox.Checked = true;
            this.stackSelectedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stackSelectedCheckBox.Location = new System.Drawing.Point( 323, 10 );
            this.stackSelectedCheckBox.Margin = new System.Windows.Forms.Padding( 4 );
            this.stackSelectedCheckBox.Name = "stackSelectedCheckBox";
            this.stackSelectedCheckBox.Size = new System.Drawing.Size( 85, 21 );
            this.stackSelectedCheckBox.TabIndex = 9;
            this.stackSelectedCheckBox.Text = "Selected";
            this.stackSelectedCheckBox.UseVisualStyleBackColor = true;
            this.stackSelectedCheckBox.CheckedChanged += new System.EventHandler( this.stackSelectedCheckBox_CheckedChanged );
            // 
            // zoomPanel
            // 
            this.zoomPanel.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.zoomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zoomPanel.Controls.Add( this.zoomPictureBox );
            this.zoomPanel.Location = new System.Drawing.Point( 802, 178 );
            this.zoomPanel.Margin = new System.Windows.Forms.Padding( 4 );
            this.zoomPanel.Name = "zoomPanel";
            this.zoomPanel.Size = new System.Drawing.Size( 450, 450 );
            this.zoomPanel.TabIndex = 14;
            this.zoomPanel.Visible = false;
            // 
            // zoomPictureBox
            // 
            this.zoomPictureBox.ContextMenuStrip = this.zoomContextMenu;
            this.zoomPictureBox.Cursor = System.Windows.Forms.Cursors.Cross;
            this.zoomPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoomPictureBox.Location = new System.Drawing.Point( 0, 0 );
            this.zoomPictureBox.Margin = new System.Windows.Forms.Padding( 0 );
            this.zoomPictureBox.Name = "zoomPictureBox";
            this.zoomPictureBox.Size = new System.Drawing.Size( 448, 448 );
            this.zoomPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.zoomPictureBox.TabIndex = 4;
            this.zoomPictureBox.TabStop = false;
            this.zoomPictureBox.Click += new System.EventHandler( this.zoomPictureBox_Click );
            // 
            // zoomContextMenu
            // 
            this.zoomContextMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stackZoomToolStripMenuItem,
            this.toolStripSeparator1,
            this.imageInfoToolMenuItem,
            this.calcStatisticsMenuItem,
            this.toolStripSeparator2,
            this.zoomSaveAsMenuItem,
            this.waveletsMenuItem,
            this.nextWaveletMenuItem,
            this.bgMaskMenuItem,
            this.detectBGMenuItem,
            this.doExperimentToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveStackToCFAToolStripMenuItem,
            this.alignOnCenterOfMassToolStripMenuItem,
            this.toolStripSeparator4,
            this.saveStackTo16bitPNGToolStripMenuItem,
            this.saveStackTo160bitTIFF1ToolStripMenuItem} );
            this.zoomContextMenu.Name = "zoomContextMenu";
            this.zoomContextMenu.Size = new System.Drawing.Size( 244, 340 );
            // 
            // stackZoomToolStripMenuItem
            // 
            this.stackZoomToolStripMenuItem.Name = "stackZoomToolStripMenuItem";
            this.stackZoomToolStripMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.stackZoomToolStripMenuItem.Text = "Do Stack";
            this.stackZoomToolStripMenuItem.Click += new System.EventHandler( this.stackZoom_Click );
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 240, 6 );
            // 
            // imageInfoToolMenuItem
            // 
            this.imageInfoToolMenuItem.Name = "imageInfoToolMenuItem";
            this.imageInfoToolMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.imageInfoToolMenuItem.Text = "Image Info";
            this.imageInfoToolMenuItem.Click += new System.EventHandler( this.imageInfo_Click );
            // 
            // calcStatisticsMenuItem
            // 
            this.calcStatisticsMenuItem.Name = "calcStatisticsMenuItem";
            this.calcStatisticsMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.calcStatisticsMenuItem.Text = "Calc Statistics";
            this.calcStatisticsMenuItem.Click += new System.EventHandler( this.calcStatistics_Click );
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size( 240, 6 );
            // 
            // zoomSaveAsMenuItem
            // 
            this.zoomSaveAsMenuItem.Name = "zoomSaveAsMenuItem";
            this.zoomSaveAsMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.zoomSaveAsMenuItem.Text = "Save As ...";
            this.zoomSaveAsMenuItem.Click += new System.EventHandler( this.zoomSaveAs_Click );
            // 
            // waveletsMenuItem
            // 
            this.waveletsMenuItem.Name = "waveletsMenuItem";
            this.waveletsMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.waveletsMenuItem.Text = "Wavelets";
            this.waveletsMenuItem.Click += new System.EventHandler( this.wavelets_Click );
            // 
            // nextWaveletMenuItem
            // 
            this.nextWaveletMenuItem.Name = "nextWaveletMenuItem";
            this.nextWaveletMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.nextWaveletMenuItem.Text = "Next";
            this.nextWaveletMenuItem.Click += new System.EventHandler( this.nextWavelet_Click );
            // 
            // bgMaskMenuItem
            // 
            this.bgMaskMenuItem.Name = "bgMaskMenuItem";
            this.bgMaskMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.bgMaskMenuItem.Text = "BG Mask";
            this.bgMaskMenuItem.Click += new System.EventHandler( this.bgMask_Click );
            // 
            // detectBGMenuItem
            // 
            this.detectBGMenuItem.Name = "detectBGMenuItem";
            this.detectBGMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.detectBGMenuItem.Text = "Detect BG";
            this.detectBGMenuItem.Click += new System.EventHandler( this.detectBG_Click );
            // 
            // doExperimentToolStripMenuItem
            // 
            this.doExperimentToolStripMenuItem.Name = "doExperimentToolStripMenuItem";
            this.doExperimentToolStripMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.doExperimentToolStripMenuItem.Text = "Do Experiment";
            this.doExperimentToolStripMenuItem.Click += new System.EventHandler( this.doExperiment_Click );
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size( 240, 6 );
            // 
            // saveStackToCFAToolStripMenuItem
            // 
            this.saveStackToCFAToolStripMenuItem.Name = "saveStackToCFAToolStripMenuItem";
            this.saveStackToCFAToolStripMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.saveStackToCFAToolStripMenuItem.Text = "Save Stack to CFA";
            this.saveStackToCFAToolStripMenuItem.Click += new System.EventHandler( this.zoomSaveStackToCFA_Click );
            // 
            // alignOnCenterOfMassToolStripMenuItem
            // 
            this.alignOnCenterOfMassToolStripMenuItem.Name = "alignOnCenterOfMassToolStripMenuItem";
            this.alignOnCenterOfMassToolStripMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.alignOnCenterOfMassToolStripMenuItem.Text = "Save Stack to PNG";
            this.alignOnCenterOfMassToolStripMenuItem.Click += new System.EventHandler( this.zoomSaveStackToPng_Click );
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size( 240, 6 );
            // 
            // saveStackTo16bitPNGToolStripMenuItem
            // 
            this.saveStackTo16bitPNGToolStripMenuItem.Name = "saveStackTo16bitPNGToolStripMenuItem";
            this.saveStackTo16bitPNGToolStripMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.saveStackTo16bitPNGToolStripMenuItem.Text = "Save Stack to 16-bit PNG";
            this.saveStackTo16bitPNGToolStripMenuItem.Click += new System.EventHandler( this.zoomSaveStackTo16BitPng_Click );
            // 
            // saveStackTo160bitTIFF1ToolStripMenuItem
            // 
            this.saveStackTo160bitTIFF1ToolStripMenuItem.Name = "saveStackTo160bitTIFF1ToolStripMenuItem";
            this.saveStackTo160bitTIFF1ToolStripMenuItem.Size = new System.Drawing.Size( 243, 24 );
            this.saveStackTo160bitTIFF1ToolStripMenuItem.Text = "Save Stack to 16-bit TIFF";
            this.saveStackTo160bitTIFF1ToolStripMenuItem.Click += new System.EventHandler( this.saveStackTo16BitTIFF_Click );
            // 
            // previewToolsPanel
            // 
            this.previewToolsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewToolsPanel.Controls.Add( this.waveletUpDown5 );
            this.previewToolsPanel.Controls.Add( this.waveletUpDown4 );
            this.previewToolsPanel.Controls.Add( this.waveletUpDown3 );
            this.previewToolsPanel.Controls.Add( this.waveletUpDown2 );
            this.previewToolsPanel.Controls.Add( this.waveletUpDown1 );
            this.previewToolsPanel.Controls.Add( this.satNumericUpDown );
            this.previewToolsPanel.Controls.Add( this.gammaNumericUpDown );
            this.previewToolsPanel.Controls.Add( this.KbNumericUpDown );
            this.previewToolsPanel.Controls.Add( this.KgNumericUpDown );
            this.previewToolsPanel.Controls.Add( this.BgBnumericUpDown );
            this.previewToolsPanel.Controls.Add( this.BgGnumericUpDown );
            this.previewToolsPanel.Controls.Add( this.BgRnumericUpDown );
            this.previewToolsPanel.Controls.Add( this.BgNumericUpDown );
            this.previewToolsPanel.Controls.Add( this.MaxVNumericUpDown );
            this.previewToolsPanel.Controls.Add( this.positionLabel );
            this.previewToolsPanel.Controls.Add( this.stackCheckBox );
            this.previewToolsPanel.Controls.Add( this.noZoomRadioButton );
            this.previewToolsPanel.Controls.Add( this.zoom1xRadioButton );
            this.previewToolsPanel.Controls.Add( this.zoom2xRadioButton );
            this.previewToolsPanel.Controls.Add( this.zoom4xRadioButton );
            this.previewToolsPanel.Controls.Add( this.zoomCFARadioButton );
            this.previewToolsPanel.Controls.Add( this.nextButton );
            this.previewToolsPanel.Controls.Add( this.prevButton );
            this.previewToolsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.previewToolsPanel.Location = new System.Drawing.Point( 1, 629 );
            this.previewToolsPanel.Margin = new System.Windows.Forms.Padding( 4 );
            this.previewToolsPanel.Name = "previewToolsPanel";
            this.previewToolsPanel.Size = new System.Drawing.Size( 1251, 40 );
            this.previewToolsPanel.TabIndex = 3;
            // 
            // waveletUpDown5
            // 
            this.waveletUpDown5.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.waveletUpDown5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.waveletUpDown5.ForeColor = System.Drawing.Color.White;
            this.waveletUpDown5.Location = new System.Drawing.Point( 1074, 8 );
            this.waveletUpDown5.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.waveletUpDown5.Name = "waveletUpDown5";
            this.waveletUpDown5.Size = new System.Drawing.Size( 56, 22 );
            this.waveletUpDown5.TabIndex = 16;
            this.waveletUpDown5.ValueChanged += new System.EventHandler( this.waveletUpDown_ValueChanged );
            // 
            // waveletUpDown4
            // 
            this.waveletUpDown4.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.waveletUpDown4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.waveletUpDown4.ForeColor = System.Drawing.Color.White;
            this.waveletUpDown4.Location = new System.Drawing.Point( 1016, 8 );
            this.waveletUpDown4.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.waveletUpDown4.Name = "waveletUpDown4";
            this.waveletUpDown4.Size = new System.Drawing.Size( 56, 22 );
            this.waveletUpDown4.TabIndex = 16;
            this.waveletUpDown4.ValueChanged += new System.EventHandler( this.waveletUpDown_ValueChanged );
            // 
            // waveletUpDown3
            // 
            this.waveletUpDown3.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.waveletUpDown3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.waveletUpDown3.ForeColor = System.Drawing.Color.White;
            this.waveletUpDown3.Location = new System.Drawing.Point( 959, 8 );
            this.waveletUpDown3.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.waveletUpDown3.Name = "waveletUpDown3";
            this.waveletUpDown3.Size = new System.Drawing.Size( 56, 22 );
            this.waveletUpDown3.TabIndex = 16;
            this.waveletUpDown3.ValueChanged += new System.EventHandler( this.waveletUpDown_ValueChanged );
            // 
            // waveletUpDown2
            // 
            this.waveletUpDown2.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.waveletUpDown2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.waveletUpDown2.ForeColor = System.Drawing.Color.White;
            this.waveletUpDown2.Location = new System.Drawing.Point( 902, 8 );
            this.waveletUpDown2.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.waveletUpDown2.Name = "waveletUpDown2";
            this.waveletUpDown2.Size = new System.Drawing.Size( 56, 22 );
            this.waveletUpDown2.TabIndex = 16;
            this.waveletUpDown2.ValueChanged += new System.EventHandler( this.waveletUpDown_ValueChanged );
            // 
            // waveletUpDown1
            // 
            this.waveletUpDown1.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.waveletUpDown1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.waveletUpDown1.ForeColor = System.Drawing.Color.White;
            this.waveletUpDown1.Location = new System.Drawing.Point( 844, 8 );
            this.waveletUpDown1.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.waveletUpDown1.Name = "waveletUpDown1";
            this.waveletUpDown1.Size = new System.Drawing.Size( 56, 22 );
            this.waveletUpDown1.TabIndex = 15;
            this.waveletUpDown1.ValueChanged += new System.EventHandler( this.waveletUpDown_ValueChanged );
            // 
            // satNumericUpDown
            // 
            this.satNumericUpDown.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.satNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.satNumericUpDown.ForeColor = System.Drawing.Color.White;
            this.satNumericUpDown.Location = new System.Drawing.Point( 761, 8 );
            this.satNumericUpDown.Maximum = new decimal( new int[] {
            50000,
            0,
            0,
            0} );
            this.satNumericUpDown.Name = "satNumericUpDown";
            this.satNumericUpDown.Size = new System.Drawing.Size( 56, 22 );
            this.satNumericUpDown.TabIndex = 16;
            this.satNumericUpDown.ValueChanged += new System.EventHandler( this.satNumericUpDown_ValueChanged );
            // 
            // gammaNumericUpDown
            // 
            this.gammaNumericUpDown.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.gammaNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gammaNumericUpDown.ForeColor = System.Drawing.Color.White;
            this.gammaNumericUpDown.Location = new System.Drawing.Point( 703, 8 );
            this.gammaNumericUpDown.Maximum = new decimal( new int[] {
            2550,
            0,
            0,
            0} );
            this.gammaNumericUpDown.Name = "gammaNumericUpDown";
            this.gammaNumericUpDown.Size = new System.Drawing.Size( 56, 22 );
            this.gammaNumericUpDown.TabIndex = 15;
            this.gammaNumericUpDown.ValueChanged += new System.EventHandler( this.curve_Changed );
            // 
            // KbNumericUpDown
            // 
            this.KbNumericUpDown.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.KbNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.KbNumericUpDown.ForeColor = System.Drawing.Color.White;
            this.KbNumericUpDown.Location = new System.Drawing.Point( 651, 8 );
            this.KbNumericUpDown.Maximum = new decimal( new int[] {
            510,
            0,
            0,
            0} );
            this.KbNumericUpDown.Name = "KbNumericUpDown";
            this.KbNumericUpDown.Size = new System.Drawing.Size( 44, 22 );
            this.KbNumericUpDown.TabIndex = 15;
            this.KbNumericUpDown.Value = new decimal( new int[] {
            245,
            0,
            0,
            0} );
            this.KbNumericUpDown.ValueChanged += new System.EventHandler( this.curve_Changed );
            // 
            // KgNumericUpDown
            // 
            this.KgNumericUpDown.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.KgNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.KgNumericUpDown.ForeColor = System.Drawing.Color.White;
            this.KgNumericUpDown.Location = new System.Drawing.Point( 606, 8 );
            this.KgNumericUpDown.Maximum = new decimal( new int[] {
            510,
            0,
            0,
            0} );
            this.KgNumericUpDown.Name = "KgNumericUpDown";
            this.KgNumericUpDown.Size = new System.Drawing.Size( 44, 22 );
            this.KgNumericUpDown.TabIndex = 15;
            this.KgNumericUpDown.Value = new decimal( new int[] {
            135,
            0,
            0,
            0} );
            this.KgNumericUpDown.ValueChanged += new System.EventHandler( this.curve_Changed );
            // 
            // BgBnumericUpDown
            // 
            this.BgBnumericUpDown.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.BgBnumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BgBnumericUpDown.DecimalPlaces = 2;
            this.BgBnumericUpDown.ForeColor = System.Drawing.Color.White;
            this.BgBnumericUpDown.Location = new System.Drawing.Point( 575, 14 );
            this.BgBnumericUpDown.Maximum = new decimal( new int[] {
            4095,
            0,
            0,
            0} );
            this.BgBnumericUpDown.Name = "BgBnumericUpDown";
            this.BgBnumericUpDown.Size = new System.Drawing.Size( 28, 22 );
            this.BgBnumericUpDown.TabIndex = 15;
            this.BgBnumericUpDown.ValueChanged += new System.EventHandler( this.curve_Changed );
            // 
            // BgGnumericUpDown
            // 
            this.BgGnumericUpDown.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.BgGnumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BgGnumericUpDown.DecimalPlaces = 2;
            this.BgGnumericUpDown.ForeColor = System.Drawing.Color.White;
            this.BgGnumericUpDown.Location = new System.Drawing.Point( 547, 14 );
            this.BgGnumericUpDown.Maximum = new decimal( new int[] {
            4095,
            0,
            0,
            0} );
            this.BgGnumericUpDown.Name = "BgGnumericUpDown";
            this.BgGnumericUpDown.Size = new System.Drawing.Size( 28, 22 );
            this.BgGnumericUpDown.TabIndex = 15;
            this.BgGnumericUpDown.ValueChanged += new System.EventHandler( this.curve_Changed );
            // 
            // BgRnumericUpDown
            // 
            this.BgRnumericUpDown.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.BgRnumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BgRnumericUpDown.DecimalPlaces = 2;
            this.BgRnumericUpDown.ForeColor = System.Drawing.Color.White;
            this.BgRnumericUpDown.Location = new System.Drawing.Point( 525, 14 );
            this.BgRnumericUpDown.Maximum = new decimal( new int[] {
            4095,
            0,
            0,
            0} );
            this.BgRnumericUpDown.Name = "BgRnumericUpDown";
            this.BgRnumericUpDown.Size = new System.Drawing.Size( 28, 22 );
            this.BgRnumericUpDown.TabIndex = 15;
            this.BgRnumericUpDown.ValueChanged += new System.EventHandler( this.curve_Changed );
            // 
            // BgNumericUpDown
            // 
            this.BgNumericUpDown.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.BgNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BgNumericUpDown.DecimalPlaces = 2;
            this.BgNumericUpDown.ForeColor = System.Drawing.Color.White;
            this.BgNumericUpDown.Location = new System.Drawing.Point( 484, 8 );
            this.BgNumericUpDown.Maximum = new decimal( new int[] {
            4095,
            0,
            0,
            0} );
            this.BgNumericUpDown.Name = "BgNumericUpDown";
            this.BgNumericUpDown.Size = new System.Drawing.Size( 40, 22 );
            this.BgNumericUpDown.TabIndex = 15;
            this.BgNumericUpDown.ValueChanged += new System.EventHandler( this.curve_Changed );
            // 
            // MaxVNumericUpDown
            // 
            this.MaxVNumericUpDown.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ), ( (int) ( ( (byte) ( 24 ) ) ) ) );
            this.MaxVNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MaxVNumericUpDown.ForeColor = System.Drawing.Color.White;
            this.MaxVNumericUpDown.Increment = new decimal( new int[] {
            51,
            0,
            0,
            0} );
            this.MaxVNumericUpDown.Location = new System.Drawing.Point( 426, 8 );
            this.MaxVNumericUpDown.Maximum = new decimal( new int[] {
            4095,
            0,
            0,
            0} );
            this.MaxVNumericUpDown.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.MaxVNumericUpDown.Name = "MaxVNumericUpDown";
            this.MaxVNumericUpDown.Size = new System.Drawing.Size( 56, 22 );
            this.MaxVNumericUpDown.TabIndex = 14;
            this.MaxVNumericUpDown.Value = new decimal( new int[] {
            1024,
            0,
            0,
            0} );
            this.MaxVNumericUpDown.ValueChanged += new System.EventHandler( this.curve_Changed );
            // 
            // positionLabel
            // 
            this.positionLabel.AutoSize = true;
            this.positionLabel.Location = new System.Drawing.Point( 220, 11 );
            this.positionLabel.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size( 20, 17 );
            this.positionLabel.TabIndex = 12;
            this.positionLabel.Text = "...";
            // 
            // stackCheckBox
            // 
            this.stackCheckBox.AutoSize = true;
            this.stackCheckBox.Location = new System.Drawing.Point( 303, 10 );
            this.stackCheckBox.Margin = new System.Windows.Forms.Padding( 4 );
            this.stackCheckBox.Name = "stackCheckBox";
            this.stackCheckBox.Size = new System.Drawing.Size( 103, 21 );
            this.stackCheckBox.TabIndex = 11;
            this.stackCheckBox.Text = "Show Stack";
            this.stackCheckBox.UseVisualStyleBackColor = true;
            this.stackCheckBox.Visible = false;
            this.stackCheckBox.CheckedChanged += new System.EventHandler( this.stackCheckBox_CheckedChanged );
            // 
            // noZoomRadioButton
            // 
            this.noZoomRadioButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.noZoomRadioButton.AutoSize = true;
            this.noZoomRadioButton.Checked = true;
            this.noZoomRadioButton.Location = new System.Drawing.Point( 859, 10 );
            this.noZoomRadioButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.noZoomRadioButton.Name = "noZoomRadioButton";
            this.noZoomRadioButton.Size = new System.Drawing.Size( 87, 21 );
            this.noZoomRadioButton.TabIndex = 6;
            this.noZoomRadioButton.TabStop = true;
            this.noZoomRadioButton.Text = "No Zoom";
            this.noZoomRadioButton.UseVisualStyleBackColor = true;
            this.noZoomRadioButton.CheckedChanged += new System.EventHandler( this.zoomRadioButton_CheckedChanged );
            // 
            // zoom1xRadioButton
            // 
            this.zoom1xRadioButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.zoom1xRadioButton.AutoSize = true;
            this.zoom1xRadioButton.Location = new System.Drawing.Point( 959, 10 );
            this.zoom1xRadioButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.zoom1xRadioButton.Name = "zoom1xRadioButton";
            this.zoom1xRadioButton.Size = new System.Drawing.Size( 43, 21 );
            this.zoom1xRadioButton.TabIndex = 7;
            this.zoom1xRadioButton.Text = "1x";
            this.zoom1xRadioButton.UseVisualStyleBackColor = true;
            this.zoom1xRadioButton.CheckedChanged += new System.EventHandler( this.zoomRadioButton_CheckedChanged );
            // 
            // zoom2xRadioButton
            // 
            this.zoom2xRadioButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.zoom2xRadioButton.AutoSize = true;
            this.zoom2xRadioButton.Location = new System.Drawing.Point( 1023, 10 );
            this.zoom2xRadioButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.zoom2xRadioButton.Name = "zoom2xRadioButton";
            this.zoom2xRadioButton.Size = new System.Drawing.Size( 43, 21 );
            this.zoom2xRadioButton.TabIndex = 8;
            this.zoom2xRadioButton.Text = "2x";
            this.zoom2xRadioButton.UseVisualStyleBackColor = true;
            this.zoom2xRadioButton.CheckedChanged += new System.EventHandler( this.zoomRadioButton_CheckedChanged );
            // 
            // zoom4xRadioButton
            // 
            this.zoom4xRadioButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.zoom4xRadioButton.AutoSize = true;
            this.zoom4xRadioButton.Location = new System.Drawing.Point( 1079, 10 );
            this.zoom4xRadioButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.zoom4xRadioButton.Name = "zoom4xRadioButton";
            this.zoom4xRadioButton.Size = new System.Drawing.Size( 43, 21 );
            this.zoom4xRadioButton.TabIndex = 8;
            this.zoom4xRadioButton.Text = "4x";
            this.zoom4xRadioButton.UseVisualStyleBackColor = true;
            this.zoom4xRadioButton.CheckedChanged += new System.EventHandler( this.zoomRadioButton_CheckedChanged );
            // 
            // zoomCFARadioButton
            // 
            this.zoomCFARadioButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.zoomCFARadioButton.AutoSize = true;
            this.zoomCFARadioButton.Location = new System.Drawing.Point( 1134, 10 );
            this.zoomCFARadioButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.zoomCFARadioButton.Name = "zoomCFARadioButton";
            this.zoomCFARadioButton.Size = new System.Drawing.Size( 55, 21 );
            this.zoomCFARadioButton.TabIndex = 10;
            this.zoomCFARadioButton.Text = "CFA";
            this.zoomCFARadioButton.UseVisualStyleBackColor = true;
            this.zoomCFARadioButton.CheckedChanged += new System.EventHandler( this.zoomRadioButton_CheckedChanged );
            // 
            // nextButton
            // 
            this.nextButton.Enabled = false;
            this.nextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextButton.Location = new System.Drawing.Point( 112, 4 );
            this.nextButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size( 100, 31 );
            this.nextButton.TabIndex = 4;
            this.nextButton.Text = ">";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler( this.nextButton_Click );
            // 
            // prevButton
            // 
            this.prevButton.Enabled = false;
            this.prevButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prevButton.Location = new System.Drawing.Point( 4, 4 );
            this.prevButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size( 100, 31 );
            this.prevButton.TabIndex = 3;
            this.prevButton.Text = "<";
            this.prevButton.UseVisualStyleBackColor = true;
            this.prevButton.Click += new System.EventHandler( this.prevButton_Click );
            // 
            // mainToolsPanel
            // 
            this.mainToolsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainToolsPanel.Controls.Add( this.exportToFITSButton );
            this.mainToolsPanel.Controls.Add( this.toolsMenuButton );
            this.mainToolsPanel.Controls.Add( this.closeButton );
            this.mainToolsPanel.Controls.Add( this.goFullScreenButton );
            this.mainToolsPanel.Controls.Add( this.exportToIRISButton );
            this.mainToolsPanel.Controls.Add( this.openFolderButton );
            this.mainToolsPanel.Controls.Add( this.refreshButton );
            this.mainToolsPanel.Controls.Add( this.stackButton );
            this.mainToolsPanel.Controls.Add( this.infoRadioButton );
            this.mainToolsPanel.Controls.Add( this.rawPreviewRadioButton );
            this.mainToolsPanel.Controls.Add( this.quickPreviewRadioButton );
            this.mainToolsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainToolsPanel.ForeColor = System.Drawing.Color.Gray;
            this.mainToolsPanel.Location = new System.Drawing.Point( 4, 4 );
            this.mainToolsPanel.Margin = new System.Windows.Forms.Padding( 4 );
            this.mainToolsPanel.Name = "mainToolsPanel";
            this.mainToolsPanel.Size = new System.Drawing.Size( 1617, 38 );
            this.mainToolsPanel.TabIndex = 4;
            // 
            // exportToFITSButton
            // 
            this.exportToFITSButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.exportToFITSButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportToFITSButton.Location = new System.Drawing.Point( 1295, 4 );
            this.exportToFITSButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.exportToFITSButton.Name = "exportToFITSButton";
            this.exportToFITSButton.Size = new System.Drawing.Size( 100, 28 );
            this.exportToFITSButton.TabIndex = 13;
            this.exportToFITSButton.Text = "To FITS";
            this.exportToFITSButton.UseVisualStyleBackColor = true;
            this.exportToFITSButton.Click += new System.EventHandler( this.exportToFITS_Click );
            // 
            // toolsMenuButton
            // 
            this.toolsMenuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toolsMenuButton.Location = new System.Drawing.Point( 602, 4 );
            this.toolsMenuButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.toolsMenuButton.Name = "toolsMenuButton";
            this.toolsMenuButton.Size = new System.Drawing.Size( 100, 28 );
            this.toolsMenuButton.TabIndex = 12;
            this.toolsMenuButton.Text = "Tools";
            this.toolsMenuButton.UseVisualStyleBackColor = true;
            this.toolsMenuButton.Click += new System.EventHandler( this.toolsMenu_Click );
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Location = new System.Drawing.Point( 1574, 4 );
            this.closeButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size( 37, 28 );
            this.closeButton.TabIndex = 12;
            this.closeButton.Text = "X";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler( this.close_Click );
            // 
            // goFullScreenButton
            // 
            this.goFullScreenButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.goFullScreenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.goFullScreenButton.Location = new System.Drawing.Point( 1533, 4 );
            this.goFullScreenButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.goFullScreenButton.Name = "goFullScreenButton";
            this.goFullScreenButton.Size = new System.Drawing.Size( 37, 28 );
            this.goFullScreenButton.TabIndex = 12;
            this.goFullScreenButton.Text = "FS";
            this.goFullScreenButton.UseVisualStyleBackColor = true;
            this.goFullScreenButton.Click += new System.EventHandler( this.goFullScreen_Click );
            // 
            // exportToIRISButton
            // 
            this.exportToIRISButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.exportToIRISButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportToIRISButton.Location = new System.Drawing.Point( 1403, 4 );
            this.exportToIRISButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.exportToIRISButton.Name = "exportToIRISButton";
            this.exportToIRISButton.Size = new System.Drawing.Size( 100, 28 );
            this.exportToIRISButton.TabIndex = 12;
            this.exportToIRISButton.Text = "To IRIS";
            this.exportToIRISButton.UseVisualStyleBackColor = true;
            this.exportToIRISButton.Click += new System.EventHandler( this.exportToIRIS_Click );
            // 
            // openFolderButton
            // 
            this.openFolderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openFolderButton.Location = new System.Drawing.Point( 3, 4 );
            this.openFolderButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size( 100, 28 );
            this.openFolderButton.TabIndex = 8;
            this.openFolderButton.Text = "Open Folder";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler( this.openFolder_Click );
            // 
            // refreshButton
            // 
            this.refreshButton.Enabled = false;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Location = new System.Drawing.Point( 107, 4 );
            this.refreshButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size( 100, 28 );
            this.refreshButton.TabIndex = 7;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler( this.refreshButton_Click );
            // 
            // stackButton
            // 
            this.stackButton.Enabled = false;
            this.stackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stackButton.Location = new System.Drawing.Point( 211, 4 );
            this.stackButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.stackButton.Name = "stackButton";
            this.stackButton.Size = new System.Drawing.Size( 100, 28 );
            this.stackButton.TabIndex = 2;
            this.stackButton.Text = "Stack";
            this.stackButton.UseVisualStyleBackColor = true;
            this.stackButton.Click += new System.EventHandler( this.createStack_Click );
            // 
            // infoRadioButton
            // 
            this.infoRadioButton.AutoSize = true;
            this.infoRadioButton.Location = new System.Drawing.Point( 525, 7 );
            this.infoRadioButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.infoRadioButton.Name = "infoRadioButton";
            this.infoRadioButton.Size = new System.Drawing.Size( 52, 21 );
            this.infoRadioButton.TabIndex = 6;
            this.infoRadioButton.TabStop = true;
            this.infoRadioButton.Text = "Info";
            this.infoRadioButton.UseVisualStyleBackColor = true;
            this.infoRadioButton.CheckedChanged += new System.EventHandler( this.viewMode_Click );
            // 
            // rawPreviewRadioButton
            // 
            this.rawPreviewRadioButton.AutoSize = true;
            this.rawPreviewRadioButton.Location = new System.Drawing.Point( 445, 7 );
            this.rawPreviewRadioButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.rawPreviewRadioButton.Name = "rawPreviewRadioButton";
            this.rawPreviewRadioButton.Size = new System.Drawing.Size( 56, 21 );
            this.rawPreviewRadioButton.TabIndex = 5;
            this.rawPreviewRadioButton.TabStop = true;
            this.rawPreviewRadioButton.Text = "Raw";
            this.rawPreviewRadioButton.UseVisualStyleBackColor = true;
            this.rawPreviewRadioButton.CheckedChanged += new System.EventHandler( this.viewMode_Click );
            // 
            // quickPreviewRadioButton
            // 
            this.quickPreviewRadioButton.AutoSize = true;
            this.quickPreviewRadioButton.Checked = true;
            this.quickPreviewRadioButton.Location = new System.Drawing.Point( 365, 7 );
            this.quickPreviewRadioButton.Margin = new System.Windows.Forms.Padding( 4 );
            this.quickPreviewRadioButton.Name = "quickPreviewRadioButton";
            this.quickPreviewRadioButton.Size = new System.Drawing.Size( 65, 21 );
            this.quickPreviewRadioButton.TabIndex = 4;
            this.quickPreviewRadioButton.TabStop = true;
            this.quickPreviewRadioButton.Text = "Quick";
            this.quickPreviewRadioButton.UseVisualStyleBackColor = true;
            this.quickPreviewRadioButton.CheckedChanged += new System.EventHandler( this.viewMode_Click );
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point( 4, 42 );
            this.splitContainer.Margin = new System.Windows.Forms.Padding( 4 );
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add( this.thumbnailsView );
            this.splitContainer.Panel1MinSize = 177;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add( this.fillPanel );
            this.splitContainer.Size = new System.Drawing.Size( 1617, 672 );
            this.splitContainer.SplitterDistance = 357;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 3;
            // 
            // toolsContextMenu
            // 
            this.toolsContextMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.buildDarksMenuItem} );
            this.toolsContextMenu.Name = "contextMenuStrip";
            this.toolsContextMenu.Size = new System.Drawing.Size( 154, 28 );
            // 
            // buildDarksMenuItem
            // 
            this.buildDarksMenuItem.Name = "buildDarksMenuItem";
            this.buildDarksMenuItem.Size = new System.Drawing.Size( 153, 24 );
            this.buildDarksMenuItem.Text = "Build Darks";
            this.buildDarksMenuItem.Click += new System.EventHandler( this.buildDarksMenuItem_Click );
            // 
            // thumbnailsContextMenu
            // 
            this.thumbnailsContextMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.createFlatMenuItem,
            this.darkMenuItem} );
            this.thumbnailsContextMenu.Name = "thumbnailsContextMenu";
            this.thumbnailsContextMenu.Size = new System.Drawing.Size( 118, 52 );
            // 
            // createFlatMenuItem
            // 
            this.createFlatMenuItem.Name = "createFlatMenuItem";
            this.createFlatMenuItem.Size = new System.Drawing.Size( 117, 24 );
            this.createFlatMenuItem.Text = "FLAT";
            this.createFlatMenuItem.Click += new System.EventHandler( this.createFlat_Click );
            // 
            // darkMenuItem
            // 
            this.darkMenuItem.Name = "darkMenuItem";
            this.darkMenuItem.Size = new System.Drawing.Size( 117, 24 );
            this.darkMenuItem.Text = "DARK";
            this.darkMenuItem.Click += new System.EventHandler( this.createDark_Click );
            // 
            // thumbnailsView
            // 
            this.thumbnailsView.AutoScroll = true;
            this.thumbnailsView.AutoSize = true;
            this.thumbnailsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thumbnailsView.Location = new System.Drawing.Point( 0, 0 );
            this.thumbnailsView.Name = "thumbnailsView";
            this.thumbnailsView.Size = new System.Drawing.Size( 357, 672 );
            this.thumbnailsView.TabIndex = 0;
            this.thumbnailsView.LoadThumbnailsCompleted += new System.EventHandler<System.EventArgs>( this.thumbnailsView_LoadThumbnailsCompleted );
            this.thumbnailsView.SelectionChanged += new System.EventHandler<System.EventArgs>( this.thumbnailsView_SelectionChanged );
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 8F, 16F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size( 1625, 716 );
            this.Controls.Add( this.splitContainer );
            this.Controls.Add( this.mainToolsPanel );
            this.ForeColor = System.Drawing.Color.Gray;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding( 4 );
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding( 4, 4, 4, 2 );
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AstroRaw Organizer for Sony NEX-5N";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler( this.MainForm_Shown );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.MainForm_FormClosing );
            this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.MainForm_KeyDown );
            ( (System.ComponentModel.ISupportInitialize) ( this.pictureBox ) ).EndInit();
            this.mainPictureContextMenu.ResumeLayout( false );
            this.fillPanel.ResumeLayout( false );
            this.pictureBoxPanel.ResumeLayout( false );
            this.stackPanel.ResumeLayout( false );
            this.stackHeaderPanel.ResumeLayout( false );
            this.stackToolsPanel.ResumeLayout( false );
            this.stackToolsPanel.PerformLayout();
            this.zoomPanel.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize) ( this.zoomPictureBox ) ).EndInit();
            this.zoomContextMenu.ResumeLayout( false );
            this.previewToolsPanel.ResumeLayout( false );
            this.previewToolsPanel.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize) ( this.waveletUpDown5 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.waveletUpDown4 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.waveletUpDown3 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.waveletUpDown2 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.waveletUpDown1 ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.satNumericUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.gammaNumericUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.KbNumericUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.KgNumericUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.BgBnumericUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.BgGnumericUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.BgRnumericUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.BgNumericUpDown ) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize) ( this.MaxVNumericUpDown ) ).EndInit();
            this.mainToolsPanel.ResumeLayout( false );
            this.mainToolsPanel.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout( false );
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout( false );
            this.splitContainer.ResumeLayout( false );
            this.toolsContextMenu.ResumeLayout( false );
            this.thumbnailsContextMenu.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Panel fillPanel;
        private System.Windows.Forms.Panel previewToolsPanel;
        private System.Windows.Forms.Panel mainToolsPanel;
        private System.Windows.Forms.Button stackButton;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.RadioButton rawPreviewRadioButton;
        private System.Windows.Forms.RadioButton quickPreviewRadioButton;
        private System.Windows.Forms.RadioButton infoRadioButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.Button exportToIRISButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.PictureBox zoomPictureBox;
        private System.Windows.Forms.RadioButton zoom4xRadioButton;
        private System.Windows.Forms.RadioButton zoom2xRadioButton;
        private System.Windows.Forms.RadioButton zoom1xRadioButton;
        private System.Windows.Forms.RadioButton noZoomRadioButton;
        private System.Windows.Forms.CheckBox stackSelectedCheckBox;
        private System.Windows.Forms.RadioButton zoomCFARadioButton;
        private System.Windows.Forms.ContextMenuStrip zoomContextMenu;
        private System.Windows.Forms.Panel stackPanel;
        private System.Windows.Forms.CheckBox stackCheckBox;
        private System.Windows.Forms.Label positionLabel;
        private System.Windows.Forms.Panel stackToolsPanel;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.FlowLayoutPanel stackFlowPanel;
        private System.Windows.Forms.Panel stackHeaderPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Panel zoomPanel;
        private System.Windows.Forms.ToolStripMenuItem imageInfoToolMenuItem;
        private System.Windows.Forms.Button exportToFITSButton;
        private ThumbnailsView thumbnailsView;
        private System.Windows.Forms.ToolStripMenuItem stackZoomToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown MaxVNumericUpDown;
        private System.Windows.Forms.NumericUpDown BgNumericUpDown;
        private System.Windows.Forms.NumericUpDown KbNumericUpDown;
        private System.Windows.Forms.NumericUpDown KgNumericUpDown;
        private System.Windows.Forms.NumericUpDown gammaNumericUpDown;
        private System.Windows.Forms.NumericUpDown satNumericUpDown;
        private System.Windows.Forms.Button goFullScreenButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button toolsMenuButton;
        private System.Windows.Forms.ContextMenuStrip toolsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem buildDarksMenuItem;
        private System.Windows.Forms.ContextMenuStrip mainPictureContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mainPictureSaveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomSaveAsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem calcStatisticsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button exitFullScreenButton;
        private System.Windows.Forms.Panel pictureBoxPanel;
        private System.Windows.Forms.ToolStripMenuItem fullScreenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waveletsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextWaveletMenuItem;
        private System.Windows.Forms.NumericUpDown waveletUpDown3;
        private System.Windows.Forms.NumericUpDown waveletUpDown2;
        private System.Windows.Forms.NumericUpDown waveletUpDown1;
        private System.Windows.Forms.NumericUpDown waveletUpDown4;
        private System.Windows.Forms.NumericUpDown waveletUpDown5;
        private System.Windows.Forms.ContextMenuStrip thumbnailsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem createFlatMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem bgMaskMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detectBGMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doStackMenuItem;
        private System.Windows.Forms.NumericUpDown BgRnumericUpDown;
        private System.Windows.Forms.NumericUpDown BgBnumericUpDown;
        private System.Windows.Forms.NumericUpDown BgGnumericUpDown;
        private System.Windows.Forms.ToolStripMenuItem darkMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doExperimentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alignOnCenterOfMassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveStackToCFAToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem saveStackTo16bitPNGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveStackTo160bitTIFF1ToolStripMenuItem;
    }
}

