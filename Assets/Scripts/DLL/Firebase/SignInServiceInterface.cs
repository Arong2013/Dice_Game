using System.Threading.Tasks;

namespace Auth.Services
{
    // Google 로그인 서비스 인터페이스
    public interface IGoogleSignInService
    {
        Task<GoogleSignInResult> SignInAsync();
        Task SignOutAsync();
        bool IsAvailable();
    }
    public interface IAppleSignInService
    {
        Task<AppleSignInResult> SignInAsync();
        bool IsAvailable();
    }
}