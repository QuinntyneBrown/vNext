namespace vNext.Core.Identity
{
    public interface ISecurityTokenFactory
    {
        string Create(string username, int userId);
    }
}
