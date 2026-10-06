using CafeManagement.API.Application.DTOs;
using CafeManagement.API.Application.Services;
using CafeManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.API.Controllers
{
    [Authorize]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class MCustomerController : ControllerBase
    {
        private readonly IMCustomerService _mCustomerService;

        public MCustomerController(IMCustomerService mCustomerService)
        {
            _mCustomerService = mCustomerService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MCustomerDto>>> GetAllCustomers([FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MCustomer> customers = await _mCustomerService.GetAllCustomersAsync(includeDeleted);

            IReadOnlyList<MCustomerDto> result = customers.Select(c => new MCustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber,
                SubmitDate = c.SubmitDate
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MCustomerDto>> GetCustomerById(Guid id, [FromQuery] bool includeDeleted = false)
        {
            MCustomer? customer = await _mCustomerService.GetCustomerByIdAsync(id, includeDeleted);

            if (customer == null)
            {
                return NotFound();
            }
            MCustomerDto result = new MCustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                SubmitDate = customer.SubmitDate
            };

            return Ok(result);
        }

        [HttpGet("by-phone/{phoneNumber}")]
        public async Task<ActionResult<MCustomerDto>> GetCustomerByPhoneNumber(string phoneNumber, [FromQuery] bool includeDeleted = false)
        {
            MCustomer? customer = await _mCustomerService.GetCustomerByPhoneNumberAsync(phoneNumber, includeDeleted);

            if (customer == null)
            {
                return NotFound();
            }
            MCustomerDto result = new MCustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                SubmitDate = customer.SubmitDate
            };

            return Ok(result);
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<IReadOnlyList<MCustomerDto>>> GetCustomersByName(string name, [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MCustomer> customers = await _mCustomerService.GetCustomerByNameAsync(name, includeDeleted);

            IReadOnlyList<MCustomerDto> result = customers.Select(c => new MCustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber,
                SubmitDate = c.SubmitDate
            }).ToList();

            return Ok(result);
        }

        [HttpGet("by-date")]
        public async Task<ActionResult<IReadOnlyList<MCustomerDto>>> GetCustomersByDate([FromQuery] DateTime from, [FromQuery] DateTime? to = null, [FromQuery] bool includeDeleted = false)
        {
            IReadOnlyList<MCustomer> customers = await _mCustomerService.GetCustomerByDateAsync(from, to, includeDeleted);

            IReadOnlyList<MCustomerDto> result = customers.Select(c => new MCustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber,
                SubmitDate = c.SubmitDate
            }).ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<MCustomerDto>> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            MCustomer customer;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                customer = await _mCustomerService.CreateCustomerWithPasswordAsync(request.Name, request.PhoneNumber, request.Password);
            }
            else
            {
                customer = await _mCustomerService.CreateCustomerAsync(request.Name, request.PhoneNumber);
            }

            MCustomerDto result = new MCustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                SubmitDate = customer.SubmitDate
            };

            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, result);
        }

        [HttpPatch("{id:guid}/set-password")]
        public async Task<ActionResult<MCustomerDto>> SetPassword(Guid id, [FromBody] SetPasswordRequest request)
        {
            MCustomer? customer = await _mCustomerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            customer = await _mCustomerService.SetCustomerPasswordAsync(customer, request.Password);

            MCustomerDto result = new MCustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                SubmitDate = customer.SubmitDate
            };

            return Ok(result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<MCustomerDto>> PatchCustomer(Guid id, [FromBody] UpdateCustomerRequest request)
        {
            MCustomer? customer = await _mCustomerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                customer = await _mCustomerService.UpdateCustomerNameAsync(customer, request.Name);
            }
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                customer = await _mCustomerService.UpdateCustomerPhoneNumberAsync(customer, request.PhoneNumber);
            }
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                customer = await _mCustomerService.UpdateCustomerPasswordAsync(customer, request.Password);
            }
            MCustomerDto result = new MCustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                SubmitDate = customer.SubmitDate
            };

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> SoftDeleteCustomer(Guid id)
        {
            MCustomer? customer = await _mCustomerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _mCustomerService.SoftDeleteCustomerAsync(customer);
            return NoContent();
        }
    }
}

