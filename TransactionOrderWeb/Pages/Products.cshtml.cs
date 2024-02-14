using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TransactionOrderWeb.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace TransactionOrderWeb.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public List<Product> Products { get; set; }

        public ProductsModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            await LoadProductsAsync();
        }

        public async Task<IActionResult> OnPostAsync(string productName, float price, int categoryID, int stock)
        {
            var httpClient = CreateHttpClient();
            var product = new Product
            {
                ProductName = productName,
                Price = price,
                CategoryID = categoryID,
                Stock = stock
            };

            var response = await httpClient.PostAsJsonAsync($"{_configuration["ApiBaseUrl"]}api/Product", product);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error or set a flag to display an error message
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var httpClient = CreateHttpClient();
            var response = await httpClient.DeleteAsync($"{_configuration["ApiBaseUrl"]}api/Product/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // Handle error or set a flag to display an error message
            }

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

        private async Task LoadProductsAsync()
        {
            var httpClient = CreateHttpClient();
            var response = await httpClient.GetFromJsonAsync<List<Product>>($"{_configuration["ApiBaseUrl"]}api/Product");
            Products = response ?? new List<Product>();
        }
    }
}
