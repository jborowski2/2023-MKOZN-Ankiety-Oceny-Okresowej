using System;
using System.Linq;
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
    /// Kontroler zarządzający operacjami związanymi z kontem użytkownika.
    /// </summary>
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="ManageController"/>.
        /// </summary>
        public ManageController()
        {
        }
        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="ManageController"/> z określonymi menedżerami użytkowników i logowania.
        /// </summary>
        /// <param name="userManager">Menedżer użytkowników.</param>
        /// <param name="signInManager">Menedżer logowania.</param>
        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        /// Wyświetla stronę główną zarządzania kontem.
        /// </summary>
        /// <param name="message">Opcjonalna wiadomość statusu.</param>
        /// <returns>Widok strony głównej zarządzania kontem.</returns>
        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Zmieniono hasło."
                : message == ManageMessageId.SetPasswordSuccess ? "Ustawiono hasło."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Ustawiono dostawcę uwierzytelniania dwuetapowego."
                : message == ManageMessageId.Error ? "Wystąpił błąd."
                : message == ManageMessageId.AddPhoneSuccess ? "Dodano numer telefonu."
                : message == ManageMessageId.RemovePhoneSuccess ? "Usunięto numer telefonu."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        /// <summary>
        /// Usuwa logowanie zewnętrzne.
        /// </summary>
        /// <param name="loginProvider">Dostawca logowania.</param>
        /// <param name="providerKey">Klucz dostawcy.</param>
        /// <returns>Przekierowanie do strony zarządzania logowaniami.</returns>
        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        /// <summary>
        /// Wyświetla formularz dodawania numeru telefonu.
        /// </summary>
        /// <returns>Widok formularza dodawania numeru telefonu.</returns>
        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }
        /// <summary>
        /// Dodaje numer telefonu do konta użytkownika.
        /// </summary>
        /// <param name="model">Model dodawania numeru telefonu.</param>
        /// <returns>Przekierowanie do weryfikacji numeru telefonu.</returns>
        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Wygeneruj i wyślij token
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Twój kod zabezpieczający: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }
        /// <summary>
        /// Włącza uwierzytelnianie dwuskładnikowe.
        /// </summary>
        /// <returns>Przekierowanie do strony głównej zarządzania kontem.</returns>
        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        /// <summary>
        /// Wyłącza uwierzytelnianie dwuskładnikowe.
        /// </summary>
        /// <returns>Przekierowanie do strony głównej zarządzania kontem.</returns>
        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }
        /// <summary>
        /// Wyświetla formularz weryfikacji numeru telefonu.
        /// </summary>
        /// <param name="phoneNumber">Numer telefonu do weryfikacji.</param>
        /// <returns>Widok formularza weryfikacji numeru telefonu.</returns>
        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Wyślij wiadomość SMS za pośrednictwem dostawcy usług SMS w celu zweryfikowania numeru telefonu
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }
        /// <summary>
        /// Weryfikuje numer telefonu użytkownika.
        /// </summary>
        /// <param name="model">Model weryfikacji numeru telefonu.</param>
        /// <returns>Przekierowanie do strony głównej zarządzania kontem.</returns>
        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Dotarcie do tego miejsca wskazuje, że wystąpił błąd, wyświetl ponownie formularz
            ModelState.AddModelError("", "Nie można zweryfikować numeru telefonu");
            return View(model);
        }

        /// <summary>
        /// Usuwa numer telefonu z konta użytkownika.
        /// </summary>
        /// <returns>Przekierowanie do strony głównej zarządzania kontem.</returns>
        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }
        /// <summary>
        /// Wyświetla formularz zmiany hasła.
        /// </summary>
        /// <returns>Widok formularza zmiany hasła.</returns>
        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Zmienia hasło użytkownika.
        /// </summary>
        /// <param name="model">Model zmiany hasła.</param>
        /// <returns>Przekierowanie do strony głównej zarządzania kontem.</returns>
        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }
        /// <summary>
        /// Wyświetla formularz ustawienia hasła.
        /// </summary>
        /// <returns>Widok formularza ustawienia hasła.</returns>
        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }
        /// <summary>
        /// Ustawia hasło użytkownika.
        /// </summary>
        /// <param name="model">Model ustawienia hasła.</param>
        /// <returns>Przekierowanie do strony głównej zarządzania kontem.</returns>
        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Dotarcie do tego miejsca wskazuje, że wystąpił błąd, wyświetl ponownie formularz
            return View(model);
        }
        /// <summary>
        /// Zarządza logowaniami zewnętrznymi użytkownika.
        /// </summary>
        /// <param name="message">Opcjonalna wiadomość statusu.</param>
        /// <returns>Widok zarządzania logowaniami.</returns>
        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Usunięto logowanie zewnętrzne."
                : message == ManageMessageId.Error ? "Wystąpił błąd."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }
        /// <summary>
        /// Inicjuje połączenie z logowaniem zewnętrznym.
        /// </summary>
        /// <param name="provider">Dostawca logowania zewnętrznego.</param>
        /// <returns>Przekierowanie do dostawcy logowania zewnętrznego.</returns>
        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Żądaj przekierowania do dostawcy logowania zewnętrznego w celu połączenia logowania bieżącego użytkownika
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }
        /// <summary>
        /// Callback po połączeniu z logowaniem zewnętrznym.
        /// </summary>
        /// <returns>Przekierowanie do zarządzania logowaniami.</returns>
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        /// <summary>
        /// Zwolnienie zasobów.
        /// </summary>
        /// <param name="disposing">Wartość true, jeśli zarządzane zasoby powinny być zwolnione; w przeciwnym razie false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Pomocnicy
        // Używane w przypadku ochrony XSRF podczas dodawania logowań zewnętrznych
        /// <summary>
        /// Identyfikator dla zapytań XSRF.
        /// </summary>
        private const string XsrfKey = "XsrfId";
        /// <summary>
        /// Pobiera menedżera uwierzytelniania.
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        /// <summary>
        /// Dodaje błędy do ModelState.
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
        /// Sprawdza, czy użytkownik ma ustawione hasło.
        /// </summary>
        /// <returns>Prawda, jeśli użytkownik ma ustawione hasło, w przeciwnym razie fałsz.</returns>
        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }
        /// <summary>
        /// Enums zarządzania wiadomościami.
        /// </summary>
        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}