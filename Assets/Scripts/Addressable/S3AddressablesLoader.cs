using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class S3AddressablesLoader : MonoBehaviour
{
    // Addressables에서 로드할 에셋 주소
    public string assetAddress = "MyPrefab";  // Addressables 설정 시 지정한 이름

    IEnumerator Start()
    {
        yield return StartCoroutine(InitializeAndLoad());
    }

    IEnumerator InitializeAndLoad()
    {
        Debug.Log("[Addressables] Initializing...");
        var initHandle = Addressables.InitializeAsync();
        yield return initHandle;

        if (initHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("[Addressables] Initialization failed.");
            yield break;
        }

        Debug.Log("[Addressables] Initialization complete. Loading asset...");

        var loadHandle = Addressables.LoadAssetAsync<GameObject>(assetAddress);
        yield return loadHandle;

        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject prefab = loadHandle.Result;
            Instantiate(prefab);
            Debug.Log($"[Addressables] Successfully loaded and instantiated '{assetAddress}'");
        }
        else
        {
            Debug.LogError($"[Addressables] Failed to load asset: {assetAddress}");
        }

        // 메모리 해제 원하면 Release
        // Addressables.Release(loadHandle);
    }
}
