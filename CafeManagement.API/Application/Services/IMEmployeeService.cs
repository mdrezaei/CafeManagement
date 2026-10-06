using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.ValueObjects;

namespace CafeManagement.API.Application.Services
{
    public interface IMEmployeeService
    {
        Task<MEmployee> CreateEmployeeAsync(string name, string password, string phoneNumber, EmployeeSections section, EmployeeLevels level);
        Task<MEmployee> UpdateEmployeeAsync(MEmployee employee); 
        Task<MEmployee> UpdateEmployeeNameAsync(MEmployee employee, string name); 
        Task<MEmployee> UpdateEmployeePasswordAsync(MEmployee employee, string password); 
        Task<MEmployee> UpdateEmployeePhoneNumberAsync(MEmployee employee, string phoneNumber); 
        Task<MEmployee> UpdateEmployeeJoinedDateAsync(MEmployee employee, DateTime newJoinedDate); 
        Task<MEmployee> UpdateEmployeeSectionAsync(MEmployee employee, EmployeeSections employeeSection); 
        Task<MEmployee> UpdateEmployeeSecondarySectionAsync(MEmployee employee, EmployeeSections employeeSection); 
        Task<MEmployee> DeleteEmployeeSecondarySectionAsync(MEmployee employee); 
        Task<MEmployee> UpdateEmployeeLevelAsync(MEmployee employee, EmployeeLevels employeeLevel); 
        Task<MEmployee> UpdateEmployeeSecondaryLevelAsync(MEmployee employee, EmployeeLevels employeeLevel); 
        Task<MEmployee> DeleteEmployeeSecondaryLevelAsync(MEmployee employee); 
        Task<MEmployee> UpdateEmployeeDescriptionAsync(MEmployee employee, string description); 
        Task<MEmployee> DeleteEmployeeDescriptionAsync(MEmployee employee);
        Task SoftDeleteEmployeeAsync(MEmployee employee);
        Task<IReadOnlyList<MEmployee>> GetAllEmployeesAsync(bool includeDeleted = false);
        Task<IReadOnlyList<MEmployee>> GetEmployeesByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false);
        Task<MEmployee?> GetEmployeeByIdAsync(Guid id, bool includeDeleted = false);
        Task<MEmployee?> GetEmployeeByPhoneNumberAsync(string phoneNumber, bool includeDeleted = false);
        Task<IReadOnlyList<MEmployee>> GetEmployeeByNameAsync(string name, bool includeDeleted = false);
        Task<IReadOnlyList<MEmployee>> GetEmployeesBySectionAsync(EmployeeSections section, bool includeDeleted = false);
        Task<IReadOnlyList<MEmployee>> GetEmployeesByLevelAsync(EmployeeLevels level, bool includeDeleted = false);
    }
}
