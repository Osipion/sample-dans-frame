using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DansGameCore;
using DansGameCore.Serialization;

namespace DansGameLib
{
    /// <summary>
    /// Provides the basic set of types which the GameManager looks up. Serializable
    /// </summary>
    [Serializable]
    public class TypeRegister
    {
        [XmlIgnore]
        private static Lazy<XmlSerializer> serializer =
            new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(TypeRegister)));

        /// <summary>
        /// A list of types to register
        /// </summary>
        [XmlArray(IsNullable = false)]
        public TypeRegisterEntry[] Entries { get; set; }

        /// <summary>
        /// Save the type registry to file
        /// </summary>
        /// <param name="file"></param>
        public void Save(string file)
        {
            using (var writer = new System.IO.StreamWriter(file))
                serializer.Value.Serialize(writer, this);
        }

        /// <summary>
        /// Load a TypeRegister from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static TypeRegister Load(System.IO.Stream stream)
        {
            return (TypeRegister)serializer.Value.Deserialize(stream);
        }

        /// <summary>
        /// Load a TypeRegister from a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static TypeRegister Load(string file)
        {
            using (var reader = new System.IO.StreamReader(file))
                return TypeRegister.Load(reader.BaseStream);
        }
    }

    /// <summary>
    /// An entry in the type registry
    /// </summary>
    [Serializable]
    public class TypeRegisterEntry
    {
        /// <summary>
        /// The order in which to create this type (from lowest to highest)
        /// </summary>
        [XmlAttribute]
        public int CreationIndex { get; set; }

        /// <summary>
        /// The TypeLookupItem containing the interface / class type match
        /// </summary>
        [XmlElement]
        public TypeLookupItem Value { get; set; }
    }
}
