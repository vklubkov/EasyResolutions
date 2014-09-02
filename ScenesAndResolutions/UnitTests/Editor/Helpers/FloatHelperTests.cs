//-----------------------------------------------------------------------
// <copyright file="FloatHelperTests.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using NUnit.Framework;

    [TestFixture]
    internal class FloatHelperTests : TestsBase
    {
        [TestCase(0f, 0f, TestName = "both values are 0 (zero).")] 
        [TestCase(1f, 1f, TestName = "both values are 1 (square).")]
        [TestCase(1.25f, 1.25f, TestName = "both values are 1.25 (5:4).")]
        [TestCase(1.333333333333333f, 1.333333333333333f, TestName = "both values are 1.333333333333333 (4:3).")]
        [TestCase(1.5f, 1.5f, TestName = "both values are 1.5 (3:2).")]
        [TestCase(1.6f, 1.6f, TestName = "both values are 1.6 (16:10).")]
        [TestCase(1.666666666666667f, 1.666666666666667f, TestName = "both values are 1.666666666666667 (5:3).")]
        [TestCase(1.706666666666667f, 1.706666666666667f, TestName = "both values are 1.706666666666667 (17:10+).")]
        [TestCase(1.770833333333333f, 1.770833333333333f, TestName = "both values are 1.770833333333333 (16:9---).")]
        [TestCase(1.770833333333333f, 1.775f, TestName = "one value is 1.770833333333333 (16:9---), other is 1.775 (16:9--).")]
        [TestCase(1.775f, 1.775f, TestName = "both values are 1.775 (16:9--).")]
        [TestCase(1.770833333333333f, 1.7775f, TestName = "one value is 1.770833333333333 (16:9---), other is 1.7775 (16:9-).")]
        [TestCase(1.775f, 1.7775f, TestName = "one value is 1.775 (16:9--), other is 1.7775 (16:9-).")]
        [TestCase(1.7775f, 1.7775f, TestName = "both values are 1.7775 (16:9-).")]
        [TestCase(1.770833333333333f, 1.777777777777778f, TestName = "one value is 1.770833333333333 (16:9---), other is 1.777777777777778 (16:9).")]
        [TestCase(1.775f, 1.777777777777778f, TestName = "one value is 1.775 (16:9--), other is 1.777777777777778 (16:9).")]
        [TestCase(1.7775f, 1.777777777777778f, TestName = "one value is 1.7775 (16:9-), other is 1.777777777777778 (16:9).")]
        [TestCase(1.777777777777778f, 1.777777777777778f, TestName = "both values are 1.777777777777778 (16:9).")]
        [TestCase(1.770833333333333f, 1.778645833333333f, TestName = "one value is 1.770833333333333 (16:9---), other is 1.778645833333333 (16:9+).")]
        [TestCase(1.775f, 1.778645833333333f, TestName = "one value is 1.775 (16:9--), other is 1.778645833333333 (16:9+).")]
        [TestCase(1.7775f, 1.778645833333333f, TestName = "one value is 1.7775 (16:9-), other is 1.778645833333333 (16:9+).")]
        [TestCase(1.777777777777778f, 1.778645833333333f, TestName = "one value is 1.777777777777778 (16:9), other is 1.778645833333333 (16:9+).")]
        [TestCase(1.778645833333333f, 1.778645833333333f, TestName = "both values are 1.778645833333333 (16:9+).")]
        [TestCase(2.333333333333333f, 2.333333333333333f, TestName = "both values are 2.333333333333333 (21:9).")]
        [TestCase(2.37037037037037f, 2.37037037037037f, TestName = "both values are 2.37037037037037 (64:27).")]
        [TestCase(3.2f, 3.2f, TestName = "both values are 3.2 (16:5).")]
        [Category("Scenes and Resolutions")]
        public void NearlyEqualsWithEpsilonForComparisonOfAspectRatioFactorsReturnsTrueIf(float a, float b)
        {
            var returned = a.NearlyEquals(b, FloatHelper.EpsilonForComparisonOfAspectRatioFactors);
            Assert.That(returned, Is.True, "NearlyEquals should have returned true (values are nearly equal).");
        }

        [TestCase(0f, 1f, TestName = "one value is 0 (zero), other is 1 (square).")]
        [TestCase(1f, 1.25f, TestName = "one value is 1 (square), other is 1.25 (5:4).")]
        [TestCase(1.25f, 1.333333333333333f, TestName = "one value is 1.25 (5:4), other is 1.333333333333333 (4:3).")]
        [TestCase(.333333333333333f, 1.5f, TestName = "one value is 1.333333333333333 (4:3), other is 1.5 (3:2).")]
        [TestCase(1.5f, 1.6f, TestName = "one value is 1.5 (3:2), other is 1.6 (16:10).")]
        [TestCase(1.6f, 1.666666666666667f, TestName = "one value is 1.6 (16:10), other is 1.666666666666667 (5:3).")]
        [TestCase(1.666666666666667f, 1.706666666666667f, TestName = "one value is 1.666666666666667 (5:3), other is 1.706666666666667 (17:10+).")]
        [TestCase(1.706666666666667f, 1.770833333333333f, TestName = "one value is 1.706666666666667 (17:10+), other is 1.770833333333333 (16:9---).")]
        [TestCase(1.778645833333333f, 2.333333333333333f, TestName = "one value is 1.778645833333333 (16:9+), other is 2.333333333333333 (21:9).")]
        [TestCase(2.333333333333333f, 2.37037037037037f, TestName = "one value is 2.333333333333333 (21:9), other is 2.37037037037037 (64:27).")]
        [TestCase(2.37037037037037f, 3.2f, TestName = "one value is 2.37037037037037 (64:27), other is 3.2 (16:5).")]
        [Category("Scenes and Resolutions")]
        public void NearlyEqualsWithEpsilonForComparisonOfAspectRatioFactorsReturnsFalseIf(float a, float b)
        {
            var returned = a.NearlyEquals(b, FloatHelper.EpsilonForComparisonOfAspectRatioFactors);
            Assert.That(returned, Is.False, "NearlyEquals should have returned false (values aren't nearly equal).");
        }
    }
}