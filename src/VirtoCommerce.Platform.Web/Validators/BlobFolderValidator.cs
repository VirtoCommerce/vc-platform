using System.Text.RegularExpressions;
using FluentValidation;
using VirtoCommerce.Platform.Core.Assets;

namespace VirtoCommerce.Platform.Web.Validators
{
    public class BlobFolderValidator : AbstractValidator<BlobFolder>
    {
        public BlobFolderValidator()
        {
            RuleFor(context => context.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage("Folder name must not be null.")
                .NotEmpty()
                .WithMessage("Folder name must not be empty.")
                .MinimumLength(3)
                .WithMessage(x => $"Minimum length is 3. You entered {x.Name.Length}")
                .MaximumLength(63)
                .WithMessage(x => $"Maximum length is 63. You entered {x.Name.Length}")
                .Must(x => !x.StartsWith("-"))
                .WithMessage("Folder name must not starts with dash symbol.")
                .Must(x => !x.EndsWith("-"))
                .WithMessage("Folder name must not ends with dash symbol.")
                .Must(x => !x.Contains("--"))
                .WithMessage("Folder name must not conatin consecutive dash symbols.")
                .Must(x => new Regex("[0-9a-z -]").IsMatch(x))
                .WithMessage("Folder name must not conatin specsymbols.");
        }
    }
}
