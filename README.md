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

Tested object mappers are:
EFCore 3.1.3 LINQ query (non-tracking)
EFCore 3.1.3 Interpolated SQL query (non-tracking)
EFCore 3.1.3 Raw SQL query (non-tracking)
EFCore 3.1.3 LINQ cached query (non-tracking)
Dapper 2.0.35
RepoDb 1.10.11
DbMap 0.5.2
Hand written 

``` ini
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=3.1.101
  [Host]     : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT
  Job-ZKXYTC : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT
```


Tiny benchmark, query 1 row of 1 string column using 1 parameter
``` ini
|                 Method |      Mean |     Error |    StdDev | Ratio | RatioSD |
|----------------------- |----------:|----------:|----------:|------:|--------:|
|         EFCoreLinqTiny | 378.73 탎 | 14.602 탎 | 32.051 탎 |  3.77 |    0.34 |
| EFCoreInterpolatedTiny | 295.35 탎 | 10.352 탎 | 22.723 탎 |  2.93 |    0.23 |
|          EFCoreRawTiny | 295.40 탎 | 11.132 탎 | 24.898 탎 |  2.97 |    0.24 |
| EFCoreCompliedLinqTiny | 168.56 탎 |  1.652 탎 |  3.484 탎 |  1.68 |    0.04 |
|             DapperTiny | 144.21 탎 |  1.178 탎 |  2.560 탎 |  1.43 |    0.03 |
|             RepoDbTiny | 110.61 탎 |  0.503 탎 |  1.039 탎 |  1.10 |    0.02 |
|              DbMapTiny | 100.54 탎 |  0.779 탎 |  1.591 탎 |  1.00 |    0.00 |
|        HandwrittenTiny |  99.64 탎 |  0.360 탎 |  0.752 탎 |  0.99 |    0.02 |
```

Extra small benchmark, query 1 row of 6 solumns using 1 parameter
``` ini
|                       Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|----------------------------- |---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqExtraSmall | 348.8 탎 | 13.48 탎 | 29.59 탎 |  3.40 |    0.31 |
| EFCoreInterpolatedExtraSmall | 286.7 탎 |  7.17 탎 | 15.88 탎 |  2.80 |    0.17 |
|          EFCoreRawExtraSmall | 279.4 탎 |  5.84 탎 | 12.57 탎 |  2.73 |    0.14 |
| EFCoreCompliedLinqExtraSmall | 173.1 탎 |  1.79 탎 |  3.85 탎 |  1.69 |    0.05 |
|             DapperExtraSmall | 151.6 탎 |  1.08 탎 |  2.34 탎 |  1.48 |    0.03 |
|             RepoDbExtraSmall | 123.6 탎 |  1.17 탎 |  2.45 탎 |  1.21 |    0.03 |
|              DbMapExtraSmall | 102.5 탎 |  0.80 탎 |  1.66 탎 |  1.00 |    0.00 |
|        HandwrittenExtraSmall | 102.0 탎 |  0.48 탎 |  1.06 탎 |  1.00 |    0.02 |
```

Small benchmark, query 10 rows of 6 columns using 1 parameter
``` ini
|                  Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|------------------------ |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqSmall | 382.8 탎 | 16.13 탎 | 35.40 탎 | 381.4 탎 |  3.27 |    0.28 |
| EFCoreInterpolatedSmall | 279.5 탎 |  4.84 탎 | 10.10 탎 | 278.0 탎 |  2.36 |    0.09 |
|          EFCoreRawSmall | 283.5 탎 |  9.54 탎 | 21.35 탎 | 281.6 탎 |  2.39 |    0.18 |
| EFCoreCompliedLinqSmall | 201.2 탎 |  3.38 탎 |  7.42 탎 | 203.9 탎 |  1.70 |    0.06 |
|             DapperSmall | 173.0 탎 |  1.84 탎 |  3.95 탎 | 173.5 탎 |  1.46 |    0.04 |
|             RepoDbSmall | 128.1 탎 |  0.97 탎 |  2.00 탎 | 127.9 탎 |  1.08 |    0.02 |
|              DbMapSmall | 118.3 탎 |  0.53 탎 |  1.12 탎 | 118.1 탎 |  1.00 |    0.00 |
|        HandwrittenSmall | 115.7 탎 |  0.43 탎 |  0.90 탎 | 115.6 탎 |  0.98 |    0.01 |
```

