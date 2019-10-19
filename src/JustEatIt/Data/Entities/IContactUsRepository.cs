using JustEatIt.Models;
using System.Linq;

namespace JustEatIt.Data.Entities
{
    public interface IContactUsRepository
    {
        IQueryable<ContactUs> News { get; }
        void Save(ContactUs contactUs);
        ContactUs Delete(int id);
    }
}