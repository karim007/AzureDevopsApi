using System.Collections.Generic;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.ServiceEndpoints.WebApi;

namespace VSTS.Application.Endpoint.Queries
{
    public class EndpointListDto
    {
        public int Count { get; set; }  
        public IEnumerable<ServiceEndpoint> value {get;set;}
}
}