using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalParser.Predicting
{
    class PredictorFactory
    {
        private PredictorTypes _type;

        public PredictorFactory(PredictorTypes type)
        {
            _type = type;
        }

        public double? PredictValue(PredictorTypes type, List<Double> values)
        {
            double? predictedValue = null;

            switch (type)
            {
                case (PredictorTypes.MA):
                    predictedValue = MAPredictor.Predict(values);
                    break;

                case (PredictorTypes.ARMA):
                    predictedValue = ARMAPredictor.Predict(values);
                    break;

                case (PredictorTypes.SSA):
                    predictedValue = SSAPredictor.Predict(values);
                    break;
            }

            return predictedValue;
        }
    }
}
