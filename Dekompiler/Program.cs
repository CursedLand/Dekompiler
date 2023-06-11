using Dekompiler.Core.Interface;
using Dekompiler.Core;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton(ThemeConfiguration.Default); // change to .Dark to get dark-theme.
builder.Services.AddSingleton<RendererService>();

var app = builder.Build();

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

if (!app.Environment.IsDevelopment())
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();