using MultiPrecision;

namespace MultiPrecisionSeriesAcceleration {
    public class SteffensenIterativeKAutoAccelerator<N> : SeriesAccelerator<N> where N : struct, IConstant {
        readonly List<MultiPrecision<N>> b = new();
        readonly AitkenDeltaSquaredAccelerator<N> k1_accelerator = new();
        readonly SteffensenIterativeK2Accelerator<N> k2_accelerator = new();
        readonly SteffensenIterativeK3Accelerator<N> k3_accelerator = new();

        public override int SamplesCount => k1_accelerator.SamplesCount;

        public override int MinimumSamples => 3;

        public override void Append(MultiPrecision<N> new_value) {
            k1_accelerator.Append(new_value);
            k2_accelerator.Append(new_value);
            k3_accelerator.Append(new_value);

            MultiPrecision<N> new_b =
                !(k1_accelerator.Error >= k2_accelerator.Error) ? k1_accelerator.LastValue :
                !(k2_accelerator.Error >= k3_accelerator.Error) ? k2_accelerator.LastValue :
                k3_accelerator.LastValue;

            b.Add(new_b);
        }

        public override IEnumerable<MultiPrecision<N>> Series => b;

        public override MultiPrecision<N> LastValue {
            get {
                if (b.Count < 1) {
                    return MultiPrecision<N>.NaN;
                }

                return b[^1];
            }
        }

        public override MultiPrecision<N> Error {
            get {
                if (b.Count < 2) {
                    return MultiPrecision<N>.NaN;
                }

                return MultiPrecision<N>.Abs(b[^1] - b[^2]);
            }
        }
    }
}
