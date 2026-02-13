using FluentValidation;
using Ordering.Application.Commands;
using System;

namespace Ordering.Application.Valdetor
{
    public class UpdateOrderCommandValidror : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidror()
        {
            // User info
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required")
                .MaximumLength(70).WithMessage("UserName must not exceed 70 characters");

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("EmailAddress is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.FIrstName)
                .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters");

            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("LastName must not exceed 50 characters");

            // Order
            RuleFor(x => x.TotalPrice)
                .GreaterThan(0).WithMessage("TotalPrice must be greater than zero");

            // Address
            RuleFor(x => x.AddressLine)
                .MaximumLength(200).WithMessage("AddressLine must not exceed 200 characters");

            RuleFor(x => x.Country)
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters");

            RuleFor(x => x.State)
                .MaximumLength(50).WithMessage("State must not exceed 50 characters");

            RuleFor(x => x.ZipCode)
                .MaximumLength(20).WithMessage("ZipCode must not exceed 20 characters");

            // Payment
            RuleFor(x => x.PaymentMethod)
                .MaximumLength(30).WithMessage("PaymentMethod must not exceed 30 characters");

            RuleFor(x => x.CardNumber)
                .CreditCard().WithMessage("Invalid card number")
                .When(x => !string.IsNullOrEmpty(x.CardNumber));

            RuleFor(x => x.CardHolderName)
                .MaximumLength(100).WithMessage("CardHolderName must not exceed 100 characters");

            RuleFor(x => x.CVV)
                .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits")
                .When(x => !string.IsNullOrEmpty(x.CVV));

            RuleFor(x => x.CardExpiration)
                .Must(BeAValidExpirationDate)
                .WithMessage("CardExpiration must be a future date")
                .When(x => x.CardExpiration.HasValue);
        }

        private bool BeAValidExpirationDate(DateTime? date)
        {
            return date.HasValue && date.Value.Date > DateTime.UtcNow.Date;
        }
    }
}
