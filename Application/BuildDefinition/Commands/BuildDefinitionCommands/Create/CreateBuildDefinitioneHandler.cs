using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.TeamFoundation.Build.WebApi;

using Newtonsoft.Json;
using VSTS.Infrastructure;


namespace VSTS.Application.BD.Commands.BuildDefinitionCommands.Create
{
    public class CreateBuildDefinitionHandler:IRequestHandler<CreateBuildDefinitionCommand, Guid?>
    {
        private readonly IClientFactoryVSTS _vstsClient;
        public CreateBuildDefinitionHandler(IClientFactoryVSTS vstsClient)
        {
            _vstsClient = vstsClient;
        }
        public async Task<Guid?> Handle(CreateBuildDefinitionCommand request, CancellationToken cancellationToken)
        {
                var client= _vstsClient.GetClient();

    
              BuildDefinition projectCreateParameters = new BuildDefinition{

                JobTimeoutInMinutes = 60,
                
                JobCancelTimeoutInMinutes = 5,
                //TODO azure pipeline should be inside the repo
                Process = new YamlProcess(){YamlFilename="azure-pipelines.yml"},
                Repository = new BuildRepository{
                    Url=new Uri($"https://{request.Organization}.visualstudio.com/${request.Project}/_git/${request.Repository}"),
                    DefaultBranch="master",
                    Type = "TfsGit",
                    Name=$"{request.Repository}"
                }, 
                    
                Name = $"{request.Repository}-build",
                Revision = 1,
                Type=DefinitionType.Build,
                Queue = new AgentPoolQueue{

                Name = "ubuntu-latest",
                Pool = new TaskAgentPoolReference
                {
                    Name = "ubuntu-latest",
                    IsHosted = true
                }


                }
                    
                 };
                
                
         
                var stringContent = _vstsClient.ConvertBody(projectCreateParameters);
                try
                {

                var response = await client.PostAsync($"{request.Organization}/{request.Project}/_apis/build/definitions?api-version=6.0",stringContent,cancellationToken);
              
                if (response.IsSuccessStatusCode)
                {
                     string responseBody = await response.Content.ReadAsStringAsync();

                     var responseResult = JsonConvert.DeserializeObject<BuildDefinition>(responseBody);

                    return new Guid();
                }
                else
                {
                    //log
                    return new Guid();
                }
                }
                catch (Exception e){
                    var d= e.Message;
                    return new Guid();
                }
        }

   
    }

}