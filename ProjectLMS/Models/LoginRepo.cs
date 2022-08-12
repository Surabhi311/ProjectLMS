using System.Linq;
namespace ProjectLMS.Models
{
    public class LoginRepo : ILoginRepo
    {

        private readonly LMS_dbContext _productContext;

        public LoginRepo(LMS_dbContext productContext)
        {
            _productContext = productContext;

        }
        Account ILoginRepo.getUserByName(string username)
        {
            return _productContext.Accounts.FirstOrDefault(u => u.Username == username);
        }

    }
}
