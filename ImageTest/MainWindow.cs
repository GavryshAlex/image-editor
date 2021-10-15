using System;
using System.Drawing;
using static System.Drawing.Color;
using System.Windows.Forms;
using static System.Math;
namespace ImageTest
{
    public partial class MainWindow : Form
    {
        Bitmap InputImage1, InputImage2;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenFile1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "jpg files (*.jpg)|*.jpg";
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog1.FileName;

                    InputImage1 = new Bitmap(filePath);
                    pictureBox1.Image = InputImage1;
                    if (!(comboBox1.SelectedIndex == 11 || comboBox1.SelectedIndex == 12))
                        CreateNewImage();
                }
            }
            catch
            {
                MessageBox.Show("Error Reading of JPG-File!");
            }
        }
        private void OpenFile2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "jpg files (*.jpg)|*.jpg";
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog1.FileName;

                    InputImage2 = new Bitmap(filePath);
                    pictureBox2.Image = InputImage2;
                    CreateNewImage();
                }
            }
            catch
            {
                MessageBox.Show("Error Reading of JPG-File!");
            }
        }
        private void CreateNewImage()
        {
            switch (comboBox1.SelectedIndex)
			{
                case 0: // invertion
                    //this.Size = new Size(500, 250);
                    ImageInvertion(InputImage1);
                    break;
                case 1: // change on const value
                    //this.Size = new Size(500, 250);
                    ImageChangeOnConstValue(InputImage1, 100, 0, 0);
                    break;
                case 2: // red
                    //this.Size = new Size(500, 250);
                    ImageRed(InputImage1);
                    break;
                case 3: // green
                    //this.Size = new Size(500, 250);
                    ImageGreen(InputImage1);
                    break;
                case 4: // blue
                    //this.Size = new Size(500, 250);
                    ImageBlue(InputImage1);
                    break;
                case 5: // filter blur
                    //this.Size = new Size(500, 250);
                    ImageBlur(InputImage1);
                    break;
                case 6: // filter enhance
                    //this.Size = new Size(750, 250);
                    ImageEnhance(InputImage1);
                    break;
                case 7:
                    ImageMedian(InputImage1);
                    break;
                case 8:
                    ImageErosion(InputImage1);
                    break;
                case 9:
                    ImageNaroshuvannya(InputImage1);
                    break;
                case 10:
                    filterSobel(InputImage1);
                    break;
                case 11:
                    ImageMerge(InputImage1, InputImage2);
                    break;
                case 12:
                    AddWatersign(InputImage1, InputImage2);
                    /*pictureBox1.Image = pictureBox3.Image;
                    InputImage1 = new Bitmap(pictureBox1.Image);
                    AddWatersign(InputImage1, InputImage2);*/
                    break;
                case 13:
                    AddWatersign(InputImage1, InputImage2); // removes watersign
                    break;

            }
            
        }
        private int[,] BitmapToBinary(Bitmap btm)
		{
            int m = btm.Width, n = btm.Height;
            int i, j;
            int[,] CGrey = new int[m, n];
            int pixelGrey;
            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    pixelGrey = (int)Round(((int)btm.GetPixel(i, j).R + (int)btm.GetPixel(i, j).G + (int)btm.GetPixel(i, j).B) / 3.0);
                    if (pixelGrey > 127)
                        CGrey[i, j] = 1;
                    else
                        CGrey[i, j] = 0;
                }
            return CGrey;
		}
        private void AddWatersign(Bitmap image, Bitmap watersign)
        {
            EqualResolution(ref image, ref watersign);
            int m = image.Width, n = image.Height;
            pictureBox1.Image = cropImage(image, new Rectangle(0, 0, m, n));
            pictureBox2.Image = cropImage(watersign, new Rectangle(0, 0, m, n));
            Bitmap OutputImage = new Bitmap(m, n);
            int[,] WGrey = BitmapToBinary(watersign);
            //int[,] CGrey = new int[m, n];
            for (int i = 0; i < m; i ++)
			{
                for (int j = 0; j < n; j ++)
				{
                    Color currentPixel = image.GetPixel(i, j);
                    int r, g, b, temp;
                    r = currentPixel.R;
                    g = currentPixel.G;
                    b = currentPixel.B;
                    //int tempBin = Convert.ToInt32(Convert.ToString(temp, 2));
                    if (WGrey[i,j] == 0)
                        b = b ^ 128;
                    Color newPixel = FromArgb((byte) r, (byte) g, (byte) b);
                    OutputImage.SetPixel(i,j, newPixel);
				}
			}

            pictureBox3.Image = OutputImage;

        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 11 || comboBox1.SelectedIndex == 12)
			{
                OpenFile2.Visible = true;
                pictureBox3.Visible = true;
                OpenFile1.Text = "Open File1 from PC";
			}
			else
			{
                OpenFile1.Text = "Open File from PC";
                OpenFile2.Visible = false;
                pictureBox3.Visible = false;
            }
        }



        private Image cropImage(Bitmap img, Rectangle cropArea) // обрезка изображения
        {
            return img.Clone(cropArea, img.PixelFormat);
        }
        private void EqualResolution(ref Bitmap img1, ref Bitmap img2) // сведение к одинаковому разрешению
		{
            int m = img1.Width, n = img1.Height;
            img1 = new Bitmap(cropImage(img1, new Rectangle(0, 0, Min(m, img2.Width), Min(n, img2.Height))));
            img2 = new Bitmap(cropImage(img2, new Rectangle(0, 0, Min(m, img2.Width), Min(n, img2.Height))));
        }
        private void ImageMerge(Bitmap img1, Bitmap img2)
		{
            EqualResolution(ref img1, ref img2);
            int m = img1.Width, n = img1.Height;
            pictureBox1.Image = cropImage(img1, new Rectangle(0, 0, m, n));
            pictureBox2.Image = cropImage(img2, new Rectangle(0, 0, m, n));
            Bitmap OutputImage = new Bitmap(m, n);
            double a = 0.3;
            for(int i = 0; i < m; i ++)
			{
                for (int j = 0; j < n; j ++)
				{
                    Color firstPixel = img1.GetPixel(i,j);
                    Color secondPixel = img2.GetPixel(i, j);

                    Color currentPixel = FromArgb((byte)Round(a*firstPixel.R+(1-a)*secondPixel.R),
                        (byte)Round(a * firstPixel.G + (1 - a) * secondPixel.G),
                        (byte)Round(a * firstPixel.B + (1 - a) * secondPixel.B));
                    OutputImage.SetPixel(i, j, currentPixel);
				}
			}
            pictureBox3.Image = OutputImage;

        }
        private void ImageInvertion(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    Color color_of_Pixel = InputImage.GetPixel(i, j);
                    Color New_color_of_Pixel = FromArgb(255-color_of_Pixel.R,
                                                                255 - color_of_Pixel.G,
                                                                255 - color_of_Pixel.B);
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            pictureBox2.Image = OutputImage;
        }
        private void ImageChangeOnConstValue(Bitmap InputImage, int valueR, int valueG, int valueB)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    Color color_of_Pixel = InputImage.GetPixel(i, j);
                    Color New_color_of_Pixel = FromArgb(Max(Min(color_of_Pixel.R + valueR,255),0),
                                                               Max(Min(color_of_Pixel.G + valueG,255),0),
                                                                Max(Min(color_of_Pixel.B + valueB, 255), 0));
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            pictureBox2.Image = OutputImage;
        }
        private void ImageRed(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    Color color_of_Pixel = InputImage.GetPixel(i, j);
                    Color New_color_of_Pixel = FromArgb(color_of_Pixel.R,
                                                                0,
                                                                0);
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            pictureBox2.Image = OutputImage;
        }
        private void ImageGreen(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    Color color_of_Pixel = InputImage.GetPixel(i, j);
                    Color New_color_of_Pixel = FromArgb(0,
                                                                color_of_Pixel.G,
                                                                0);
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            pictureBox2.Image = OutputImage;
        }
        private void ImageBlue(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    Color color_of_Pixel = InputImage.GetPixel(i, j);
                    Color New_color_of_Pixel = FromArgb(0,
                                                                0,
                                                                color_of_Pixel.B);
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            pictureBox2.Image = OutputImage;
        }
        private void ImageBlur(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            double[,] mask = {
               {0.000789, 0.006581, 0.013347, 0.006581, 0.000789},
               {0.006581, 0.054901, 0.111345, 0.054901, 0.006581},
               {0.013347, 0.111345, 0.225821, 0.111345, 0.013347},
               {0.006581, 0.054901, 0.111345, 0.054901, 0.006581},
               {0.000789, 0.006581, 0.013347, 0.006581, 0.000789}
            };
            /*double div = 1.0 / 6;
            for (int k = 0; k < 3; k++)
            {
                for (int l = 0; l < 3; l++)
                {
                    mask[k, l] *= div;
                }
            }*/
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 2; i < m - 2; i++)
            {
                for (j = 2; j < n - 2; j++)
                {
                    Color New_color_of_Pixel;
                    byte[] newColor = new byte[3] { 0, 0, 0 }; // rgb
                    for (int k = 0; k < 5; k++)
                    {
                        for (int l = 0; l < 5; l++)
                        {
                            newColor[0] += (byte)Round((mask[k, l] * (int)InputImage.GetPixel(i + k - 2, j + l - 2).R));
                            newColor[1] += (byte)Round((mask[k, l] * (int)InputImage.GetPixel(i + k - 2, j + l - 2).G));
                            newColor[2] += (byte)Round((mask[k, l] * (int)InputImage.GetPixel(i + k - 2, j + l - 2).B));
                        }
                    }

                    New_color_of_Pixel = FromArgb(Max(Min(newColor[0], (byte)255), (byte)0),
                        Max(Min(newColor[1], (byte)255), (byte)0),
                        Max(Min(newColor[2], (byte)255), (byte)0));
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            }
            pictureBox2.Image = OutputImage;
        }
        private void ImageEnhance(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            double[,] mask = {
               {-1, -1, -1},
               {-1,  9, -1},
               {-1, -1, -1}
            };
            Bitmap OutputImage = new Bitmap(m,n);

            for (i = 1; i < m-1; i++)
            {
                for (j = 1; j < n-1; j++)
                {
                    Color New_color_of_Pixel;
                    double r = 0, g = 0, b = 0; // rgb
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            r += Round(mask[k, l] * InputImage.GetPixel(i + k - 1, j + l - 1).R);
                            g += Round(mask[k, l] * InputImage.GetPixel(i + k - 1, j + l - 1).G);
                            b += Round(mask[k, l] * InputImage.GetPixel(i + k - 1, j + l - 1).B);
                        }
                    }
                    New_color_of_Pixel = FromArgb((byte)Max(Min(r,255),0),
                        (byte)Max(Min(g, 255), 0),
                        (byte)Max(Min(b, 255), 0));
                    
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            }
            pictureBox2.Image = OutputImage;
        }
        private void ImageMedian(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            Bitmap OutputImage = new Bitmap(m, n);
            byte[] arrayR = new byte[9], arrayG = new byte[9], arrayB = new byte[9];
            for (i = 1; i < m - 1; i++)
            {
                for (j = 1; j < n - 1; j++)
                {
                    Color New_color_of_Pixel;
                    int index = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            arrayR[index] = InputImage.GetPixel(i + k - 1, j + l - 1).R;
                            arrayG[index] = InputImage.GetPixel(i + k - 1, j + l - 1).G;
                            arrayB[index++] = InputImage.GetPixel(i + k - 1, j + l - 1).B;
                        }
                    }
                    Array.Sort(arrayR);
                    Array.Sort(arrayG);
                    Array.Sort(arrayB);

                    New_color_of_Pixel = FromArgb(arrayR[4], arrayG[4], arrayB[4]);

                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            }
            pictureBox2.Image = OutputImage;
        }
        private void ImageErosion(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            byte[,] mask = {
               {0, 0, 1, 0, 0},
               {0, 1, 1, 1, 1},
               {1, 1, 1, 1, 1},
               {0, 1, 1, 1, 1},
               {0, 0, 1, 0, 0}
            };
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 2; i < m - 2; i++)
            {
                for (j = 2; j < n - 2; j++)
                {
                    Color New_color_of_Pixel;
                    byte r = 255, g = 255, b = 255; // rgb
                    for (int k = 0; k < 5; k++)
                    {
                        for (int l = 0; l < 5; l++)
                        {
                            if (mask[k,l] != 0)
							{
                                r = (byte)Min(mask[k, l] * InputImage.GetPixel(i + k - 2, j + l - 2).R, r);
                                g = (byte)Min(mask[k, l] * InputImage.GetPixel(i + k - 2, j + l - 2).G, g);
                                b = (byte)Min(mask[k, l] * InputImage.GetPixel(i + k - 2, j + l - 2).B, b);
							}
                            
                        }
                    }

                    New_color_of_Pixel = FromArgb(r,g,b);
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            }
            pictureBox2.Image = OutputImage;
        }
        private void ImageNaroshuvannya(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            byte[,] mask = {
               {0, 0, 1, 0, 0},
               {0, 1, 1, 1, 1},
               {1, 1, 1, 1, 1},
               {0, 1, 1, 1, 1},
               {0, 0, 1, 0, 0}
            };
            Bitmap OutputImage = new Bitmap(m, n);

            for (i = 2; i < m - 2; i++)
            {
                for (j = 2; j < n - 2; j++)
                {
                    Color New_color_of_Pixel;
                    byte r = 0, g = 0, b = 0; // rgb
                    for (int k = 0; k < 5; k++)
                    {
                        for (int l = 0; l < 5; l++)
                        {
                            r = (byte)Max(mask[k, l] * InputImage.GetPixel(i + k - 2, j + l - 2).R, r);
                            g = (byte)Max(mask[k, l] * InputImage.GetPixel(i + k - 2, j + l - 2).G, g);
                            b = (byte)Max(mask[k, l] * InputImage.GetPixel(i + k - 2, j + l - 2).B, b);
                        }
                    }

                    New_color_of_Pixel = FromArgb(r, g, b);
                    OutputImage.SetPixel(i, j, New_color_of_Pixel);
                }
            }
            pictureBox2.Image = OutputImage;
        }

		private void filterSobel(Bitmap InputImage)
        {
            int m = InputImage.Width, n = InputImage.Height;
            int i, j;
            int[,] Gx = {
               {-1, -2, -1},
               {0, 0, 0},
               {1, 2, 1}
            };
            int[,] Gy = {
               {-1, 0, 1},
               {-2, 0, 2},
               {-1, 0, 1}
            };
            Bitmap OutputImage = new Bitmap(m, n);
            int[,] CGrey = new int[m, n];
            for (i = 0; i < m; i ++)
                for (j = 0; j < n; j ++)
                    CGrey[i, j] = (int)Round(((int)InputImage.GetPixel(i, j).R + (int)InputImage.GetPixel(i, j).G + (int)InputImage.GetPixel(i, j).B) / 3.0);
            int[,] CSobel = new int[m, n];
            for (i = 2; i < m - 1; i++)
            {
                for (j = 2; j < n - 1; j++)
                {
                    int X = 0, Y = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            X += Gx[k,l]*CGrey[i + k - 1, j + l - 1];
                            Y += Gy[k, l] * CGrey[i + k - 1, j + l - 1];
                        }
                    }
                    CSobel[i,j] = (int)Round(Sqrt(X * X + Y * Y));
                    CSobel[i, j] = Min(Max(CSobel[i, j], 0), 255);
                    Color currentPixel = FromArgb(CSobel[i,j], CSobel[i, j], CSobel[i, j]);
                    OutputImage.SetPixel(i, j, currentPixel);
                }
            }
            



            pictureBox2.Image = OutputImage;
        }

		
	}
}
