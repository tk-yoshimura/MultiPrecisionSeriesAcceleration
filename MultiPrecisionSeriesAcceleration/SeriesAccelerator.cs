using MultiPrecision;

namespace MultiPrecisionSeriesAcceleration {
    public abstract class SeriesAccelerator<N> where N : struct, IConstant {
        public abstract int SamplesCount { get; }
        public abstract int MinimumSamples { get; }
        public abstract void Append(MultiPrecision<N> new_value);
        public abstract IEnumerable<MultiPrecision<N>> Series { get; }
        public abstract MultiPrecision<N> LastValue { get; }
        public abstract MultiPrecision<N> Error { get; }
    }
}
