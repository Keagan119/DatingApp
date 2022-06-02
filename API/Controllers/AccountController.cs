using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
         private readonly ITokenService _tokenService;
        public AccountController(ApplicationDbContext dbContext,ITokenService tokenService)
        {
            _context = dbContext;
            _tokenService = tokenService;
        }

        //registering a user
        //implementing hmac for password hashing
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO dto)
        {
            //upon registration checks the entered username
            if(await UserExists(dto.Username)){
                return BadRequest("Username is taken");
            }
            using var hmac = new HMACSHA512();

            var user = new AppUser{
                UserName = dto.Username.ToLower(),
                 PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
               
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
            
        }

        [HttpPost("login")]

        public async Task<ActionResult<UserDTO>> Login(LoginDTO log){

            var user = await _context.Users.SingleOrDefaultAsync(x=> x.UserName == log.Username);

            if(user == null){
                return Unauthorized("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(log.Password));

            for(int i = 0; i<computedHash.Length;i++){
                if(computedHash[i] != user.PasswordHash[i]){
                    return Unauthorized("Invalid password");
                }
            }

             return new UserDTO{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
             };
            
        }

        //checks to see if username exists in table
        private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x=> x.UserName == username.ToLower());
        }
    }
}