using Domain;

namespace Infrastructure.Interfaces
{
    public interface IJwtGenerator
    {
         string CreateToken(User user);
    }
}