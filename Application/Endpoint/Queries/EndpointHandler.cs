using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.TeamFoundation.Core.WebApi;
using Newtonsoft.Json;
using VSTS.Infrastructure;

namespace VSTS.Application.Endpoint.Queries
{
    public class EndpointsHandler:IRequestHandler<EndpointstQuery, EndpointListDto>
    {
        private readonly IClientFactoryVSTS _vstsClient;

        public EndpointsHandler(IClientFactoryVSTS vstsClient)
        {
            _vstsClient = vstsClient;
        }
        
                public async Task<EndpointListDto> Handle( EndpointstQuery query, CancellationToken cancellationToken)
        {

    
                
                var client= _vstsClient.GetClient();
         
       
                var response = await client.GetAsync($"{query.Organization}/{query.Project}/_apis/serviceendpoint/endpoints?api-version=6.0-preview.4",cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                      string responseBody = await response.Content.ReadAsStringAsync();

                    var responseResult = JsonConvert.DeserializeObject<EndpointListDto>(responseBody);
                    return responseResult;
                }
                else
                {
                    //log
                    return null;
                }
        }
    }
}