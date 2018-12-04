using AgileTrackingSystem.Data;
using AgileTrackingSystem.Data.Entities;
using AgileTrackingSystem.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace AgileTrackingSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly IDBRepository _dBRepository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDBRepository dBRepository, 
                                ILogger<OrdersController> logger,
                                IMapper mapper)
        {
            _dBRepository = dBRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;
                var results = _dBRepository.GetAllOrders(includeItems);
                return Ok(_mapper.Map<IEnumerable<Order>,IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get orders: {e}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var username = User.Identity.Name;
                var order = _dBRepository.GetAllOrderById(username,id);
                if (order != null)
                    return Ok(_mapper.Map<Order,OrderViewModel>(order));
                else return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get orders: {e}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderViewModel, Order>(model);
                    if (newOrder.OrderDate == DateTime.Now)
                        newOrder.OrderDate = DateTime.Now;

                    _dBRepository.AddEntity(newOrder);
                    if (_dBRepository.SaveAll())                    {
                        
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order,OrderViewModel>(newOrder));
                    }
                }
                else return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to save a new order: {e}");
            }
            return BadRequest("Failed to create new order.");
        }
    }
}
