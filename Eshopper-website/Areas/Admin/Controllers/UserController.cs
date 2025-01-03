﻿using Eshopper_website.Areas.Admin.DTOs.request;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum;
using Eshopper_website.Utils.Enum.Member;
using Eshopper_website.Utils.Extension;
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
		public UserController(EShopperContext context, IOptions<Appsettings> options)
        {
            _context = context;
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
                if (result != null && result.ACC_Status == AccountStatusEnum.Active)
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
            if (!String.IsNullOrEmpty(login.UserName) && !String.IsNullOrEmpty(login.Password))
            {
                var account = await _context.Accounts.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ACC_Username == login.UserName &&
                        x.ACC_Password == login.Password);

                ViewData["Message"] = "Get account successful";

                if (account != null && account.ACC_Status == AccountStatusEnum.Active)
                {
                    var member = await _context.Members.AsNoTracking().Where(x => x.ACC_ID == account.ACC_ID).FirstOrDefaultAsync();

                    if (member != null)
                    {
                        var user = new UserInfo(account, member.ACR_ID);

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

                        return RedirectToAction("Index", "Home");
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
                if (register.UserName.Contains(" "))
                {
                    ViewData["Message"] = "Username should not contain blank spaces!";
                    return View(register);
                }

                var existingAccount = await _context.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.ACC_Username == register.UserName ||
                    x.ACC_Email == register.Email ||
                    x.ACC_Phone == register.Phone ||
                    x.ACC_DisplayName == register.DisplayName);

                if (existingAccount != null)
                {
                    if (existingAccount.ACC_Username == register.UserName)
                        ViewData["Message"] = "Username already exists!";
                    else if (existingAccount.ACC_Email == register.Email)
                        ViewData["Message"] = "Email already exists!";
                    else if (existingAccount.ACC_Phone == register.Phone)
                        ViewData["Message"] = "Phone number already exists!";
                    else if (existingAccount.ACC_DisplayName == register.DisplayName)
                        ViewData["Message"] = "Display name already exists!";

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
                    ACC_Password = register.Password,
                    ACC_Email = register.Email,
                    ACC_Phone = register.Phone,
                    ACC_DisplayName = register.DisplayName,
                    ACC_Status = AccountStatusEnum.Inactive,
                    CreatedDate = DateTime.Now
                };

                _context.Accounts.Add(newAccount);
                await _context.SaveChangesAsync();

                var newMember = new Member
                {
                    ACC_ID = newAccount.ACC_ID,
                    ACR_ID = 1,
                    MEM_Email = newAccount.ACC_Email,
                    MEM_Phone = newAccount.ACC_Phone,
                    MEM_Gender = MemberGenderEnum.Other,
                    CreatedDate = DateTime.Now,
                    MEM_Status = MemberStatusEnum.Inactive,
                };
                _context.Members.Add(newMember);
                await _context.SaveChangesAsync();

                ViewData["Message"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewData["Message"] = "Missing registration fields!";
            }

            return View(register);
        }

    }
}
