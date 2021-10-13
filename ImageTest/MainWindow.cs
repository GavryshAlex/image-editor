using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Math;
namespace ImageTest
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenFille_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "jpg files (*.jpg)|*.jpg";
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog1.FileName;

                    Bitmap fileImage = new Bitmap(filePath);
                    pictureBox1.Image = fileImage;
                    CreateNewImage(fileImage);
                }
            }
            catch
            {
                MessageBox.Show("Error Reading of JPG-File!");
            }
        }
        private void CreateNewImage(Bitmap InputImage)
        {
            switch (comboBox1.SelectedIndex)
			{
                case 0: // invertion
                    this.Size = new Size(500, 250);
                    CreateNewImageInvertion(InputImage);
                    break;
                case 1: // change on const value
                    this.Size = new Size(500, 250);
                    CreateNewImageChangeOnConstValue(InputImage, 100, 0, 0);
                    break;
                case 2: // red
                    this.Size = new Size(500, 250);
                    CreateNewImageRed(InputImage);
                    break;
                case 3: // green
                    this.Size = new Size(500, 250);
                    CreateNewImageGreen(InputImage);
                    break;
                case 4: // blue
                    this.Size = new Size(500, 250);
                    CreateNewImageBlue(InputImage);
                    break;
                case 5: // new filter
                    this.Size = new Size(750, 250);
                    break;
            }
            
        }
        private void CreateNewImageInvertion(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    Color color_of_Pixel = InputImage.GetPixel(i, j);
                    Color New_color_of_Pixel = Color.FromArgb(255-color_of_Pixel.R,
                                                                255 - color_of_Pixel.G,
                                                                255 - color_of_Pixel.B);
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            pictureBox2.Image = OutputImage;
        }
        private void CreateNewImageChangeOnConstValue(Bitmap InputImage, int valueR, int valueG, int valueB)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    Color color_of_Pixel = InputImage.GetPixel(i, j);
                    Color New_color_of_Pixel = Color.FromArgb(Max(Min(color_of_Pixel.R + valueR,255),0),
                                                               Max(Min(color_of_Pixel.G + valueG,255),0),
                                                                Max(Min(color_of_Pixel.B + valueB, 255), 0));
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            pictureBox2.Image = OutputImage;
        }
        private void CreateNewImageRed(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    Color color_of_Pixel = InputImage.GetPixel(i, j);
                    Color New_color_of_Pixel = Color.FromArgb(color_of_Pixel.R,
                                                                0,
                                                                0);
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            pictureBox2.Image = OutputImage;
        }
        private void CreateNewImageGreen(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    Color color_of_Pixel = InputImage.GetPixel(i, j);
                    Color New_color_of_Pixel = Color.FromArgb(0,
                                                                color_of_Pixel.G,
                                                                0);
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            pictureBox2.Image = OutputImage;
        }
        private void CreateNewImageBlue(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    Color color_of_Pixel = InputImage.GetPixel(i, j);
                    Color New_color_of_Pixel = Color.FromArgb(0,
                                                                0,
                                                                color_of_Pixel.B);
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            pictureBox2.Image = OutputImage;
        }
	}
}
