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
    public class TCharacter : TestBase
    {
        [Test, MSTest.TestMethod]
        public void SetsGuidOnInitialize()
        {
            var cha = new SerializableCharacter();

            Assert.That(cha.ID, Is.Not.Null);
        }

        [Test, MSTest.TestMethod]
        public void CanStoreDecision()
        {
            var cha = new SerializableCharacter();

            var mock_frame = TestBase.MockGameFrame.GetTestFrame();

            cha.StoreDecision(mock_frame.ID);

            Assert.That(cha.DecisionList.Length, Is.EqualTo(1));

            Assert.That(cha.DecisionList.First().ChosenFrame, Is.EqualTo(mock_frame.ID));

            Assert.That(cha.DecisionList.First().DecisionHistoryOrder, Is.EqualTo(0));
        }

        [Test, MSTest.TestMethod]
        public void AssignsNextIdsSequentially()
        {
            var cha = new SerializableCharacter();

            var mock_frame = TestBase.MockGameFrame.GetTestFrame();

            for (var i = 0; i < 10; i++)
                cha.StoreDecision(mock_frame.ID);

            Assert.That(cha.DecisionList.Count(), Is.EqualTo(10));

            var vals = Enumerable.Range(0, 10).ToList();

            var counter = 0;

            foreach (var decision in cha.DecisionList)
            {
                if (vals.Contains(decision.DecisionHistoryOrder))
                    vals.Remove(decision.DecisionHistoryOrder);
                else
                {
                    counter++;
                    Logger.ErrorFormat("Value {0} was found in the character's decision history, but it was not expected", decision.DecisionHistoryOrder);
                }
            }

            Assert.That(counter, Is.EqualTo(0), "More than zero values were found that were not as expected");
            Assert.That(vals.Count, Is.EqualTo(0), "At least one of the expected values was not found");
        }

        [Test, MSTest.TestMethod]
        public void CanSerialize()
        {
            var cha = new SerializableCharacter();

            var mock_frame = TestBase.MockGameFrame.GetTestFrame();

            for (var i = 0; i < 10; i++)
                cha.StoreDecision(mock_frame.ID);

            var file_name = System.IO.Path.ChangeExtension(Guid.NewGuid().ToString("N"), SerializableCharacter.CharacterFileExtension);

            if (System.IO.File.Exists(file_name))
                System.IO.File.Delete(file_name);

            Assert.That(System.IO.File.Exists(file_name), Is.False);

            cha.Save(file_name);

            Assert.That(System.IO.File.Exists(file_name), Is.True);

            System.IO.File.Delete(file_name);
        }
    }
}
