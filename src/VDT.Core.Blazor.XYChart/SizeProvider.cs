﻿using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace VDT.Core.Blazor.XYChart;

internal class SizeProvider : ISizeProvider {
    internal const string ModuleLocation = "./_content/VDT.Core.Blazor.XYChart/sizeprovider.8bb0df6555.js";

    public static async Task<ISizeProvider> Create(IJSRuntime jsRuntime) {
        var moduleReference = await jsRuntime.InvokeAsync<IJSObjectReference>("import", ModuleLocation);

        await moduleReference.InvokeVoidAsync("register");

        return new SizeProvider(moduleReference);
    }

    private readonly IJSObjectReference moduleReference;

    public SizeProvider(IJSObjectReference moduleReference) {
        this.moduleReference = moduleReference;
    }

    public async Task<TextSize> GetTextSize(string text, string? cssClass)
        => await moduleReference.InvokeAsync<TextSize>("getTextSize", text, cssClass);

    /// <inheritdoc/>
    public async ValueTask DisposeAsync() {
        if (moduleReference != null) {
            await moduleReference.InvokeVoidAsync("unregister");
            await moduleReference.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}