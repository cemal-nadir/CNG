

#nullable enable
namespace CNG.Abstractions.Signatures
{
  public interface ICreated
  {
    string? CreatedUser { get; set; }

    DateTime CreatedAt { get; set; }
  }
}
