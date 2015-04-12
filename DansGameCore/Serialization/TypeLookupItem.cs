using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Reflection;
using System.ComponentModel;

namespace DansGameCore.Serialization
{
    ///<summary>
    /// <para>
    /// This class is intended for use by the GameManager class only. It is public only so that it can be serialized. DO NOT directly instatiate this class outside of GameManager
    /// </para>
    /// </summary>
    [Serializable]
    public class TypeLookupItem
    {
        [XmlIgnore]
        private object instance = null;

        [XmlElement]
        public bool IsSingleton { get; set; }

        [XmlIgnore]
        public object Instance 
        { 
            get
            {
                if (!this.IsAssigned)
                    throw new InvalidOperationException("The TypeLookupItem class and interface values were not been initialized before being accessed");

                if (this.IsSingleton)
                {
                    if (this.instance == null)
                        this.instance = Activator.CreateInstance(this.ClassType);

                    return this.instance;
                }

                return Activator.CreateInstance(this.ClassType);
            }
            set
            {

                if (value == null)
                {
                    this.instance = null;
                    return;
                }

                if(!(value.GetType().IsAssignableFrom(this.ClassType)))
                    throw new ArgumentException("The instance property must be assignable from the class type");

                this.instance = value;
            }
        }

        [XmlIgnore]
        private Type interface_type = null;

        [XmlIgnore]
        public Type InterfaceType 
        { 
            get
            {
                if (!this.IsAssigned)
                    throw new InvalidOperationException("The TypeLookupItem class and interface values were not been initialized before being accessed");

                return this.interface_type;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("The class type must not be null");

                if(!value.IsInterface)
                    throw new ArgumentException("The property InterfaceType must be an interface type");

                if(this.class_type != null)
                {
                    if (!value.IsAssignableFrom(this.class_type))
                        throw new ArgumentException("The interface type must be implemented by the class type");
                }

                this.interface_type = value;
            }
        }

        [XmlIgnore]
        private Type class_type = null;

        [XmlIgnore]
        public Type ClassType 
        { 
            get
            {
                if (!this.IsAssigned)
                    throw new InvalidOperationException("The TypeLookupItem class and interface values were not been initialized before being accessed");

                return this.class_type;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("The class type must not be null");

                if(value.GetConstructor(Type.EmptyTypes) == null)
                    throw new ArgumentException("The class type must have a public, parameterless constructor");

                if(this.interface_type != null)
                {
                    if (!(value.GetInterfaces().Contains(this.interface_type)))
                        throw new ArgumentException("The class type must implement the specified interface type");
                }

                this.class_type = value;
            }
        }

        [XmlElement("InterfaceType", IsNullable = false)]
        public string InterfaceTypeName
        {
            get
            {
                return this.InterfaceType.AssemblyQualifiedName;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "The interface type name must not be null");

                this.InterfaceType = Type.GetType(value);
            }
        }

        [XmlElement("ClassType", IsNullable = false)]
        public string ClassTypeName
        {
            get
            {
                return this.ClassType.AssemblyQualifiedName;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "The class type type name must not be null");

                this.ClassType = Type.GetType(value);
            }
        }

        public TypeLookupItem() {  }

        public TypeLookupItem(Type interface_type, Type class_type, object instance = null, bool is_singleton = false)
        {
            this.ClassType = class_type;
            this.InterfaceType = interface_type;
            this.Instance = instance;
            this.IsSingleton = is_singleton;
        }

        [XmlIgnore]
        public bool IsAssigned
        {
            get
            {
                return this.class_type != null && this.interface_type != null;
            }
        }
    }
}
