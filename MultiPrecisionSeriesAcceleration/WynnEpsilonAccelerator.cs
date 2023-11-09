using MultiPrecision;
using System.Diagnostics;

namespace MultiPrecisionSeriesAcceleration {
    public class WynnEpsilonAccelerator<N> : SeriesAccelerator<N> where N : struct, IConstant {
        readonly List<MultiPrecision<N>> a = new(), b = new();
        readonly List<List<MultiPrecision<Double<N>>>> epsilon_table = new() {
            new List<MultiPrecision<Double<N>>>()
        };

        public override int SamplesCount => a.Count;

        public override int MinimumSamples => 3;

        public override void Append(MultiPrecision<N> new_value) {
            a.Add(new_value);

            MultiPrecision<N> new_b = Kernel(new_value);
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

        internal MultiPrecision<N> Kernel(MultiPrecision<N> new_value) {
            epsilon_table[0].Add(new_value.Convert<Double<N>>());
            epsilon_table.Add(new List<MultiPrecision<Double<N>>>());

            for (int j = 1; j < epsilon_table.Count - 1; j++) {
                MultiPrecision<Double<N>> e, d = epsilon_table[j - 1][^1] - epsilon_table[j - 1][^2];

                if (MultiPrecision<Double<N>>.IsFinite(d) && d.Exponent > new_value.Exponent - MultiPrecision<N>.Bits) {
                    e = ((j > 1) ? epsilon_table[j - 2][^2] : 0) + 1 / d;
                }
                else {
                    e = ((j & 1) == 1) ? MultiPrecision<Double<N>>.NaN : epsilon_table[j - 2][^1];
                }

                epsilon_table[j].Add(e);
            }

            MultiPrecision<N> y = epsilon_table[(epsilon_table.Count - 2) & ~1][^1].Convert<N>();

            Debug.WriteLine($"{epsilon_table.Count - 2 & ~1}");

            return y;
        }
    }
}
