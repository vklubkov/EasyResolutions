//-----------------------------------------------------------------------
// <copyright file="ZeroResolutionComparisonComponentTests.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    internal class ZeroResolutionComparisonComponentTests : TestsBase
    {
        [TestCaseSource(typeof(Equality), "TestCases")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs0If(Resolution resolutionA, Resolution resolutionB)
        {
            var component = new ZeroResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             resolutionA, 
                                             resolutionB, 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (equality).");
        }

        [TestCase(0, 0, 1366, 768, TestName = "only resolution A is zero.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIsMinus1If(int widthA, int heightA, int widthB, int heightB)
        {
            var component = new ZeroResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(-1), "Compare result should have been -1 (A is greater).");
        }

        [TestCase(1366, 768, 0, 0, TestName = "only resolution B is zero.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs1If(int widthA, int heightA, int widthB, int heightB)
        {
            var component = new ZeroResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(1), "Compare result should have been 1 (B is greater).");
        }

        [TestCase(1366, 768, 1024, 768, TestName = "both resolutions are non-zero.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsFalseAndResultIs0If(int widthA, int heightA, int widthB, int heightB)
        {
            var component = new ZeroResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.False, "Compare should have returned false (further comparison necessary).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (default value).");
        }

        private class Equality
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(null, null).SetName("both resolutions A and B are null.");
                    yield return new TestCaseData(null, new Resolution()).SetName("only resolution A is null.");
                    yield return new TestCaseData(new Resolution(), null).SetName("only resolutions B is null.");
                    yield return new TestCaseData(new Resolution(), new Resolution()).SetName("both resolutions are zero.");
                }
            }  
        }
    }
}