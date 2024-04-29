namespace CNG.Abstractions.Models
{
    public sealed class SearchValue
    {
        public string FieldName { get; set; }
        public FilterOperations Operation { get; set; }
        public ICollection<string> FieldValues { get; set; }
        public SearchValue()
        {
            FieldValues = new HashSet<string>();
            FieldName = string.Empty;
        }
    }
}
