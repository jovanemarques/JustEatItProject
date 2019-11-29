using System.Linq;
using JustEatIt.Models;

namespace JustEatIt.Data.Entities
{
    public interface IUserRepository
    {
        IQueryable<UserInfo> GetAll { get; }
    }
}