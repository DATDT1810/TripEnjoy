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
            if (string.IsNullOrWhiteSpace(account.email) || string.IsNullOrWhiteSpace(account.password))
            {
                throw new ArgumentNullException("Email and password must not be null");
            }
            var userBlock = context.Accounts.FirstOrDefault(a => a.AccountEmail == account.email);
            if (userBlock?.AccountIsDeleted == true)
            {
                throw new UnauthorizedAccessException("This account has been banned");
            }
            // check user identity
            var user = await userManager.FindByEmailAsync(account.email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }
            
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
                        expires: DateTime.UtcNow.AddHours(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
                    );
                    return new  JwtSecurityTokenHandler().WriteToken(token);
                }

            throw new UnauthorizedAccessException("Invalid login attempt");
        }

        public async Task<IdentityUser> Register(AccountDTO account)
        {
            if (string.IsNullOrWhiteSpace(account.email) || string.IsNullOrWhiteSpace(account.password))
            {
                throw new ArgumentNullException("Email and password must not be null or empty");
            }
            // check email exists
            var email = await userManager.FindByEmailAsync(account.email);
            if (email != null)
            {
                throw new InvalidOperationException("Email already exists");
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
               
                // Start
                // Set giá trị mặc định cho Account
               // accountNew.AccountPassword = user.PasswordHash;
                accountNew.AccountUsername =  account.email;
                accountNew.AccountFullname = "Anonymous Customer";
                accountNew.AccountRole = 1;
                accountNew.AccountIsDeleted = false;
                accountNew.AccountBalance = 0;
                accountNew.AccountUpLevel = false;
                accountNew.AccountPhone = "+84";
                accountNew.AccountAddress = "VietNam";
                accountNew.AccountGender = "Male";
                accountNew.AccountDateOfBirth = DateTime.Now;
                accountNew.AccountImage = "/...";
                accountNew.UserId = user.Id;               
                // End 

                context.Accounts.Add(accountNew);
                await context.SaveChangesAsync();
                return user;
            }
            throw new InvalidOperationException("User registration failed");
        }
    }

}
