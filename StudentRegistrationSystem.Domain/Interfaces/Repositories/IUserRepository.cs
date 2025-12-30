using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);

    
    Task<User?> GetByUsernameAsync(string username);

   
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Gets a user by email verification token
    /// </summary>
    Task<User?> GetByEmailVerificationTokenAsync(string token);

    /// <summary>
    /// Creates a new user
    /// </summary>
    Task<string> CreateAsync(User user);

    /// <summary>
    /// Updates an existing user
    /// </summary>
    Task<bool> UpdateAsync(User user);

    /// <summary>
    /// Checks if username exists
    /// </summary>
    Task<bool> UsernameExistsAsync(string username);

    /// <summary>
    /// Checks if email exists
    /// </summary>
    Task<bool> EmailExistsAsync(string email);
}