Medium benchmark, query 100 rows of 10 columns using 5 parameters
``` ini
|                   Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|------------------------- |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqMedium | 616.5 탎 | 18.87 탎 | 40.62 탎 | 601.4 탎 |  1.92 |    0.20 |
| EFCoreInterpolatedMedium | 504.9 탎 |  6.29 탎 | 12.99 탎 | 507.9 탎 |  1.58 |    0.12 |
|          EFCoreRawMedium | 528.7 탎 | 13.43 탎 | 29.76 탎 | 517.9 탎 |  1.64 |    0.13 |
| EFCoreCompliedLinqMedium | 425.8 탎 |  9.47 탎 | 19.98 탎 | 419.7 탎 |  1.32 |    0.10 |
|             DapperMedium | 414.8 탎 | 11.65 탎 | 25.33 탎 | 405.8 탎 |  1.29 |    0.15 |
|             RepoDbMedium | 355.3 탎 |  9.48 탎 | 20.40 탎 | 354.4 탎 |  1.11 |    0.12 |
|              DbMapMedium | 322.5 탎 | 12.69 탎 | 26.49 탎 | 313.0 탎 |  1.00 |    0.00 |
|        HandwrittenMedium | 333.5 탎 | 12.94 탎 | 28.41 탎 | 316.9 탎 |  1.04 |    0.10 |
```

Large benchmark, query 1,000 rows of 22 columns using 10 parameters
``` ini
|                  Method |     Mean |     Error |    StdDev |   Median | Ratio | RatioSD |
|------------------------ |---------:|----------:|----------:|---------:|------:|--------:|
|         EFCoreLinqLarge | 3.649 ms | 0.0704 ms | 0.1486 ms | 3.579 ms |  1.23 |    0.07 |
| EFCoreInterpolatedLarge | 3.443 ms | 0.0381 ms | 0.0770 ms | 3.415 ms |  1.16 |    0.05 |
|          EFCoreRawLarge | 3.501 ms | 0.0583 ms | 0.1256 ms | 3.437 ms |  1.18 |    0.06 |
|             DapperLarge | 3.705 ms | 0.0911 ms | 0.1941 ms | 3.600 ms |  1.25 |    0.09 |
|             RepoDbLarge | 3.151 ms | 0.0599 ms | 0.1302 ms | 3.086 ms |  1.06 |    0.06 |
|              DbMapLarge | 2.968 ms | 0.0566 ms | 0.1219 ms | 2.899 ms |  1.00 |    0.00 |
|        HandwrittenLarge | 3.020 ms | 0.0672 ms | 0.1461 ms | 2.938 ms |  1.02 |    0.06 |
```

Extra large benchmark, query 10,000 rows of 22 columns using 10 parameters
``` ini
|                       Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|----------------------------- |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqExtraLarge | 42.79 ms | 0.710 ms | 1.528 ms | 42.11 ms |  1.26 |    0.07 |
| EFCoreInterpolatedExtraLarge | 41.64 ms | 0.812 ms | 1.765 ms | 40.79 ms |  1.23 |    0.08 |
|          EFCoreRawExtraLarge | 42.22 ms | 0.851 ms | 1.850 ms | 41.50 ms |  1.25 |    0.07 |
|             DapperExtraLarge | 42.71 ms | 0.652 ms | 1.375 ms | 41.97 ms |  1.26 |    0.06 |
|             RepoDbExtraLarge | 36.80 ms | 0.654 ms | 1.409 ms | 36.19 ms |  1.08 |    0.06 |
|              DbMapExtraLarge | 34.00 ms | 0.608 ms | 1.281 ms | 33.27 ms |  1.00 |    0.00 |
|        HandwrittenExtraLarge | 34.31 ms | 0.635 ms | 1.395 ms | 33.73 ms |  1.01 |    0.04 |

```