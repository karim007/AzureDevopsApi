using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace VSTS.Infrastructure
{
    public interface IClientFactoryVSTS
    {
        HttpClient GetClient();
        StringContent ConvertBody(object o);
    }

    public class ClientFactoryVSTS : IClientFactoryVSTS
    {
         private readonly IHttpClientFactory _clientFactory;

        public ClientFactoryVSTS(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public StringContent ConvertBody(object o)
        {
             var json = JsonConvert.SerializeObject(o); // or JsonSerializer.Serialize if using System.Text.Json
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
                return stringContent;
        }

        public HttpClient GetClient()
        {
            var client = _clientFactory.CreateClient();

                //TODO Token from Azure Devops
                string credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", "Token from Azure Devops")));
                client.BaseAddress=new Uri("https://dev.azure.com/");;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials); 
                return client;
        }
    }
}