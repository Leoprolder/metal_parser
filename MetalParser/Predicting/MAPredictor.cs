using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalParser.Predicting
{
    class MAPredictor
    {
        public static double Predict(List<Double> values)
        {
            Double maSum = 0;

            for(int i = values.Count - 1; i > values.Count/10; i--) //Gotta get rid of magic number
            {
                maSum += values[i];
            }

            return maSum/ values.Count / 10;
        }
    }
}
