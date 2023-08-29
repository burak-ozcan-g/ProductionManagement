using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Controllers;
using ProductionManagement.Dto;
using ProductionManagement.Interfaces;
using ProductionManagement.Models;
using ProductionManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionManagement.Tests.Controller
{
    public class ProductControllerTests
    {
        private readonly IProductRepository _productSut;
        private readonly IMapper _mapperSut;

        public ProductControllerTests()
        {
            _productSut = A.Fake<IProductRepository>();
            _mapperSut = A.Fake<IMapper>();
        }

        //GetAll
        [Fact]
        public void GetProductsReturnsListOfProducts()
        {
            // Arrange
            var products = GetProductsData();
            var gettedProduct = A.Fake<List<Product>>();

            A.CallTo(() => _productSut.GetProducts())
                .Returns(gettedProduct);
            A.CallTo(() => _mapperSut.Map<List<ProductDto>>(gettedProduct))
                .Returns(products);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.GetProducts();
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void GetProducts_InvalidProduct_ReturnsBadRequest()
        {
            // Arrange
            var controller = new ProductController(_productSut, _mapperSut);
            controller.ModelState.AddModelError("UnitCost", "UnitCost must be positive value!");
            // Act
            var result = controller.GetProducts();
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        //GetbyId
        [Fact]
        public void GetProduct_ExistingProductIdReturnsProduct()
        {
            var productId = 15;
            var product = A.Fake<Product>();
            // Arrange
            A.CallTo(() => _productSut.ProductExist(productId))
                .Returns(true);
            A.CallTo(() => _productSut.GetProduct(productId))
                .Returns(product);
            A.CallTo(() => _mapperSut.Map<ProductDto>(product))
                .Returns(new ProductDto
                {
                    Id = productId,
                    Name = "yarn",
                    Description = "deneme1",
                    Weight = 100,
                    UnitCost = 120
                });
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.GetProduct(productId);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var productDto = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(productId, productDto.Id);
            Assert.Equal("yarn", productDto.Name);
        }
        [Fact]
        public void GetProduct_NonExistentProductIdReturnsNotFound()
        {
            // Arrange
            A.CallTo(() => _productSut.ProductExist(A<int>.Ignored))
                .Returns(false);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.GetProduct(1);
            // Assert
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        //Create
        [Fact]
        public void CreateProduct_NonExistentProductReturnsOK()
        {
            // Arrange
            var packageId = 5;
            var unitCount = 10;
            var productCreate = A.Fake<ProductDto>();
            var newProduct = GetFakeProduct();
            A.CallTo(() => _productSut.GetProductTrimToUpper(productCreate))
                .Returns(null);
            A.CallTo(() => _mapperSut.Map<Product>(productCreate))
                .Returns(newProduct);
            A.CallTo(() => _productSut.CreateProduct(packageId, newProduct, unitCount))
                .Returns(true);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.CreateProduct(packageId, productCreate, unitCount);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
        [Fact]
        public void CreateProduct_ExistentProductReturns422()
        {
            // Arrange
            var packageId = 5;
            var unitCount = 10;
            var product = A.Fake<Product>();
            var productCreate = A.Fake<ProductDto>();
            A.CallTo(() => _productSut.GetProductTrimToUpper(productCreate))
                .Returns(product);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.CreateProduct(packageId, productCreate, unitCount);
            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(422);
            //result.Should().BeOfType<ObjectResult>().Which.Value.Should()
            //  .Be("Product already exists");
        }
        [Fact]
        public void CreateProduct_RepositoryFail_ProductReturns500()
        {
            // Arrange
            var packageId = 5;
            var unitCount = 10;
            var productCreate = A.Fake<ProductDto>();
            var newProduct = GetFakeProduct();
            A.CallTo(() => _productSut.GetProductTrimToUpper(productCreate))
                .Returns(null);
            A.CallTo(() => _mapperSut.Map<Product>(productCreate))
                .Returns(newProduct);
            A.CallTo(() => _productSut.CreateProduct(packageId, newProduct, unitCount))
                .Returns(false);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.CreateProduct(packageId, productCreate, unitCount);
            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            //result.Should().BeOfType<ObjectResult>().Which.Value.Should()
            //    .Be("Something went wrong while saving");
        }

        //Update
        [Fact]
        public void UpdateProduct_ExistentProductIdReturnsNoContent()
        {
            var productId = 17;
            var product = GetFakeProduct();
            var updatedProduct = A.Fake<ProductDto>();
            updatedProduct.Id = 17;
            A.CallTo(() => _productSut.ProductExist(productId))
                .Returns(true);
            A.CallTo(() => _mapperSut.Map<Product>(updatedProduct))
                .Returns(product);
            A.CallTo(() => _productSut.UpdateProduct(product))
                .Returns(true);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.UpdateProduct(productId, updatedProduct);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }
        [Fact]
        public void UpdateProduct_NonExistentProductIdReturnsNotFound()
        {
            var productId = 17;
            var updatedProduct = A.Fake<ProductDto>();
            updatedProduct.Id = 17;
            A.CallTo(() => _productSut.ProductExist(productId))
                .Returns(false);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.UpdateProduct(productId, updatedProduct);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NotFoundResult));

        }
        [Fact]
        public void UpdateProduct_RepositoryFailProductIdReturns500()
        {
            var productId = 17;
            var product = GetFakeProduct();
            var updatedProduct = A.Fake<ProductDto>();
            updatedProduct.Id = 17;
            A.CallTo(() => _productSut.ProductExist(productId))
                .Returns(true);
            A.CallTo(() => _mapperSut.Map<Product>(updatedProduct))
                .Returns(product);
            A.CallTo(() => _productSut.UpdateProduct(product))
                .Returns(false);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.UpdateProduct(productId, updatedProduct);
            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        //Delete
        [Fact]
        public void DeleteProduct_ExistentProductIdReturnsNoContent()
        {
            var productId = 17;
            var product = GetFakeProduct();

            A.CallTo(() => _productSut.ProductExist(productId))
                .Returns(true);
            A.CallTo(() => _productSut.GetProduct(productId))
                .Returns(product);
            A.CallTo(() => _productSut.DeleteProduct(product))
                .Returns(true);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.DeleteProduct(productId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }
        [Fact]
        public void DeleteProduct_NonExistentProductIdReturnsNotFound()
        {
            var productId = 17;
            var product = GetFakeProduct();
            A.CallTo(() => _productSut.ProductExist(productId))
                .Returns(false);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.DeleteProduct(productId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NotFoundResult));
        }
        [Fact]
        public void DeleteProduct_RepositoryFailProductIdReturnsNoContent()
        {
            var productId = 17;
            var product = GetFakeProduct();

            A.CallTo(() => _productSut.ProductExist(productId))
                .Returns(true);
            A.CallTo(() => _productSut.GetProduct(productId))
                .Returns(product);
            A.CallTo(() => _productSut.DeleteProduct(product))
                .Returns(false);
            var controller = new ProductController(_productSut, _mapperSut);
            // Act
            var result = controller.DeleteProduct(productId);
            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }


        //FakeDatas
        private List<ProductDto> GetProductsData()
        {
            List<ProductDto> customerList = new List<ProductDto>
            {
                new ProductDto { Id=1, Name = "yarn", Description = "deneme1", Weight=100, UnitCost=120},
                new ProductDto { Id=2, Name = "paper", Description = "deneme1", Weight=100, UnitCost=120},
                new ProductDto { Id=3, Name = "cement", Description = "deneme1", Weight=100, UnitCost=120},
                new ProductDto { Id=4, Name = "deneme", Description = "deneme1", Weight=100, UnitCost=120},
            };
            return customerList;
        }
        private Product GetFakeProduct()
        {
            Product fakeProduct = new Product()
            {
                Id = 17,
                Name = "FakePaper",
                Description = "papers",
                Weight = 100,
                UnitCost = 150,
            };
            return fakeProduct;
        }
    }
}
