using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BankingLedger_AltSource
{
    /// <summary>
    /// The controller for a banking ledger system.
    /// </summary>
    public class AccountController
    {
        /// <summary>
        /// A list containing all the accounts in the system.
        /// </summary>
        private IList<IAccount> _accounts;

        /// <summary>
        /// The number of times the salt and password will be run through the hashing algorithm.
        /// </summary>
        private int numberOfHashes = 10000;

        /// <summary>
        /// Controller constructor.
        /// </summary>
        /// <param name="view">The view for the controller to call.</param>
        /// <param name="accounts">A list of the accounts in the system.</param>
        public AccountController(IList<IAccount> accounts)
        {            
            _accounts = accounts;
        }

        /// <summary>
        /// Adds a new account to the system.
        /// The starting balance will be 0 and have no transaction history.
        /// </summary>
        /// <param name="username">The username to create the account with.</param>
        /// <param name="password">The password to create the account with.</param>
        /// <returns>Whether or not the given username already exists.</returns>
        public bool CreateNewAccount(string username, string password)
        {
            foreach(IAccount a in _accounts)
            {
                if(a.Username == username)
                {
                    return false;
                }
            }            
            password = HashPassword(password);
            IAccount acc = new Account(username, password, 0, new List<string>());
            _accounts.Add(acc);
            return true;
        }

        /// <summary>
        /// Checks to see if user credentials match with the account with the same username.
        /// </summary>
        /// <param name="username">The username of the account trying to be accessed.</param>
        /// <param name="pass">The password of the account trying to be accessed.</param>
        /// <returns>Whether the username and password match the account.</returns>
        public bool Login(string username, string pass)
        {
            return ComparePasswordHash(pass, _accounts[FindAccountIndex(username)]);
        }

        /// <summary>
        /// Checks the balance of the account with the given username.
        /// </summary>
        /// <param name="username">The username of the account to be checked.</param>
        /// <returns>The balance of the account.</returns>
        public decimal CheckBalance(string username)
        {
            return _accounts[FindAccountIndex(username)].Balance;            
        }

        /// <summary>
        /// Deposits the given amount into the account with the given username.
        /// </summary>
        /// <param name="username">The username of the account to be deposited.</param>
        /// <param name="amount">The deposit amount.</param>
        /// <returns>The balance of the account after deposit.</returns>
        public decimal Deposit(string username, decimal amount)
        {
            int i = FindAccountIndex(username);
            _accounts[i].Balance += amount;
            decimal bal = _accounts[i].Balance;            
            _accounts[i].History.Add(String.Format("{0} - Deposit of {1:C2}; New balance: {2:C2}",DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss").ToString(), amount, bal));
            return bal;
        }

        /// <summary>
        /// Withdrawals the given amount from the account with the given username if there are enough funds.
        /// </summary>
        /// <param name="username">The username of the account to be withdrawn from.</param>
        /// <param name="amount">The amount to withdraw.</param>
        /// <returns>The balance of the account after withdrawal.</returns>
        public decimal Withdrawal(string username, decimal amount)
        {
            int i = FindAccountIndex(username);            
            if(!(_accounts[i].Balance - amount >= 0))
            {
                throw new ArgumentException("There are not enough funds to withdraw that amount.");
            }
            _accounts[i].Balance -= amount;
            decimal bal = _accounts[i].Balance;
            _accounts[i].History.Add(String.Format("{0} - Withdrawal of {1:C2}; New balance: {2:C2}", DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss").ToString(), amount, bal));
            return bal;
        }

        /// <summary>
        /// Gets the history of the account with the given username.
        /// </summary>
        /// <param name="username">The username of the account.</param>
        /// <returns>The list of strings of transaction history.</returns>
        public IList<string> History(string username)
        {
            return _accounts[FindAccountIndex(username)].History;
        }

        /// <summary>
        /// Finds the index of the account that has the given username from the active account list.
        /// </summary>
        /// <param name="username">The username of the account.</param>
        /// <returns>The index of the account with the given username.</returns>
        private int FindAccountIndex(string username)
        {
            int i = 0;
            IAccount acc = null;
            foreach (IAccount a in _accounts)
            {
                if (a.Username == username)
                {
                    acc = a;
                    break;
                }
                i++;
            }
            if (acc == null)
            {
                throw new ArgumentException("No account with that username exists.");
            }
            return i;
        }

        /// <summary>
        /// Hashes the given password.
        /// </summary>
        /// <param name="pass">The password to be hashed.</param>
        /// <param name="passSalt">The salt used to hash the password.</param>
        /// <returns>The sale + password hash string.</returns>
        private string HashPassword(string pass)
        {
            //Following guide from: https://medium.com/@mehanix/lets-talk-security-salted-password-hashing-in-c-5460be5c3aae
                        
            byte[] salt;
            //Generate salt
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            //Hash and salt password
            var rfc = new Rfc2898DeriveBytes(pass, salt, numberOfHashes);
            //Place string in byte array
            byte[] hash = rfc.GetBytes(20);
            //Byte array to store salt + password
            byte[] hashBytes = new byte[36];
            //Place salt and hash in repective places
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);                        
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Compares the entered password with the password stored in the given account.
        /// </summary>
        /// <param name="pass">The entered password.</param>
        /// <param name="acc">The account to compare to.</param>
        /// <returns>Whether the entered password when hashed equals the stored passwordhash of the account.</returns>
        private bool ComparePasswordHash(string pass, IAccount acc)
        {
            string passHash = acc.PasswordHash;
            byte[] hashBytes = Convert.FromBase64String(passHash);
            //Seperate salt from rest of string
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var rfc = new Rfc2898DeriveBytes(pass, salt, numberOfHashes);
            byte[] hash = rfc.GetBytes(20);
            //Compare byte by byte
            //Start at index 16 because salt stored in 0-15
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
