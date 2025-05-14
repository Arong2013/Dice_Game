using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Auth.Services;
using UnityEngine;
using UnityEngine.Apple.SignInWithApple;

namespace Auth.Implementation
{
    // Apple 로그인 서비스 구현
    public class AppleSignInService : BaseSocialAuthService, IAppleSignInService
    {
        private string _currentNonce;

        public AppleSignInService() : base("Apple")
        {
            Initialize();
        }

        protected override void Initialize()
        {
            if (_isInitialized) return;

#if UNITY_EDITOR
            _isInitialized = true;
#else
            try
            {
                // Apple Sign In은 별도의 초기화가 필요 없음 
                // (iOS 빌드 설정에서 Sign in with Apple 기능 활성화 필요)
                _isInitialized = true;
                Logger.Log($"[{_providerName}Auth] 초기화 성공");
            }
            catch (Exception ex)
            {
                Logger.LogError($"[{_providerName}Auth] 초기화 실패: {ex.Message}");
            }
#endif
        }

        // IAppleSignInService 인터페이스 구현
        public new async Task<AppleSignInResult> SignInAsync()
        {
            return await base.SignInAsync<AppleSignInResult>();
        }

        // 베이스 클래스의 추상 메서드 구현
        protected override async Task<T> ExecuteSignInAsync<T>()
        {
#if UNITY_EDITOR
            return CreateEditorMockResult<T>();
#elif UNITY_IOS
            try
            {
                // nonce 생성 (CSRF 방지를 위한 보안 조치)
                _currentNonce = GenerateNonce();
                var nonceStr = _currentNonce;
                
                // Apple 로그인 요청 설정
                var loginArgs = new AppleAuthLoginArgs(
                    LoginOptions.IncludeEmail | LoginOptions.IncludeFullName,
                    nonceStr);
                
                // TaskCompletionSource를 사용하여 콜백 기반 API를 Task 기반으로 변환
                var tcs = new TaskCompletionSource<IAppleIDCredential>();
                
                // Apple 로그인 요청
                SignInWithApple.Login(loginArgs, credential => {
                    if (credential is IAppleIDCredential appleIDCredential)
                    {
                        tcs.SetResult(appleIDCredential);
                    }
                    else
                    {
                        tcs.SetException(new Exception("Failed to get Apple ID credential"));
                    }
                }, error => {
                    tcs.SetException(new Exception(error.ToString()));
                });
                
                // 로그인 결과 대기
                var appleCredential = await tcs.Task;
                
                // identity token, UTF-8 인코딩을 사용하여 문자열로 변환
                string identityToken = Encoding.UTF8.GetString(appleCredential.IdentityToken);
                string email = appleCredential.Email;
                
                if (typeof(T) == typeof(AppleSignInResult))
                {
                    var result = new AppleSignInResult
                    {
                        Success = true,
                        IdentityToken = identityToken,
                        Nonce = nonceStr,
                        Email = email
                    };
                    
                    return result as T;
                }
                
                throw new InvalidCastException($"Cannot convert AppleSignInResult to {typeof(T).Name}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"[{_providerName}Auth] 로그인 실패: {ex.Message}");
                
                if (typeof(T) == typeof(AppleSignInResult))
                {
                    var errorResult = new AppleSignInResult
                    {
                        Success = false,
                        Error = ex.Message
                    };
                    
                    return errorResult as T;
                }
                
                throw;
            }
#else
            // iOS가 아닌 다른 플랫폼에서는 Apple 로그인 지원하지 않음
            if (typeof(T) == typeof(AppleSignInResult))
            {
                var errorResult = new AppleSignInResult
                {
                    Success = false,
                    Error = "Apple Sign In is only supported on iOS platform"
                };
                
                return errorResult as T;
            }
            
            throw new NotSupportedException("Apple Sign In is only supported on iOS platform");
#endif
        }

        protected override T CreateEditorMockResult<T>()
        {
            if (typeof(T) == typeof(AppleSignInResult))
            {
                var mockResult = new AppleSignInResult
                {
                    Success = true,
                    IdentityToken = "dummy_identity_token",
                    Nonce = "dummy_nonce",
                    Email = "test@icloud.com"
                };

                return mockResult as T;
            }

            return new T { Success = true };
        }

        // Apple은 일반적으로 로그아웃 API를 제공하지 않으므로 기본 구현 사용

        #region Helper Methods

        // 보안 nonce 생성 메서드
        private string GenerateNonce(int length = 32)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
            var random = new System.Random();
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }

        // SHA256 해시 생성 메서드 (Firebase 인증에 필요)
        private string GenerateSHA256Hash(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        #endregion
    }
}