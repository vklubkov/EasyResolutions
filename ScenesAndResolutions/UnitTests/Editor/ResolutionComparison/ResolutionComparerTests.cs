//-----------------------------------------------------------------------
// <copyright file="ResolutionComparerTests.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using NUnit.Framework;
    
    [TestFixture]
    internal class ResolutionComparerTests : TestsBase
    {
        [TestCase(TestName = "valid object if valid object specified.")]
        [Category("Scenes and Resolutions")]
        public void GameResolutionPropertySets()
        {
            var resolution = new UnityEngine.Resolution();
            var comparer = new ResolutionComparer{GameResolution = resolution};
            Assert.That(comparer.GameResolution, Is.EqualTo(resolution), string.Format("Property should have returned '{0}'.", resolution));
        }

        [TestCase(TestName = "null specified.")]
        [Category("Scenes and Resolutions")]
        public void AppendComparisonComponentAppendsNothingIf()
        {
            var comparer = new ResolutionComparer();
            var returned = comparer.AppendComparisonComponent(null);
            Assert.That(returned, Is.False, "AppendComparisonComponent should have returned false (failure).");
            var result = comparer.Compare(new Resolution(1366, 768), new Resolution(320, 480));
            Assert.That(result, Is.EqualTo(0), "Comparer result should have been equal to 0.");
        }

        [TestCase(TestName = "valid comparison component specified.")]
        [Category("Scenes and Resolutions")]
        public void AppendComparisonComponentAppendsResolutionComparisonComponentIf()
        {
            var comparer = new ResolutionComparer();

            var component = new ResolutionComparisonComponentMock{Result = -1, Retval = true};
            var returned = comparer.AppendComparisonComponent(component);
            Assert.That(returned, Is.True, "AppendComparisonComponent should have returned true (success).");

            var result = comparer.Compare(new Resolution(), new Resolution());
            Assert.That(result, Is.EqualTo(-1), "Comparer result should have been equal to -1.");
        }
       
        [TestCase(TestName = "no components appended.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturns0If()
        {
            var comparer = new ResolutionComparer();
            var result = comparer.Compare(new Resolution(1366, 768), new Resolution(320, 480));
            Assert.That(result, Is.EqualTo(0), "Comparer result should have been equal to 0.");
        }

        [TestCase(-1, true, 1, true, -1, TestName = "-1, if first component returns -1 and is final, and next component returns 1.")]
        [TestCase(0, true, 1, false, 0, TestName = "0, if first component returns 0 and is final, and next component return 1.")]
        [TestCase(1, true, -1, false, 1, TestName = "1, if tfirst component returns 1 and is final, and next component returns -1.")]
        [TestCase(1, false, -1, true, -1, TestName = "-1, if first component is not final and returns 1, and second is final and returns -1.")]       
        [TestCase(-1, false, 1, true, 1, TestName = "1, if first component is not final and returns -1, and second is final and returns 1.")]
        [TestCase(1, false, 0, true, 0, TestName = "0, if first component is not final and returns 1, and second is final and returns 0.")]
        [TestCase(1, false, -1, false, 0, TestName = "0, if all components are not final.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturns(int result1, bool isFinal1, int result2, bool isFinal2, int expected)
        {
            var comparer = new ResolutionComparer();

            var component1 = new ResolutionComparisonComponentMock{Result = result1, Retval = isFinal1 };
            comparer.AppendComparisonComponent(component1);

            var component2 = new ResolutionComparisonComponentMock{Result = result2, Retval = isFinal2 };
            comparer.AppendComparisonComponent(component2);

            var result = comparer.Compare(new Resolution(), new Resolution());
            Assert.That(result, Is.EqualTo(expected), string.Format("Comparer result should have been equal to {0}.", expected));
        }

        ///<summary>
        /// Custom mock.
        /// Necessary, because NSubstitute doesn't work, when a lambda is specifed to 
        /// Returns like this: something.DoesSomething().Returs(x => {x[0] = result; return retval;});
        /// Such lambda is not always invoked properly, and, thus, the tests are failing with NSubstitute!
        /// </summary>
        private class ResolutionComparisonComponentMock : IResolutionComparisonComponent
        {
            public int Result
            {
                get;
                set;
            }

            public bool Retval
            {
                get;
                set;
            }

            public bool Compare(out int result, Resolution resolutionA, Resolution resolutionB, UnityEngine.Resolution gameResolution)
            {
                result = this.Result;
                return this.Retval;
            }
        }
    }
}