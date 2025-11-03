using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class DataFetcher : MonoBehaviour
{
    [Tooltip("Set this to your API endpoint")]
    public string apiUrl = "https://api.jsonbin.io/v3/b/6686a992e41b4d34e40d06fa";

    public event Action<Root> OnDataLoaded;
    public event Action<string> OnError;

    void Start()
    {
        // Optionally start automatically:
        StartCoroutine(FetchData());
    }

    public IEnumerator FetchData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                // If the API wraps raw data in an envelope (jsonbin v3 sometimes puts data under "record"),
                // ensure your Root structure matches. We're using Root above.
                try
                {
                    Root data = JsonUtility.FromJson<Root>(json);

                    if (data != null && data.record != null)
                    {
                        OnDataLoaded?.Invoke(data);
                    }
                    else
                    {
                        // Some JSON endpoints return nested wrappers, try extracting "record" sub-JSON manually:
                        // (Only if you see parsing issues — not needed usually)
                        OnError?.Invoke("Parsed JSON but record was null. Raw JSON length: " + json.Length);
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke("JSON parse error: " + ex.Message);
                }
            }
            else
            {
                OnError?.Invoke("Network error: " + request.error);
            }
        }
    }
}
