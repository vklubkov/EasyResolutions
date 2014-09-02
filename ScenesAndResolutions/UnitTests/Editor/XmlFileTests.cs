//-----------------------------------------------------------------------
// <copyright file="XmlFileTests.cs" company="">
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
    internal class XmlFileTests : TestsBase
    {
        [TestCase(TestName = "file name of existing valid XML file is specified.")]
        [Category("Scenes and Resolutions")]
        public void LoadReadsFileAndReturnsValidXmlIf()
        {
            // Prepare.
            var oldFileName = XmlFile.FileName;
            XmlFile.FileName = "TestScenesAndResolutionsFile.xml";
            
            var xml = new XElement("TestElement");
            var xmlFile = new XmlFile();
            xmlFile.Save(xml);
            var loaded = xmlFile.Load();

            // Cleanup before checks.
            xmlFile.Delete();
            XmlFile.FileName = oldFileName;
            AssetDatabase.Refresh();
            
            Assert.That(loaded.Name.LocalName, Is.EqualTo("TestElement"), "Element name should have been 'TestElement'.");
        }

        [TestCase(TestName = "file name of existing file not containing XML data is specified.")]
        [Category("Scenes and Resolutions")]
        public void LoadDoesntReadFileAndReturnsNullIf()
        {
            // Prepare.
            var oldFileName = XmlFile.FileName;
            XmlFile.FileName = "TestScenesAndResolutionsFile.xml";
            this.CreateEmptyFile();

            var xmlFile = new XmlFile();
            var loaded = xmlFile.Load();

            // Cleanup before checks.
            xmlFile.Delete();
            XmlFile.FileName = oldFileName;
            AssetDatabase.Refresh();
            
            Assert.That(loaded, Is.Null, "Loaded XElement should have been null.");
        }

        [TestCase(null, TestName = "file name is null.")]
        [TestCase("", TestName = "file name is empty.")]
        [TestCase("NotExistingFile.xml", TestName = "file does not exist.")]
        [Category("Scenes and Resolutions")]
        public void LoadDoesntReadFileAndReturnsNullIf(string fileName)
        {
            // Prepare.
            var oldFileName = XmlFile.FileName;
            XmlFile.FileName = fileName;

            var xmlFile = new XmlFile();
            var loaded = xmlFile.Load();

            // Cleanup before checks.
            XmlFile.FileName = oldFileName;
            
            Assert.That(loaded, Is.Null, "Loaded XElement should have been null.");
        }
       
        private void CreateEmptyFile()
        {
            var path = GetFilePath();
            using(StreamWriter writer = new StreamWriter(path))
            {
            }
            
            var relativePath = Path.Combine("Assets/ScenesAndResolutions/Resources", XmlFile.FileName);
            AssetDatabase.ImportAsset(relativePath);
        }

        private string GetFilePath()
        {
            var directory = Path.Combine(Application.dataPath, "ScenesAndResolutions/Resources");
            return Path.Combine(directory, XmlFile.FileName);
        }
    }
}
