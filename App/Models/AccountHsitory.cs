using System;

namespace InMemoryWebAPI.Models
{
public class AccountHistory{
    public string UserEmail { get; set; }

    public DateTime Date { get; set; }= DateTime.Now;

    public double Amount { get; set; }= 0.0;

    public bool IsCredit { get; set; }=true;

    public string Id { get; set; }

  }  
}