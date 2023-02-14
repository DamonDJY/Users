using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;
using Users.Domain.ValueObject;

namespace Users.Domain
{
    public interface IUserDomainRepository
    {
        public Task<User?> FindOneAsync(PhoneNumber phoneNumber);
        public Task<User?> FindOneAsync(Guid UserId);

        public Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string message);

        public Task SavePhoneNnumberCodeAsync(PhoneNumber phoneNumber, string code);

        public Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber);

        public Task PublishEventAsync(UserAcessReultEvent reultEvent);

       
    }
}
