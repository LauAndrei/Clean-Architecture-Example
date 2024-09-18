using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> _validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Please provide a description");

        RuleFor(dto => dto.Category)
            .Must(category => _validCategories.Contains(category))
            .WithMessage("Invalid category. Please choose from the valid categories");
        // .Custom((value, context) =>
        // {
        //     var isValidCategory = validCategories.Contains(value);
        //
        //     if (!isValidCategory)
        //     {
        //         context.AddFailure(nameof(CreateRestaurantDto.Category),
        //             "Invalid category. Please choose from the valid categories");
        //     }
        // });

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX).");
    }
}