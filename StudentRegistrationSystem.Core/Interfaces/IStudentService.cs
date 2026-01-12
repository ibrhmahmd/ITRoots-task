using System.Threading.Tasks;
using StudentRegistrationSystem.Core.DTOs;

namespace StudentRegistrationSystem.Core.Interfaces;

public interface IStudentService
{
    Task<StudentDto?> GetByUserIdAsync(string userId);
}
