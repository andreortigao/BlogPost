using FluentValidation;

namespace BlogPostApplication.Features.AddComment
{
    public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidator()
        {
            RuleFor(x => x.BlogPostId).GreaterThan(0);
            RuleFor(x => x.Model.UserName).NotEmpty();
            RuleFor(x => x.Model.Content).NotEmpty();
        }
    }
}
