
namespace CNG.Abstractions.Signatures
{
  public interface IUpdated
  {
    string? UpdatedUser { get; set; }

    DateTime? UpdatedAt { get; set; }
  }
}
