﻿using MultiPrecision;

namespace MultiPrecisionSeriesAcceleration {
    public class AitkenDeltaSquaredAccelerator<N> : SeriesAccelerator<N> where N : struct, IConstant {
        readonly List<MultiPrecision<N>> a = new(), b = new();

        public override int SamplesCount => a.Count;

        public override int MinimumSamples => 3;

        public override void Append(MultiPrecision<N> new_value) {
            lock (a) {
                a.Add(new_value);
            }

            lock (b) {
                if (a.Count >= MinimumSamples) {
                    MultiPrecision<N> new_b = Kernel(a);

                    if (MultiPrecision<N>.IsFinite(new_b)) {
                        b.Add(new_b);
                    }
                    else {
                        b.Add(new_value);
                    }
                }
                else {
                    b.Add(new_value);
                }
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
            MultiPrecision<Double<N>> a0 = a[^3].Convert<Double<N>>();
            MultiPrecision<Double<N>> a1 = a[^2].Convert<Double<N>>();
            MultiPrecision<Double<N>> a2 = a[^1].Convert<Double<N>>();

            MultiPrecision<Double<N>> div = a0 + a2 - 2 * a1;

            MultiPrecision<Double<N>> aa = a0 * a2 - a1 * a1;
            MultiPrecision<N> new_b = (aa / div).Convert<N>();
            return new_b;
        }
    }
}
