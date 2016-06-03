# ManyConfig

[![NuGet version](https://badge.fury.io/nu/manyconfig.svg)](https://badge.fury.io/nu/manyconfig)
[![Build status](https://ci.appveyor.com/api/projects/status/yy63ivdnny35ra01?svg=true)](https://ci.appveyor.com/project/chugunov/manyconfig)

Helper package for easy configuration

## Usage

Add 'ManyConfig' attribute to your configuration properties and specify DefaultValue for them.

```csharp
public class ElasticConfig
{
    [ManyConfig(Key = "elasticsearch-connection-string",
        DefaultValue = "http://localhost:9200")]
    public string ConnectionString { get; set; }

    [ManyConfig(Key = "elasticsearch-index", 
        DefaultValue = "comments")]
    public string IndexName { get; set; }
}
```

In 'app.config' you can override your DefaultValue property by 'Key' field:

```xml
<appSettings>
  <add key="elasticsearch-connection-string" value="http://127.0.0.1:9200" />
</appSettings>
```

Then you can use configuration:

```csharp
ElasticConfig elasticConfiguration = ConfigHandler.Get<ElasticConfig>();
```
