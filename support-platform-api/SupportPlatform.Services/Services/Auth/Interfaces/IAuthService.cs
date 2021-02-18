using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface IAuthService
    {
        Task<IServiceResult> RegisterAsync(UserToRegisterDto userToRegister);
        Task<IServiceResult> ConfirmEmailAsync(AccountToConfirmDto accountToConfirm);
    }
}
