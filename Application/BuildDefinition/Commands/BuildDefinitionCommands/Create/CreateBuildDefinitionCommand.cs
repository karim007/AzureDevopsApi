using System;
using MediatR;

namespace VSTS.Application.BD.Commands.BuildDefinitionCommands.Create
{
    public class CreateBuildDefinitionCommand: IRequest<Guid?>
    {
        public string Organization { get; set; }
        public string Project { get; set; }
        public string Repository { get; set; }
    }
}