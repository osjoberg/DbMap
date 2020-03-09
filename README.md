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
|          Method |     Mean |   Error |   StdDev |   Median | Ratio | RatioSD |
|---------------- |---------:|--------:|---------:|---------:|------:|--------:|
| EFCoreSmallLinq | 161.5 us | 5.28 us | 10.78 us | 158.9 us |  1.60 |    0.11 |
|     EFCoreSmall | 251.6 us | 5.99 us | 13.39 us | 256.8 us |  2.48 |    0.13 |
|     DapperSmall | 133.0 us | 0.56 us |  1.17 us | 132.7 us |  1.31 |    0.02 |
|     RepoDbSmall | 120.4 us | 1.43 us |  3.01 us | 120.3 us |  1.19 |    0.03 |
|      DbMapSmall | 101.2 us | 0.56 us |  1.19 us | 101.1 us |  1.00 |    0.00 |
```

Medium benchmark, query 100 rows of 10 columns using 5 parameters
```cmd
|           Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|----------------- |---------:|---------:|---------:|---------:|------:|--------:|
| EFCoreMediumLinq | 393.2 us | 12.54 us | 27.53 us | 387.4 us |  1.25 |    0.17 |
|     EFCoreMedium | 478.0 us | 11.14 us | 24.69 us | 477.1 us |  1.53 |    0.21 |
|     DapperMedium | 367.7 us | 12.26 us | 26.65 us | 359.6 us |  1.17 |    0.14 |
|     RepoDbMedium | 342.0 us | 20.77 us | 45.15 us | 326.9 us |  1.09 |    0.17 |
|      DbMapMedium | 318.3 us | 18.36 us | 40.31 us | 302.1 us |  1.00 |    0.00 |
```

Large benchmark, query 10.000 rows of 22 columns using 10 parameters
```cmd
|          Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|---------------- |---------:|---------:|---------:|------:|--------:|
| EFCoreLargeLinq | 42.56 ms | 0.443 ms | 0.924 ms |  1.26 |    0.04 |
|     EFCoreLarge | 41.86 ms | 0.443 ms | 0.933 ms |  1.24 |    0.04 |
|     DapperLarge | 43.04 ms | 0.410 ms | 0.864 ms |  1.27 |    0.05 |
|     RepoDbLarge | 37.17 ms | 0.350 ms | 0.737 ms |  1.10 |    0.03 |
|      DbMapLarge | 33.83 ms | 0.415 ms | 0.894 ms |  1.00 |    0.00 |

```