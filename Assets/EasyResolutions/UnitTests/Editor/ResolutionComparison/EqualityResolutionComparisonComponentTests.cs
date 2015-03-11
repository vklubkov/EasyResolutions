//-----------------------------------------------------------------------
// <copyright file="EqualityResolutionComparisonComponentTests.cs" company="https://github.com/marked-one/EasyResolutions">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions.Tests
{
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    internal class EqualityResolutionComparisonComponentTests : TestsBase
    {
        [TestCase(1366, 768, 1024, 768, 1366, 768, TestName = "resolution A is equal to game resolution, and B isn't.")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIsMinus1If(int widthA, int heightA, 
                                                          int widthB, int heightB, 
                                                          int gameWidth, int gameHeight)
        {
            var component = new EqualityResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(-1), "Compare result should have been -1 (A is greater).");
        }

        [TestCase(1366, 768, 1024, 768, 1024, 768, TestName = "resolution B is equal to game resolution, and A isn't.")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIs1If(int widthA, int heightA, 
                                                     int widthB, int heightB, 
                                                     int gameWidth, int gameHeight)
        {
            var component = new EqualityResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(1), "Compare should have been 1 (B is greater).");
        }

        [TestCase(1366, 768, 1024, 768, 320, 480, TestName="resolution A is not equal to B, and neither A nor B is equal to game resolution.")]
        [Category("Easy Resolutions")]
        public void CompareReturnsFalseAndResultIs0If(int widthA, int heightA, 
                                                      int widthB, int heightB, 
                                                      int gameWidth, int gameHeight)
        {
            var component = new EqualityResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.False, "Compare should have returned false (further comparison necessary).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (default value).");
        }

        [TestCaseSource(typeof(Equality), "TestCases")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIs0If(Resolution resolutionA, Resolution resolutionB)
        {
            var component = new EqualityResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             resolutionA, 
                                             resolutionB, 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (equality.");
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
                    yield return new TestCaseData(new Resolution(), new Resolution()).SetName("resolution A is equal to B and they both are zero.");
                    yield return new TestCaseData(new Resolution(1366, 768), new Resolution(1366, 768)).SetName("resolution A is equal to B and they are non zero.");
                }
            }  
        }
    }
}