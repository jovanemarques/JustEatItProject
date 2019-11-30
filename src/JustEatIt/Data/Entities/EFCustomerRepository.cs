using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data.Entities
{
    public class EFCustomerRepository : ICustomerRepository
    {
        private AppDataDbContext context;

        public EFCustomerRepository(AppDataDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Customer> GetAll => context.Customers;

        public string Save(Customer customer)
        {
            Customer dbCustomer;

            dbCustomer = context.Customers.FirstOrDefault(c => c.Id == customer.Id);
            if (dbCustomer == null)
            {
                var newCustomer = context.Customers.Add(customer);
                context.SaveChanges();
                dbCustomer = newCustomer.Entity;
            }
            else
            {
                dbCustomer.FirstName = customer.FirstName;
                dbCustomer.LastName = customer.LastName;
                dbCustomer.Address = customer.Address;
                dbCustomer.City = customer.City;
                dbCustomer.PostalCode = customer.PostalCode;
                context.SaveChanges();
            }

            return dbCustomer.Id;
        }

        public Customer Delete(string id)
        {
            Customer dbCustomer = context.Customers.FirstOrDefault(c => c.Id == id);
            if (dbCustomer != null)
            {
                context.Customers.Remove(dbCustomer);
                context.SaveChanges();
            }
            return dbCustomer;
        }

        public void DeleteAll(string id)
        {
            // Remove the orders
            var orders = context.Orders.Where(o => o.CustomerId == id).Include(o => o.Items);
            if (orders.Count() > 0)
            {
                context.Orders.RemoveRange(orders);
            }

            // Remove the customer
            var customer = context.Customers.FirstOrDefault(p => p.Id == id);
            if (customer != null)
            {
                context.Customers.Remove(customer);
            }

            context.SaveChanges();
            return;
        }

    }
}
