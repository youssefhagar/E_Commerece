using E_Commerece.Application.Contracts;
using E_Commerece.Application.Dtos;
using E_Commerece.Application.Dtos.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;

namespace E_Commerece.API.Controllers
{
    public class AuthController(IAuthService authService) 
        : ApiBaseController
    {


        //Login
        [HttpPost("login")]
        public async Task<ActionResult<UserDTo>> Login(LoginDto loginDto)
            => ToActionResult( await authService.Login(loginDto));

        //Register
        [HttpPost("register")]
        public async Task<ActionResult<UserDTo>> Register(RegisterDto registerDto)
            => ToActionResult( await authService.Register(registerDto));

        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> Register([FromQuery]string email)
            => ToActionResult(await authService.EmailExist(email));

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetAddress(CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            return ToActionResult(await authService.GetAddress(email!));
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address,CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            return ToActionResult(await authService.UpsertAddress(email!, address));
        }

        /*

         {
  "email": "youssef@gmail.com",
  "password": "P@ssw0rd"
} 

         * */

    }
}
