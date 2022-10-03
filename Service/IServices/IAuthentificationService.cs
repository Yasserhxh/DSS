using Domain.Authentication;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IAuthentificationService
    {
        Task<bool> Register(RegisterModel userModel);
        Task<ApplicationUser> Login(LoginModel loginModel);
        Task<bool> Logout();
        Task<IEnumerable<UserModel>> getListUsers();
        Task<bool> EnableDisableUser(string Id, int code);
        Task<IEnumerable<RoleModel>> GetRoles();
        Task<Response> AjouterUnUtilisateur(RegisterModel registerModel);
        Task<Response> UpdateUser(UserModel model);
        Task<Response> UpdateProfil(UpdateUserModel model);
    }
}
