/* AppHostDemo - (C) 2022 Premysl Fara  */

using System.Globalization;
using System.Net.Http.Json;

using AppHost.Client.Models;


Console.WriteLine("AppHost Client App v1.0.0");

var port = 80;
var repeatsCount = 10;

ParseArgs(args);

using (var client = new HttpClient
       {
           BaseAddress = new Uri($"http://localhost:{port}")
       })
{
    for (var i = 1; i <= repeatsCount; i++)
    {
        Console.WriteLine("Sending message NO. {0}", i);
    
        //SendLogMessageUsingFormUrlEncodedContent(client, "trace", $"A test message NO. {i}!");
        //SendLogMessageUsingMultipartFormDataContent(client, "trace", $"A test message NO. {i}!");
        SendLogMessageUsingJsonContent(client, "trace", $"A test message NO. {i}!");
        
        Thread.Sleep(1000);
    }
}


void ParseArgs(string[] args)
{
    foreach (var a in args)
    {
        if (a.StartsWith("-p=", StringComparison.InvariantCulture))
        {
            var v = a.Substring(3);
            var success = int.TryParse(v, NumberStyles.Integer, CultureInfo.InvariantCulture, out var p);
            if (success && p >= 80)
            {
                port = p;
                Console.WriteLine("The port is set to {0}.", port);
            }
            else
            {
                Console.Error.WriteLine("The '{0}' port value cannot be parsed or is less than 80.", v);
            }
        }
        else if (a.StartsWith("-r=", StringComparison.InvariantCulture))
        {
            var v = a.Substring(3);
            var success = int.TryParse(v, NumberStyles.Integer, CultureInfo.InvariantCulture, out var r);
            if (success && r >= 1)
            {
                repeatsCount = r;
                Console.WriteLine("The repeats count is set to {0}.", repeatsCount);
            }
            else
            {
                Console.Error.WriteLine("The '{0}' repeats count value cannot be parsed or is less than 1.", v);
            }
        }
    }
}


void SendLogMessageUsingFormUrlEncodedContent(HttpClient httpClient, string logLevel, string message)
{
    using var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
    {
        new("logLevel", logLevel),
        new("message", message)
    });

    try
    {
        var response = httpClient.PostAsync("/log", content).Result; 
        
        Console.WriteLine("Status: {0}", response.IsSuccessStatusCode
            ? "OK"
            : "FAILED");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
    }
}


void SendLogMessageUsingMultipartFormDataContent(HttpClient httpClient, string logLevel, string message)
{
    // https://brokul.dev/sending-files-and-additional-data-using-httpclient-in-net-core
    using var content = new MultipartFormDataContent
    {
        { new StringContent(logLevel), "logLevel" },
        { new StringContent(message), "message" },
    };

    try
    {
        var response = httpClient.PostAsync("/log", content).Result; 
        
        Console.WriteLine("Status: {0}", response.IsSuccessStatusCode
            ? "OK"
            : "FAILED");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
    }
}


void SendLogMessageUsingJsonContent(HttpClient httpClient, string logLevel, string message)
{
    // https://www.stevejgordon.co.uk/sending-and-receiving-json-using-httpclient-with-system-net-http-json
    var content = new LogMessage()
    {
        LogLevel = logLevel,
        Message = message
    };

    try
    {
        var response = httpClient.PostAsJsonAsync("/logMessage", content).Result; 
        
        Console.WriteLine("Status: {0}", response.IsSuccessStatusCode
            ? "OK"
            : "FAILED");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
    }
}