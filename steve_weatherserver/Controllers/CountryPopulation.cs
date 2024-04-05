namespace WeatherServer.Controllers
{
    public class CountryPopulation
    {
        public required string Name { get; set; }
        public int CountryID { get; set; }
        public int Population { get; set; }
    }
}