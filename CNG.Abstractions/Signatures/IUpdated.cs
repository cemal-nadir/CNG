
namespace CNG.Abstractions.Signatures
{
  public interface IUpdated
  {
    string? UpdatedUserId { get; set; }

    DateTime? UpdatedAt { get; set; }
  }
}
