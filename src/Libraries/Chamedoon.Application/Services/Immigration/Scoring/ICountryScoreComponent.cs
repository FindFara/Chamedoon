namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public interface ICountryScoreComponent
    {
        string Key { get; }
        double Weight { get; }
        double CalculateScore(CountryType country, CountryScoreContext context);
    }
}
