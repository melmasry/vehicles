using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.Enums;
using Vehicles.Entities.HelperEntities;
using Vehicles.Services;

namespace Vehicles.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomersService _customersService;
        private readonly PagingOptions _defaultPagingOptions;

        public CustomersController(ICustomersService customersService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _customersService = customersService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        // GET api/customers?getVehicles=True&vehicleStatus=Active
        [HttpGet]
        public async Task<Page<Customer>> Get([FromQuery] PagingOptions pagingOptions, [FromQuery] bool getVehicles = true, [FromQuery] SearchVehicleStatus vehicleStatus = SearchVehicleStatus.Any)
        {
            //Set default paging options if no paging configuration passed
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            return await _customersService.GetAllAsync(pagingOptions, getVehicles, vehicleStatus);
        }

        // GET api/customers/5
        [HttpGet("{id}")]
        public async Task<Customer> Get(int id)
        {
            return await _customersService.GetAsync(id);
        }

        // GET api/customers/name?name=mohammed&getVehicles=True&vehicleStatus=Active
        [HttpGet("name")]
        public async Task<Page<Customer>> Get([FromQuery] string name = "", [FromQuery] PagingOptions pagingOptions = null, [FromQuery] bool getVehicles = true, 
                                                [FromQuery] SearchVehicleStatus vehicleStatus = SearchVehicleStatus.Any)
        {
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            return await _customersService.GetAsyncByName(name, pagingOptions, getVehicles, vehicleStatus);
        }

        // POST api/customers
        [HttpPost]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await _customersService.AddAsync(customer);
            if (!success)
                return BadRequest();

            return Ok(customer);
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var success = await _customersService.UpdateAsync(id, customer);
                if (!success)
                    return NotFound();
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _customersService.DeleteAsync(id);
                if (!success)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}
