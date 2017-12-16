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
    public class VehiclesController : Controller
    {
        private readonly IVehiclesService _vehiclesService;
        private readonly PagingOptions _defaultPagingOptions;
        public VehiclesController(IVehiclesService vehiclesService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _vehiclesService = vehiclesService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        // GET api/vehicles
        [HttpGet]
        public async Task<Page<Vehicle>> Get([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            return await _vehiclesService.GetAllAsync(pagingOptions);
        }

        // GET api/vehicles/5
        [HttpGet("{id}")]
        public async Task<Vehicle> Get(int id)
        {
            return await _vehiclesService.GetAsync(id);
        }

        // GET api/vehicles/vin/123
        [HttpGet("vin/{vehicleId}")]
        public async Task<Vehicle> GetByVehicleId(string vehicleId)
        {
            return await _vehiclesService.GetByVehicleId(vehicleId);
        }

        // GET api/vehicles/regno/333
        [HttpGet("regno/{regNo}")]
        public async Task<Vehicle> GetByRegNo(string regNo)
        {
            return await _vehiclesService.GetByRegNo(regNo);
        }

        // GET api/vehicles/customer/1?vehicleStatus=Active&Offset=0&Limit=5
        [HttpGet("customer/{customerId}")]
        public async Task<Page<Vehicle>> GetByCustomerId(int customerId, [FromQuery] SearchVehicleStatus vehicleStatus, [FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            return await _vehiclesService.GetByCustomerId(customerId, vehicleStatus, pagingOptions);
        }

        // GET api/vehicles/status/Active?Offset=0&Limit=5
        [HttpGet("status/{vehicleStatus}")]
        public async Task<Page<Vehicle>> GetByStatus(SearchVehicleStatus vehicleStatus, [FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            return await _vehiclesService.GetByStatus(vehicleStatus, pagingOptions);
        }

        // POST api/vehicles
        [HttpPost]
        [ProducesResponseType(typeof(Vehicle), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody]Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await _vehiclesService.AddAsync(vehicle);
            if (!success)
                return BadRequest();

            return Ok(vehicle);
        }

        // PUT api/vehicles/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Vehicle), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody]Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var success = await _vehiclesService.UpdateAsync(id, vehicle);
                if (!success)
                    return NotFound();
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
            
        }

        // DELETE api/vehicles/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _vehiclesService.DeleteAsync(id);
                if (!success)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        // Post api/vehicles/5
        [HttpPost("ping/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Ping(int id)
        {
            var success = await _vehiclesService.PingAsync(id);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}
