using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Text.Json;

namespace Website.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public Search SearchParams { get; set; }

        public List<Restroom> Restrooms { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            using var client = new HttpClient();

            var url = $"https://www.refugerestrooms.org/api/v1/restrooms/by_location?page=1&per_page=10&offset=0&lat={SearchParams.Latitude.ToString(CultureInfo.InvariantCulture)}&lng={SearchParams.Longtitude.ToString(CultureInfo.InvariantCulture)}";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into C# objects
                Restrooms = JsonSerializer.Deserialize<List<Restroom>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            // redisplay the page with results
            return Page();
        }
    }

    public class Search
    {
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
    }

    public class Restroom
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool Accessible { get; set; }
        public bool Unisex { get; set; }
        public string Directions { get; set; }
        public string Comment { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
