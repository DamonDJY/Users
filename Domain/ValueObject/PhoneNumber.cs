using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.ValueObject
{
    public record PhoneNumber(int RegionCode,string Number);
  
}
