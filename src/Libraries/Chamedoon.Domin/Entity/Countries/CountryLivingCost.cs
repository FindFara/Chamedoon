using Chamedoon.Domin.Base;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Domin.Entity.Countries
{
    public class CountryLivingCost : BaseEntity
    {
        public long CountryId { get; set; }
        public Country Country { get; set; } = null!;

        public LivingCostType Type { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
