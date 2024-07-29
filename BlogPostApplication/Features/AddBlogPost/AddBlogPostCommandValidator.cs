using FluentValidation;

namespace BlogPostApplication.Features.AddBlogPost
{
    public class AddBlogPostCommandValidator : AbstractValidator<AddBlogPostCommand>
    {
        public AddBlogPostCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
