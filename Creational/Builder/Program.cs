using System.Text.Json;

var builder = new ObjectToLogBuilder();
var logger = new Logger();

var log = builder.WithPlaceholders(new { UserName = "Peter Cliff" })
    .WithJsonPath("{$.UserName} logged in the system")
    .WithTraceId(Guid.NewGuid().ToString())
    .GetResult();

logger.Log(log);

interface IObjectToLogBuilder
{
    public IObjectToLogBuilder WithPlaceholders(object placeholders);
    public IObjectToLogBuilder WithJsonPath(string message);
    public IObjectToLogBuilder WithTraceId(string id);
    public void Reset();
    public ObjectToLog GetResult();
}

class ObjectToLogBuilder : IObjectToLogBuilder
{
    private ObjectToLog _objectToLog = new ObjectToLog();
    public ObjectToLogBuilder()
    {
    }

    public ObjectToLog GetResult()
    {
        if (string.IsNullOrEmpty(_objectToLog.JsonPathMessage) || _objectToLog.Placeholders == null)
        {
            throw new ArgumentException($"{nameof(_objectToLog.Placeholders)} and {nameof(_objectToLog.JsonPathMessage)} should be initialized");
        }

        var result = _objectToLog;

        Reset();

        return result;
    }

    public void Reset()
    {
        _objectToLog = new ObjectToLog();
    }

    public IObjectToLogBuilder WithJsonPath(string message)
    {
        _objectToLog.JsonPathMessage = message;
        return this;
    }

    public IObjectToLogBuilder WithPlaceholders(object placeholders)
    {
        _objectToLog.Placeholders = placeholders;
        return this;
    }

    public IObjectToLogBuilder WithTraceId(string id)
    {
        _objectToLog.TraceId = id;
        return this;
    }
}

class ObjectToLog
{
    public string TraceId { get; set; }
    public DateTime Date { get; set; }
    public string JsonPathMessage { get; set; }
    public object Placeholders { get; set; }
}

class Logger
{
    public void Log(ObjectToLog data)
    {
        string log = JsonSerializer.Serialize(data);

        Console.WriteLine(log);
    }
}