namespace BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NotificationService notificationService = new NotificationService();
            notificationService.TransactionNotification(1, 5000m);

            // Test 2: exakt gräns – notis skickas
            notificationService.TransactionNotification(1, 15000m);

            // Test 3: stor transaktion – notis skickas
            notificationService.TransactionNotification(2, 25000m);
        }
    }
}