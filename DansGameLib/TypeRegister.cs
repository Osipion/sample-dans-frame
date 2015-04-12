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
    [Serializable]
    public class TypeRegister
    {
        [XmlIgnore]
        private static Lazy<XmlSerializer> serializer =
            new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(TypeRegister)));

        [XmlArray(IsNullable = false)]
        public TypeRegisterEntry[] Entries { get; set; }

        public void Save(string file)
        {
            using (var writer = new System.IO.StreamWriter(file))
                serializer.Value.Serialize(writer, this);
        }

        public static TypeRegister Load(System.IO.Stream stream)
        {
            return (TypeRegister)serializer.Value.Deserialize(stream);
        }

        public static TypeRegister Load(string file)
        {
            using (var reader = new System.IO.StreamReader(file))
                return TypeRegister.Load(reader.BaseStream);
        }

        //public void Register(bool overwrite_existing_interface_keys)
        //{
        //    foreach (var entry in this.Entries.OrderBy(x => x.CreationIndex))
        //    {
        //        if (entry.Value.InterfaceType.Equals(typeof(ICharacter)))
        //        {
        //            var inner = GameManager.Get<ISerializableCharacter>();

        //            entry.Value.Instance = new Character(inner);

        //        }
        //        else if (entry.Value.InterfaceType.Equals(typeof(ISerializableCharacter)))
        //        {
        //            var target = System.IO.Path.ChangeExtension(Properties.Resources.CharacterFileName, SerializableCharacter.CharacterFileExtension);

        //            if (System.IO.File.Exists(target))
        //                entry.Value.Instance = SerializableCharacter.Load(target);
        //        }

        //        GameManager.Manager.RegisterTypeEntry(entry.Value, overwrite_existing_interface_keys);
        //    }

        //}
    }

    [Serializable]
    public class TypeRegisterEntry
    {
        [XmlAttribute]
        public int CreationIndex { get; set; }

        [XmlElement]
        public TypeLookupItem Value { get; set; }
    }
}
