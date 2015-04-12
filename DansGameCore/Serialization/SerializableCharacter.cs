using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DansGameCore.Serialization
{


    public interface ISerializableCharacter
    {    
        Guid ID { get; set; }
       
        SerializableDecision[] DecisionList { get; set; }

        void StoreDecision(Guid decision);

        void Add(object decision);

        void Save(string file);

        SerializableCounters Counters { get; set;  }
    }

    /// <summary>
    /// <para>
    /// This class is intended for use by the GameManager class only. It is public only so that it can be serialized. DO NOT directly instatiate this class outside of GameManager
    /// </para>
    /// </summary>
    /// <typeparam name="TCounters"></typeparam>
    [Serializable]
    public class SerializableCharacter : ISerializableCharacter
    {

        [XmlIgnore]
        public const string CharacterFileExtension = ".dgchar";

        [XmlIgnore]
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SerializableCharacter));

        [XmlIgnore]
        private static Lazy<XmlSerializer> serializer =
            new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(SerializableCharacter)));

        /// <summary>
        /// The unique identifier for this character
        /// </summary>
        [XmlElement(Order = 1, IsNullable = false)]
        public Guid ID { get; set; }

        [XmlElement(Order= 2, IsNullable = false)]
        public DateTime CharacterCreatedOn { get; set; }

        [XmlElement("Counters", Order = 3, IsNullable = false)]
        public SerializableCounters Counters { get; set; }

        private List<SerializableDecision> decisions = new List<SerializableDecision>();

        [XmlArray("Decisions", IsNullable = false, Order = 4)]
        public SerializableDecision[] DecisionList
        {
            get
            {
                return this.decisions.ToArray();
            }
            set
            {
                this.decisions = new List<SerializableDecision>(value);
            }
        }

        private int next_id()
        {
            return this.decisions.Count;
        }

        public void StoreDecision(Guid decision)
        {
            var d = new SerializableDecision(this.next_id(), decision);
            logger.InfoFormat("A game frame choice decision was stored for character {0}. The chosen frame was {1}", this.ID, decision);
            this.decisions.Add(d);
        }

        public SerializableCharacter()
        {
            this.ID = Guid.NewGuid();
            this.CharacterCreatedOn = DateTime.Now;
            this.Counters = new SerializableCounters();
        }

        /// <summary>
        /// Should only be used by the serializer
        /// </summary>
        /// <param name="decision"></param>
        public void Add(object decision)
        {
            if (this.decisions == null)
                this.decisions = new List<SerializableDecision>();

            this.decisions.Add((SerializableDecision)decision);
        }

        public static SerializableCharacter Load(string file)
        {
            logger.InfoFormat("Starting load of character file @ {0}", file);

            using (var reader = new System.IO.StreamReader(file))
                return (SerializableCharacter)SerializableCharacter.serializer.Value.Deserialize(reader);
        }

        public void Save(string file)
        {
            file = System.IO.Path.ChangeExtension(file, SerializableCharacter.CharacterFileExtension);

            logger.InfoFormat("Starting save of character to file @ {0}", file);

            using (var writer = new System.IO.StreamWriter(file))
                SerializableCharacter.serializer.Value.Serialize(writer, this);
        }

    }
}
