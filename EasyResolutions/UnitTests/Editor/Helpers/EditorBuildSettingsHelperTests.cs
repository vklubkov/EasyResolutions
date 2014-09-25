//-----------------------------------------------------------------------
// <copyright file="EditorBuildSettingsHelperTests.cs" company="https://github.com/marked-one/EasyResolutions">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions.Tests
{
    using System.Xml.Linq;
    using NUnit.Framework;
    using UnityEditor;
    
    [TestFixture]
    internal class EditorBuildSettingsHelperTests : TestsBase
    {
        [TestCase(TestName = "EditorBuildSettings is null.")]
        [Category("Easy Resolutions")]
        public void GetScenesAndResolutionsReturnsNullIf()
        {
            EditorBuildSettings editorBuildSettings = null;
            var returned = editorBuildSettings.GetScenesAndResolutions();
            Assert.That(returned, Is.Null, "GetScenesAndResolutions should have returned null.");
        }

        [TestCase(TestName = "EditorBuildSettings is not null.")]
        [Category("Easy Resolutions")]
        public void GetScenesAndResolutionsReturnsValidScenesAndResolutionsXmlIf()
        {
            var editorBuildSettings = new EditorBuildSettings();
            var returned = editorBuildSettings.GetScenesAndResolutions();
            Assert.That(returned, Is.Not.Null, "GetScenesAndResolutions should not have returned null.");
            Assert.That(returned.Name.LocalName, Is.EqualTo("EasyResolutions"), "XML root should have been 'EasyResolutions'.");
            
            var scenes = returned.Element("Scenes");
            Assert.That(scenes, Is.Not.Null, "XML should have contained 'Scenes'.");
            
            var resolutions = returned.Element("Resolutions");
            Assert.That(resolutions, Is.Not.Null, "XML should have contained 'Resolutions'.");
        }
    }
}
