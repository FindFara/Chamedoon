using Chamedoon.Domin.Base;

namespace Chamedoon.Domin.Entity.Countries
{
    public class CountryJob : BaseEntity
    {
        public long CountryId { get; set; }
        public Country Country { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Score { get; set; }
        public string ExperienceImpact { get; set; } = string.Empty;
    }
}
