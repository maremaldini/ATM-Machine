namespace ATM_Machine.Interfaces
{
    public interface ILoggedInOperations
    {
        void AddAmount(User user);
        void CheckAmount(User user);
        void ChooseOperation(User user);
        void WithdrawAmout(User user);
    }
}