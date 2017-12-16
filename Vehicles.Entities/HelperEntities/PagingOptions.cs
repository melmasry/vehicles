using System.ComponentModel.DataAnnotations;

namespace Vehicles.Entities.HelperEntities
{
    public sealed class PagingOptions
    {
        public const int MaxPageSize = 100;
        
        [Range(1, int.MaxValue, ErrorMessage = "Offset value must be greater than 0")]
        public int? Offset { get; set; }
        
        [Range(1, MaxPageSize, ErrorMessage = "Limit value must be greater than 0 and less than 100")]
        public int? Limit { get; set; }
    }
}
