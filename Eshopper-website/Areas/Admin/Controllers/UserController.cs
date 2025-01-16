using Eshopper_website.Areas.Admin.DTOs.request;
using Eshopper_website.Areas.Admin.Repository;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Services.Recaptcha;
using Eshopper_website.Utils.Enum;
using Eshopper_website.Utils.Enum.Member;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
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
        private readonly EShopperContext _context;
		private readonly Appsettings _appsettings;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        //private readonly IRecaptchaService _recaptchaService;
        public UserController(
            EShopperContext context, IOptions<Appsettings> options, 
            IEmailSender emailSender, IConfiguration configuration
            )
        {
            _context = context;
			_appsettings = options.Value;
            _emailSender = emailSender;
            _configuration = configuration;
            //_recaptchaService = recaptchaService; IRecaptchaService recaptchaService
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login([FromQuery] string? url)
        {
            // Check cookies
            var login = Request.Cookies.Get<LoginDTO>("UserCredential");
            if (login != null)
            {
                var account = _context.Accounts.AsNoTracking()
                                .FirstOrDefault(x => x.ACC_Username == login.UserName &&
                                    x.ACC_Password == login.Password);

                if (account != null && account.ACC_Status == AccountStatusEnum.Active)
                {
                    var member = _context.Members.AsNoTracking().Where(x => x.ACC_ID == account.ACC_ID).FirstOrDefault();

                    if (member != null)
                    {
                        var user = new UserInfo(account, member.ACR_ID, member.MEM_ID);

                        if (login.RememberMe)
                        {
                            Response.Cookies.Append<LoginDTO>("UserCredential", login, new CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddDays(7),
                                HttpOnly = true,
                                IsEssential = true
                            });
                        }

                        // ============== generate token ===========================
                        List<Claim> claimData = new List<Claim>
                        {
                            new Claim(ClaimTypes.Sid, account.ACC_ID.ToString()),
                            new Claim(ClaimTypes.Name, account.ACC_Username),
                            new Claim(ClaimTypes.Role, member.ACR_ID.ToString()),
                            new Claim(ClaimTypes.Email, account.ACC_Email),
                            new Claim(ClaimTypes.MobilePhone, account.ACC_Phone),
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
                        // ============== /generate token ===========================

                        if (token != null)
                        {
                            Response.Cookies.Append<String>("UserToken", tokenStr, new CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddDays(7),
                                HttpOnly = true,
                                IsEssential = true
                            });

                            var newStatusLogin = new AccountStatusLogin
                            {
                                ACC_ID = account.ACC_ID,
                                ACSL_JwtToken = tokenStr,
                                ACSL_Status = AccountStatusLoginEnum.Active,
                                ACSL_DatetimeLogin = DateTime.Now,
                                ACSL_ExpiredDatetimeLogin = DateTime.Now.AddDays(7),
                                CreatedDate = DateTime.Now,
                            };

                            _context.AccountStatusLogins.Add(newStatusLogin);
                            _context.SaveChangesAsync();
                        }

                        HttpContext.Session.Set<UserInfo>("userInfo", user);
                    }
                }
            }
            ViewBag.referer = url;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateRecaptcha]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            if (!String.IsNullOrEmpty(login.UserName) && !String.IsNullOrEmpty(login.Password))
            {
                //var reCaptchaToken = Request.Form["g-recaptcha-response"].ToString();
                //var reCaptchaValid = await _recaptchaService.VerifyToken(reCaptchaToken);
                //if (!reCaptchaValid)
                //{
                //    ViewBag.ReCaptchaError = string.IsNullOrEmpty(_recaptchaService.LastError)
                //        ? "Vui lòng xác nhận bạn không phải là robot"
                //        : _recaptchaService.LastError;
                //    return View(login);
                //}

                if (login.UserName.Contains(" ") || login.Password.Contains(" "))
                {
                    ViewData["Message"] = "Username or password should not contain blank spaces!";
                    return View(login);
                }

                var account = await _context.Accounts.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ACC_Username == login.UserName);

                if (account != null &&
                    BCrypt.Net.BCrypt.Verify(login.Password, account.ACC_Password) 
                    && account.ACC_Status == AccountStatusEnum.Active)
                {
                    var member = await _context.Members.AsNoTracking().Where(x => x.ACC_ID == account.ACC_ID).FirstOrDefaultAsync();

                    if (member != null)
                    {
                        var user = new UserInfo(account, member.ACR_ID, member.MEM_ID);

                        if (login.RememberMe)
                        {
                            Response.Cookies.Append<LoginDTO>("UserCredential", login, new CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddDays(7),
                                HttpOnly = true,
                                IsEssential = true
                            });
                        }

                        // ============== generate token ===========================
                        List<Claim> claimData = new List<Claim>
                        {
                            new Claim(ClaimTypes.Sid, account.ACC_ID.ToString()),
                            new Claim(ClaimTypes.Name, account.ACC_Username),
                            new Claim(ClaimTypes.Role, member.ACR_ID.ToString()),
                            new Claim(ClaimTypes.Email, account.ACC_Email),
                            new Claim(ClaimTypes.MobilePhone, account.ACC_Phone),
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
                        // ============== /generate token ===========================

                        if (token != null)
                        {
                            Response.Cookies.Append<String>("UserToken", tokenStr, new CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddDays(7),
                                HttpOnly = true,
                                IsEssential = true
                            });

                            var newStatusLogin = new AccountStatusLogin
                            {
                                ACC_ID = account.ACC_ID,
                                ACSL_JwtToken = tokenStr,
                                ACSL_Status = AccountStatusLoginEnum.Active,
                                ACSL_DatetimeLogin = DateTime.Now,
                                ACSL_ExpiredDatetimeLogin = DateTime.Now.AddDays(7),
                                CreatedDate = DateTime.Now,
                            };

                            _context.AccountStatusLogins.Add(newStatusLogin);
                            await _context.SaveChangesAsync();
                        }

                        HttpContext.Session.Set<UserInfo>("userInfo", user);

                        return RedirectToAction("Index", "Home", new {Area = ""} );
                    }
                }
                else
                {
                    ViewData["Message"] = "Wrong username or password!";
                }
            }
			else
			{
				ViewData["Message"] = "Username or password is empty!";
			}
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterDTO register)
		{
            if (ModelState.IsValid)
            {
                //if (!IsValidRecaptcha(Request.Headers["g-recaptcha-response"]))
                //{
                //    ViewData["Message"] = "ReCaptcha invalid";
                //    return View(register);
                //}

                if (register.UserName.Contains(" "))
                {
                    ViewData["Message"] = "Username should not contain blank spaces!";
                    return View(register);
                }

                var existingAccount = await _context.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.ACC_Username == register.UserName ||
                    x.ACC_Email == register.Email ||
                    x.ACC_Phone == register.Phone);

                if (existingAccount != null)
                {
                    if (existingAccount.ACC_Username == register.UserName)
                        ViewData["Message"] = "Username already exists!";
                    else if (existingAccount.ACC_Email == register.Email)
                        ViewData["Message"] = "Email already exists!";
                    else if (existingAccount.ACC_Phone == register.Phone)
                        ViewData["Message"] = "Phone number already exists!";

                    return View(register);
                }

                if (register.Password != register.ConfirmedPassword)
                {
                    ViewData["Message"] = "Password and Confirmed Password do not match!";
                    return View(register);
                }

                var newAccount = new Account
                {
                    ACC_Username = register.UserName,
                    ACC_Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                    ACC_Email = register.Email,
                    ACC_Phone = register.Phone,
                    ACC_DisplayName = register.DisplayName,
                    ACC_Status = AccountStatusEnum.Inactive,
                    CreatedDate = DateTime.Now,
                };
                //TempData["PendingAccount"] = newAccount;
                HttpContext.Session.Set<Account>("accountConfirmOTP", newAccount);
                // Generate OTP (6-digit number)
                var random = new Random();
                var otp = random.Next(100000, 999999).ToString();
                //TempData["OTP"] = otp;
                HttpContext.Session.Set<String>("OTPtoken", otp);

                await _emailSender.SendEmailAsync(newAccount.ACC_Email, 
                    "OTP TOKEN TO AUTHORIZE",
                    $@"Token have been sent to your email: {newAccount.ACC_Email}. Your OTP token: {otp}"
                );

                //_context.Accounts.Add(newAccount);
                //await _context.SaveChangesAsync();

                ViewData["Message"] = "Create new account successful! You must be confirm otp code to finish.";
                return RedirectToAction("ConfirmOTP");
            }
            else
            {
                ViewData["Message"] = "Missing registration fields!";
            }

            return View(register);
        }

        public async Task<IActionResult> Logout()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Append<String>(cookie, "", new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(-1),
                    HttpOnly = true,
                    IsEssential = true
                });
            }
            await HttpContext.SignOutAsync();

            HttpContext.Response.Clear();
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("userInfo");
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public async Task LoginByGoogle()
        {
            // Use Google authentication scheme for challenge
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return RedirectToAction("Login", "User", new {Area = "Admin"});
            }

            var claims = result?.Principal?.Identities?.FirstOrDefault()?.Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            var loginProvider = result?.Principal?.Identity?.AuthenticationType;

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var providerDisplayName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var providerKey = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var existingUser = await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.ACC_Email == email);

            if (existingUser == null || existingUser?.ACC_Status == AccountStatusEnum.Inactive)
            {
                ViewData["Message"] = "Account not found!";
				return RedirectToAction("Login", "User", new { Area = "Admin" });
			}

			var member = await _context.Members.AsNoTracking().Where(x => x.ACC_ID == existingUser!.ACC_ID).FirstOrDefaultAsync();

			if (member == null && member?.MEM_Status == MemberStatusEnum.Inactive)
			{
				ViewData["Message"] = "Member not found!";
				return RedirectToAction("Login", "User", new { Area = "Admin" });
			}

			var user = new UserInfo(existingUser, member!.ACR_ID, member!.MEM_ID);

			var existingAccountLogin = await _context.AccountLogins.AsNoTracking()
			.FirstOrDefaultAsync(x =>
				x.ProviderKey == providerKey);

			if (existingAccountLogin != null)
			{
                HttpContext.Session.Set<UserInfo>("userInfo", user);
			    TempData["success"] = "Login successfully.";
			}
			else
			{
				var newAccountLogin = new AccountLogin()
				{
					ACC_ID = existingUser!.ACC_ID,
					LoginProvider = loginProvider ?? "",
					ProviderKey = providerKey ?? "",
					ProviderDisplayName = providerDisplayName ?? "",
					CreatedBy = existingUser.ACC_Username,
					CreatedDate = DateTime.Now,
					UpdatedBy = existingUser.ACC_Username,
					UpdatedDate = DateTime.Now,
				};

				HttpContext.Session.Set<UserInfo>("userInfo", user);

				_context.AccountLogins.Add(newAccountLogin);
				await _context.SaveChangesAsync();

				TempData["success"] = "Login successfully.";
			}

			return RedirectToAction("Index", "Home", new { Area = "" });
		}

        public IActionResult ConfirmOTP()
        {
            var pendingAccount = HttpContext.Session.Get<Account>("accountConfirmOTP");

            //var pendingAccount = TempData["PendingAccount"] as Account;

            if (pendingAccount == null)
            {
                return RedirectToAction("Register");
            }

            ViewData["EmailReiceive"] = pendingAccount.ACC_Email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOTP(string otp)
        {
            if (string.IsNullOrEmpty(otp) || otp.Length != 6 || !otp.All(char.IsDigit))
            {
                ViewData["Message"] = "OTP must be 6 digits.";
                return View();
            }

            var storedOTP = HttpContext.Session.Get<String>("OTPtoken");
            var pendingAccount = HttpContext.Session.Get<Account>("accountConfirmOTP");

            if (storedOTP == null || pendingAccount == null)
            {
                return RedirectToAction("Register");
            }

            if (otp == storedOTP)
            {
                // Create account
                //pendingAccount!.ACC_Status = AccountStatusEnum.Active;

                _context.Accounts.Add(pendingAccount);
                await _context.SaveChangesAsync();

                var newAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.ACC_ID == pendingAccount.ACC_ID);
                // Create member
                var newMember = new Member
                {
                    ACC_ID = newAccount!.ACC_ID,
                    ACR_ID = 1,
                    MEM_Email = newAccount.ACC_Email,
                    MEM_Phone = newAccount.ACC_Phone,
                    MEM_Gender = MemberGenderEnum.Other,
                    CreatedDate = DateTime.Now,
                    MEM_Status = MemberStatusEnum.Active,
                };
                _context.Members.Add(newMember);
                await _context.SaveChangesAsync();

                // Update account status to active
                newAccount.ACC_Status = AccountStatusEnum.Active;
                _context.Accounts.Update(pendingAccount);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("OTPtoken");
                HttpContext.Session.Remove("accountConfirmOTP");

                TempData["success"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login");
            }

            ViewData["Message"] = "Invalid OTP. Please try again.";
            return View();
        }

    }
}
