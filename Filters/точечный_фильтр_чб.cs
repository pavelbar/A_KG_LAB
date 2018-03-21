using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Filters
{
    class точечный_фильтр_чб:класс_FiltersBaranov 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            float intensity = 0.36f * sourceColor.R + 0.53f * sourceColor.G + 0.11f * sourceColor.B;
            Color resultColor = Color.FromArgb((int) (intensity),
                                               (int) (intensity),
                                               (int)(intensity));
            return resultColor;    
        }

    }
}
