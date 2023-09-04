using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// TODO improve examples
// TODO docs <GenerateDocumentationFile>true</GenerateDocumentationFile>
// TODO perhaps add a class element to data series to allow custom styling per series, could even replace line width
// TODO maybe rename data shapes _again_ to reverse data-x to x-data

namespace VDT.Core.Blazor.XYChart.Examples;

public class Program {
    public static async Task Main(string[] args) {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        await builder.Build().RunAsync();
    }
}
