using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EventManagementSystem.Entity.Model.Authorization;
using EventManagementSystem.Entity.Model.Common;
using EventManagementSystem.Helper;
using EventManagementSystem.Service;
using EventManagementSystem.Service.Helper;
using EventManagementSystem.Web.Helpers;
using EventManagementSystem.Web.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;

namespace EventManagementSystem.Web.Controllers
{
    [RoutePrefix("kullanici")]
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult AutoLogin(string key)
        {
            try
            {
                UserRoleCookie.CookieRemove();
                var userService = new UserService();
                var passPhrase = ConfigurationManager.AppSettings["EncKey"];
                key = Base64Converter.Decode(key);
                var username = StringCipher.Decrypt(key, passPhrase);
                var user = userService.GetByUsername(username);
                if (user != null)
                {
                    if (!user.MailConfirmation)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), true);
                    user.LastLoginDate = DateTime.Now;
                    userService.Update(user);
                    Logger.Info(user.Id + " idli kullanıcı oto. giriş yaptı");
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }

            return RedirectToAction("Index", "Home");
        }

        [Route("oturum-ac")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            var random = new Random();
            var accountModel = new AccountModel();

            UserRoleCookie.CookieRemove();
            try
            {
                GenarateCommonModel(accountModel);
                accountModel.LoginBackgroundPhoto = accountModel.LoginBackgroundPhotos?[random.Next(accountModel.LoginBackgroundPhotos.Length)];


                if (accountModel.AzureLoginStatus == "Her Zaman Aktif")
                {
                    //Azure login aktif ise
                    if (!Request.IsAuthenticated || string.IsNullOrEmpty(User.Identity.Name))
                    {
                        //Giriş yok ise
                        HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);

                    }
                    else
                    {
                        //Giriş var ise

                        //kullanıcı normal giriş işlemleri burda yapılacak
                        var username = User.Identity.Name;
                        accountModel.User = UserService.GetByUsername(username);
                        try
                        {
                            if (accountModel.User != null)
                            {
                                if (!accountModel.User.MailConfirmation)
                                {
                                    accountModel.HasError = true;
                                    accountModel.ErrorMessage = "E-Mail gelen kutusundan E-Mail'inizi doğrulayınız.";
                                    accountModel.ConfirmationError = true;
                                    return View(accountModel);
                                }
                                accountModel.User.Email = accountModel.User.Email;
                                accountModel.User.LastLoginDate = DateTime.Now;
                                UserService.Update(accountModel.User);
                                Logger.Info(accountModel.User.Id + " idli kullanıcı azure ile giriş yaptı");
                                if (Request.QueryString["ReturnUrl"] != null)
                                    return Redirect(Request.QueryString["ReturnUrl"]);
                                return RedirectToAction("Index", "Home");
                            }

                        }
                        catch (Exception e)
                        {

                        }

                    }

                }
                else
                {
                    //Azure kullanılmayacaksa ve login ise anasayfaya yönlendirme yapılacak,


                }

            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }

            return View(accountModel);
        }


        [Route("sifremi-unuttum")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgottenPassword()
        {
            var random = new Random();
            var accountModel = new AccountModel();

            GenarateCommonModel(accountModel);
            accountModel.LoginBackgroundPhoto = accountModel.LoginBackgroundPhotos?[random.Next(accountModel.LoginBackgroundPhotos.Length)];
            return View(accountModel);
        }


        [Route("sifremi-unuttum")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgottenPassword(AccountModel accountModel)
        {
            Random random = new Random();
            GenarateCommonModel(accountModel);
            accountModel.LoginBackgroundPhoto = accountModel.LoginBackgroundPhotos?[random.Next(accountModel.LoginBackgroundPhotos.Length)];
            try
            {

                var authUser = UserService.GetByEmail(accountModel.User.Email);
                if (authUser == null)
                {
                    accountModel.HasError = true;
                    accountModel.ErrorMessage = "Kullanıcı bulunamadı.";
                    return View(accountModel);
                }
                else
                {
                    if (!authUser.MailConfirmation)
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "E-Mail gelen kutusundan E-Mail'inizi doğrulayınız.";
                        accountModel.ConfirmationError = true;
                        return View(accountModel);
                    }
                    else
                    {
                        var isSend = MailSendHelper.SendEmail(new MailModel
                        {
                            FullName = authUser.FullName,
                            Guid = authUser.ChangePasswordCode,
                            Subject = accountModel.SiteHeader+" Şifre Değiştirme Kodu",
                            MailAddress = authUser.Email,
                            MailTemplate = MailTemplate.ForgottenPassword
                        });
                        if (!isSend)
                        {
                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "Hata Oluştu. Daha sonra tekrar deneyiniz.";
                            return View(accountModel);
                        }
                        accountModel.HasError = false;
                        accountModel.IsPasswordSent = true;
                        accountModel.ErrorMessage = "Şifreniz değişim talebi E-Mail adresinize gönderildi.";
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return View(accountModel);
        }


        [Route("oturum-ac")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(AccountModel accountModel)
        {

            try
            {
                Random random = new Random();
                GenarateCommonModel(accountModel);
                accountModel.LoginBackgroundPhoto = accountModel.LoginBackgroundPhotos?[random.Next(accountModel.LoginBackgroundPhotos.Length)];

                if (accountModel.CaptchaStatus == "Her Zaman Aktif" ||
                    (accountModel.CaptchaStatus == "Hatalı Girişte Aktif" && accountModel.HasError))
                {
                    if (!GoogleReCaptchaHelper.IsValidCaptcha())
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "Kutuyu işaretlemeniz gerekmektedir.";
                        return View(accountModel);
                    }
                }
                try
                {
                    var authUser = UserService.Get(accountModel.User.Email, accountModel.User.Password);
                    if (authUser == null)
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "Hatalı kullanıcı adı veya şifre.";
                        return View(accountModel);
                    }
                    else
                    {
                        if (!authUser.MailConfirmation)
                        {
                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "E-Mail gelen kutusundan E-Mail'inizi doğrulayınız.";
                            accountModel.ConfirmationError = true;
                            return View(accountModel);
                        }
                        FormsAuthentication.SetAuthCookie(authUser.Id.ToString(), false);
                        authUser.LastLoginDate = DateTime.Now;
                        UserService.Update(authUser);
                        Logger.Info(authUser.Id + " idli kullanıcı  giriş yaptı");


                      


                        if (Request.QueryString["ReturnUrl"] != null)
                            return Redirect(Request.QueryString["ReturnUrl"]);
                        return RedirectToAction("Index", "Home");
                    }

                }
                catch (Exception e)
                {
                    Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                }

                var user = UserService.Get(accountModel.User.Email, accountModel.User.Password);
                if (user == null)
                {
                    accountModel.HasError = true;
                    accountModel.ErrorMessage = "Hatalı kullanıcı adı veya şifre.";
                    return View(accountModel);
                }
                else
                {
                    if (!accountModel.User.MailConfirmation)
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "E-Mail gelen kutusundan E-Mail'inizi doğrulayınız.";
                        accountModel.ConfirmationError = true;
                        return View(accountModel);
                    }
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                    user.LastLoginDate = DateTime.Now;
                    UserService.Update(user);
                    Logger.Info(user.Id + " idli kullanıcı  giriş yaptı");
                    if (Request.QueryString["ReturnUrl"] != null)
                        return Redirect(Request.QueryString["ReturnUrl"]);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return RedirectToAction("Index", "Home");
            }

        }



        [Route("onay-eposta-gonder")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult NewConfirmationEmail(String eposta)
        {
            var random = new Random();
            var accountModel = new AccountModel();

            GenarateCommonModel(accountModel);
            accountModel.LoginBackgroundPhoto = accountModel.LoginBackgroundPhotos?[random.Next(accountModel.LoginBackgroundPhotos.Length)];

            if (String.IsNullOrEmpty(eposta))
            {
                accountModel.HasError = true;
                accountModel.ErrorMessage = "Lütfen E-Posta alanını doldurunuz.";
                return View(accountModel);
            }
            else
            {
                var authUser = UserService.GetByEmail(eposta);
                if (authUser == null)
                {
                    accountModel.HasError = true;
                    accountModel.ErrorMessage = "E-Posta ile uyuşan bir hesap bulamadık. Tekrar deneyiniz.";
                    return View(accountModel);
                }
                else
                {
                    if (!authUser.MailConfirmation)
                    {
                        #region Mail onayla

                        var isSendWelcome = MailSendHelper.SendEmail(new MailModel
                        {
                            FullName = authUser.FullName,
                            Subject = accountModel.SiteHeader+" Hoşgeldiniz",
                            Guid = authUser.MailConfirmationCode,
                            MailAddress = authUser.Email,
                            MailTemplate = MailTemplate.Welcome
                        });
                        if (!isSendWelcome)
                        {
                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "Hata Oluştu. Daha sonra tekrar deneyiniz.";
                            return View(accountModel);
                        }
                        #endregion
                        accountModel.HasError = false;
                        accountModel.ErrorMessage = "Yeni Onaylama mesajını E-Postanıza gönderdik. Lütfen kontrol ediniz.";
                        accountModel.ConfirmationError = false;
                        return View(accountModel);
                    }
                }
            }

            return View(accountModel);
        }



        [Route("check-login/{email}/{password}")]
        [AllowAnonymous]
        [HttpGet]
        public string CheckLoginCredentials(string email, string password)
        {
            try
            {
                try
                {
                    var authUser = UserService.Get(email, password);
                    if (authUser == null)
                    {
                        return "INVALID";
                    }
                    else
                    {
                        if (!authUser.MailConfirmation)
                        {
                            return "NEED_CONFIRM";
                        }
                        else
                        {
                            return "OK";
                        }
                    }
                }
                catch (Exception e)
                {
                    return "ERROR";
                }
            }
            catch (Exception e)
            {
                return "ERROR";
            }
        }


        [Route("auto-login/{email}/{password}")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult AutoLogin(string email, string password)
        {
            AccountModel accountModel = new AccountModel();
            try
            {

                UserRoleCookie.CookieRemove();
                var authUser = UserService.Get(email, password);
                if (authUser == null)
                {
                    accountModel.HasError = true;
                    accountModel.ErrorMessage = "Hatalı kullanıcı adı veya şifre.";
                    return View(accountModel);
                }
                else
                {
                    if (!authUser.MailConfirmation)
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "E-Mail gelen kutusundan E-Mail'inizi doğrulayınız.";
                        accountModel.ConfirmationError = true;
                        return View(accountModel);
                    }
                    FormsAuthentication.SetAuthCookie(authUser.Id.ToString(), false);
                    authUser.LastLoginDate = DateTime.Now;
                    UserService.Update(authUser);
                    Logger.Info(authUser.Id + " idli kullanıcı  giriş yaptı");

                    

                    if (Request.QueryString["ReturnUrl"] != null)
                        return Redirect(Request.QueryString["ReturnUrl"]);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }

            var user = UserService.Get(email, password);
            if (user == null)
            {
                accountModel.HasError = true;
                accountModel.ErrorMessage = "Hatalı kullanıcı adı veya şifre.";
                return View(accountModel);
            }
            else
            {
                if (!user.MailConfirmation)
                {
                    accountModel.HasError = true;
                    accountModel.ErrorMessage = "E-Mail gelen kutusundan E-Mail'inizi doğrulayınız.";
                    accountModel.ConfirmationError = true;
                    return View(accountModel);
                }
                FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                user.LastLoginDate = DateTime.Now;
                UserService.Update(user);
                Logger.Info(user.Id + " idli kullanıcı  giriş yaptı");
                if (Request.QueryString["ReturnUrl"] != null)
                    return Redirect(Request.QueryString["ReturnUrl"]);
                return RedirectToAction("Index", "Home");
            }
        }

        


        [Route("control-login/{Email}/{Password}")]
        [AllowAnonymous]
        [HttpGet]
        public JsonResult ControlLogin(string Email, string Password)
        {

            try
            {
                var accountModel = new AccountModel();
                GenarateCommonModel(accountModel);
                accountModel.User = new User();

                accountModel.User.Email = Email;
                accountModel.User.Password = md5Helper.GetMD5_2(Password);


                try
                {
                    var authUser = UserService.Get(accountModel.User.Email, accountModel.User.Password);
                    if (authUser == null)
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "Kullanıcınız " + accountModel.SiteHeader + " Portal'i kullanmak için yetkili değildir.";

                        return Json(new LoginJsonModel
                        {
                            IsLogin = !accountModel.HasError,
                            Link = "",
                            ErrorMessage = accountModel.ErrorMessage
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (!authUser.MailConfirmation)
                        {
                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "E-Mail gelen kutusundan E-Mail'inizi doğrulayınız.";
                            accountModel.ConfirmationError = true;

                            return Json(new LoginJsonModel
                            {
                                IsLogin = !accountModel.HasError,
                                Link = "",
                                ErrorMessage = accountModel.ErrorMessage
                            }, JsonRequestBehavior.AllowGet);
                        }
                        FormsAuthentication.SetAuthCookie(authUser.Id.ToString(), false);
                        authUser.LastLoginDate = DateTime.Now;
                        UserService.Update(authUser);
                        Logger.Info(authUser.Id + " idli kullanıcı  giriş yaptı");


                        return Json(new LoginJsonModel
                        {
                            IsLogin = !accountModel.HasError,
                            Link = this.Url.Action("Login", "Account", null, this.Request.Url?.Scheme),
                            ErrorMessage = accountModel.ErrorMessage
                        }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception e)
                {
                    Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                }

                var user = UserService.Get(accountModel.User.Email, accountModel.User.Password);
                if (user == null)
                {
                    accountModel.HasError = true;
                    accountModel.ErrorMessage = "Hatalı kullanıcı adı veya şifre.";

                    return Json(new LoginJsonModel
                    {
                        IsLogin = !accountModel.HasError,
                        Link = "",
                        ErrorMessage = accountModel.ErrorMessage
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (!accountModel.User.MailConfirmation)
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "E-Mail gelen kutusundan E-Mail'inizi doğrulayınız.";
                        accountModel.ConfirmationError = true;

                        return Json(new LoginJsonModel
                        {
                            IsLogin = !accountModel.HasError,
                            Link = "",
                            ErrorMessage = accountModel.ErrorMessage
                        }, JsonRequestBehavior.AllowGet);
                    }
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                    user.LastLoginDate = DateTime.Now;
                    UserService.Update(user);
                    Logger.Info(user.Id + " idli kullanıcı  giriş yaptı");
                    if (Request.QueryString["ReturnUrl"] != null)
                        return Json(new LoginJsonModel
                        {
                            IsLogin = !accountModel.HasError,
                            Link = Request.QueryString["ReturnUrl"],
                            ErrorMessage = accountModel.ErrorMessage
                        }, JsonRequestBehavior.AllowGet);

                    return Json(new LoginJsonModel
                    {
                        IsLogin = !accountModel.HasError,
                        Link = this.Url.Action("Login", "Account", null, this.Request.Url?.Scheme),
                        ErrorMessage = accountModel.ErrorMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);

                return Json(new LoginJsonModel
                {
                    IsLogin = false,
                    Link = "",
                    ErrorMessage = e.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [AllowAnonymous]
        [Route("oturum-kapat")]
        public ActionResult Logout()
        {

            try
            {
                var model = new AccountModel();
                GenarateCommonModel(model);

                int number;
                var resul = int.TryParse(User.Identity.Name, out number);
                if (!resul)
                {
                    FormsAuthentication.SignOut();

                    Logger.Info(User.Identity.Name + " idli kullanıcı çıkış yaptı");
                    HttpContext.GetOwinContext().Authentication.SignOut(
                        OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
                }
                else
                {

                    FormsAuthentication.SignOut();
                    Logger.Info(User.Identity.Name + " idli kullanıcı çıkış yaptı");
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }


            return RedirectToAction("Index", "Home");
        }

        [Route("kayit-ol")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            var random = new Random();
            var accountModel = new AccountModel();

            try
            {
                GenarateCommonModel(accountModel);
                accountModel.LoginBackgroundPhoto = accountModel.LoginBackgroundPhotos?[random.Next(accountModel.LoginBackgroundPhotos.Length)];

                
                accountModel.User = new User();
                accountModel.User.IsActive = true;
                accountModel.User.IsDeleted = false;
                accountModel.User.MailPermission = true;


                var defaultChoice = Request.QueryString["lc"];
                if (defaultChoice != null)
                {
                    accountModel.SelectedLicenseTypeId = int.Parse(defaultChoice);
                }

            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }



            return View(accountModel);
        }

        [Route("kayit-ol")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(AccountModel accountModel)
        {
            
            try
            {
                GenarateCommonModel(accountModel);
                if (accountModel.CaptchaStatus == "Her Zaman Aktif" ||
                    (accountModel.CaptchaStatus == "Hatalı Girişte Aktif" && accountModel.HasError))
                {
                    if (!GoogleReCaptchaHelper.IsValidCaptcha())
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "Kutuyu işaretlemeniz gerekmektedir.";
                        return View(accountModel);
                    }
                }
                try
                {
                    var Email = accountModel.User.Email;

                    var authUser = UserService.GetByEmail(Email);
                    if (authUser == null)
                    {
                        var registerUser = new User();
                        if (accountModel.User.Password != accountModel.PasswordConfirmation)
                        {
                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "Şifreler uyuşmuyor.";
                            accountModel.PasswordConfirmation = string.Empty;
                            return View(accountModel);
                        }
                        
                

                        registerUser.FullName = accountModel.User.FullName;
                        registerUser.Email = accountModel.User.Email;
                        registerUser.Password = md5Helper.GetMD5_2(accountModel.User.Password);
                        registerUser.Username = accountModel.User.Username;
                       
                        registerUser.Adress = accountModel.User.Adress;
                        registerUser.PhoneNumber = (accountModel.User.PhoneNumber == "0000 000 00 00" ? "" : PhoneMaskHelper.FormatPhoneNumber(accountModel.User.PhoneNumber));
                        registerUser.MailPermission = accountModel.User.MailPermission;
                        registerUser.IsActive = true;
                        registerUser.IsDeleted = false;
                        registerUser.IsAdmin = false;
                        registerUser.IsTestUser = false;
                        UserService.Add(registerUser);



                        #region Mail onayla

                        var isSendWelcome = MailSendHelper.SendEmail(new MailModel
                        {
                            FullName = registerUser.FullName,
                            Subject = accountModel.SiteHeader+" Hoşgeldiniz",
                            UserID = registerUser.Id,
                            Guid = registerUser.MailConfirmationCode,
                            MailAddress = registerUser.Email,
                            MailTemplate = MailTemplate.Welcome
                        });
                        if (!isSendWelcome)
                        {
                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "Hata Oluştu. Daha sonra tekrar deneyiniz.";
                            return View(accountModel);
                        }
                        #endregion
                       

                        var userGroup = RoleService.GetByName("Standart Kullanıcı");
                        if (userGroup != null)
                        {
                            UserRoleService.Add(new UserRole
                            {
                                RoleId = userGroup.Id,
                                UserId = registerUser.Id,
                                IsActive = true,
                                IsDeleted = false
                            });
                        }
                        

                        #region Lisans_Olusturma


                       
                        


                        #endregion


                        


                       
                            return RedirectToAction("Index", "Home");
                        
                    }
                    else
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "Bu E-Mail " + accountModel.SiteHeader + " Portal'ine daha önceden kayıt yaptırmıştır.";
                        return View(accountModel);
                    }

                }
                catch (Exception e)
                {
                    Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return RedirectToAction("Index", "Home");
            }

        }

        [Route("odeme/{UserId}/{LicenseTypeId}")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterPayment(int UserId, int LicenseTypeId)
        {
            var AccountModel = new AccountModel();
            try
            {
                GenarateCommonModel(AccountModel);
                AccountModel.UserId = UserId;
                AccountModel.SelectedLicenseTypeId = LicenseTypeId;
                AccountModel.User = UserService.Get(UserId);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return View(AccountModel);
        }

        

        
        [AuthorizeUser(Action = "RegisterEdit")]
        [Route("duzenle")]
        [HttpGet]
        public ActionResult RegisterEdit()
        {
            var random = new Random();
            var accountModel = new AccountModel();

            try
            {
                GenarateCommonModel(accountModel);
                accountModel.LoginBackgroundPhoto = accountModel.LoginBackgroundPhotos?[random.Next(accountModel.LoginBackgroundPhotos.Length)];

                var user = CurrentUser;
                
                accountModel.User = user;
                
                
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return View(accountModel);
        }

        [AuthorizeUser(Action = "RegisterEdit")]
        [Route("duzenle")]
        [HttpPost]
        public ActionResult RegisterEdit(AccountModel accountModel)
        {
            
            try
            {
                GenarateCommonModel(accountModel);
                if (accountModel.CaptchaStatus == "Her Zaman Aktif" ||
                    (accountModel.CaptchaStatus == "Hatalı Girişte Aktif" && accountModel.HasError))
                {
                    if (!GoogleReCaptchaHelper.IsValidCaptcha())
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "Kutuyu işaretlemeniz gerekmektedir.";
                        return View(accountModel);
                    }
                }
                try
                {

                    var authUser = CurrentUser;
                    if (authUser != null)
                    {
                        var userOther = UserService.GetByEmail(accountModel.User.Email);
                        if (userOther != null && userOther.Id != authUser.Id)
                        {

                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "E-Mail Adresi Uyngun Değil!";
                            return View(accountModel);
                        }

                        authUser.FullName = accountModel.User.FullName;


                        authUser.TCKN = "";
                        authUser.CompanyName = "";
                        
                            authUser.CompanyName = accountModel.User.CompanyName;
                           
                       
                            authUser.TCKN = accountModel.User.TCKN;
                        

                        authUser.PhotoPath = accountModel.User.PhotoPath;
                        authUser.Adress = accountModel.User.Adress;
                        authUser.PhoneNumber = PhoneMaskHelper.FormatPhoneNumber(accountModel.User.PhoneNumber);
                        authUser.MailPermission = accountModel.User.MailPermission;
                        authUser.Email = accountModel.User.Email;

                        UserService.Update(authUser);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "Hata Oluştu.";
                        return View(accountModel);
                    }

                }
                catch (Exception e)
                {
                    Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return RedirectToAction("Index", "Home");
            }

        }



        #region registerJsonResult

        [Route("kullanici-resim-yukle")]
        [HttpPost]
        public ActionResult UploadUserImage()
        {
            try
            {

                var file = FileService.UploadFile(Request.Files[0], uploadPath: $"Contents/Files/UserFiles/");
                return Json(file.Path);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return Json("");
            }
        }


        #endregion
        [AuthorizeUser(Action = "RegisterEdit")]
        [Route("sifremi-degistir")]
        [HttpGet]
        public ActionResult RegisterChangePasswordInSite()
        {


            var random = new Random();
            var accountModel = new AccountModel();

            try
            {
                GenarateCommonModel(accountModel);
                accountModel.LoginBackgroundPhoto = accountModel.LoginBackgroundPhotos?[random.Next(accountModel.LoginBackgroundPhotos.Length)];
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }



            return View(accountModel);
        }

        [AuthorizeUser(Action = "RegisterEdit")]
        [Route("sifremi-degistir")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterChangePasswordInSite(AccountModel accountModel)
        {
            
            try
            {
                GenarateCommonModel(accountModel);
                try
                {
                    var authUser = CurrentUser;
                    if (authUser != null)
                    {
                        if (authUser.Password != md5Helper.GetMD5_2(accountModel.User.Password))
                        {

                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "Eski Şifre Yanlış !";
                            return View(accountModel);
                        }
                        if (authUser.Password ==  md5Helper.GetMD5_2(accountModel.PasswordNew))
                        {

                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "Yeni Şifre Eski Şifre İle Aynı!";
                            return View(accountModel);
                        }


                        if (accountModel.PasswordConfirmation != accountModel.PasswordNew)
                        {

                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "Şifre Tekrarı Uyuşmuyor !";
                            return View(accountModel);
                        }


                        authUser.IsChangePassword = true;
                        authUser.ChangePasswordCode = Guid.NewGuid().ToString().Replace("-", "").ToLower();
                        authUser.Password = md5Helper.GetMD5_2(accountModel.PasswordNew);

                        UserService.Update(authUser);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        accountModel.HasError = true;
                        accountModel.ErrorMessage = "Hata Oluştu.";
                        return View(accountModel);
                    }

                }
                catch (Exception e)
                {
                    Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return RedirectToAction("Index", "Home");
            }

        }
        [Route("mail-aktif-et/{MailConfirmationCode}")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterMailConfirmation(string MailConfirmationCode)
        {
            var AccountModel = new AccountModel();
            try
            {
                var user = UserService.GetByMailConfirmationCode(MailConfirmationCode);
                if (user == null)
                {

                    AccountModel.HasError = true;
                    AccountModel.ErrorMessage = "E-Mail Onay Kodu Yanlış.";

                }
                else if (!user.MailConfirmation)
                {
                    AccountModel.HasError = false;
                    AccountModel.ErrorMessage = "E-Mail aktif edildi.";
                    user.MailConfirmation = true;
                    UserService.Update(user);
                    
                }
                else
                {
                    AccountModel.HasError = true;
                    AccountModel.ErrorMessage = "Bu E-Mail daha önceden aktif edilmiştir.";
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }

            return View(AccountModel);
        }

        [Route("sifre-degis-talep")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SendMailRegisterChangePassword()
        {
            var AccountModel = new AccountModel();

            return View(AccountModel);
        }
        [Route("sifre-degis-talep")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SendMailRegisterChangePassword(AccountModel Model)
        {
            var AccountModel = Model;
            try
            {
                var user = UserService.GetByEmail(Model.User.Email);
                if (user == null)
                {

                    AccountModel.HasError = true;
                    AccountModel.ErrorMessage = "Böyle Bir Kullanıcı Bulunamadı.";

                }
                else
                {

                    #region Mail onayla

                    var isSend = MailSendHelper.SendEmail(new MailModel
                    {
                        FullName = user.FullName,
                        Guid = user.ChangePasswordCode,
                        Subject = Model.SiteHeader + " Şifre Değiştirme Kodu",
                        MailAddress = user.Email,
                        MailTemplate = MailTemplate.ForgottenPassword
                    });
                    if (!isSend)
                    {
                        AccountModel.HasError = true;
                        AccountModel.ErrorMessage = "Hata Oluştu. Daha sonra tekrar deneyiniz.";
                        return View(AccountModel);
                    }
                    #endregion
                    AccountModel.HasError = false;
                    AccountModel.ErrorMessage = "Şifreniz değişim talebi E-Mail adresinize gönderildi.";
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }

            return View(AccountModel);
        }
        [Route("sifre-degis/{ChangePasswordCode}")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterChangePassword(string ChangePasswordCode)
        {
            var AccountModel = new AccountModel();
            try
            {
                var user = UserService.GetByChangePasswordCode(ChangePasswordCode);
                if (user == null)
                {

                    AccountModel.HasError = true;
                    AccountModel.ErrorMessage = "Hata Oluştu !";

                }
                else
                {
                    AccountModel.User = user;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }

            return View(AccountModel);
        }
        [Route("sifre-degis/{ChangePasswordCode}")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterChangePassword(AccountModel Model)
        {
            var AccountModel = Model;
            try
            {
                if (Model.User.Password != Model.PasswordConfirmation)
                {

                    AccountModel.HasError = true;
                    AccountModel.ErrorMessage = "Şifreler aynı değil.";
                    Model.PasswordConfirmation = "";
                    Model.User.Password = "";
                    return View(AccountModel);
                }
                var user = UserService.Get(Model.User.Id);
                if (user == null)
                {

                    AccountModel.HasError = true;
                    AccountModel.ErrorMessage = "Hata Oluştu !";

                }
                else
                {
                    user.ChangePasswordCode = Guid.NewGuid().ToString().Replace("-", "").ToLower();
                    user.IsChangePassword = true;
                    user.Password = md5Helper.GetMD5_2(Model.User.Password);
                    UserService.Update(user);
                    AccountModel.HasError = false;
                    AccountModel.ErrorMessage = "Şifreniz değiştirildi.";
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }

            return View(AccountModel);
        }
        [AuthorizeUser(Action = "AccountSettings")]
        [Route("modul-ayarlari")]
        [HttpGet]
        public ActionResult Settings()
        {
            var model = new AccountModel();

            try
            {

                GenarateCommonModel(model);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return View(model);
        }

        [AuthorizeUser(Action = "AccountSettings")]
        [Route("modul-ayarlari")]
        [HttpPost]
        public ActionResult Settings(AccountModel accountModel)
        {

            try
            {

                ParameterService.SetValueByKey("InvalidLoginAction", accountModel.InvalidLoginAction);
                ParameterService.SetValueByKey("InvalidLoginLimit", accountModel.InvalidLoginLimit);
                ParameterService.SetValueByKey("CaptchaStatus", accountModel.CaptchaStatus);
                ParameterService.SetValueByKey("AzureLoginStatus", accountModel.AzureLoginStatus);
                ParameterService.SetValueByKey("ShowUserPhotoOnLogin", accountModel.ShowUserPhotoOnLogin);
                ParameterService.SetValueByKey("SiteHeader", accountModel.SiteHeader);
                ParameterService.SetValueByKey("OneSignalAppId", accountModel.OneSignalAppId);
                ParameterService.SetValueByKey("OneSignalApiKey", accountModel.OneSignalApiKey);

                GenarateCommonModel(accountModel);
                accountModel.IsSaved = true;
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return View(accountModel);
        }




        [Route("resim-yukle")]
        [HttpPost]
        public ActionResult UploadImage()
        {
            var file = FileService.UploadFile(Request.Files[0]);


            try
            {

                var loginBackgroundPhotos = ParameterService.GetValueByKey("LoginBackgroundPhotos");
                if (string.IsNullOrEmpty(loginBackgroundPhotos))
                    loginBackgroundPhotos = file.Path;
                else
                    loginBackgroundPhotos += ";" + file.Path;
                ParameterService.SetValueByKey("LoginBackgroundPhotos", loginBackgroundPhotos);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return Json(file.Path);
        }

        [Route("resim-sil")]
        [HttpGet]
        public bool DeleteImage(string filePath)
        {

            try
            {

                var loginBackgroundPhotos = ParameterService.GetValueByKey("LoginBackgroundPhotos");
                loginBackgroundPhotos = loginBackgroundPhotos.Replace(filePath, "").Replace(";;", ";").Trim(';');
                ParameterService.SetValueByKey("LoginBackgroundPhotos", loginBackgroundPhotos);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return true;
        }

        [Route("kullanici-onizleme")]
        [AllowAnonymous]
        [HttpGet]
        public string UserPreview(string userField)
        {

            try
            {
                var user = userField.Contains("@") ? UserService.GetByEmail(userField) : UserService.GetByUsername(userField);
                if (user != null)
                {
                    var result = new
                    {
                        PhotoPath = user.PhotoPath,
                        FullName = user.FullName
                    };
                    return JsonConvert.SerializeObject(result);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return "";
        }


        private void GenarateCommonModel(AccountModel model)
        {

            try
            {

                model.InvalidLoginActions = new List<SelectListItem>
            {
                new SelectListItem {Text = "Devre Dışı"},
                new SelectListItem {Text = "Kullanıcı Hesabını Kilitle"},
                new SelectListItem {Text = "5 Dakika Oturum Açmayı Engelle"}
            };

                model.CaptchaStatuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Devre Dışı"},
                new SelectListItem {Text = "Hatalı Girişte Aktif"},
                new SelectListItem {Text = "Her Zaman Aktif"}
            };

                model.AzureLoginStatuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Devre Dışı"},
                new SelectListItem {Text = "Her Zaman Aktif"}
            };
                model.YesNo = new List<SelectListItem>
            {
                new SelectListItem {Text = "Hayır"},
                new SelectListItem {Text = "Evet"}
            };

                var ParameterKeys = ParameterService.GetAll();

                model.InvalidLoginAction = ParameterKeys.FirstOrDefault(r => r.Key == "InvalidLoginAction")?.Value;
                model.InvalidLoginLimit = ParameterKeys.FirstOrDefault(r => r.Key == "InvalidLoginLimit")?.Value;
                model.CaptchaStatus = ParameterKeys.FirstOrDefault(r => r.Key == "CaptchaStatus")?.Value;
                model.AzureLoginStatus = ParameterKeys.FirstOrDefault(r => r.Key == "AzureLoginStatus")?.Value;
                model.ShowUserPhotoOnLogin = ParameterKeys.FirstOrDefault(r => r.Key == "ShowUserPhotoOnLogin")?.Value;
                model.LoginBackgroundPhotos = ParameterKeys.FirstOrDefault(r => r.Key == "LoginBackgroundPhotos")?.Value?.Split(';');
                model.SiteHeader = ParameterKeys.FirstOrDefault(r => r.Key == "SiteHeader")?.Value;
                model.OneSignalAppId = ParameterKeys.FirstOrDefault(r => r.Key == "OneSignalAppId")?.Value;
                model.OneSignalApiKey = ParameterKeys.FirstOrDefault(r => r.Key == "OneSignalApiKey")?.Value;
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
        }
    }
}