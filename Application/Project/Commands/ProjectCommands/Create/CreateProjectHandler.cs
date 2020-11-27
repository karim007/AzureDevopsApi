using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Newtonsoft.Json;
using VSTS.Infrastructure;

namespace VSTS.Application.Project.Commands.ProjectCommands.Create
{
    public class CreateProjectHandler:IRequestHandler<CreateProjectCommand, bool>
    {
        private readonly IClientFactoryVSTS _vstsClient;

        public CreateProjectHandler(IClientFactoryVSTS vstsClient)
        {
            _vstsClient = vstsClient;
        }

        public async Task<bool> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {

                TeamProject projectCreateParameters = new TeamProject()
                {
                    Name = request.ProjectName,
                    Description = request.Description,
                    Capabilities = SetCapabilities(request.ProcessName, request.VersionControl)
                };
                
                var client= _vstsClient.GetClient();
         
                var stringContent = _vstsClient.ConvertBody(projectCreateParameters);
       
                var response = await client.PostAsync($"{request.Organization}/_apis/projects?api-version=6.0",stringContent,cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    using var contents = await response.Content.ReadAsStreamAsync();
                    var responseResult = await System.Text.Json.JsonSerializer.DeserializeAsync
                        <Response>(contents);
                    return true;
                }
                else
                {
                    //log
                    return false;
                }
        }

        private Dictionary<string, Dictionary<string, string>> SetCapabilities(string processName, string versionControl)
        {
            Dictionary<string, string> processProperties = new Dictionary<string, string>();
            processProperties.Add("Agile","adcc42ab-9882-485e-a3ed-7678f01f66bc");
            processProperties.Add("CMMI","27450541-8e31-4150-9947-dc59f998fc01");
            processProperties.Add("SCRUM","6b724908-ef14-45cf-84f8-768b5384da45");

            Guid processId = Guid.Parse(processProperties[processName]); 

            Dictionary<string, string> processProperaties = new Dictionary<string, string>();

            processProperaties[TeamProjectCapabilitiesConstants.ProcessTemplateCapabilityTemplateTypeIdAttributeName] =
                processId.ToString();

            Dictionary<string, string> versionControlProperties = new Dictionary<string, string>();

            versionControlProperties[TeamProjectCapabilitiesConstants.VersionControlCapabilityAttributeName] = versionControl =="Tfvc"?
                SourceControlTypes.Tfvc.ToString():
                SourceControlTypes.Git.ToString();

            Dictionary<string, Dictionary<string, string>> capabilities = new Dictionary<string, Dictionary<string, string>>();

            capabilities[TeamProjectCapabilitiesConstants.VersionControlCapabilityName] = 
                versionControlProperties;
            capabilities[TeamProjectCapabilitiesConstants.ProcessTemplateCapabilityName] = 
            
                processProperaties;
                return capabilities;
        }
    }

}