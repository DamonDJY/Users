using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.Entities
{
    public record UserAcessFail
    {
        public Guid Id { get; init; }

        public User User { get; init; }

        public Guid UserId { get; init; }

        private bool isLookOut;

        public DateTime? LookEnd { get; private set; }

        public int AccessFailCount { get; private set; }

        public UserAcessFail() { }

        public UserAcessFail(User user)
        {
            Id = Guid.NewGuid();
            UserId = user.Id;
            User = user;
        }

        public void Reset()
        {
            AccessFailCount = 0;
            LookEnd = null;
            isLookOut = false;
        }

        public void Fail()
        {
            AccessFailCount++;

            if(AccessFailCount > 5)
            {
                LookEnd = DateTime.Now.AddMinutes(5);
                isLookOut = true;
            }
           

        }

        public bool IsLookOut()
        {
            if (isLookOut)
            {
                if (LookEnd >= DateTime.Now)
                {
                    return true;
                }
                else
                {
                    Reset();
                    return false;
                }
            }
            else return false;
        }
    }
}
