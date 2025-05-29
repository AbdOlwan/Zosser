using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using BLL_OnlineStore.DTOs.UserDTOs;
using BLL_OnlineStore.Interfaces;
using BLL_OnlineStore.Interfaces.PeopleBusServices;
using BLL_OnlineStore.Services.PeopleBusServices;
using DAL_OnlineStore.Configurations.Config;
using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Repositories.Implementations.PeopleRepository;
using DAL_OnlineStore.Repositories.Interfaces.PeopleRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomerServices _customerServices;
        private readonly IPersonServices _personServices;
        private readonly AppDbContext _context;

        private readonly JWT _jwt;


        public AuthService(UserManager<ApplicationUser> usermanager, RoleManager<IdentityRole> roleManager,
                              IOptions<JWT> jwt, ICustomerServices customerServices, IPersonServices personServices
            , AppDbContext contex)
        {
            _userManager = usermanager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _customerServices = customerServices;
            _personServices = personServices;
            _context = contex;
        }

        public async Task<AuthModel?> RegisterAsync(RegisterDTO model)
        {
            // 1. تأكد الرقم مش مسجل
            if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber))
                return new AuthModel { Message = "Phone Number Is Already Used!" };

            var person = await _personServices
                                .FindPersonByPhonNumber(model.PhoneNumber);
            if(person == null)
            {


                await _personServices.AddNewPerson(new PersonDTO
                {

                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    IsActive    = model.IsActive,

                });
                
            }


             person = await _personServices
                                        .FindPersonByPhonNumber(model.PhoneNumber);

            if (person == null) throw new Exception("Person Not Added!!");

            // إضافة المستخدم
            var user = new ApplicationUser
            {
                PhoneNumber = model.PhoneNumber,
                PersonId = person.PersonId,
                UserName = model.Username

            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return new AuthModel { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

            var customerDto = new CustomerDTO
            {
                PersonId = person.PersonId
                // يمكنك إضافة خصائص أخرى للعميل هنا
            };
            await _customerServices.AddNewCustomer(customerDto);

            // ربط بالـRole
            await _userManager.AddToRoleAsync(user, "Customer");
            
            // إصدار التوكن
            var token = await CreateJwtToken(user);

            return await BuildAuthModel(user, token);

        }
        public async Task<AuthModel> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.Users
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return new AuthModel { Message = "Phone or Password is incorrect." };

            var jwtToken = await CreateJwtToken(user);
            return await BuildAuthModel(user, jwtToken);
        }

        public async Task<AuthModel> GetTokenAsync(TokenRequestDTO model)
        {
            var authModel = new AuthModel();

            // 1. ابحث عن المستخدم برقم الموبايل
            var user = await _userManager.Users
                .Include(u => u.Person)              // لجلب بيانات الـPerson
                .SingleOrDefaultAsync(u => u.PhoneNumber == model.Phone);

            // 2. تحقق من صحة الـ credentials
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Phone Number Or Passwerd Not Correct!!!";
                return authModel;
            }

            // 3. أنشئ التوكن
            var token = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            // 4. املأ الـ AuthModel
            authModel.IsAuthenticated = true;
            authModel.Token = token;
            authModel.Username = user.UserName!;
            authModel.PhoneNumber = user.PhoneNumber!;    // بقى الـ UserName = Phone
            authModel.Expiry = DateTime.UtcNow.AddDays(_jwt.DurationInDays);
            authModel.Roles = rolesList.ToList();


            return authModel;
        }

        public async Task<bool> AddUserRoleAsynce(AddRoleDTO role)
        {
            var user = await _userManager.FindByIdAsync(role.UserId);
            if (user == null) return false;

            if (!await _roleManager.RoleExistsAsync(role.RoleName))
                await _roleManager.CreateAsync(new IdentityRole(role.RoleName));

            if (await _userManager.IsInRoleAsync(user, role.RoleName))
                return false;

            return (await _userManager.AddToRoleAsync(user, role.RoleName)).Succeeded;
        }

        public async Task<bool> AddUserClaimAsync(AddClaimDTO claim)
        {
            var user = await _userManager.FindByIdAsync(claim.userId);
            if (user == null) return false;

            var existingClaims = await _userManager.GetClaimsAsync(user);
            if (existingClaims.Any(c => c.Type == claim.claimType && c.Value == claim.claimValue))
                return false;

            return (await _userManager.AddClaimAsync(user, new Claim(claim.claimType, claim.claimValue))).Succeeded;
        }
        private async Task<string> CreateJwtToken(ApplicationUser user)
        {
            // 1. اجمع Claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(r => new Claim("roles", r));

            var baseClaims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.PhoneNumber ?? ""),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("uid", user.Id),
        // تقدر تضيف رقم الموبايل كـ claim أيضاً
       // new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber ?? "")
    };

            var allClaims = baseClaims
                .Union(userClaims)
                .Union(roleClaims);

            // 2. جهّز Signing Credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. أنشئ الـ JWT
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: allClaims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private async Task<AuthModel> BuildAuthModel(ApplicationUser user, string jwtToken)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new AuthModel
            {
                IsAuthenticated = true,
                Token = jwtToken,
                Expiry = DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                Roles = roles.ToList(),
                PhoneNumber = user.PhoneNumber!,
                Username = user.UserName!

            };
        }

    }

}
