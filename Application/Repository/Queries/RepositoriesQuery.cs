using System.Collections.Generic;
using MediatR;
using Microsoft.TeamFoundation.Core.WebApi;

namespace VSTS.Application.Repository.Queries
{
    public class RepositoriesQuery:IRequest<RepositoriesListDto>
    {
        public string Organization { get; set; }
        public string Project { get; set; }
    }
}