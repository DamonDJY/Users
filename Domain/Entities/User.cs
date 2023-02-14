using Users.Domain.ValueObject;
using Zack.Commons;

namespace Users.Domain.Entities
{
    public class User : IAggregateRoot
    {
        public Guid Id { get; init; }
        
        public PhoneNumber PhoneNumber { get; private set; }

        private string? passwordHash;

        public UserAcessFail UserAcessFail { get; private set; }
        private User() { }

        public User(PhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
            this.Id= Guid.NewGuid();
            this.UserAcessFail = new UserAcessFail(this);
        }

        public bool HasPassword()
        {
            return !string.IsNullOrEmpty(this.passwordHash);
        }

        public bool ChangePassword(string newPassword)
        {
            if(newPassword.Length <= 3)
            {
                throw new ArgumentException("密码字数必须大于3");
            }
            else
            {
                this.passwordHash= HashHelper.ComputeMd5Hash(newPassword);
                return true;
            }
        }

        public bool CheckPassword(string newPassword)
        {
            return this.passwordHash == HashHelper.ComputeMd5Hash(newPassword);
        }

        public void ChangePhoneNumber(PhoneNumber phoneNumber)
        {
            this.PhoneNumber = phoneNumber;
        }

    }
}