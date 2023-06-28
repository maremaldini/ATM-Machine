namespace ATM_Machine.Interfaces
{
    public interface IValidations
    {
        bool ValidateName(string text);
        bool ValidatePIN(string text);
        string GetNewCardID();
        float GetAmount(string message);
    }
}