using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TransactionOrderWeb.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace TransactionOrderWeb.Pages
{
    [AllowAnonymous]
    public class CategoriesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public List<Category> Categories { get; set; }
        [BindProperty]
        public Category Category { get; set; }

        public CategoriesModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            await LoadCategoriesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = CreateHttpClient();
            if (Category.CategoryID == 0)
            {
                // Create
                await httpClient.PostAsJsonAsync($"{_configuration["ApiBaseUrl"]}api/Category", Category);
            }
            else
            {
                // Update
                await httpClient.PutAsJsonAsync($"{_configuration["ApiBaseUrl"]}api/Category/{Category.CategoryID}", Category);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var httpClient = CreateHttpClient();
            await httpClient.DeleteAsync($"{_configuration["ApiBaseUrl"]}api/Category/{id}");
            return RedirectToPage();
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Request.Cookies["JwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }

        private async Task LoadCategoriesAsync()
        {
            var httpClient = CreateHttpClient();
            var response = await httpClient.GetFromJsonAsync<List<Category>>($"{_configuration["ApiBaseUrl"]}api/Category");
            Categories = response ?? new List<Category>();
        }
    }
}
