

#nullable enable
namespace CNG.Abstractions.Signatures
{
  public interface ICreated
  {
    string? CreatedUserId { get; set; }

    DateTimeOffset? CreatedAt { get; set; }
  }
}
