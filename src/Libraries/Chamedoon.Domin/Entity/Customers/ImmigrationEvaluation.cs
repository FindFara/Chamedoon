using System;
using Chamedoon.Domin.Base;

namespace Chamedoon.Domin.Entity.Customers
{
    public class ImmigrationEvaluation : BaseEntity
    {
        public long CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public int Age { get; set; }
        public int MaritalStatus { get; set; }
        public int MBTIPersonality { get; set; }
        public decimal InvestmentAmount { get; set; }

        public int JobCategory { get; set; }
        public string? JobTitle { get; set; }
        public int WorkExperienceYears { get; set; }

        public int FieldCategory { get; set; }
        public int DegreeLevel { get; set; }
        public int LanguageCertificate { get; set; }
        public bool WillingToStudy { get; set; }

        public string? Email { get; set; }
        public string? Notes { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
