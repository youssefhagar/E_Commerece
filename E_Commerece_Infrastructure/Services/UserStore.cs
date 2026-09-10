using E_Commerece.Application.Common;
using E_Commerece.Application.Dtos;
using E_Commerece.Application.Dtos.AuthDtos;
using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Infrastructure.Repository
{
    internal class UserStore(UserManager<ApplicationUser> userManager)
        : IUserStore
    {
        public async Task<bool> CheckEmailExist(string email)
            => await userManager.FindByEmailAsync(email) is not null ;

        public async Task<bool> CheckUserPasswordAsync(string password, string email)
        {
            
            var user = await userManager.FindByEmailAsync(email);

            if (user == null) return false;
            

            return
                await userManager.CheckPasswordAsync(user, password) ? true : false;
        }

        public async Task<UserDTo> CreateUserAsync(ApplicationUser user,string password)
        {
            var result = await userManager.CreateAsync(user,password);
            if(result.Succeeded)
            {
                var mappedUser = new UserDTo()
                {
                    DisplayName = user.DisplayName,
                    Id = user.Id,
                    Eamil = user.Email!,
                    UserName = user.UserName!
                };
                return mappedUser;
            }
            return null;
        }

        public async Task<UserDTo> FindUserbyEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null) return null!;

            var mappedUser = new UserDTo()
            {
                DisplayName = user.DisplayName,
                Id = user.Id,
                Eamil = user.Email!,
                UserName = user.UserName!
            };

            return  mappedUser;
        }

        

        public async Task<List<string>> GetRoles(string email)
        {
            var user  = await userManager.FindByEmailAsync(email);
            if (user == null)
                return null!;
            var roles = await userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<AddressDto> GetAddressAsync(string email)
        {
            var user = 
                await userManager.Users
                .Include(X => X.Address)
                .FirstOrDefaultAsync(U => U.Email == email);

            if (user == null)return null!;
            if (user.Address == null) return null!;
            var address = new AddressDto
            {
                City = user.Address.City,
                Street = user.Address.Street,
                Country = user.Address.Country,
                FirstName = "Tset",
                LastName = "Tset"
                
            };
            return address;

        }
        public async Task<AddressDto> UpsertAddress(string email, AddressDto address)
        {
            var user =
                await userManager.Users
                .Include(X => X.Address)
                .FirstOrDefaultAsync(U => U.Email == email);

            if (user == null) return null!;

            if(user.Address == null)
            {
                //Insert
                user.Address = new Address
                {
                    Street = address.Street,
                    Country = address.Country,
                    City = address.City,

                };
            }
            else
            {
                //Update
                user.Address.Street = address.Street;
                user.Address.Country = address.Country;
                user.Address.City = address.City;
            }

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded ? address : null;

        }
    }
}
