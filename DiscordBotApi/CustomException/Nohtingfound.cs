namespace DiscordBotApi.CustomException
{
    public class Nohtingfound: Exception
    {
        public Nohtingfound() : base() { }

        public Nohtingfound(string message) : base(message) { }

        public Nohtingfound(string message, Exception innerException) : base(message, innerException) { }

    }
}
