using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSTS.Application.Repository.Commands.RepositoryCommands.Create.DotNetCore
{
    public class CreateDotNetCoreRepositoryValidator : AbstractValidator<CreateDotNetCoreRepositoryCommand>
    {
        public CreateDotNetCoreRepositoryValidator()
        {
        }
    }
}