using MultiPrecision;
using MultiPrecisionSeriesAcceleration;

namespace MultiPrecisionSeriesAccelerationTest {
    [TestClass()]
    public class WynnEpsilonAcceleratorTests {
        [TestMethod()]
        public void AppendTest() {
            WynnEpsilonAccelerator<Pow2.N8> accelerator = new();
            for (long i = 2; i <= 50; i++) {
                MultiPrecision<Pow2.N8> n = MultiPrecision<Pow2.N8>.Ldexp(1, i);
                MultiPrecision<Pow2.N8> s = n / 2 * MultiPrecision<Pow2.N8>.SinPI(2 * MultiPrecision<Pow2.N8>.Div(1, n));

                accelerator.Append(s);

                MultiPrecision<Pow2.N8> raw_err = MultiPrecision<Pow2.N8>.Abs(MultiPrecision<Pow2.N8>.PI - s);
                MultiPrecision<Pow2.N8> ext_err = MultiPrecision<Pow2.N8>.Abs(MultiPrecision<Pow2.N8>.PI - accelerator.LastValue);

                Console.WriteLine($"n  : {n}");
                Console.WriteLine($"s  : {s}");
                Console.WriteLine($"val: {accelerator.LastValue}");
                Console.WriteLine($"acc err: {accelerator.Error:e10}");
                Console.WriteLine($"raw err: {raw_err:e10}");
                Console.WriteLine($"ext err: {ext_err:e10}\n");

                if (MultiPrecision<Pow2.N8>.IsFinite(accelerator.LastValue)) {
                    Assert.IsTrue(raw_err >= ext_err);
                }
            }

            foreach (MultiPrecision<Pow2.N8> v in accelerator.Series) {
                Console.WriteLine(v);
            }
        }
    }
}