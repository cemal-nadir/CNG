
namespace CNG.Core.Exceptions
{
  public class ValidationException
  {
    public string? ErrorCode { get; set; }

    public string? ErrorMessage { get; set; }

    public List<PlaceHolderValue>? PlaceHolderValues { get; set; }

    public class PlaceHolderValue
    {
      public string? Key { get; set; }

      public string? Value { get; set; }
    }
  }
}
