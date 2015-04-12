using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using DansGameCore;
using DansGameCore.Serialization;

namespace DansGameLib.Tests
{
    [TestFixture, MSTest.TestClass]
    public class TGameManager : TestBase
    {

        [Test, MSTest.TestMethod]
        public void CanRegisterCharacter()
        {
            GameManager.RegisterType<ISerializableCharacter, SerializableCharacter>(null, false, true);
        }

        [Test, MSTest.TestMethod]
        public void CanRetrieveRegisteredCharacter()
        {
            GameManager.RegisterType<ISerializableCharacter, SerializableCharacter>(null, false, true);

            var character_ser = GameManager.Get<ISerializableCharacter>();

            GameManager.RegisterType<ICharacter, Character>(null, false, true);

            var c = GameManager.Get<ICharacter>();

            c.SetUnderlyingDetails(character_ser);

            var i = c.ID;
        }

        [Test, MSTest.TestMethod]
        public void DefaultCharacterAutomaticallyAssigned()
        {
            GameManager.NewCharacter(true, false);

            var c = GameManager.Get<ICharacter>();

            var id = c.ID;
        }
    }
}
