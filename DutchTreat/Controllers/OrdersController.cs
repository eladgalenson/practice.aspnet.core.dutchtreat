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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        // GET: /<controller>/
        private IDutchRepository _dutchRepository { get; set; }
        private ILogger<ProductsController> _logger { get; set; }
        private IMapper _mapper{ get; set; }

        private UserManager<StoreUser> _userManager { get; set; }
        public OrdersController(IDutchRepository dutchRepository, ILogger<ProductsController> logger, IMapper mapper, UserManager<StoreUser> userManager)
        {
            _dutchRepository = dutchRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems=true)
        {
            try
            {
                var userName = User.Identity.Name;
                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(_dutchRepository.GetAllOrdersByName(userName, includeItems)));
            }
            catch (Exception exc)
            {
                _logger.LogError($"problem found {exc.Message}");
                return BadRequest($"problem found {exc.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order =  _dutchRepository.GetOrderById(User.Identity.Name, id);
                if (order != null)
                {
                    return Ok(_mapper.Map<Order,OrderViewModel>(order));
                }
                return NotFound();
            }
            catch (Exception exc)
            {
                _logger.LogError($"problem found {exc.Message}");
                return BadRequest($"problem found {exc.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    if (model.OrderDate == DateTime.MinValue)
                    {
                        model.OrderDate = DateTime.Now;
                    }

                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    
                    var order = _mapper.Map<OrderViewModel, Order>(model);

                    order.User = user;

                    _dutchRepository.AddEntity(order);
                    _dutchRepository.SaveAll();

                  
                    return Created($"api/orders/{order.Id}", _mapper.Map<Order, OrderViewModel>(order));
                }
                else
                {
                    return BadRequest(ModelState);
                }
                
            }
            catch (Exception exc)
            {
                _logger.LogError($"problem found {exc.Message}");
                return BadRequest($"problem found {exc.Message}");
            }
            
        }

    }
}
