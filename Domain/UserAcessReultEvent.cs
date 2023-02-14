using MediatR;
using Users.Domain.ValueObject;

namespace Users.Domain
{
    public record UserAcessReultEvent(PhoneNumber PhoneNumber , UserAcessResult UserAcessResult):INotification;
     
}