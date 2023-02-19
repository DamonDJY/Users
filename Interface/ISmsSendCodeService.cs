using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.ValueObject;

namespace Users.Interface
{
    public interface ISmsSendCodeService
    {
        Task SendCodeAsync(PhoneNumber phoneNumber, string code);
    }
}
