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
        
        var customer = CustomerQuery.ExecuteQuerySingleOrDefault<Customer>(connection, new { customerId = 10 });
        
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
|       Method |     Mean |   Error |  StdDev |
|------------- |---------:|--------:|--------:|
|  EFCoreSmall | 161.9 us | 4.09 us | 8.97 us | => ~ 44% slower than DbMap
|  DapperSmall | 138.8 us | 4.25 us | 9.25 us | => ~ 23% slower than DbMap
|  RepoDbSmall | 121.2 us | 1.31 us | 2.76 us | => ~ 12% slower than DbMap
|   DbMapSmall | 106.2 us | 3.58 us | 7.63 us |
```

Medium benchmark, query 100 rows of 10 columns using 5 parameters
```cmd
|        Method |     Mean |    Error |   StdDev |
|-------------- |---------:|---------:|---------:|
|  EFCoreMedium | 399.6 us | 13.91 us | 29.34 us | => ~ 21% slower than DbMap
|  DapperMedium | 369.8 us | 11.53 us | 24.82 us | => ~ 14% slower than DbMap
|  RepoDbMedium | 334.0 us | 11.10 us | 24.60 us | => ~  5% slower than DbMap
|   DbMapMedium | 315.2 us | 13.35 us | 29.85 us |
```

Large benchmark, query 10.000 rows of 22 columns using 10 parameters
```cmd
|       Method |     Mean |    Error |   StdDev |
|------------- |---------:|---------:|---------:|
|  EFCoreLarge | 47.60 ms | 0.450 ms | 0.949 ms | => ~ 16% slower than DbMap
|  DapperLarge | 48.02 ms | 0.353 ms | 0.736 ms | => ~ 17% slower than DbMap
|  RepoDbLarge | 42.38 ms | 0.349 ms | 0.745 ms | => ~  6% slower than DbMap
|   DbMapLarge | 39.66 ms | 0.345 ms | 0.728 ms |
```