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
|          Method |     Mean |   Error |   StdDev | Ratio | RatioSD |
|---------------- |---------:|--------:|---------:|------:|--------:|
| EFCoreSmallLinq | 161.7 us | 4.15 us |  9.02 us |  1.60 |    0.09 |
|     EFCoreSmall | 249.1 us | 5.33 us | 11.93 us |  2.45 |    0.12 |
|     DapperSmall | 135.1 us | 0.95 us |  2.08 us |  1.33 |    0.02 |
|     RepoDbSmall | 120.5 us | 1.33 us |  2.77 us |  1.19 |    0.03 |
|      DbMapSmall | 101.3 us | 0.44 us |  0.95 us |  1.00 |    0.00 |
```

Medium benchmark, query 100 rows of 10 columns using 5 parameters
```cmd
|           Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|----------------- |---------:|---------:|---------:|---------:|------:|--------:|
| EFCoreMediumLinq | 375.1 us |  4.55 us |  9.40 us | 376.0 us |  1.25 |    0.08 |
|     EFCoreMedium | 477.9 us | 10.82 us | 23.28 us | 477.2 us |  1.59 |    0.11 |
|     DapperMedium | 369.0 us | 12.35 us | 26.85 us | 361.8 us |  1.23 |    0.11 |
|     RepoDbMedium | 337.3 us | 20.05 us | 44.42 us | 320.5 us |  1.11 |    0.14 |
|      DbMapMedium | 301.0 us |  8.58 us | 18.47 us | 301.6 us |  1.00 |    0.00 |
```

Large benchmark, query 10.000 rows of 22 columns using 10 parameters
```cmd
|          Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|---------------- |---------:|---------:|---------:|------:|--------:|
| EFCoreLargeLinq | 42.90 ms | 0.480 ms | 1.023 ms |  1.27 |    0.04 |
|     EFCoreLarge | 41.63 ms | 0.353 ms | 0.753 ms |  1.24 |    0.03 |
|     DapperLarge | 42.89 ms | 0.430 ms | 0.907 ms |  1.27 |    0.04 |
|     RepoDbLarge | 36.78 ms | 0.338 ms | 0.714 ms |  1.09 |    0.03 |
|      DbMapLarge | 33.67 ms | 0.307 ms | 0.654 ms |  1.00 |    0.00 |
```