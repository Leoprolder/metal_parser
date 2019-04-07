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

        public double? PredictValue(PredictorTypes type)
        {
            double? predictedValue = null;

            switch (type)
            {
                case (PredictorTypes.MA):
                    predictedValue = MAPredictor.Predict();
                    break;

                case (PredictorTypes.ARMA):
                    predictedValue = ARMAPredictor.Predict();
                    break;

                case (PredictorTypes.SSA):
                    predictedValue = SSAPredictor.Predict();
                    break;
            }

            return predictedValue;
        }
    }
}
