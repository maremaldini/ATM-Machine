using System;
using ATM_Machine.Interfaces;

namespace ATM_Machine
{
    public class DataBase : IDataBase
    {
        private readonly ApplicationDbContext _context;

        List<User> users = new List<User>();
        List<int> IDs = new List<int>();

        public DataBase()
        {
                
        }

        public DataBase(ApplicationDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context)) ;
        }

        public void Add(User user)
        {
            if (user == null)
            {
                throw new Exception("User not found!");
            }

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.OrderBy(user => user.ID).ToList();
        }

        public User GetUserByID(int id)
        {
            User? user = _context.Users.FirstOrDefault(user => user.ID == id);

            if(user == null)
            {
                DisplayBase.DisplayLine($"User with ID {id} not found!", ConsoleColor.Red);
                throw new Exception("User with invalid ID!");
            }

            return user;
        }

        public void PrintAllUsers()
        {
            foreach (var user in GetAllUsers())
            {
                user.PrintUser();
            }
            Console.WriteLine("\n");
        }

        public List<string> GetAllCardIDs()
        {
            List<string> list = new List<string>();
            
            foreach (var user in GetAllUsers())
            {
                list.Add(user.CardID.ToString());
            }

            return list;
        }
    }
}