using Eshopper_website.Areas.Admin.DTOs.request;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum.Member;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
		//private readonly JWTExtension _jWTExtension;
        private readonly EShopperContext _context;
		private readonly Appsettings _appsettings;
		public UserController(EShopperContext context, IOptions<Appsettings> options  ) //JWTExtension jWTExtension
        {
            _context = context;
			//_jWTExtension = jWTExtension;
			_appsettings = options.Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            // Check cookies
            var login = Request.Cookies.Get<LoginDTO>("UserCredential");
            if (login != null)
            {
                var result = _context.Accounts.AsNoTracking()
                                .FirstOrDefault(x => x.ACC_Username == login.UserName &&
                                    x.ACC_Password == login.Password);
                if (result != null)
                {
                    // Set Session
                    HttpContext.Session.Set<Account>("userInfo", result);
                    // Redirect to Dashboard
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginDTO login)
        {
			var account = await _context.Accounts.AsNoTracking()
					.FirstOrDefaultAsync(x => x.ACC_Username == login.UserName &&
						x.ACC_Password == login.Password);
			//if (account != null)
			//{
			var member = await _context.Members.AsNoTracking().Where(x => x.ACC_ID == account.ACC_ID).FirstOrDefaultAsync();
			//	var user = new AccountDTO(account, member != null ? member.MEM_Status : MemberStatusEnum.Inactive);

			//	if (member != null)
			//	{
			//		if (login.RememberMe)
			//		{
			//			Response.Cookies.Append<LoginDTO>("UserCredential", login, new CookieOptions
			//			{
			//				Expires = DateTimeOffset.UtcNow.AddDays(7),
			//				HttpOnly = true, // Accessible only by the server
			//				IsEssential = true // Required for GDPR compliance
			//			});
			//		}
			//		var token = _jWTExtension.GenerateToken(user);

			//		if (token != null)
			//		{
			//			Response.Cookies.Append<String>("UserToken", token, new CookieOptions
			//			{
			//				Expires = DateTimeOffset.UtcNow.AddDays(7),
			//				HttpOnly = true,
			//				IsEssential = true
			//			});
			//		}
			//		HttpContext.Session.Set<Account>("userInfo", account);

			//		return RedirectToAction("Index", "Home");
			//	}

			//	return Unauthorized();
			//}
			//else
			//{
			//	ViewData["Message"] = "Wrong username or password!";
			//}
			//return View();
			var result = await _context.Accounts.AsNoTracking()
								.FirstOrDefaultAsync(x => x.ACC_Username == login.UserName &&
									x.ACC_Password == login.Password);
			if (result != null)
			{
				if (login.RememberMe)
				{
					Response.Cookies.Append<LoginDTO>("UserCredential", login, new CookieOptions
					{
						Expires = DateTimeOffset.UtcNow.AddDays(7),
						HttpOnly = true, // Accessible only by the server
						IsEssential = true // Required for GDPR compliance
					});
				}

				// ============== generate token ===========================
				List<Claim> claimData = new List<Claim>
				{
					new Claim(ClaimTypes.Sid, result.ACC_ID.ToString()),
					new Claim(ClaimTypes.Name, result.ACC_Username),
					new Claim(ClaimTypes.Role, member.ACR_ID.ToString()),
					new Claim(ClaimTypes.Email, result.ACC_Email),
					new Claim(ClaimTypes.MobilePhone, result.ACC_Phone),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appsettings.Key));
				var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var token = new JwtSecurityToken(
					issuer: _appsettings.Issuer,
					audience: _appsettings.Audience,
					expires: DateTime.Now.AddDays(7),
					claims: claimData,
					signingCredentials: signingCredential
				);

				var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
				Response.Cookies.Append<String>("UserToken", tokenStr, new CookieOptions
				{
					Expires = DateTimeOffset.UtcNow.AddDays(7),
					HttpOnly = true,
					IsEssential = true
				});
				// ============== /generate token ===========================
				//Set Session

				HttpContext.Session.Set<Account>("userInfo", result);
				//Redirect to Dashboard

				return RedirectToAction("Index", "Home");
			}
			else
			{
				ViewData["Message"] = "Wrong username or password!";
			}
			return View();
		}

	}
}
