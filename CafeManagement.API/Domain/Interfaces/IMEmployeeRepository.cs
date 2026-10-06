using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.ValueObjects;

namespace CafeManagement.API.Domain.Interfaces
{
    public interface IMEmployeeRepository
    {
        Task AddEmployeeAsync(MEmployee employee);
        Task UpdateEmployeeAsync(MEmployee employee);
        Task<IReadOnlyList<MEmployee>> GetAllEmployeesAsync(bool includeDeleted = false);
        Task<IReadOnlyList<MEmployee>> GetEmployeesByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false);
        Task<MEmployee?> GetEmployeeByIdAsync(Guid id, bool includeDeleted = false);
        Task<MEmployee?> GetEmployeeByPhoneNumberAsync(string phoneNumber, bool includeDeleted = false);
        Task<IReadOnlyList<MEmployee>> GetEmployeeByNameAsync(string name, bool includeDeleted = false);
        Task<IReadOnlyList<MEmployee>> GetEmployeesBySectionAsync(EmployeeSections section, bool includeDeleted = false);
        Task<IReadOnlyList<MEmployee>> GetEmployeesByLevelAsync(EmployeeLevels level, bool includeDeleted = false);


    } 
}
