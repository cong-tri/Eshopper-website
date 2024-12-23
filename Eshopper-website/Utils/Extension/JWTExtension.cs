using Eshopper_website.Areas.Admin.DTOs.request;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eshopper_website.Utils.Extension
{
	public class JWTExtension
	{
		private readonly IConfiguration _configuration;

		public JWTExtension(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateToken(AccountDTO request)
		{
            if (request != null)
            {
				var jwtSettings = _configuration.GetSection("JwtSettings");
				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
				var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var claims = new[]
				{
				new Claim(JwtRegisteredClaimNames.Sub, request.ACC_ID.ToString()),
				new Claim(ClaimTypes.Name, request.ACC_Username),
				new Claim(ClaimTypes.Role, request.MEM_Status.ToString()),
				new Claim(ClaimTypes.Email, request.ACC_Email),
				new Claim(ClaimTypes.MobilePhone, request.ACC_Phone),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};

				var token = new JwtSecurityToken(
					issuer: jwtSettings["Issuer"],
					audience: jwtSettings["Audience"],
					claims: claims,
					expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireDates"])),
					signingCredentials: credentials
				);

				return new JwtSecurityTokenHandler().WriteToken(token);
            }
			return "";
		}
	}
}
