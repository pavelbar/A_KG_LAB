using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Filters
{
    class матричный_фильтр_медианный: класс_MatrixFilter
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
int n;
int cR_, cB_, cG_;
int k = 0;
int rad = 2;
            n = (2 * rad + 1) * (2 * rad + 1);

int[] cR = new int[n+1];
int[] cB = new int[n+1];
int[] cG = new int[n+1];

for (int i = 0; i < n + 1; i++)
            {
                cR[i] = 0;
                cG[i] = 0;
                cB[i] = 0;
            }
for (int i = - rad; i < rad ; i++)
            {
for (int j = - rad; j < rad ; j++)
                {
int idX = Clamp(x + j, 0, sourceImage.Width - 1);
int idY = Clamp(y + i, 0, sourceImage.Height - 1);
                        System.Drawing.Color c = sourceImage.GetPixel(idX, idY);
                        cR[k] = System.Convert.ToInt32(c.R); 
                        cG[k] = System.Convert.ToInt32(c.G);
                        cB[k] = System.Convert.ToInt32(c.B);
                        k++;

                }
            }

            quickSort(cR, 0, n - 1);
            quickSort(cG, 0, n - 1);
            quickSort(cB, 0, n - 1);

int n_ = (int)(n / 2)+1;

            cR_ = cR[n_];
            cG_ = cG[n_];
            cB_ = cB[n_];

Color resultColor = Color.FromArgb(cR_, cG_, cB_);
return resultColor;
        }

static void quickSort(int[] a, int l, int r)
        {
int temp;
int x = a[l + (r - l) / 2];
int i = l;
int j = r;
while (i <= j)
            {
while (a[i] < x) i++;
while (a[j] > x) j--;
if (i <= j)
                {
                    temp = a[i];
                    a[i] = a[j];
                    a[j] = temp;
                    i++;
                    j--;
                }
            }
if (i < r)
                quickSort(a, i, r);

if (l < j)
                quickSort(a, l, j);
        }
    }

    }

