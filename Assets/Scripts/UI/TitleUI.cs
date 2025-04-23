using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour, ITitleUI
{
    [SerializeField] private Button _startBtn;

    public event Action OnContinueClicked;

    private void Awake()
    {
        _startBtn.onClick.AddListener(() => OnContinueClicked?.Invoke());
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
