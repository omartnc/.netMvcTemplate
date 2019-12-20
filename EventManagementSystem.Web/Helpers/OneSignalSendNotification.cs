using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventManagementSystem.Service;
using OneSignal.CSharp.SDK;
using OneSignal.CSharp.SDK.Resources;
using OneSignal.CSharp.SDK.Resources.Notifications;

namespace EventManagementSystem.Web.Helpers
{
    public class OneSignalSendNotification
    {
        private string AppId;
        private string ApiKey;
        public OneSignalSendNotification()
        {
            ApiKey = new ParameterService().GetValueByKey("OneSignalApiKey");
            AppId = new ParameterService().GetValueByKey("OneSignalAppId");
        }
        public string Send(string Message, string Url, List<int> FilterUserIds, DateTime? StartingDate, DateTime? EndingDate, string title = "Sistem Portal")
        {
            var client = new OneSignalClient(ApiKey);

            var options = new NotificationCreateOptions { Filters = new List<INotificationFilter>(), AppId = Guid.Parse(AppId) };
            

            options.SendAfter = StartingDate;
            if (EndingDate != null) options.TimeToLive = EndingDate.Value.Subtract(DateTime.Now).Days;


            var index = 0;
            foreach (var FilterUserId in FilterUserIds)
            {

                index++;
                options.Filters.Add(new NotificationFilterField
                {
                    Field = NotificationFilterFieldTypeEnum.Tag,
                    Key = "Id",
                    Relation = "=",
                    Value = FilterUserId.ToString()
                });


                if (index != FilterUserIds.Count)
                {
                    options.Filters.Add(new NotificationFilterOperator
                    {
                        Operator = "OR"
                    });
                }

            }

            options.Headings.Add(LanguageCodes.English, title);
            options.Contents.Add(LanguageCodes.English, Message);
            options.Url = Url;
            var returnNotificationId = client.Notifications.Create(options);

            return returnNotificationId.Id;
        }

        public void Delete(string notificationId)
        {
            var client = new OneSignalClient(ApiKey);
            var options = new NotificationCancelOptions {Id = notificationId,AppId = AppId};
            client.Notifications.Cancel(options);
        }
    }
}