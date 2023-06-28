using System;

namespace ATM_Machine
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PIN { get; set; }
        public string CardID { get; set; }
        public float Balance { get; set; } = 0;

        public User()
        {
                
        }

        public User(string firstName, string lastName, string PIN, string cardID)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PIN = PIN;
            this.CardID = cardID;
        }

        public void PrintUser()
        {
            Console.Write("\nFirst name: ");
            DisplayBase.Display($"{FirstName}", ConsoleColor.Blue);
            Console.Write(". Last name: ");
            DisplayBase.Display($"{LastName}", ConsoleColor.Blue);
            Console.Write(". Card ID: ");
            DisplayBase.Display($"{CardID}", ConsoleColor.Blue);
            Console.Write(". Balance: ");
            DisplayBase.Display($"{Balance}$.", ConsoleColor.Blue);
        }
    }
}

