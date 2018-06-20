using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DutchTreat.Controllers
{
    [Route("api/orders/{orderId}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : Controller
    {
        // GET: /<controller>/
        private IDutchRepository _dutchRepository { get; set; }
        private ILogger<ProductsController> _logger { get; set; }
        private IMapper _mapper { get; set; }

        public OrderItemsController(IDutchRepository dutchRepository, ILogger<ProductsController> logger, IMapper mapper)
        {
            _dutchRepository = dutchRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            try
            {

                var order = _dutchRepository.GetOrderById(User.Identity.Name, orderId);
                if (order != null)
                {
                    return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
                }
                
            }
            catch (Exception exc)
            {
                _logger.LogError($"problem found {exc.Message}");
                return BadRequest($"problem found {exc.Message}");
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int id)
        {
            try
            {
                var order = _dutchRepository.GetOrderById(User.Identity.Name, orderId);
                if (order != null)
                {
                    return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(order.Items.Where<OrderItem>(i => i.Id == id).FirstOrDefault()));
                }

            }
            catch (Exception exc)
            {
                _logger.LogError($"problem found {exc.Message}");
                return BadRequest($"problem found {exc.Message}");
            }
            return NotFound();
        }
    }
}
