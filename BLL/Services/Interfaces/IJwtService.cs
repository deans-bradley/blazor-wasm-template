using Models;

namespace BLL.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(User user);
    }
}
