namespace CNG.ChromeDriver.Models
{
    public class ProxyModel
    {
        public ProxyModel(string ip, string port, string? username = null, string? password = null)
        {
            Ip=ip;
            Port = port;
            Username = username;
            Password = password;
        }

        public string Ip { get; set; }
        public string Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
