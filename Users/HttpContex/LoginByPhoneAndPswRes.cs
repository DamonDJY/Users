using Users.Domain.ValueObject;

namespace Users.WebApi.HttpContex
{
    public record LoginByPhoneAndPswRes(PhoneNumber Phone,string password);
}