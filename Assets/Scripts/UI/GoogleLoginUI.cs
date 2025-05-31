using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using Google;
using Firebase.Auth;

public class GoogleLoginUI : MonoBehaviour, IGameUI
{
    [Title("Google Login UI Settings")]
    [SerializeField, Required]
    private Button _googleLoginButton;

    [SerializeField, Required]
    private TextMeshProUGUI _statusText;

    [Space(10)]
    [Header("Login Configuration")]
    [Tooltip("로그인 성공 시 자동으로 다음 씬으로 이동할지 여부")]
    [SerializeField]
    private bool _autoMoveToNextScene = true;

    [ShowIf(nameof(_autoMoveToNextScene))]
    [SerializeField]
    private SceneName _nextSceneOnSuccess = SceneName.Lobby;

    [Space(10)]
    [Header("Login Debug")]
    [ReadOnly]
    private string _currentUserEmail;

    [ReadOnly]
    private bool _isLoginInProgress = false;

    private GoogleSignInConfiguration _configuration;
    private FirebaseAuth _auth;

    private void Awake()
    {
        InitializeGoogleSignIn();
        SetupLoginButton();
    }

    private void InitializeGoogleSignIn()
    {
        _configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            RequestEmail = true,
            WebClientId = "434111853481-trko8bii79u519gthcbkhsrup3n3j9d0.apps.googleusercontent.com" // Firebase 콘솔에서 발급받은 WebClientId
        };

        GoogleSignIn.Configuration = _configuration;
        _auth = FirebaseAuth.DefaultInstance;
    }

    private void SetupLoginButton()
    {
        _googleLoginButton.onClick.AddListener(OnGoogleLoginButtonClicked);
    }

    [InfoBox("Google 로그인 버튼 클릭 시 실행되는 메서드입니다.", InfoMessageType.Info)]
    private async void OnGoogleLoginButtonClicked()
    {
        if (_isLoginInProgress) return;

        _statusText.text = "[로그인 시도 중] 버튼 클릭됨";

        try
        {
            _isLoginInProgress = true;
            UpdateUIForLoginProcess(true);

            await PerformGoogleSignIn();
        }
        catch (System.Exception ex)
        {
            if (ex is GoogleSignIn.SignInException signInEx)
            {
                _statusText.text = $"[로그인 오류] 상태: {signInEx.Status}\n메시지: {signInEx.Message}";
            }
            else
            {
                _statusText.text = $"[로그인 오류] 일반 예외: {ex.GetType()}\n{ex.Message}";
            }
        }
        finally
        {
            _isLoginInProgress = false;
            UpdateUIForLoginProcess(false);
        }
    }

    private async Task PerformGoogleSignIn()
{
       try 
    {
        // 로그인 구성 생성
        GoogleSignInConfiguration config = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            RequestEmail = true,
            WebClientId = "YOUR_WEB_CLIENT_ID"
        };

        // 기존 메서드로 로그인 시도
        GoogleSignInUser googleUser = await GoogleSignIn.DefaultInstance.SignIn();

        if (googleUser != null)
        {
            // 로그인 성공 로직
            _currentUserEmail = googleUser.Email;
            
            Credential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);
            FirebaseUser firebaseUser = await _auth.SignInWithCredentialAsync(credential);
        }
    }
    catch (System.Exception ex)
    {
        // 로그인 실패 처리
        Debug.LogError($"로그인 오류: {ex.Message}");
        _statusText.text = $"로그인 오류: {ex.Message}";
    }
}

    private void UpdateUIForLoginProcess(bool isLoading)
    {
        _googleLoginButton.interactable = !isLoading;
        _statusText.color = isLoading ? Color.yellow : Color.black;
    }

    [Button("Reset UI")]
    private void ResetUI()
    {
        _statusText.text = "Google 로그인";
        _statusText.color = Color.black;
        _googleLoginButton.interactable = true;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ResetUI();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    [Button("Print Login Debug Info")]
    private void PrintLoginDebugInfo()
    {
        _statusText.text = $"[Debug] 이메일: {_currentUserEmail}\n로그인중: {_isLoginInProgress}\n자동 이동: {_autoMoveToNextScene}";
    }
}
