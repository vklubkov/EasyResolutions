//-----------------------------------------------------------------------
// <copyright file="FitResolutionComparerTests.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using NUnit.Framework;

    [TestFixture]
    internal class FitResolutionComparerTests : TestsBase
    {
        [TestCase(TestName = "both resolutions are null.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs0If()
        {
            var comparer = new FitResolutionComparer();
            var result = comparer.Compare(null, null);
            Assert.That(result, Is.EqualTo(0), "Compare should have returned 0.");
        }
        
        [TestCase(TestName = "only resolution B is null.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIsMinus1If()
        {
            var comparer = new FitResolutionComparer();
            var result = comparer.Compare(new Resolution(), null);
            Assert.That(result, Is.EqualTo(-1), "Compare should have returned -1.");
        }
        
        [TestCase(TestName = "only resolution A is null.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs1If()
        {
            var comparer = new FitResolutionComparer();
            var result = comparer.Compare(null, new Resolution());
            Assert.That(result, Is.EqualTo(1), "Compare should have returned 1.");
        }
        
        [TestCase(0, 0, 0, 0, TestName = "both resolutions are zero.")]
        [TestCase(1366, 768, 1366, 768, TestName = "both resolutions are equal and not zero.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs0If(int widthA, int heightA, 
                                                     int widthB, int heightB)
        {
            var comparer = new FitResolutionComparer();
            var result = comparer.Compare(new Resolution(widthA, heightA), new Resolution(widthB, heightB));
            Assert.That(result, Is.EqualTo(0), "Compare should have returned 0.");
        }

        [TestCase(0, 0, 1366, 768, 0, 0, TestName = "(1) only resolution A is zero.")]
        [TestCase(1366, 768, 320, 480, 1366, 768, TestName = "(2) only resolution A is equal to game resolution, and B is not zero.")]
        [TestCase(1366, 768, 1680, 720, 1366, 768, TestName = "(3) resolutions A is equal to game resolution, and aspect ratio factor of resolution B is not nearly equal to aspect ratio factor of game resolution.")]
        [TestCase(1360, 768, 1024, 768, 1366, 768, TestName = "(4) resolutions A != game resolution, but aspect ratio factor of resolution A is ~= aspect ratio factor of game resolution, and aspect ratio factor of resolution B is not.")]
        [TestCase(1366, 768, 1024, 768, 1680, 720, TestName = "(5) aspect ratio factor of resolution B is < aspect ratio factor of resolution A, which is < aspect ratio factor of game resolution.")]
        [TestCase(1366, 768, 1680, 720, 1024, 768, TestName = "(6) aspect ratio factor of resolution B is > aspect ratio factor of resolution A, which is > aspect ratio factor of game resolution.")]
        [TestCase(1024, 768, 1680, 720, 1366, 768, TestName = "(7) aspect ratio factor of resolution B is > aspect ratio factor of game resolution and aspect ratio factor of A is < aspect ratio factor of game resolution.")]
        [TestCase(1136, 640, 400, 711, 1024, 768, TestName = "(8) aspect ratio factors of A and B are nearly equal, game resolution is landscape, resolution A is landscape and resolution B is portrait.")]
        [TestCase(640, 1136, 711, 400, 768, 1024, TestName = "(9) aspect ratio factors of A and B are nearly equal, game resolution is portrait, resolution A is portrait and resolution B is landscape.")]
        [TestCase(768, 1366, 400, 711, 1366, 768, TestName="(10) aspect ratio factors of A and B are nearly equal, resolutions A and B are not equal, width of resolution A is equal to height of game resolution and height of resolution A is equal to width of game resolution.")]
        [TestCase(711, 400, 1136, 640, 768, 1366, TestName="(11) aspect ratio factors of A and B are nearly equal, both resolutions A and B are lower than the game resolution, and resolution A is lower than resolution B.")]
        [TestCase(1136, 640, 711, 400, 1366, 768, TestName="(12) aspect ratio factors of A and B are nearly equal, both resolutions A and B are lower than the game resolution, and resolution B is lower than resolution A.")]
        [TestCase(1136, 640, 1366, 768, 400, 711, TestName="(13) aspect ratio factors of A and B are nearly equal, both resolutions A and B are greater than the game resolution, and resolution B is greater than resolution A.")]
        [TestCase(1136, 640, 1366, 768, 711, 400, TestName="(14) aspect ratio factors of A and B are nearly equal, both resolutions A and B are greater than the game resolution, and resolution B is greater than resolution A.")]
        [TestCase(711, 400, 1366, 768, 1136, 640, TestName="(15) aspect ratio factors of A and B are nearly equal, all resolutions are of same direction, resolution A is lower than game resolution and resolution B is greater than the game resolution B.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIsMinus1If(int widthA, int heightA, 
                                                          int widthB, int heightB, 
                                                          int gameWidth, int gameHeight)
        {
            var comparer = new FitResolutionComparer();
            comparer.GameResolution = new UnityEngine.Resolution().Set(gameWidth, gameHeight);
            var result = comparer.Compare(new Resolution(widthA, heightA), new Resolution(widthB, heightB));
            Assert.That(result, Is.EqualTo(-1), "Compare should have returned -1.");
        }
        
        [TestCase(1366, 768, 0, 0, 0, 0, TestName = "(1) only resolution B is zero.")]
        [TestCase(320, 480, 1366, 768, 1366, 768, TestName = "(2) only resolution B is equal to game resolution, and A is not zero.")]
        [TestCase(1680, 720, 1366, 768, 1366, 768, TestName = "(3) resolutions B is equal to game resolution, and aspect ratio factor of resolution A is not nearly equal to aspect ratio factor of game resolution.")]
        [TestCase(1024, 768, 1360, 768, 1366, 768, TestName = "(4) resolutions B != game resolution, but aspect ratio factor of resolution B is ~= aspect ratio factor of game resolution, and aspect ratio factor of resolution A is not.")]
        [TestCase(1024, 768, 1366, 768, 1680, 720, TestName = "(5) aspect ratio factor of resolution A is < aspect ratio factor of resolution B, which is < aspect ratio factor of game resolution.")]
        [TestCase(1680, 720, 1366, 768, 1024, 768, TestName = "(6) aspect ratio factor of resolution A is > aspect ratio factor of resolution B, which is > aspect ratio factor of game resolution.")]
        [TestCase(1680, 720, 1024, 768, 1366, 768, TestName = "(7) aspect ratio factor of resolution A is > aspect ratio factor of game resolution and aspect ratio factor of B is < aspect ratio factor of game resolution.")]
        [TestCase(400, 711, 1136, 640, 1024, 768, TestName = "(8) aspect ratio factors of A and B are nearly equal, game resolution is landscape, resolution A is portrait and resolution B is landscape.")]
        [TestCase(711, 400, 640, 1136, 768, 1024, TestName = "(9) aspect ratio factors of A and B are nearly equal, game resolution is portrait, resolution A is landscape and resolution B is portrait.")]
        [TestCase(711, 400, 1366, 768, 768, 1366, TestName="(10) aspect ratio factors of A and B are nearly equal, resolutions A and B are not equal, width of resolution B is equal to height of game resolution and height of resolution B is equal to width of game resolution.")]
        [TestCase(1136, 640, 711, 400, 768, 1366, TestName="(11) aspect ratio factors of A and B are nearly equal, both resolutions A and B are lower than the game resolution, and resolution B is lower than resolution A.")]
        [TestCase(711, 400, 1136, 640, 1366, 768, TestName="(12) aspect ratio factors of A and B are nearly equal, both resolutions A and B are lower than the game resolution, and resolution A is lower than resolution B.")]
        [TestCase(1366, 768, 1136, 640, 400, 711, TestName="(13) aspect ratio factors of A and B are nearly equal, both resolutions A and B are greater than the game resolution, and resolution A is greater than resolution B.")]
        [TestCase(1366, 768, 1136, 640, 711, 400, TestName="(14) aspect ratio factors of A and B are nearly equal, both resolutions A and B are greater than the game resolution, and resolution A is greater than resolution B.")]
        [TestCase(1366, 768, 711, 400, 1136, 640, TestName="(15) aspect ratio factors of A and B are nearly equal, all resolutions are of same direction, resolution A is greater than game resolution and resolution B is lower than the game resolution B.")]
        [Category("Scenes and Resolutions")]
        public void CompareReturnsTrueAndResultIs1If(int widthA, int heightA, 
                                                     int widthB, int heightB, 
                                                     int gameWidth, int gameHeight)
        {
            var comparer = new FitResolutionComparer();
            comparer.GameResolution = new UnityEngine.Resolution().Set(gameWidth, gameHeight);
            var result = comparer.Compare(new Resolution(widthA, heightA), new Resolution(widthB, heightB));
            Assert.That(result, Is.EqualTo(1), "Compare should have returned 1.");
        }
    }
}