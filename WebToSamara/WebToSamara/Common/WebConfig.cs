namespace WebToSamara.Common
{
    public class WebConfig
    {
        public string ClientId { get; set; }
        public string Os { get; set; }
        public string Secret_key { get; set; }
        public string RequestUrl { get; set; }

        public WebConfig(string clientId, string os, string secret_key, string requestUrl)
        {
            ClientId = clientId;
            Os = os;
            Secret_key = secret_key;
            RequestUrl = requestUrl;
        }
    }
}
