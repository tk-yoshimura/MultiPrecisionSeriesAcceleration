# MultiPrecisionSeriesAcceleration
 MultiPrecision Series Acceleration Implements

## Requirement
.NET 10.0  
AVX2 suppoted CPU. (Intel:Haswell(2013)-, AMD:Excavator(2015)-)  
[MultiPrecision](https://github.com/tk-yoshimura/MultiPrecision)

## Install

[Download DLL](https://github.com/tk-yoshimura/MultiPrecisionSeriesAcceleration/releases)  
[Download Nuget](https://www.nuget.org/packages/tyoshimura.MultiPrecision.seriesacceleration/)  

## Usage

```csharp
WynnEpsilonAccelerator<Pow2.N8> accelerator = new();
for (long i = 2; i <= 50; i++) {
    MultiPrecision<Pow2.N8> n = MultiPrecision<Pow2.N8>.Ldexp(1, i);
    MultiPrecision<Pow2.N8> s = n / 2 * MultiPrecision<Pow2.N8>.SinPi(2 * MultiPrecision<Pow2.N8>.Div(1, n));

    accelerator.Append(s);

    MultiPrecision<Pow2.N8> raw_err = MultiPrecision<Pow2.N8>.Abs(MultiPrecision<Pow2.N8>.Pi - s);
    MultiPrecision<Pow2.N8> ext_err = MultiPrecision<Pow2.N8>.Abs(MultiPrecision<Pow2.N8>.Pi - accelerator.LastValue);

    Console.WriteLine($"n  : {n}");
    Console.WriteLine($"s  : {s}");
    Console.WriteLine($"val: {accelerator.LastValue}");
    Console.WriteLine($"acc err: {accelerator.Error:e10}");
    Console.WriteLine($"raw err: {raw_err:e10}");
    Console.WriteLine($"ext err: {ext_err:e10}\n");
}
```

## Licence
[MIT](https://github.com/tk-yoshimura/MultiPrecisionSeriesAcceleration/blob/main/LICENSE)

## Author

[T.Yoshimura](https://github.com/tk-yoshimura)
