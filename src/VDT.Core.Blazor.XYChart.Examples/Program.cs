using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// TODO add chart style as package file and as template
// TODO multiple target frameworks
// TODO build action
// TODO improve examples
// TODO warnings & messages

namespace VDT.Core.Blazor.XYChart.Examples;

public class Program {
    public static async Task Main(string[] args) {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        await builder.Build().RunAsync();
    }
}
