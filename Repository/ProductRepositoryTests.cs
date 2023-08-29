using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using ProductionManagement.Controllers;
using ProductionManagement.Data;
using ProductionManagement.Dto;
using ProductionManagement.Interfaces;
using ProductionManagement.Models;
using ProductionManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionManagement.Tests.Repository
{
    public class ProductRepositoryTests
    {
        //public ProductRepositoryTests()
        //{
        //    var options = new DbContextOptionsBuilder<DataContext>()
        //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        //        .Options;
        //    _contextSut = new DataContext(options);

        //}

        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .EnableSensitiveDataLogging()
              .Options;
            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();

            if (!await databaseContext.Products.AnyAsync())
            {
                for (int i = 0; i < 2; i++)
                {
                    databaseContext.ProductPackages.Add(
                        new ProductPackage()
                        {
                            Product = new Product()
                            {
                                Name = "Paper",
                                Description = "paper",
                                Weight = 100,
                                UnitCost = 100,
                            },
                            Package = new Package()
                            {
                                CreateDate = new DateTime(2021, 10, 15),
                                Cost = 1000,
                                Customer = new Customer() { Name = "Burak Özcan" }
                            },
                            Units = 10,
                        });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        

        [Fact]
        public async void ProductRepository_GetProducts_ReturnAllProducts()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var repository = new ProductRepository(dbContext);

            //Act
            var result = repository.GetProducts();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<Product>));
            result.Should().HaveCount(10);

        }

        [Fact]
        public async void ProductRepository_GetProductbyId_ReturnProduct()
        {
            //Arrange
            var productId = 4;
            var dbContext = await GetDatabaseContext();
            var repository = new ProductRepository(dbContext);

            //Act
            var result = repository.GetProduct(4);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Product>();
            result.Id.Should().Be(productId);

        }

        [Fact]
        public async void ProductRepository_CreateProduct_ReturnOK()
        {
            //Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Weight = 110,
                UnitCost = 130,
            };
            var units = 17;
            var packageId = 4;

            var dbContext = await GetDatabaseContext();
            var repository = new ProductRepository(dbContext);

            //Act
            var result = repository.CreateProduct(packageId, product, units);

            //Assert
            result.Should().BeTrue();

        }

        [Fact]
        public async void ProductRepository_UpdateProduct_ReturnOK()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var repository = new ProductRepository(dbContext);

            var productToUpdate = await dbContext.Products.FirstOrDefaultAsync();
            
            if (productToUpdate != null)
            {
                productToUpdate.Name = "Updated Product";
                productToUpdate.Description = "Updated Description";
                productToUpdate.Weight = 110;
                productToUpdate.UnitCost = 130;

                // Act
                var result = repository.UpdateProduct(productToUpdate);

                // Assert
                result.Should().BeTrue();
            }

        }

        [Fact]
        public async void ProductRepository_DeleteProduct_ReturnOK()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ProductRepository(dbContext);

            var productToUpdate = await dbContext.Products.FirstOrDefaultAsync();

            if (productToUpdate != null)
            {

                // Act
                var result = repository.DeleteProduct(productToUpdate);

                // Assert
                result.Should().BeTrue();
            }

        }
    }

    //[Fact]
    //public void ProductRepository_GetProducts_ReturnAllProducts()
    //{
    //    //
    //    var products = GetProductsData();
    //    _contextSut.Products.AddRange(products);
    //    _contextSut.SaveChanges();
    //    var repository = new ProductRepository(_contextSut);
    //    ///
    //    var result = repository.GetProducts();
    //    //
    //    result.Should().NotBeNull();
    //    result.Should().BeOfType(typeof(List<Product>));
    //    result.Should().BeEquivalentTo(products);
    //}

    //[Fact]
    //public void ProductRepository_GetProductbyId_ReturnProduct()
    //{
    //    //
    //    var productId = 1;
    //    var products = GetProductsData();
    //    var product = products.Where(p => p.Id == productId).FirstOrDefault();
    //    _contextSut.Products.AddRange(products);
    //    _contextSut.SaveChanges();
    //    var repository = new ProductRepository(_contextSut);
    //    ///
    //    var result = repository.GetProduct(productId);
    //    //
    //    result.Should().NotBeNull();
    //    result.Should().BeOfType(typeof(Product));
    //    result.Should().BeEquivalentTo(product);
    //}

    //[Fact]
    //public void ProductRepository_CreateProduct_ReturnOK()
    //{
    //    //
    //    var fakePackage = A.Fake<Package>();
    //    var product = new Product { Id = 1, Name = "Test Product" }; ;
    //    var units = 17;
    //    var packageId = 1;

    //    //var repository = new ProductRepository(_contextSut);
    //    var package = repository.GetPackage(packageId);


    //    //
    //    var result = repository.CreateProduct(packageId, product, units);
    //    //
    //    result.Should().BeTrue();
    //}

    //private List<Product> GetProductsData()
    //{
    //    List<Product> customerList = new List<Product>
    //        {
    //            new Product { Id=1, Name = "yarn", Description = "deneme1", Weight=100, UnitCost=120},
    //            new Product { Id=2, Name = "paper", Description = "deneme1", Weight=100, UnitCost=120},
    //            new Product { Id=3, Name = "cement", Description = "deneme1", Weight=100, UnitCost=120},
    //            new Product { Id=4, Name = "deneme", Description = "deneme1", Weight=100, UnitCost=120},
    //        };
    //    return customerList;
    //}
}

