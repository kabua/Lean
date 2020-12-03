;echo off
SETLOCAL
set output=.\package.output
set args=-OutputDirectory %output%

del /f /q %output%\*.* 

nuget pack .\Algorithm\QuantConnect.Algorithm.csproj %args%
nuget pack .\Algorithm.CSharp\QuantConnect.Algorithm.CSharp.csproj %args%
nuget pack .\Algorithm.Framework\QuantConnect.Algorithm.Framework.csproj %args%
nuget pack .\AlgorithmFactory\QuantConnect.AlgorithmFactory.csproj %args%
nuget pack .\Api\QuantConnect.Api.csproj %args%
nuget pack .\Brokerages\QuantConnect.Brokerages.csproj %args%
nuget pack .\Common\QuantConnect.csproj %args%
nuget pack .\Compression\QuantConnect.Compression.csproj %args%
nuget pack .\Configuration\QuantConnect.Configuration.csproj %args%
nuget pack .\Engine\QuantConnect.Lean.Engine.csproj %args%
nuget pack .\Indicators\QuantConnect.Indicators.csproj %args%
nuget pack .\Launcher\QuantConnect.Lean.Launcher.csproj %args%
nuget pack .\Logging\QuantConnect.Logging.csproj %args%
nuget pack .\Messaging\QuantConnect.Messaging.csproj %args%
nuget pack .\Queues\QuantConnect.Queues.csproj %args%