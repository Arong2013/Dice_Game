using System;
using System.Threading.Tasks;
using Auth.Services;
using UnityEngine;
using Google;

namespace Auth.Implementation
{
    // Google 로그인 서비스 구현
    public class GoogleSignInService : BaseSocialAuthService, IGoogleSignInService
    {
        private GoogleSignInConfiguration _configuration;

        public GoogleSignInService() : base("Google")
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
                // Google SignIn SDK 초기화
                _configuration = new GoogleSignInConfiguration
                {
                    RequestIdToken = true,
                    RequestEmail = true,
                    // 파이어베이스 콘솔에서 웹 클라이언트 ID를 가져와서 설정해야 함
                    WebClientId = "YOUR_FIREBASE_WEB_CLIENT_ID"
                };
                
                GoogleSignIn.Configuration = _configuration;
                _isInitialized = true;
                Logger.Log($"[{_providerName}Auth] 초기화 성공");
            }
            catch (Exception ex)
            {
                Logger.LogError($"[{_providerName}Auth] 초기화 실패: {ex.Message}");
            }
#endif
        }

        // IGoogleSignInService 인터페이스 구현
        public new async Task<GoogleSignInResult> SignInAsync()
        {
            return await base.SignInAsync<GoogleSignInResult>();
        }

        // 베이스 클래스의 추상 메서드 구현
        protected override async Task<T> ExecuteSignInAsync<T>()
        {
#if UNITY_EDITOR
            return CreateEditorMockResult<T>();
#else
            try
            {
                // Google 로그인 UI 표시 및 인증 수행
                var signInTask = GoogleSignIn.DefaultInstance.SignIn();
                
                // Task를 await 가능한 형태로 변환
                var taskCompletionSource = new TaskCompletionSource<GoogleSignInUser>();
                
                signInTask.ContinueWith(task => {
                    if (task.IsCanceled)
                    {
                        taskCompletionSource.SetCanceled();
                    }
                    else if (task.IsFaulted)
                    {
                        taskCompletionSource.SetException(task.Exception);
                    }
                    else
                    {
                        taskCompletionSource.SetResult(task.Result);
                    }
                });
                
                // 로그인 결과 대기
                var signInResult = await taskCompletionSource.Task;
                
                if (typeof(T) == typeof(GoogleSignInResult))
                {
                    var result = new GoogleSignInResult
                    {
                        Success = true,
                        IdToken = signInResult.IdToken,
                        AccessToken = null, // Google SignIn API는 직접 AccessToken을 제공하지 않음
                        Email = signInResult.Email
                    };
                    
                    return result as T;
                }
                
                // 타입이 일치하지 않는 경우 (발생할 수 없지만 안전을 위해)
                throw new InvalidCastException($"Cannot convert GoogleSignInResult to {typeof(T).Name}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"[{_providerName}Auth] 로그인 실패: {ex.Message}");
                
                if (typeof(T) == typeof(GoogleSignInResult))
                {
                    var errorResult = new GoogleSignInResult
                    {
                        Success = false,
                        Error = ex.Message
                    };
                    
                    return errorResult as T;
                }
                
                throw; // 재던지기
            }
#endif
        }

        protected override T CreateEditorMockResult<T>()
        {
            if (typeof(T) == typeof(GoogleSignInResult))
            {
                var mockResult = new GoogleSignInResult
                {
                    Success = true,
                    IdToken = "dummy_id_token",
                    AccessToken = "dummy_access_token",
                    Email = "test@gmail.com"
                };

                return mockResult as T;
            }

            return new T { Success = true };
        }

        public override async Task SignOutAsync()
        {
            if (!_isInitialized) return;

#if UNITY_EDITOR
            await Task.Delay(100);
#else
            try
            {
                // Google 로그아웃
                GoogleSignIn.DefaultInstance.SignOut();
                await Task.CompletedTask;
                Logger.Log($"[{_providerName}Auth] 로그아웃 성공");
            }
            catch (Exception ex)
            {
                Logger.LogError($"[{_providerName}Auth] 로그아웃 실패: {ex.Message}");
            }
#endif
        }
    }
}