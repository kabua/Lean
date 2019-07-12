using System.Reflection;

// common assembly attributes
[assembly: AssemblyDescription("Lean Engine is an open-source, platform agnostic C# and Python algorithmic trading engine. " +
                               "Allows strategy research, backtesting and live trading with Equities, FX, CFD, Crypto, Options and Futures Markets.")]
[assembly: AssemblyCopyright("QuantConnect™ 2018. All Rights Reserved")]
[assembly: AssemblyCompany("QuantConnect Corporation")]
[assembly: AssemblyVersion("101.0")]

// Configuration used to build the assembly is by defaulting 'Debug'.
// To create a package using a Release configuration, -properties Configuration=Release on the command line must be use.
// source: https://docs.microsoft.com/en-us/nuget/reference/nuspec#replacement-tokens