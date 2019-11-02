using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models
{
    public interface IPartnerRepository
    {
        IQueryable<Partner> GetAll { get; }
        Partner Save(Partner partner);
        Partner Delete(string partnerId);
    }
}
