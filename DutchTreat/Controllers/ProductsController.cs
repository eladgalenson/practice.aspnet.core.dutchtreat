using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private IDutchRepository _dutchRepository { get; set; }
        private ILogger<ProductsController> _logger { get; set; }
        public ProductsController(IDutchRepository dutchRepository, ILogger<ProductsController> logger)
        {
            _dutchRepository = dutchRepository;
            _logger = logger;

        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_dutchRepository.GetAllProducts());
            }
            catch(Exception exc)
            {
                _logger.LogError($"problem found {exc.Message}");
                return BadRequest($"problem found {exc.Message}");
            }
            
        }
    }
}
