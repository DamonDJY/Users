using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Domain;
using Users.Domain.Entities;
using Users.Infrastructure;
using Users.Interface;
using Users.WebApi.HttpContex;
using Zack.Commons;

namespace Users.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [UnitOfWork(typeof(UserDbContext))]
    public class CRUDController : ControllerBase
    {
        private readonly UserDbContext _userDbContext;
        private readonly IUserDomainRepository _userDomainRepository;

        public CRUDController(UserDbContext userDbContext, IUserDomainRepository userDomainRepository)
        {
            _userDbContext = userDbContext;
            _userDomainRepository = userDomainRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser(AddUserRes res )
        {
            if (res.Password.Length <= 3)
            {
                return BadRequest("密码至少大于3");
            }

            if(await _userDomainRepository.FindOneAsync(res.PhoneNumber) != null)
            {
                return BadRequest("手机号已存在");
            }
            _userDbContext.Users.Add(new User(res.PhoneNumber));

            return Ok("注册成功");

        }

        [HttpPost]
        public async Task<IActionResult> UnlockUser(Guid id)
        {

            //需要权限操作，可能需要用到授权filter，这个以后再补充

            var user = await _userDbContext.Users.Include(u=>u.UserAcessFail).SingleOrDefaultAsync(x=> x.Id == id);
            if (user == null)
            {
                return BadRequest("用户id不存在");
            }

            user.UserAcessFail.Reset();

            return Ok("用户已解锁");

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePsswordRes res)
        {
            if (res.NewPassword.Length <= 3)
            {
                return BadRequest("密码至少大于3");
            }

            var user = await _userDomainRepository.FindOneAsync(res.UserId);
           
            //compare oldValue 

            if(user!.CheckPassword(res.NewPassword))
            {
                return BadRequest("与旧密码一致");
            }

            if (user.ChangePassword(res.NewPassword))
            {
                return Ok("修改密码成功");
            }
            else
            {
                return BadRequest("修改密码失败");
            }
            
           

        }

    }
}
