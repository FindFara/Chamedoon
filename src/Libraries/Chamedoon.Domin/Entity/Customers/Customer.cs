using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Users;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Domin.Entity.Customers
{
    public class Customer : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Job { get; set; }
        public JobCategory JobCategory { get; set; }
        public int WorkExperienceYears { get; set; }
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImage { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string? MbtiType { get; set; }
        public decimal InvestmentAmount { get; set; }
        public string? FieldCategory { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public string? LanguageCertificate { get; set; }
        public bool WantsFurtherEducation { get; set; }
        public string? SubscriptionPlanId { get; set; }
        public DateTime? SubscriptionStartDateUtc { get; set; }
        public DateTime? SubscriptionEndDateUtc { get; set; }
        public int UsedEvaluations { get; set; }
        public User User { get; set; }
        public ICollection<Payments.PaymentRequest> PaymentRequests { get; set; } = new List<Payments.PaymentRequest>();
        public ICollection<CustomerReport> Reports { get; set; } = new List<CustomerReport>();
    }
}
