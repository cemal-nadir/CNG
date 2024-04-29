namespace CNG.Abstractions.Models;
public record OrderBy(string fieldName, bool isDescending);

public class SearchFilter
{
    public string CultureCode { get; set; } = "en";
    public virtual List<SearchValue> SearchValues { get; set; } = new();
    public OrderBy? OrderBy { get; set; } = null;
    public int PageSize { get; set; } = 20;
    public int CurrentPage { get; set; } = 1;

    //public int RecordCount { get; set; } = 1;
    //public int TotalPages { get => (RecordCount / PageSize) + 1; }

    public void AddSearchFilter(string fieldName, FilterOperations filterOperation, string fieldValue)
    {

        var searchValue = this.SearchValues.FirstOrDefault(t => t.FieldName == fieldName);
        if (searchValue == null)
        {
            searchValue = new SearchValue
            {
                FieldName = fieldName,
                Operation = filterOperation
            };
            searchValue.FieldValues.Add(fieldValue);
            SearchValues.Add(searchValue);
        }
        else
        {
            searchValue.Operation = filterOperation;
            searchValue.FieldValues.Clear();
            searchValue.FieldValues.Add(fieldValue);
        }
    }
    public override string ToString()
    {
        var result = "?";
        result += $"{nameof(CultureCode)}={CultureCode}";
        result += $"&{nameof(PageSize)}={PageSize}";
        result += $"&{nameof(CurrentPage)}={CurrentPage}";
        if (OrderBy != null)
        {
            result += $"&OrderBy.fieldName={OrderBy.fieldName}";
            result += $"&OrderBy.isDescending={OrderBy.isDescending}";
        }

        if (SearchValues.Count <= 0) return result;
        var svCount = 0;
        foreach (var searchValue in SearchValues)
        {
            result += $"&SearchValues[{svCount}].FieldName={searchValue.FieldName}";
            result += $"&SearchValues[{svCount}].Operation={searchValue.Operation}";
            var fvCount = 0;
            foreach (var fieldValue in searchValue.FieldValues)
            {
                result += $"&SearchValues[{svCount}].FieldValues[{fvCount}]={fieldValue}";
                fvCount++;
            }
            svCount++;
        }
        return result;

    }

}

