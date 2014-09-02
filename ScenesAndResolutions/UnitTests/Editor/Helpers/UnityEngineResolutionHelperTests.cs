//-----------------------------------------------------------------------
// <copyright file="UnityEngineResolutionHelperTests.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    internal class UnityEngineResolutionHelperTests : TestsBase
    {
        [TestCaseSource(typeof(TestCases), "ZeroAndPositiveWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void SetSetsSpecifiedWidthAndHeightIf(int width, int height)
        {
            var resolution = new UnityEngine.Resolution().Set(width, height);
            Assert.That(resolution.width, Is.EqualTo(width), string.Format("Resolution width should have been {0}.", width));
            Assert.That(resolution.height, Is.EqualTo(height), string.Format("Resolution height should have been {0}.", height));
        }

        [TestCaseSource(typeof(TestCases), "ZeroAndNegativeWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void SetSetsZeroWidthAndHeightIf(int width, int height)
        {
            var resolution = new UnityEngine.Resolution().Set(width, height);
            Assert.That(resolution.width, Is.EqualTo(0), "Resolution width should have been 0.");
            Assert.That(resolution.height, Is.EqualTo(0), "Resolution height should have been 0.");
        }

        [TestCase(0, 0, 0f, TestName = "0f, if both width and height are 0.")]
        [TestCase(480, 480, 1f, TestName = "1f, if width and height are equal.")]
        [TestCase(1366, 768, 1.778645833333333f, TestName = "(float)width/(float)height, if width > height.")]
        [TestCase(768, 1024, 1.333333333333333f, TestName = "(float)height/(float)width, if width < height.")]
        [Category("Scenes and Resolutions")]
        public void GetAspectRatioFactorReturns(int width, int height, float expected)
        {
            var resolution = new UnityEngine.Resolution().Set(width, height);
            var returned = resolution.GetAspectRatioFactor();
            var epsilon = FloatHelper.EpsilonForComparisonOfAspectRatioFactors;
            Assert.That(returned, Is.EqualTo(expected).Within(epsilon), string.Format("GetAspectRatioFactor should have returned a value nearly equal to {0}.", expected));
        }

        [TestCase(0, 0, TestName = "both width and height are 0.")]
        [TestCase(480, 480, TestName = "width and height are equal.")]
        [TestCase(1366, 768, TestName = "width > height.")]
        [Category("Scenes and Resolutions")]
        public void IsLandscapeReturnsTrueIf(int width, int height)
        {
            var resolution = new UnityEngine.Resolution().Set(width, height);
            Assert.That(resolution.IsLandscape(), "IsLandscape should have returned true.");
        }
        
        [TestCase(768, 1024, TestName = "height > width.")]
        [Category("Scenes and Resolutions")]
        public void IsLandscapeReturnsFalseIf(int width, int height)
        {
            var resolution = new UnityEngine.Resolution().Set(width, height);
            Assert.That(resolution.IsLandscape(), Is.Not.True, "IsLandscape should have returned false.");
        }
        
        [TestCase(0, 0, TestName = "both width and height are 0.")]
        [TestCase(480, 480, TestName = "width and height are equal.")]
        [TestCase(768, 1024, TestName = "height > width.")]
        [Category("Scenes and Resolutions")]
        public void IsPortraitReturnsTrueIf(int width, int height)
        {
            var resolution = new UnityEngine.Resolution().Set(width, height);
            Assert.That(resolution.IsPortrait(), "IsPortrait should have returned true.");
        }
        
        [TestCase(1366, 768, TestName = "width > height.")]
        [Category("Scenes and Resolutions")]
        public void IsPortraitReturnsFalseIf(int width, int height)
        {
            var resolution = new UnityEngine.Resolution().Set(width, height);
            Assert.That(resolution.IsPortrait(), Is.Not.True, "IsPortrait should have returned false.");
        }

        private class TestCases
        {
            public static IEnumerable ZeroAndPositiveWidthAndHeight
            {
                get
                {
                    yield return new TestCaseData(1366, 768).SetName("specified width and height are > 0.");
                    yield return new TestCaseData(0, 0).SetName("specified width and height are 0.");
                }
            }  
            
            public static IEnumerable ZeroAndNegativeWidthAndHeight
            {
                get
                {
                    yield return new TestCaseData(-1366, -768).SetName("specified width and height are < 0.");
                    yield return new TestCaseData(1366, -768).SetName("specified width is > 0 and height is < 0.");
                    yield return new TestCaseData(-1366, 768).SetName("specified width is < 0 and height is > 0.");
                    yield return new TestCaseData(0, 0).SetName("specified width and height are 0.");
                    yield return new TestCaseData(1366, 0).SetName("specified width is > 0 and height is 0.");
                    yield return new TestCaseData(0, 768).SetName("specified width is 0 and height is > 0 .");
                    yield return new TestCaseData(-1366, 0).SetName("specified width is < 0 and height is 0.");
                    yield return new TestCaseData(0, -768).SetName("specified width is 0 and height is < 0.");
                }
            }  
        }
    }
}