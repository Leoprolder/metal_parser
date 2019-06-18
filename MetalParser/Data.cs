using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MetalParser
{
    [DataContract]
    class Data
    {
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public Double Value { get; set; }

        public Data(DateTime date, Double value)
        {
            Date = date;
            Value = value;
        }
    }
}
