using System;
using ATM_Machine.Interfaces;

namespace ATM_Machine
{
    public class Validations : IValidations
    {
        public Validations()
        {
                
        }

        public bool ValidateName(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                DisplayBase.DisplayLine("\nNo input introduced!\n", ConsoleColor.Red);
                return false;
            }

            if (text == "X" || text == "x")
            {
                DisplayBase.DisplayLine("\nCancel the registration!\n", ConsoleColor.Red);
                return false;
            }

            if (text.Length < 3 || text.Length > 20)
            {
                DisplayBase.DisplayLine("\nInput must be greater than 3 and lower than 20 characters!\n", ConsoleColor.Red);
                return false;
            }

            if (!text.All(Char.IsLetter))
            {
                DisplayBase.DisplayLine("\nAll characters must be letters!\n", ConsoleColor.Red);
                return false;
            }

            return true;
        }

        public bool ValidatePIN(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                DisplayBase.DisplayLine("\nNo input introduced!\n", ConsoleColor.Red);
                return false;
            }

            if (text.Length != 4)
            {
                DisplayBase.DisplayLine("\nPIN must have 4 digits!\n", ConsoleColor.Red);
                return false;
            }

            if (!text.Any(Char.IsNumber) || text.Any(Char.IsLetter))
            {
                DisplayBase.DisplayLine("\nPIN can only contain numbers!\n", ConsoleColor.Red);
                return false;
            }

            if (text == "X" || text == "x")
            {
                DisplayBase.DisplayLine("\nCancelation!\n", ConsoleColor.Red);
                return false;
            }

            return true;
        }

        public string GetNewCardID()
        {
            Random rand = new Random();
            string cardID = "99";

            for (int i = 0; i < 4; i++)
            {
                cardID += (rand.Next(0, 10)).ToString();
            }

            return cardID;
        }
        
        public float GetAmount(string message)
        {
            DisplayBase.Display("\n" + message, ConsoleColor.DarkGray);

            bool ok = float.TryParse(Console.ReadLine(), out float amount);

            if (ok)
            {
                if (amount < 0)
                {
                    DisplayBase.Display("\nThe transfered amount cannot be negative!\n", ConsoleColor.Red);
                    return 0;
                }

                if (amount > 5000)
                {
                    DisplayBase.Display("\nThe maximum amount to be transfered is 5000$!\n", ConsoleColor.Red);
                    return 0;
                }

                return amount;

            }
            else
            {
                return 0;
            }
        }
    }
}