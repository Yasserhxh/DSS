using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Domain.Authentication;
using Repository.IRepositories;
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
            if (user == null) return null;
            var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, lockoutOnFailure: true);
            return result.Succeeded ? user : null;
            //return user;
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {
            var user = new ApplicationUser { UserName = registerModel.UserName, Email = registerModel.Email };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            return result.Succeeded;
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
            var role = Roles.FirstOrDefault();
            return role;
        }

        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<string> FindUserRoleByEmail(string email)
        {
            if (await _userManager.FindByEmailAsync(email) is not { } user)
                return null;
            var roles= await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
        public async Task<List<UserModel>> getListUsers()
        {
            var users = (_db.Users.Where(x => x.Id != "3375dc1e-b359-403e-9f13-5e2b395ffafc")
                .Join(_db.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                .Join(_db.Roles, @t => @t.ur.RoleId, r => r.Id,
                    (@t, r) => new UserModel()
                    {
                        Id = @t.u.Id,
                        UserName = @t.u.UserName,
                        Email = @t.u.Email,
                        Role = r.Name,
                        Nom = @t.u.Nom,
                        Prenom = @t.u.Prenom,
                        PhoneNumber = @t.u.PhoneNumber,
                        Statut = @t.u.IsActive
                    })).ToListAsync();
            return await users;
        }

        public async Task<bool> EnableDisableUser(string Id, int code)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
                return false;
            try
            {
                user.IsActive = code != 0;

                await _unitOfWork.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            return await _db.Roles
                .Where(x => !x.Name.Contains("Admin"))
                .ToListAsync();
        }

        public async Task<Response> AjouterUnUtilisateur(RegisterModel registerModel)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
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

        public async Task<Response> UpdateUser(UserModel model)
        {
            await using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
                    var roles = await _userManager.GetRolesAsync(user);
                    var role = roles.FirstOrDefault();
                    var usernames = await _db.Users.Where(x => x.Id != model.Id).Select(x => x.UserName).ToListAsync();
                    var emails = await _db.Users.Where(x => x.Id != model.Id).Select(x => x.Email).ToListAsync();
                    if (usernames.Any(u => string.Equals(u, model.UserName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        return new Response { Success = false, Message = "Utilisateur existe déjà!" };
                    }
                    if (emails.Any(e => e == model.Email))
                    {
                        return new Response { Success = false, Message = "Email existe déjà!" };
                    }

                    if (user != null)
                    {
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
                            await transaction.RollbackAsync();
                            return new Response { Success = false, Message = Messages.ErrorRole };
                            
                        }
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

            if (usernames.Any(u => string.Equals(u, model.UserName, StringComparison.CurrentCultureIgnoreCase)))
            {
                return new Response { Success = false, Message = "Utilisateur existe déjà!" };
            }

            if (emails.Any(e => e == model.Email))
            {
                return new Response { Success = false, Message = "Email existe déjà!" };
            }

            if (user == null) return new Response { Success = true, Message = "" };
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
