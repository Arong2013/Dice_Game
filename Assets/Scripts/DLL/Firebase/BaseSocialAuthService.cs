using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Auth.Services
{
    // 소셜 로그인 서비스 추상 클래스
    public abstract class BaseSocialAuthService
    {
        protected bool _isInitialized = false;
        protected string _providerName;

        protected BaseSocialAuthService(string providerName)
        {
            _providerName = providerName;
        }

        // 템플릿 메서드 패턴 적용
        public async Task<T> SignInAsync<T>() where T : SocialAuthResult, new()
        {
            if (!_isInitialized)
            {
                Initialize();
            }

#if UNITY_EDITOR
            // 에디터에서는 가상 로그인 결과 반환
            await Task.Delay(500); // 지연 시뮬레이션
            return CreateEditorMockResult<T>();
#else
            try
            {
                // 실제 로그인 프로세스 실행 (하위 클래스에서 구현)
                return await ExecuteSignInAsync<T>();
            }
            catch (Exception ex)
            {
                Logger.LogError($"[{_providerName}Auth] 로그인 실패: {ex.Message}");
                return new T
                {
                    Success = false,
                    Error = ex.Message
                };
            }
#endif
        }

        // 하위 클래스에서 구현할 메서드들
        protected abstract void Initialize();
        protected abstract Task<T> ExecuteSignInAsync<T>() where T : SocialAuthResult, new();
        protected abstract T CreateEditorMockResult<T>() where T : SocialAuthResult, new();

        // 선택적으로 재정의 가능한 메서드
        public virtual async Task SignOutAsync()
        {
#if UNITY_EDITOR
            await Task.Delay(100); // 지연 시뮬레이션
#else
            await Task.CompletedTask; // 기본 구현은 아무것도 하지 않음
#endif
        }

        public virtual bool IsAvailable()
        {
            return _isInitialized;
        }
    }
}