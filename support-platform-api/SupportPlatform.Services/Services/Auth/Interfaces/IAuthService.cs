using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface IAuthService
    {
        Task<IServiceResult> RegisterAsync(UserToRegisterDto userToRegister);
        Task<IServiceResult<string>> LoginAsync(UserToLoginDto userToLogin);
        Task<IServiceResult> ConfirmEmailAsync(AccountToConfirmDto accountToConfirm);
    }
}
