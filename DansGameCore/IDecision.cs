using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DansGameCore
{
    public interface IDecision : IComparable<IDecision>
    {
        int DecisionHistoryOrder { get; }

        Guid ChosenFrame { get; }
    }

    public class Decision : IDecision
    {
        public int DecisionHistoryOrder { get; protected set; }

        public Guid ChosenFrame { get; protected set; }

        public Decision(int order, Guid chosen_frame)
        {
            this.DecisionHistoryOrder = order;
            this.ChosenFrame = chosen_frame;
        }

        public int CompareTo(IDecision other)
        {
            if (this.DecisionHistoryOrder < other.DecisionHistoryOrder)
                return -1;
            if (this.DecisionHistoryOrder == other.DecisionHistoryOrder)
                return 0;
            return 1;
        }

        public static Decision FromSerializedInstance(Serialization.SerializableDecision instance)
        {
            return new Decision(instance.DecisionHistoryOrder, instance.ChosenFrame);
        }

        public static IDecision InterfaceFromSerializedInstance(Serialization.SerializableDecision instance)
        {
            return FromSerializedInstance(instance);
        }
    }

}
