using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    const string WEATHER_API_KEY = "1b123c9a91468b0da3e0a39c238b2a01";
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

            var url = $"http://api.openweathermap.org/data/2.5/weather?zip={zipcode}&appid={WEATHER_API_KEY}&units=imperial";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            Log.Information("Response code: {StatusCode}", response.StatusCode);

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