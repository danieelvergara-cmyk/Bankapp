using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class NotificationService 
    {
        // recall in main by  NotificationService notificationService = new NotificationService();
        private const decimal LargeTransactionLimit  = 15000.00m;

        public void TransactionNotification(User user, decimal transactionAmount)
        {
         

            
            if (transactionAmount >= LargeTransactionLimit)
            {


                string message = $"Stor transaktion på {transactionAmount} kr skedde {DateTime.Now}";

                SendEmail(user.Email, message);
                SendSms(user.Phone, message);
            }
        }
        
        private void SendEmail(string email, string message)
        {
            Console.WriteLine($"Notification skikat till  {email}: {message}");
        }
        private void  SendSms(string phoneNumber, string message)
        {

            Console.WriteLine($"SMS  till användare {phoneNumber}: {message}");
        }
    }
} 

