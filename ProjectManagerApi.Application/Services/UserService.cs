using AutoMapper;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Application.Interfaces;
using ProjectManagerApi.Application.Services.Interfaces;
using ProjectManagerApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagerApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto userDto)
        {
            var user = new User(userDto.Name, userDto.Email);
            user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApplicationException($"Erro ao criar usuário: {errors}");
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(Guid id, UpdateUserDto userDto)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if (existingUser == null)
            {
                return null;
            }

            bool changesMade = false;

            if (userDto.Name != null && existingUser.Name != userDto.Name)
            {
                existingUser.UpdateName(userDto.Name);
                changesMade = true;
            }

            if (userDto.Email != null && existingUser.Email != userDto.Email)
            {
                var userWithNewEmail = await _userManager.FindByEmailAsync(userDto.Email);
                if (userWithNewEmail != null && userWithNewEmail.Id != existingUser.Id)
                {
                    throw new ApplicationException("O email já está em uso por outro usuário.");
                }

                var setEmailResult = await _userManager.SetEmailAsync(existingUser, userDto.Email);
                if (!setEmailResult.Succeeded)
                {
                    var errors = string.Join(", ", setEmailResult.Errors.Select(e => e.Description));
                    throw new ApplicationException($"Erro ao atualizar o email: {errors}");
                }
                changesMade = true;
            }

            if (userDto.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var resetPasswordResult = await _userManager.ResetPasswordAsync(existingUser, token, userDto.Password);
                if (!resetPasswordResult.Succeeded)
                {
                    var errors = string.Join(", ", resetPasswordResult.Errors.Select(e => e.Description));
                    throw new ApplicationException($"Erro ao atualizar a senha: {errors}");
                }
                changesMade = true;
            }

            if (changesMade)
            {
                var updateResult = await _userManager.UpdateAsync(existingUser);
                if (!updateResult.Succeeded)
                {
                    var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                    throw new ApplicationException($"Erro ao salvar alterações no usuário: {errors}");
                }
            }

            return _mapper.Map<UserResponseDto>(existingUser);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<UserResponseDto?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, password);

            if (passwordCheck)
            {
                return _mapper.Map<UserResponseDto>(user);
            }
            return null;
        }
    }
}