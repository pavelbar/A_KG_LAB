using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Filters
{
    public partial class Form1 : Form
    {
        bool[,] Mask = new bool[,] { { false, true, false },
                { true, true, true },
                { false, true, false } };
        Bitmap imageS;
        Bitmap image;
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files | *.png; *.jpg; *.bmp | All Files (*.*) | *.*";
            if (dialog.ShowDialog()==DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
            }
            pictureBox1.Image = image;
            imageS = image;
            pictureBox1.Refresh();
        }

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            точечный_фильтр_инверсия filter = new точечный_фильтр_инверсия();     
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((класс_FiltersBaranov)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                image = newImage;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            класс_FiltersBaranov filter = new матричный_фильтр_размытие();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void гауссToolStripMenuItem_Click(object sender, EventArgs e)
        {
            класс_FiltersBaranov filter = new матричный_фильтр_гаусс();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void чернобелоеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            класс_FiltersBaranov filter = new точечный_фильтр_чб();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void серыйМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            класс_FiltersBaranov filter = new точечный_фильтр_серый_мир(image);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void линейноеРастяжениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            класс_FiltersBaranov filter = new матричный_фильтр_линейное_растяжение(image);
            backgroundWorker1.RunWorkerAsync(filter);
        }
        private void медианный1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            класс_FiltersBaranov filter = new матричный_фильтр_медианный();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmpSave = (Bitmap)pictureBox1.Image;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "jpg";
            sfd.Filter = "Image files|*.png;*.jpg;*.bmp|All files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
                bmpSave.Save(sfd.FileName);
        }

        private void расширениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap res = new Bitmap(image);

                int width = image.Width;
                int height = image.Height;

                int[,] allPixR = new int[width, height];
                int[,] allPixG = new int[width, height];
                int[,] allPixB = new int[width, height];
                int[,] resPixR = new int[width, height];
                int[,] resPixG = new int[width, height];
                int[,] resPixB = new int[width, height];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        allPixR[i, j] = image.GetPixel(i, j).R;
                        allPixG[i, j] = image.GetPixel(i, j).G;
                        allPixB[i, j] = image.GetPixel(i, j).B;
                    }
                }

                // Применяем расширение
                Dilation(allPixR, allPixG, allPixB, Mask, resPixR, resPixG, resPixB, image.Width, image.Height);


                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                        res.SetPixel(i, j, Color.FromArgb(resPixR[i, j], resPixG[i, j], resPixB[i, j]));
                }

                pictureBox1.Image = res;
                pictureBox1.Refresh();
            }

        }

        // Морфологическое расширение
        private void Dilation(int[,] srcR, int[,] srcG, int[,] srcB, bool[,] mask, int[,] ResR, int[,] ResG, int[,] ResB, int width, int height)
        {
            int MW = mask.GetLength(0);
            int MH = mask.GetLength(1);

            for (int y = MH / 2; y < height - MH / 2; y++)
            {
                for (int x = MW / 2; x < width - MW / 2; x++)
                {
                    int maxR = 0;
                    int maxG = 0;
                    int maxB = 0;

                    for (int j = -MH / 2; j <= MH / 2; j++)
                    {
                        for (int i = -MW / 2; i <= MW / 2; i++)
                        {
                            if ((i >= 0) && (j >= 0))
                            {
                                if ((mask[i, j]) && (srcR[x + i, y + j] > maxR))
                                    maxR = srcR[x + i, y + j];
                                if ((mask[i, j]) && (srcG[x + i, y + j] > maxG))
                                    maxG = srcG[x + i, y + j];
                                if ((mask[i, j]) && (srcB[x + i, y + j] > maxB))
                                    maxB = srcB[x + i, y + j];
                            }

                        }
                    }

                    ResR[x, y] = maxR;
                    ResG[x, y] = maxG;
                    ResB[x, y] = maxB;
                }
            }
        }

        private void сужениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap res = new Bitmap(image);

                int width = image.Width;
                int height = image.Height;

                int[,] allPixR = new int[width, height];
                int[,] allPixG = new int[width, height];
                int[,] allPixB = new int[width, height];
                int[,] resPixR = new int[width, height];
                int[,] resPixG = new int[width, height];
                int[,] resPixB = new int[width, height];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        allPixR[i, j] = image.GetPixel(i, j).R;
                        allPixG[i, j] = image.GetPixel(i, j).G;
                        allPixB[i, j] = image.GetPixel(i, j).B;
                    }
                }
                Erosion(allPixR, allPixG, allPixB, Mask, resPixR, resPixG, resPixB, image.Width, image.Height);

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                        res.SetPixel(i, j, Color.FromArgb(resPixR[i, j], resPixG[i, j], resPixB[i, j]));
                }

                pictureBox1.Image = res;
                pictureBox1.Refresh();
            }
        }

        // Морфологическое сужение
        void Erosion(int[,] srcR, int[,] srcG, int[,] srcB, bool[,] mask, int[,] ResR, int[,] ResG, int[,] ResB, int width, int height)
        {
            int MW = mask.GetLength(0);
            int MH = mask.GetLength(1);

            for (int y = MH / 2; y < height - MH / 2; y++)
            {
                for (int x = MW / 2; x < width - MW / 2; x++)
                {
                    int minR = 255;
                    int minG = 255;
                    int minB = 255;
                    for (int j = -MH / 2; j <= MH / 2; j++)
                    {
                        for (int i = -MW / 2; i <= MW / 2; i++)
                        {
                            if ((i >= 0) && (j >= 0))
                            {
                                if ((mask[i, j]) && (srcR[x + i, y + j] < minR))
                                    minR = srcR[x + i, y + j];
                                if ((mask[i, j]) && (srcG[x + i, y + j] < minG))
                                    minG = srcG[x + i, y + j];
                                if ((mask[i, j]) && (srcB[x + i, y + j] < minB))
                                    minB = srcB[x + i, y + j];
                            }

                        }
                    }

                    ResR[x, y] = minR;
                    ResG[x, y] = minG;
                    ResB[x, y] = minB;
                }
            }
        }

        private void открытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap res = new Bitmap(image);

                int width = image.Width;
                int height = image.Height;

                int[,] allPixR = new int[width, height];
                int[,] allPixG = new int[width, height];
                int[,] allPixB = new int[width, height];
                int[,] resPixR = new int[width, height];
                int[,] resPixG = new int[width, height];
                int[,] resPixB = new int[width, height];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        allPixR[i, j] = image.GetPixel(i, j).R;
                        allPixG[i, j] = image.GetPixel(i, j).G;
                        allPixB[i, j] = image.GetPixel(i, j).B;
                    }
                }
                Erosion(allPixR, allPixG, allPixB, Mask, resPixR, resPixG, resPixB, image.Width, image.Height);
                Dilation(resPixR, resPixG, resPixB, Mask, allPixR, allPixG, allPixB, image.Width, image.Height);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                        res.SetPixel(i, j, Color.FromArgb(allPixR[i, j], allPixG[i, j], allPixB[i, j]));
                }
                pictureBox1.Image = res;
                pictureBox1.Refresh();
            }
        }

        private void закрытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap res = new Bitmap(image);

                int width = image.Width;
                int height = image.Height;

                int[,] allPixR = new int[width, height];
                int[,] allPixG = new int[width, height];
                int[,] allPixB = new int[width, height];
                int[,] resPixR = new int[width, height];
                int[,] resPixG = new int[width, height];
                int[,] resPixB = new int[width, height];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        allPixR[i, j] = image.GetPixel(i, j).R;
                        allPixG[i, j] = image.GetPixel(i, j).G;
                        allPixB[i, j] = image.GetPixel(i, j).B;
                    }
                }

                Dilation(allPixR, allPixG, allPixB, Mask, resPixR, resPixG, resPixB, image.Width, image.Height);

                Erosion(resPixR, resPixG, resPixB, Mask, allPixR, allPixG, allPixB, image.Width, image.Height);

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                        res.SetPixel(i, j, Color.FromArgb(allPixR[i, j], allPixG[i, j], allPixB[i, j]));
                }

                pictureBox1.Image = res;
                pictureBox1.Refresh();
            }

        }

        private void gradToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap res = new Bitmap(image);
                Color sourceColor;

                int width = image.Width;
                int height = image.Height;

                int[,] allPixR = new int[width, height];
                int[,] allPixG = new int[width, height];
                int[,] allPixB = new int[width, height];
                int[,] resPixR = new int[width, height];
                int[,] resPixG = new int[width, height];
                int[,] resPixB = new int[width, height];
                int[,] res2PixR = new int[width, height];
                int[,] res2PixG = new int[width, height];
                int[,] res2PixB = new int[width, height];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        allPixR[i, j] = image.GetPixel(i, j).R;
                        allPixG[i, j] = image.GetPixel(i, j).G;
                        allPixB[i, j] = image.GetPixel(i, j).B;
                    }
                }

                Erosion(allPixR, allPixG, allPixB, Mask, resPixR, resPixG, resPixB, image.Width, image.Height);

                Dilation(resPixR, resPixG, resPixB, Mask, allPixR, allPixG, allPixB, image.Width, image.Height);


                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        sourceColor = image.GetPixel(i, j);
                        int aR = sourceColor.R - allPixR[i, j];
                        int aG = sourceColor.G - allPixG[i, j];
                        int aB = sourceColor.B - allPixB[i, j];
                        res.SetPixel(i, j, Color.FromArgb(Clamp(aR, 0, 255), Clamp(aG, 0, 255), Clamp(aB, 0, 255)));
                    }
                }


                pictureBox1.Image = res;
                pictureBox1.Refresh();
            }

        }
        private void отменаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = imageS;
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void морфологияToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
