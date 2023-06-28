using System;
using System.Globalization;
using ATM_Machine.Interfaces;

namespace ATM_Machine
{
    public class ATM_Operations
	{
		private readonly IValidations? _validations;
		private readonly IDataBase? _db;
        private readonly ApplicationDbContext? _context;
		private readonly ILoggedInOperations? _loggedInOperations;

        public ATM_Operations()
        {
				
        }

        public ATM_Operations(IValidations validations, IDataBase db, ApplicationDbContext context, ILoggedInOperations loggedInOperations)
        {
            this._validations = validations ?? throw new ArgumentNullException(nameof(validations));
            this._db = db ?? throw new ArgumentNullException(nameof(db));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._loggedInOperations = loggedInOperations ?? throw new ArgumentNullException(nameof(loggedInOperations));
        }

        public void Start_ATM()
		{
			Console.Write("- 1 - ");
			DisplayBase.DisplayLine("Log in", ConsoleColor.Blue);
            Console.Write("- 2 - ");
            DisplayBase.DisplayLine("Register", ConsoleColor.Blue);
            Console.Write("- 3 - ");
            DisplayBase.DisplayLine("Print all users", ConsoleColor.Blue);
            Console.Write("- 4 - ");
            DisplayBase.DisplayLine("Exit", ConsoleColor.Blue);

            DisplayBase.Display("\nWhat operation would you like to do? ", ConsoleColor.DarkGray);

            string? operation = Console.ReadLine();

			switch (operation)
			{
				case "1":
					LogIn();
					break;
				case "2":
					Register();
					break;
				case "3":
					_db.PrintAllUsers();
					break;
                case "4":
					DisplayBase.DisplayLine("\nExiting the ATM Machine!", ConsoleColor.Red);
					throw new Exception();
				default:
                    DisplayBase.DisplayLine("\nNot valid operation chosen!\n", ConsoleColor.Red);
                    return;
            }
        }

		public void LogIn()
		{
			List<User> users = _db.GetAllUsers();
			bool loggedIn = false;

            DisplayBase.Display("\nEnter your ID Card: ", ConsoleColor.DarkGray);
			string? inputIdCard = Console.ReadLine();

			foreach (User user in users)
			{
                if (inputIdCard == user.CardID)
				{
                    DisplayBase.Display("Enter the PIN: ", ConsoleColor.DarkGray);
                    string PIN = Console.ReadLine();

                    if (PIN == user.PIN)
					{
                        DisplayBase.DisplayLine("\nLog in successfully!\n", ConsoleColor.Green);
						loggedIn = true;
						_loggedInOperations.ChooseOperation(user);
					}
					else
					{
                        DisplayBase.DisplayLine("\nInvalid PIN!\n", ConsoleColor.Red);
						return;
                    }
                }
			}

            if (loggedIn)
            {
                DisplayBase.DisplayLine("\nLogging out...\n", ConsoleColor.Red);
                return;
            }
            else
            {
                DisplayBase.DisplayLine("\nInvalid Card ID!\n", ConsoleColor.Red);
                return;
            }
        }

		public void Register()
		{
			string? lastName = string.Empty;
			string? firstName = string.Empty;
			string? PIN = string.Empty, PIN1 = string.Empty, PIN2 = string.Empty;
			string? x = string.Empty;
            string? cardID = "99";

            DisplayBase.DisplayLine("\nPlease register below! For cancelation submit X!\n", ConsoleColor.DarkGray);

            DisplayBase.Display("Enter your first name: ", ConsoleColor.DarkGray);
            x = Console.ReadLine();
            if (_validations.ValidateName(x))
            {
                firstName = x;
            }
            else
            {
                return;
            }

            DisplayBase.Display("Enter your last name: ", ConsoleColor.DarkGray);
			x = Console.ReadLine();

			if (_validations.ValidateName(x))
			{
				lastName = x;
			}
			else
			{
				return;
			}

			DisplayBase.Display("Enter your PIN - numbers only, format (XXXX) : ", ConsoleColor.DarkGray);
			PIN1 = Console.ReadLine();

			if (_validations.ValidatePIN(PIN1))
			{
				DisplayBase.Display("Enter the PIN again: ", ConsoleColor.DarkGray);
				PIN2 = Console.ReadLine();

				if (PIN1 == PIN2)
				{
					PIN = PIN1;
				}
				else
				{
                    DisplayBase.DisplayLine("\nPINs do not match!\n", ConsoleColor.Red);
                    return;
                }
            }
			else 
			{ 
				return; 
			}	

			Random rand = new Random();
			List<string> userCardIDs = new List<string>();
            List<User> users = _db.GetAllUsers();

			userCardIDs = _db.GetAllCardIDs();

            cardID = _validations.GetNewCardID();

            while (userCardIDs.Contains(cardID))
			{
				cardID = _validations.GetNewCardID();
            }

            DisplayBase.Display($"Your Card ID is: {cardID}.\n", ConsoleColor.DarkGray);

            User user = new User(firstName, lastName, PIN, cardID);
			_db.Add(user);

			DisplayBase.DisplayLine($"\nYou created the account succesfully. Welcome, {firstName}!\n", ConsoleColor.Green);
        }
	}
}