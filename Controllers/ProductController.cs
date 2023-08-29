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
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProducts()
        {
            var products = _mapper.Map<List<ProductDto>>(_productRepository.GetProducts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(int id)
        {
            if (!_productRepository.ProductExist(id))
                return NotFound();

            var product = _mapper.Map<ProductDto>(_productRepository.GetProduct(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(product);

        }

        [HttpGet("{productId}/packages")]
        [ProducesResponseType(200, Type = typeof(Package))]
        [ProducesResponseType(400)]
        public IActionResult GetPackagesbyProduct(int productId)
        {
            var packages = _mapper.Map<List<PackageDto>>(_productRepository.GetPackagesbyProduct(productId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(packages);

        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct(int packageId, [FromBody] ProductDto productCreate, int units)
        {
            if (productCreate == null)
                return BadRequest(ModelState);

            var product = _productRepository.GetProductTrimToUpper(productCreate);

            if (product != null)
            {
                ModelState.AddModelError("", "Product already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productMap = _mapper.Map<Product>(productCreate);

            if (!_productRepository.CreateProduct(packageId, productMap, units))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }
        [HttpPut("productId")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductDto updatedProduct)
        {
            if (updatedProduct == null) return BadRequest(ModelState);
            if (productId != updatedProduct.Id) return BadRequest(ModelState);
            if (!_productRepository.ProductExist(productId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest();

            var productMap = _mapper.Map<Product>(updatedProduct);

            if (!_productRepository.UpdateProduct(productMap))
            {
                ModelState.AddModelError("", "Something went wrong update");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int productId)
        {
            if (!_productRepository.ProductExist(productId)) return NotFound();

            var productToDelete = _productRepository.GetProduct(productId);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_productRepository.DeleteProduct(productToDelete))
            {
                ModelState.AddModelError("", "Something went wrong delete");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
