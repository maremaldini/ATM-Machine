using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ATM_Machine.Interfaces;

namespace ATM_Machine
{
    public class LoggedInOperations : ILoggedInOperations
    {
        private readonly ApplicationDbContext? _context;
        private readonly IValidations? _validations;

        public LoggedInOperations(ApplicationDbContext context, IValidations? validations)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._validations = validations ?? throw new ArgumentNullException(nameof(validations));
        }

        public void ChooseOperation(User user)
        {
            while (true)
            {
                Console.Write("- 1 - ");
                DisplayBase.DisplayLine("Check balance", ConsoleColor.Blue);
                Console.Write("- 2 - "); 
                DisplayBase.DisplayLine("Withdraw amount", ConsoleColor.Blue);
                Console.Write("- 3 - "); 
                DisplayBase.DisplayLine("Add amount", ConsoleColor.Blue);
                Console.Write("- 4 - "); 
                DisplayBase.DisplayLine("Transfer amount", ConsoleColor.Blue);
                Console.Write("- 5 - "); 
                DisplayBase.DisplayLine("Change PIN", ConsoleColor.Blue);
                Console.Write("- 6 - "); 
                DisplayBase.DisplayLine("Change user details", ConsoleColor.Blue);
                Console.Write("- 7 - ");
                DisplayBase.DisplayLine("Log out\n", ConsoleColor.Blue);

                DisplayBase.Display($"What would you like to do, {user.FirstName}? ", ConsoleColor.DarkGray);
                string? operation = Console.ReadLine();

                switch (operation)
                {
                    case "1":
                        CheckAmount(user);
                        break;
                    case "2":
                        WithdrawAmout(user);
                        break;
                    case "3":
                        AddAmount(user);
                        break;
                    case "4":
                        TransferAmount(user);
                        break;
                    case "5":
                        ChangePIN(user);
                        break;
                    case "6":
                        ChangeDetails(user);
                        break;
                    case "7":
                        return;
                    default:
                        DisplayBase.DisplayLine("\nNo operation chosen!\n", ConsoleColor.Red);
                        break;
                }
            }
        }

        public void CheckAmount(User user)
        {
            DisplayBase.DisplayLine($"\n{user.FirstName} {user.LastName}, your current balance is {user.Balance}$.\n", ConsoleColor.Green);
        }

        public void WithdrawAmout(User user)
        {
            float amount = _validations.GetAmount("Introduce the amount you want to withdraw: ");

            if (amount == 0)
            {
                DisplayBase.DisplayLine("\nYou did not enter a valid amount!\n", ConsoleColor.Red);
                return;
            }

            if (user.Balance >= amount)
            {
                user.Balance -= amount;
                _context.SaveChanges();

                DisplayBase.DisplayLine($"\nYou have successfully withdrawn {amount}$. Your current balance is {user.Balance}$.\n", ConsoleColor.Green);
            }
            else
            {
                DisplayBase.DisplayLine("\nInsuficient fonds!\n", ConsoleColor.Red);
                return;
            }
        }

        public void AddAmount(User user)
        {
            float amount = _validations.GetAmount("Enter the amount you want to add: ");

            if (amount == 0) 
            {
                DisplayBase.DisplayLine("\nYou did not enter a valid amount!\n", ConsoleColor.Red);
                return;
            }

            user.Balance += amount;
            _context.SaveChanges();
            DisplayBase.DisplayLine($"\nYou have succesfully introduced {amount}$.", ConsoleColor.Green);
            CheckAmount(user);
        }

        public void ChangePIN(User user)
        {
            DisplayBase.Display("Enter the new PIN: ", ConsoleColor.DarkGray);
            string PIN1 = Console.ReadLine();

            if (_validations.ValidatePIN(PIN1))
            {
                DisplayBase.Display("Enter the new PIN again: ", ConsoleColor.DarkGray);
                string PIN2 = Console.ReadLine();

                if(PIN1 == PIN2)
                {
                    user.PIN = PIN1;
                    DisplayBase.DisplayLine($"\nYou have successfully changed your PIN, {user.FirstName}!\n", ConsoleColor.Green);
                    _context.SaveChanges();
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
        }

        public void TransferAmount(User user)
        {
            DisplayBase.Display("\nEnter the Card ID of the user you want to transfer to: ", ConsoleColor.DarkGray);
            string? input = Console.ReadLine();
            bool cardIdTransfer = false;

            if(input == user.CardID)
            {
                DisplayBase.DisplayLine("\nYou can't transfer amount to yourself!\n", ConsoleColor.Red);
                return;
            }

            List<User> users = _context.Users.ToList();

            foreach(var possibleUser in users)
            {
                if (possibleUser.CardID == input)
                {
                    float amount = _validations.GetAmount("Enter the amount you want to transfer: ");

                    if (amount == 0)
                    {
                        DisplayBase.DisplayLine("\nYou did not enter a valid amount!\n", ConsoleColor.Red);
                        return;
                    }

                    if (user.Balance < amount)
                    {
                        DisplayBase.DisplayLine("\nInsuficient fonds!\n", ConsoleColor.Red);
                        return;
                    }

                    possibleUser.Balance += amount;
                    user.Balance -= amount;
                    _context.SaveChanges();

                    CheckAmount(user);
                    CheckAmount(possibleUser);

                    cardIdTransfer = true;
                }
            }

            if (!cardIdTransfer)
            {
                DisplayBase.DisplayLine("\nYou did not enter a valid card ID!\n", ConsoleColor.Red);
                return;
            }
        }

        public void ChangeDetails(User user)
        {
            DisplayBase.Display("Enter your new first name (for cancelation submit x): ", ConsoleColor.DarkGray);

            string? firstName = Console.ReadLine();

            if (_validations.ValidateName(firstName)) 
            {
                DisplayBase.DisplayLine("\nYou have succesfully updated last first name!\n", ConsoleColor.Green);
                user.FirstName = firstName;
                _context.SaveChanges();
            }

            else
            {
                return;
            }

            DisplayBase.Display("Enter your new last name (for cancelation submit x): ", ConsoleColor.DarkGray);

            string lastName = Console.ReadLine();

            if (_validations.ValidateName(lastName))
            {
                DisplayBase.DisplayLine("\nYou have succesfully updated last first name!\n", ConsoleColor.Green);
                user.LastName = lastName;
                _context.SaveChanges();
            }
            else
            {
                return;
            }

            DisplayBase.DisplayLine($"\nYou have succesfully updated the details, {user.LastName} {user.FirstName}!\n", ConsoleColor.Green);
        }
    }
}