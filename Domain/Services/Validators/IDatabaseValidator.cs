namespace Domain.Services.Validators
{
    public interface IDatabaseValidator
    {
        bool IsValidName(string name);

        bool IsUniqueName(string name);

        bool IsValidServerName(string serverName);
    }
}