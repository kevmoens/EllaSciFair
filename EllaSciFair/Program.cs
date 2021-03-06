using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using EllaSciFair.Data;
using HashidsNet;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<SignUpContext, SignUpContext>();
builder.Services.AddTransient<ITakeANumberRepository, TakeANumberRepository>();
builder.Services.AddTransient<ISignUpRepository, SignUpRepository>();
builder.Services.AddTransient<IHashids, Hashids>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SignUpContext>();
    db.Database.Migrate();
}
//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
