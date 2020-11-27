using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Core.WebApi;
using VSTS.Application.Project.Commands.ProjectCommands.Create;
using VSTS.Application.Project.Queries;

namespace VSTS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
 
        private readonly IMediator _mediator;
        
        public ProjectController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
 
        [HttpPost]
        public async Task<ActionResult<bool>> Post(CreateProjectCommand command)
        {
           return Ok( await _mediator.Send(command));

        }
        
        [HttpGet]
        public async  Task<ActionResult<ProjectListDto>>  Get([FromQuery] ProjectstQuery q){
           return Ok( await _mediator.Send(q));
        }
    }
}
