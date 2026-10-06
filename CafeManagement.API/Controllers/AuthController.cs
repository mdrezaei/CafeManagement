using CafeManagement.API.Application.DTOs;
using CafeManagement.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMCustomerService _mCustomerService;
        private readonly IMEmployeeService _mEmployeeService;
        private readonly JwtService _jwtService;

        public AuthController(IMCustomerService customerService, IMEmployeeService employeeService, JwtService jwtService)
        {
            _mCustomerService = customerService;
            _mEmployeeService = employeeService;
            _jwtService = jwtService;
        }

        // ── ثبت‌نام مشتری ─────────────────────────
        [HttpPost("customer/register")]
        public async Task<IActionResult> CustomerRegister([FromBody] CustomerRegisterRequest request)
        {
            var customer = await _mCustomerService.CreateCustomerWithPasswordAsync(
                request.Name,
                request.PhoneNumber,
                request.Password
            );

            return Ok(new { customerId = customer.Id });
        }

        // ── ورود مشتری ───────────────────────────
        [HttpPost("customer/login")]
        public async Task<IActionResult> CustomerLogin([FromBody] LoginRequestDto request)
        {
            var customer = await _mCustomerService.GetCustomerByPhoneNumberAsync(request.PhoneNumber);

            if (customer == null || customer.Password != request.Password)
                return Unauthorized("شماره تلفن یا رمز عبور اشتباه است.");

            var token = _jwtService.GenerateToken(
                customer.Id,
                customer.PhoneNumber,
                "Customer"
            );

            return Ok(new { token });
        }

        // ── ورود کارمند ───────────────────────────
        [HttpPost("employee/login")]
        public async Task<IActionResult> EmployeeLogin([FromBody] LoginRequestDto request)
        {
            var employee = await _mEmployeeService.GetEmployeeByPhoneNumberAsync(request.PhoneNumber);

            if (employee == null || employee.Password != request.Password)
                return Unauthorized("شماره تلفن یا رمز عبور اشتباه است.");

            var token = _jwtService.GenerateToken(
                employee.Id,
                employee.PhoneNumber,
                employee.Section.ToString()
            );

            return Ok(new { token });
        }
    }
}
