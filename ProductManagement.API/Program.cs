using ProductManagement.Application.Helpers;
using ProductManagement.Infrastructure.DbContexts;
using ProductManagement.Infrastructure.Repos;
using ProductManagement.Presentation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddPresentation()
                .AddApplication(builder.Configuration)
                .AddInfrastructure(builder.Configuration);

var app = builder.Build();


app.MapOpenApi().ExcludeFromDescription();
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Product Management API")
        .WithTheme(ScalarTheme.BluePlanet)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);        
});



using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    await SeedData.Initialize(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error"); // the one above
}
app.UseStaticFiles();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
