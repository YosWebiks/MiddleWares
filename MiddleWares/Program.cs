using static System.Net.Mime.MediaTypeNames;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.Use(async (HttpContext ctx, Func<Task> next) =>
{

    if (ctx.Request.Query["blah"] == "not-blah" || !ctx.Request.Path.Equals("/Home/classified"))
    {
        await next();
    }
    else
    {
        ctx.Response.StatusCode = 403;
        await ctx.Response.WriteAsync("You're Not an Admin, you shall not pass!");
    }
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
