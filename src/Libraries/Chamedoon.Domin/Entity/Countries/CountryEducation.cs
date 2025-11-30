using Chamedoon.Domin.Base;

namespace Chamedoon.Domin.Entity.Countries
{
    public class CountryEducation : BaseEntity
    {
        public long CountryId { get; set; }
        public Country Country { get; set; } = null!;

        public string FieldName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Score { get; set; }
        public string Level { get; set; } = string.Empty;
        public string LanguageRequirement { get; set; } = string.Empty;
    }
}
