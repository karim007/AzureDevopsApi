using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSTS.Application.Project.Commands.ProjectCommands.CreateWithAzureSE
{
    public class CreateProjectWithAzureSEValidator : AbstractValidator<CreateProjectWithAzureSECommand>
    {
        public CreateProjectWithAzureSEValidator()
        {
        }
    }
}
