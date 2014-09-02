//-----------------------------------------------------------------------
// <copyright file="NullResolutionComparisonComponentTests.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using NUnit.Framework;

    [TestFixture]
    internal class NullResolutionComparisonComponentTests : TestsBase
    {
        [TestCase(TestName = "both resolutions are null.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs0If()
        {
            var component = new NullResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, null, null, new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (equality).");
        }

        [TestCase(TestName = "only resolution B is null.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIsMinus1If()
        {
            var component = new NullResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, new Resolution(), null, new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(-1), "Compare result should have been -1 (A is greater).");
        }

        [TestCase(TestName = "only resolution A is null.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs1If()
        {
            var component = new NullResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, null, new Resolution(), new UnityEngine.Resolution());
            Assert.That(returned, Is.True, "Compare should have returned true (comparison finished).");
            Assert.That(result, Is.EqualTo(1), "Compare result should have been 1 (B is greater).");
        }

        [TestCase(TestName = "both resolutions are not null.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsFalseAndResultIs0If()
        {
            var component = new NullResolutionComparisonComponent();
            int result;
            var returned = component.Compare(out result, new Resolution(), new Resolution(), new UnityEngine.Resolution());
            Assert.That(returned, Is.False, "Compare should have returned false (further comparison necessary).");
            Assert.That(result, Is.EqualTo(0), "Compare result should have been 0 (default value).");
        }
    }
}