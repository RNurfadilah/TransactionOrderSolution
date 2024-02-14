using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TransactionOrderWeb.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace TransactionOrderWeb.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public List<Order> Orders { get; set; }

        public OrdersModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            var httpClient = CreateHttpClient(); // Use the encapsulated method to create HttpClient
            var response = await httpClient.GetFromJsonAsync<List<Order>>($"{_configuration["ApiBaseUrl"]}api/Order");
            Orders = response ?? new List<Order>();
        }

        // Encapsulate HttpClient creation with authorization header setup
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
    }
}
