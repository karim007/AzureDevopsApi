using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VSTS.Application.Repository.Commands.RepositoryCommands.Create.DotNetCore;
using VSTS.Application.Repository.Queries;

namespace VSTS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepositoryController : ControllerBase
    {
 
        private readonly IMediator _mediator;
        
        public RepositoryController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
 
 
        [HttpPost("CreateDotNetCoreRepository")]
        public async Task<IActionResult> CreateDotNetCoreRepository(CreateDotNetCoreRepositoryCommand command)
        {
            return Ok(await _mediator.Send(command));

        }

           
        [HttpGet("Repositories")]
        public async  Task<ActionResult<RepositoriesListDto>>  Get([FromQuery] RepositoriesQuery q){
           return Ok( await _mediator.Send(q));
        }
    }
}
