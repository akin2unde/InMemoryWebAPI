using Microsoft.VisualStudio.TestTools.UnitTesting;
using InMemoryWebAPI.Controllers;
using InMemoryWebAPI.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace App.Tests
{
    [TestClass]
    public class UnitTest1
    {

        UserController controller;
        private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
        List<User> users = new List<User>(){
            new User(){ Fullname="Angel Gomez", Username= "gomezz",Email="gomeza@hotmail.com",Password="craft"},
            new User(){ Fullname="Makinde Joy", Username= "joyee",Email="joyee@hotmail.com",Password="craft"},
            new User(){ Fullname="Dancer Maker", Username= "makerD",Email="makerd@hotmail.com",Password="craft"}
        };

        public UnitTest1(){
           controller = new UserController(_memoryCache);
        }
        
        [TestMethod]
        public void TestAddUser()
        {
            // ActionResult<Post> result= controller.Create(new User{
            //     Fullname="Angel Gomez", Username= "gomezz",Email="gomeza@hotmail.com",Password="craft"
            // });
            // Post createdPost = result.Value;

        }
        [TestMethod]
        public void TestGet()
        {
            

        }
        [TestMethod]
        public void TestGetMyBalance()
        {
            

        }
        [TestMethod]
        public void TestFundMyAccount()
        {
            

        }
        [TestMethod]
        public void TestSendMoney()
        {
            

        }
    }
}
