using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain
{
    public enum UserAcessResult
    {
        OK,PhoneNumberNotFound,LockOut,NoPassword,PasswordError
    }
}
