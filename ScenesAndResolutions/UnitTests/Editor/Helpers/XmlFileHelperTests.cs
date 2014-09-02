//-----------------------------------------------------------------------
// <copyright file="XmlFileHelperTests.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using System.IO;
    using System.Xml.Linq;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEditor;
    
    [TestFixture]
    internal class XmlFileHelperTests : TestsBase
    {
        [TestCase(TestName = "valid file name specified.")]
        [Category("Scenes and Resolutions")]
        public void SaveReturnsTrueAndSavesFileIf()
        {
            // Prepare.
            var oldFileName = XmlFile.FileName;
            XmlFile.FileName = "TestScenesAndResolutionsFile.xml";
            
            var xml = new XElement("TestElement");
            var xmlFile = new XmlFile();
            var returned = xmlFile.Save(xml);
            var loaded = xmlFile.Load();
            
            // Cleanup before checks.
            xmlFile.Delete();
            XmlFile.FileName = oldFileName;
            AssetDatabase.Refresh();
            
            Assert.That(returned, Is.True, "Save should have returned true (success).");
            Assert.That(loaded.Name.LocalName, Is.EqualTo("TestElement"), "Element name should have been 'TestElement'.");
        }
        
        [TestCase(null, TestName = "file name is null.")]
        [TestCase("", TestName = "file name is empty.")]
        [Category("Scenes and Resolutions")]
        public void SaveReturnsFalseAndFailsToSaveFileIf(string fileName)
        {
            // Prepare.
            var oldFileName = XmlFile.FileName;
            XmlFile.FileName = fileName;
            
            var xml = new XElement("TestElement");
            var xmlFile = new XmlFile();
            var returned = xmlFile.Save(xml);
            var loaded = xmlFile.Load();
            
            // Cleanup before checks.
            XmlFile.FileName = oldFileName;
            
            Assert.That(returned, Is.False, "Save should have returned false (failure).");
            Assert.That(loaded, Is.Null, "Loaded XElement should have been null.");
        }
        
        [TestCase(TestName = ("file exists."))]
        [Category("Scenes and Resolutions")]
        public void DeleteDeletesFileIf()
        {
            // Prepare.
            var oldFileName = XmlFile.FileName;
            XmlFile.FileName = "TestScenesAndResolutionsFile.xml";
            
            var xml = new XElement("TestElement");
            var xmlFile = new XmlFile();
            xmlFile.Save(xml);
            
            var path = GetFilePath();
            var existsBefore = File.Exists(path);
            xmlFile.Delete();
            var existsAfter = File.Exists(path);
            
            // Cleanup before check,
            XmlFile.FileName = oldFileName;
            AssetDatabase.Refresh();
            
            Assert.That(existsBefore, Is.True, "File should have existed before deleting.");
            Assert.That(existsAfter, Is.Not.True, "File should not have existed after deleting.");
        }

        private string GetFilePath()
        {
            var directory = Path.Combine(Application.dataPath, "ScenesAndResolutions/Resources");
            return Path.Combine(directory, XmlFile.FileName);
        }
    }
}
