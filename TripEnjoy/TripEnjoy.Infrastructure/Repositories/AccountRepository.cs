using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;
using TripEnjoy.Infrastructure.Entities;
using TripEnjoy.Infrastructure.Helper;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager , IConfiguration configuration , IMapper mapper , RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }
        public async Task<string> Login(AccountDTO account)
        {
            if (account.email == null || account.password == null)
            {
                throw new ArgumentNullException("Email and password must not be null");
            }
            // check user identity
            var user = await userManager.FindByEmailAsync(account.email);
            if (user == null)
            {
                throw new ArgumentNullException("User not found");
            }
            // check password identity
            var password = await userManager.CheckPasswordAsync(user, account.password);
            var result = await signInManager.PasswordSignInAsync(account.email, account.password, false, false);
            if (result.Succeeded)
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, account.email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                var userRole = await userManager.GetRolesAsync(user);
                foreach (var roles in userRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }
                // key value
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512Signature)
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return "";
        }

        public async Task<bool> Register(AccountDTO account)
        {
            if (account.email == null || account.password == null)
            {
                throw new ArgumentNullException("Email and password must not be null");
            }

            var user = new IdentityUser()
            {
                UserName = account.email,
                Email = account.email
            };
            var identity = await userManager.CreateAsync(user, account.password);
            if (identity.Succeeded)
            {
                if(!await roleManager.RoleExistsAsync(AppRole.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(AppRole.User));
                }
                await userManager.AddToRoleAsync(user, AppRole.User);
              
                var accountNew = mapper.Map<Account>(account);
                context.Accounts.Add(accountNew);
                return true;
            }
            return false;
        }
    }

}
