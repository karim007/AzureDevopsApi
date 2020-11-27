using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSTS.Application.Project.Commands.ProjectCommands.Create
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectValidator()
        {
            // RuleFor(x => x.FirstName).NotEmpty().MinimumLength(5).MaximumLength(50);
        }
    }
}