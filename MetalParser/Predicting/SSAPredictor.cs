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

        public static double Predict(List<Double> values)
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


    }
}
