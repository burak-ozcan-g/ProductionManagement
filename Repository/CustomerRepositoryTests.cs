using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
using Xunit.Abstractions;

namespace ProductionManagement.Tests.Repository
{
    public class CustomerRepositoryTests
    {

        


        //[Fact]
        //public void CustomerController_GetCustomers_ReturnOK()
        //{
        //    var customers = GetCustomersData();
        //    _customerSut.Setup(x => x.GetCustomers()).Returns(customers);

        //    result.Should().NotBeNull();
        //    result.Should().BeOfType(typeof(OkObjectResult));
        //}

        //[Fact]
        //public void CustomerController_GetCustomer_ReturnOK()
        //{
        //    //arrange
        //    var customer = new CustomerDto { Id = 2, Name = "Burak Özcan" };
        //    var customers = GetCustomersData();
        //    _customerSut.Setup(x => x.GetCustomer(2)).Returns(customers[1]);

        //    //act

        //    //assert
        //    result.Should().NotBeNull();
        //    result.Should().BeOfType(typeof(OkObjectResult));

        //}

        private List<Customer> GetCustomersData()
        {
            List<Customer> customerList = new List<Customer>
            {
                new Customer { Id = 1, Name = "Burak Özcan"},
                new Customer { Id = 2, Name = "Burak Özcan"},
                new Customer { Id = 3, Name = "Burak Özcan"},
                new Customer { Id = 4, Name = "Burak Özcan"},
            };
            return customerList;
        }



        //[Fact]
        //public async void CustomerRepository_GetCustomers_ReturnsOK()
        //{
        //    var dbContext = await GetDatabaseContext();

        //    var customerRepository = new CustomerRepository(dbContext);
        //    var result = customerRepository.GetCustomers();

        //    result.Should().NotBeNull();
        //    result.Should().BeOfType<List<Customer>>();
        //    result.Should().NotBeEmpty().And.HaveCount(10);
        //}

        //[Fact]
        //public async void CustomerRepository_GetCustomer_ReturnsOK()
        //{
        //    int Id = 5;
        //    var dbContext = await GetDatabaseContext();

        //    var customerRepository = new CustomerRepository(dbContext);
        //    var result = customerRepository.GetCustomer(Id);

        //    result.Should().NotBeNull();
        //    result.Should().BeOfType<Customer>();
        //    result.Id.Should().Be(Id);
        //}

        //[Fact]
        //public async void CustomerRepository_CreateCustomer_ReturnsOK()
        //{
        //    var dbContext = await GetDatabaseContext();
        //    var demoCustomer = GetDemoCustomer();

        //    var customerRepository = new CustomerRepository(dbContext);
        //    var result = customerRepository.CreateCustomer(demoCustomer);
        //    var control = customerRepository.CustomerExist(demoCustomer.Id);

        //    result.Should().BeTrue();
        //    control.Should().BeTrue();

        //}

        //[Fact]
        //public async void CustomerRepository_UpdateCustomer_ReturnsOK()
        //{
        //    var dbContext = await GetDatabaseContext();
        //    var demoCustomer = GetDemoCustomer();
        //    var customerRepository = new CustomerRepository(dbContext);
        //    customerRepository.CreateCustomer(demoCustomer);

        //    var result = customerRepository.UpdateCustomer(demoCustomer);

        //    result.Should().BeTrue();
        //}

        //[Fact]
        //public async void CustomerRepository_DeleteCustomer_ReturnsOK()
        //{
        //    var dbContext = await GetDatabaseContext();
        //    var demoCustomer = GetDemoCustomer();
        //    var customerRepository = new CustomerRepository(dbContext);
        //    customerRepository.CreateCustomer(demoCustomer);

        //    var result = customerRepository.DeleteCustomer(demoCustomer);
        //    var control = customerRepository.CustomerExist(demoCustomer.Id);

        //    result.Should().BeTrue();
        //    control.Should().BeFalse();
        //}

        //Customer GetDemoCustomer()
        //{
        //    return new Customer() { Id = 99, Name = "Hüdayi Yıldırım" };
        //}
    }
}
