using System;






namespace BankApp
{
    public class NotificationService 
    {
        // recall in main by  NotificationService notificationService = new NotificationService();
        private const decimal LargeTransactionLimit  = 15000.00m;
        private readonly ExchangeRate exchangeRate;

        // Inject the ExchangeRate service 
        public NotificationService(ExchangeRate exchangeRate)
        {
            this.exchangeRate = exchangeRate;
        }
        public void TransactionNotification(User user, decimal amount, Currency currency)
        {

            decimal amountInsek = exchangeRate.Convert(amount, currency, Currency.SEK);
            
            if (amountInsek >= LargeTransactionLimit)
            {
                

                string message = $"Stor transaktion på {amountInsek:F2}  kr skedde {DateTime.Now}";

                SendEmail(user.Email, message);
                SendSms(user.Phone, message);
            }
        }
        
        private void SendEmail(string email, string message)
        {
            Console.WriteLine($"Notifikation skickad till {email}: {message}");
        }
        private void  SendSms(string phoneNumber, string message)
        {

            Console.WriteLine($"SMS  till användare {phoneNumber}: {message}");
        }
    }
} 

