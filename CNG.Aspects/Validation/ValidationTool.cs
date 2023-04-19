#nullable enable
using CNG.Extensions;
using FluentValidation;
using FluentValidation.Results;

namespace CNG.Aspects.Validation
{
  public static class ValidationTool
  {
    public static void Validate(IValidator validator, object obj)
    {
      ValidationResult validationResult = validator.Validate(new ValidationContext<object>(obj));
      if (validationResult.IsValid) return;
      var notContains = new[]
      {
          "ComparisonProperty",
          "ComparisonValue"
      };
      throw new ValidationException(validationResult.Errors.Select<ValidationFailure, ValidationErrorResponse>(x => new ValidationErrorResponse()
      {
          ErrorCode = x.ErrorCode,
          ErrorMessage = x.ErrorMessage,
          PlaceHolderValues = x.FormattedMessagePlaceholderValues.Where((Func<KeyValuePair<string, object>, bool>) (y => !notContains.Contains(y.Key))).ToList()
      }).ToList().ToJson());
    }
  }
}
