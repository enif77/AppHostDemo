/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Services;

using AppHost.Models;


public interface ILoggingService
{
    IEnumerable<string> GetLogLevels();
    
    void Log(LogMessage? logMessage);
}