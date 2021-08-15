using Microsoft.VisualStudio.TestTools.UnitTesting;
using InMemoryWebAPI.Controllers;
using InMemoryWebAPI.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using System;

using Microsoft.AspNetCore.Mvc;

namespace App.Tests
{
    using Xunit;
    [TestClass]
    public class UnitTest1
    {

        UserController controller;
        private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
        public UnitTest1(){
           controller = new UserController(_memoryCache);
        }
        
        [Fact]
        public void TestAddUser()
        {
            var  result= controller.Create(new User{
                Fullname="Angel Gomez", Username= "gomezz",Email="gomeza@hotmail.com",Password="craft"
            });
             Assert.NotNull(result);
            //  Assert.Equal(200, result.StatusCode);
             Assert.IsType<User>(result.Value);     
        }
        [TestMethod]
        public void TestGet()
        {
          var  result= controller.GetUser("gomeza@hotmail.com","craft");
          Assert.NotNull(result);

        }
         [Fact]
        public void TestFundMyAccount()
        {
          var  result= controller.FundAccount(new UserUpdate() {
          Email="gomeza@hotmail.com",
          Amount=800.0
          });
         Xunit.Assert.Equal("Account successfully funded, your balance is : $800",result.Value);
        }
        [Fact]
        public void TestGetMyBalance()
        {
             var  result= controller.GetMyBalance("gomeza@hotmail.com");
            //  Xunit.Assert.Equal("$500",result.Value.balance);
        }
       
        [Fact]
        public void TestSendMoney()
        {
           var  result= controller.Create(new User{
                Fullname="Dancer Whales", Username= "dancee",Email="dancee@hotmail.com",Password="craft"
          });
        //   var  result= controller.SendMoney("gomeza@hotmail.com","dancee@hotmail.com",500.0);
        //   Xunit.Assert.Equal("Money successfully sent, your balance is : $300",result.Value.message);
        }
    }
}
