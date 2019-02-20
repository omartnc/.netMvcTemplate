using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using log4net;

namespace EventManagementSystem.Web.Helpers
{
    public static class MailSendHelper
    {
        public static readonly ILog Logger = LogManager.GetLogger(System.Environment.UserName);
        public static bool SendEmail(MailModel model)
        {
            var info = false;

            try
            {
                var fromAddress = new MailAddress("bilgilendirme@EventManagementSystem.com", "İnşaat Hesabı");
                var toAddress = new MailAddress(model.MailAddress);
                string subject = model.Subject;
                SmtpClient smtp = new SmtpClient("smtp.yandex.ru",587);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress.Address, "asdfghQW33.");
                smtp.EnableSsl = true;
                var message = new MailMessage(fromAddress, toAddress);
                message.Subject = subject;
                message.IsBodyHtml = true;

                switch (model.MailTemplate)
                {
                    case MailTemplate.Welcome:
                        message.Body = TemplateHelper.RenderTemplate("~/Views/_MailTemplate/WelcomeTemplate.cshtml", model);
                        break;
                    case MailTemplate.ForgottenPassword:
                        message.Body = TemplateHelper.RenderTemplate("~/Views/_MailTemplate/ForgottenTemplate.cshtml", model);
                        break;
                }
                smtp.Send(message);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return false;
            }
        }
    }

    public class MailModel
    {
        public string Guid { get; set; }
        public string FullName { get; set; }
        public int UserID { get; set; }
        public string MailAddress { get; set; }
        public string Subject { get; set; }
        public MailTemplate MailTemplate { get; set; }
    }

    public enum MailTemplate
    {

        /// <summary>
        /// Onaylamak
        /// </summary>
        [Display(Name = "Onaylamak")]
        Confirm = 0,
        /// <summary>
        /// Hoşgeldin
        /// </summary>
        [Display(Name = "Hoşgeldin")]
        Welcome = 1,
        /// <summary>
        /// Şifremi Unuttum
        /// </summary>
        [Display(Name = "Şifremi Unuttum")]
        ForgottenPassword = 2,
        /// <summary>
        /// Şifre Yenile
        /// </summary>
        [Display(Name = "Şifre Yenile")]
        ResetPassword = 3,
        /// <summary>
        /// Engelleme Bildirimi
        /// </summary>
        [Display(Name = "Engelleme Bildirimi")]
        NotificationForBlocked = 4,
        /// <summary>
        /// Kurulum
        /// </summary>
        [Display(Name = "Kurulum")]
        Setup = 5
    }
}