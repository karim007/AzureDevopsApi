using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VSTS.Application.BD.Commands.BuildDefinitionCommands.Create;
using VSTS.Application.BD.Queries;

namespace VSTS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildDefinitionController : ControllerBase
    {
 
        private readonly IMediator _mediator;
        
        public BuildDefinitionController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
 
        [HttpPost]
        public IEnumerable<string> Post(CreateBuildDefinitionCommand command)
        {
            _mediator.Send(command);
            return null;

        }

        [HttpGet("List")]
        public async  Task<ActionResult<BuildDefinitionListDto>>  Get([FromQuery] BuildDefinitionstQuery q){
           return Ok( await _mediator.Send(q));
        }
    }
}
