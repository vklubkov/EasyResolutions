//-----------------------------------------------------------------------
// <copyright file="SingleDimensionComparer.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions
{
    /// <summary>
    /// Represents comparison of sizes of resolutions A and B in a single dimension
    /// and their check against the size of the game resolution in this dimension.
    /// </summary>
    public class SingleDimensionComparer : ISingleDimensionComparer
    {
        /// <summary>
        /// Compares sizes of resolutions A and B in same single dimension and checks 
        /// them against the game resolution size in this dimension. If sizeA is equal to 
        /// sizeB, the sizes are equal. If A is equal to the game resolution size - A 
        /// is "bigger". If B is equal to the game resolution size - B is "bigger". 
        /// If sizes of A and B both are greater than or both are lower than the size of the 
        /// game resolution, the size, which is closer to the size of game resolution, is 
        /// "bigger". Otherwise, the smallest value of sizes A and B is "bigger". 
        /// </summary>
        /// <returns>0 if sizes are equal, -1 if A is "bigger", 1 if B is "bigger".</returns>
        /// <param name="sizeA">Size of the first resolution.</param>
        /// <param name="sizeB">Size of the second resolution.</param>
        /// <param name="gameSize">Game resolution size.</param>
        public int Compare(int sizeA, int sizeB, int gameSize)
        {
            if (sizeA == sizeB)
            {
                return 0;
            }
            
            if (sizeA == gameSize)
            {
                return -1;
            }
            
            if (sizeB == gameSize)
            {
                return 1;
            }
            
            if ((sizeB < sizeA && sizeA < gameSize) ||
                (sizeB > sizeA && sizeA > gameSize))
            {
                return -1;
            }
            
            if ((sizeA < sizeB && sizeB < gameSize) ||
                (sizeA > sizeB && sizeB > gameSize))
            {
                return 1;
            }
            
            return (sizeA < sizeB) ? -1 : 1;
        }
    }
}
