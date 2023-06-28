using ATM_Machine.Interfaces;

namespace ATM_Machine
{
    class Program
    {
        public static void Main(string[] args)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            IDataBase data = new DataBase(context);
            IValidations validations = new Validations();
            ILoggedInOperations loggedInOperations = new LoggedInOperations(context, validations);

            DisplayBase.DisplayLine("Welcome to the ATM Machine!\n", ConsoleColor.Blue);

            try
            {
                while (true)
                {
                    ATM_Operations atm = new ATM_Operations(validations, data, context, loggedInOperations);
                    atm.Start_ATM();
                }
            }
            catch (Exception e)
            {
                
            }
        }
    }
}