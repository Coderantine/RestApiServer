# RestApiServer
<BUILD_TAGS_HERE>
+ [General Information](#general-information)
+ [Setup](#setup)
+ [Usage](#usage)
+ [License](#license)

## General Information
RestApiServer provides simple REST API based on EF DbContext. It is fast and lightweight and also easy to configure.

## Setup
1. Create ASP.NET Core 3.x application.
2. Create entities and DbContext.
```c#
public class Customer
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
}

public class DemoDbContext : DbContext
{
    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options){}

    public DbSet<Customer> Customers { get; set; }
}
```
3. Then configure it in `startup.cs`.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<DemoDbContext>(options =>
    {
        options.UseInMemoryDatabase("memmory-db");
    });
```

4. Install nuget package 
```
install-package RestApiServer
```

5. Configure RestApiServer
```c#
using RestApiServer;
```

```c#
public void ConfigureServices(IServiceCollection services)
{
    // Configure RestApiServer
    services.AddRestApiServer<DemoDbContext>();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Use RestApiServer middlewere
    app.UseRestApiServer<DemoDbContext>();
}
```

You can find full working example [here](https://github.com/Coderantine/RestApiServer/tree/master/src/RestApiServer.Demo).

## Usage
Run web application. REST endpoints will be awailable in this pattern:
`[HOST]/api/[DBSET_NAME]/[ID]`

#### Get Collection
`GET` `/api/customers`

##### Response  
```json
[
    {
        "id": 1,
        "name": "John",
        "dateOfBirth": "1995-05-05"
    }
]
```

#### Get Signle
`GET` `/api/customers/1`

##### Response 200 (application/json)
```json
{
    "id": 1,
    "name": "John",
    "dateOfBirth": "1995-05-05"
}
```

#### Create
`POST` `/api/customers`

##### Request (application/json)
```json
{
    "name": "Jane",
    "dateOfBirth": "1995-05-05"
}
```

##### Response 200 (application/json)
```json
{
    "id": 2,
    "name": "Jane",
    "dateOfBirth": "1995-05-05"
}
```

#### Update
`PUT` `/api/customers/2`

##### Request (application/json)
```json
{
    "id": 2,
    "name": "Jane",
    "dateOfBirth": "2000-05-05"
}
```

##### Response 200 (application/json)
```json
{
    "id": 2,
    "name": "Jane",
    "dateOfBirth": "2000-05-05"
}
```

#### Delete
`DELETE` `/api/customers/2`

##### Response 204

## License
[MIT License](https://github.com/Coderantine/RestApiServer/blob/master/LICENSE)
