using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DansGameCore
{
    public interface ICharacter : IEnumerable<IDecision>
    {
        /// <summary>
        /// Gets the character's unique identifier
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// Stores a decision the character has made
        /// </summary>
        /// <param name="next_frame"></param>
        void StoreDecision(Guid next_frame);

        /// <summary>
        /// Supplies the character with the underlying stored data it needs - usually loaded from file when a game is restarted
        /// </summary>
        /// <param name="character"></param>
        void SetUnderlyingDetails(Serialization.ISerializableCharacter character);

        /// <summary>
        /// Saves the ICharacter's details to file
        /// </summary>
        /// <param name="file_path"></param>
        void Serialize(string file_path);

        /// <summary>
        /// Gets the collection of counters associated with the character
        /// </summary>
        Counters Counters { get; }

        /// <summary>
        /// Gets a flag indicating whether the character has been initialized
        /// </summary>
        bool IsInitialized { get; }
    }

    public class Character : ICharacter
    {
        private Serialization.ISerializableCharacter character = null;

        private Counters counters = null;

        public bool IsInitialized
        {
            get
            {
                return this.character != null && this.counters != null;
            }
        }

        public Character(Serialization.ISerializableCharacter character)
        {
            this.SetUnderlyingDetails(character);
        }

        public Character() 
        { 
        }

        public void SetUnderlyingDetails(Serialization.ISerializableCharacter character)
        {
            this.character = character;
            this.counters = character.Counters.ToCounters();
        }

        public Guid ID 
        { 
            get 
            {
                if (!this.IsInitialized)
                    throw new InvalidOperationException("The character has not been initialzed with it's underlying details");

                return this.character.ID; 
            } 
        }

        public void StoreDecision(Guid decision)
        {
            if (!this.IsInitialized)
                throw new InvalidOperationException("The character has not been initialzed with it's underlying details");

            this.character.StoreDecision(decision);
        }

        public IEnumerator<IDecision> GetEnumerator()
        {
            if (!this.IsInitialized)
                throw new InvalidOperationException("The character has not been initialzed with it's underlying details");

            foreach(var dec in this.character.DecisionList)
                yield return Decision.InterfaceFromSerializedInstance(dec);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Serialize(string file_path)
        {
            if (!this.IsInitialized)
                throw new InvalidOperationException("The character has not been initialzed with it's underlying details");

            this.character.Counters = Serialization.SerializableCounters.FromCounters(this.Counters);

            this.character.Save(file_path);
        }

        public Counters Counters 
        { 
            get 
            {
                if(!this.IsInitialized)
                    throw new InvalidOperationException("The character has not been initialzed with it's underlying details");

                return this.counters; 
            } 
        }
    }
}
