using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.ValueObject;

namespace Users.Domain.Entities
{
    public class UserLoginHistory:IAggregateRoot
    {
         private UserLoginHistory() { }

        public long Id { get; init; }

        public Guid? UserId { get; init; }

        public PhoneNumber PhoneNumber { get; init; }

        public DateTime CreatedTime { get; init; }

        public string Message { get; init; }

        public UserLoginHistory( Guid? userId, PhoneNumber phoneNumber, string message)
        {
            
            UserId = userId;
            PhoneNumber = phoneNumber;
            CreatedTime = DateTime.Now;
            Message = message;
        }
    }
}
