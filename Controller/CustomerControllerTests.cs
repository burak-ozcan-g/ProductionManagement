using AutoFixture;
using AutoMapper;
using Castle.Core.Resource;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Moq;
using ProductionManagement.Controllers;
using ProductionManagement.Data;
using ProductionManagement.Dto;
using ProductionManagement.Interfaces;
using ProductionManagement.Models;
using ProductionManagement.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ProductionManagement.Tests.Controller
{
    public class CustomerControllerTests
    {
        private readonly ICustomerRepository _customerSut;
        private readonly IMapper _mapperSut;

        public CustomerControllerTests()
        {
            _customerSut = A.Fake<ICustomerRepository>();
            _mapperSut = A.Fake<IMapper>();
        }
        [Fact]
        public void GetCustomersReturnsListOfCustomers()
        {
            // Arrange
            A.CallTo(() => _customerSut.GetCustomers())
                .Returns(new List<Customer>());
            A.CallTo(() => _mapperSut.Map<List<CustomerDto>>(A<List<Customer>>.Ignored))
                .Returns(GetCustomersData());
            var controller = new CustomerController(_customerSut, _mapperSut);

            // Act
            var result = controller.GetCustomers();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void GetCustomer_ExistingCustomerIdReturnsCustomer()
        {
            // Arrange
            A.CallTo(() => _customerSut.CustomerExist(A<int>.Ignored))
                .Returns(true);
            A.CallTo(() => _customerSut.GetCustomer(A<int>.Ignored))
                .Returns(new Customer { Id = 1, Name = "Burak Özcan" });
            A.CallTo(() => _mapperSut.Map<CustomerDto>(A<Customer>.Ignored))
                .Returns(new CustomerDto { Id = 1, Name = "Burak Özcan" });
            var controller = new CustomerController(_customerSut, _mapperSut);

            // Act
            var result = controller.GetCustomer(1);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var customerDto = Assert.IsType<CustomerDto>(okResult.Value);

            Assert.Equal(1, customerDto.Id);
            Assert.Equal("Burak Özcan", customerDto.Name);
        }

        [Fact]
        public void GetCustomer_NonExistentCustomerIdReturnsNotFound()
        {
            // Arrange
            A.CallTo(() => _customerSut.CustomerExist(A<int>.Ignored))
                .Returns(false);
            var controller = new CustomerController(_customerSut, _mapperSut);

            // Act
            var result = controller.GetCustomer(1);

            // Assert
            result.Should().BeOfType(typeof(NotFoundResult));

        }

        //[Fact]
        //public void CustomerController_GetCustomers_ReturnOK()
        //{

        //    var customers = A.CollectionOfDummy<ICollection<Customer>>;
        //    var customerList = A.Fake<List<CustomerDto>>();
        //    A.CallTo(() => _mapperSut.Map<List<CustomerDto>>(customers)).Returns(customerList);
        //    var controller = new CustomerController(_customerSut, _mapperSut);

        //    var result = controller.GetCustomers();

        //    result.Should().NotBeNull();
        //    result.Should().BeOfType(typeof(OkObjectResult));
        //}

        //[Fact]
        //public void CustomerController_GetCustomer_ReturnOK()
        //{
        //    var customerDto = A.Dummy<CustomerDto>();
        //    A.CallTo(() => _mapperSut.Map<CustomerDto>(customer)).Returns(customerDto);
        //    var controller = new CustomerController(_customerSut, _mapperSut);

        //    var result = controller.GetCustomer(1);

        //    result.Should().NotBeNull();
        //    result.Should().BeOfType(typeof(OkObjectResult));

        //}


        //[Fact]
        //public void CustomerController_CreateCustomer_ReturnOK()
        //{
        //    var customerMap = A.Fake<Customer>();
        //    var customer = A.Fake<Customer>();
        //    var customerCreate = A.Fake<CustomerDto>();
        //    var customers = A.Fake<ICollection<CustomerDto>>();
        //    var customerList = A.Fake<IList<CustomerDto>>();
        //    A.CallTo(() => _customerRepository.GetCustomerTrimToUpper(customerCreate)).Returns(customer);
        //    A.CallTo(() => _mapper.Map<Customer>(customerCreate)).Returns(customer);
        //    A.CallTo(() => _customerRepository.CreateCustomer(customerMap)).Returns(true);
        //    var controller = new CustomerController(_customerRepository, _packageRepository, _mapper);

        //    var result = controller.CreateCustomer(customerCreate);

        //    result.Should().NotBeNull();
        //    result.Should().BeOfType(typeof(ObjectResult));
        //}


        private List<CustomerDto> GetCustomersData()
        {
            List<CustomerDto> customerList = new List<CustomerDto>
            {
                new CustomerDto { Id=1, Name = "Burak Özcan"},
                new CustomerDto { Id=2, Name = "Burak Özcan"},
                new CustomerDto { Id=3, Name = "Burak Özcan"},
                new CustomerDto { Id=4, Name = "Burak Özcan"},
            };
            return customerList;
        }
    }
}
