using Users.Domain.ValueObject;

namespace Users.WebApi.HttpContex
{
    public record ChangePsswordRes(Guid UserId,string NewPassword);
}