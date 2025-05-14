using Firebase.Auth;

namespace Auth.Signals
{
    // 인증 상태 변경 시그널
    public class AuthStateChangedSignal
    {
        public FirebaseUser User;
        public bool IsAnonymous;
        public string Provider;
        public string Email;
    }

    // 계정 연동 결과 시그널
    public class AccountLinkResultSignal
    {
        public bool Success;
        public string Message;
        public string Provider;
    }

    // 계정 충돌 시그널
    public class AccountCollisionSignal
    {
        public Credential Credential;
        public string Provider;
        public string Email;
    }

    // 연동 프로세스 시작 시그널
    public class LinkingStartedSignal
    {
        public string Provider;
    }

    // 로그인 프로세스 완료 시그널
    public class LoginCompletedSignal
    {
        public bool IsSuccess;
        public string UserId;
        public bool IsAnonymous;
    }
}