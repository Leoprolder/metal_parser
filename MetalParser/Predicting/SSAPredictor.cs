using MetalParser.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using plane;

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
            Matrix X = new Matrix();
            X.items = BuildTrayectoryMatrix(values);
            Matrix S = new Matrix();
            S.items = X.MultiplyRight(X.Transpose()).items;

            Class1 class1 = new Class1();
            //https://habr.com/ru/post/132487/

        }

        private void Group()
        {

        }

        private void Reconstruct()
        {

        }
    }
}
