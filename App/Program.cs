using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using Serilog;
using Serilog.Events;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand
        {
            new Option<string>(
                "--zipcode",
                description: "The zipcode to get the current temperature for.")
        };

        rootCommand.Description = "Logs the current temperature for a given zipcode using Serilog.";

        rootCommand.Handler = CommandHandler.Create<string>(async (zipcode) =>
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(new CompactJsonFormatter())
                .CreateLogger();

            var apiKey = "YOUR_API_KEY_HERE";
            var url = $"http://api.openweathermap.org/data/2.5/weather?zip={zipcode}&appid={apiKey}&units=imperial";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<WeatherData>(content);

            Log.Information("The current temperature in {City} is {Temperature}°F.", weatherData.Name, weatherData.Main.Temp);
        });

        return await rootCommand.InvokeAsync(args);
    }
}

public class WeatherData
{
    public string Name { get; set; }
    public MainData Main { get; set; }
}

public class MainData
{
    public double Temp { get; set; }
}