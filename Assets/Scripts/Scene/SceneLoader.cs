using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : ISceneLoader
{
    public void Load(SceneName scene)
    {
        SceneManager.LoadScene(scene.ToSceneString());
    }

    public void LoadAsync(SceneName scene, Action onComplete = null)
    {
        var op = SceneManager.LoadSceneAsync(scene.ToSceneString());
        op.completed += _ => onComplete?.Invoke();
    }
}
