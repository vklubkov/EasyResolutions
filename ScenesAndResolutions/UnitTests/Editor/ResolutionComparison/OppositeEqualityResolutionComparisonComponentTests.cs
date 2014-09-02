//-----------------------------------------------------------------------
// <copyright file="OppositeEqualityResolutionComparisonComponentTests.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    internal class OppositeEqualityResolutionComparisonComponentTests : TestsBase
    {
        [TestCase(768, 1366, 1024, 768, 1366, 768, TestName="resolutions A and B are not equal, width of resolution A is equal to height of game resolution and height of resolution A is equal to width of game resolution.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIsMinus1If(int widthA, int heightA, 
                                                          int widthB, int heightB, 
                                                          int gameWidth, int gameHeight)
        {
            var component = new OppositeEqualityResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(-1), "Compare result should have been -1 (A is greater).");
        }
        
        [TestCase(1024, 768, 1366, 768, 768, 1366, TestName="resolutions A and B are not equal, width of resolution B is equal to height of game resolution and height of resolution B is equal to width of game resolution.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs1If(int widthA, int heightA, 
                                                     int widthB, int heightB, 
                                                     int gameWidth, int gameHeight)
        {
            var component = new OppositeEqualityResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(1), "Compare result should have been 1 (B is greater).");
        }

        [TestCase(1366, 768, 1024, 768, 320, 480, TestName="resolutions A and B are not equal, neither the width of A nor the width of B is equal to the height of game resolution and neither the height of A nor the height of B is equal to the width of game resolution.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsFalseAndResultIs0If(int widthA, int heightA, 
                                                      int widthB, int heightB, 
                                                      int gameWidth, int gameHeight)
        {
            var component = new OppositeEqualityResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.False, "Compare should have returned false (further comparison necessary).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (default value).");
        }

        [TestCaseSource(typeof(Equality), "TestCases")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs0If(Resolution resolutionA, Resolution resolutionB)
        {
            var component = new OppositeEqualityResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             resolutionA, 
                                             resolutionB, 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (equality).");
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
                    yield return new TestCaseData(new Resolution(1366, 768), new Resolution(1366, 768)).SetName("resolutions A and B are equal.");
                }
            }  
        }
    }
}