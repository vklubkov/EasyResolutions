//-----------------------------------------------------------------------
// <copyright file="ISingleDimensionComparer.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions
{
    /// <summary>
    /// Single dimension comparer interface.
    /// Single dimension comparer compares sizes of resolutions A and B in a single dimension
    /// and checks them against the size of the game resolution in this dimension.
    /// </summary>
    public interface ISingleDimensionComparer
    {
        /// <summary>
        /// Compares sizes of resolutions A and B in the same single dimension and checks 
        /// them against the game resolution size in this dimension. 
        /// </summary>
        /// <returns><c>0</c> if <c>sizeA</c> and <c>sizeB</c> are equal, 
        /// <c>-1</c> if <c>sizeA</c> is "bigger", 
        /// <c>1</c> if <c>sizeB</c> is "bigger".</returns>
        /// <param name="sizeA">Size of the first resolution.</param>
        /// <param name="sizeB">Size of the second resolution.</param>
        /// <param name="gameSize">Game resolution size.</param>
        int Compare(int sizeA, int sizeB, int gameSize);
    }
}
