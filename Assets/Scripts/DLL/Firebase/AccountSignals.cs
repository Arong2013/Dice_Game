using Firebase.Auth;

namespace Auth.Signals
{
    // ���� ���� ���� �ñ׳�
    public class AuthStateChangedSignal
    {
        public FirebaseUser User;
        public bool IsAnonymous;
        public string Provider;
        public string Email;
    }

    // ���� ���� ��� �ñ׳�
    public class AccountLinkResultSignal
    {
        public bool Success;
        public string Message;
        public string Provider;
    }

    // ���� �浹 �ñ׳�
    public class AccountCollisionSignal
    {
        public Credential Credential;
        public string Provider;
        public string Email;
    }

    // ���� ���μ��� ���� �ñ׳�
    public class LinkingStartedSignal
    {
        public string Provider;
    }

    // �α��� ���μ��� �Ϸ� �ñ׳�
    public class LoginCompletedSignal
    {
        public bool IsSuccess;
        public string UserId;
        public bool IsAnonymous;
    }
}