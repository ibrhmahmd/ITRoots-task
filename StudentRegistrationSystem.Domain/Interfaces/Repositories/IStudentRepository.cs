using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Student entity operations
/// </summary>
public interface IStudentRepository
{
    /// <summary>
    /// Gets a student by ID
    /// </summary>
    Task<Student?> GetByIdAsync(string id);

    /// <summary>
    /// Gets a student by UserId
    /// </summary>
    Task<Student?> GetByUserIdAsync(string userId);

    /// <summary>
    /// Gets all students
    /// </summary>
    Task<IEnumerable<Student>> GetAllAsync();

    /// <summary>
    /// Creates a new student
    /// </summary>
    Task<string> CreateAsync(Student student);

    /// <summary>
    /// Updates an existing student
    /// </summary>
    Task<bool> UpdateAsync(Student student);

    /// <summary>
    /// Checks if a student exists for a user
    /// </summary>
    Task<bool> ExistsByUserIdAsync(string userId);
}

