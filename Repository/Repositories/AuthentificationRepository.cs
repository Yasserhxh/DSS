using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Domain.Authentication;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Repository.Data;
using Microsoft.EntityFrameworkCore;
using Repository.UnitOfWork;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage;

namespace Repository.Repositories
{
    public class AuthentificationRepository : IAuthentificationRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthentificationRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApplicationUser> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                    return user;
                else
                    return null;
            }
            //return user;
            return null;
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {
            var user = new ApplicationUser { UserName = registerModel.UserName, Email = registerModel.Email };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
                return true;
            else
                return false;
        }

        public async Task<bool> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ApplicationUser> GetUserByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return user;
        }

        public async Task<string> GetUserRole(ApplicationUser user)
        {
            var Roles = await _userManager.GetRolesAsync(user);
            string role = Roles.FirstOrDefault();
            return role;
        }

        public async Task<IEnumerable<UserModel>> getListUsers()
        {
            var users = (from u in _db.Users.Where(x => x.Id != "3375dc1e-b359-403e-9f13-5e2b395ffafc")
                         join ur in _db.UserRoles on u.Id equals ur.UserId
                         join r in _db.Roles on ur.RoleId equals r.Id
                         select new UserModel()
                         {
                             Id = u.Id,
                             UserName = u.UserName,
                             Email = u.Email,
                             Role = r.Name,
                             Nom = u.Nom,
                             Prenom = u.Prenom,
                             PhoneNumber = u.PhoneNumber,
                             Statut = u.IsActive
                         }).ToListAsync();
            return await users;
        }

        public async Task<bool> EnableDisableUser(string Id, int code)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
                return false;
            try
            {
                if (code == 0)
                    user.IsActive = false;
                else
                    user.IsActive = true;

                await _unitOfWork.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await _db.Roles
                .Where(x => !x.Name.Contains("Admin"))
                .ToListAsync();
        }

        public async Task<Response> AjouterUnUtilisateur(RegisterModel registerModel)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var userExists = await _userManager.FindByNameAsync(registerModel.UserName);
                    if (userExists != null)
                        return new Response { Success = false, Message = "Utilisateur existe déjà!" };

                    var emailExists = await _userManager.FindByEmailAsync(registerModel.Email);
                    if (emailExists != null)
                        return new Response { Success = false, Message = "Email existe déjà!" };

                    var user = new ApplicationUser
                    {
                        UserName = registerModel.UserName,
                        Email = registerModel.Email,
                        Nom = registerModel.Nom,
                        Prenom = registerModel.Prenom,
                        PhoneNumber = registerModel.PhoneNumber,
                        IsActive = true,
                    };

                    var result = await _userManager.CreateAsync(user, registerModel.Password);

                    if (result.Succeeded)
                    {
                        var resultRole = await _userManager.AddToRoleAsync(user, registerModel.Role);
                        if (!resultRole.Succeeded)
                        {
                            transaction.Rollback();
                            return new Response { Success = false, Message = Messages.ErrorRole }; ;
                        }
                        transaction.Commit();
                        return new Response { Success = true, Message = "" };
                    }
                    else
                    {
                        transaction.Rollback();
                        return new Response { Success = false, Message = Messages.Error };
                    }
                }
                catch
                {
                    transaction.Rollback();
                    return new Response { Success = false, Message = Messages.Error };
                }
            }
        }

        public async Task<Response> UpdateUser(UserModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
                    var roles = await _userManager.GetRolesAsync(user);
                    var role = roles.FirstOrDefault();
                    var usernames = await _db.Users.Where(x => x.Id != model.Id).Select(x => x.UserName).ToListAsync();
                    var emails = await _db.Users.Where(x => x.Id != model.Id).Select(x => x.Email).ToListAsync();
                    foreach (var u in usernames)
                    {
                        if (u.ToUpper() == model.UserName.ToUpper())
                            return new Response { Success = false, Message = "Utilisateur existe déjà!" };
                    }
                    foreach (var e in emails)
                    {
                        if (e == model.Email)
                            return new Response { Success = false, Message = "Email existe déjà!" };
                    }

                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.Nom = model.Nom;
                    user.Prenom = model.Prenom;
                    user.PhoneNumber = model.PhoneNumber;

                    await _userManager.UpdateAsync(user);

                    await _userManager.RemoveFromRoleAsync(user, role);

                    var resultRole = await _userManager.AddToRoleAsync(user, model.Role);

                    if (!resultRole.Succeeded)
                    {
                        transaction.Rollback();
                        return new Response { Success = false, Message = Messages.ErrorRole }; ;
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    return new Response { Success = false, Message = Messages.Error };
                }
            }
            return new Response { Success = true, Message = "" };
        }

        public async Task<Response> UpdateProfil(UpdateUserModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
            var usernames = await _db.Users.Where(x => x.Id != model.Id).Select(x => x.UserName).ToListAsync();
            var emails = await _db.Users.Where(x => x.Id != model.Id).Select(x => x.Email).ToListAsync();

            foreach (var u in usernames)
            {
                if (u.ToUpper() == model.UserName.ToUpper())
                    return new Response { Success = false, Message = "Utilisateur existe déjà!" };
            }

            foreach (var e in emails)
            {
                if (e == model.Email)
                    return new Response { Success = false, Message = "Email existe déjà!" };
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Nom = model.Nom;
            user.Prenom = model.Prenom;
            user.PhoneNumber = model.PhoneNumber;
            var hasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = hasher.HashPassword(user, model.Password);
            await _userManager.UpdateAsync(user);

            return new Response { Success = true, Message = "" };
        }
    }
}
