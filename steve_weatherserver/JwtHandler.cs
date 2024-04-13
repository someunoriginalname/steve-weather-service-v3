using CountryModel.models;
using Microsoft.AspNet.Identity;

namespace steve_weatherserver
{
    public class JwtHandler(IConfiguration configuration, UserManager<WorldCitiesUser> userManager)
    {
    }
}
