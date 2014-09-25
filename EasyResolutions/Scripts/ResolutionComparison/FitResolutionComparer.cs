//-----------------------------------------------------------------------
// <copyright file="FitResolutionComparer.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions
{
    /// <summary>
    /// Default <see cref="EasyResolutions.Resolution"/>'s 
    /// comparer, which treats the scene, which best "fills" 
    /// the game window, as "bigger". This means, that the 
    /// algorithm will prefer the one, which completely fits 
    /// into the game window rectangle, or at least the one, 
    /// which is cropped as little as possible, over others. 
    /// The comparer contains some cunning algorithm, which 
    /// compares two resolutions and checks them against the 
    /// game window resolution. It consists of the following components:
    /// 1) <see cref="EasyResolutions.NullResolutionComparisonComponent"/>,
    /// 2) <see cref="EasyResolutions.ZeroResolutionComparisonComponent"/>,
    /// 3) <see cref="EasyResolutions.EqualityResolutionComparisonComponent"/>,
    /// 4) <see cref="EasyResolutions.FitAspectRatioResolutionComparisonComponent"/>,
    /// 5) <see cref="EasyResolutions.OrientationResolutionComparisonComponent"/>,
    /// 6) <see cref="EasyResolutions.OppositeEqualityResolutionComparisonComponent"/>,
    /// 7) <see cref="EasyResolutions.SizeResolutionComparisonComponent"/>.
    /// </summary>
    public class FitResolutionComparer : ResolutionComparer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EasyResolutions.FitResolutionComparer"/> class.
        /// </summary>
        public FitResolutionComparer()
        {
            this.AppendComparisonComponent(new NullResolutionComparisonComponent());
            this.AppendComparisonComponent(new ZeroResolutionComparisonComponent());
            this.AppendComparisonComponent(new EqualityResolutionComparisonComponent());
            this.AppendComparisonComponent(new OrientationResolutionComparisonComponent());
            this.AppendComparisonComponent(new FitAspectRatioResolutionComparisonComponent());
            this.AppendComparisonComponent(new SizeResolutionComparisonComponent());
        }
    }
}