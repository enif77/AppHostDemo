/* AppHostDemo - (C) 2022 Premysl Fara  */

Console.WriteLine("AppHost Client App v1.0.0");

using (var client = new HttpClient
       {
           BaseAddress = new Uri("http://localhost:9999")
       })
{
    for (var i = 1; i <= 10; i++)
    {
        Console.WriteLine("Sending message NO. {0}", i);
    
        var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("logLevel", "trace"),
            new("message", $"A test message NO. {i}!")
        });

        try
        {
            var response = client.PostAsync("/log", content).Result; 
        
            Console.WriteLine("Status: {0}", response.IsSuccessStatusCode
                ? "OK"
                : "FAILED");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
        
        Thread.Sleep(1000);
    }
}
