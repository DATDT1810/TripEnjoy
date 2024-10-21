using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;
using TripEnjoy.Infrastructure.Helper;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
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
            var userBlock = _context.Accounts.FirstOrDefault(a => a.AccountEmail == account.email);
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
                 //new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
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

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var accountNew = mapper.Map<Account>(account);

                        // Start
                        // Set giá trị mặc định cho Account
                        // accountNew.AccountPassword = user.PasswordHash;
                        accountNew.AccountUsername = account.email;
                        accountNew.AccountFullname = "Anonymous Customer";
                        accountNew.AccountRole = 1;
                        accountNew.AccountIsDeleted = false;
                        //accountNew.WalletID = accountNew.AccountId;
                        accountNew.AccountUpLevel = false;
                        accountNew.AccountPhone = "+84";
                        accountNew.AccountAddress = "VietNam";
                        accountNew.AccountGender = "Male";
                        accountNew.AccountDateOfBirth = DateTime.Now;
                        accountNew.AccountImage = "https://res.cloudinary.com/dgqkuowgr/image/upload/v1729254303/user/haoncce171957%40fpt.edu.vn.jpg";
                        accountNew.UserId = user.Id;
                        // End 
                        _context.Accounts.Add(accountNew);
                        await _context.SaveChangesAsync();

                        var wallet = new Wallet()

                        {
                            WalletBalance = 0,
                            AccountId = accountNew.AccountId
                        };
                        _context.Wallets.Add(wallet);
                        await _context.SaveChangesAsync();

                        accountNew.WalletID = wallet.WalletId;
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return user;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
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

        public async Task<TokenResponseDTO> LoginGoogle(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            // chưa có thì tạo thông tin đăng nhập lần đầu
            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = email,
                    Email = email
                };
                var identity = await userManager.CreateAsync(user);
                if (identity.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync(AppRole.User))
                    {
                        await roleManager.CreateAsync(new IdentityRole(AppRole.User));
                    }
                    await userManager.AddToRoleAsync(user, AppRole.User);

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var accountNew = new Account();

                            // Start
                            // Set giá trị mặc định cho Account
                            accountNew.AccountEmail = user.Email;
                            accountNew.AccountPassword = RandowPassword.GenaratePasss(6, true);
                            accountNew.AccountUsername = user.Email;
                            accountNew.AccountFullname = "Anonymous Customer";
                            accountNew.AccountRole = 1;
                            accountNew.AccountIsDeleted = false;
                            //accountNew.WalletID = 1;
                            accountNew.AccountUpLevel = false;
                            accountNew.AccountPhone = "+84";
                            accountNew.AccountAddress = "VietNam";
                            accountNew.AccountGender = "Male";
                            accountNew.AccountDateOfBirth = DateTime.Now;
                            accountNew.AccountImage = "https://res.cloudinary.com/dgqkuowgr/image/upload/v1729254303/user/haoncce171957%40fpt.edu.vn.jpg";
                            accountNew.UserId = user.Id;
                            // End 

                            _context.Accounts.Add(accountNew);
                            await _context.SaveChangesAsync();

                            var wallet = new Wallet()
                            {
                                WalletBalance = 0,
                                AccountId = accountNew.AccountId
                            };
                            _context.Wallets.Add(wallet);
                            await _context.SaveChangesAsync();

                            accountNew.WalletID = wallet.WalletId;
                            await _context.SaveChangesAsync();

                            await transaction.CommitAsync();
                          
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }
            var result = await signInManager.CanSignInAsync(user);
            var token = await GenerateAccessToken(user);
            if (token != null)
            {
                return new TokenResponseDTO
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken
                };
            }
            throw new UnauthorizedAccessException("Invalid login attempt");
        }



        public async Task<string> CheckEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user.Email;
        }

        public async Task<bool> ResetPassword(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Mã hóa mật khẩu mới
                user.PasswordHash = userManager.PasswordHasher.HashPassword(user, password);
                var identity = await userManager.UpdateAsync(user);
                if (identity.Succeeded)
                {
                    var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == user.Id);
                    if (account != null)
                    {
                        account.AccountPassword = password;
                        _context.Accounts.Update(account);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            return false;

        }


        public async Task<UserProfile> GetUserProfile(string userId)
        {
            try
            {
                var user = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountEmail.Equals(userId));
                if (user != null)
                {
                    return mapper.Map<UserProfile>(user);
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("The task was canceled. Possible timeout issue.");
                Console.WriteLine($"Error details: {ex.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
            return null;
        }

        public async Task<UserProfile> UpdateUserProfile(UserProfile userProfile)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == userProfile.AccountId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            mapper.Map(userProfile, user);
            _context.Accounts.Update(user);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return mapper.Map<UserProfile>(user);
            }
            throw new Exception("No changes were made to the user profile.");
        }

        public Task<UserProfile> CreateUser(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }

        // Get all accounts
        public async Task<IEnumerable<TripEnjoy.Domain.Models.Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        // Get account by Id
        public async Task<Account> GetAccountByIdAsync(string userId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);
        }

        // Add a new account
        public async Task<Account> AddAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        // Update an existing account
        public async Task<Account> UpdateAccountAsync(Account account)
        {
            Account obj = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == account.UserId);
            if (obj == null)
            {
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
            }
            return obj;
        }

        public async Task<Account> UpdateAccountLevelAsync(string UId)
        {
            Account acc = await _context.Accounts.Where(a => a.UserId == UId).FirstOrDefaultAsync();
            if (acc != null)
            {
                acc.AccountUpLevel = true;
                acc.AccountRole = 2;
                _context.Accounts.Update(acc);
                await _context.SaveChangesAsync();
            }
            return acc;

        }

		public async Task<Account> GetAccountById(int accountId)
		{
			var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
			if (account == null)
			{
				throw new Exception($"Account with ID {accountId} not found.");
			}
			return account;
		}
	}
}
