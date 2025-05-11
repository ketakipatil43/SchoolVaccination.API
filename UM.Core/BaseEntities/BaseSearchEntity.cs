using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UM.Core.BaseEntities
{
    public class BaseSearchEntity
    {
        public decimal PrimaryKey { get; set; } = 0;
        public int StartIndex { get; set; } = 0;
        public int PageSize { get; set; } = 50;
        public string Sorting { get; set; } = string.Empty;
        public string Search { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
    }
}
