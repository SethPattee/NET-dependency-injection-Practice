namespace NET_dependency_injection_Practice.Lib
{
    internal interface IObjectStore
    {
        Task<string?> GetNextAsync();
        Task<string?> MarkAsync(string? next);
    }
}