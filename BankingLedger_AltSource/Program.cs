using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLedger_AltSource
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountController controller = new AccountController(new List<IAccount>());
            ConsoleView view = new ConsoleView();
            view.SetController(controller);
            view.Run();
        }
    }
}
