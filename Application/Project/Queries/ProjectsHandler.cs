using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.TeamFoundation.Core.WebApi;
using Newtonsoft.Json;
using VSTS.Infrastructure;

namespace VSTS.Application.Project.Queries
{
    public class ProjectsHandler:IRequestHandler<ProjectstQuery, ProjectListDto>
    {
        private readonly IClientFactoryVSTS _vstsClient;

        public ProjectsHandler(IClientFactoryVSTS vstsClient)
        {
            _vstsClient = vstsClient;
        }
        
                public async Task<ProjectListDto> Handle( ProjectstQuery query, CancellationToken cancellationToken)
        {

    
                
                var client= _vstsClient.GetClient();
         
       
                var response = await client.GetAsync($"{query.Organization}/_apis/projects?api-version=6.0",cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                      string responseBody = await response.Content.ReadAsStringAsync();

                    var responseResult = JsonConvert.DeserializeObject<ProjectListDto>(responseBody);
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