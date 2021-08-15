using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace InMemoryWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<UserController> _logger;
        private readonly AppJSON _appJSON = new AppJSON();        
        public UserController( IMemoryCache memoryCache)
        {           
            //  _appJSON= appJSON;
            _memoryCache = memoryCache;
        }
        [HttpGet ("GetAll")]
        public ActionResult<List<User>> GetAll()
        {
            var userList  = new List<User>();
            _memoryCache.TryGetValue(_appJSON.MemoryName, out userList);
            return Ok(userList);
        }
        [HttpGet ("GetUser/{email}/{password}")]
        public ActionResult<User> GetUser(string email, string password)
        {
            var userList  = new List<User>();
            _memoryCache.TryGetValue(_appJSON.MemoryName, out userList);
            if(userList==null)
            {
                return Problem(detail:"User not found");
            }
            var result =userList.Find(u=>u.Email==email && u.Password==password);
            if(result==null){
                 return Problem(detail:"User not found");
            }
            return Ok(result);
        }
        [HttpGet ("GetMyBalance/{email}")]
        public ActionResult<Object> GetMyBalance(string email)
        {
            var userList  = new List<User>();
            _memoryCache.TryGetValue(_appJSON.MemoryName, out userList);
             if(userList==null)
            {
                return Problem(detail:"User not found");
            }
            var result = userList.FirstOrDefault(u=>u.Email==email);
            if(result==null){
                return Problem(detail:"User not found");
            }
            return Ok(new  { Balance = "$"+result.AccountBalance} );
        }

        [HttpGet ("SendMoney/{fromEmail}/{toEmail}/{amount}")]
        public ActionResult<Object> SendMoney(string fromEmail, string toEmail,double amount)
        {
             var userList  = new List<User>();
            _memoryCache.TryGetValue(_appJSON.MemoryName, out userList);
             if(userList==null)
            {
                return Problem(detail:"User not found");
            }
            if(userList.Count(u=>u.Email==fromEmail)==0 || 
                    userList.Count(u=>u.Email==toEmail)==0){
                return userList.Count(u=>u.Email==fromEmail)==0? Problem(detail:"From user does not exist"):
                Problem(detail:"To user does not exist");        
            }
            //locate fromuser and touser
            var fromUser= userList.Find(u=>u.Email==fromEmail);
            var toUser= userList.Find(u=>u.Email==toEmail);
            //check if from user has enough to send
            if(fromUser.AccountBalance<amount){
               return Problem(detail:"Balance not sufficient to carry out the transfer");  
            }
            //debit from fromuser and create accounthistory
            fromUser.AccountBalance-=amount;
            var Id= Guid.NewGuid().ToString();
            fromUser.Histories.Add(new AccountHistory{UserEmail= toEmail, Amount= amount,Id=Id,IsCredit=false });
            //credit toUser and create accounthistory
            toUser.AccountBalance+=amount;
            toUser.Histories.Add(new AccountHistory{UserEmail= fromEmail, Amount= amount,Id=Id,IsCredit=true });
            _memoryCache.TryGetValue(_appJSON.MemoryName, out userList);
            //return from user balance
            return Ok(new { Message = $"Money successfully sent, your balance is : ${fromUser.AccountBalance}"});
        }

        [HttpPost ("Create")]
        public ActionResult<Object> Create([FromBody] User user)
        {
             var userList  = new List<User>();
            _memoryCache.TryGetValue(_appJSON.MemoryName, out userList);
            //cache has not been created
            if(userList==null){
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(120),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(60)
            };
            userList= new List<User>(){user};
            _memoryCache.Set(_appJSON.MemoryName, userList, cacheExpiryOptions);
            return Ok(user);
          }
          else{
              //throw error if user exist
              if(userList.Count(u=>u.Email==user.Email)>0){
                  return Problem(detail:"User with the same email already exist");
              }
              userList.Add(user);
            _memoryCache.Set(_appJSON.MemoryName, userList);
            return Ok(user);
          }
        }

        [HttpPut ("FundAccount")]
        public ActionResult<Object> FundAccount([FromBody]  UserUpdate obj)
        {    var email = obj.Email;
             var amount = obj.Amount;
             var userList  = new List<User>();
            _memoryCache.TryGetValue(_appJSON.MemoryName, out userList);
             if(userList==null)
            {
                return Problem(detail:"User not found");
            }
            if(userList.Count(u=>u.Email==email)==0){
                return Problem(detail:"User does not exist");
            }
            //locate user
            var user= userList.Find(u=>u.Email==email);
            //credit user and create accounthistory
            user.AccountBalance+=amount;
            var Id= Guid.NewGuid().ToString();
            user.Histories.Add(new AccountHistory{UserEmail= email, Amount= amount,Id=Id,IsCredit=true });
            _memoryCache.TryGetValue(_appJSON.MemoryName, out userList);
            //return from user balance
            return Ok(new { Message = $"Account successfully funded, your balance is : ${user.AccountBalance}"});
        }

    }
}
