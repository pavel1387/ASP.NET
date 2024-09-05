using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync(CancellationToken cancellationToken = default)
        {
            var employees = await _employeeRepository.GetAllAsync(cancellationToken);

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать нового сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync(EmployeeCreateDTO employeeCreateDTO, CancellationToken cancellationToken = default)
        {
            var newEntity = new Employee()
            {
                FirstName = employeeCreateDTO.FirstName,
                LastName = employeeCreateDTO.LastName,
                Email = employeeCreateDTO.Email,
                AppliedPromocodesCount = employeeCreateDTO.AppliedPromocodesCount,
                Roles = new List<Role>(),
            };

            var result = await _employeeRepository.CreateAsync(newEntity, cancellationToken);

            var employeeResponse = new EmployeeResponse()
            {
                Id = result.Id,
                Email = result.Email,
                Roles = result.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = result.FullName,
                AppliedPromocodesCount = result.AppliedPromocodesCount
            };

            return employeeResponse;
        }


        /// <summary>
        /// Обновить сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployeeAsync(EmployeeUpdateDTO employeeUpdateDTO, CancellationToken cancellationToken = default)
        {
            var entity = await _employeeRepository.GetByIdAsync(employeeUpdateDTO.Id, cancellationToken);

            if (entity == null)
                return NotFound();

            entity.Email = employeeUpdateDTO.Email;
            entity.FirstName = employeeUpdateDTO.FirstName;
            entity.LastName = employeeUpdateDTO.LastName;
            entity.AppliedPromocodesCount = employeeUpdateDTO.AppliedPromocodesCount;

            var result = await _employeeRepository.UpdateAsync(entity, cancellationToken);

            var employeeResponse = new EmployeeResponse()
            {
                Id = result.Id,
                Email = result.Email,
                Roles = result.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = result.FullName,
                AppliedPromocodesCount = result.AppliedPromocodesCount
            };

            return employeeResponse;
        }


        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpGet("delete/{id}")]
        public async Task<ActionResult> DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _employeeRepository.GetByIdAsync(id, cancellationToken);

            if (entity == null)
                return NotFound();

            var deleted = await _employeeRepository.DeleteAsync(id, cancellationToken);
            if (deleted)
                return Ok("Сотрудник удален");

            return BadRequest();
        }

    }
}