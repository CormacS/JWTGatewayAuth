using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace SimpleApiGateway
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly TokenService _tokenService;
        public ApiController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        // Generate a JWT Token
        [HttpGet("generate-token")]
        public IActionResult GenerateToken()
        {

            var token = _tokenService.GenerateToken("testuser");
            return Ok(new { token });
        }

        // Make an authenticated call to an external API
        [Authorize]
        [HttpGet("external-api")]
        public async Task<IActionResult> CallExternalApi()
        {
            var client = new RestClient("https://meowfacts.herokuapp.com/?count=3"); // Example public API
            var request = new RestRequest("", Method.Get);
            var response = await client.ExecuteAsync(request);

            if(!response.IsSuccessful)
                return StatusCode((int)response.StatusCode, response.ErrorMessage);

            return Ok(response.Content);
        }
    }
}
