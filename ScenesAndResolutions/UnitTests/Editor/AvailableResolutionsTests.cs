//-----------------------------------------------------------------------
// <copyright file="AvailableResolutionsTests.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    internal class AvailableResolutionsTests : TestsBase
    {
        private static System.Random random = new System.Random();

        private static string sceneName = "TestScene";
        
        private static SceneMode RandomSceneMode
        {
            get
            {
                bool needFill = random.NextDouble() > 0.5;
                return needFill ? SceneMode.Fill : SceneMode.Fit;
            }
        }
        
        private static UnityEngine.Resolution RandomGameResolution
        {
            get
            {
                var width = random.Next(8192);
                var height = random.Next(8192);
                return new UnityEngine.Resolution().Set(width, height);
            }
        }

        [TestCase(TestName = "creates default resolution comparers.")]
        [Category("Scenes and Resolutions")]
        public void DefaultConstructor()
        {
            var availableResolutions = new AvailableResolutions();
            Assert.That(availableResolutions.FillComparer, Is.Not.Null, "FillComparer should not have been null.");
            Assert.That(availableResolutions.FitComparer, Is.Not.Null, "FitComparer should not have been null.");
        }

        [TestCaseSource(typeof(PropertyIResolutionComparer), "TestCases")]
        [Category("Scenes and Resolutions")]
        public void FillComparerPropertySets(IResolutionComparer comparer)
        {
            var availableResolutions = new AvailableResolutions();
            availableResolutions.FillComparer = comparer;
            Assert.That(availableResolutions.FillComparer, Is.SameAs(comparer), string.Format("Property should have returned {0}.", comparer));
        }
        
        [TestCaseSource(typeof(PropertyIResolutionComparer), "TestCases")]
        [Category("Scenes and Resolutions")]
        public void FitComparerPropertySets(IResolutionComparer comparer)
        {
            var availableResolutions = new AvailableResolutions();
            availableResolutions.FitComparer = comparer;
            Assert.That(availableResolutions.FitComparer, Is.SameAs(comparer), string.Format("Property should have returned {0}.", comparer));
        }

        private class PropertyIResolutionComparer
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(null).SetName("null if null specified.");
                    yield return new TestCaseData(Substitute.For<IResolutionComparer>()).SetName("valid object if valid object specified.");
                }
            }  
        }

        [TestCase(null, 1024, 768, TestName = "null specified.")]
        [TestCase("", 1366, 768, TestName = "empty string specified.")]
        [Category("Scenes and Resolutions")]
        public void AddFailsIf(string sceneName, int width, int height)
        {
            {
                var availableResolutions = new AvailableResolutions();
                var returned = availableResolutions.Add(sceneName);
                Assert.That(returned, Is.False, "Add should have returned false (failure).");
            }

            {
                var availableResolutions = new AvailableResolutions();
                var returned = availableResolutions.Add(sceneName, width, height);
                Assert.That(returned, Is.False, "Add should have returned false (failure).");
            }

        }

        [TestCase(1366, 768, TestName = "scene name with positive resolution specified.")]
        [Category("Scenes and Resolutions")]
        public void AddAddsSceneWithSpecifiedResolutionIf(int width, int height)
        {
            {
                var sceneNameWithResolution = string.Format("{0}.{1}x{2}", sceneName, width, height);
                var availableResolutions = new AvailableResolutions();
                var returned = availableResolutions.Add(sceneNameWithResolution);
                Assert.That(returned, Is.True, "Add should have returned true (success).");
                CheckIfResolutionAvailableForScene(availableResolutions, sceneName, width, height);
            }

            {
                var availableResolutions = new AvailableResolutions();
                var returned = availableResolutions.Add(sceneName, width, height);
                Assert.That(returned, Is.True, "Add should have returned true (success).");
                CheckIfResolutionAvailableForScene(availableResolutions, sceneName, width, height);
            }
        }

        [TestCase(TestName = "only scene name is specified.")]
        [Category("Scenes and Resolutions")]
        public void AddAddsSceneWithZeroResolutionIf()
        {
            var availableResolutions = new AvailableResolutions();
            var returned = availableResolutions.Add(sceneName);
            Assert.That(returned, Is.True, "Add should have returned true (success).");
            CheckIfResolutionAvailableForScene(availableResolutions, sceneName, 0, 0);
        }

        [TestCaseSource(typeof(SceneNameWithWrongResolutionSpecified), "TestCases")]
        [Category("Scenes and Resolutions")]
        public void AddAddsSceneWithZeroResolutionIf(int width, int height)
        {
            {
                var sceneNameWithResolution = string.Format("{0}.{1}x{2}", sceneName, width, height);
                var availableResolutions = new AvailableResolutions();
                var returned = availableResolutions.Add(sceneNameWithResolution);
                Assert.That(returned, Is.True, "Add should have returned true (success).");
                CheckIfResolutionAvailableForScene(availableResolutions, sceneName, 0, 0);
            }

            {
                var availableResolutions = new AvailableResolutions();
                var returned = availableResolutions.Add(sceneName, width, height);
                Assert.That(returned, Is.True, "Add should have returned true (success).");
                CheckIfResolutionAvailableForScene(availableResolutions, sceneName, 0, 0);
            }
        }

        private class SceneNameWithWrongResolutionSpecified
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(-1366, -768).SetName("scene name + resolution with both negative width and height specified.");
                    yield return new TestCaseData(-1366, 768).SetName("scene name + resolution with negative width and positive height specified.");
                    yield return new TestCaseData(1366, -768).SetName("scene name + resolution with positive width and negative height specified.");
                    yield return new TestCaseData(0, 0).SetName("scene name + resolution with both zero width and height specified.");
                    yield return new TestCaseData(0, 768).SetName("scene name + resolution with zero width and positive height specified.");
                    yield return new TestCaseData(1366, 0).SetName("scene name + resolution with positive width and zero height specified.");
                    yield return new TestCaseData(0, -768).SetName("scene name + resolution with zero width and negative height specified.");
                    yield return new TestCaseData(-1366, 0).SetName("scene name + resolution with negative width and zero height specified.");
                }
            }  
        }
       
        private void CheckIfResolutionAvailableForScene(AvailableResolutions availableResolutions, string expectedName, int expectedWidth, int expectedHeight)
        {
            var root = availableResolutions.ToXml();
            var scenes = root.Element("Scenes");
            var scene = scenes.Element("Scene");
            var name = scene.Attribute("name").Value;
            Assert.That(name, Is.EqualTo(expectedName), string.Format("Available resolutions should have contained a scene with name {0}.", expectedName));
            var resolution = new Resolution();
            resolution.FromXml(scene.Element("Resolution"));
            Assert.That(resolution.Width, Is.EqualTo(expectedWidth), string.Format("Resolution width for the scene should have been equal to {0}.", expectedWidth));
            Assert.That(resolution.Height, Is.EqualTo(expectedHeight), string.Format("Resolution height for the scene should have been equal to {0}.", expectedHeight));
        }

        [TestCase(TestName = "no resolutions available for the specified scene")]
        [Category("Scenes and Resolutions")]
        public void GetBestReturnsOnlySceneNameIf()
        {
            var availableResolutions = new AvailableResolutions();
            var returned = availableResolutions.GetBest(sceneName, RandomGameResolution, RandomSceneMode);
            Assert.That(returned, Is.EqualTo(sceneName), string.Format("GetBest should have returned '{0}'.", sceneName));
        }

        [TestCase(".0x0", TestName = "only zero resolution is available for the specified scene.")]
        [Category("Scenes and Resolutions")]
        public void GetBestReturnsOnlySceneNameIf(string resolution)
        {
            var sceneNameWithResolution = sceneName + resolution;
            var availableResolutions = new AvailableResolutions();
            availableResolutions.Add(sceneNameWithResolution);
            var returned = availableResolutions.GetBest(sceneName, RandomGameResolution, RandomSceneMode);
            Assert.That(returned, Is.EqualTo(sceneName), string.Format("GetBest should have returned {0}.", sceneName));
        } 

        [TestCase(".1366x768", TestName = "only one positive resolution is available for the specified scene.")]
        [Category("Scenes and Resolutions")]
        public void GetBestReturnsSceneNameWithResolutionIf(string resolution)
        {
            var sceneNameWithResolution = sceneName + resolution;
            var availableResolutions = new AvailableResolutions();
            availableResolutions.Add(sceneNameWithResolution);
            var returned = availableResolutions.GetBest(sceneName, RandomGameResolution, RandomSceneMode);
            Assert.That(returned, Is.EqualTo(sceneNameWithResolution), string.Format("GetBest should have returned {0}.", sceneNameWithResolution));
        } 

        [TestCase(TestName = "multiple positive resolution are available for the specified scene.")]
        [Category("Scenes and Resolutions")]
        public void GetBestReturnsSceneNameWithResolutionIf()
        {
            var availableResolutions = new AvailableResolutions();
            availableResolutions.Add(sceneName + ".1366x768");
            availableResolutions.Add(sceneName + ".320x480");
            availableResolutions.Add(sceneName + ".640x480");
            var returned = availableResolutions.GetBest(sceneName, RandomGameResolution, RandomSceneMode);
            Assert.That(returned, Is.Not.EqualTo(sceneName), string.Format("GetBest should not have returned {0}.", sceneName));
        } 

        [TestCase(TestName = "SceneMode.Fill is specified and > 1 resolutions available.")]
        [Category("Scenes and Resolutions")]
        public void GetBestPassesGameResolutionToFillResolutionComparerAndCallsItsCompareIf()
        {
            var availableResolutions = new AvailableResolutions();
            availableResolutions.Add(sceneName + ".1366x768");
            availableResolutions.Add(sceneName + ".768x1366");
            
            var comparer = Substitute.For<IResolutionComparer>();
            availableResolutions.FillComparer = comparer;
            
            var gameResolution = RandomGameResolution;
            availableResolutions.GetBest(sceneName, gameResolution, SceneMode.Fill);
            
            comparer.Received(1).GameResolution = gameResolution;
            comparer.Received().Compare(Arg.Any<Resolution>(), Arg.Any<Resolution>());
        } 
        
        [TestCase(TestName = "SceneMode.Fit is specified and > 1 resolutions available.")]
        [Category("Scenes and Resolutions")]
        public void GetBestDoesntUseFillResolutionComparerIf()
        {
            var availableResolutions = new AvailableResolutions();
            availableResolutions.Add(sceneName + ".1366x768");
            availableResolutions.Add(sceneName + ".768x1366");
            
            var comparer = Substitute.For<IResolutionComparer>();
            availableResolutions.FillComparer = comparer;
            
            var gameResolution = RandomGameResolution;
            availableResolutions.GetBest(sceneName, gameResolution, SceneMode.Fit);
            
            comparer.Received(0).GameResolution = gameResolution;
            comparer.Received(0).Compare(Arg.Any<Resolution>(), Arg.Any<Resolution>());
        } 
        
        [TestCase(TestName = "SceneMode.Fit is specified and > 1 resolutions available.")]
        [Category("Scenes and Resolutions")]
        public void GetBestPassesGameResolutionToFitResolutionComparerAndCallsItsCompareIf()
        {
            var availableResolutions = new AvailableResolutions();
            availableResolutions.Add(sceneName + ".1366x768");
            availableResolutions.Add(sceneName + ".768x1366");
            
            var comparer = Substitute.For<IResolutionComparer>();
            availableResolutions.FitComparer = comparer;
            
            var gameResolution = RandomGameResolution;
            availableResolutions.GetBest(sceneName, gameResolution, SceneMode.Fit);
            
            comparer.Received(1).GameResolution = gameResolution;
            comparer.Received().Compare(Arg.Any<Resolution>(), Arg.Any<Resolution>());
        } 
        
        [TestCase(TestName = "SceneMode.Fill is specified and > 1 resolutions available.")]
        [Category("Scenes and Resolutions")]
        public void GetBestDoesntUseFitResolutionComparerIf()
        {
            var availableResolutions = new AvailableResolutions();
            availableResolutions.Add(sceneName + ".1366x768");
            availableResolutions.Add(sceneName + ".768x1366");
            
            var comparer = Substitute.For<IResolutionComparer>();
            availableResolutions.FitComparer = comparer;
            
            var gameResolution = RandomGameResolution;
            availableResolutions.GetBest(sceneName, gameResolution, SceneMode.Fill);
            
            comparer.Received(0).GameResolution = gameResolution;
            comparer.Received(0).Compare(Arg.Any<Resolution>(), Arg.Any<Resolution>());
        } 

        [TestCaseSource(typeof(WrongXElement), "TestCases")]
        [Category("Scenes and Resolutions")]
        public void FromXmlReturnsFalseIf(XElement xml)
        {
            var availableResolutions = new AvailableResolutions();
            var returned = availableResolutions.FromXml(xml);
            Assert.That(returned, Is.False, "FromXml should have returned false (failure)");
        }

        private class WrongXElement
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(null).SetName("null specified.");
                    yield return new TestCaseData(new XElement("WrongElement")).SetName("'ScenesAndResolutions' element does not exist.");
                    yield return new TestCaseData(new XElement("ScenesAndResolutions")).SetName("'ScenesAndResolutions' element does exist, but 'Scenes' and 'Resolutions' elements does not exist.");
                    yield return new TestCaseData(new XElement("ScenesAndResolutions", new XElement("Scenes"))).SetName("'ScenesAndResolutions' and 'Scenes' elements does exist, but 'Resolutions' element does not exist.");
                    yield return new TestCaseData(new XElement("ScenesAndResolutions", new XElement("Resolutions"))).SetName("'ScenesAndResolutions' and 'Resolutions' elements does exist, but 'Scenes' element does not exist.");
                    yield return new TestCaseData(new XElement("ScenesAndResolutions", new XElement("Scenes", new XElement("Scene")), new XElement("Resolutions"))).SetName("'ScenesAndResolutions', 'Scenes' and 'Resolutions' elements does exist, but 'Scenes' contains incorrect 'Scene' element.");
                    yield return new TestCaseData(new XElement("ScenesAndResolutions", new XElement("Scenes", new XElement("Scene", new XAttribute("name", sceneName), new XElement("Resolution"))), new XElement("Resolutions"))).SetName("'ScenesAndResolutions', 'Resolutions' and 'Scenes'+'Scene' elements does exist, but 'Scene' contains incorrect 'Resolution' element.");
                    yield return new TestCaseData(new XElement("ScenesAndResolutions", new XElement("Scenes"), new XElement("Resolutions", new XElement("Resolution")))).SetName("'ScenesAndResolutions', 'Scenes' and 'Resolutions' elements does exist, but 'Resolutions' contains incorrect 'Resolution' element.");
                }
            }
        }

        [TestCase(TestName = "'ScenesAndResolutions' contains 'Scenes' and 'Resolutions' elements and they are empty.")]
        [Category("Scenes and Resolutions")]
        public void FromXmlReturnsTrueAndReadsNoResolutionsIf()
        {
            var xml = new XElement("ScenesAndResolutions", 
                                   new XElement("Scenes"), 
                                   new XElement("Resolutions"));

            var availableResolutions = new AvailableResolutions();
            var returned = availableResolutions.FromXml(xml);
            Assert.That(returned, Is.True, "FromXml should have returned true (success)");
            var resultingSceneName = availableResolutions.GetBest(sceneName, RandomGameResolution, RandomSceneMode);
            Assert.That(resultingSceneName, Is.EqualTo(sceneName), string.Format("GetBest should have returned {0}.", sceneName));
        }

        [TestCase(TestName = "'ScenesAndResolutions' contains 'Scenes' and 'Resolutions' elements; 'Scenes' contains valid 'Scene' with 'Resolution'; 'Resolutions' contains valid 'Resolution' element.")]
        [Category("Scenes and Resolutions")]
        public void FromXmlReturnsTrueAndReadsSomeResolutionsIf()
        {
            var width = 1024;
            var height = 768;
            var expected = string.Format("{0}.{1}x{2}", sceneName, width, height);

            var resolution = new XElement("Resolution", 
                                          new XAttribute("w", width), 
                                          new XAttribute("h", height));

            var scene = new XElement("Scene", 
                                     new XAttribute("name", sceneName), 
                                     resolution);

            var xml = new XElement("ScenesAndResolutions", 
                                   new XElement("Scenes", scene), 
                                   new XElement("Resolutions", resolution));

            var availableResolutions = new AvailableResolutions();
            var returned = availableResolutions.FromXml(xml);
            Assert.That(returned, Is.True, "FromXml should have returned true (success)");
            var resultingSceneName = availableResolutions.GetBest(sceneName, RandomGameResolution, RandomSceneMode);
            Assert.That(resultingSceneName, Is.EqualTo(expected), string.Format("GetBest should have returned {0}.", expected));
        }

        [TestCase(TestName = "no scenes and resolutions are available.")]
        [Category("Scenes and Resolutions")]
        public void ToXmlReturnsXElementOfValidStructureButWithoutScenesAndResolutionsIf()
        {
            var availableResolutions = new AvailableResolutions();

            var xml = availableResolutions.ToXml();
            Assert.That(xml, Is.Not.Null, "ToXml should not have returned null.");
            Assert.That(xml.Name.LocalName, Is.EqualTo("ScenesAndResolutions"), "XML root should have been 'ScenesAndResolutions'.");

            var scenes = xml.Element("Scenes");
            Assert.That(scenes, Is.Not.Null, "XML should have contained 'Scenes'.");
            var scenesChildren = scenes.Elements();
            Assert.That(scenesChildren.Count(), Is.EqualTo(0), "'Scenes' should not have contained children.");

            var resolutions = xml.Element("Resolutions");
            Assert.That(resolutions, Is.Not.Null, "XML should have contained 'Resolutions'.");
            var resolutionsChildren = resolutions.Elements();
            Assert.That(resolutionsChildren.Count(), Is.EqualTo(0), "'Resolutions' should not have contained children.");
        }

        [TestCase(TestName = "some scenes and resolutions are available.")]
        [Category("Scenes and Resolutions")]
        public void ToXmlReturnsXElementOfValidStructureWithScenesAndResolutionsIf()
        {
            var availableResolutions = new AvailableResolutions();
            availableResolutions.Add(sceneName, 1024, 768);
            availableResolutions.Add("OtherSceneName", 1136, 640);

            var xml = availableResolutions.ToXml();
            Assert.That(xml, Is.Not.Null, "ToXml should not have returned null.");
            Assert.That(xml.Name.LocalName, Is.EqualTo("ScenesAndResolutions"), "XML root should have been 'ScenesAndResolutions'.");

            var scenes = xml.Element("Scenes");
            Assert.That(scenes, Is.Not.Null, "XML should have contained 'Scenes'.");
            var scenesChildren = scenes.Elements("Scene");
            int count = 0;
            foreach (var scene in scenesChildren)
            {
                var sceneChildren = scene.Elements("Resolution");
                Assert.That(sceneChildren.Count(), Is.EqualTo(1), "'Scene' should have contained 1 'Resolution' child.");
                count++;
            }
            Assert.That(count, Is.EqualTo(2), "'Scenes' should have contained 2 'Scene' children.");

            var resolutions = xml.Element("Resolutions");
            Assert.That(resolutions, Is.Not.Null, "XML should have contained 'Resolutions'.");
            var resolutionsChildren = resolutions.Elements("Resolution");
            Assert.That(resolutionsChildren.Count(), Is.EqualTo(2), "'Resolutions' should have contained 2 'Resolution' children.");
        }

        [TestCase(TestName = "no scenes and resolutions are available.")]
        [Category("Scenes and Resolutions")]
        public void ToStringReturnsStringContainingXmlOfValidStructureButWithoutScenesAndResolutionsIf()
        {
            var availableResolutions = new AvailableResolutions();
            
            var @string = availableResolutions.ToString();
            Assert.That(@string, Is.Not.Null, "ToString should not have returned null.");
            
            var xml = XElement.Parse(@string);
            Assert.That(xml.Name.LocalName, Is.EqualTo("ScenesAndResolutions"), "XML root should have been 'ScenesAndResolutions'.");

            var scenes = xml.Element("Scenes");
            Assert.That(scenes, Is.Not.Null, "XML should have contained 'Scenes'.");
            var scenesChildren = scenes.Elements();
            Assert.That(scenesChildren.Count(), Is.EqualTo(0), "'Scenes' should not have contained children.");
            
            var resolutions = xml.Element("Resolutions");
            Assert.That(resolutions, Is.Not.Null, "XML should have contained 'Resolutions'.");
            var resolutionsChildren = resolutions.Elements();
            Assert.That(resolutionsChildren.Count(), Is.EqualTo(0), "'Resolutions' should not have contained children.");
        }

        [TestCase(TestName = "some scenes and resolutions are available.")]
        [Category("Scenes and Resolutions")]
        public void ToStringReturnsStringContainingXmlOfValidStructureWithScenesAndResolutionsIf()
        {
            var availableResolutions = new AvailableResolutions();
            availableResolutions.Add(sceneName, 1024, 768);
            availableResolutions.Add("OtherSceneName", 1136, 640);
            
            var @string = availableResolutions.ToString();
            Assert.That(@string, Is.Not.Null, "ToString should not have returned null.");

            var xml = XElement.Parse(@string);
            Assert.That(xml.Name.LocalName, Is.EqualTo("ScenesAndResolutions"), "XML root should have been 'ScenesAndResolutions'.");
            
            var scenes = xml.Element("Scenes");
            Assert.That(scenes, Is.Not.Null, "XML should have contained 'Scenes'.");
            var scenesChildren = scenes.Elements("Scene");
            int count = 0;
            foreach (var scene in scenesChildren)
            {
                var sceneChildren = scene.Elements("Resolution");
                Assert.That(sceneChildren.Count(), Is.EqualTo(1), "'Scene' should have contained 1 'Resolution' child.");
                count++;
            }
            Assert.That(count, Is.EqualTo(2), "'Scenes' should have contained 2 'Scene' children.");
            
            var resolutions = xml.Element("Resolutions");
            Assert.That(resolutions, Is.Not.Null, "XML should have contained 'Resolutions'.");
            var resolutionsChildren = resolutions.Elements("Resolution");
            Assert.That(resolutionsChildren.Count(), Is.EqualTo(2), "'Resolutions' should have contained 2 'Resolution' children.");
        }

        [TestCase(TestName = "no scenes and resolutions are available.")]
        [Category("Scenes and Resolutions")]
        public void ResolutionsListToStringReturnsEmptyStringIf()
        {
            var availableResolutions = new AvailableResolutions();
            
            var @string = availableResolutions.ResolutionsListToString();
            Assert.That(@string, Is.Not.Null, "ResolutionsListToString should not have returned null.");
            Assert.That(@string.Length, Is.EqualTo(0), "ResolutionsListToString should be empty.");
        }

        [TestCase(TestName = "some scenes and resolutions are available.")]
        [Category("Scenes and Resolutions")]
        public void ResolutionsListToStringReturnsStringContainingAllAvailableResolutionsIf()
        {
            var resolutionA = "1024x768";
            var resolutionB = "1136x640";

            var availableResolutions = new AvailableResolutions();
            availableResolutions.Add(string.Format("{0}.{1}", sceneName, resolutionA));
            availableResolutions.Add(string.Format("OtherSceneName.{0}", resolutionB));

            var @string = availableResolutions.ResolutionsListToString();
            Assert.That(@string, Is.Not.Null, "ResolutionsListToString should not have returned null.");
            Assert.That(@string.Contains(resolutionA), string.Format("ResolutionsListToString should contain {0}.", resolutionA));
            Assert.That(@string.Contains(resolutionB), string.Format("ResolutionsListToString should contain {0}.", resolutionB));
        }
    }
}
