namespace Infrastructure.Interfaces
{
    public interface IUserAccessor
    {
         string GetCurrentUsername();
         string GetCurrentUserId();
    }
}