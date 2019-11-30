using JustEatIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data.Entities
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAll { get; }

        string Save(Customer customer);

        Customer Delete(string customerId);

        void DeleteAll(string customerId);
    }
}
