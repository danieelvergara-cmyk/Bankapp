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
    }
}
