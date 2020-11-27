using System;
using MediatR;

namespace VSTS.Application.Repository.Commands.RepositoryCommands.Create.DotNetCore
{
    public class CreateDotNetCoreRepositoryCommand: IRequest<bool>
    {
        public string Organization { get; set; }
        public string Project { get; set; }
        public string RepositoryName { get; set; }
        
    }
}