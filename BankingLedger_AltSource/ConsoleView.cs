using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLedger_AltSource
{
    public enum State { CreateOrLogin, Menu, Deposit, Withdrawal, Balance, History, Logout, Exit}

    /// <summary>
    /// The view for a bank ledger console program.
    /// </summary>
    public class ConsoleView
    {
        /// <summary>
        /// The controller for the view to send commands to.
        /// </summary>
        AccountController _controller;
        /// <summary>
        /// The state of the system.
        /// </summary>
        State _state = State.CreateOrLogin;
        /// <summary>
        /// The username of the current user.
        /// </summary>
        string _username;

        /// <summary>
        /// Sets the controller for the view.
        /// </summary>
        /// <param name="ac">The account controller.</param>
        public void SetController(AccountController ac)
        {
            _controller = ac;
        }

        /// <summary>
        /// Manages the state transitions and calls the appropriate methods for the display.
        /// </summary>
        public void Run()
        {
            Console.WriteLine("Welcome to Lane's bank!");
            while(_state != State.Exit)
            {
                switch (_state)
                {
                    case State.CreateOrLogin:
                        CreateOrLogin();
                        break;
                    case State.Menu:
                        Menu();
                        break;
                    case State.Deposit:
                        Deposit();
                        break;
                    case State.Withdrawal:
                        Withdrawal();
                        break;
                    case State.Balance:
                        Balance();
                        break;
                    case State.History:
                        History();
                        break;
                    case State.Logout:
                        Logout();
                        break;
                }
            }
        }

        /// <summary>
        /// Handles display for creating or logging into an account.
        /// </summary>
        private void CreateOrLogin()
        {            
            Console.Write("(C)reate a new account or (l)ogin: ");            
            char res = Console.ReadLine().ToLower()[0];
            while (res != 'c' && res != 'l')
            {
                Console.WriteLine("Invalid response.");
                Console.Write("Enter 'c' to create a new account or 'l' to login to an existing account:");
                res = Console.ReadLine().ToLower()[0];
            }
            Console.WriteLine("Please enter the following information.");
            Console.Write("Username:");
            _username = Console.ReadLine().Trim();
            Console.Write("Password:");
            string pass = Console.ReadLine().Trim();
            if (res == 'c')
            {
                while (!_controller.CreateNewAccount(_username, pass))
                {
                    Console.WriteLine("An account with that username already exists. Try a different username.");
                    Console.Write("Username:");
                    _username = Console.ReadLine().Trim();
                    Console.Write("Password:");
                    pass = Console.ReadLine().Trim();
                }
                Console.WriteLine("Account creation successful!");
            }
            else if (res == 'l')
            {
                try
                {
                    while (!_controller.Login(_username, pass))
                    {
                        Console.WriteLine("That password does not match the account. Try again.");
                        Console.Write("Username:");
                        _username = Console.ReadLine().Trim();
                        Console.Write("Password:");
                        pass = Console.ReadLine().Trim();
                    }
                    Console.WriteLine("Login successful!");
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }                                   
            _state = State.Menu;
        }

        /// <summary>
        /// Handles display for the menu.
        /// </summary>
        private void Menu()
        {
            Console.WriteLine("Select a number below:" +
                "\n1 - Deposit" +
                "\n2 - Withdraw" +
                "\n3 - Check balance" +
                "\n4 - See transaction history" +
                "\n5 - Logout");
            char res = Console.ReadLine().Trim()[0];
            while(res != '1' && res != '2' && res != '3' && res != '4' && res != '5')
            {
                Console.WriteLine("Invalid response.");
                Console.Write("Enter the number associated with your menu selection:");
                res = Console.ReadLine().ToLower()[0];
            }
            switch (res)
            {
                case '1':
                    _state = State.Deposit;
                    break;
                case '2':
                    _state = State.Withdrawal;
                    break;
                case '3':
                    _state = State.Balance;
                    break;
                case '4':
                    _state = State.History;
                    break;
                case '5':
                    _state = State.Logout;
                    break;
            }
        }

        /// <summary>
        /// Handles the display for a deposit.
        /// </summary>
        private void Deposit()
        {
            Console.Write("Enter the deposit amount: $");
            decimal res;
            while (!decimal.TryParse(Console.ReadLine(), out res))
            {
                Console.WriteLine("Invalid amount. Try again.");
                Console.Write("Enter the deposit amount: $");
            }
            res = Math.Round(res, 2);
            decimal bal = _controller.Deposit(_username, res);
            Console.WriteLine("You deposited {0:C2}. Your account balance is now {1:C2}", res, bal);
            _state = State.Menu;
        }

        /// <summary>
        /// Handles the display for a withdrawal.
        /// </summary>
        private void Withdrawal()
        {
            Console.Write("Enter the withdrawal amount: $");
            decimal res;
            while (!decimal.TryParse(Console.ReadLine(), out res))
            {
                Console.WriteLine("Invalid amount. Try again.");
                Console.Write("Enter the deposit amount: $");
            }
            res = Math.Round(res, 2);
            decimal bal = 0;
            try
            {
                bal = _controller.Withdrawal(_username, res);
                Console.WriteLine("You withdrew {0:C2}. Your account balance is now {1:C2}", res, bal);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            _state = State.Menu;
        }

        /// <summary>
        /// Handles the display for a balance check.
        /// </summary>
        private void Balance()
        {
            Console.WriteLine("Your account balance is {0:C2}", _controller.CheckBalance(_username));
            _state = State.Menu;
        }

        /// <summary>
        /// Handles the display for transaction history report.
        /// </summary>
        private void History()
        {
            IList<string> hist = _controller.History(_username);
            Console.WriteLine("-----Transanction history for {0}-----", _username);
            foreach(string s in hist)
            {
                Console.WriteLine(s);
            }
            _state = State.Menu;
        }

        /// <summary>
        /// Handles the display for logout.
        /// </summary>
        private void Logout()
        {
            Console.WriteLine("You have successfully logged out.");
            _username = null;
            Console.Write("Do you with to exit the program (y/n)? ");
            char res = Console.ReadLine().ToLower().Trim()[0];
            while (res != 'y' && res != 'n')
            {
                Console.WriteLine("Invalid response.");
                Console.Write("Type 'y' if you want to exit the program and 'n' if you want to go back to login: ");
                res = Console.ReadLine().ToLower().Trim()[0];
            }
            if (res == 'n')
            {
                _state = State.CreateOrLogin;
            }
            else
            {
                _state = State.Exit;
            }
        }
    }
}
