# DbMap
Performance-oriented database-to-object mapper for .NET

Features
- Easy to setup and use
- Performant

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

Small benchmark, query 1 row of 6 columns using 1 parameter
```cmd
|                  Method |      Mean |     Error |    StdDev |    Median |      P95 | Ratio | RatioSD |
|------------------------ |----------:|----------:|----------:|----------:|---------:|------:|--------:|
|         EFCoreLinqSmall | 389.53 us | 25.409 us | 56.831 us | 407.26 us | 449.2 us |  4.12 |    0.62 |
|          EFCoreRawSmall | 255.31 us | 12.464 us | 27.620 us | 254.69 us | 307.2 us |  2.72 |    0.32 |
| EFCoreInterpolatedSmall | 275.82 us | 16.283 us | 36.420 us | 273.48 us | 333.1 us |  2.93 |    0.41 |
|             DapperSmall | 127.53 us |  1.253 us |  2.671 us | 126.48 us | 131.3 us |  1.36 |    0.06 |
|             RepoDbSmall | 100.54 us |  0.769 us |  1.606 us | 100.16 us | 102.4 us |  1.07 |    0.04 |
|              DbMapSmall |  93.90 us |  1.774 us |  3.819 us |  92.74 us | 106.7 us |  1.00 |    0.00 |
```

Medium benchmark, query 100 rows of 10 columns using 5 parameters
```cmd
|                   Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|------------------------- |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqMedium | 626.9 us | 27.16 us | 60.74 us | 602.9 us |  1.95 |    0.28 |
|          EFCoreRawMedium | 523.4 us | 24.42 us | 52.04 us | 506.5 us |  1.62 |    0.27 |
| EFCoreInterpolatedMedium | 546.4 us | 25.40 us | 56.81 us | 535.0 us |  1.70 |    0.24 |
|             DapperMedium | 379.0 us | 10.78 us | 23.66 us | 377.9 us |  1.17 |    0.15 |
|             RepoDbMedium | 363.1 us | 20.50 us | 45.01 us | 341.7 us |  1.12 |    0.20 |
|              DbMapMedium | 327.9 us | 16.90 us | 35.65 us | 315.0 us |  1.00 |    0.00 |
```

Large benchmark, query 10.000 rows of 22 columns using 10 parameters
```cmd
|                  Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|------------------------ |---------:|---------:|---------:|---------:|------:|--------:|
|         EFCoreLinqLarge | 42.36 ms | 0.305 ms | 0.643 ms | 42.37 ms |  1.27 |    0.04 |
|          EFCoreRawLarge | 42.03 ms | 0.689 ms | 1.468 ms | 41.62 ms |  1.26 |    0.06 |
| EFCoreInterpolatedLarge | 41.68 ms | 0.446 ms | 0.931 ms | 41.59 ms |  1.24 |    0.04 |
|             DapperLarge | 42.67 ms | 0.403 ms | 0.867 ms | 42.57 ms |  1.28 |    0.04 |
|             RepoDbLarge | 37.27 ms | 0.382 ms | 0.822 ms | 36.97 ms |  1.11 |    0.04 |
|              DbMapLarge | 33.48 ms | 0.426 ms | 0.916 ms | 33.32 ms |  1.00 |    0.00 |
```