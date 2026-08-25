// Api/Controllers/RoleController.cs
using Application.Roles;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    // GET: api/role
    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    // GET: api/role/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        if (role == null) return NotFound();
        return Ok(role);
    }

    // GET: api/role/search?term=president
    // Called by the dashboard dropdown as the user types
    [HttpGet("search")]
    public async Task<IActionResult> SearchRoles([FromQuery] string? term)
    {
        var roles = await _roleService.SearchRolesAsync(term);
        return Ok(roles);
    }

    // POST: api/role
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] Role role)
    {
        var createdBy = User.Identity?.Name ?? "system";
        var created = await _roleService.CreateRoleAsync(role, createdBy);
        return CreatedAtAction(nameof(GetRoleById), new { id = created.Id }, created);
    }

    // PUT: api/role/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] Role role)
    {
        var updatedBy = User.Identity?.Name ?? "system";
        var updated = await _roleService.UpdateRoleAsync(id, role, updatedBy);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    // DELETE: api/role/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        var success = await _roleService.DeleteRoleAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}