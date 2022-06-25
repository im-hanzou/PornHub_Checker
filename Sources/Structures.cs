namespace PHChecker
{
    public class Structures
    {
        public static int ThreadInProgress { get; set; }
    }
    
    public struct UserData
    {
        public string? username;
        public string? password;
    }

    public struct Response
    {
        public bool Success;
        public bool HavePremium;
    }
}
