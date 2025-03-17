namespace CNG.Abstractions.Signatures
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        string? DeletedUserId { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
