using System.Collections.Generic;

namespace InMemoryWebAPI.Models
{
public class User{
    public string Fullname { get; set; }
    public string Username { get; set; }
    //Key
    public string Email { get; set; }

    public string Password { get; set; }

    public double AccountBalance { get; set; }

    public List<AccountHistory> Histories { get; set; } = new List<AccountHistory>();
 }

 public class UserUpdate{
    public double Amount { get; set; }
    public string Email { get; set; }
 }

}