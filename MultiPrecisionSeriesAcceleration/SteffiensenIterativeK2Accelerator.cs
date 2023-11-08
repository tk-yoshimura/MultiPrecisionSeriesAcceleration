using MultiPrecision;

namespace MultiPrecisionSeriesAcceleration {
    public class SteffiensenIterativeK2Accelerator<N> : SeriesAccelerator<N> where N : struct, IConstant {
        readonly List<MultiPrecision<N>> a = new(), b = new();

        public override int SamplesCount => a.Count;

        public override int MinimumSamples => 5;

        public override void Append(MultiPrecision<N> new_value) {
            a.Add(new_value);

            if (a.Count >= MinimumSamples) {
                MultiPrecision<N> new_b = Kernel(a);
                b.Add(new_b);
            }
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


        internal static MultiPrecision<N> Kernel(List<MultiPrecision<N>> a) {
            MultiPrecision<Double<N>> a0 = a[^5].Convert<Double<N>>();
            MultiPrecision<Double<N>> a1 = a[^4].Convert<Double<N>>();
            MultiPrecision<Double<N>> a2 = a[^3].Convert<Double<N>>();
            MultiPrecision<Double<N>> a3 = a[^2].Convert<Double<N>>();
            MultiPrecision<Double<N>> a4 = a[^1].Convert<Double<N>>();

            MultiPrecision<Double<N>> div0 = a0 + a2 - 2 * a1;
            MultiPrecision<Double<N>> div1 = a1 + a3 - 2 * a2;
            MultiPrecision<Double<N>> div2 = a2 + a4 - 2 * a3;

            MultiPrecision<Double<N>> div = div0 * div2 - div1 * div1;

            MultiPrecision<Double<N>> aa = a0 * (a2 * a4 - a3 * a3) - a1 * (a1 * a4 - a2 * a3) + a2 * (a1 * a3 - a2 * a2);

            MultiPrecision<N> new_b = (aa / div).Convert<N>();
            return new_b;
        }
    }
}
