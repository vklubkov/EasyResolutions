//-----------------------------------------------------------------------
// <copyright file="SceneResolutionPickerTests.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ScenesAndResolutions.Tests
{
    using System.Collections;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    internal class SceneResolutionPickerTests : TestsBase
    {
        private static System.Random random = new System.Random();

        private static SceneMode RandomSceneMode
        {
            get
            {
                bool needFill = random.NextDouble() > 0.5;
                return needFill ? SceneMode.Fill : SceneMode.Fit;
            }
        }

        private static int RandomResolutionWidth
        {
            get
            {
                return random.Next(8192);
            }
        }

        private static int RandomResolutionHeight
        {
            get
            {
                return random.Next(8192);
            }
        }

        [TestCase(TestName="the specified scene name is null.")]
        [Category("Scenes and Resolutions")]
        public void PickResolutionReturnsNullIf()
        {
            string sceneName = null;
            Assert.That(sceneName.PickResolution(), Is.Null, "PickResolution should have returned null.");
            Assert.That(sceneName.PickResolution(RandomSceneMode), Is.Null, "PickResolution(SceneMode) should have returned null.");
            Assert.That(sceneName.PickResolution(RandomResolutionWidth, RandomResolutionHeight), Is.Null, "PickResolution(int, int) should have returned null.");
            Assert.That(sceneName.PickResolution(RandomResolutionWidth, RandomResolutionHeight, RandomSceneMode), Is.Null, "PickResolution(int, int, SceneMode) should have returned null.");
        }

        [TestCase(null, TestName="null, if IAvailableResolutions mock has returned null.")]
        [TestCase("", TestName="empty string, if IAvailableResolutions mock has returned an empty string.")]
        [TestCase("1366x768", TestName="non-empty string, if IAvailableResolutions mock has returned a non-empty string.")]
        [Category("Scenes and Resolutions")]
        public void PickResolutionReturns(string expected)
        {
            var sceneName = "SceneName";
            var available = Substitute.For<IAvailableResolutions>();
            available.GetBest(sceneName, Arg.Any<UnityEngine.Resolution>(), Arg.Any<SceneMode>()).Returns(expected);
            SceneResolutionPicker.AvailableResolutions = available;

            Assert.That(sceneName.PickResolution(), Is.EqualTo(expected), string.Format("PickResolution should have returned {0}.", expected));
            Assert.That(sceneName.PickResolution(RandomSceneMode), Is.EqualTo(expected), string.Format("PickResolution(SceneMode) should have returned {0}.", expected));
            Assert.That(sceneName.PickResolution(RandomResolutionWidth, RandomResolutionHeight), Is.EqualTo(expected), string.Format("PickResolution(int, int) should have returned {0}.", expected));
            Assert.That(sceneName.PickResolution(RandomResolutionWidth, RandomResolutionHeight, RandomSceneMode), Is.EqualTo(expected), string.Format("PickResolution(int, int, SceneMode) should have returned {0}.", expected));
            available.Received(4).GetBest(sceneName, Arg.Any<UnityEngine.Resolution>(), Arg.Any<SceneMode>());

            // Cleanup.
            SceneResolutionPicker.AvailableResolutions = null; 
        }

        [TestCaseSource(typeof(PropertyIAvailableResolutions), "TestCases")]
        [Category("Scenes and Resolutions")]
        public void AvailableResolutionsPropertySets(IAvailableResolutions available)
        {
            SceneResolutionPicker.AvailableResolutions = available;
            Assert.That(SceneResolutionPicker.AvailableResolutions, Is.SameAs(available), string.Format("Property should have returned '{0}'.", available));

            // Cleanup.
            SceneResolutionPicker.AvailableResolutions = null; 
        }
        
        private class PropertyIAvailableResolutions
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(null).SetName("null if null specified.");
                    yield return new TestCaseData(Substitute.For<IAvailableResolutions>()).SetName("valid object if valid object specified.");
                }
            }  
        }
    }
}