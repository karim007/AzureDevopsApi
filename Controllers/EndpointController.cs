using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Services.ServiceEndpoints.WebApi;
using VSTS.Application.Endpoint.Queries;

namespace VSTS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EndpointController : ControllerBase
    {
 
        private readonly IMediator _mediator;
        
        public EndpointController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
 
 

        [HttpGet("Endpoints")]
        public async  Task<ActionResult<EndpointListDto>>  Get([FromQuery] EndpointstQuery q){
           return Ok( await _mediator.Send(q));
        }
    }
}
