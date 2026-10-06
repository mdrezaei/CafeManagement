using CafeManagement.API.Application.DTOs;
using CafeManagement.API.Application.Services;
using CafeManagement.API.Domain.Entities;
using CafeManagement.API.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.API.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class MEmployeeController : ControllerBase
    {
        private readonly IMEmployeeService _mEmployeeService;

        public MEmployeeController(IMEmployeeService mEmployeeService)
        {
            _mEmployeeService = mEmployeeService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MEmployeeDto>>> GetAllEmployees([FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MEmployee> employees = await _mEmployeeService.GetAllEmployeesAsync(includeDeleted);

            IReadOnlyList<MEmployeeDto> result = employees.Select(e => new MEmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                PhoneNumber = e.PhoneNumber,
                JoinedDate = e.JoinedDate,
                Section = e.Section.ToString(),
                Level = e.Level.ToString(),
                SecondarySection = e.SecondarySection.ToString(),
                SecondaryLevel = e.SecondaryLevel.ToString(),
                Description = e.Description
            }).ToList();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MEmployeeDto>> GetEmployeeById(Guid id, [FromQuery] bool includeDeleted = false)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByIdAsync(id, includeDeleted);

            if (employee == null)
            {
                return NotFound();
            }
            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return Ok(result);
        }

        [Authorize]
        [HttpGet("by-phone/{phoneNumber}")]
        public async Task<ActionResult<MEmployeeDto>> GetEmployeeByPhoneNumber(string phoneNumber, [FromQuery] bool includeDeleted = false)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByPhoneNumberAsync(phoneNumber, includeDeleted);

            if (employee == null)
            {
                return NotFound();
            }
            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return Ok(result);
        }

        [Authorize]
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<IReadOnlyList<MEmployeeDto>>> GetEmployeesByName(string name, [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MEmployee> employees = await _mEmployeeService.GetEmployeeByNameAsync(name, includeDeleted);

            IReadOnlyList<MEmployeeDto> result = employees.Select(e => new MEmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                PhoneNumber = e.PhoneNumber,
                JoinedDate = e.JoinedDate,
                Section = e.Section.ToString(),
                Level = e.Level.ToString(),
                SecondarySection = e.SecondarySection.ToString(),
                SecondaryLevel = e.SecondaryLevel.ToString(),
                Description = e.Description
            }).ToList();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("by-date")]
        public async Task<ActionResult<IReadOnlyList<MEmployeeDto>>> GetEmployeesByDate([FromQuery] DateTime from, [FromQuery] DateTime? to = null, [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MEmployee> employees = await _mEmployeeService.GetEmployeesByDateAsync(from, to, includeDeleted);

            IReadOnlyList<MEmployeeDto> result = employees.Select(e => new MEmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                PhoneNumber = e.PhoneNumber,
                JoinedDate = e.JoinedDate,
                Section = e.Section.ToString(),
                Level = e.Level.ToString(),
                SecondarySection = e.SecondarySection.ToString(),
                SecondaryLevel = e.SecondaryLevel.ToString(),
                Description = e.Description
            }).ToList();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("section/{section}")]
        public async Task<ActionResult<IReadOnlyList<MEmployeeDto>>> GetEmployeesBySection(string section, [FromQuery] bool includeDeleted = false)
        {
            if (!Enum.TryParse<EmployeeSections>(section, true, out var employeeSection))
            {
                return BadRequest("بخش نامعتبر است.");
            }
            IReadOnlyList<MEmployee> employees = await _mEmployeeService.GetEmployeesBySectionAsync(employeeSection, includeDeleted);

            IReadOnlyList<MEmployeeDto> result = employees.Select(e => new MEmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                PhoneNumber = e.PhoneNumber,
                JoinedDate = e.JoinedDate,
                Section = e.Section.ToString(),
                Level = e.Level.ToString(),
                SecondarySection = e.SecondarySection.ToString(),
                SecondaryLevel = e.SecondaryLevel.ToString(),
                Description = e.Description
            }).ToList();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("level/{level}")]
        public async Task<ActionResult<IReadOnlyList<MEmployeeDto>>> GetEmployeesByLevel(string level, [FromQuery] bool includeDeleted = false)
        {
            if (!Enum.TryParse<EmployeeLevels>(level, true, out var employeeLevel))
            {
                return BadRequest("سطح نامعتبر است.");
            }
            IReadOnlyList<MEmployee> employees = await _mEmployeeService.GetEmployeesByLevelAsync(employeeLevel, includeDeleted);

            IReadOnlyList<MEmployeeDto> result = employees.Select(e => new MEmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                PhoneNumber = e.PhoneNumber,
                JoinedDate = e.JoinedDate,
                Section = e.Section.ToString(),
                Level = e.Level.ToString(),
                SecondarySection = e.SecondarySection.ToString(),
                SecondaryLevel = e.SecondaryLevel.ToString(),
                Description = e.Description
            }).ToList();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<MEmployeeDto>> CreateEmployee([FromBody] CreateEmployeeRequest request)
        {
            if (!Enum.TryParse<EmployeeSections>(request.Section, true, out var section))
            {
                return BadRequest("بخش نامعتبر است.");
            }
            if (!Enum.TryParse<EmployeeLevels>(request.Level, true, out var level))
            {
                return BadRequest("سطح نامعتبر است.");
            }
            MEmployee employee = await _mEmployeeService.CreateEmployeeAsync(
                request.Name,
                request.Password,
                request.PhoneNumber,
                section,
                level
            );

            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, result);
        }

        [Authorize]
        [HttpPatch("{id:guid}/update-password")]
        public async Task<ActionResult<MEmployeeDto>> UpdatePassword(Guid id, [FromBody] SetPasswordRequest request)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee = await _mEmployeeService.UpdateEmployeePasswordAsync(employee, request.Password);

            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<MEmployeeDto>> PatchEmployee(Guid id, [FromBody] UpdateEmployeeRequest request)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                employee = await _mEmployeeService.UpdateEmployeeNameAsync(employee, request.Name);
            }
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                employee = await _mEmployeeService.UpdateEmployeePasswordAsync(employee, request.Password);
            }
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                employee = await _mEmployeeService.UpdateEmployeePhoneNumberAsync(employee, request.PhoneNumber);
            }
            if (request.JoinedDate.HasValue)
            {
                employee = await _mEmployeeService.UpdateEmployeeJoinedDateAsync(employee, request.JoinedDate.Value);
            }
            if (!string.IsNullOrWhiteSpace(request.Section))
            {
                if (!Enum.TryParse<EmployeeSections>(request.Section, true, out var section))
                {
                    return BadRequest("بخش نامعتبر است.");
                }
                employee = await _mEmployeeService.UpdateEmployeeSectionAsync(employee, section);
            }

            if (!string.IsNullOrWhiteSpace(request.Level))
            {
                if (!Enum.TryParse<EmployeeLevels>(request.Level, true, out var level))
                {
                    return BadRequest("سطح نامعتبر است.");
                }
                employee = await _mEmployeeService.UpdateEmployeeLevelAsync(employee, level);
            }

            if (!string.IsNullOrWhiteSpace(request.SecondarySection))
            {
                if (!Enum.TryParse<EmployeeSections>(request.SecondarySection, true, out var secondarySection))
                {
                    return BadRequest("بخش دوم نامعتبر است.");
                }
                employee = await _mEmployeeService.UpdateEmployeeSecondarySectionAsync(employee, secondarySection);
            }

            if (!string.IsNullOrWhiteSpace(request.SecondaryLevel))
            {
                if (!Enum.TryParse<EmployeeLevels>(request.SecondaryLevel, true, out var secondaryLevel))
                {
                    return BadRequest("سطح دوم نامعتبر است.");
                }
                employee = await _mEmployeeService.UpdateEmployeeSecondaryLevelAsync(employee, secondaryLevel);
            }

            if (request.Description != null)
            {
                employee = await _mEmployeeService.UpdateEmployeeDescriptionAsync(employee, request.Description);
            }
            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("{id:guid}/secondary-section")]
        public async Task<ActionResult<MEmployeeDto>> SetSecondarySection(Guid id, [FromBody] SetSectionRequest request)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            if (!Enum.TryParse<EmployeeSections>(request.Section, true, out var section))
            {
                return BadRequest("بخش نامعتبر است.");
            }
            employee = await _mEmployeeService.UpdateEmployeeSecondarySectionAsync(employee, section);

            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id:guid}/secondary-section")]
        public async Task<ActionResult<MEmployeeDto>> DeleteSecondarySection(Guid id)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee = await _mEmployeeService.DeleteEmployeeSecondarySectionAsync(employee);

            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("{id:guid}/secondary-level")]
        public async Task<ActionResult<MEmployeeDto>> SetSecondaryLevel(Guid id, [FromBody] SetLevelRequest request)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            if (!Enum.TryParse<EmployeeLevels>(request.Level, true, out var level))
            {
                return BadRequest("سطح نامعتبر است.");
            }
            employee = await _mEmployeeService.UpdateEmployeeSecondaryLevelAsync(employee, level);

            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id:guid}/secondary-level")]
        public async Task<ActionResult<MEmployeeDto>> DeleteSecondaryLevel(Guid id)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee = await _mEmployeeService.DeleteEmployeeSecondaryLevelAsync(employee);

            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("{id:guid}/description")]
        public async Task<ActionResult<MEmployeeDto>> SetDescription(Guid id, [FromBody] SetDescriptionRequest request)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee = await _mEmployeeService.UpdateEmployeeDescriptionAsync(employee, request.Description ?? string.Empty);

            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id:guid}/description")]
        public async Task<ActionResult<MEmployeeDto>> DeleteDescription(Guid id)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee = await _mEmployeeService.DeleteEmployeeDescriptionAsync(employee);

            MEmployeeDto result = new MEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                JoinedDate = employee.JoinedDate,
                Section = employee.Section.ToString(),
                Level = employee.Level.ToString(),
                SecondarySection = employee.SecondarySection.ToString(),
                SecondaryLevel = employee.SecondaryLevel.ToString(),
                Description = employee.Description
            };

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> SoftDeleteEmployee(Guid id)
        {
            MEmployee? employee = await _mEmployeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            await _mEmployeeService.SoftDeleteEmployeeAsync(employee);
            return NoContent();
        }
    }
}
