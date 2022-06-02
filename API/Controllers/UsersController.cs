using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context){
            _context = context;
        }

        [HttpGet]
      
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
                return await _context.Users.ToListAsync();

               
        }
        
         [HttpGet("{id}")]
        public async  Task<ActionResult<AppUser>> GetUser(int id){
                return await _context.Users.FindAsync(id);
                
                
        }



    }
}