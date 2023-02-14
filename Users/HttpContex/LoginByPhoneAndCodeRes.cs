using Users.Domain.ValueObject;

namespace Users.WebApi.HttpContex
{
    public record LoginByPhoneAndCodeRes(PhoneNumber Phone, string Code);
}