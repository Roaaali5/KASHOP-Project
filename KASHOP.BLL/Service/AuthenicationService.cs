using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class AuthenicationService : IAuthenicationService
    {
        private readonly UserManager<ApplicationUser>  _userManager;
        public AuthenicationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user,request.Password);
            if (! result.Succeeded)
                return new RegisterResponse() { Success = false, Message = "Error" };
            
            return new RegisterResponse() { Success = true, Message = "success" };

        }
    }
}
