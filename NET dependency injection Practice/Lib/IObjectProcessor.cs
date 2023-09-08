namespace NET_dependency_injection_Practice.Lib
{
    internal interface IObjectProcessor
    {
        Task ProcessAsync(string? next);
    }
}