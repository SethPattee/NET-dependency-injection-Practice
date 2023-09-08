namespace NET_dependency_injection_Practice.Lib
{
    internal interface IObjectRelay
    {
        Task RelayAsync(string? next);
    }
}