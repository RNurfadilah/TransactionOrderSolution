using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TransactionOrderWeb.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;

namespace TransactionOrderWeb.Pages
{
    [AllowAnonymous]
    public class OrderDetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public List<OrderDetail> OrderDetails { get; set; }

        public OrderDetailsModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var token = HttpContext.Request.Cookies["JwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.GetFromJsonAsync<List<OrderDetail>>($"{_configuration["ApiBaseUrl"]}api/OrderDetail");

            if (response != null)
            {
                OrderDetails = response;
            }
            else
            {
                OrderDetails = new List<OrderDetail>();
            }
        }
    }
}