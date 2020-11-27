using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.Services.ServiceEndpoints.WebApi;
using Newtonsoft.Json;
using VSTS.Infrastructure;

namespace VSTS.Application.Repository.Commands.RepositoryCommands.Create.DotNetCore
{
    public class CreateDotNetCoreRepositoryHandler:IRequestHandler<CreateDotNetCoreRepositoryCommand, bool>
    {
               private readonly IClientFactoryVSTS _vstsClient;

        public CreateDotNetCoreRepositoryHandler(IClientFactoryVSTS vstsClient)
        {
            _vstsClient = vstsClient;
        }

        public async Task<bool> Handle(CreateDotNetCoreRepositoryCommand request, CancellationToken cancellationToken)
        {

  
                var repoCreated= await CreateRepostory(request, cancellationToken);
                if(repoCreated==null) throw new Exception();
                // if it's a private repo
                // var id = await CreateServiceEndpoint(request,repoCreated, cancellationToken);
                // if(id==Guid.Empty) throw new Exception();

               return await ImportCodeInsideRepo(request,new Guid(), cancellationToken);

             
        }

        private async Task<bool> ImportCodeInsideRepo(CreateDotNetCoreRepositoryCommand request,Guid serviceEndpointId, CancellationToken cancellationToken)
        {
                 var client= _vstsClient.GetClient();

               GitImportRequestParameters git = new GitImportRequestParameters()
                {
                        //TODO code source you want to import inside the new repository 
                        GitSource= new GitImportGitSource(){Url="repo source",},
                        ServiceEndpointId=serviceEndpointId,
                        DeleteServiceEndpointAfterImportIsDone=true,
                        TfvcSource=null
                };
                 GitImportRequest projectCreateParameters = new GitImportRequest{
                     Parameters =git,
                     
                 };
                
                
         
                var stringContent = _vstsClient.ConvertBody(projectCreateParameters);
                var response = await client.PostAsync($"{request.Organization}/{request.Project}/_apis/git/repositories/{request.RepositoryName}/importRequests?api-version=5.0-preview",stringContent,cancellationToken);
              
                if (response.IsSuccessStatusCode)
                {
                    using var contents = await response.Content.ReadAsStreamAsync();
                    var responseResult = await System.Text.Json.JsonSerializer.DeserializeAsync
                        <GitImportRequest>(contents);
                    return true;
                }
                else
                {
                    //log
                    return false;
                }
        }

        private async Task<Guid> CreateServiceEndpoint(CreateDotNetCoreRepositoryCommand request, GitRepository repoCreated, CancellationToken cancellationToken)
        {
                var client= _vstsClient.GetClient();
                System.Collections.Generic.IDictionary<string, string> dicoParameters = new Dictionary<string, string> () ;
                //TODO
                dicoParameters.Add("password","Token from Azure devops");
                var CloneEndPoint=$"clone-template";
                
                
                ServiceEndpoint projectCreateParameters = new ServiceEndpoint{
                        Name=CloneEndPoint,
                        Type="Git",
                        Url= new Uri(repoCreated.RemoteUrl),
                        Authorization= new EndpointAuthorization(){
                            Scheme="UsernamePassword",
                            Parameters=dicoParameters
                        },
                        IsShared=true,
                        IsReady=true ,

                    };


                var stringContent = _vstsClient.ConvertBody(projectCreateParameters);
                var response = await client.PostAsync($"{request.Organization}/{repoCreated.ProjectReference.Name}/_apis/serviceendpoint/endpoints?api-version=5.0-preview.2",stringContent,cancellationToken);
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

        private async Task<GitRepository> CreateRepostory(CreateDotNetCoreRepositoryCommand request, CancellationToken cancellationToken)
        {
                 var client= _vstsClient.GetClient();

                GitRepositoryCreateOptions projectCreateParameters = new GitRepositoryCreateOptions{
                     Name=request.RepositoryName
                 };
                

                var stringContent = _vstsClient.ConvertBody(projectCreateParameters);
                var response = await client.PostAsync($"{request.Organization}/{request.Project}/_apis/git/repositories/?api-version=6.0-preview.1",stringContent,cancellationToken);
                   if (response.IsSuccessStatusCode)
                {
                    var contents = await response.Content.ReadAsStringAsync();
                    var responseResult = JsonConvert.DeserializeObject
                        <GitRepository>(contents);
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