//-----------------------------------------------------------------------
// <copyright file="OrientationResolutionComparisonComponentTests.cs" company="https://github.com/marked-one/EasyResolutions">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions.Tests
{
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    internal class OrientationResolutionComparisonComponentTests : TestsBase
    {
        [TestCase(1024, 768, 320, 480, 1366, 768, TestName = "game resolution is landscape, resolution A is landscape and resolution B is portrait.")]
        [TestCase(1024, 1024, 320, 480, 1366, 768, TestName = "game resolution is landscape, resolution A is square and resolution B is portrait.")]
        [TestCase(768, 1024, 480, 320, 768, 1366, TestName = "game resolution is portrait, resolution A is portrait and resolution B is landscape.")]
        [TestCase(1024, 1024, 480, 320, 768, 1366, TestName = "game resolution is portrait, resolution A is square and resolution B is landscape.")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIsMinus1If(int widthA, int heightA, 
                                                          int widthB, int heightB, 
                                                          int gameWidth, int gameHeight)
        {
            var component = new OrientationResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(-1), "Compare result should have been -1 (A is greater).");
        }

        [TestCase(320, 480, 1024, 768, 1366, 768, TestName = "game resolution is landscape, resolution A is portrait and resolution B is landscape.")]
        [TestCase(320, 480, 1024, 1024, 1366, 768, TestName = "game resolution is landscape, resolution A is portrait and resolution B is square.")]
        [TestCase(480, 320, 768, 1024, 768, 1366, TestName = "game resolution is portrait, resolution A is landscape and resolution B is portrait.")]
        [TestCase(480, 320, 1024, 1024, 768, 1366, TestName = "game resolution is portrait, resolution A is landscape and resolution B is square.")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIs1If(int widthA, int heightA, 
                                                     int widthB, int heightB, 
                                                     int gameWidth, int gameHeight)
        {
            var component = new OrientationResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(1), "Compare result should have been 1 (B is greater).");
        }

        [TestCase(1024, 768, 480, 320, 1366, 768, TestName="(1) game resolution is landscape and resolutions A and B are both landscape.")]
        [TestCase(1024, 768, 480, 320, 768, 1366, TestName="(2) game resolution is portrait and resolutions A and B are both landscape.")]
        [TestCase(1024, 768, 480, 320, 768, 768, TestName="(3) game resolution is square and resolutions A and B are both landscape.")]
        [TestCase(768, 1024, 320, 480, 1366, 768, TestName="(4) game resolution is landscape and resolutions A and B are both portrait.")]
        [TestCase(768, 1024, 320, 480, 768, 1366, TestName="(5) game resolution is portrait and resolutions A and B are both portrait.")]
        [TestCase(768, 1024, 320, 480, 768, 768, TestName="(6) game resolution is square and resolutions A and B are both portrait.")]
        [TestCase(1024, 1024, 480, 480, 1366, 768, TestName="(7) game resolution is landscape and resolutions A and B are both square.")]
        [TestCase(1024, 1024, 480, 480, 768, 1366, TestName="(8) game resolution is portrait and resolutions A and B are both square.")]
        [TestCase(1024, 1024, 480, 480, 768, 768, TestName="(9) game resolution is square and resolutions A and B are both square.")]
        [TestCase(1024, 1024, 480, 320, 1366, 768, TestName="(10) game resolution is landscape and one of resolutions A and B is square and other is landscape.")]
        [TestCase(1024, 1024, 480, 320, 768, 768, TestName="(11) game resolution is square and one of resolutions A and B is square and other is landscape.")]
        [TestCase(1024, 1024, 320, 480, 768, 1366, TestName="(12) game resolution is portrait and one of resolutions A and B is square and other is portrait.")]
        [TestCase(1024, 1024, 320, 480, 768, 768, TestName="(13) game resolution is square and one of resolutions A and B is square and other is portrait.")]
        [TestCase(1024, 768, 320, 480, 768, 768, TestName="(14) game resolution is square, resolutions A is landscape and resolution B is portrait.")]
        [TestCase(768, 1024, 480, 320, 768, 768, TestName="(15) game resolution is square, resolutions A is portrait and resolution B is landscape.")]
        [Category("Easy Resolutions")]
        public void CompareReturnsFalseAndResultIs0If(int widthA, int heightA, 
                                                      int widthB, int heightB, 
                                                      int gameWidth, int gameHeight)
        {
            var component = new OrientationResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.False, "Compare should have returned false (further comparison necessary).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (default value).");
        }

        [TestCaseSource(typeof(NullResolutions), "TestCases")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIs0If(Resolution resolutionA, Resolution resolutionB)
        {
            var component = new OrientationResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             resolutionA, 
                                             resolutionB, 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (equality).");
        }
        
        private class NullResolutions
        {
            public static IEnumerable TestCases
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