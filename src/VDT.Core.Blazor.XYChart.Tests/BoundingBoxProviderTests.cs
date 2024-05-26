﻿using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class BoundingBoxProviderTests {
    [Fact]
    public void ModuleLocation_Is_Correct() {
        var fileName = System.IO.Path.GetFileName(BoundingBoxProvider.ModuleLocation);

        var expectedFilePath = Directory.GetFiles(System.IO.Path.Combine("..", "..", "..", "..", "VDT.Core.Blazor.XYChart", "wwwroot"), "boundingboxprovider.*.js").Single();
        var expectedFileName = System.IO.Path.GetFileName(expectedFilePath);

        Assert.Equal(expectedFileName, fileName);
    }

    [Fact]
    public void Module_Has_Correct_Fingerprint() {
        using var sha256 = SHA256.Create();

        var filePath = Directory.GetFiles(System.IO.Path.Combine("..", "..", "..", "..", "VDT.Core.Blazor.XYChart", "wwwroot"), "boundingboxprovider.*.js").Single();
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
        var fingerprintFinder = new Regex("boundingboxprovider\\.([a-f0-9]+)\\.js$", RegexOptions.IgnoreCase);
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
        var fingerprint = fingerprintFinder.Match(filePath).Groups[1].Value;
        var fileContents = File.ReadAllBytes(filePath).Where(b => b != '\r').ToArray(); // Normalize newlines between Windows and Linux
        var expectedFingerprint = string.Join("", SHA256.HashData(fileContents).Take(5).Select(b => b.ToString("x2")));

        Assert.Equal(expectedFingerprint, fingerprint);
    }
}
