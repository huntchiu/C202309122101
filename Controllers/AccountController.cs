using C202309122101.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace C202309122101.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel register)
    {
        if (!ModelState.IsValid) return View();

        // Validate Email Address (optional)
        // 驗證Email是可選的， 因為我們已將身份配置為始終需要唯一的電子郵件地址。

        // Create the user
        var user = new IdentityUser
        {
            Email = register.Email,
            UserName = register.Email, // UserName 是必須填寫的字段
        };
        //PasswordSignInAsync 会为成功登录的用户创建一个安全上下文，
        //並设置必要的认证 cookie，这样在后续请求中，系统能够识别用户并提供其相关的权限和身份信息。
        var result = await _userManager.CreateAsync(user, register.Password);
        if (result.Succeeded)
        {
            // IdentityUser創建成功
            return RedirectToAction("Login");
        }
        else
        {
            // 回傳創建IdentityUser的錯誤信息
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }

            return View();
        }
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel login)
    {
        if (!ModelState.IsValid) return View();
        var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, true, false);

        if (result.Succeeded)
        {
            return Redirect("/");
        }
        else
        {
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Login", "You are locked out");
            }
            else
            {
                ModelState.AddModelError("Login", "Failed to login");
            }

            return View();
        }
    }
}