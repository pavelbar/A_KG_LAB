using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Filters
{
    class точечный_фильтр_серый_мир : класс_FiltersBaranov
    {
        public точечный_фильтр_серый_мир(Bitmap img)
        {
            SumR = 0;
            SumG = 0;
            SumB = 0;
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color sourceColor = img.GetPixel(i, j);
                    SumR = SumR + sourceColor.R;
                    SumG = SumG + sourceColor.G;
                    SumB = SumB + sourceColor.B;
                }
            }
        }
        protected int SumR = 0;
        protected int SumG = 0;
        protected int SumB = 0;
        protected точечный_фильтр_серый_мир() { }
        public точечный_фильтр_серый_мир(int SumR, int SumG, int SumB)
        {
            this.SumR = SumR;
            this.SumG = SumG;
            this.SumB = SumB;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int N = sourceImage.Height * sourceImage.Width;
            Color sourceColor = sourceImage.GetPixel(x, y);
            float R_ = SumR / N;
            float G_ = SumG / N;
            float B_ = SumB / N;
            float Avg = (R_ + G_ + B_) / 3;
            int Rs = (int)(sourceColor.R * Avg / R_);
            int Gs = (int)(sourceColor.G * Avg / G_);
            int Bs = (int)(sourceColor.B * Avg / B_);
            Color resultColor = Color.FromArgb(Clamp(Rs, 0, 255), Clamp(Gs, 0, 255), Clamp(Bs, 0, 255));
            return resultColor;
        }

    }
}
