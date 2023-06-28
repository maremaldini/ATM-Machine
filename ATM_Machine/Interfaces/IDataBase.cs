namespace ATM_Machine.Interfaces
{
    public interface IDataBase
    {
        void Add(User user);
        List<User> GetAllUsers();
        User GetUserByID(int id);
        void PrintAllUsers();

        List<string> GetAllCardIDs();
    }
}