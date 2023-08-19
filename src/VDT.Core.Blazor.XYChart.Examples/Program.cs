using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// TODO improve examples
// TODO add chart style as example
// TODO warnings & messages
// TODO docs <GenerateDocumentationFile>true</GenerateDocumentationFile>
// TODO figure out how to reload when rendering... render twice? on parameters set?

namespace VDT.Core.Blazor.XYChart.Examples;

public class Program {
    public static async Task Main(string[] args) {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        await builder.Build().RunAsync();
    }
}
