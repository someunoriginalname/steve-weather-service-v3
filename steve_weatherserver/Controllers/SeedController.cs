using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel.models;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;
using steve_weatherserver.Controllers;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Security.Policy;
using System.Security;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

namespace steve_weatherserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
       
        private readonly CountriesGoldenContext _context;
        private readonly IWebHostEnvironment _env;
        public SeedController(
            CountriesGoldenContext context,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<ActionResult> Import()
        {
            if (!_env.IsDevelopment())
                throw new SecurityException("Not allowed");
            var path = Path.Combine(
                _env.ContentRootPath,
                "Data/worldcities.xlsx");
            using var stream = System.IO.File.OpenRead(path);
            using var excelPackage = new ExcelPackage(stream);
            // get the first worksheet 
            var worksheet = excelPackage.Workbook.Worksheets[0];
            // define how many rows we want to process 
            var nEndRow = worksheet.Dimension.End.Row;
            // create a lookup dictionary 
            // containing all the countries already existing 
            // into the Database (it will be empty on first run).
            var countriesByName = _context.Countries.AsNoTracking()
                .ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            int cityCount = 0;
            int countryCount = 0;
            for(int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[nRow, 1, nRow, worksheet.Dimension.End.Column];
                var name = row[nRow, 5].GetValue<string>();
                if (countriesByName.ContainsKey(name))
                    continue;
                var iso2 = row[nRow, 6].GetValue<string>();
                var iso3 = row[nRow, 7].GetValue<string>();
                var country = new Country
                {
                    Name = name,
                    Iso2 = iso2,
                    Iso3 = iso3
                };

                await _context.Countries.AddAsync(country);
                countriesByName.Add(name, country);
                countryCount++;

            }
            if(countryCount > 0) 
                await _context.SaveChangesAsync();
            var cities = _context.Cities
                    .AsNoTracking()
                    .ToDictionary(x => (
                        
                        Name: x.Name,
                        CityId: x.CityId,
                        Latitude: x.Latitude,
                        Longitude: x.Longitude,
                        Population: x.Population,
                        CountryId: x.CountryId
                    ));
            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[
                    nRow, 1, nRow, worksheet.Dimension.End.Column];
                var name = row[nRow, 1].GetValue<string>();
                var country = row[nRow, 5].GetValue<string>();
                var lat = row[nRow, 3].GetValue<decimal>();
                var lng = row[nRow, 4].GetValue<decimal>();
                var cityid = row[nRow, 11].GetValue <int>();
                var population = row[nRow, 10].GetValue<int>();
                var countryid = countriesByName[country].CountryId;
                if (cities.ContainsKey((
                    Name: name,
                    CityId: cityid,
                    Latitude: lat,
                    Longitude: lng,
                    Population: population,
                    CountryId: countryid
                )))
                    continue;
                var city = new City
                {
                    Name = name,
                    CityId = cityid,
                    Latitude = lat,
                    Longitude = lng,
                    Population = population,
                    CountryId = countryid
                };
                _context.Cities.Add(city);
                cityCount++;
            }
            if(cityCount > 0)
                await _context.SaveChangesAsync();
            return new JsonResult(new
            {
                Cities = cityCount,
                Countries = countryCount
            });
        }
    }
}