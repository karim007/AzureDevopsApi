
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VSTS.Infrastructure;

namespace VSTS.Config
{
    public static class Bootstrapper
    {
        public static IServiceCollection ConfigureMediaR(this IServiceCollection service)
        {
            service.AddOptions();
            service.AddMediatR(typeof(Startup));
            service.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            return service;
        }

        public static IServiceCollection ConfigureIOC(this IServiceCollection service){

            service.AddSingleton<IClientFactoryVSTS, ClientFactoryVSTS>();
            return service;
        }

        public static IServiceCollection ConfigureValidation(this IServiceCollection service)
        {
            service.AddMvc().AddFluentValidation(fv =>
                    {
                        fv.RegisterValidatorsFromAssemblyContaining(typeof(Startup));
                    });
            return service;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection service)
        {
            service.AddSwaggerGen(c =>
            {
   
            });
            return service;

        }
    }

            public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
        {
            private readonly IEnumerable<IValidator<TRequest>> _validators;

            public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
            {
                _validators = validators;
            }

            public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
            {
                var context = new ValidationContext<TRequest>(request);

                var failures = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(result => result.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    string exceptions="";
                    failures.ForEach(x => exceptions+=x.ErrorMessage);
                    throw new Exception(exceptions);
                }

                return next();
            }
   
}
}