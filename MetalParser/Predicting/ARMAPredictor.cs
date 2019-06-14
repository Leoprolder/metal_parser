using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;

namespace MetalParser.Predicting
{
    class ARMAPredictor
    {
        public static double Predict(List<Double> values, int accuracy)
        {

            double epsilon = Normal.Sample(0.0, StandardDeviation(values));
            double X = epsilon + MAPredictor.Predict(values, accuracy) + ARModel(values, 2);

            return X;
        }

        private static double StandardDeviation(List<Double> values) //Sample Variance i.e. standard deviation
        {
            Double mean = values.Sum() / values.Count;
            List<Double> squaredDiff = new List<Double>(values.Count);
            for (int i = 0; i < values.Count; i++)
            {
                squaredDiff[i] = Math.Pow((values[i] - mean), 2.0);
            }

            return squaredDiff.Sum()/values.Count;
        }

        private static double ARModel(List<Double> values, int parameter)
        {
            double result = 0.0;
            double[] coefficients = new double[parameter];

            //Coefficients evaluation
            for (int i = 0; i < values.Count; i++)
            {
                double upper = 0.0;
                double lower = 0.0;

                for (int j = i + 1; j < parameter; j++) 
                {
                    upper += (values[j] - values.Mean()) * (values[j - 1] - values.Mean());
                    
                }
                for (int j = 0; j < parameter; j++)
                {
                    lower += Math.Pow(values[j] - values.Mean(), 2.0);
                }

                coefficients[i] = upper / lower;
            }

            for (int i = 0; i < parameter; i++)
            {
                result += coefficients[i] * values[values.Count - i + 1];
            }

            return result;
        }
    }
}
