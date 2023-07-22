﻿using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult>LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsynce(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string OldPassword, string newPassword);

        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);

        // Metodo que valida a password do utilizador
        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        //Task<string> GenerateEmailConfirmationTokenAsync(User user);

        //Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        //Task<User> GetUserByIdAsync(string userId);

        //Task<string> GeneratePasswordResetTokenAsync(User user);

        //Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    }
}
