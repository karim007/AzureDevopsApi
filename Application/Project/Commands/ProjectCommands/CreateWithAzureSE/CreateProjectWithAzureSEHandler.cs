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
using Microsoft.VisualStudio.Services.ServiceEndpoints.WebApi;
using Newtonsoft.Json;
using VSTS.Infrastructure;

namespace VSTS.Application.Project.Commands.ProjectCommands.CreateWithAzureSE
{
    public class CreateProjectWithAzureSEHandler:IRequestHandler<CreateProjectWithAzureSECommand, bool>
    {
        private readonly IClientFactoryVSTS _vstsClient;

        public CreateProjectWithAzureSEHandler(IClientFactoryVSTS vstsClient)
        {
            _vstsClient = vstsClient;
        }

        public async Task<bool> Handle(CreateProjectWithAzureSECommand request, CancellationToken cancellationToken)
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
                   await CreateServiceEndpoint(request,cancellationToken);
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

            versionControlProperties[TeamProjectCapabilitiesConstants.VersionControlCapabilityAttributeName] = versionControl=="Tfvc"?
                SourceControlTypes.Tfvc.ToString():
                SourceControlTypes.Git.ToString();

            Dictionary<string, Dictionary<string, string>> capabilities = new Dictionary<string, Dictionary<string, string>>();

            capabilities[TeamProjectCapabilitiesConstants.VersionControlCapabilityName] = 
                versionControlProperties;
            capabilities[TeamProjectCapabilitiesConstants.ProcessTemplateCapabilityName] = 
            
                processProperaties;
                return capabilities;
        }

             private async Task<Guid> CreateServiceEndpoint( CreateProjectWithAzureSECommand request,CancellationToken cancellationToken)
        {
                var client= _vstsClient.GetClient();
                System.Collections.Generic.IDictionary<string, string> dicoParameters = new Dictionary<string, string> () ;
                //TODO
                dicoParameters.Add("ServicePrincipalId","find id from azure");
                dicoParameters.Add("authenticationType","spnKey");
                dicoParameters.Add("ServicePrincipalKey","find id from azure");
                dicoParameters.Add("tenantId","find id from azure");

                var azureEndPoint=$"vsts-azure-service-connection";
                //TODO

                        var data= new Dictionary<string,string>();
                        data.Add("SubscriptionId","find id from azure");
                        data.Add("SubscriptionName","find id from azure");

                ServiceEndpoint projectCreateParameters = new ServiceEndpoint{
                        Data=data,
                        Name=azureEndPoint,                        
                        Type="azurerm",
                        Authorization= new EndpointAuthorization(){
                            Scheme="ServicePrincipal",
                            Parameters=dicoParameters
                        },

                    };


                var stringContent = _vstsClient.ConvertBody(projectCreateParameters);
                var response = await client.PostAsync($"{request.Organization}/{request.ProjectName}/_apis/serviceendpoint/endpoints?api-version=5.0-preview.2",stringContent,cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var contents = await response.Content.ReadAsStringAsync();
                    var responseResult = JsonConvert.DeserializeObject
                        <ServiceEndpoint>(contents);
   

                    return responseResult.Id;
                }
                else
                {
                    //log
                    return new Guid();
                }
            
        }

    }

}