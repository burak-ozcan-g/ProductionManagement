using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Dto;
using ProductionManagement.Interfaces;
using ProductionManagement.Models;
using ProductionManagement.Repository;
using System;

namespace ProductionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerController(
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        public IActionResult GetCustomers()
        {
            var customers = _mapper.Map<List<CustomerDto>>(_customerRepository.GetCustomers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(customers);
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(400)]
        public IActionResult GetCustomer(int customerId)
        {
            if (!_customerRepository.CustomerExist(customerId))
                return NotFound();

            var customer = _mapper.Map<CustomerDto>(_customerRepository.GetCustomer(customerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(customer);

        }

        [HttpGet("{customerId}/packages")]
        [ProducesResponseType(200, Type = typeof(Package))]
        [ProducesResponseType(400)]
        public IActionResult GetPackagesACustomer(int customerId)
        {
            if (!_customerRepository.CustomerExist(customerId))
                return NotFound();

            var packages = _mapper.Map<List<PackageDto>>(_customerRepository.GetPackagesACustomer(customerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(packages);

        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerCreate)
        {
            if (customerCreate == null)
                return BadRequest(ModelState);

            var customer = _customerRepository.GetCustomerTrimToUpper(customerCreate);

            //var customer = _customerRepository.GetCustomers()
            //    .Where(c => c.Name.Trim().ToUpper() == customerCreate.Name.TrimEnd().ToUpper())
            //    .FirstOrDefault();

            if (customer != null)
            {
                ModelState.AddModelError("", "Customer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerMap = _mapper.Map<Customer>(customerCreate);

            if (!_customerRepository.CreateCustomer(customerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("customerId")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCustomer(int customerId, [FromBody] CustomerDto updatedCustomer)
        {
            if (updatedCustomer == null) return BadRequest(ModelState);
            if (customerId != updatedCustomer.Id) return BadRequest(ModelState);
            if (!_customerRepository.CustomerExist(customerId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest();

            var customerMap = _mapper.Map<Customer>(updatedCustomer);

            if (!_customerRepository.UpdateCustomer(customerMap))
            {
                ModelState.AddModelError("", "Something went wrong update");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{customerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCustomer(int customerId)
        {
            if (!_customerRepository.CustomerExist(customerId)) return NotFound();

            var customerToDelete = _customerRepository.GetCustomer(customerId);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_customerRepository.DeleteCustomer(customerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong delete");
            }

            return NoContent();

        }
    }
}
