/* AppHostDemo - (C) 2022 Premysl Fara  */

using System.Net.Http.Json;

using AppHost.Client.Models;


Console.WriteLine("AppHost Client App v1.0.0");

using (var client = new HttpClient
       {
           BaseAddress = new Uri("http://localhost:9999")
       })
{
    for (var i = 1; i <= 10; i++)
    {
        Console.WriteLine("Sending message NO. {0}", i);
    
        //SendLogMessageUsingFormUrlEncodedContent(client, "trace", $"A test message NO. {i}!");
        //SendLogMessageUsingMultipartFormDataContent(client, "trace", $"A test message NO. {i}!");
        SendLogMessageUsingJsonContent(client, "trace", $"A test message NO. {i}!");
        
        Thread.Sleep(1000);
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