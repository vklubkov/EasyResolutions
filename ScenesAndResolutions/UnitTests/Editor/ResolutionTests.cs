//-----------------------------------------------------------------------
// <copyright file="ResolutionTests.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ScenesAndResolutions.Tests
{
    using System.Collections;
    using System.Xml.Linq;
    using NUnit.Framework;

    [TestFixture]
    internal class ResolutionTests : TestsBase
    {
        private static System.Random random = new System.Random();

        [TestCase(TestName = "creates zero resolution.")]
        [Category("Scenes and Resolutions")]
        public void DefaultConstructor()
        {
            var resolution = new Resolution();
            Assert.That(resolution.IsZero(), "Resolution should have been zero.");
            Assert.That(resolution.Width, Is.EqualTo(0), "Resolution width should have been 0.");
            Assert.That(resolution.Height, Is.EqualTo(0), "Resolution height should have been 0.");
        }

        [TestCaseSource(typeof(TestCases), "ZeroAndPositiveWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void WidthAndHeightConstructorCreatesResolutionUsingSpecifiedWithAndHeightIf(int width, int height)
        {
            var resolution = new Resolution(width, height);
            Assert.That(resolution.Width, Is.EqualTo(width), string.Format("Resolution width should have been {0}.", width));
            Assert.That(resolution.Height, Is.EqualTo(height), string.Format("Resolution height should have been {0}.", height));
        }

        [TestCaseSource(typeof(TestCases), "ZeroAndNegativeWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void WidthAndHeightConstructorCreatesZeroResolutionIf(int width, int height)
        {
            var resolution = new Resolution(width, height);
            Assert.That(resolution.IsZero(), "Resolution should have been zero.");
            Assert.That(resolution.Width, Is.EqualTo(0), "Resolution width should have been 0.");
            Assert.That(resolution.Height, Is.EqualTo(0), "Resolution height should have been 0.");
        }

        [TestCaseSource(typeof(TestCases), "ZeroAndPositiveWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void SetSetsSpecifiedWidthAndHeightIf(int width, int height)
        {
            var resolution = new Resolution();
            resolution.Set(width, height);
            Assert.That(resolution.Width, Is.EqualTo(width), string.Format("Resolution width should have been {0}.", width));
            Assert.That(resolution.Height, Is.EqualTo(height), string.Format("Resolution height should have been {0}.", height));
        }
        
        [TestCaseSource(typeof(TestCases), "ZeroAndNegativeWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void SetSetsZeroWidthAndHeightIf(int width, int height)
        {
            var resolution = new Resolution();
            resolution.Set(width, height);
            Assert.That(resolution.IsZero(), "Resolution should have been zero.");
            Assert.That(resolution.Width, Is.EqualTo(0), "Resolution width should have been 0.");
            Assert.That(resolution.Height, Is.EqualTo(0), "Resolution height should have been 0.");
        }

        [TestCaseSource(typeof(TestCases), "StringContainingZeroAndPositiveWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromExtensionSetsWidthAndHeightAndReturnsTrueIfNoDotsPrependedAnd(string resolutionString,
                                                                                      int expectedWidth, 
                                                                                      int expecedHeight)
        {
            var resolution = new Resolution();
            var returned = resolution.FromExtension(resolutionString);
            Assert.That(returned, Is.True, "FromExtension should have returned true (success).");
            Assert.That(resolution.Width, Is.EqualTo(expectedWidth), string.Format("Resolution width should have been {0}.", expectedWidth));
            Assert.That(resolution.Height, Is.EqualTo(expecedHeight), string.Format("Resolution height should have been {0}.", expecedHeight));
        }

        [TestCaseSource(typeof(TestCases), "StringContainingZeroAndPositiveWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromExtensionSetsWidthAndHeightAndReturnsTrueIfSingleDotPrependedAnd(string resolutionString, 
                                                                                         int expectedWidth, 
                                                                                         int expecedHeight)
        {
            var resolution = new Resolution();
            resolutionString = prependDotsTo(resolutionString, 1);
            var returned = resolution.FromExtension(resolutionString);
            Assert.That(returned, Is.True, "FromExtension should have returned true (success).");
            Assert.That(resolution.Width, Is.EqualTo(expectedWidth), string.Format("Resolution width should have been {0}.", expectedWidth));
            Assert.That(resolution.Height, Is.EqualTo(expecedHeight), string.Format("Resolution height should have been {0}.", expecedHeight));
        }

        [TestCaseSource(typeof(TestCases), "StringContainingZeroAndPositiveWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromExtensionSetsWidthAndHeightAndReturnsTrueIfMultipleDotsPrependedAnd(string resolutionString, 
                                                                                         int expectedWidth, 
                                                                                         int expecedHeight)
        {
            var resolution = new Resolution();
            resolutionString = prependDotsTo(resolutionString);
            var returned = resolution.FromExtension(resolutionString);
            Assert.That(returned, Is.True, "FromExtension should have returned true (succes).");
            Assert.That(resolution.Width, Is.EqualTo(expectedWidth), string.Format("Resolution width should have been {0}.", expectedWidth));
            Assert.That(resolution.Height, Is.EqualTo(expecedHeight), string.Format("Resolution height should have been {0}.", expecedHeight));
        }

        [TestCaseSource(typeof(TestCases), "StringContainingZeroAndNegativeWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromExtensionSetsZeroWidthAndHeightAndReturnsTrueIfNoDotsPrependedAnd(string resolutionString)
        {
            var resolution = new Resolution();
            var returned = resolution.FromExtension(resolutionString);
            Assert.That(returned, Is.True, "FromExtension should have returned true (success).");
            Assert.That(resolution.IsZero(), "Resolution should have been zero.");
            Assert.That(resolution.Width, Is.EqualTo(0), "Resolution width should have been 0.");
            Assert.That(resolution.Height, Is.EqualTo(0), "Resolution height should have been 0.");
        }

        [TestCaseSource(typeof(TestCases), "StringContainingZeroAndNegativeWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromExtensionSetsZeroWidthAndHeightAndReturnsTrueIfSingleDotPrependedAnd(string resolutionString)
        {
            var resolution = new Resolution();
            resolutionString = prependDotsTo(resolutionString, 1);
            var returned = resolution.FromExtension(resolutionString);
            Assert.That(returned, Is.True, "FromExtension should have returned true (success).");
            Assert.That(resolution.IsZero(), "Resolution should have been zero.");
            Assert.That(resolution.Width, Is.EqualTo(0), "Resolution width should have been 0.");
            Assert.That(resolution.Height, Is.EqualTo(0), "Resolution height should have been 0.");
        }

        [TestCaseSource(typeof(TestCases), "StringContainingZeroAndNegativeWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromExtensionSetsZeroWidthAndHeightAndReturnsTrueIfMultipleDotsPrependedAnd(string resolutionString)
        {
            var resolution = new Resolution();
            resolutionString = prependDotsTo(resolutionString);
            var returned = resolution.FromExtension(resolutionString);
            Assert.That(returned, Is.True, "FromExtension should have returned true (success).");
            Assert.That(resolution.IsZero(), "Resolution should have been zero.");
            Assert.That(resolution.Width, Is.EqualTo(0), "Resolution width should have been 0.");
            Assert.That(resolution.Height, Is.EqualTo(0), "Resolution height should have been 0.");
        }

        [TestCaseSource(typeof(TestCases), "StringNotContaingWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromExtensionChangesNothingAndReturnsFalseIfNoDotsPrependedAnd(string resolutionString)
        {
            int width = 2560;
            int height = 1440;
            var resolution = new Resolution(width, height);
            var returned = resolution.FromExtension(resolutionString);
            Assert.That(returned, Is.False, "FromExtension should have returned false (failure).");
            Assert.That(resolution.Width, Is.EqualTo(width), string.Format("Resolution width should have remained unchanged ({0}).", width));
            Assert.That(resolution.Height, Is.EqualTo(height), string.Format("Resolution height should have remained unchanged ({0}).", height));
        }

        [TestCaseSource(typeof(TestCases), "StringNotContaingWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromExtensionChangesNothingAndReturnsFalseIfSingleDotPrependedAnd(string resolutionString)
        {
            int width = 2560;
            int height = 1440;
            var resolution = new Resolution(width, height);
            resolutionString = prependDotsTo(resolutionString, 1);
            var returned = resolution.FromExtension(resolutionString);
            Assert.That(returned, Is.False, "FromExtension should have returned false (failure).");
            Assert.That(resolution.Width, Is.EqualTo(width), string.Format("Resolution width should have remained unchanged ({0}).", width));
            Assert.That(resolution.Height, Is.EqualTo(height), string.Format("Resolution height should have remained unchanged ({0}).", height));
        }

        [TestCaseSource(typeof(TestCases), "StringNotContaingWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromExtensionChangesNothingAndReturnsFalseIfMultipleDotsPrependedAnd(string resolutionString)
        {
            int width = 2560;
            int height = 1440;
            var resolution = new Resolution(width, height);
            resolutionString = prependDotsTo(resolutionString);
            var returned = resolution.FromExtension(resolutionString);
            Assert.That(returned, Is.False, "FromExtension should have returned false (failure).");
            Assert.That(resolution.Width, Is.EqualTo(width), string.Format("Resolution width should have remained unchanged ({0}).", width));
            Assert.That(resolution.Height, Is.EqualTo(height), string.Format("Resolution height should have remained unchanged ({0}).", height));
        }

        [TestCase(0, 0, "", TestName = "empty string, if resolution is zero.")]
        [TestCase(1366, 768, ".1366x768", TestName = "resolution as string with a single dot prepended, if resolution is non-zero.")]
        [Category("Scenes and Resolutions")]
        public void ToExtensionReturns(int width, int height, string expected)
        {
            var resolution = new Resolution(width, height);
            var returned = resolution.ToExtension();
            Assert.That(returned, Is.EqualTo(expected), string.Format("ToExtension should have returned '{0}'.", expected));
        }

        [TestCaseSource(typeof(TestCases), "StringContainingZeroAndPositiveWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromStringSetsWidthAndHeightAndReturnsTrueIf(string resolutionString, int expectedWidth, int expectedHeight)
        {
            var resolution = new Resolution();
            var returned = resolution.FromString(resolutionString);
            Assert.That(returned, "FromString should have returned true (success).");
            Assert.That(resolution.Width, Is.EqualTo(expectedWidth), string.Format("Resolution width should have been {0}.", expectedWidth));
            Assert.That(resolution.Height, Is.EqualTo(expectedHeight), string.Format("Resolution height should have been {0}.", expectedHeight));
        }

        [TestCaseSource(typeof(TestCases), "StringContainingZeroAndNegativeWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromStringSetsZeroWidthAndHeightAndReturnsTrueIf(string resolutionString)
        {
            var resolution = new Resolution();
            var returned = resolution.FromString(resolutionString);
            Assert.That(returned, Is.True, "FromString should have returned true (success).");
            Assert.That(resolution.IsZero(), "Resolution should have been zero.");
            Assert.That(resolution.Width, Is.EqualTo(0), "Resolution width should have been 0.");
            Assert.That(resolution.Height, Is.EqualTo(0), "Resolution height should have been 0.");
        }

        [TestCaseSource(typeof(TestCases), "StringNotContaingWidthAndHeight")]
        public void FromStringChangesNothingAndReturnsFalseIf(string resolutionString)
        {
            int width = 2560;
            int height = 1440;
            var resolution = new Resolution(width, height);
            var returned = resolution.FromString(resolutionString);
            Assert.That(returned, Is.False, "FromString should have returned false (failure).");
            Assert.That(resolution.Width, Is.EqualTo(width), string.Format("Resolution width should have remained unchanged ({0}).", width));
            Assert.That(resolution.Height, Is.EqualTo(height), string.Format("Resolution height should have remained unchanged ({0}).", height));
        }

        [TestCase(0, 0, "0x0", TestName = "if resolution is zero.")]
        [TestCase(1366, 768, "1366x768", TestName = "if resolution is non-zero.")]
        [Category("Scenes and Resolutions")]
        public void ToStringReturnsResolutionAsString(int width, int height, string expected)
        {
            var resolution = new Resolution(width, height);
            var returned = resolution.ToString();
            Assert.That(returned, Is.EqualTo(expected), string.Format("ToString should have returned '{0}'.", expected));
        }

        [TestCase(0, 0, 0f, TestName = "0f, if both width and height are 0.")]
        [TestCase(480, 480, 1f, TestName = "1f, if width and height are equal.")]
        [TestCase(1366, 768, 1.778645833333333f, TestName = "(float)width/(float)height, if width > height.")]
        [TestCase(768, 1024, 1.333333333333333f, TestName = "(float)height/(float)width, if width < height.")]
        [Category("Scenes and Resolutions")]
        public void GetAspectRatioFactorReturns(int width, int height, float expected)
        {
            var resolution = new Resolution(width, height);
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
            var resolution = new Resolution(width, height);
            Assert.That(resolution.IsLandscape(), "IsLandscape should have returned true.");
        }
        
        [TestCase(768, 1024, TestName = "height > width.")]
        [Category("Scenes and Resolutions")]
        public void IsLandscapeReturnsFalseIf(int width, int height)
        {
            var resolution = new Resolution(width, height);
            Assert.That(resolution.IsLandscape(), Is.Not.True, "IsLandscape should have returned false.");
        }

        [TestCase(0, 0, TestName = "both width and height are 0.")]
        [TestCase(480, 480, TestName = "width and height are equal.")]
        [TestCase(768, 1024, TestName = "height > width.")]
        [Category("Scenes and Resolutions")]
        public void IsPortraitReturnsTrueIf(int width, int height)
        {
            var resolution = new Resolution(width, height);
            Assert.That(resolution.IsPortrait(), "IsPortrait should have returned true.");
        }
        
        [TestCase(1366, 768, TestName = "width > height.")]
        [Category("Scenes and Resolutions")]
        public void IsPortraitReturnsFalseIf(int width, int height)
        {
            var resolution = new Resolution(width, height);
            Assert.That(resolution.IsPortrait(), Is.Not.True, "IsPortrait should have returned false.");
        }

        [TestCase(TestName = "resolution has zero width and height.")]
        [Category("Scenes and Resolutions")]
        public void IsZeroReturnsTrueIf()
        {
            var resolution = new Resolution();
            Assert.That(resolution.IsZero(), "IsZero should have returned true.");
            Assert.That(resolution.Width, Is.EqualTo(0), "Resolution width should have been 0.");
            Assert.That(resolution.Height, Is.EqualTo(0), "Resolution height should have been 0.");
        }

        [TestCase(1366, 768, TestName = "resolution is non-zero.")]
        [Category("Scenes and Resolutions")]
        public void IsZeroReturnsFalseIf(int width, int height)
        {
            var resolution = new Resolution(width, height);
            Assert.That(resolution.IsZero(), Is.Not.True, "IsZero should have returned false.");
            Assert.That(resolution.Width, Is.EqualTo(width), string.Format("Resolution width should have been {0}.", width));
            Assert.That(resolution.Height, Is.EqualTo(height), string.Format("Resolution height should have been {0}.", height));
        }

        [TestCaseSource(typeof(TestCases), "TwoEqualResolutions")]
        [Category("Scenes and Resolutions")]
        public void GetHashCodeReturnsEqualHashCodesForTwoResolutionsIf(int widthA, int heightA, int widthB, int heightB)
        {
            var resolutionA = new Resolution(widthA, heightA);
            var resolutionB = new Resolution(widthB, heightB);
            var hashA = resolutionA.GetHashCode();
            var hashB = resolutionB.GetHashCode();
            Assert.That(hashA, Is.EqualTo(hashB), "GetHashCode should should have returned equal hash codes for both resolutions.");
        }

        [TestCaseSource(typeof(TestCases), "TwoNotEqualResolutions")]
        [Category("Scenes and Resolutions")]
        public void GetHashCodeReturnsDifferentHashCodes(int widthA, int heightA, int widthB, int heightB)
        {
            var resolutionA = new Resolution(widthA, heightA);
            var resolutionB = new Resolution(widthB, heightB);
            var hashA = resolutionA.GetHashCode();
            var hashB = resolutionB.GetHashCode();
            Assert.That(hashA, Is.Not.EqualTo(hashB), "GetHashCode should should have returned different hash codes for two resolutions.");
        }

        [TestCaseSource(typeof(TestCases), "TwoEqualResolutions")]
        [Category("Scenes and Resolutions")]
        public void EqualsReturnsTrue(int widthA, int heightA, int widthB, int heightB)
        {
            var resolutionA = new Resolution(widthA, heightA);
            var resolutionB = new Resolution(widthB, heightB);
            var returned = resolutionA.Equals(resolutionB);
            Assert.That(returned, Is.True, "Equals should have returned true.");
        }

        [TestCaseSource(typeof(TestCases), "TwoNotEqualResolutions")]
        [Category("Scenes and Resolutions")]
        public void EqualsReturnsFalse(int widthA, int heightA, int widthB, int heightB)
        {
            var resolutionA = new Resolution(widthA, heightA);
            var resolutionB = new Resolution(widthB, heightB);
            var returned = resolutionA.Equals(resolutionB);
            Assert.That(returned, Is.False, "Equals should have returned false.");
        }

        [TestCaseSource(typeof(TestCases), "ZeroAndPositiveWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromXmlReadsResolutionFromValidXElementIf(int width, int height)
        {
            var resolutionElement = new XElement("Resolution");
            resolutionElement.SetAttributeValue("w", width);
            resolutionElement.SetAttributeValue("h", height);
            var resolution = new Resolution();
            var returned = resolution.FromXml(resolutionElement);
            Assert.That(returned, Is.True, "FromXml should have returned true (success).");
        }

        [TestCaseSource(typeof(TestCases), "ZeroAndNegativeWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void FromXmlReadsZeroResolutionFromValidXElementIf(int width, int height)
        {
            var resolutionElement = new XElement("Resolution");
            resolutionElement.SetAttributeValue("w", width);
            resolutionElement.SetAttributeValue("h", height);
            var resolution = new Resolution();
            var returned = resolution.FromXml(resolutionElement);
            Assert.That(returned, Is.True, "FromXml should have returned true (success).");
            Assert.That(resolution.IsZero(), "Resolution should have been zero.");
            Assert.That(resolution.Width, Is.EqualTo(0), string.Format("Resolution width should have been 0."));
            Assert.That(resolution.Height, Is.EqualTo(0), string.Format("Resolution height should have been 0."));
        }

        [TestCaseSource(typeof(TestCases), "XElementWithoutResolution")]
        [Category("Scenes and Resolutions")]
        public void FromXmlChangesNothingAndReturnsFalseIf(XElement resolutionElement)
        {
            int width = 2560;
            int height = 1440;
            var resolution = new Resolution(width, height);
            var returned = resolution.FromXml(resolutionElement);
            Assert.That(returned, Is.False, "FromXml should have returned false (failure).");
            Assert.That(resolution.Width, Is.EqualTo(width), string.Format("Resolution width should have remained unchanged ({0}).", width));
            Assert.That(resolution.Height, Is.EqualTo(height), string.Format("Resolution height should have remained unchanged ({0}).", height));
        }

        [TestCaseSource(typeof(TestCases), "ZeroAndPositiveWidthAndHeight")]
        [Category("Scenes and Resolutions")]
        public void ToXmlWritesResolutionToXElement(int width, int height)
        {
            var resoltuion = new Resolution(width, height);
            var element = resoltuion.ToXml();
            Assert.That(element.Name.LocalName, Is.EqualTo("Resolution"), "Element name should have been 'Resolution'.");
            var w = (int)element.Attribute("w");
            Assert.That(w, Is.EqualTo(width), "Value of width attribute should have been equal to resolution width.");
            var h = (int)element.Attribute("h");
            Assert.That(h, Is.EqualTo(height), "Value of height attribute should have been equal to resolution height.");
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

            public static IEnumerable StringContainingZeroAndPositiveWidthAndHeight
            {
                get
                {
                    yield return new TestCaseData("1366x768", 1366, 768).SetName("extension string contains positive width and height.");
                    yield return new TestCaseData("0x0", 0, 0).SetName("extension string contains zero width and height.");
                }
            }

            public static IEnumerable StringContainingZeroAndNegativeWidthAndHeight
            {
                get
                {
                    yield return new TestCaseData("-1366x-768").SetName("extension string contains negative width and height.");
                    yield return new TestCaseData("1366x-768").SetName("extension string contains positive width and negative height.");
                    yield return new TestCaseData("-1366x768").SetName("extension string contains negative width and positive height.");
                    yield return new TestCaseData("0x0").SetName("extension string contains zero width and height.");
                    yield return new TestCaseData("1366x0").SetName("extension string contains positive width and zero height.");
                    yield return new TestCaseData("0x768").SetName("extension string contains zero width and positive height.");
                    yield return new TestCaseData("-1366x0").SetName("extension string contains negative width and zero height.");
                    yield return new TestCaseData("0x-768").SetName("extension string contains zero width and negative height.");
                }
            }  

            public static IEnumerable StringNotContaingWidthAndHeight
            {
                get
                {
                    yield return new TestCaseData(null).SetName("string is null.");
                    yield return new TestCaseData(string.Empty).SetName("string is empty.");
                    yield return new TestCaseData("abcdef").SetName("string contains only non-numeric characters.");
                    yield return new TestCaseData("abcxdef").SetName("string contains only non-numeric characters separated by 'x'.");
                    yield return new TestCaseData("xxx").SetName("string contains only multiple 'x' characters.");
                    yield return new TestCaseData("1366x768x60").SetName("string contains only numbers separated by mopre than 1 'x'.");
                    yield return new TestCaseData("100500").SetName("string contains only numbers.");
                }
            }  
          
            public static IEnumerable TwoEqualResolutions
            {
                get
                {
                    yield return new TestCaseData(0, 0, 0, 0).SetName("two resolutions are zero.");
                    yield return new TestCaseData(1366, 768, 1366, 768).SetName("width of A == width of B and height of A == height of B");
                }
            }  

            public static IEnumerable TwoNotEqualResolutions
            {
                get
                {
                    yield return new TestCaseData(0, 0, 1366, 768).SetName("one resoltuion is zero and other is not.");
                    yield return new TestCaseData(768, 1366, 768, 1024).SetName("width of A == width of B and height of A != height of B.");
                    yield return new TestCaseData(1366, 768, 1024, 768).SetName("width of A != width of B and height of A == height of B.");
                    yield return new TestCaseData(1366, 768, 768, 1366).SetName("width of A != height of A, width of A == height of B and height of A == width of B.");
                    yield return new TestCaseData(1024, 768, 768, 1366).SetName("width of A != width of B != height of B and height of A == width of B.");
                    yield return new TestCaseData(1280, 1024, 720, 1280).SetName("width of A == height of B and height of A != width of B != height of B.");
                    yield return new TestCaseData(1024, 1024, 768, 768).SetName("two resolutions are square and sizes are not equal.");
                    yield return new TestCaseData(1366, 768, 1440, 2560).SetName("width of A != width of B != height of A != height of B.");
                }
            }  

            public static IEnumerable XElementWithoutResolution
            {
                get
                {
                    yield return new TestCaseData(null).SetName("XElement is null.");
                    yield return new TestCaseData(new XElement("NotAResolution")).SetName("XElement is not a Resolution element.");
                    yield return new TestCaseData(new XElement("Resolution")).SetName("XElement is a Resolution element, but has no attributes.");
                    yield return new TestCaseData(new XElement("Resolution", new XAttribute("w", 1366))).SetName("XElement is a Resolution element, but has only width attribute.");
                    yield return new TestCaseData(new XElement("Resolution", new XAttribute("h", 768))).SetName("XElement is a Resolution element, but has only height attribute.");
                    yield return new TestCaseData(new XElement("Resolution", new XAttribute("some_attributeA", "SomeValue"), new XAttribute("some_attributeB", "SomeAttribute"))).SetName("XElement is a Resolution element, but has wrong attributes.");
                }
            } 
        }

        private string prependDotsTo(string @string, int dotsCount)
        {
            if (dotsCount == 0)
            {
                return prependDotsTo(@string);
            }

            if (@string == null || dotsCount < 0)
            {
                return @string;
            }

            var dots = new string('.', dotsCount);
            return string.Format("{0}{1}", dots,  @string);
        }

        private string prependDotsTo(string @string)
        {
            if (@string == null)
            {
                return @string;
            }

            var dotsCount = random.Next(50);
            var dots = new string('.', dotsCount);
            return string.Format("{0}{1}", dots,  @string);
        }
    }
}