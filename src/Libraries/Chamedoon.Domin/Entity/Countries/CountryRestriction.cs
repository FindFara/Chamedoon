using Chamedoon.Domin.Base;

namespace Chamedoon.Domin.Entity.Countries
{
    public class CountryRestriction : BaseEntity
    {
        public long CountryId { get; set; }
        public Country Country { get; set; } = null!;

        public string Description { get; set; } = string.Empty;
    }
}
