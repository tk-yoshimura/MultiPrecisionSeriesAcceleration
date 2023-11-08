using MultiPrecision;

namespace MultiPrecisionSeriesAcceleration {
    internal struct Double<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value * 2);
    }
}
