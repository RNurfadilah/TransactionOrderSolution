using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TransactionOrderWeb.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace TransactionOrderWeb.Pages
{
    [AllowAnonymous]
    public class OrdersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public List<Order> Orders { get; set; }
        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();

        public OrdersModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            var httpClient = CreateHttpClient(); // Use the encapsulated method to create HttpClient
            var responseOrder = await httpClient.GetFromJsonAsync<List<Order>>($"{_configuration["ApiBaseUrl"]}api/Order");
            Orders = responseOrder ?? new List<Order>();

            var responseProducts = await httpClient.GetFromJsonAsync<List<ProductViewModel>>($"{_configuration["ApiBaseUrl"]}api/Product");
            Products = responseProducts ?? new List<ProductViewModel>();
        }

        public async Task<IActionResult> OnPostAsync(DateTime transactionDate, string transactionNumber, float totalAmount)
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                username = "God";
            }

            var httpClient = CreateHttpClient();

            var order = new Order
            {
                TransactionDate = transactionDate,
                TransactionNumber = transactionNumber,
                CashierName = username,
                TotalAmount = totalAmount
            };

            var orderResponse = await httpClient.PostAsJsonAsync($"{_configuration["ApiBaseUrl"]}api/Order", order);
            if (!orderResponse.IsSuccessStatusCode)
            {
                var errorContent = await orderResponse.Content.ReadAsStringAsync();
                return BadRequest($"Error creating order: {errorContent}");
            }

            var createdOrder = await orderResponse.Content.ReadFromJsonAsync<Order>();
            if (createdOrder == null)
            {
                return BadRequest("Failed to create order.");
            }

            var productResponse = await httpClient.GetFromJsonAsync<Product>($"{_configuration["ApiBaseUrl"]}api/Product/{transactionNumber}");
            if (productResponse == null)
            {
                return BadRequest("Product not found.");
            }

            float totalPrice = productResponse.Price * totalAmount;

            var orderDetail = new OrderDetail
            {
                TransactionID = createdOrder.TransactionID,
                ProductID = productResponse.ProductID,
                CategoryID = productResponse.CategoryID,
                TotalPrice = totalPrice
            };

            var detailResponse = await httpClient.PostAsJsonAsync($"{_configuration["ApiBaseUrl"]}api/OrderDetail", orderDetail);
            if (!detailResponse.IsSuccessStatusCode)
            {
                var errorContent = await detailResponse.Content.ReadAsStringAsync();
                return BadRequest($"Error creating order detail: {errorContent}");
            }

            return RedirectToPage();
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

        public async Task OnGetEditAsync(string transactionId)
        {
            var httpClient = CreateHttpClient();
            var order = await httpClient.GetFromJsonAsync<Order>($"{_configuration["ApiBaseUrl"]}api/Order/{transactionId}");
        }

        public async Task<IActionResult> OnPostDeleteAsync(string transactionId)
        {
            var httpClient = CreateHttpClient();
            var response = await httpClient.DeleteAsync($"{_configuration["ApiBaseUrl"]}api/Order/{transactionId}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Orders");
            }

            return Page();
        }
    }
}
