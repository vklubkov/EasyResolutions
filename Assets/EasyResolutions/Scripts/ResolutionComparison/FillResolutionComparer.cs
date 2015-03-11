//-----------------------------------------------------------------------
// <copyright file="FillResolutionComparer.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions
{
    /// <summary>
    /// Default <see cref="EasyResolutions.Resolution"/>'s 
    /// comparer, which treats the scene, which best "fills" 
    /// the game window, as "bigger". This means, that the 
    /// algorithm will prefer the one, which covers the whole 
    /// game window rectangle (thus, the scene may be cropped), 
    /// or at least the one, which covers more area of the rectangle, 
    /// over others. The comparer contains some cunning algorithm, 
    /// which compares two resolutions and checks them against 
    /// the game window resolution. It consists of the following components:
    /// 1) <see cref="EasyResolutions.NullResolutionComparisonComponent"/>,
    /// 2) <see cref="EasyResolutions.ZeroResolutionComparisonComponent"/>,
    /// 3) <see cref="EasyResolutions.EqualityResolutionComparisonComponent"/>,
    /// 4) <see cref="EasyResolutions.FillAspectRatioResolutionComparisonComponent"/>,
    /// 5) <see cref="EasyResolutions.OrientationResolutionComparisonComponent"/>,
    /// 6) <see cref="EasyResolutions.OppositeEqualityResolutionComparisonComponent"/>,
    /// 7) <see cref="EasyResolutions.SizeResolutionComparisonComponent"/>.
    /// </summary>
    public class FillResolutionComparer : ResolutionComparer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EasyResolutions.FillResolutionComparer"/> class.
        /// </summary>
        public FillResolutionComparer()
        {
            this.AppendComparisonComponent(new NullResolutionComparisonComponent());
            this.AppendComparisonComponent(new ZeroResolutionComparisonComponent());
            this.AppendComparisonComponent(new EqualityResolutionComparisonComponent());
            this.AppendComparisonComponent(new OrientationResolutionComparisonComponent());
            this.AppendComparisonComponent(new FillAspectRatioResolutionComparisonComponent());
            this.AppendComparisonComponent(new SizeResolutionComparisonComponent());
        }
    }
}