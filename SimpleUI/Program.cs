using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;
using SimpleUI;
using SimpleUI.DataAccess;

internal class Program
{
    private static async Task Main(string[] args)
    {
        //https://localhost:7202/api

        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddRefitClient<IAuthorData>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("https://localhost:7202/api");
        });
        builder.Services.AddRefitClient<IBookData>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("https://localhost:7202/api");
        });


        await builder.Build().RunAsync();
    }
}