using Chamedoon.Domin.Base;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Domin.Entity.Customers
{
    public class CustomerReport : BaseEntity
    {
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int Age { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string? MbtiType { get; set; }
        public decimal InvestmentAmount { get; set; }

        public JobCategory JobCategory { get; set; }
        public string? JobTitle { get; set; }
        public int WorkExperienceYears { get; set; }

        public string? FieldCategory { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public string? LanguageCertificate { get; set; }
        public bool WantsFurtherEducation { get; set; }

        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
