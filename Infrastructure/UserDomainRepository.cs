using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain;
using Users.Domain.Entities;
using Users.Domain.ValueObject;
using Users.Interface;

namespace Users.Infrastructure
{
    public class UserDomainRepository : IUserDomainRepository
    {
        private readonly UserDbContext _context;
        private readonly IDistributedCache _distCache;
        private readonly IMediator _mediator;

        public UserDomainRepository(UserDbContext userDbContext, IMediator mediator, IDistributedCache distCache)
        {
            _context = userDbContext;
            _mediator = mediator;
            _distCache = distCache;
        }



        public async Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string message)
        {
            var user = await FindOneAsync(phoneNumber);
           
                var userLoginHistory = new UserLoginHistory(user?.Id, phoneNumber, message);
                _context.UserLoginHistorys.Add(userLoginHistory);
            
        }

        public Task<User?> FindOneAsync(PhoneNumber phoneNumber)
        {
            return _context.Users.Include(u => u.UserAcessFail).SingleOrDefaultAsync(x => x.PhoneNumber.RegionCode == phoneNumber.RegionCode && x.PhoneNumber.Number==phoneNumber.Number);
        }

        public Task<User?> FindOneAsync(Guid UserId)
        {
            return _context.Users.Include(u=>u.UserAcessFail).SingleOrDefaultAsync(u=>u.Id==UserId);
        }

        public Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber)
        {
            string fullNumber = phoneNumber.RegionCode + phoneNumber.Number;
            string cacheKey = $"LoginByPhoneAndCode_Code_{fullNumber}";
            string? code = _distCache.GetString(cacheKey);
            _distCache.Remove(cacheKey);
            return Task.FromResult(code);
        }

        public Task PublishEventAsync(UserAcessReultEvent reultEvent)
        {
          return  _mediator.Publish(reultEvent);
        }

        public Task SavePhoneNnumberCodeAsync(PhoneNumber phoneNumber, string code)
        {
            string fullNumber = phoneNumber.RegionCode + phoneNumber.Number;
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
            _distCache.SetString($"LoginByPhoneAndCode_Code_{fullNumber}", code, options);
            return Task.CompletedTask;
        }

        
    }
}
