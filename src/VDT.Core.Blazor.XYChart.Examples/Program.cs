using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// TODO add example for area chart
// TODO add example for general chart settings? Plot area? Auto scale? Canvas?
// TODO docs <GenerateDocumentationFile>true</GenerateDocumentationFile>
// TODO add layer index to data shapes to prevent double keys for multiple same type layers
// TODO legend
// TODO explain defaults for all customization

namespace VDT.Core.Blazor.XYChart.Examples;

public class Program {
    public static async Task Main(string[] args) {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        await builder.Build().RunAsync();
    }
}
