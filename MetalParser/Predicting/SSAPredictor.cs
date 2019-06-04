using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalParser.Predicting
{
    class SSAPredictor
    {
        private int L = 100;

        public static double Predict(List<Double> values, int accuracy)
        {
            return 0;
        }

        private Double[,] BuildTrayectoryMatrix(List<Double> values)
        {
            int N = values.Count;
            if (L > N / 2)
                L = N - L;
            int K = N - L + 1;
            Double[,] X = new Double[L,K];
            for (int i = 0; i < K; i++)
                for (int j = 0; j < L; j++)
                    X[i, j] = 0;

            for(int i = 0; i < L; i++)
            {
                for (int j = 0; j < L + j -1; j++)
                {
                    X[i, j] = values[j];
                }
            }

            return X;
        }

        private void SVD(List<Double> values, int accuracy)
        {
            Double[,] X = BuildTrayectoryMatrix(values);
            Double[,] S = MultiplyMatriсes(X, Transpose(X));


        }

        private void Group()
        {

        }

        private void Reconstruct()
        {

        }

        private Double[,] Transpose(Double[,] matrix)
        {
            Double[,] transposedMatrix = null;

            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    transposedMatrix[i, j] = matrix[j, i];
                }
            }

            return transposedMatrix;
        }

        private Double[,] MultiplyMatriсes(Double[,] leftMatrix, Double[,] rightMatrix)
        {
            Double[,] result = new Double[leftMatrix.GetLength(0), rightMatrix.GetLength(1)];

            int width = leftMatrix.GetLength(0);
            int height = rightMatrix.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < rightMatrix.GetLength(0); k++)
                    {
                        result[i, j] += leftMatrix[i,k] * rightMatrix[k, j];
                    }
                }
            }

            return result;
        }
    }
}
