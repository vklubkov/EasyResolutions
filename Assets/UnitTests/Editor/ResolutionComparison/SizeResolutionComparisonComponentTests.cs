//-----------------------------------------------------------------------
// <copyright file="SizeResolutionComparisonComponentTests.cs" company="https://github.com/marked-one/EasyResolutions">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions.Tests
{
    using System.Collections;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    internal class SizeResolutionComparisonComponentTests : TestsBase
    {
        [TestCase(TestName = "default width and height comparers.")]
        [Category("Easy Resolutions")]
        public void DefaultConstructorCreates()
        {
            var component = new SizeResolutionComparisonComponent();
            Assert.That(component.WidthComparer, Is.Not.Null, "WidthComparer property should not have returned null");
            Assert.That(component.HeightComparer, Is.Not.Null, "HeightComparer property should not have returned null");
        }

        [TestCase(-1, TestName = "-1, if width comparer result is equal to height comparer result and is -1.")]
        [TestCase(0, TestName = "0, if width comparer result is equal to height comparer result and is 0.")]
        [TestCase(1, TestName = "1, if width comparer result is equal to height comparer result and is 1.")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIsEqualTo(int expected)
        {
            var comparer = Substitute.For<ISingleDimensionComparer>();
            comparer.Compare(0, 0, 0).Returns(expected);
            var component = new SizeResolutionComparisonComponent{WidthComparer = comparer, HeightComparer = comparer};
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(), 
                                             new Resolution(), 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(expected), string.Format("Compare result should have been {0}.", expected));
            comparer.Received(2).Compare(0, 0, 0);
        }

        [TestCase(0, 1, 320, 480, 1366, 768, 1024, 768, TestName="(1) game resolution is landscape && width comparer result == 0  && height comparer result != 0 && widthA < widthB.")]
        [TestCase(0, 1, 1366, 768, 1366, 768, 1024, 768, TestName="(2) game resolution is landscape && width comparer result == 0  && height comparer result != 0 && widthA == widthB.")]
        [TestCase(0, 1, 320, 480, 1366, 768, 1024, 1024, TestName="(3) game resolution is square && width comparer result == 0  && height comparer result != 0 && heightA < heightB.")]
        [TestCase(0, 1, 1366, 768, 1366, 768, 1024, 1024, TestName="(4) game resolution is square &&, width comparer result == 0  && height comparer result != 0 and heightA == heightB.")]
        [TestCase(-1, 1, 1366, 768, 1366, 768, 1024, 768, TestName="(5) game resolution is landscape && width comparer result !=0  && height comparer result is opposite to it && widthA == widthB.")]
        [TestCase(-1, 1, 1366, 768, 1366, 768, 1024, 1024, TestName="(6) game resolution is square  && width comparer result !=0  && height comparer result is opposite to it && widthA == widthB.")]
        [TestCase(1, 0, 320, 480, 1366, 768, 768, 1024, TestName="(7) game resolution is portrait && height comparer result == 0 && width comparer result != 0 && widthA < widthB.")]
        [TestCase(1, 0, 1366, 768, 1366, 768, 768, 1024, TestName="(8) game resolution is portrait && height comparer result == 0 && width comparer result != 0 && widthA == widthB.")]
        [TestCase(-1, 1, 1366, 768, 1366, 768, 768, 1024, TestName="(9) game resolution is portrait && height comparer result != 0 && width comparer result is opposite to it && heightA == heightB.")]
        [TestCase(-1, 1, 320, 480, 1366, 768, 1024, 768, TestName="(10) game resolution is landscape && width comparer result != 0 && height comparer result is opposite to it && widthA < widthB")]
        [TestCase(-1, 1, 320, 480, 1366, 768, 1024, 1024, TestName="(11) game resolution is square && width comparer result != 0 && height comparer result is opposite to it && widthA < widthB")]
        [TestCase(-1, 1, 320, 480, 1366, 768, 768, 1024, TestName="(12) game resolution is portrait && height comparer result != 0 && width comparer result is opposite to it && heightA < heightB.")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIsMinus1If(int widthComparerResult, 
                                                          int heightComparerResult, 
                                                          int widthA, int heightA, 
                                                          int widthB, int heightB, 
                                                          int gameWidth, int gameHeight)
        {
            var comparer = Substitute.For<ISingleDimensionComparer>();
            comparer.Compare(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(widthComparerResult, heightComparerResult);
            var component = new SizeResolutionComparisonComponent{WidthComparer = comparer, HeightComparer = comparer};
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(-1), "Compare result should have been -1 (A is greater).");
            comparer.Received(1).Compare(widthA, widthB, gameWidth);
            comparer.Received(1).Compare(heightA, heightB, gameHeight);
        }

        [TestCase(0, 1, 1280, 1024, 1366, 768, 1024, 768, TestName="(1) game resolution is landscape && width comparer result == 0 && height comparer result != 0  && heightA > heightB.")]
        [TestCase(0, 1, 1280, 1024, 1366, 768, 1024, 1024, TestName="(2) game resolution is square && width comparer result == 0 && height comparer result != 0  && heightA > heightB.")]
        [TestCase(1, 0, 1280, 1024, 320, 480, 768, 1024, TestName="(3) game resolution is portrait && height comparer result == 0 && width comparer result != 0 && widthA > widthB.")]
        [TestCase(-1, 1, 1366, 768, 320, 480, 768, 1024, TestName="(4) game resolution is portrait && height comparer result != 0 && width comparer result is opposite to it && heightA > heightB.")]
        [TestCase(-1, 1, 1366, 768, 320, 480, 1024, 768, TestName="(5) game resolution is landscape && width comparer result !=0  && height comparer result is opposite to it && widthA > widthB.")]
        [TestCase(-1, 1, 1366, 768, 320, 480, 1024, 1024, TestName="(6) game resolution is square && width comparer result !=0  && height comparer result is opposite to it && widthA > widthB.")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIs1If(int widthComparerResult, 
                                                     int heightComparerResult, 
                                                     int widthA, int heightA, 
                                                     int widthB, int heightB, 
                                                     int gameWidth, int gameHeight)
        {
            var comparer = Substitute.For<ISingleDimensionComparer>();
            comparer.Compare(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(widthComparerResult, heightComparerResult);
            var component = new SizeResolutionComparisonComponent{WidthComparer = comparer, HeightComparer = comparer};
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(widthA, heightA), 
                                             new Resolution(widthB, heightB), 
                                             new UnityEngine.Resolution().Set(gameWidth, gameHeight));
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(1), "Compare result should have been 1 (B is greater).");
            comparer.Received(1).Compare(widthA, widthB, gameWidth);
            comparer.Received(1).Compare(heightA, heightB, gameHeight);
        }

        [TestCaseSource(typeof(PropertyISingleDimensionComparer), "TestCases")]
        [Category("Easy Resolutions")]
        public void WidthComparerPropertySets(ISingleDimensionComparer comparer)
        {
            var component = new SizeResolutionComparisonComponent{WidthComparer = comparer};
            Assert.That(component.WidthComparer, Is.SameAs(comparer), string.Format("Property should have returned {0}.", comparer));
        }
        
        [TestCaseSource(typeof(PropertyISingleDimensionComparer), "TestCases")]
        [Category("Easy Resolutions")]
        public void HeightComparerPropertySets(ISingleDimensionComparer comparer)
        {
            var component = new SizeResolutionComparisonComponent{HeightComparer = comparer};
            Assert.That(component.HeightComparer, Is.SameAs(comparer), string.Format("Property should have returned {0}.", comparer));
        }

        [TestCaseSource(typeof(NullResolutions), "TestCases")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIs0If(Resolution resolutionA, Resolution resolutionB)
        {
            var component = new SizeResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, 
                                             resolutionA, 
                                             resolutionB, 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (equality).");
        }

        [TestCaseSource(typeof(NullWidthAndHeightComparers), "TestCases")]
        [Category("Easy Resolutions")]
        public void CompareReturnsTrueAndResultIs0If(ISingleDimensionComparer widthComparer, 
                                                     ISingleDimensionComparer heightComparer)
        {
            var component = new SizeResolutionComparisonComponent{WidthComparer = widthComparer, HeightComparer = heightComparer};
            int result;
            var returned = component.Compare(out result, 
                                             new Resolution(), 
                                             new Resolution(), 
                                             new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (equality).");
        }

        private class PropertyISingleDimensionComparer
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(null).SetName("null if null specified.");
                    yield return new TestCaseData(Substitute.For<ISingleDimensionComparer>()).SetName("valid object if valid object specified.");
                }
            }  
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

        private class NullWidthAndHeightComparers
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(null, null).SetName("both width and height comparers are null.");
                    yield return new TestCaseData(null, Substitute.For<ISingleDimensionComparer>()).SetName("only width comparer is null.");
                    yield return new TestCaseData(Substitute.For<ISingleDimensionComparer>(), null).SetName("only height comparer is null.");
                }
            }  
        }
    }
}