using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace MetalParser.Predicting
{
    class SSAPredictor
    {
        private static int L = 100;
        private static int K;

        public static double Predict(List<Double> values, int accuracy)
        {
            return Reconstruct(values, accuracy)[0];
        }

        private static Double[,] BuildTrayectoryMatrix(List<Double> values)
        {
            int N = values.Count;
            if (L > N / 2)
                L = N - L;

            K = N - L + 1;
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

        private static Matrix<Double> SVD(List<Double> values, int accuracy) //Singular Value Decomposition  //shitshitshitshitshit
        {                                                                                             //shitshitshitshitshit
            Matrix<Double> X = DenseMatrix.OfArray(BuildTrayectoryMatrix(values));                    //shitshitshitshitshit
            Matrix<Double> V = X.Transpose() * X;                                                     //shitshitshitshitshit
            Svd<Double> svd = V.Svd(true);                                                            //shitshitshitshitshit
            Matrix<Double> U = svd.VT;                                                                //shitshitshitshitshit                                                               //shitshitshitshitshit
            Matrix<Double> rca = U * V.Inverse();                                                     //shitshitshitshitshit

            return rca;
        }

        private static Vector<Double> Reconstruct(List<Double> values, int accuracy)
        {
            int N = values.Count;
            Matrix<Double> rca = SVD(values, accuracy);
            Vector<Double> y = DenseVector.Create(N, 0.0);
            int Lp = Math.Max(L, K);
            int Kp = Math.Min(L, K);

            for (int k = 0; k < Lp - 2; k++)
            {
                for (int m = 0; m < k+1; m++)
                {
                    y[k + 1] += (1 / (k + 1)) * rca[m, k - m + 2];
                }
            }

            for (int k = Lp - 1; k < Kp - 1; k++)
            {
                for (int m = 1; m < Lp; m++)
                {
                    y[k + 1] += (1 / Lp) * rca[m, k - m + 2];
                }
            }

            for (int k = Kp; k < N; k++)
            {
                for (int m = k - Kp + 2; m < N - Kp + 1; m++)
                {
                    y[k + 1] += (1 / (N - K)) * rca[m, k - m + 2];
                }
            }

            return y;
        }
    }
}
