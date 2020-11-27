using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.TeamFoundation.Core.WebApi;
using Newtonsoft.Json;
using VSTS.Infrastructure;

namespace VSTS.Application.Repository.Queries
{
    public class RepositoriesHandler:IRequestHandler<RepositoriesQuery, RepositoriesListDto>
    {
        private readonly IClientFactoryVSTS _vstsClient;

        public RepositoriesHandler(IClientFactoryVSTS vstsClient)
        {
            _vstsClient = vstsClient;
        }
        
                public async Task<RepositoriesListDto> Handle( RepositoriesQuery query, CancellationToken cancellationToken)
        {

    
                
                var client= _vstsClient.GetClient();
         
       
                var response = await client.GetAsync($"{query.Organization}/{query.Project}/_apis/git/repositories?api-version=6.0",cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                      string responseBody = await response.Content.ReadAsStringAsync();

                    var responseResult = JsonConvert.DeserializeObject<RepositoriesListDto>(responseBody);
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