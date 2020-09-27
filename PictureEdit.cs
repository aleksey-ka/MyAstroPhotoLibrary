using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AstroPhoto.LibRaw;

namespace MyAstroPhotoLibrary
{
    internal partial class PictureEdit : Form
    {
        RgbImage stackResult;
        string filePath;

        const int maxV = 255;
        const int Kr = 255;
        const int Kg = 135;
        const int Kb = 205;

        public PictureEdit( RgbImage _stackResult, int count, string _filePath )
        {
            InitializeComponent();

            stackResult = _stackResult;
            filePath = _filePath;

            minRUpDown.Value = 0;
            maxRUpDown.Value = ( 255 * 255 * count ) / Kr;

            minGUpDown.Value = 0;
            maxGUpDown.Value = ( 255 * 255 * count ) / Kg;
            
            minBUpDown.Value = 0;
            maxBUpDown.Value = ( 255 * 255 * count ) / Kb;

            //stackResult.Divide( count );

            //pictureBox.Image = stackResult.RenderBitmap();
        }

        private void render_Click( object sender, EventArgs e )
        {
            /*pictureBox.Image.Dispose();

            stackResult.MinR = (int)minRUpDown.Value;
            stackResult.MaxR = (int)maxRUpDown.Value;

            stackResult.MinG = (int) minGUpDown.Value;
            stackResult.MaxG = (int) maxGUpDown.Value;

            stackResult.MinB = (int) minBUpDown.Value;
            stackResult.MaxB = (int) maxBUpDown.Value;
            
            pictureBox.Image = stackResult.Render();*/
        }

        private void save_Click( object sender, EventArgs e )
        {
            pictureBox.Image.Save( filePath, System.Drawing.Imaging.ImageFormat.Bmp );
        }

        private void autoLevelsButton_Click( object sender, EventArgs e )
        {
            /*stackResult.Autolevels( 
                double.Parse( lowTextBox.Text ), double.Parse( hiTextBox.Text )  );

            minRUpDown.Value = stackResult.MinR;
            maxRUpDown.Value = stackResult.MaxR;

            minGUpDown.Value = stackResult.MinG;
            maxGUpDown.Value = stackResult.MaxG;

            minBUpDown.Value = stackResult.MinB;
            maxBUpDown.Value = stackResult.MaxB;

            pictureBox.Image = stackResult.Render();*/
        }
    }
}
