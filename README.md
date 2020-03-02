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
|      Method |     Mean |   Error |  StdDev |
|------------ |---------:|--------:|--------:|
| EFCoreSmall | 163.3 us | 2.72 us | 5.92 us | => ~ 46% slower than DbMap
| DapperSmall | 141.2 us | 3.19 us | 6.92 us | => ~ 26% slower than DbMap
| RepoDbSmall | 122.6 us | 1.45 us | 3.05 us | => ~ 15% slower than DbMap
|  DbMapSmall | 103.4 us | 1.31 us | 2.79 us |
```

Medium benchmark, query 100 rows of 10 columns using 5 parameters
```cmd
|       Method |     Mean |    Error |   StdDev |
|------------- |---------:|---------:|---------:|
| EFCoreMedium | 382.8 us |  7.52 us | 16.04 us | => ~ 17% slower than DbMap
| DapperMedium | 387.6 us | 17.32 us | 38.02 us | => ~ 18% slower than DbMap
| RepoDbMedium | 334.9 us | 10.50 us | 23.05 us | => ~  5% slower than DbMap
|  DbMapMedium | 315.0 us | 14.70 us | 31.95 us |
```

Large benchmark, query 10.000 rows of 22 columns using 10 parameters
```cmd
|      Method |     Mean |    Error |   StdDev |
|------------ |---------:|---------:|---------:|
| EFCoreLarge | 46.16 ms | 0.661 ms | 1.394 ms | => ~ 16% slower than DbMap
| DapperLarge | 46.33 ms | 0.398 ms | 0.840 ms | => ~ 16% slower than DbMap
| RepoDbLarge | 40.40 ms | 0.381 ms | 0.805 ms | => ~  5% slower than DbMap
|  DbMapLarge | 38.04 ms | 0.382 ms | 0.813 ms |
```