# DbMap
Performance-oriented database-to-object mapper for .NET

## Install via NuGet
To install DbMap, run the following command in the Package Manager Console:

```cmd
PM> Install-Package DbMap
```

You can also view the package page on [Nuget](https://www.nuget.org/packages/DbMap/).

## Example usage

```c#

using DbMap;

class Program
{
    private static readonly DbQuery CustomerQuery = new DbQuery("SELECT Name FROM Customer WHERE CustomerId = @customerId");

    static void Main()
    {
        // TODO: Set connection string.
        var connectionString = "...";
    
        using var connection = new SqlConnection(connectionString);
        
        var customer = CustomerQuery.QuerySingleOrDefault<Customer>(connection, new { customerId = 10 });
        
        // TODO: Do something with customer.
        .
        .
        .    
    }

}
```


## Benchmarks
``` ini
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=3.1.101
  [Host]     : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT
  Job-ZKXYTC : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT
```


Tiny benchmark, query 1 row of 1 string column using 1 parameter
``` ini
|                 Method |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD |
|----------------------- |----------:|----------:|----------:|----------:|------:|--------:|
|         EFCoreLinqTiny | 424.51 us | 29.211 us | 65.335 us | 416.45 us |  4.39 |    0.66 |
| EFCoreInterpolatedTiny | 324.03 us | 23.650 us | 51.414 us | 311.50 us |  3.32 |    0.58 |
|          EFCoreRawTiny | 328.09 us | 23.360 us | 52.249 us | 308.84 us |  3.34 |    0.52 |
|             DapperTiny | 134.92 us |  1.269 us |  2.620 us | 134.14 us |  1.37 |    0.06 |
|             RepoDbTiny | 106.28 us |  1.070 us |  2.257 us | 105.67 us |  1.08 |    0.05 |
|              DbMapTiny |  98.25 us |  1.921 us |  4.052 us |  96.89 us |  1.00 |    0.00 |
```

Extra small benchmark, query 1 row of 6 solumns using 1 parameter
``` ini
|                       Method |      Mean |     Error |    StdDev | Ratio | RatioSD |
|----------------------------- |----------:|----------:|----------:|------:|--------:|
|         EFCoreLinqExtraSmall | 377.08 us | 23.351 us | 52.228 us |  3.99 |    0.56 |
| EFCoreInterpolatedExtraSmall | 264.78 us | 12.509 us | 27.978 us |  2.84 |    0.30 |
|          EFCoreRawExtraSmall | 267.35 us | 14.488 us | 32.104 us |  2.87 |    0.36 |
|             DapperExtraSmall | 127.55 us |  1.368 us |  2.885 us |  1.37 |    0.03 |
|             RepoDbExtraSmall | 100.90 us |  0.574 us |  1.223 us |  1.08 |    0.02 |
|              DbMapExtraSmall |  93.22 us |  0.286 us |  0.603 us |  1.00 |    0.00 |
```

Small benchmark, query 10 rows of 6 columns using 1 parameter
``` ini
|                  Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|------------------------ |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqSmall | 379.6 us | 26.30 us | 57.72 us | 360.5 us |  3.71 |    0.54 |
| EFCoreInterpolatedSmall | 317.4 us | 24.33 us | 54.43 us | 297.7 us |  3.15 |    0.56 |
|          EFCoreRawSmall | 311.3 us | 23.37 us | 51.30 us | 279.9 us |  3.07 |    0.50 |
|             DapperSmall | 150.8 us |  0.89 us |  1.88 us | 150.6 us |  1.48 |    0.03 |
|             RepoDbSmall | 121.6 us |  1.65 us |  3.41 us | 120.2 us |  1.20 |    0.04 |
|              DbMapSmall | 101.6 us |  0.84 us |  1.80 us | 101.2 us |  1.00 |    0.00 |
```

Medium benchmark, query 100 rows of 10 columns using 5 parameters
``` ini
|                   Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|------------------------- |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqMedium | 646.2 us | 27.20 us | 60.85 us | 634.9 us |  1.82 |    0.31 |
| EFCoreInterpolatedMedium | 550.4 us | 23.28 us | 52.06 us | 550.8 us |  1.55 |    0.27 |
|          EFCoreRawMedium | 566.8 us | 23.10 us | 51.66 us | 595.7 us |  1.59 |    0.24 |
|             DapperMedium | 418.2 us | 20.04 us | 44.40 us | 408.9 us |  1.18 |    0.20 |
|             RepoDbMedium | 374.6 us | 20.78 us | 46.47 us | 366.4 us |  1.06 |    0.20 |
|              DbMapMedium | 361.2 us | 21.12 us | 47.23 us | 380.0 us |  1.00 |    0.00 |```
```

Large benchmark, query 1.000 rows of 22 columns using 10 parameters
``` ini
|                  Method |     Mean |     Error |    StdDev |   Median | Ratio | RatioSD |
|------------------------ |---------:|----------:|----------:|---------:|------:|--------:|
|         EFCoreLinqLarge | 3.626 ms | 0.0350 ms | 0.0752 ms | 3.618 ms |  1.27 |    0.05 |
| EFCoreInterpolatedLarge | 3.492 ms | 0.0335 ms | 0.0699 ms | 3.489 ms |  1.22 |    0.04 |
|          EFCoreRawLarge | 3.493 ms | 0.0318 ms | 0.0671 ms | 3.480 ms |  1.22 |    0.04 |
|             DapperLarge | 3.589 ms | 0.0284 ms | 0.0593 ms | 3.564 ms |  1.26 |    0.04 |
|             RepoDbLarge | 3.043 ms | 0.0316 ms | 0.0674 ms | 3.030 ms |  1.06 |    0.04 |
|              DbMapLarge | 2.861 ms | 0.0381 ms | 0.0812 ms | 2.860 ms |  1.00 |    0.00 |
```

Extra large benchmark, query 10.000 rows of 22 columns using 10 parameters
``` ini

|                       Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|----------------------------- |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqExtraLarge | 41.80 ms | 0.214 ms | 0.446 ms | 41.75 ms |  1.24 |    0.03 |
| EFCoreInterpolatedExtraLarge | 41.30 ms | 0.405 ms | 0.854 ms | 41.11 ms |  1.23 |    0.04 |
|          EFCoreRawExtraLarge | 41.69 ms | 0.582 ms | 1.241 ms | 41.49 ms |  1.24 |    0.06 |
|             DapperExtraLarge | 43.28 ms | 0.415 ms | 0.875 ms | 43.24 ms |  1.28 |    0.04 |
|             RepoDbExtraLarge | 37.44 ms | 0.397 ms | 0.847 ms | 37.13 ms |  1.11 |    0.04 |
|              DbMapExtraLarge | 33.71 ms | 0.414 ms | 0.883 ms | 33.56 ms |  1.00 |    0.00 |
```