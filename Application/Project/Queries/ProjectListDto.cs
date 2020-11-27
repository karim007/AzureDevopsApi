using System.Collections.Generic;
using Microsoft.TeamFoundation.Core.WebApi;

namespace VSTS.Application.Project.Queries
{
    public class ProjectListDto
    {
        public int Count { get; set; }  
        public IEnumerable<TeamProjectReference> value {get;set;}
}
}