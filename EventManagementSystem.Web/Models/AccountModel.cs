using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagementSystem.Web.Models
{
    public class AccountModel : BaseModel
    {
        public bool IsSaved { get; set; }
        public bool HasError { get; set; }
        public bool ConfirmationError { get; set; }
        public string ErrorMessage { get; set; }
        public string CaptchaStatus { get; set; }
        public bool IsPasswordSent { get; set; }
        public string OneSignalAppId { get; set; }
        public string OneSignalApiKey { get; set; }
        public string AzureLoginStatus { get; set; }
        public string InvalidLoginLimit { get; set; }
        public string InvalidLoginAction { get; set; }
        public string ShowUserPhotoOnLogin { get; set; }
        public List<SelectListItem> InvalidLoginActions { get; set; }
        public List<SelectListItem> CaptchaStatuses { get; set; }
        public List<SelectListItem> AzureLoginStatuses { get; set; }
        public List<SelectListItem> YesNo { get; set; }
        public int UserId { get; set; }
        public string[] LoginBackgroundPhotos { get; set; }
        public string LoginBackgroundPhoto { get; set; }
        public string SiteHeader { get; set; }
        /// <summary>
        /// seçili ülke id
        /// </summary>
        public int SelectedCountryId { get; set; }
        /// <summary>
        /// seçili il id
        /// </summary>
        public int SelectedProvinceId { get; set; }
        /// <summary>
        /// seçili ilçe id
        /// </summary>
        public int SelectedDistrictId { get; set; }
        /// <summary>
        /// seçili lisans tip id
        /// </summary>
        public int SelectedLicenseTypeId { get; set; }
        /// <summary>
        /// indirim kodu
        /// </summary>
        public string DiscountCode { get; set; }
        /// <summary>
        /// Şifre Yeni
        /// </summary>
        public string PasswordNew { get; set; }
        /// <summary>
        /// Şifre Tekrar
        /// </summary>
        public string PasswordConfirmation { get; set; }
        public int UserLastLicenseDay { get; internal set; }
        public int UserProjectConut { get; internal set; }
        public int UserRemainingProjectConut { get; internal set; }
    }
}