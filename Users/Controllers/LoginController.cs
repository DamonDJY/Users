using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Other;
using Users.Domain;
using Users.Domain.ValueObject;
using Users.Infrastructure;
using Users.WebApi.HttpContex;

namespace Users.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [UnitOfWork(typeof(UserDbContext))]
    public class LoginController : ControllerBase
    {
        private readonly UserDomainService _service;

        public LoginController(UserDomainService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ApiResult> GetVaildCode(PhoneNumber phoneNumber)
        {
          var result =  await _service.SendCodeAsync(phoneNumber);

            switch (result)
            {
                case UserAcessResult.PhoneNumberNotFound:
                   return ResultHelper.Error("手机未注册");
                case UserAcessResult.LockOut:
                   return ResultHelper.Error($"用户已锁定");
                case UserAcessResult.OK:
                    return ResultHelper.Success("已发送到手机");
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost]
        public async Task<ApiResult> LoginByPassword(LoginByPhoneAndPswRes res)
        {
            var result = await _service.CheckLoginAsync(res.Phone, res.password);

            switch (result) {
                case UserAcessResult.PasswordError:
                case UserAcessResult.NoPassword:
                case UserAcessResult.PhoneNumberNotFound:
                    return ResultHelper.Error("手机或密码错误");
                case UserAcessResult.LockOut:
                    return ResultHelper.Error("用户已锁定");
                case UserAcessResult.OK:
                    return ResultHelper.Success("登录成功"); 
                default:
                        throw new NotImplementedException();
            }

        }
        [HttpPost]
        public async Task<ApiResult> LoginByCode(LoginByPhoneAndCodeRes res)
        {
            var result = await _service.CheckCodeAsync(res.Phone, res.Code);

            switch (result)
            {
                case CheckCodeResult.OK:
                    return ResultHelper.Success("登录成功");
                case CheckCodeResult.PhoneNumberNotFound:
                    return ResultHelper.Error("请求错误");//避免泄密
                case CheckCodeResult.LockOut:
                    return ResultHelper.Error("用户被锁定，请稍后再试");
                case CheckCodeResult.CodeError:
                    return ResultHelper.Error("验证码错误");
                default:
                    throw new NotImplementedException();
            }

        }

    }
}
