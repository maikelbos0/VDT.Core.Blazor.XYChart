using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorPlayground {

    /*
     * TODO
     * Snap to existing point
     * Add line cap?
     * Add stroke array?
     * Add enable/disable fill
     * Add enable/disable stroke
     * Add opacity
     * Add ellipse
     * Add quadratic bezier curve
     * Add cubic bezier curve
     */

    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
