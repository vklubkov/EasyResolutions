using System;
using UnityEngine;

namespace EasyResolutions {
    public abstract class EasyResolutionsComparer : ScriptableObject, IEasyResolutionsComparer {
        public abstract double Epsilon { protected get; set; }
        public abstract (int Width, int Height) Target { protected get; set; }
        public abstract int Compare(IResolution a, IResolution b);
    }

    [CreateAssetMenu(fileName = "EasyResolutions_Comparer", menuName = "Easy Resolutions/Comparer")]
    // ReSharper disable InconsistentNaming because it matches the asset name
    public class EasyResolutions_Comparer : EasyResolutionsComparer, IEasyResolutionsComparer {
        // ReSharper restore InconsistentNaming
        enum ResolutionPreference {
            Closest,
            ClosestLarger,
            ClosestSmaller
        }

        [SerializeField] bool _preferAspectOverResolution;
        [SerializeField] bool _respectOrientation = true;
        [SerializeField] ResolutionPreference _resolutionPreference = ResolutionPreference.Closest;

        public override double Epsilon { protected get; set; }
        public override (int Width, int Height) Target { protected get; set; }

        public override int Compare(IResolution a, IResolution b) {
            if (CompareNullValues(a, b, out var result))
                return result;

            // Non-null

            if (CompareNegativeValues(a, b, out result))
                return result;

            // Non-null and non-negative

            if (CompareZeroValues(a, b, out result))
                return result;

            // Non-null, non-negative and non-zero

            if (CompareEqualValues(a, b, out result))
                return result;

            // Non-null, non-negative, non-zero and non-equal

            if (_respectOrientation) {
                if (CompareOrientationValues(a, b, out result))
                    return result;

                // Non-null, non-negative, non-zero, non-equal and of same orientation
            }

            if (_preferAspectOverResolution) {
                if (CompareAspectRatios(a, b, out result))
                    return result;

                // Non-null, non-negative, non-zero, non-equal, of same orientation and:
                // either of same aspect, or of same excess area percentage

                if (CompareResolutions(a, b, out result))
                    return result;

                // Non-null, non-negative, non-zero, non-equal, of same orientation,
                // either of same aspect, or of same excess area percentage and:
                // of same vector distance
            }
            else {
                if (CompareResolutions(a, b, out result))
                    return result;

                // Non-null, non-negative, non-zero, non-equal, of same orientation and of same vector distance

                if (CompareAspectRatios(a, b, out result))
                    return result;

                // Non-null, non-negative, non-zero, non-equal, of same orientation, of same vector distance and:
                // either of same aspect, or of same excess area percentage
            }

            if (!_respectOrientation) {
                if (CompareOrientationValues(a, b, out result))
                    return result;
            }

            return 0;
        }

        bool CompareNullValues(IResolution a, IResolution b, out int result) {
            if (a == null && b == null) {
                result = 0; // a = b
                return true; // Comparison done
            }

            if (b == null) {
                result = -1; // a > b
                return true; // Comparison done
            }

            if (a == null) {
                result = 1; // b > a
                return true; // Comparison done
            }

            result = 0; // a = b
            return false; // Enable further comparison
        }

        bool CompareNegativeValues(IResolution a, IResolution b, out int result) {
            if (a.Width < 0 && a.Height < 0 && b.Width < 0 && b.Height < 0) {
                result = 0; // a = b
                return true; // Comparison done
            }

            if (b.Width < 0 && b.Height < 0) {
                result = -1; // a > b
                return true; // Comparison done
            }

            if (a.Width < 0 && a.Height < 0) {
                result = 1; // b > a
                return true; // Comparison done
            }

            result = 0; // a = b
            return false; // Enable further comparison
        }

        bool CompareZeroValues(IResolution a, IResolution b, out int result) {
            if (a.Width == 0 && a.Height == 0 && b.Width == 0 && b.Height == 0) {
                result = 0; // a = b
                return true; // Comparison done
            }

            if (b.Width == 0 && b.Height == 0) {
                result = -1; // a > b
                return true; // Comparison done
            }

            if (a.Width == 0 && a.Height == 0) {
                result = 1; // b > a
                return true; // Comparison done
            }

            result = 0; // a = b
            return false; // Enable further comparison
        }

        bool CompareEqualValues(IResolution a, IResolution b, out int result) {
            if (a.Width == b.Width && a.Height == b.Height) {
                result = 0; // a = b
                return true; // Comparison done
            }

            if (a.Width == Target.Width && a.Height == Target.Height) {
                result = -1; // a > b
                return true; // Comparison done
            }

            if (b.Width == Target.Width && b.Height == Target.Height) {
                result = 1; // b > a
                return true; // Comparison done
            }

            result = 0; // a = b
            return false; // Enable further comparison
        }

        bool CompareOrientationValues(IResolution a, IResolution b, out int result) {
            var isALandscape = a.Width > a.Height;
            var isAPortrait = a.Height > a.Width;
            var isASquare = a.Width == a.Height;

            var isBLandscape = b.Width > b.Height;
            var isBPortrait = b.Height > b.Width;
            var isBSquare = b.Width == b.Height;

            if ((isALandscape && isBLandscape) ||
                (isAPortrait && isBPortrait) ||
                (isASquare && isBSquare)) {
                result = 0; // a = b
                return false; // Enable further comparison
            }

            var isTargetLandscape = Target.Width > Target.Height;
            var isTargetPortrait = Target.Height > Target.Width;
            var isTargetSquare = Target.Width == Target.Height;

            if ((isTargetLandscape && isALandscape) ||
                (isTargetPortrait && isAPortrait) ||
                (isTargetSquare && isASquare)) {
                result = -1; // a > b
                return true; // Comparison done
            }

            if ((isTargetLandscape && isBLandscape) ||
                (isTargetPortrait && isBPortrait) ||
                (isTargetSquare && isBSquare)) {
                result = 1; // b > a
                return true; // Comparison done
            }

            result = 0; // a = b
            return false; // Enable further comparison
        }

