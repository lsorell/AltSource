using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLedger_AltSource
{
    /// <summary>
    /// Defines IAccount interface.
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// The username for the account.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// The hashed password string for the account.
        /// </summary>
        string PasswordHash { get; set; }

        /// <summary>
        /// The account balance.
        /// </summary>
        decimal Balance { get; set; }

        /// <summary>
        /// A list containing the transaction history.
        /// </summary>
        IList<string> History { get; set; }
    }
}
