using TestChat.Core.Models.DomainModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestChat.Api.Validations
{
    public class SignUpModelValidation : AbstractValidator<SignUpModel>
    {
        public SignUpModelValidation()
        {
            RuleFor(a => a.FirstName).NotEmpty().MaximumLength(150);
            RuleFor(a => a.LastName).NotEmpty().MaximumLength(150);
            RuleFor(a => a.Email).NotEmpty().EmailAddress().MaximumLength(150);
        }
    }
}
