using System;
using System.Collections.Generic;

namespace MetalParser.Predicting
{
    class PredictorFactory
    {
        public PredictorTypes Type;

        public PredictorFactory(PredictorTypes type)
        {
            Type = type;
        }

        public double? PredictValue(List<Double> values, int accuracy)
        {
            double? predictedValue = null;

            switch (Type)
            {
                case (PredictorTypes.MA):
                    predictedValue = MAPredictor.Predict(values, accuracy);
                    break;

                case (PredictorTypes.ARMA):
                    predictedValue = ARMAPredictor.Predict(values, accuracy);
                    break;

                case (PredictorTypes.SSA):
                    predictedValue = SSAPredictor.Predict(values, accuracy);
                    break;
            }

            return predictedValue;
        }
    }
}
