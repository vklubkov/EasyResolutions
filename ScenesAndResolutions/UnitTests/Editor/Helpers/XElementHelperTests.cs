//-----------------------------------------------------------------------
// <copyright file="XElementHelperTests.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using System.Collections;
    using System.Xml.Linq;
    using NUnit.Framework;
    
    [TestFixture]
    internal class XElementHelperTests : TestsBase
    {
        [TestCase(TestName="XElement contains int attribute with desired name.")]
        public void TryIntAttributeReturnsTrueAndSuccesfullyReadsAttributeValueIf()
        {
            var attributeValue = 42;
            var element = new XElement("SomeElement", new XAttribute("name", attributeValue));
            int value;
            var returned = element.TryIntAttribute("name", out value);
            Assert.That(returned, Is.True, "TryIntAttribute should have returned true (success).");
            Assert.That(value, Is.EqualTo(attributeValue), string.Format("Attribute value should have been equal to {0}.", attributeValue));
        }

        [TestCaseSource(typeof(TestCases), "XElementWithInvalidIntAttribute")]
        public void TryIntAttributeReturnsFalseAndValueIs0If(XElement element)
        {
            int value;
            var returned = element.TryIntAttribute("name", out value);
            Assert.That(returned, Is.False, "TryIntAttribute should have returned false (failure).");
            Assert.That(value, Is.EqualTo(0), "Attribute value should have been 0.");
        }

        private class TestCases
        {
            public static IEnumerable XElementWithInvalidIntAttribute
            {
                get
                {
                    yield return new TestCaseData(null).SetName("XElement is null");
                    yield return new TestCaseData(new XElement("SomeElement")).SetName("XElement does not contain attributes.");
                    yield return new TestCaseData(new XElement("SomeElement", new XAttribute("wrong_name", 42))).SetName("XElement contains int attribute, but with different name.");
                    yield return new TestCaseData(new XElement("SomeElement", new XAttribute("name", "Some nice string."))).SetName("XElement contains attribute with desired name, but it is not integer attribute.");
                }
            }  
        }
    }
}