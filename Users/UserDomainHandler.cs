using MediatR;
using Users.Domain;
using Users.Infrastructure;

namespace Users.WebApi
{
    public class UserDomainHandler : INotificationHandler<UserAcessReultEvent>
    {
        private readonly IUserDomainRepository _repository;

        public UserDomainHandler(IUserDomainRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UserAcessReultEvent notification, CancellationToken cancellationToken)
        {

            var result = notification.UserAcessResult;
            var phoneNumber= notification.PhoneNumber;
            string msg;

            switch (result)
            {
                case UserAcessResult.OK:
                    msg = $"{phoneNumber}登陆成功";
                    break;
                case UserAcessResult.PhoneNumberNotFound:
                    msg = $"{phoneNumber}登陆失败，因为用户不存在";
                    break;
                case UserAcessResult.PasswordError:
                    msg = $"{phoneNumber}登陆失败，密码错误";
                    break;
                case UserAcessResult.NoPassword:
                    msg = $"{phoneNumber}登陆失败，没有设置密码";
                    break;
                case UserAcessResult.LockOut:
                    msg = $"{phoneNumber}登陆失败，被锁定";
                    break;
                default:
                    throw new NotImplementedException();

            }



            await _repository.AddNewLoginHistoryAsync(notification.PhoneNumber,msg);
        }
    }
}
