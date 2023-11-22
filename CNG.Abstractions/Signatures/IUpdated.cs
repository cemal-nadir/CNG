
namespace CNG.Abstractions.Signatures
{
  public interface IUpdated
  {
    string? UpdatedUserId { get; set; }

    DateTimeOffset? UpdatedAt { get; set; }
  }
}
