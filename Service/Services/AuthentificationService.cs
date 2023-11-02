using AutoMapper;
using Domain.Authentication;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Repository.IRepositories;
using Service.IServices;

namespace Service.Services
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly IAuthentificationRepository _authentificationRepository;
        private readonly IMapper _mapper;


        public AuthentificationService(IAuthentificationRepository authentificationRepository, IMapper mapper)
        {
            this._authentificationRepository = authentificationRepository;
            this._mapper = mapper;
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {

            return await this._authentificationRepository.Register(registerModel);
        }

        public async Task<ApplicationUser?> Login(LoginModel loginModel)
        {
            var user = await this._authentificationRepository.Login(loginModel);

            return user ?? null;
        }
        public async Task<bool> Logout()
        {
            return await this._authentificationRepository.Logout();
        }

        public async Task<List<UserModel>> getListUsers()
        {
            return await _authentificationRepository.getListUsers();
        }
        public async Task<bool> EnableDisableUser(string id, int code)
        {
            return await _authentificationRepository.EnableDisableUser(id, code);
        }

        public async Task<List<RoleModel>> GetRoles()
        {
            return _mapper.Map<List<IdentityRole>, List<RoleModel>>(await this._authentificationRepository.GetRoles());
        }

        public async Task<Response> AjouterUnUtilisateur(RegisterModel registerModel)
        {
            return await this._authentificationRepository.AjouterUnUtilisateur(registerModel);
        }
        public async Task<Response> UpdateUser(UserModel model)
        {
            return await this._authentificationRepository.UpdateUser(model);
        }
        public async Task<Response> UpdateProfil(UpdateUserModel model)
        {
            return await _authentificationRepository.UpdateProfil(model);
        }

        public async Task<string> FindUserRoleByEmail(string email)
        {
            return await _authentificationRepository.FindUserRoleByEmail(email);
        }
        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await _authentificationRepository.FindUserByEmail(email);
        }

        public async Task<Response> UpdateUserRole(UserModel model)
        {
            return await _authentificationRepository.UpdateUserRole(model);
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await _authentificationRepository.GetUserByEmail(email);
        }
    }
}
