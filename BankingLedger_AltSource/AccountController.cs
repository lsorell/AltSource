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
        /// The view that the controller can access.
        /// </summary>
        private IView _view;
        /// <summary>
        /// A list containing all the accounts in the system.
        /// </summary>
        private IList<IAccount> _accounts;
        /// <summary>
        /// The account that is currently logged in.
        /// </summary>
        private IAccount _activeAccount;

        /// <summary>
        /// The number of times the salt and password will be run through the hashing algorithm.
        /// </summary>
        private int numberOfHashes = 10000;

        /// <summary>
        /// Controller constructor.
        /// </summary>
        /// <param name="view">The view for the controller to call.</param>
        /// <param name="accounts">A list of the accounts in the system.</param>
        AccountController(IView view, IList<IAccount> accounts)
        {
            _view = view;
            _accounts = accounts;
        }

        /// <summary>
        /// Adds a new account to the system.
        /// The starting balance will be 0 and have no transaction history.
        /// </summary>
        /// <param name="username">The username to create the account with.</param>
        /// <param name="password">The password to create the account with.</param>
        public void CreateNewAccount(string username, string password)
        {
            foreach(IAccount a in _accounts)
            {
                if(a.Username == username)
                {
                    throw new ArgumentException("Another account already exist with the same username.");
                }
            }            
            password = HashPassword(password);
            IAccount acc = new Account(username, password, 0, new List<string>());
            _accounts.Add(acc);    
        }

        /// <summary>
        /// Checks to see if user credentials match with the account with the same username.
        /// </summary>
        /// <param name="username">The username of the account trying to be accessed.</param>
        /// <param name="pass">The password of the account trying to be accessed.</param>
        /// <returns>Whether the username and password match the account.</returns>
        public bool Login(string username, string pass)
        {
            IAccount acc = null; 
            foreach(IAccount a in _accounts)
            {
                if(a.Username == username)
                {
                    acc = a;
                    break;
                }
            }
            if(acc == null)
            {
                throw new ArgumentException("No account with that username exists.");
            }
            return ComparePasswordHash(pass, acc);
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
            var rfc = new Rfc2898DeriveBytes(passHash, salt, numberOfHashes);
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
