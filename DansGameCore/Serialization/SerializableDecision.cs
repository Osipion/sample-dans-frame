using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DansGameCore.Serialization
{
    /// <summary>
    /// <para>
    /// This class is intended for use by the GameManager class only. It is public only so that it can be serialized. DO NOT directly instatiate this class outside of GameManager
    /// </para>
    /// </summary>
    [Serializable]
    public class SerializableDecision : IComparable<IDecision>, IDecision
    {
        [XmlAttribute]
        public int DecisionHistoryOrder { get; set; }

        [XmlElement]
        public Guid ChosenFrame { get; set; }

        public SerializableDecision(int order, Guid chosen_frame)
        {
            this.DecisionHistoryOrder = order; this.ChosenFrame = chosen_frame;
        }

        public SerializableDecision() { }

        public SerializableDecision Clone()
        {
            return new SerializableDecision(this.DecisionHistoryOrder, this.ChosenFrame);
        }

        public int CompareTo(IDecision other)
        {
            if (this.DecisionHistoryOrder < other.DecisionHistoryOrder)
                return -1;
            if (this.DecisionHistoryOrder == other.DecisionHistoryOrder)
                return 0;
            return 1;
        }
    }
}
