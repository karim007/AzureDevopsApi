using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSTS.Application.BD.Commands.BuildDefinitionCommands.Create
{
    public class CreateBuildDefinitionValidator : AbstractValidator<CreateBuildDefinitionCommand>
    {
        public CreateBuildDefinitionValidator()
        {

        }
    }
}