using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<Double> PredictList(List<Double> values, int accuracy)
        {
            List<Double> orig = new List<Double>(values);
            List<Double> rec = new List<Double>(Reconstruct(values, accuracy).ToList());

            for (int i = 0; i < values.Count; i++)
            {
                rec[i] = rec[i] * 10 + orig[i];
            }
            return rec;
        }

        private static Matrix<Double> BuildTrayectoryMatrix(List<Double> values)
        {
            int N = values.Count;
            if (L > N / 2)
                L = N - L;

            K = N - L + 1;
            Matrix<Double> X = DenseMatrix.Create(K, L, 0);

            for (int i = 0; i < K; i++) //Rows
            {
                for (int j = 0; j < L; j++) //Cols
                {
                    if (i + j <= values.Count - 1)
                        X[i, j] = values[i + j];
                }
            }

            //Double[,] X = new Double[L,K];
            //for (int i = 0; i < L; i++)
            //    for (int j = 0; j < K; j++)
            //        X[i, j] = 0;

            //for(int i = 0; i < L; i++)
            //{
            //    for (int j = 0; j < L + j -1; j++)
            //    {
            //        X[i, j] = values[j];
            //    }
            //}

            return X;
        }

        private static Matrix<Double> SVD(List<Double> values, int accuracy) //Singular Value Decomposition
        {                                                    
            Matrix<Double> X = BuildTrayectoryMatrix(values);
            Matrix<Double> V = X.Transpose() * X;            
            Svd<Double> svd = V.Svd(true);                   
            Matrix<Double> U = svd.VT;                       
            Matrix<Double> rca = U * V.Inverse();            

            return rca;
        }

        private static Vector<Double> Reconstruct(List<Double> values, int accuracy)
        {
            int N = values.Count;
            Matrix<Double> rca = SVD(values, accuracy);
            Vector<Double> y = DenseVector.Create(N, 0.0);
            int Lp = Math.Max(L, K);
            int Kp = Math.Min(L, K);

            for (int k = 0; k < Kp; k++)
            {
                for (int m = 1; m < k; m++)
                {
                    y[k + 1] += /*(1 / (Double.Parse(k.ToString()) + 1)) **/ rca[m, k - m];
                }
            }

            for (int k = Lp - 1; k < Kp - 1; k++)
            {
                for (int m = 1; m < Lp; m++)
                {
                    y[k + 1] += /*(1 / Double.Parse(Lp.ToString())) **/ rca[m, k - m];
                }
            }

            for (int k = Kp; k < N; k++)
            {
                for (int m = k - Kp + 2; m < N - Lp; m++)
                {
                    y[k + 1] += /*(1 / Double.Parse((N - K).ToString())) **/ rca[m, k - m];
                }
            }

            return y;
        }
    }
}
