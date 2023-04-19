#nullable enable
namespace CNG.Aspects.Validation
{
  public class ValidationException : Exception
  {
    public ValidationException(string message)
      : base(message)
    {
    }
  }
}
