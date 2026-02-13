using FluentValidation;
using Ordering.Application.Commands;
using System;

namespace Ordering.Application.Valdetor
{
    public class CheckoutOrderCommandValidtor
        : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidtor()
        {
            // User
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required")
                .MaximumLength(70).WithMessage("UserName must not exceed 70 characters");

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("EmailAddress is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.FIrstName)
                .NotEmpty().WithMessage("FirstName is required")
                .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .MaximumLength(50).WithMessage("LastName must not exceed 50 characters");

            // Order
            RuleFor(x => x.TotalPrice)
                .GreaterThan(0).WithMessage("TotalPrice must be greater than zero");

            // Address
            RuleFor(x => x.AddressLine)
                .NotEmpty().WithMessage("AddressLine is required")
                .MaximumLength(200).WithMessage("AddressLine must not exceed 200 characters");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required")
                .MaximumLength(50).WithMessage("State must not exceed 50 characters");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("ZipCode is required")
                .MaximumLength(20).WithMessage("ZipCode must not exceed 20 characters");

            // Payment
            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("PaymentMethod is required");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("CardNumber is required")
                .CreditCard().WithMessage("Invalid card number")
                .When(x => x.PaymentMethod == "Card");

            RuleFor(x => x.CardHolderName)
                .NotEmpty().WithMessage("CardHolderName is required")
                .MaximumLength(100).WithMessage("CardHolderName must not exceed 100 characters")
                .When(x => x.PaymentMethod == "Card");

            RuleFor(x => x.CVV)
                .NotEmpty().WithMessage("CVV is required")
                .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits")
                .When(x => x.PaymentMethod == "Card");

            RuleFor(x => x.CardExpiration)
                .NotNull().WithMessage("CardExpiration is required")
                .Must(BeAValidExpirationDate)
                .WithMessage("CardExpiration must be a future date")
                .When(x => x.PaymentMethod == "Card");
        }

        private bool BeAValidExpirationDate(DateTime? date)
        {
            return date.HasValue && date.Value.Date > DateTime.UtcNow.Date;
        }
    }
}
