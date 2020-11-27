using System.Collections.Generic;
using MediatR;
using Microsoft.TeamFoundation.Core.WebApi;

namespace VSTS.Application.Project.Queries
{
    public class ProjectstQuery:IRequest<ProjectListDto>
    {
        public string Organization { get; set; }
    }
}