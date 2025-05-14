namespace Auth.Services
{
    // 소셜 로그인 결과를 위한 공통 베이스 클래스
    public abstract class SocialAuthResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string Email { get; set; }
    }

    // Google 로그인 결과 클래스
    public class GoogleSignInResult : SocialAuthResult
    {
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
    }

    // Apple 로그인 결과 클래스
    public class AppleSignInResult : SocialAuthResult
    {
        public string IdentityToken { get; set; }
        public string Nonce { get; set; }
    }
}