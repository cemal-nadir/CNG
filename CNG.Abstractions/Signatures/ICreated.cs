

#nullable enable
namespace CNG.Abstractions.Signatures
{
  public interface ICreated
  {
    string? CreatedUserId { get; set; }

    DateTime CreatedAt { get; set; }
  }
}
