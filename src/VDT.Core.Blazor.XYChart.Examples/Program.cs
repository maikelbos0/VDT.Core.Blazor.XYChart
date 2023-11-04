using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// TODO docs <GenerateDocumentationFile>true</GenerateDocumentationFile>
// TODO explain defaults for all customization
// TODO make charts responsive?
// TODO fluent lines?

namespace VDT.Core.Blazor.XYChart.Examples;

public class Program {
    public static async Task Main(string[] args) {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        await builder.Build().RunAsync();
    }
}
