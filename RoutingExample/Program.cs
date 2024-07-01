using RoutingExample.CustomContraints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

                     //GetEndpoint() is an extension method that returns the endpoint that will handle the request

//app.Use(async (context, next) =>
//{
//    Endpoint? endpoint = context.GetEndpoint();
//    if (endpoint != null)
//    {
//        await context.Response.WriteAsync($"Endpoint Name: {endpoint.DisplayName}");
//    }
//    await next(context);

//});



//enable routing
app.UseRouting();

//app.Use(async (context, next) =>
//{
//    Endpoint? endpoint = context.GetEndpoint();
//    if (endpoint != null)
//    {
//        await context.Response.WriteAsync($"Endpoint Name: {endpoint.DisplayName}\n");
//        await context.Response.WriteAsync($"Endpoint Route Pattern: {endpoint.RequestDelegate}\n");
//    }
//    await next(context);

//});



                                           //creating endpoints
app.UseEndpoints(endpoints =>
{
    //add your endpoints here

    endpoints.MapGet("/map1", async context =>
    {
        await context.Response.WriteAsync("In Map1\n");
    });

    endpoints.MapPost("/map2", async context =>
    {
        await context.Response.WriteAsync("In Map2\n");
    });


    //Using Route Parameters
    endpoints.Map("/files/{filename}.{extension}", async context =>
    {
        //As it(RouteValues) return values in sytem.object type so we need to convert it to string
        string? filename = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
        await context.Response.WriteAsync($"In Files-{filename}-{extension}\n");
    });

    //Default parameter has been set as Sameer
    endpoints.Map("/employee/profile/{name:length(4,7)=Sameer}", async context =>
    {
        string? name = Convert.ToString(context.Request.RouteValues["name"]);
        await context.Response.WriteAsync($"In Employee Profile-{name}\n");

    });

    //Eg: /products/details/1  -  this is an incoming http request to server from browser
    endpoints.Map("/products/details/{id:int?}", async context =>
    {
        if(context.Request.RouteValues.ContainsKey("id"))
        {
            int? id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"In Products-{id}\n");
        }
        else
        {
            await context.Response.WriteAsync($"In Products-missing id\n");
        }
        
    });

    //Eg: daily-digest-report/{reportdate}
    endpoints.Map("/daily-digest-report/{reportdate:datetime?}", async context => {
        if (context.Request.RouteValues.ContainsKey("reportdate")){
            DateTime? reportdate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
            await context.Response.WriteAsync($"In daily report-{reportdate}\n");
        }
        else
        {
            await context.Response.WriteAsync($"In daily report-missing date\n");
        }
    });

    //Eg: /cities/cityid
    endpoints.Map("/cities/{cityid:guid}", async context =>
    {
        Guid cityid=Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
        await context.Response.WriteAsync($"In Cities-{cityid}\n");
    });

    //Eg: /sales-report/2030/apr

    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);

        await context.Response.WriteAsync($"In sales report-{year}-{month}");
    });

    //Eg: /sales-report/2024/jan

    endpoints.Map("/sales-report/2024/jan", async context =>
    {
        await context.Response.WriteAsync("In sales report-2024-jan\n");
    });


});

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync($"No Route matched at {context.Request.Path}");
});

app.Run();
