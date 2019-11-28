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
            if (partner.Id == "")
            {
                context.Partners.Add(partner);
            }
            else
            {
                Partner dbPartner = context.Partners.FirstOrDefault(r => r.Id == partner.Id);
                if (dbPartner != null)
                {
                    dbPartner.Rate = partner.Rate;
                    dbPartner.Address = partner.Address;
                    dbPartner.City = partner.City;
                    dbPartner.PostalCode = partner.PostalCode;
                    dbPartner.Latitude = partner.Latitude;
                    dbPartner.Longitude = partner.Longitude;
                }
            }
            context.SaveChanges();
            return partner;
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
    }
}
