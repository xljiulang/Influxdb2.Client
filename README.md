# Influxdb2.Client
Influxdb2的.net高性能客户端库，读写性能为官方客户端库的180%


### 服务注册
```
services.AddInfuxdb(o =>
{
    o.Host = new Uri("http://localhost:8086");
    o.Token = "base64 token value";
    o.DefaultOrg = "my-org";
    o.DefaultBucket = "my-bucket";
});
```

### 写入实体
```
var book = new Book();
await infuxdb.WriteAsync(book);
```
或者
```
var book = new PointBuilder("Book")
    .SetTag("key", "value")
    .SetField("field", "value")
    .Build();
await infuxdb.WriteAsync(book);
```

```
class Book
{
    [ColumnType(ColumnType.Tag)]
    public string Serie { get; set; }


    [ColumnType(ColumnType.Field)]
    public string Name { get; set; }


    [ColumnType(ColumnType.Field)]
    public double? Price { get; set; }


    [ColumnType(ColumnType.Field)]
    public bool? SpecialOffer { get; set; }


    [ColumnType(ColumnType.Timestamp)]
    public DateTimeOffset? CreateTime { get; set; }
}
```

### 读取数据

```
var flux = Flux
    .From(defaultBucket)
    .Range("-3d")
    .Filter(FnBody.R.MeasurementEquals($"{nameof(Book)}"))
    .Pivot()
    .Sort(Columns.Time, desc: true)
    .Limit(10)
    ;

var tables = await infuxdb.QueryAsync(flux);
// 可以映射为任意模型（未必是Book)
var books = tables.Single().ToModels<Book>();
```
