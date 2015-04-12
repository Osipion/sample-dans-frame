using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DansGameCore.Serialization
{
    public class SerializableCounters
    {
        [XmlArray]
        public List<CounterRecord> Counters { get; set; }

        public SerializableCounters()
        {
            this.Counters = new List<CounterRecord>();
        }

        public static SerializableCounters FromCounters(Counters input)
        {
            var tmp = new SerializableCounters();

            foreach (var counter in input)
                tmp.Counters.Add(new CounterRecord() { Name = counter.Key, Value = counter.Value });

            return tmp;
        }

        public Counters ToCounters()
        {
            var counters = new Counters();

            foreach (var c in this.Counters)
            {
                counters.Add(c.Name, c.Value);
            }

            return counters;
        }

        [Serializable]
        public class CounterRecord
        {
            [XmlAttribute]
            public string Name { get; set; }

            [XmlAttribute]
            public double Value { get; set; }
        }
    }
}
