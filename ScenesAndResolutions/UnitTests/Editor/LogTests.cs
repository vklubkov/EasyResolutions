//-----------------------------------------------------------------------
// <copyright file="LogTests.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using System.Collections;
    using NUnit.Framework;
    
    [TestFixture]
    internal class LogTests : TestsBase
    {
        [TestCaseSource(typeof(PropertyEnabled), "TestCases")]
        [Category("Scenes and Resolutions")]
        public void EnabledPropertySets(bool value)
        {
            Log.Enabled = value;
            Assert.That(Log.Enabled, Is.EqualTo(value), string.Format("Property should have returned {0}.", value));
            Log.Enabled = false;
        }
        
        private class PropertyEnabled
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(true).SetName("true if true specified.");
                    yield return new TestCaseData(false).SetName("false if false specified.");
                }
            }  
        }

        [TestCase(null, TestName  = "null specified.")]
        [TestCase("", TestName  = "empty string specified.")]
        [TestCase("Message.", TestName  = "valid string specified.")]
        [Category("Scenes and Resolutions")]
        public void InfoDoesntFailIf(string value)
        {
            Log.Info(value);
        }

        [TestCase(null, TestName  = "null specified.")]
        [TestCase("", TestName  = "empty string specified.")]
        [TestCase("Message.", TestName  = "valid string specified.")]
        [Category("Scenes and Resolutions")]
        public void WarningDoesntFailIf(string value)
        {
            Log.Warning(value);
        }
    }
}
