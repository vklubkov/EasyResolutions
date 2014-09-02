//-----------------------------------------------------------------------
// <copyright file="FillAspectRatioResolutionComparisonComponentTests.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    internal class FillAspectRatioResolutionComparisonComponentTests : TestsBase
    {
        [TestCase(0, 0, 0, 0, TestName = "both resolutions A and B are zero.")]
        [TestCase(1366, 768, 1366, 768, TestName = "resolution A == resolution B.")]
        [TestCase(1366, 768, 768, 1360, TestName = "resolution A != resolution B, but aspect ratio factors of A and B are nearly equal.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsFalseAndResultIs0If(int widthA, int heightA, 
                                                      int widthB, int heightB)
        {
            var component = new FillAspectRatioResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.False, "Compare should have returned false (further comparison necessary).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (default value).");
        }

        [TestCase(1366, 768, 1680, 720, 1366, 768, TestName = "resolutions A is equal to game resolution, and aspect ratio factor of resolution B is not nearly equal to aspect ratio factor of game resolution.")]
        [TestCase(1360, 768, 1024, 768, 1366, 768, TestName = "resolutions A != game resolution, but aspect ratio factor of resolution A is ~= aspect ratio factor of game resolution, and aspect ratio factor of resolution B is not.")]
        [TestCase(1366, 768, 1024, 768, 1680, 720, TestName = "aspect ratio factor of resolution B is < aspect ratio factor of resolution A, which is < aspect ratio factor of game resolution.")]
        [TestCase(1366, 768, 1680, 720, 1024, 768, TestName = "aspect ratio factor of resolution B is > aspect ratio factor of resolution A, which is > aspect ratio factor of game resolution.")]
        [TestCase(1680, 720, 1024, 768, 1366, 768, TestName = "aspect ratio factor of resolution A is > aspect ratio factor of game resolution and aspect ratio factor of B is < aspect ratio factor of game resolution.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIsMinus1If(int widthA, int heightA, 
                                                          int widthB, int heightB, 
                                                          int gameWidth, int gameHeight)
        {
            var component = new FillAspectRatioResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(-1), "Compare result should have been -1 (A is greater).");
        }

        [TestCase(1680, 720, 1366, 768, 1366, 768, TestName = "resolutions B is equal to game resolution, and aspect ratio factor of resolution A is not nearly equal to aspect ratio factor of game resolution.")]
        [TestCase(1024, 768, 1360, 768, 1366, 768, TestName = "resolutions B != game resolution, but aspect ratio factor of resolution B is ~= aspect ratio factor of game resolution, and aspect ratio factor of resolution A is not.")]
        [TestCase(1024, 768, 1366, 768, 1680, 720, TestName = "aspect ratio factor of resolution A is < aspect ratio factor of resolution B, which is < aspect ratio factor of game resolution.")]
        [TestCase(1680, 720, 1366, 768, 1024, 768, TestName = "aspect ratio factor of resolution A is > aspect ratio factor of resolution B, which is > aspect ratio factor of game resolution.")]
        [TestCase(1024, 768, 1680, 720, 1366, 768, TestName = "aspect ratio factor of resolution B is > aspect ratio factor of game resolution and aspect ratio factor of A is < aspect ratio factor of game resolution.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs1If(int widthA, int heightA, 
                                                     int widthB, int heightB, 
                                                     int gameWidth, int gameHeight)
        {
            var component = new FillAspectRatioResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(1), "Compare result should have been 1 (B is greater).");
        }

        [TestCaseSource(typeof(TestCases), "NullResolutions")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs0If(Resolution resolutionA, Resolution resolutionB)
        {
            var component = new FillAspectRatioResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             resolutionA, 
                                             resolutionB, 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been equal to 0 (equality).");
        }
        
        private class TestCases
        {
            public static IEnumerable NullResolutions
            {
                get
                {
                    yield return new TestCaseData(null, null).SetName("both resolutions A and B are null.");
                    yield return new TestCaseData(null, new Resolution()).SetName("only resolution A is null.");
                    yield return new TestCaseData(new Resolution(), null).SetName("only resolutions B is null.");
                }
            }  
        }

    }
}