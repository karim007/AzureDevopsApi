using System.Collections.Generic;
using MediatR;
using Microsoft.TeamFoundation.Core.WebApi;

namespace VSTS.Application.BD.Queries
{
    public class BuildDefinitionstQuery:IRequest<BuildDefinitionListDto>
    {
        public string Organization { get; set; }
        public string Project { get; set; }
    }
}