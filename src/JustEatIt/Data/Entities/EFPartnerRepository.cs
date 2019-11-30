using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data.Entities
{
    public class EFPartnerRepository : IPartnerRepository
    {
        private AppDataDbContext context;

        public EFPartnerRepository(AppDataDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Partner> GetAll => context.Partners;

        public Partner Save(Partner partner)
        {
            Partner dbPartner = context.Partners.FirstOrDefault(r => r.Id == partner.Id);
            if (dbPartner == null)
            {
                var newPartner = context.Partners.Add(partner);
                context.SaveChanges();
                dbPartner = newPartner.Entity;
            }
            else
            {
                dbPartner.Rate = partner.Rate;
                dbPartner.Address = partner.Address;
                dbPartner.City = partner.City;
                dbPartner.PostalCode = partner.PostalCode;
                dbPartner.Latitude = partner.Latitude;
                dbPartner.Longitude = partner.Longitude;
                context.SaveChanges();
            }

            return dbPartner;
        }

        public Partner Delete(string id)
        {
            Partner dbPartner = context.Partners.FirstOrDefault(r => r.Id == id);
            if (dbPartner != null)
            {
                context.Partners.Remove(dbPartner);
                context.SaveChanges();
            }
            return dbPartner;
        }

        public void DeleteAll(string id)
        {
            // Remove the orders
            var orders = context.Orders.Where(o => o.PartnerId == id).Include(o => o.Items);
            if (orders.Count() > 0)
            {
                context.Orders.RemoveRange(orders);
            }

            // Remove the dishes
            var dishes = context.Dishes.Where(d => d.PartnerId == id).Include(d => d.DishAvailabilities);
            if (dishes.Count() > 0)
            {
                context.Dishes.RemoveRange(dishes);
            }

            // Remove the partner
            var partner = context.Partners.FirstOrDefault(p => p.Id == id);
            if (partner != null)
            {
                context.Partners.Remove(partner);
            }

            context.SaveChanges();
            return;
        }
    }
}
