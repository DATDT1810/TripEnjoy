using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public AccountRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }
        public async Task<TokenResponseDTO> Login(AccountDTO account)
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
                var token = await GenerateAccessToken(user);
                if (token != null)
                {
                    return new TokenResponseDTO
                    {
                        AccessToken = token.AccessToken,
                        RefreshToken = token.RefreshToken
                    };
                }
            }

            throw new UnauthorizedAccessException("Invalid login attempt");
        }

        public async Task<TokenResponseDTO> GenerateAccessToken(IdentityUser identityUser)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, identityUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var userRole = await userManager.GetRolesAsync(identityUser);
            foreach (var roles in userRole)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            }
            // key value
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            // access token
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            await userManager.SetAuthenticationTokenAsync(identityUser, "TripEnjoy", "RefreshToken", refreshToken);

            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber).Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
            }
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
                if (!await roleManager.RoleExistsAsync(AppRole.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(AppRole.User));
                }
                await userManager.AddToRoleAsync(user, AppRole.User);

                var accountNew = mapper.Map<Account>(account);

                // Start
                // Set giá trị mặc định cho Account
                // accountNew.AccountPassword = user.PasswordHash;
                accountNew.AccountUsername = account.email;
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

        public async Task<TokenResponseDTO> RefreshToken(string refreshToken)
        {
            var user = userManager.Users.SingleOrDefault(u =>
             userManager.GetAuthenticationTokenAsync(u, "TripEnjoy", "RefreshToken")
             .Result == refreshToken);
            if (user != null)
            {
                var tokens = await GenerateAccessToken(user);
                return tokens;
            }
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        public async Task<bool> Logout(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await signInManager.SignOutAsync();
                await userManager.RemoveAuthenticationTokenAsync(user, "TripEnjoy", "RefreshToken");
                return true;
            }
            return false;
        }
    }
}
