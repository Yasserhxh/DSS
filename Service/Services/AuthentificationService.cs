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
        private readonly IAuthentificationRepository authentificationRepository;
        private readonly IMapper mapper;


        public AuthentificationService(IAuthentificationRepository authentificationRepository, IMapper mapper)
        {
            this.authentificationRepository = authentificationRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {

            return await this.authentificationRepository.Register(registerModel);
        }

        public async Task<ApplicationUser> Login(LoginModel loginModel)
        {
            var user = await this.authentificationRepository.Login(loginModel);

            if (user == null)
                return null;

            return user;
        }
        public async Task<bool> Logout()
        {
            return await this.authentificationRepository.Logout();
        }

        public async Task<List<UserModel>> getListUsers()
        {
            return await authentificationRepository.getListUsers();
        }
        public async Task<bool> EnableDisableUser(string Id, int code)
        {
            return await authentificationRepository.EnableDisableUser(Id, code);
        }

        public async Task<List<RoleModel>> GetRoles()
        {
            return mapper.Map<List<IdentityRole>, List<RoleModel>>(await this.authentificationRepository.GetRoles());
        }

        public async Task<Response> AjouterUnUtilisateur(RegisterModel registerModel)
        {
            return await this.authentificationRepository.AjouterUnUtilisateur(registerModel);
        }
        public async Task<Response> UpdateUser(UserModel model)
        {
            return await this.authentificationRepository.UpdateUser(model);
        }
        public async Task<Response> UpdateProfil(UpdateUserModel model)
        {
            return await this.authentificationRepository.UpdateProfil(model);
        }
    }
}
