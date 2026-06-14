using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.ViewComponents
{
    public class CityMapViewComponent : ViewComponent
    {
        private readonly ICityService _cityService;

        private static readonly Dictionary<string, (string Left, string Top, int Size)> CityCoordinates = new()
    {
        { "İstanbul",  ("38%", "42%", 20) },
        { "Ankara",    ("54%", "36%", 16) },
        { "İzmir",     ("28%", "55%", 14) },
        { "Bursa",     ("42%", "38%", 12) },
        { "Antalya",   ("36%", "68%", 12) },
        { "Adana",     ("62%", "62%", 11) },
        { "Konya",     ("60%", "40%", 10) },
        { "Gaziantep", ("72%", "55%", 10) },
        { "Kayseri",   ("67%", "35%", 9)  },
        { "Mersin",    ("76%", "72%", 8)  },
        { "Kocaeli",   ("41%", "35%", 8)  },
        { "Diyarbakır",("75%", "45%", 8)  },
        { "Samsun",    ("62%", "22%", 8)  },
        { "Trabzon",   ("74%", "18%", 7)  },
        { "Eskişehir", ("48%", "40%", 7)  },
    };

        public CityMapViewComponent(ICityService cityService)
        {
            _cityService = cityService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cities = await _cityService.GetTopCitiesAsync();
            var maxRevenue = cities.Any() ? cities.Max(x => x.TotalRevenue) : 1;

            var mapCities = cities
                .Where(c => CityCoordinates.ContainsKey(c.CityName))
                .Select(c =>
                {
                    var coords = CityCoordinates[c.CityName];
                    var opacity = 0.3 + (double)(c.TotalRevenue / maxRevenue) * 0.7;
                    return new
                    {
                        c.CityName,
                        c.TotalRevenue,
                        c.TotalOrders,
                        coords.Left,
                        coords.Top,
                        Size = coords.Size,
                        Opacity = opacity.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                    };
                })
                .ToList();

            return View(mapCities);
        }
    }
}
