using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using EventManagementSystem.Entity.Model.Authorization;
using EventManagementSystem.Entity.Model.Common;
using EventManagementSystem.Helper;
using EventManagementSystem.Service;
using EventManagementSystem.Web.Helpers;
using EventManagementSystem.Web.Models;

namespace EventManagementSystem.Web.Controllers
{

    [RoutePrefix("yetkilendirme")]
    public class AuthorizationController : BaseController
    {
        public AuthorizationController()
        {

        }

        [AuthorizeUser(Action = "AuthorizationList")]
        [Route("yetki-grubu/liste")]
        [HttpGet]
        public ActionResult AuthorizationList()
        {
            try
            {

                var model = new AuthorizationModel()
                {
                    Roles = RoleService.GetAll()
                };
                return View(model);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return View(new AuthorizationModel());
            }
        }

        [AuthorizeUser(Action = "UserList")]
        [Route("kullanicilar/liste/{userID?}")]
        [HttpGet]
        public ActionResult UserList(int? userID = null)
        {

            try
            {
                var model = new AuthorizationModel()
                {
                    Users = new List<User>() //UserService.GetAll()
                };
                return View(model);

            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return View(new AuthorizationModel());
            }
        }

        [AuthorizeUser(Action = "UserList")]
        [Route("user-query")]
        [HttpPost]
        public JsonResult UserListQuery(DataTablesModel.DataTableAjaxPostModel model)
        {
            try
            {

                var data = UserService.GetAllForDatatables(model);
                return Json(data);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return Json("");
            }
        }

        [AuthorizeUser(Action = "UserEdit")]
        [Route("kullanicilar/duzenle")]
        [HttpGet]
        public ActionResult UserEdit(int UserId)
        {


            var random = new Random();
            var accountModel = new AccountModel();

            try
            {
                if (UserId != 0)
                {
                    GenarateCommonModel(accountModel);
                    accountModel.LoginBackgroundPhoto = accountModel.LoginBackgroundPhotos?[random.Next(accountModel.LoginBackgroundPhotos.Length)];

                    var user = UserService.Get(UserId);

                    accountModel.User = user;
                }
                else
                {
                    GenarateCommonModel(accountModel);
                    accountModel.LoginBackgroundPhoto = accountModel.LoginBackgroundPhotos?[random.Next(accountModel.LoginBackgroundPhotos.Length)];


                    accountModel.User = new User();
                    accountModel.User.MailPermission = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }



            return View(accountModel);
        }

        [AuthorizeUser(Action = "UserEdit")]
        [Route("kullanicilar/duzenle")]
        [HttpPost]
        public ActionResult UserEdit(int UserId, AccountModel accountModel)
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
                    if (UserId == 0)
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
                            registerUser.Adress = accountModel.User.Adress;
                            registerUser.PhoneNumber = PhoneMaskHelper.FormatPhoneNumber(accountModel.User.PhoneNumber);
                            registerUser.MailPermission = accountModel.User.MailPermission;
                            registerUser.IsActive = accountModel.User.IsActive;
                            registerUser.IsAdmin = accountModel.User.IsAdmin;


                            registerUser.CompanyName = accountModel.User.CompanyName;

                            registerUser.TCKN = accountModel.User.TCKN;



                            registerUser.IsTestUser = false;
                            registerUser.MailConfirmation = true;
                            UserService.Add(registerUser);




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

                            return View(accountModel);
                        }
                        else
                        {
                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "Bu E-Mail " + accountModel.SiteHeader + " Portal'ine daha önceden kayıt yaptırmıştır.";
                            return View(accountModel);
                        }
                    }
                    else
                    {

                        var authUser = UserService.Get(UserId);
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
                            authUser.Password = md5Helper.GetMD5_2(accountModel.User.Password);
                            authUser.PhotoPath = accountModel.User.PhotoPath;
                            authUser.Adress = accountModel.User.Adress;
                            authUser.PhoneNumber = PhoneMaskHelper.FormatPhoneNumber(accountModel.User.PhoneNumber);


                            authUser.CompanyName = accountModel.User.CompanyName;

                            authUser.TCKN = accountModel.User.TCKN;


                            authUser.MailPermission = accountModel.User.MailPermission;
                            authUser.IsActive = accountModel.User.IsActive;
                            authUser.IsAdmin = accountModel.User.IsAdmin;
                            authUser.IsTestUser = false;
                            authUser.MailConfirmation = true;
                            authUser.Email = accountModel.User.Email;

                            UserService.Update(authUser);
                            return View(accountModel);
                        }
                        else
                        {
                            accountModel.HasError = true;
                            accountModel.ErrorMessage = "Hata Oluştu.";
                            return View(accountModel);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                    return View(accountModel);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return View(accountModel);
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
        #region Delete
        [AuthorizeUser(Action = "UserEdit")]
        [Route("sil/{id}")]
        [HttpGet]
        public bool UserDelete(int id)
        {
            try
            {
                UserService.Delete(id);
                Logger.Info(CurrentUser.Id + " idli kullanıcı " + id + " idli User silindi.");

            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);

            }
            return true;
        }

