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
|         EFCoreLinqTiny | 387.92 us | 24.015 us | 53.216 us | 376.58 us |  4.01 |    0.61 |
| EFCoreInterpolatedTiny | 328.59 us | 25.715 us | 57.515 us | 314.67 us |  3.37 |    0.60 |
|          EFCoreRawTiny | 317.43 us | 25.162 us | 55.757 us | 297.64 us |  3.28 |    0.56 |
|             DapperTiny | 134.62 us |  1.640 us |  3.460 us | 134.17 us |  1.39 |    0.05 |
|             RepoDbTiny | 106.53 us |  1.398 us |  2.949 us | 105.84 us |  1.10 |    0.06 |
|              DbMapTiny |  97.20 us |  1.977 us |  4.298 us |  95.85 us |  1.00 |    0.00 |
```

Extra small benchmark, query 1 row of 6 solumns using 1 parameter
``` ini
|                       Method |      Mean |     Error |    StdDev | Ratio | RatioSD |
|----------------------------- |----------:|----------:|----------:|------:|--------:|
|         EFCoreLinqExtraSmall | 360.74 us | 27.958 us | 62.531 us |  3.82 |    0.62 |
| EFCoreInterpolatedExtraSmall | 255.15 us |  6.991 us | 15.637 us |  2.76 |    0.17 |
|          EFCoreRawExtraSmall | 252.49 us |  8.905 us | 19.917 us |  2.73 |    0.20 |
|             DapperExtraSmall | 127.13 us |  1.137 us |  2.373 us |  1.36 |    0.06 |
|             RepoDbExtraSmall | 100.18 us |  0.446 us |  0.950 us |  1.07 |    0.04 |
|              DbMapExtraSmall |  93.41 us |  1.724 us |  3.749 us |  1.00 |    0.00 |
```

Small benchmark, query 10 rows of 6 columns using 1 parameter
``` ini
|                  Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|------------------------ |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqSmall | 373.2 us | 22.27 us | 48.42 us | 356.3 us |  3.65 |    0.48 |
| EFCoreInterpolatedSmall | 318.7 us | 22.45 us | 50.21 us | 312.3 us |  3.10 |    0.52 |
|          EFCoreRawSmall | 317.9 us | 18.49 us | 40.98 us | 313.8 us |  3.09 |    0.41 |
|             DapperSmall | 150.0 us |  1.34 us |  2.82 us | 149.9 us |  1.47 |    0.07 |
|             RepoDbSmall | 121.2 us |  2.63 us |  5.77 us | 119.0 us |  1.19 |    0.08 |
|              DbMapSmall | 102.4 us |  2.13 us |  4.62 us | 100.7 us |  1.00 |    0.00 |
```

Medium benchmark, query 100 rows of 10 columns using 5 parameters
``` ini
|                   Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|------------------------- |---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqMedium | 628.6 us | 23.88 us | 53.40 us |  1.81 |    0.32 |
| EFCoreInterpolatedMedium | 549.3 us | 22.12 us | 49.48 us |  1.58 |    0.27 |
|          EFCoreRawMedium | 519.7 us | 22.64 us | 49.70 us |  1.50 |    0.27 |
|             DapperMedium | 423.0 us | 20.50 us | 44.57 us |  1.22 |    0.23 |
|             RepoDbMedium | 388.2 us | 19.47 us | 43.56 us |  1.11 |    0.20 |
|              DbMapMedium | 354.2 us | 20.33 us | 45.46 us |  1.00 |    0.00 |
```

Large benchmark, query 1.000 rows of 22 columns using 10 parameters
``` ini
|                  Method |     Mean |     Error |    StdDev |   Median | Ratio | RatioSD |
|------------------------ |---------:|----------:|----------:|---------:|------:|--------:|
|         EFCoreLinqLarge | 3.588 ms | 0.0480 ms | 0.1012 ms | 3.565 ms |  1.26 |    0.09 |
| EFCoreInterpolatedLarge | 3.337 ms | 0.0425 ms | 0.0896 ms | 3.317 ms |  1.17 |    0.07 |
|          EFCoreRawLarge | 3.339 ms | 0.0450 ms | 0.0959 ms | 3.325 ms |  1.17 |    0.08 |
|             DapperLarge | 3.472 ms | 0.0522 ms | 0.1111 ms | 3.456 ms |  1.22 |    0.09 |
|             RepoDbLarge | 2.967 ms | 0.0502 ms | 0.1080 ms | 2.909 ms |  1.04 |    0.07 |
|              DbMapLarge | 2.859 ms | 0.0920 ms | 0.1961 ms | 2.783 ms |  1.00 |    0.00 |
```

Extra large benchmark, query 10.000 rows of 22 columns using 10 parameters
``` ini
|                       Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|----------------------------- |---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqExtraLarge | 41.15 ms | 0.373 ms | 0.787 ms |  1.22 |    0.05 |
| EFCoreInterpolatedExtraLarge | 40.73 ms | 0.422 ms | 0.891 ms |  1.21 |    0.05 |
|          EFCoreRawExtraLarge | 41.19 ms | 0.636 ms | 1.356 ms |  1.22 |    0.06 |
|             DapperExtraLarge | 42.18 ms | 0.392 ms | 0.801 ms |  1.26 |    0.05 |
|             RepoDbExtraLarge | 37.30 ms | 0.685 ms | 1.489 ms |  1.10 |    0.05 |
|              DbMapExtraLarge | 33.69 ms | 0.501 ms | 1.057 ms |  1.00 |    0.00 |

```