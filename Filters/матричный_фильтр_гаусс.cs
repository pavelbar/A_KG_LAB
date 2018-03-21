﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filters
{
    class матричный_фильтр_гаусс:класс_MatrixFilter 
    {
        public void createGaussianKernel(int radius, float sigma)
        {
            int size = 2 * radius + 1;
            kernel = new float[size, size];
            float norm = 0;
            for(int i=-radius; i<=radius;i++)
                for(int j=-radius; j<=radius; j++)
                {
                    kernel[i + radius,j+radius] = (float)(Math.Exp(-(i * i + j * j) / (sigma * sigma)));
                    norm += kernel[i + radius, j + radius];
                }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] /= norm;
        }
        public матричный_фильтр_гаусс()
        {
            createGaussianKernel(3, 1);
        }
    }
}
