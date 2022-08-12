namespace ProjectLMS.Models
{
    public interface ILoginRepo
    {
        public Account getUserByName(string userName);
    }
}
