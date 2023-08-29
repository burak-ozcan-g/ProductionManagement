using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Dto;
using ProductionManagement.Interfaces;
using ProductionManagement.Models;
using ProductionManagement.Repository;

namespace ProductionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : Controller
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public PackageController(IPackageRepository packageRepository,
            IProductRepository productRepository, 
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _packageRepository = packageRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Package>))]
        public IActionResult GetPackages()
        {
            var packages = _mapper.Map<List<PackageDto>>(_packageRepository.GetPackages());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(packages);
        }

        [HttpGet("{packageId}")]
        [ProducesResponseType(200, Type = typeof(Package))]
        [ProducesResponseType(400)]
        public IActionResult GetPackage(int packageId)
        {
            if (!_packageRepository.PackageExist(packageId))
                return NotFound();

            var package = _mapper.Map<PackageDto>(_packageRepository.GetPackage(packageId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(package);

        }

        [HttpGet("{packageId}/products")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProductsbyPackage(int packageId)
        {
            var products = _mapper.Map<List<ProductDto>>(_packageRepository.GetProductsbyPackage(packageId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);

        }
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreatePackage([FromQuery]int customerId, [FromBody] PackageDto packageCreate)
        {
            if (packageCreate == null)
                return BadRequest(ModelState);

            var package = _packageRepository.GetPackages()
                .Where(p => p.Id == packageCreate.Id).FirstOrDefault();

            if (package != null)
            {
                ModelState.AddModelError("", "Package already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var packageMap = _mapper.Map<Package>(packageCreate);

            packageMap.Customer = _customerRepository.GetCustomer(customerId);

            if (!_packageRepository.CreatePackage(packageMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("packageId")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePackage(int packageId, [FromBody] PackageDto updatedPackage)
        {
            if (updatedPackage == null) return BadRequest(ModelState);
            if (packageId != updatedPackage.Id) return BadRequest(ModelState);
            if (!_packageRepository.PackageExist(packageId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest();

            var packageMap = _mapper.Map<Package>(updatedPackage);

            if (!_packageRepository.UpdatePackage(packageMap))
            {
                ModelState.AddModelError("", "Something went wrong update");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{packageId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePackage(int packageId)
        {
            if (!_packageRepository.PackageExist(packageId)) return NotFound();

            var productsToDelete = _packageRepository.GetProductsbyPackage(packageId);

            var packageToDelete = _packageRepository.GetPackage(packageId);

            if(!ModelState.IsValid) return BadRequest(ModelState);
            if (!_packageRepository.DeletePackage(packageToDelete))
            {
                ModelState.AddModelError("", "Something went wrong delete");
            }
            //if(! _productRepository.DeleteProducts(productsToDelete.ToList()))
            //{
            //    ModelState.AddModelError("", "Something went wrogn delete");
            //}

            return NoContent();

        }
    }
}
