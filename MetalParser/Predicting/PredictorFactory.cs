using System;
using System.Collections.Generic;

namespace MetalParser.Predicting
{
    class PredictorFactory
    {
        private PredictorTypes _type;

        public PredictorFactory(PredictorTypes type)
        {
            _type = type;
        }

        public double? PredictValue(PredictorTypes type, List<Double> values, int accuracy)
        {
            double? predictedValue = null;

            switch (type)
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
