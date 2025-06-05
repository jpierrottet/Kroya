// Models/IdentityModels.cs
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BlazorProyectoPostres.Models
{

    public class ApplicationUser : IdentityUser
    {
       
    }

 
    public class IdentityModels : IdentityDbContext<ApplicationUser>
    {
        public IdentityModels(DbContextOptions<IdentityModels> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }


    }


    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private readonly IdentityModels _db;

        public CustomClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IdentityModels db)
            : base(userManager, roleManager, optionsAccessor)
        {
            _db = db;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
         
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity!;

           
            var usuario = await _db.Users
                .Where(u => u.Id == user.Id)

                .Select(u => new
                {
                    Nombre = u.UserName,  
                    Ciudad = "",           
                    Modulos = _db.UserRoles
                                 .Where(ur => ur.UserId == u.Id)
                                 .Join(_db.Roles,
                                       ur => ur.RoleId,
                                       r => r.Id,
                                       (ur, r) => r.Name)
                                 .Distinct()
                                 .ToList()
                })
                .FirstOrDefaultAsync();

            if (usuario != null)
            {
                identity.AddClaim(new Claim("Nombre", usuario.Nombre));
                identity.AddClaim(new Claim("Ciudad", usuario.Ciudad));
                identity.AddClaim(new Claim("Modulos", JsonConvert.SerializeObject(usuario.Modulos)));


            }

            return principal;
        }
    }

    public class InMemoryTicketStore : ITicketStore
    {
        private readonly Dictionary<string, AuthenticationTicket> _store = new();

        public Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = Guid.NewGuid().ToString();
            _store[key] = ticket;
            return Task.FromResult(key);
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            _store[key] = ticket;
            return Task.CompletedTask;
        }

        public Task<AuthenticationTicket?> RetrieveAsync(string key)
        {
            _store.TryGetValue(key, out var ticket);
            return Task.FromResult(ticket);
        }

        public Task RemoveAsync(string key)
        {
            _store.Remove(key);
            return Task.CompletedTask;
        }
    }

}
