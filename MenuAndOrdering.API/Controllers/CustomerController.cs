using MenuAndOrdering.API.Application.DTOs;
using MenuAndOrdering.API.Application.Services;
using MenuAndOrdering.API.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenuAndOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderingService _orderingService;

        public CustomerController(ICustomerService customerService, IOrderingService orderingService)
        {
            _customerService = customerService;
            _orderingService = orderingService;
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid id, [FromQuery] bool includeDeleted = false)
        {
            Customer? customer = await _customerService.GetCustomerByIdAsync(id, includeDeleted);

            if (customer == null)
            {
                return NotFound();
            }
            CustomerDto result = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                SubmitDate = customer.SubmitDate
            };

            return Ok(result);
        }


        [HttpGet("by-phone/{phoneNumber}")]
        public async Task<ActionResult<CustomerDto>> GetByPhoneNumber(string phoneNumber, [FromQuery] string? name = null)
        {
            Customer? customer = await _customerService.GetCustomerByPhoneNumberAsync(phoneNumber);

            if (customer == null)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    customer = await _customerService.CreateCustomerAsync(name, phoneNumber);
                }
                else
                {
                    return NotFound("مشتری پیدا نشد. نام را وارد کنید.");
                }
            }

            CustomerDto result = new CustomerDto()
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                SubmitDate = customer.SubmitDate
            };

            return Ok(result);

        }


        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            Customer customer = await _customerService.CreateCustomerAsync(request.Name, request.PhoneNumber);

            //if (!string.IsNullOrWhiteSpace(request.Password))
            //{
            //    customer = await _customerService.CreateCustomerWithPasswordAsync(request.Name, request.PhoneNumber, request.Password);
            //}
            //else
            //{
            //    customer = await _customerService.CreateCustomerAsync(request.Name, request.PhoneNumber);
            //}

            CustomerDto result = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                SubmitDate = customer.SubmitDate
            };

            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, result);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest request)
        {
            Customer? customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                customer.UpdateName(request.Name);
            }
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                customer.SetPassword(request.Password);
            }
            await _customerService.UpdateCustomerAsync(customer);

            CustomerDto result = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                SubmitDate = customer.SubmitDate
            };

            return Ok(result);

        }

    }
}
