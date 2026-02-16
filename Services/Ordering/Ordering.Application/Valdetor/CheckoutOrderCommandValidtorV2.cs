using FluentValidation;
using Ordering.Application.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Valdetor
{
    public abstract class CheckoutOrderCommandValidtorV2 : AbstractValidator<CheckoutOrderCommandV2>
    {
        public CheckoutOrderCommandValidtorV2()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required")
                .MaximumLength(70).WithMessage("UserName must not exceed 70 characters");
            RuleFor(x => x.TotalaPrice)
                    .GreaterThan(0).WithMessage("TotalPrice must be greater than zero");

        }
    } 
}
