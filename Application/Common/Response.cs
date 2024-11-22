namespace Application.Common;

public class Response<T>
{
    public T? Result { get; set; }
    public ErrorProvider ErrorProvider { get; } = new();
}

public class ErrorProvider
{
    private readonly List<string> _errors = new();

    public void AddError(string source, string message)
    {
        _errors.Add($"[{source}] {message}");
    }

    public bool HasErrors => _errors.Any();

    public List<string> Errors => _errors;
}
