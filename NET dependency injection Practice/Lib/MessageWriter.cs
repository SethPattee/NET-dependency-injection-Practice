namespace NET_dependency_injection_Practice.Lib
{
    public class MessageWriter : IMessageWriter 
    {
        public void Write(string message)
        {
            Console.WriteLine($"MessageWriter.Write(message: \"{message}\")");
        }
    }
}
