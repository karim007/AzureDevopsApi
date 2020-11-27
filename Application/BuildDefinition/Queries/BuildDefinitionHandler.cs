using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.TeamFoundation.Core.WebApi;
using Newtonsoft.Json;
using VSTS.Infrastructure;

namespace VSTS.Application.BD.Queries
{
    public class BuildDefinitionsHandler:IRequestHandler<BuildDefinitionstQuery, BuildDefinitionListDto>
    {
        private readonly IClientFactoryVSTS _vstsClient;

        public BuildDefinitionsHandler(IClientFactoryVSTS vstsClient)
        {
            _vstsClient = vstsClient;
        }
        
                public async Task<BuildDefinitionListDto> Handle( BuildDefinitionstQuery query, CancellationToken cancellationToken)
        {

    
                
                var client= _vstsClient.GetClient();
         
       
                var response = await client.GetAsync($"{query.Organization}/{query.Project}/_apis/build/definitions?api-version=6.0",cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                      string responseBody = await response.Content.ReadAsStringAsync();

                    var responseResult = JsonConvert.DeserializeObject<BuildDefinitionListDto>(responseBody);
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