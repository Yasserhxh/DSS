using Domain.Authentication;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Repository.IRepositories
{
    public interface IAuthentificationRepository
    {
        Task<bool> Register(RegisterModel userModel);
        Task<ApplicationUser> Login(LoginModel loginModel);
        Task<bool> Logout();
        Task<ApplicationUser> GetUserByName(string name);
        Task<string> GetUserRole(ApplicationUser user);
        Task<List<UserModel>> getListUsers();
        Task<bool> EnableDisableUser(string Id, int code);
        Task<List<IdentityRole>> GetRoles();
        Task<Response> AjouterUnUtilisateur(RegisterModel registerModel);
        Task<Response> UpdateUser(UserModel model);
        Task<Response> UpdateProfil(UpdateUserModel model);
        Task<string> FindUserRoleByEmail(string email);
        Task<ApplicationUser> FindUserByEmail(string email);
    }
}
