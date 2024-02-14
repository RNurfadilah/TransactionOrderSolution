using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TransactionOrderWeb.Models;

namespace TransactionOrderWeb.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        // Handles the user login
        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync($"{_configuration["ApiBaseUrl"]}api/Auth/login", new { Username = username, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                // Store the token in an httpOnly cookie
                Response.Cookies.Append("JwtToken", tokenResponse.Token, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });
                return RedirectToPage("/Index");
            }

            // Handle login failure
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        // Handles user logout
        public IActionResult OnPostLogout()
        {
            Response.Cookies.Delete("JwtToken");
            return RedirectToPage("/Login");
        }
    }
}