using System.Collections.Generic;
using Chamedoon.Domin.Base;

namespace Chamedoon.Domin.Entity.Countries
{
    public class Country : BaseEntity
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal InvestmentAmount { get; set; }
        public string InvestmentCurrency { get; set; } = string.Empty;
        public string InvestmentNotes { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;
        public string MaritalStatusImpact { get; set; } = string.Empty;

        public ICollection<CountryLivingCost> LivingCosts { get; set; } = new List<CountryLivingCost>();
        public ICollection<CountryRestriction> Restrictions { get; set; } = new List<CountryRestriction>();
    }
}
