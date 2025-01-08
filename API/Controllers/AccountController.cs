using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Dtos;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")] //account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await UserExists(registerDto.Username)) return BadRequest("username is taken");
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {

                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,

            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDto{
                Username = user.Username,
                Token = tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")] //account/login
        public async Task<ActionResult<UserDto>> LoginDto(LoginDto loginDto)
        {

            var user = await context.Users.
            FirstOrDefaultAsync(x => x.Username == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("user not found");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {

                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }
            return new UserDto{
                Username = user.Username,
                Token = tokenService.CreateToken(user)
            };
        }


        //method to check if the username exists
        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower());
        }

    }
}
