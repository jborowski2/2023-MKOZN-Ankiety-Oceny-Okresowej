using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OOP.Models;

namespace OOP.Controllers
{
    /// <summary>
    /// Kontroler zarządzający kontami użytkowników.
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="AccountController"/>.
        /// </summary>
        public AccountController()
        {
        }

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="AccountController"/> z podanym menedżerem użytkowników i menedżerem logowania.
        /// </summary>
        /// <param name="userManager">Menedżer użytkowników.</param>
        /// <param name="signInManager">Menedżer logowania.</param>
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        /// <summary>
        /// Pobiera menedżera logowania.
        /// </summary>
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        /// <summary>
        /// Pobiera menedżera użytkowników.
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Wyświetla widok usuwania kont użytkowników.
        /// </summary>
        /// <param name="searchString">Ciąg wyszukiwania.</param>
        /// <returns>Widok usuwania kont użytkowników.</returns>
        [HttpGet]
        public ActionResult DeleteAccount(string searchString)
        {
            IEnumerable<ApplicationUser> users = db.Users.ToList();
      
            return View(users);
        }

        /// <summary>
        /// Wyświetla widok potwierdzenia usunięcia użytkownika.
        /// </summary>
        /// <param name="userID">Identyfikator użytkownika.</param>
        /// <returns>Widok potwierdzenia usunięcia użytkownika.</returns>
        [HttpGet]
        public ActionResult ConfirmDelete(string userID)
        {
            if (userID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(userID);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// Potwierdza i usuwa użytkownika.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns>Przekierowanie do widoku usuwania kont.</returns>
        [HttpPost, ActionName("ConfirmDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {

            ApplicationUser userToDelete = db.Users.Find(id);
            db.Users.Remove(userToDelete);
            await db.SaveChangesAsync();
            return RedirectToAction("DeleteAccount");
        }

        /// <summary>
        /// Wyświetla widok logowania.
        /// </summary>
        /// <param name="returnUrl">Adres URL powrotu.</param>
        /// <returns>Widok logowania.</returns>
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Obsługuje żądanie logowania.
        /// </summary>
        /// <param name="model">Model logowania.</param>
        /// <param name="returnUrl">Adres URL powrotu.</param>
        /// <returns>Przekierowanie na odpowiednią stronę po zalogowaniu.</returns>
        /// 
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Nie powoduje to liczenia niepowodzeń logowania w celu zablokowania konta
            // Aby włączyć wyzwalanie blokady konta po określonej liczbie niepomyślnych prób wprowadzenia hasła, zmień ustawienie na shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Nieprawidłowa próba logowania.");
                    return View(model);
            }
        }

        /// <summary>
        /// Wyświetla widok weryfikacji kodu dwuskładnikowego.
        /// </summary>
        /// <param name="provider">Dostawca kodu.</param>
        /// <param name="returnUrl">Adres URL powrotu.</param>
        /// <param name="rememberMe">Czy zapamiętać użytkownika.</param>
        /// <returns>Widok weryfikacji kodu dwuskładnikowego.</returns>
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Wymagaj, aby użytkownik zalogował się już za pomocą nazwy użytkownika/hasła lub logowania zewnętrznego
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        /// <summary>
        /// Obsługuje żądanie weryfikacji kodu dwuskładnikowego.
        /// </summary>
        /// <param name="model">Model weryfikacji kodu.</param>
        /// <returns>Przekierowanie na odpowiednią stronę po weryfikacji.</returns>
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Poniższy kod chroni przed atakami na zasadzie pełnego przeglądu kodu dwuczynnikowego. 
            // Jeśli użytkownik będzie wprowadzać niepoprawny kod przez określoną ilość czasu, konto użytkownika 
            // zostanie zablokowane na określoną ilość czasu. 
            // Możesz skonfigurować ustawienia blokady konta w elemencie IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Nieprawidłowy kod.");
                    return View(model);
            }
        }

        /// <summary>
        /// Wyświetla widok rejestracji użytkownika.
        /// </summary>
        /// <returns>Widok rejestracji.</returns>
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Obsługuje żądanie rejestracji nowego użytkownika.
        /// </summary>
        /// <param name="model">Model rejestracji.</param>
        /// <returns>Przekierowanie na odpowiednią stronę po rejestracji.</returns>
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                user.Adres = new Adres();
                user.Adres.Ulica = model.Street;
                user.Adres.Miasto = model.Town;
                user.Adres.KodPocztowy = model.ZipCode;
                user.Adres.Numer = model.HauseNumber;

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    var pracownik = new Pracownik
                    {
                        ApplicationUserID = user.Id,
                        Imie = model.FirstName,
                        Nazwisko = model.LastName,
                        Tytul = model.Title,
                        Stopien = model.Degree,
                        PrzelozonyID = 0,
                        NumerTelefonu = model.PhoneNumber
                    };
                    db.Pracownicy.Add(pracownik);

                    IdentityManager im = new IdentityManager();
                    im.AddUserToRole(user.Id, "Pracownik");

                    await db.SaveChangesAsync();

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // Aby uzyskać więcej informacji o sposobie włączania potwierdzania konta i resetowaniu hasła, odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=320771
                    // Wyślij wiadomość e-mail z tym łączem
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Potwierdź konto", "Potwierdź konto, klikając <a href=\"" + callbackUrl + "\">tutaj</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // Dotarcie do tego miejsca wskazuje, że wystąpił błąd, wyświetl ponownie formularz
            return View(model);
        }

        /// <summary>
        /// Potwierdza adres e-mail użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <param name="code">Kod potwierdzający.</param>
        /// <returns>Widok potwierdzenia lub błąd.</returns>
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        /// <summary>
        /// Wyświetla formularz zapomnianego hasła.
        /// </summary>
        /// <returns>Widok formularza zapomnianego hasła.</returns>
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Przetwarza żądanie zapomnianego hasła.
        /// </summary>
        /// <param name="model">Model zapomnianego hasła.</param>
        /// <returns>Widok potwierdzenia zapomnianego hasła lub formularza z błędami.</returns>
        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Nie ujawniaj informacji o tym, że użytkownik nie istnieje lub nie został potwierdzony
                    return View("ForgotPasswordConfirmation");
                }

                // Aby uzyskać więcej informacji o sposobie włączania potwierdzania konta i resetowaniu hasła, odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=320771
                // Wyślij wiadomość e-mail z tym łączem
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Resetuj hasło", "Resetuj hasło, klikając <a href=\"" + callbackUrl + "\">tutaj</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Dotarcie do tego miejsca wskazuje, że wystąpił błąd, wyświetl ponownie formularz
            return View(model);
        }

        /// <summary>
        /// Wyświetla potwierdzenie zapomnianego hasła.
        /// </summary>
        /// <returns>Widok potwierdzenia zapomnianego hasła.</returns>
        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Wyświetla formularz resetowania hasła.
        /// </summary>
        /// <param name="code">Kod resetowania hasła.</param>
        /// <returns>Widok formularza resetowania hasła lub błąd.</returns>
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        /// <summary>
        /// Przetwarza żądanie resetowania hasła.
        /// </summary>
        /// <param name="model">Model resetowania hasła.</param>
        /// <returns>Widok potwierdzenia resetowania hasła lub formularza z błędami.</returns>
        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Nie ujawniaj informacji o tym, że użytkownik nie istnieje
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        /// <summary>
        /// Wyświetla potwierdzenie resetowania hasła.
        /// </summary>
        /// <returns>Widok potwierdzenia resetowania hasła.</returns>
        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Przetwarza żądanie logowania zewnętrznego.
        /// </summary>
        /// <param name="provider">Dostawca logowania zewnętrznego.</param>
        /// <param name="returnUrl">Adres URL powrotu.</param>
        /// <returns>Wywołanie ChallengeResult.</returns>
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Żądaj przekierowania do dostawcy logowania zewnętrznego
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        /// <summary>
        /// Wyświetla formularz wysyłania kodu dwuskładnikowego uwierzytelniania.
        /// </summary>
        /// <param name="returnUrl">Adres URL powrotu.</param>
        /// <param name="rememberMe">Flaga pamiętaj mnie.</param>
        /// <returns>Widok formularza wysyłania kodu.</returns>
        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        /// <summary>
        /// Przetwarza żądanie wysłania kodu dwuskładnikowego uwierzytelniania.
        /// </summary>
        /// <param name="model">Model wysyłania kodu.</param>
        /// <returns>Widok potwierdzenia wysłania kodu lub formularza z błędami.</returns>
        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Wygeneruj i wyślij token
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        /// <summary>
        /// Przetwarza odpowiedź z zewnętrznego logowania.
        /// </summary>
        /// <param name="returnUrl">Adres URL powrotu.</param>
        /// <returns>Widok odpowiedzi z logowania zewnętrznego.</returns>
        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Zaloguj użytkownika przy użyciu tego dostawcy logowania zewnętrznego, jeśli użytkownik ma już nazwę logowania
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Jeśli użytkownik nie ma konta, poproś go o utworzenie konta
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        /// <summary>
        /// Przetwarza potwierdzenie zewnętrznego logowania.
        /// </summary>
        /// <param name="model">Model potwierdzenia zewnętrznego logowania.</param>
        /// <param name="returnUrl">Adres URL powrotu.</param>
        /// <returns>Widok potwierdzenia zewnętrznego logowania.</returns>
        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Uzyskaj informacje o użytkowniku od dostawcy logowania zewnętrznego
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        /// <summary>
        /// Wylogowuje użytkownika.
        /// </summary>
        /// <returns>Przekierowanie do strony głównej.</returns>
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Wyświetla widok błędu logowania zewnętrznego.
        /// </summary>
        /// <returns>Widok błędu logowania zewnętrznego.</returns>
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        /// <summary>
        /// Zwalnia zasoby używane przez kontroler.
        /// </summary>
        /// <param name="disposing">Wartość logiczna wskazująca, czy zarządzane zasoby powinny być zwolnione.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Pomocnicy
        // Używane w przypadku ochrony XSRF podczas dodawania logowań zewnętrznych
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Dodaje błędy do stanu modelu.
        /// </summary>
        /// <param name="result">Wynik operacji tożsamości.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// Przekierowuje do lokalnego adresu URL.
        /// </summary>
        /// <param name="returnUrl">Adres URL powrotu.</param>
        /// <returns>Akcja przekierowania.</returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Wynik wyzwania do uwierzytelniania.
        /// </summary>
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            /// <summary>
            /// Inicjalizuje nowe wystąpienie klasy ChallengeResult z podanym dostawcą i URI przekierowania.
            /// </summary>
            /// <param name="provider">Nazwa dostawcy logowania.</param>
            /// <param name="redirectUri">URI przekierowania po zakończeniu uwierzytelniania.</param>
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }
            /// <summary>
            /// Inicjalizuje nowe wystąpienie klasy ChallengeResult z podanym dostawcą, URI przekierowania i identyfikatorem użytkownika.
            /// </summary>
            /// <param name="provider">Nazwa dostawcy logowania.</param>
            /// <param name="redirectUri">URI przekierowania po zakończeniu uwierzytelniania.</param>
            /// <param name="userId">Identyfikator użytkownika (opcjonalny).</param>
            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }
            /// <summary>
            /// Pobiera lub ustawia nazwę dostawcy logowania.
            /// </summary>
            public string LoginProvider { get; set; }
            /// <summary>
            /// Pobiera lub ustawia URI przekierowania po zakończeniu uwierzytelniania.
            /// </summary>
            public string RedirectUri { get; set; }
            /// <summary>
            /// Pobiera lub ustawia identyfikator użytkownika.
            /// </summary>
            public string UserId { get; set; }
            /// <summary>
            /// Wykonuje wynik, wywołując wyzwanie uwierzytelniania.
            /// </summary>
            /// <param name="context">Kontekst kontrolera.</param>
            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}