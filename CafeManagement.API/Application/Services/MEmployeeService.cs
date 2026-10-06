using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.Interfaces;
using CafeManagement.API.Domain.ValueObjects;

namespace CafeManagement.API.Application.Services
{
    public class MEmployeeService : IMEmployeeService
    {
        private readonly IMEmployeeRepository _mEmployeeRepository;

        public MEmployeeService(IMEmployeeRepository mEmployeeRepository)
        {
            _mEmployeeRepository = mEmployeeRepository;
        }

        public async Task<MEmployee> CreateEmployeeAsync(string name, string password, string phoneNumber, EmployeeSections section, EmployeeLevels level)
        {
            MEmployee? existing = await _mEmployeeRepository.GetEmployeeByPhoneNumberAsync(phoneNumber);
            if (existing != null)
            {
                return existing;
            }

            MEmployee employee = new MEmployee(name, password, phoneNumber, section, level);
            await _mEmployeeRepository.AddEmployeeAsync(employee);
            return employee;

        }

        public async Task<MEmployee> UpdateEmployeeAsync(MEmployee employee)
        {
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> UpdateEmployeeNameAsync(MEmployee employee, string name)
        {
            employee.UpdateName(name);
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> UpdateEmployeePasswordAsync(MEmployee employee, string password)
        {
            employee.UpdatePassword(password);
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> UpdateEmployeePhoneNumberAsync(MEmployee employee, string phoneNumber)
        {
            employee.UpdatePhoneNumber(phoneNumber);
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> UpdateEmployeeJoinedDateAsync(MEmployee employee, DateTime newJoinedDate)
        {
            employee.UpdateJoinedDate(newJoinedDate);
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> UpdateEmployeeSectionAsync(MEmployee employee, EmployeeSections employeeSection)
        {
            employee.UpdateSection(employeeSection);
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> UpdateEmployeeSecondarySectionAsync(MEmployee employee, EmployeeSections employeeSection)
        {
            employee.UpdateSecondarySection(employeeSection);
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> DeleteEmployeeSecondarySectionAsync(MEmployee employee)
        {
            employee.DeleteSecondarySection();
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> UpdateEmployeeLevelAsync(MEmployee employee, EmployeeLevels employeeLevel)
        {
            employee.UpdateLevel(employeeLevel);
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> UpdateEmployeeSecondaryLevelAsync(MEmployee employee, EmployeeLevels employeeLevel)
        {
            employee.UpdateSecondaryLevel(employeeLevel);
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> DeleteEmployeeSecondaryLevelAsync(MEmployee employee)
        {
            employee.DeleteSecondaryLevel();
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> UpdateEmployeeDescriptionAsync(MEmployee employee, string description)
        {
            employee.UpdateDescription(description);
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task<MEmployee> DeleteEmployeeDescriptionAsync(MEmployee employee)
        {
            employee.DeleteDescription();
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
            return employee;
        }

        public async Task SoftDeleteEmployeeAsync(MEmployee employee)
        {
            employee.SoftDelete();
            await _mEmployeeRepository.UpdateEmployeeAsync(employee);
        }

        public async Task<IReadOnlyList<MEmployee>> GetAllEmployeesAsync(bool includeDeleted = false)
        {
            return await _mEmployeeRepository.GetAllEmployeesAsync(includeDeleted);
        }

        public async Task<IReadOnlyList<MEmployee>> GetEmployeesByDateAsync(DateTime from, DateTime? to = null, bool includeDeleted = false)
        {
            return await _mEmployeeRepository.GetEmployeesByDateAsync(from, to, includeDeleted);
        }

        public async Task<MEmployee?> GetEmployeeByIdAsync(Guid id, bool includeDeleted = false)
        {
            return await _mEmployeeRepository.GetEmployeeByIdAsync(id, includeDeleted);
        }

        public async Task<MEmployee?> GetEmployeeByPhoneNumberAsync(string phoneNumber, bool includeDeleted = false)
        {
            return await _mEmployeeRepository.GetEmployeeByPhoneNumberAsync(phoneNumber, includeDeleted);
        }

        public async Task<IReadOnlyList<MEmployee>> GetEmployeeByNameAsync(string name, bool includeDeleted = false)
        {
            return await _mEmployeeRepository.GetEmployeeByNameAsync(name, includeDeleted);
        }

        public async Task<IReadOnlyList<MEmployee>> GetEmployeesBySectionAsync(EmployeeSections section, bool includeDeleted = false)
        {
            return await _mEmployeeRepository.GetEmployeesBySectionAsync(section, includeDeleted);
        }

        public async Task<IReadOnlyList<MEmployee>> GetEmployeesByLevelAsync(EmployeeLevels level, bool includeDeleted = false)
        {
            return await _mEmployeeRepository.GetEmployeesByLevelAsync(level, includeDeleted);
        }
    }
}
