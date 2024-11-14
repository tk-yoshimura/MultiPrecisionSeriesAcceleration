using MultiPrecision;

namespace MultiPrecisionSeriesAcceleration {

    public class RichardsonExtrapolationAccelerator<N> : SeriesAccelerator<N> where N : struct, IConstant {
        static readonly List<MultiPrecision<N>> rs = new() { MultiPrecision<N>.NaN };
        readonly List<MultiPrecision<N>[]> values = new();

        public override int SamplesCount => values.Count;

        public override int MinimumSamples => 2;

        public override void Append(MultiPrecision<N> new_value) {
            lock (values) {
                if (SamplesCount <= 0) {
                    values.Add([new_value]);
                    return;
                }

                MultiPrecision<N>[] t = values[SamplesCount - 1], t_next = new MultiPrecision<N>[SamplesCount + 1];

                t_next[0] = new_value;

                for (int i = 1; i <= SamplesCount; i++) {
                    t_next[i] = t_next[i - 1] + (t_next[i - 1] - t[i - 1]) * R(i);
                }

                values.Add(t_next);
            }
        }

        public override IEnumerable<MultiPrecision<N>> Series {
            get {
                for (int i = 0; i < values.Count; i++) {
                    yield return values[i][i];
                }
            }
        }

        public override MultiPrecision<N> LastValue {
            get {
                if (SamplesCount < 1) {
                    return MultiPrecision<N>.NaN;
                }

                return values[^1][^1];
            }
        }

        public override MultiPrecision<N> Error {
            get {
                if (SamplesCount < 2) {
                    return MultiPrecision<N>.NaN;
                }

                return MultiPrecision<N>.Abs(values[^1][^1] - values[^2][^1]);
            }
        }

        private static MultiPrecision<N> R(int i) {
            if (i < rs.Count) {
                return rs[i];
            }

            lock (rs) {
                for (int k = rs.Count; k <= i; k++) {
                    MultiPrecision<N> r = 1d / (MultiPrecision<N>.Ldexp(1d, k * 2) - 1);

                    rs.Add(r);
                }

                return rs[i];
            }
        }
    }
}
