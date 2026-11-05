using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class LoanMenu
    {
        // Private fields to hold the LoanService and LoanRules instances.
        // They are readonly because they should not be modified after initialization.3
        private readonly LoanService _loanService;
        private readonly LoanRules _loanRules;

        // This constructor initializes the LoanMenu with the provided LoanService and LoanRules instances.
        public LoanMenu(LoanService loanService, LoanRules loanRules)
        {
            _loanRules = loanRules;
            _loanService = loanService;
        }

        public void ShowLoanMenu()
        {
            Console.WriteLine("\n--- Lån ---");
            Console.WriteLine("Välj ett alternativ:");
            Console.WriteLine("1. Skapa ett nytt lån");
            Console.WriteLine("2. Visa mina lån");
            Console.WriteLine("0. Tillbaka till Huvudmeny");
        }

    }
}
