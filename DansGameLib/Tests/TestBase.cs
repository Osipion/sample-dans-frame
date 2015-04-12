using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using DansGameCore;

namespace DansGameLib.Tests
{
    [TestFixture, MSTest.TestClass]
    public class TestBase
    {
        protected log4net.ILog Logger;

        public TestBase()
        {

            using (var reader = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.test_logging_config)))
                log4net.Config.XmlConfigurator.Configure(reader);

            this.Logger = log4net.LogManager.GetLogger(this.GetType());
            Logger.InfoFormat("{0}{0}{1}{0}BEGIN TEST CLASS LOG{0}{1}{0}{0}", Environment.NewLine, new String('*', 40));

        }

        [TestFixtureTearDown, MSTest.TestCleanup]
        public void End()
        {
            if (this.Logger != null)
            {
                this.Logger.InfoFormat("{0}{1}END TEST CLASS LOG{1}{0}", Environment.NewLine, new String('-', 20));
                log4net.LogManager.Shutdown();
            }
        }

        public class MockGameFrame : IBaseGameFrame
        {
            public Guid ID { get; set; }

            public IDictionary<int, Option> Choices { get; set;  }

            public string Message { get; set; }

            public static MockGameFrame GetTestFrame()
            {
                return new MockGameFrame()
                {
                    ID = Guid.NewGuid(),
                    Message = "My Message",
                    Choices = new Dictionary<int, Option>()
                    { 
                        {0, new Option() { Message = "Message1", NextFrameID = Guid.NewGuid() } }, 
                        {1, new Option() { Message = "Message2", NextFrameID = Guid.NewGuid() } },
                        {2, new Option() { Message = "Message3", NextFrameID = Guid.NewGuid() } },
                        {3, new Option() { Message = "Message4", NextFrameID = Guid.NewGuid() } },
                        {4, new Option() { Message = "Message5", NextFrameID = Guid.NewGuid() } },
                    }
                };
            }

            public void MakeDecision(int choice, ICharacter character_context)
            {
            }
        }
    }
}
