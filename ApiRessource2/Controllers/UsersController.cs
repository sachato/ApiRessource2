﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRessource2.Models;
using ApiRessource2.Services;
//using ApiRessource2.Migrations;
using ApiRessource2.Helpers;

namespace ApiRessource2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private IUserService _userService;

        public UsersController(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            if (!Tools.IsEmailValid(model.Email))
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var token = _userService.Authenticate(model, user);

            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(token);
        }



        // GET: api/Users
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (!Tools.IsEmailValid(user.Email))
                return BadRequest("Une adresse mail valide doit etre rentré.");
            if (!Tools.IsValidPhoneNumber(user.PhoneNumber))
                return BadRequest("Un numéro de téléphone valide doit etre rentré et doit respecter ce format : +33XXXXXXX .");
            if (!Tools.IsValidPassword(user.Password))
                return BadRequest("Le mot de passe ne convient pas car il ne contient pas : 1 majuscule, 1 minuscule, 1 chiffre, 1 caractère spécial et 12 caractère minimum.");
            //TODO: vérifier si email et pseudo unique, a faire dans la base en modifiant les class peut etre ?
            user.Password = Tools.HashCode(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
