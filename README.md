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
|      Method |     Mean |   Error |  StdDev | Ratio | RatioSD |
|------------ |---------:|--------:|--------:|------:|--------:|
| EFCoreSmall | 157.4 us | 0.99 us | 2.09 us |  1.56 |    0.02 |
| DapperSmall | 134.1 us | 0.81 us | 1.67 us |  1.33 |    0.02 |
| RepoDbSmall | 120.2 us | 1.30 us | 2.71 us |  1.19 |    0.03 |
|  DbMapSmall | 100.7 us | 0.47 us | 0.99 us |  1.00 |    0.00 |
```

Medium benchmark, query 100 rows of 10 columns using 5 parameters
```cmd
|       Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|------------- |---------:|---------:|---------:|------:|--------:|
| EFCoreMedium | 399.0 us | 13.59 us | 30.40 us |  1.30 |    0.15 |
| DapperMedium | 368.9 us | 12.88 us | 27.18 us |  1.21 |    0.13 |
| RepoDbMedium | 363.5 us | 18.94 us | 42.35 us |  1.20 |    0.18 |
|  DbMapMedium | 307.2 us | 13.18 us | 28.38 us |  1.00 |    0.00 |
```

Large benchmark, query 10.000 rows of 22 columns using 10 parameters
```cmd
|      Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|------------ |---------:|---------:|---------:|------:|--------:|
| EFCoreLarge | 42.75 ms | 0.452 ms | 0.945 ms |  1.25 |    0.04 |
| DapperLarge | 42.48 ms | 0.309 ms | 0.639 ms |  1.24 |    0.04 |
| RepoDbLarge | 37.14 ms | 0.277 ms | 0.585 ms |  1.09 |    0.03 |
|  DbMapLarge | 34.23 ms | 0.393 ms | 0.846 ms |  1.00 |    0.00 |
```