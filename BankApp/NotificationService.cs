using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class NotificationService 
    {
        // recall in main by  NotificationService notificationService = new NotificationService();
        private decimal LargeTransactionLimit { get; set; } = 15000.00m;

        public void TransactionNotification(int userId, decimal transactionAmount)
        {
            int UserId = userId;
            // ..... = transactionAmount;

            //If true, it stops the notification method from sending the notification.
            if (transactionAmount < LargeTransactionLimit)
            {
                return;
            }


            string message = $"Stor transaktion på {transactionAmount} kr skedde {DateTime.Now}";

            SendEmail(userId, message);
            SendSms(userId, message);
        }
        
        private void SendEmail(int userId, string message)
        {
            Console.WriteLine($"Email till användare {userId}: {message}");
        }
        private void  SendSms(int userId, string message)
        {
            Console.WriteLine($"SMS  till användare {userId}: {message}");
        }
    }
} 

