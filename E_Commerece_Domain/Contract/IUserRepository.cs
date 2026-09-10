using E_Commerece.Domain.Entites.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Domain.Contract
{
    public interface IUserRepository
    {

        Task<ApplicationUser> FindUserbyEmailAsync(string email);

        Task<ApplicationUser> CreateUserAsync(ApplicationUser user);

        Task<bool> CheckUserPasswordAsync(string password, string email);
        Task<bool> CheckEmailExist(string  email);
    }
}
