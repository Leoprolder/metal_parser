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

            double epsilon = Normal.Sample(0.0, values.StandardDeviation());
            double X = epsilon + MAPredictor.Predict(values, accuracy) + ARModel(values, 2);

            return X;
        }

        public static List<Double> PredictList(List<Double> values, int accuracy)
        {
            List<Double> result = new List<Double>();
            int parameter = 2;
            double[] coefficients = GetCoefficients(values, parameter);
            double epsilon = Normal.Sample(0.0, values.StandardDeviation());
            List<Double> maModel = MAPredictor.PredictList(values, accuracy);

            for (int i = 0; i < values.Count; i++)
            {
                double sum = 0;
                for (int j = 0; j < parameter; j++)
                {
                    if (i > parameter)
                    {
                        sum += values[i - j] * coefficients[parameter - j - 1];
                    }
                    else
                    {
                        sum = values[i];
                        break;
                    }
                }
                result.Add(epsilon + (sum + maModel[i])/2);
            }

            return result;
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

        private static double[] GetCoefficients(List<Double> values, int parameter)
        {
            double result = 0.0;
            double[] coefficients = new double[parameter];

            //Coefficients evaluation
            for (int i = 0; i < coefficients.Count(); i++)
            {
                double upper = 0.0;
                double lower = 0.0;

                for (int j = i + 1; j < parameter; j++) 
                {
                    upper += (values[values.Count - j] - values.Mean()) * (values[values.Count - j - i] - values.Mean());
                    
                }
                for (int j = 0; j < parameter; j++)
                {
                    lower += Math.Pow(values[values.Count - j - 1] - values.Mean(), 2.0);
                }

                coefficients[i] = upper / lower;
            }

            return coefficients;
        }

        private static double ARModel(List<Double> values, int parameter)
        {
            double[] coefficients = GetCoefficients(values, parameter);
            double result = 0;

            for (int i = 0; i < parameter; i++)
            {
                result += values[values.Count - i - 1] * coefficients[parameter - i - 1];
            }

            return result;
        }
    }
}
