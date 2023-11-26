namespace LandisGyrProject
{
    public class ServiceLog : IServiceLog
    {
        private string[] logsInMemory { get; set; } = new string[] { };
        public void Log(string message)
        {
            logsInMemory.Append(message);
        }
    }
}
