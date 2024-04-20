
using CountryModel.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using steve_weatherserver.Controllers;
using System.IdentityModel.Tokens.Jwt;

namespace WeatherServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<WorldCitiesUser> userManager,
        JwtHandler jwtHandler) : ControllerBase
    {
        //We need method
        //NEVER use entity framework direct frame access
        //Only use microsoft identity methods to handle identity
        // We use post because we do not want to see the password in the URL.

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {
            // Question mark means it may be not; if the user doesn't exist, it returns null.
            WorldCitiesUser? user = await userManager.FindByNameAsync(loginRequest.UserName);
            if(user == null)
            {
                // return a 401 error
                return Unauthorized("Bad username or password.");
            }
            bool success = await userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!success)
            {
                return Unauthorized("Bad username or password.");
            }
            // generate token for the user
            JwtSecurityToken token = await jwtHandler.GetTokenAsync(user);
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResult
            {
                Success = true,
                Message = "Welcome",
                Token = jwtToken
            });
        }
    }
}