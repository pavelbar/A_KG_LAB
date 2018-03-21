using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Filters
{
    class матричный_фильтр_линейное_растяжение : класс_MatrixFilter
    {
        public матричный_фильтр_линейное_растяжение(Bitmap img)
        {
            MinR = 255;
            MinG = 255;
            MinB = 255;
            MaxR = 0;
            MaxG = 0;
            MaxB = 0;
            for (int i = 0; i < img.Width; i++)            
                for (int j = 0; j < img.Height; j++)
            {
            Color sourceColor = img.GetPixel(i, j);
            if (sourceColor.R > MaxR)
                    {
                        MaxR = sourceColor.R;
                    }
            if (sourceColor.G > MaxG)
                    {
                        MaxG = sourceColor.G;
                    }
            if (sourceColor.B > MaxB)
                    {
                        MaxB = sourceColor.B;
                    }
            if (sourceColor.R < MinR)
                    {
                        MinR = sourceColor.R;
                    }
            if (sourceColor.G < MinG)
                    {
                        MinG = sourceColor.G;
                    }
            if (sourceColor.B < MinB)
                    {
                        MinB = sourceColor.B;
                    }
            }            
        }
        protected int MinR=0;
        protected int MinG=0;
        protected int MinB=0;
        protected int MaxR = 0;
        protected int MaxG = 0;
        protected int MaxB = 0;
        protected матричный_фильтр_линейное_растяжение() { }
        public матричный_фильтр_линейное_растяжение(int MinR, int MinG, int MinB, int MaxR,int MaxG,int MaxB)
        {
            this.MinR = MinR;
            this.MinG = MinG;
            this.MinB = MinB;
            this.MaxR = MaxR;
            this.MaxG = MaxG;
            this.MaxB = MaxB;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            double I = 0.2125 * sourceColor.R + 0.7154 * sourceColor.G + 0.0721 * sourceColor.B;
            int resultR = (int)(sourceColor.R - MinR) * (255 / (MaxR - MinR));
            int resultG = (int)(sourceColor.G - MinG) * (255 / (MaxG - MinG));
            int resultB = (int)(sourceColor.B - MinB) * (255 / (MaxB - MinB));
            Color resultColor = Color.FromArgb(Clamp(resultR, 0, 255), Clamp(resultG, 0, 255), Clamp(resultB, 0, 255));
            return resultColor;
        }

    }
}
