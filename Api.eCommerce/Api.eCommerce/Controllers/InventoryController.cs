using Library.eCommerce.DTO;
using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Specht2025_Samples.Models;

namespace Api.eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product?> Get()
        {
            return new List<Product?>
            {
                new Product {Id = 1, Name ="Something 1"},
                new Product {Id = 2, Name ="Something 2"},
                new Product {Id = 3, Name ="Something 3"},
            };
        }
    }
}
