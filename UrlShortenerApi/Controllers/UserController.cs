using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortenerApi.Dto;
using UrlShortenerApi.Entity.DBEntities;
using UrlShortenerApi.Service.Services;
using UrlShortenerApi.Util;

namespace UrlShortenerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly IConfiguration _config;
        protected readonly UserService _service;
        protected readonly IMapper _mapper;
        protected readonly IMailer _mailer;

        public UserController(IConfiguration config, UserService service, IMapper mapper, IMailer mailer)
        {
            _config = config;
            _service = service;
            _mapper = mapper;
            _mailer = mailer;
        }

        [HttpGet("{username}")]
        [Authorize]
        public ActionResult Get(string username) {

            try
            {
                var cs = _config["CS"];
                var entity = _service.GetById(cs, username);

                if(entity == null)
                    return NotFound();

                var dto = _mapper.Map<UserViewDto>(entity);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) { 
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("SignIn")]
        public ActionResult SignIn(UserLoginDto dto)
        {

            try
            {
                var cs = _config["CS"];
                var entity = _mapper.Map<UserEntity>(dto);

                var username = _service.Login(cs, dto.username, dto.pwd);

                if(username == null)
                    return NotFound();

                var token = GetToken(username);

                return Ok(new { token, username });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("SignUp")]
        public ActionResult SignUp(UserRegisterDto dto)
        {

            try
            {
                var cs = _config["CS"];
                var entity = _mapper.Map<UserEntity>(dto);

                _service.Register(cs, entity);

                var tokenEmailVerification = GetEmailVerificationToken(entity.username);
                _mailer.SendEmailAsync(entity.email, "Account Verification", $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Verify Your Account</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            background-color: #ffffff;
            border: 1px solid #ddd;
            border-radius: 5px;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            color: #ffffff;
            background-color: #007bff;
            border: none;
            border-radius: 5px;
            text-decoration: none;
            font-size: 16px;
        }}
        .footer {{
            text-align: center;
            font-size: 12px;
            color: #888888;
            margin-top: 20px;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h2 style=""color: #333;"">Hi {entity.first_name} {entity.last_name}!</h2>
        <p>Thanks for signing up with <strong>Url Shortener</strong>! We're excited to have you on board.</p>
        <p>To complete your registration, please verify your email address by clicking the link below:</p>
        <p style=""text-align: center;"">
            <a href=""http://localhost:4200/verification/{tokenEmailVerification}"" class=""button"">Verify My Account</a>
        </p>
        <p>If the button doesn't work, you can copy and paste the following URL into your browser:</p>
        <p style=""word-break: break-all; color: #007bff;"">http://localhost:4200/verification/{tokenEmailVerification}</p>
        <p>If you didn't sign up for an account, please ignore this email.</p>
        <p>Thanks for joining us!</p>
        <p>Best regards,<br>Url Shortener</p>
    </div>
    <div class=""footer"">
        &copy; {DateTime.Now.ToString("yyyy")} Url Shortener. All rights reserved.
    </div>
</body>
</html>");
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("ActivateUser")]
        public ActionResult ActivateUser(UserActivateDto dto)
        {

            try
            {
                var cs = _config["CS"];

                var username = GetEmailByVerificationToken(dto.Code);

                _service.Activate(cs, username);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private string GetToken(string username)
        {
            var llave = _config["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(llave));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var listClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("User", username),
            };

            var token = new JwtSecurityToken(claims: listClaims, expires: DateTime.Now.AddHours(24), signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodeToken;
        }

        private string GetEmailVerificationToken(string username)
        {
            var llave = _config["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(llave));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var listClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Username", username),
            };

            var token = new JwtSecurityToken(claims: listClaims, expires: DateTime.Now.AddHours(24), signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodeToken;
        }

        private string GetEmailByVerificationToken(string token)
        {
            var jwt = new JwtSecurityToken(token);
            return jwt.Claims.First(x => x.Type == "Username").Value;
        }
    }
}
