using System;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;

namespace GamerRater.Application.Services
{
    public static class GrToast
    {
        
        public static void SmallToast(string msg)
        {
            var visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = msg
                        },
                    },
                }
            };
            var toastContent = new ToastContent() { Visual = visual };
            var toast = new ToastNotification(toastContent.GetXml()) { ExpirationTime = DateTime.Now.AddSeconds(2) };
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
        
    }
}
