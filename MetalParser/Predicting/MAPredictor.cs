using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalParser.Predicting
{
    class MAPredictor
    {
        /// <summary>
        /// Predicts next number using MA methond with defined accuracy
        /// </summary>
        /// <param name="values">Time series</param>
        /// <param name="accuracy">Number of last items in time series</param>
        /// <returns></returns>
        public static double Predict(List<Double> values, int accuracy)
        {
            Double maSum = 0;
            int window = values.Count / accuracy;

            for (int i = values.Count - 1; i > window; i--) //Gotta get rid of magic number
            {
                maSum += values[i];
            }

            return maSum/window;
        }
    }
}
