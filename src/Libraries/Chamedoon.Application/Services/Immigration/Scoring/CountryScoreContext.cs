using System;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class CountryScoreContext
    {
        public CountryScoreContext(ImmigrationInput input)
        {
            Input = input;
        }

        public ImmigrationInput Input { get; }

        public bool IsTechProfile =>
            Input.FieldCategory == FieldCategoryType.IT ||
            Input.FieldCategory == FieldCategoryType.Engineering ||
            Input.JobCategory == JobCategoryType.IT ||
            Input.JobCategory == JobCategoryType.Engineering ||
            Input.JobCategory == JobCategoryType.Telecommunications;

        public bool IsMedicalProfile =>
            Input.FieldCategory == FieldCategoryType.Medicine ||
            Input.JobCategory == JobCategoryType.Healthcare;

        public bool IsBusinessProfile =>
            Input.FieldCategory == FieldCategoryType.Business ||
            Input.JobCategory == JobCategoryType.Business ||
            Input.JobCategory == JobCategoryType.Sales ||
            Input.JobCategory == JobCategoryType.Finance;

        public double NormalizedExperience => Math.Clamp(Input.WorkExperienceYears / 10d, 0, 1);

        public double NormalizedInvestment => Math.Clamp((double)Input.InvestmentAmount / 200_000d, 0, 1);
    }
}
