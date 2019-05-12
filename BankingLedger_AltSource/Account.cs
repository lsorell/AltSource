using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLedger_AltSource
{
    /// <summary>
    /// Defines class for a bank account.
    /// </summary>
    public class Account : IAccount
    {
        /// <summary>
        /// The username for the account.
        /// </summary>
        private string _username;
        /// <summary>
        /// The hashed password for the account.
        /// </summary>
        private string _passwordhash;
        /// <summary>
        /// The account balance.
        /// </summary>
        private decimal _balance = 0;
        /// <summary>
        /// A list of the transaction history.
        /// </summary>
        private IList<string> _history;

        /// <summary>
        /// Account constructor.
        /// </summary>
        /// <param name="user">The account username.</param>
        /// <param name="pass">The hashed password for the account.</param>
        /// <param name="salt">The salt used to hash the password.</param>
        /// <param name="bal">The account starting balance.</param>
        /// <param name="hist">The transaction history for the account.</param>
        public Account(string user, string pass, decimal bal, IList<string> hist)
        {
            _username = user;
            _passwordhash = pass;            
            _balance = bal;
            _history = hist;
        }
        
        /// <summary>
        /// Accessor for _username.
        /// </summary>
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        /// <summary>
        /// Accessor for _passwordhash.
        /// </summary>
        public string PasswordHash
        {
            get
            {
                return _passwordhash;
            }
            set
            {
                _passwordhash = value;
            }
        }

        /// <summary>
        /// Accessor for _balance.
        /// </summary>
        public decimal Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                _balance = value;
            }
        }

        /// <summary>
        /// Accessor for _history.
        /// </summary>
        public IList<string> History
        {
            get
            {
                return _history;
            }
            set
            {
                _history = value;
            }
        }
    }
}
