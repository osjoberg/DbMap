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
These benchmarks are focused on the deserialization performance of different object-relational mappers. 
Given differences should be interpreted as theoretical maximum. In reality your query, the network latency and the performance of your database server is more likely to affect performance.


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
|         EFCoreLinqTiny | 387.48 us | 22.420 us | 50.145 us | 385.41 us |  4.04 |    0.54 |
| EFCoreInterpolatedTiny | 335.82 us | 21.934 us | 49.058 us | 328.49 us |  3.52 |    0.50 |
|          EFCoreRawTiny | 325.45 us | 22.691 us | 50.753 us | 314.28 us |  3.40 |    0.54 |
| EFCoreCompliedLinqTiny | 158.05 us |  1.486 us |  3.135 us | 157.35 us |  1.64 |    0.04 |
|             DapperTiny | 133.41 us |  1.723 us |  3.635 us | 132.27 us |  1.38 |    0.04 |
|             RepoDbTiny | 105.54 us |  1.516 us |  3.263 us | 104.74 us |  1.09 |    0.04 |
|              DbMapTiny |  96.43 us |  0.439 us |  0.927 us |  96.09 us |  1.00 |    0.00 |
```

Extra small benchmark, query 1 row of 6 solumns using 1 parameter
``` ini
|                       Method |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD |
|----------------------------- |----------:|----------:|----------:|----------:|------:|--------:|
|         EFCoreLinqExtraSmall | 358.38 us | 23.261 us | 52.028 us | 355.45 us |  3.87 |    0.56 |
| EFCoreInterpolatedExtraSmall | 299.41 us | 24.467 us | 54.724 us | 271.32 us |  3.30 |    0.61 |
|          EFCoreRawExtraSmall | 297.39 us | 24.379 us | 52.998 us | 280.40 us |  3.23 |    0.58 |
| EFCoreCompliedLinqExtraSmall | 145.47 us |  1.073 us |  2.287 us | 144.82 us |  1.59 |    0.04 |
|             DapperExtraSmall | 123.47 us |  1.460 us |  3.173 us | 122.04 us |  1.35 |    0.04 |
|             RepoDbExtraSmall | 101.11 us |  2.379 us |  5.320 us |  99.21 us |  1.11 |    0.06 |
|              DbMapExtraSmall |  91.64 us |  0.545 us |  1.173 us |  91.49 us |  1.00 |    0.00 |
```

Small benchmark, query 10 rows of 6 columns using 1 parameter
``` ini
|                  Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|------------------------ |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqSmall | 386.5 us | 23.96 us | 51.57 us | 377.9 us |  3.38 |    0.45 |
| EFCoreInterpolatedSmall | 303.5 us | 22.73 us | 49.41 us | 279.0 us |  2.66 |    0.44 |
|          EFCoreRawSmall | 317.3 us | 23.86 us | 53.36 us | 312.9 us |  2.75 |    0.48 |
| EFCoreCompliedLinqSmall | 177.5 us |  1.50 us |  3.10 us | 177.2 us |  1.55 |    0.04 |
|             DapperSmall | 157.6 us |  0.89 us |  1.84 us | 157.3 us |  1.38 |    0.03 |
|             RepoDbSmall | 119.5 us |  1.13 us |  2.36 us | 118.7 us |  1.05 |    0.03 |
|              DbMapSmall | 114.2 us |  0.74 us |  1.59 us | 113.7 us |  1.00 |    0.00 |
```

Medium benchmark, query 100 rows of 10 columns using 5 parameters
``` ini
|                   Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|------------------------- |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqMedium | 651.2 us | 25.26 us | 56.49 us | 657.9 us |  1.90 |    0.27 |
| EFCoreInterpolatedMedium | 521.0 us | 18.42 us | 40.82 us | 519.3 us |  1.53 |    0.23 |
|          EFCoreRawMedium | 538.2 us | 18.90 us | 42.27 us | 542.5 us |  1.57 |    0.21 |
| EFCoreCompliedLinqMedium | 465.8 us | 15.33 us | 34.28 us | 480.7 us |  1.36 |    0.19 |
|             DapperMedium | 436.6 us | 16.67 us | 37.28 us | 440.4 us |  1.27 |    0.19 |
|             RepoDbMedium | 381.7 us | 17.53 us | 39.21 us | 377.2 us |  1.12 |    0.20 |
|              DbMapMedium | 348.0 us | 19.96 us | 44.65 us | 336.5 us |  1.00 |    0.00 |
```

Large benchmark, query 1,000 rows of 22 columns using 10 parameters
``` ini
|                  Method |     Mean |     Error |    StdDev |   Median | Ratio | RatioSD |
|------------------------ |---------:|----------:|----------:|---------:|------:|--------:|
|         EFCoreLinqLarge | 3.634 ms | 0.0518 ms | 0.1104 ms | 3.625 ms |  1.30 |    0.05 |
| EFCoreInterpolatedLarge | 3.443 ms | 0.0562 ms | 0.1210 ms | 3.423 ms |  1.23 |    0.05 |
|          EFCoreRawLarge | 3.379 ms | 0.0542 ms | 0.1154 ms | 3.341 ms |  1.21 |    0.05 |
|             DapperLarge | 3.538 ms | 0.0548 ms | 0.1120 ms | 3.534 ms |  1.26 |    0.06 |
|             RepoDbLarge | 3.027 ms | 0.0517 ms | 0.1135 ms | 2.983 ms |  1.08 |    0.05 |
|              DbMapLarge | 2.803 ms | 0.0420 ms | 0.0904 ms | 2.783 ms |  1.00 |    0.00 |
```

Extra large benchmark, query 10,000 rows of 22 columns using 10 parameters
``` ini
|                       Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|----------------------------- |---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqExtraLarge | 41.59 ms | 0.355 ms | 0.740 ms |  1.25 |    0.04 |
| EFCoreInterpolatedExtraLarge | 40.82 ms | 0.486 ms | 1.036 ms |  1.22 |    0.05 |
|          EFCoreRawExtraLarge | 40.95 ms | 0.651 ms | 1.387 ms |  1.23 |    0.05 |
|             DapperExtraLarge | 42.40 ms | 0.462 ms | 0.985 ms |  1.27 |    0.04 |
|             RepoDbExtraLarge | 36.51 ms | 0.397 ms | 0.837 ms |  1.09 |    0.04 |
|              DbMapExtraLarge | 33.42 ms | 0.421 ms | 0.905 ms |  1.00 |    0.00 |

```