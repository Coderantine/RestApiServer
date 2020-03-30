namespace RestApiServer
{
    public class RestApiServerOptions
    {
        public bool UsePrefix { get; set; } = true;

        public string Prefix { get; set; } = "/api";
    }
}
