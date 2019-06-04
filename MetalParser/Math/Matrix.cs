using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalParser.Math
{
    class Matrix
    {
        public Double[,] items;
        public int width;
        public int height;

        public Matrix()
        {
            
        }

        public Matrix Transpose()
        {
            Matrix transposedMatrix = null;

            int width = items.GetLength(0);
            int height = items.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    transposedMatrix.items[i, j] = items[j, i];
                }
            }

            return transposedMatrix;
        }

        public Matrix MultiplyRight(Matrix rightMatrix)
        {
            Matrix result = new Matrix();
            result.items = new Double[items.GetLength(0), rightMatrix.items.GetLength(1)];

            int width = items.GetLength(0);
            int height = rightMatrix.items.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < rightMatrix.items.GetLength(0); k++)
                    {
                        result.items[i, j] += items[i, k] * rightMatrix.items[k, j];
                    }
                }
            }

            return result;
        }

        public Matrix MultiplyLeft(Matrix leftMatrix)
        {
            Matrix result = new Matrix();
            result.items = new Double[leftMatrix.items.GetLength(0), items.GetLength(1)];

            int width = leftMatrix.items.GetLength(0);
            int height = items.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < items.GetLength(0); k++)
                    {
                        result.items[i, j] += leftMatrix.items[i, k] * items[k, j];
                    }
                }
            }

            return result;
        }
    }
}
