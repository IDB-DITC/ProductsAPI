using ProductsAPI.Models;

namespace ProductsAPI.Services
{
	public interface ITokenService
	{
		string CreateToken(ApplicationUser user);
	}
}