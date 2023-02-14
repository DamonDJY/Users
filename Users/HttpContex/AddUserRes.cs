using Users.Domain.ValueObject;

namespace Users.WebApi.HttpContex
{
    public record AddUserRes(PhoneNumber PhoneNumber, string Password);
}