        #endregion
        [AuthorizeUser(Action = "AuthorizationEdit")]
        [Route("yetki-grubu/duzenle/{authorizationId}")]
        [HttpGet]
        public ActionResult AuthorizationEdit(int authorizationId)
        {

            try
            {
                var model = new AuthorizationModel
                {
                    Role = authorizationId == 0 ? new Role() : RoleService.Get(authorizationId)
                };
                return View(model);

            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return View(new AuthorizationModel());
            }
        }

        [AuthorizeUser(Action = "AuthorizationEdit")]
        [Route("yetki-grubu/duzenle/{authorizationId}")]
        [HttpPost]
        public ActionResult AuthorizationEdit(int authorizationId, AuthorizationModel authorizationModel)
        {

            try
            {
                if (authorizationId == 0)
                {
                    var item = new Role
                    {
                        Name = authorizationModel.Role.Name,
                        Description = authorizationModel.Role.Description,
                    };
                    RoleService.Add(item);
                    Logger.Info(CurrentUser.Id + " idli kullanıcı " + item.Id + " idli yetki grubunu ekledi");
                }
                else
                {
                    var item = RoleService.Get(authorizationId);
                    item.Name = authorizationModel.Role.Name;
                    item.Description = authorizationModel.Role.Description;
                    RoleService.Update(item);
                    Logger.Info(CurrentUser.Id + " idli kullanıcı " + item.Id + " idli yetki grubunu güncelledi");
                }

            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return RedirectToAction("AuthorizationList");
        }

        [AuthorizeUser(Action = "AuthorizationEdit")]
        [Route("yetki-grubu-yetkileri/duzenle/{authorizationId}")]
        [HttpGet]
        public ActionResult AuthorizationItemEdit(int authorizationId)
        {
            try
            {
                var model = new AuthorizationModel
                {
                    RoleModules = RoleModuleService.GetAll(authorizationId).Select(r => r.ModuleId.Value).ToList(),
                    Role = RoleService.Get(authorizationId),
                    Modules = ModuleService.GetAll()
                };
                return View(model);

            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return View(new AuthorizationModel());
            }
        }

        [AuthorizeUser(Action = "AuthorizationEdit")]
        [Route("yetki-grubu-yetkileri/duzenle/{authorizationId}")]
        [HttpPost]
        public ActionResult AuthorizationItemEdit(int authorizationId, AuthorizationModel authorizationModel)
        {
            try
            {
                var authorizationItems = RoleModuleService.GetAll(authorizationId);
                foreach (var authorizationItem in authorizationItems)
                {
                    RoleModuleService.DeleteReal(authorizationItem.Id);
                }

                var newItems = authorizationModel.RoleModules?.ToList();
                if (newItems != null)
                    foreach (var item in newItems)
                    {
                        var a = new RoleModule
                        {
                            RoleId = authorizationId,
                            ModuleId = item
                        };
                        RoleModuleService.Add(a);
                    }
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return RedirectToAction("AuthorizationList");
        }

        [AuthorizeUser(Action = "UserEdit")]
        [Route("kullanicilar/yetki-duzenle/{userId}")]
        [HttpGet]
        public ActionResult UserAuthorizationEdit(int userId)
        {

            try
            {

                var model = new AuthorizationModel
                {
                    Roles = RoleService.GetAll(),
                    Modules = ModuleService.GetAll(),
                    User = UserService.Get(userId)
                };
                return View(model);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return View(new AuthorizationModel());
            }
        }


        [AuthorizeUser(Action = "UserEdit")]
        [Route("kullanicilar/yetki-duzenle/{userId}")]
        [HttpPost]
        public ActionResult UserAuthorizationEdit(int userId, AuthorizationModel authorizationModel)
        {

            try
            {
                var oldUserRoles = UserRoleService.GetAll(userId);
                foreach (var authorization in oldUserRoles)
                {
                    UserRoleService.Delete(authorization.Id);
                }

                var userModules = authorizationModel.UserModules?.ToList();
                if (userModules != null)
                    foreach (var item in userModules)
                    {
                        var a = new UserRole
                        {
                            ModuleId = item,
                            UserId = userId
                        };
                        UserRoleService.Add(a);
                    }

                var userRoles = authorizationModel.UserRoles?.ToList();
                if (userRoles != null)
                    foreach (var item in userRoles)
                    {
                        var a = new UserRole
                        {
                            RoleId = item,
                            UserId = userId
                        };
                        UserRoleService.Add(a);
                    }

            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return RedirectToAction("UserList");
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