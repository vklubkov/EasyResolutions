//-----------------------------------------------------------------------
// <copyright file="SingleDimensionComparerTests.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using NUnit.Framework;

    [TestFixture]
    internal class SingleDimensionComparerTests : TestsBase
    {
        [TestCase(1024, 1024, 800, TestName = "A and B are equal.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturns0If(int a, int b, int game)
        {
            var comparer = new SingleDimensionComparer();
            var returned = comparer.Compare(a, b, game);
            Assert.That(returned, Is.EqualTo(0), "Compare should have returned 0 (equality).");
        }

        [TestCase(1024, 800, 1024, TestName = "A is equal to game and B is not.")]
        [TestCase(1024, 800, 2560, TestName = "B is lower than a which is lower than game.")]
        [TestCase(1024, 2560, 800, TestName = "B is greater than a which is greater than game.")]
        [TestCase(800, 2560, 1024, TestName = "A is lower than game and B is greater than game.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsMinus1(int a, int b, int game)
        {
            var comparer = new SingleDimensionComparer();
            var returned = comparer.Compare(a, b, game);
            Assert.That(returned, Is.EqualTo(-1), "Compare should have returned -1 (A is greater).");
        }

        [TestCase(1024, 800, 800, TestName = "B is equal to game and A is not.")]
        [TestCase(800, 1024, 2560, TestName = "A is lower than B which is lower than game.")]
        [TestCase(2560, 1024, 800, TestName = "A is greater than B which is greater than game.")]
        [TestCase(2560, 800, 1024, TestName = "B is lower than game and A is greater than game.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturns1If(int a, int b, int game)
        {
            var comparer = new SingleDimensionComparer();
            var returned = comparer.Compare(a, b, game);
            Assert.That(returned, Is.EqualTo(1), "Compare should have returned 1 (B is greater).");
        }
    }
}