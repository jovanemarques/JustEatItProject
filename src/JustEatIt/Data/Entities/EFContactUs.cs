using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data.Entities
{
    public class EFContactUsRepository : IContactUsRepository
    {
        private ApplicationDbContext context;

        public EFContactUsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ContactUs> News => context.ContactUs;

        public void Save(ContactUs contactUs)
        {
            if (contactUs.Id == 0)
            {
                context.ContactUs.Add(contactUs);
            }
            else
            {
                ContactUs regDb = context.ContactUs.FirstOrDefault(r => r.Id == contactUs.Id);
                if (regDb != null)
                {
                    regDb.Name = contactUs.Name;
                    regDb.Email = contactUs.Email;
                    regDb.Message = contactUs.Message;
                }
            }
            context.SaveChanges();
        }

        public ContactUs Delete(int id)
        {
            ContactUs regDb = context.ContactUs.FirstOrDefault(r => r.Id == id);
            if (regDb != null)
            {
                context.ContactUs.Remove(regDb);
                context.SaveChanges();
            }
            return regDb;
        }

    }
}
