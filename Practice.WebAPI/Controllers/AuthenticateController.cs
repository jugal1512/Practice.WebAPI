using AutoMapper;
using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Practice.Domain.RegisterUser;
using Practice.Domain.Role;
using Practice.WebAPI.Models;
using Practice.WebAPI.Utility;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Practice.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly RegisterUserService _registerUserService;
        private readonly RoleService _roleService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ISendEmail _sendEmail;
        private static Dictionary<string, (string otp,DateTime Expiration)> _otpStore = new();
        public AuthenticateController(RegisterUserService registerUserService,IMapper mapper,RoleService roleService,UserManager<IdentityUser> userManager,IConfiguration configuration,ISendEmail sendEmail)
        {
            _registerUserService = registerUserService;
            _mapper = mapper;
            _roleService = roleService;
            _userManager = userManager;
            _configuration = configuration;
            _sendEmail = sendEmail;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerModelDto)
        {
            try
            {
                var registerUser = _mapper.Map<RegisterUser>(registerModelDto);
                var userExists = await _registerUserService.UserExists(registerUser.UserName);
                if (userExists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Already Exists." });
                }
                var createUserResult = await _registerUserService.RegisterUser(registerUser);
                if (!createUserResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
                }
                else
                {
                    var roleExist = await _roleService.RoleExists(registerUser.RoleName);
                    if (!roleExist)
                    {
                        var resultRole = await _roleService.CreateRole(registerUser.RoleName);
                        if (resultRole.Succeeded)
                        {
                            return Ok(new Response { Status = "Success", Message = "Role Created Successfully." });
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Something Want Wrong!" });
                        }
                    }
                    else
                    {
                        var addToRole = await _registerUserService.AddToRole(registerUser, registerUser.RoleName);
                        if (!addToRole.Succeeded)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role can not be Added!" });
                        }
                    }
                    return Ok(new Response { Status = "Success", Message = "User Register Successfully." });
                }
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDto userDetail)
        {
            try
            {
                var loginUser = _mapper.Map<Login>(userDetail);
                var user = await _registerUserService.UserExists(loginUser.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
                {
                    if (user.TwoFactorEnabled)
                    {
                        var otp = GenerateOtp();
                        var toMail = await _userManager.GetEmailAsync(user);
                        var subject = "Authentication OTP";
                        var body = " Hello, " + user.UserName + "<br/><br/>" + " Your OTP : " + otp + "<br/><br/> <p style='color:red;'>This is Valid for 5 Minutes.<p>";
                        await _sendEmail.SendMail(toMail, subject, body);
                        storeOTP(user.UserName, otp);
                        return Ok(new Response { Status = "Success", Message = $"OTP Send Successfully in your Mail : {toMail}" });
                    }
                    else 
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        };

                        foreach (var userRole in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }

                        var token = GetToken(authClaims);

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Unauthorized User." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        [HttpPost("Login2-FA")]
        public async Task<IActionResult> LoginWithOTP([FromForm] OtpDto otpDto)
        {
            try
            {
                var isValidOtp = ValidateOtp(otpDto.UserName,otpDto.OTP);
                if (!isValidOtp)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Invalid OTP." });
                }
                else
                {
                    var user = await _registerUserService.UserExists(otpDto.UserName);
                    if (user != null )
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        };

                        foreach (var userRole in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }

                        var token = GetToken(authClaims);

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
            return Ok();
        }

        private string GenerateOtp()
        { 
            Random randomOtp = new Random();
            return randomOtp.Next(100000, 999999).ToString();
        }

        private void storeOTP(string userName, string otp)
        {
            //_otpStore is a static dictionary where the key is the username and the value is a tuple containing the OTP and its expiration time.ValidateOtp Method:
            var _expirationTime = DateTime.UtcNow.AddMinutes(5);
            _otpStore[userName] = (otp, _expirationTime);
        }

        private bool ValidateOtp(string userName,string otp)
        {
            if (_otpStore.TryGetValue(userName, out var storedOtpInfo))
            {
                if (DateTime.UtcNow <= storedOtpInfo.Expiration)
                {
                    if (storedOtpInfo.otp == otp)
                    {
                        _otpStore.Remove(userName);
                        return true;

                    }
                }
                _otpStore.Remove(userName);
            }
            return false;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddHours(3),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
                        );
            return token;
        }
    }
}