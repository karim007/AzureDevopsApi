using System.Collections.Generic;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace VSTS.Application.Repository.Queries
{
    public class RepositoriesListDto
    {
        public int Count { get; set; }  
        public IEnumerable<GitRepository> value {get;set;}
}
}