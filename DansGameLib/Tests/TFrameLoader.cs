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
    public class TFrameLoader : TestBase
    {
      [Test, MSTest.TestMethod]
      public void CanLoadFrames()
      {
          var frames = FrameLoader.LoadFrames();

          Assert.That(frames, Is.Not.Null);

          Assert.That(frames.Count, Is.GreaterThan(0));
      }
    }
}
