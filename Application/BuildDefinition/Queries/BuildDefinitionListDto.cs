using System.Collections.Generic;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;

namespace VSTS.Application.BD.Queries
{
    public class BuildDefinitionListDto
    {
        public int Count { get; set; }  
        public IEnumerable<BuildDefinitionReference> value {get;set;}
}
}