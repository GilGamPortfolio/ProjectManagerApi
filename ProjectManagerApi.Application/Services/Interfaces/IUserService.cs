using ProjectManagerApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagerApi.Application.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">The DTO containing user creation data.</param>
        /// <returns>A DTO representing the created user.</returns>
        Task<UserResponseDto> CreateUserAsync(CreateUserDto userDto);

        /// <summary>
        /// Retrieves a user by their unique ID.
        /// </summary>
        /// <param name="id">The user's unique ID.</param>
        /// <returns>A DTO representing the user, or null if not found.</returns>
        Task<UserResponseDto?> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>A DTO representing the user, or null if not found.</returns>
        Task<UserResponseDto?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A collection of DTOs representing all users.</returns>
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userDto">The DTO containing updated user data.</param>
        /// <returns>A DTO representing the updated user, or null if the user was not found.</returns>
        Task<UserResponseDto?> UpdateUserAsync(Guid id, UpdateUserDto userDto);

        /// <summary>
        /// Deletes a user by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>True if the user was successfully deleted, false otherwise.</returns>
        Task<bool> DeleteUserAsync(Guid id);
    }
}