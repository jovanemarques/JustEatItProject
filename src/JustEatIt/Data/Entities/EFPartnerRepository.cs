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

        public IQueryable<Partner> GetAll => context.Partner;

        public Partner Save(Partner partner)
        {
            if (partner.Id == "")
            {
                context.Partner.Add(partner);
            }
            else
            {
                Partner dbPartner = context.Partner.FirstOrDefault(r => r.Id == partner.Id);
                if (dbPartner != null)
                {
                    dbPartner.Rate = partner.Rate;
                }
            }
            context.SaveChanges();
            return partner;
        }

        public Partner Delete(string id)
        {
            Partner dbPartner = context.Partner.FirstOrDefault(r => r.Id == id);
            if (dbPartner != null)
            {
                context.Partner.Remove(dbPartner);
                context.SaveChanges();
            }
            return dbPartner;
        }
    }
}
