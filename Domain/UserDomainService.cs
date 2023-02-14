using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;
using Users.Domain.ValueObject;

namespace Users.Domain
{
    public class UserDomainService
    {
        public readonly IUserDomainRepository userDomainRespitory;
        public readonly ISmsSendCodeService smsSendCodeService;

        public UserDomainService(IUserDomainRepository userDomainRespitory, ISmsSendCodeService smsSendCodeService)
        {
            this.userDomainRespitory = userDomainRespitory;
            this.smsSendCodeService = smsSendCodeService;
        }

        public async Task<CheckCodeResult> CheckCodeAsync(PhoneNumber phoneNumber, string code)
        {
            CheckCodeResult result;
            var user = await userDomainRespitory.FindOneAsync(phoneNumber);

            if (user == null)
            {
                return result = CheckCodeResult.PhoneNumberNotFound;
            }
             if(IsLockOut(user))
                result = CheckCodeResult.LockOut;
            string? codeInServer = await userDomainRespitory.FindPhoneNumberCodeAsync(phoneNumber);

            if (codeInServer == null)
                result = CheckCodeResult.CodeError;

            if (code == codeInServer)
                result = CheckCodeResult.OK;
            else
                result = CheckCodeResult.CodeError;

            if(result != CheckCodeResult.OK)
            {
                AccessFail(user);
            }

            return result;
        }

        public async Task<UserAcessResult> CheckLoginAsync(PhoneNumber phoneNumber, string password)
        {
            UserAcessResult result;
            var user = await userDomainRespitory.FindOneAsync(phoneNumber);
            if (user == null)
            {
                result = UserAcessResult.PhoneNumberNotFound;
                return result;
            }
            else if (IsLockOut(user))
            {
                result = UserAcessResult.LockOut;
            }
            else if (!user.HasPassword())
            {
                result = UserAcessResult.NoPassword;
            }
            else if (user.CheckPassword(password))
            {
                result = UserAcessResult.OK;
            }
            else
            {
                result = UserAcessResult.PasswordError;
            }

            if (result == UserAcessResult.OK)
                ResetAccessFail(user);
            else
                AccessFail(user);
           
            await userDomainRespitory.PublishEventAsync(new UserAcessReultEvent(phoneNumber, result));
            return result;
        }

        public async Task<UserAcessResult> SendCodeAsync(PhoneNumber phoneNum)
        {
            var user = await userDomainRespitory.FindOneAsync(phoneNum);
            if (user == null)
            {
                return UserAcessResult.PhoneNumberNotFound;
            }
            if (IsLockOut(user))
            {
                return UserAcessResult.LockOut;
            }
            string code = Random.Shared.Next(1000, 9999).ToString();
            await userDomainRespitory.SavePhoneNnumberCodeAsync(phoneNum, code);
            await smsSendCodeService.SendCodeAsync(phoneNum, code);
            return UserAcessResult.OK;
        }



        private void AccessFail(User user)
        {
            user.UserAcessFail.Fail();
        }

        private void ResetAccessFail(User user)
        {
            user.UserAcessFail.Reset();
        }

        public bool IsLockOut(User user)
        {
            return user.UserAcessFail.IsLookOut();
        }


    }
}
