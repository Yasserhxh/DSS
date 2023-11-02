using Domain.Authentication;
using Domain.Enums;
using Domain.Models;

namespace Service.IServices
{
    public interface IAuthentificationService
    {
        Task<bool> Register(RegisterModel userModel);
        Task<ApplicationUser?> Login(LoginModel loginModel);
        Task<bool> Logout();
        Task<List<UserModel>> getListUsers();
        Task<bool> EnableDisableUser(string Id, int code);
        Task<List<RoleModel>> GetRoles();
        Task<UserModel> GetUserByEmail(string email);
        Task<Response> AjouterUnUtilisateur(RegisterModel registerModel);
        Task<Response> UpdateUser(UserModel model);
        Task<Response> UpdateProfil(UpdateUserModel model);
        Task<string> FindUserRoleByEmail(string email);
        Task<ApplicationUser> FindUserByEmail(string email);
        Task<Response> UpdateUserRole(UserModel model);
    }
}
