using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    private void Awake()
    {
        if(Instance !=  null) Destroy(gameObject);
        Instance = this;
    }

    public void Get(string uri, Action<String> onFinished)
    {
        StartCoroutine(GetRequest(uri));

        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.Log(webRequest.error);
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.Log(pages[page] + $": Error {webRequest.error}");
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        break;
                }

                onFinished?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }
}