        bool CompareAspectRatios(IResolution a, IResolution b, out int result) {
            var aspectRatioA = CalculateAspectRatio(a.Width, a.Height);
            var aspectRatioB = CalculateAspectRatio(b.Width, b.Height);
            if (NearlyEquals(aspectRatioA, aspectRatioB)) {
                result = 0; // a = b
                return false; // Enable further comparison
            }

            var targetAspectRatio = CalculateAspectRatio(Target.Width, Target.Height);
            if (NearlyEquals(aspectRatioA, targetAspectRatio)) {
                result = -1; // a > b
                return true; // Comparison done
            }

            if (NearlyEquals(aspectRatioB, targetAspectRatio)) {
                result = 1; // b > a
                return true; // Comparison done
            }

            return CompareExcessAreas(aspectRatioA, aspectRatioB, targetAspectRatio, out result);
        }

        double CalculateAspectRatio(int width, int height) {
            if (width <= 0 || height <= 0)
                return 0f;

            if (_respectOrientation)
                return width / (double)height;

            return width > height
                ? width / (double)height
                : height / (double)width;
        }

        bool CompareExcessAreas(double aspectRatioA, double aspectRatioB, double targetAspectRatio, out int result) {
            var areaA = CalculateExcessArea(aspectRatioA, targetAspectRatio);
            var areaB = CalculateExcessArea(aspectRatioB, targetAspectRatio);
            if (NearlyEquals(areaA, areaB)) {
                result = 0; // a = b
                return false; // Enable further comparison
            }

            result = areaA < areaB
                ? -1 // a > b
                : 1; // b > a

            return true; // Comparison done
        }

        static double CalculateExcessArea(double candidateAspectRatio, double targetAspectRatio) {
            var candidateScale = 1 / candidateAspectRatio;
            var targetScale = 1 / targetAspectRatio;

            if (targetAspectRatio >= candidateAspectRatio) {
                var fitScale = Math.Min(candidateScale, targetScale);
                return 1 - fitScale * candidateAspectRatio;
            }

            var fillScale = Math.Max(candidateScale, targetScale);
            return 1 - 1 / (fillScale * candidateAspectRatio);
        }

        bool NearlyEquals(double a, double b) => Math.Abs(a - b) < Epsilon;

        bool CompareResolutions(IResolution a, IResolution b, out int result) {
            var isALargerThanTarget = a.Width >= Target.Width && a.Height >= Target.Height;
            var isBLargerThanTarget = b.Width >= Target.Width && b.Height >= Target.Height;
            if (isALargerThanTarget && isBLargerThanTarget)
                return CompareResolutionsClosest(a, b, out result);

            var isASmallerThanTarget = a.Width <= Target.Width && a.Height <= Target.Height;
            var isBSmallerThanTarget = b.Width <= Target.Width && b.Height <= Target.Height;
            if (isASmallerThanTarget && isBSmallerThanTarget)
                return CompareResolutionsClosest(a, b, out result);

            if (!isALargerThanTarget && !isBLargerThanTarget && !isASmallerThanTarget && !isBSmallerThanTarget)
                return CompareResolutionsClosest(a, b, out result);

            switch (_resolutionPreference) {
                case ResolutionPreference.Closest:
                    return CompareResolutionsClosest(a, b, out result);
                case ResolutionPreference.ClosestLarger: {
                    if (isALargerThanTarget) {
                        result = -1; // a > b
                        return true; // Comparison done
                    }

                    if (isBLargerThanTarget) {
                        result = 1; // b > a
                        return true; // Comparison done
                    }

                    if (isBSmallerThanTarget) {
                        result = -1; // a > b
                        return true; // Comparison done
                    }

                    result = 1; // b > a
                    return true; // Comparison done
                }
                case ResolutionPreference.ClosestSmaller: {
                    if (isASmallerThanTarget) {
                        result = -1; // a > b
                        return true; // Comparison done
                    }

                    if (isBSmallerThanTarget) {
                        result = 1; // b > a
                        return true; // Comparison done
                    }

                    if (isBLargerThanTarget) {
                        result = -1; // a > b
                        return true; // Comparison done
                    }

                    result = 1; // b > a
                    return true; // Comparison done
                }
            }

            result = 0; // a = b
            return false; // Enable further comparison
        }

        bool CompareResolutionsClosest(IResolution a, IResolution b, out int result) {
            var distanceA = CalculateVectorDistance(a.Width, a.Height);
            var distanceB = CalculateVectorDistance(b.Width, b.Height);
            if (NearlyEquals(distanceA, distanceB)) {
                result = 0; // a = b
                return false; // Enable further comparison
            }

            result = distanceA < distanceB
                ? -1 // a > b
                : 1; // b > a

            return true; // Comparison done
        }

        float CalculateVectorDistance(int width, int height) {
            var candidateVector = GetCandidateVector(width, height);
            var targetVector = new Vector2Int(Target.Width, Target.Height);
            return (targetVector - candidateVector).magnitude;
        }

        Vector2Int GetCandidateVector(int width, int height) {
            if (_respectOrientation)
                return new Vector2Int(width, height);

            return width > height
                ? new Vector2Int(width, height)
                : new Vector2Int(height, width);
        }
    }
}