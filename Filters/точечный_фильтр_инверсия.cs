using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Filters
{
    class точечный_фильтр_инверсия: класс_FiltersBaranov 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
    {
        Color sourceColor = sourceImage.GetPixel(x,y);
        Color resultColor = Color.FromArgb(255-sourceColor.R,
                                           255-sourceColor.G, 
                                           255-sourceColor.B);
        return resultColor;    
    }
    }
}
