using System;
using System.Collections.Generic;
using MediatR;

namespace VSTS.Application.Project.Commands.ProjectCommands.CreateWithAzureSE
{
    public class CreateProjectWithAzureSECommand: IRequest<bool>
    {
        public string Organization { get; set; }
        public string ProjectName { get; set; }
        public string ProcessName { get; set; } ="Agile";
        public string VersionControl { get; set; } ="Git";
        public string Description {get;set;}
        
    }
}
