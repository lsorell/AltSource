using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLedger_AltSource
{
    /// <summary>
    /// Defines interface for a view.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Method to bind controller to view.
        /// </summary>
        /// <param name="ac">The controller to bind the the view.</param>
        void SetController(AccountController ac);

        /// <summary>
        /// Method to display create account or login to existing account.
        /// </summary>
        void CreateOrLogin();

        /// <summary>
        /// Method to display the menu.
        /// </summary>
        void Menu();

        /// <summary>
        /// Method to show transaction history.
        /// </summary>
        /// <param name="hist">List containing the transaction history of an account.</param>
        void ShowHistory(IList<string> hist);

        /// <summary>
        /// Method to display the balance of an account.
        /// </summary>
        /// <param name="bal"></param>
        void ShowBalance(decimal bal);

        /// <summary>
        /// Method to display a deposit action and the account balance afterward.
        /// </summary>
        /// <param name="dpst">The deposit amount.</param>
        void ShowDeposit(decimal dpst);

        /// <summary>
        /// Method to display a withdrawl action and the account balance afterward.
        /// </summary>
        /// <param name="wd">The withdrawal amount.</param>
        void ShowWithdrawal(decimal wd);

        /// <summary>
        /// Method to display a logout action.
        /// </summary>
        void ShowLogout();
    }
}
