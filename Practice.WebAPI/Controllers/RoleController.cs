using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Practice.Domain.Role;
using Practice.WebAPI.Models;

namespace Practice.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleService _roleService;
        public RoleController(UserManager<IdentityUser> userManager,RoleService roleService)
        {
            _userManager = userManager;
            _roleService = roleService;
        }

        [Authorize]
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRoles();
                if (roles.Count() > 0)
                {
                    return Ok(roles);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role Can not Found!" });
                }
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        [Authorize (Roles = "Admin")]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromForm]string role)
        {
            try
            {
                var roleExists = await _roleService.RoleExists(role);
                if (roleExists)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role Already have Exists!" });
                }
                else 
                {
                    var resultRole = await _roleService.CreateRole(role);
                    if (resultRole.Succeeded)
                    {
                        return Ok(new Response { Status = "Success", Message = "Role Created Successfully." });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Something Want Wrong!" });
                    }
                }
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
    }
}
