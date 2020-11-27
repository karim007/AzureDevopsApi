using System.Collections.Generic;
using MediatR;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.ServiceEndpoints.WebApi;

namespace VSTS.Application.Endpoint.Queries
{
    public class EndpointstQuery:IRequest<EndpointListDto>
    {
        public string Organization { get; set; }
        public string Project { get; set; }

    }
}