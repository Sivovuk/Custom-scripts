using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Newtonsoft.Json; // Import Newtonsoft.Json

public class GoogleOAuth2Login : MonoBehaviour
{
    public Text statusText;
    public string clientId = "https://702839863875-qqrvo12i37f0vd6mqjeod950pi5pa16t.apps.googleusercontent.com";
    //public string clientId = "http://localhost:3000";
    public string clientSecret = "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"; // Keep this secure
    public string redirectUri = "http://localhost";

    private const string authEndpoint = "https://accounts.google.com/o/oauth2/auth";
    private const string tokenEndpoint = "https://accounts.google.com/o/oauth2/token";
    private const string scope = "openid email profile"; // Adjust the scope as needed

    private string accessToken;

    private void Start()
    {
        statusText.text = "Not logged in.";
    }

    public void Login()
    {
        // Generate the OAuth2 authorization URL
        string authUrl = $"{authEndpoint}?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope={scope}";

        // Open the Google login page in a new browser window or tab
        Application.ExternalEval($"window.open('{authUrl}','_blank')");
    }

    public void HandleRedirect(string code)
    {
        StartCoroutine(ExchangeCodeForToken(code));
    }

    private IEnumerator ExchangeCodeForToken(string code)
    {
        // Prepare the token request parameters
        WWWForm form = new WWWForm();
        form.AddField("code", code);
        form.AddField("client_id", clientId);
        form.AddField("client_secret", clientSecret);
        form.AddField("redirect_uri", redirectUri);
        form.AddField("grant_type", "authorization_code");

        // Send a POST request to exchange the code for an access token
        using (UnityWebRequest www = UnityWebRequest.Post(tokenEndpoint, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
                statusText.text = "Error during login.";
            }
            else
            {
                string responseJson = www.downloadHandler.text;

                // Use Newtonsoft.Json to parse the JSON response
                TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseJson);

                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.access_token))
                {
                    accessToken = tokenResponse.access_token;
                    statusText.text = "Logged in!";
                }
                else
                {
                    Debug.LogError("Failed to parse access token.");
                    statusText.text = "Error during login.";
                }
            }
        }
    }

    // Example: Use the access token to make an authenticated API request
    public void MakeAuthenticatedRequest()
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            Debug.LogError("Access token is missing.");
            return;
        }

        string apiUrl = "https://www.googleapis.com/oauth2/v2/userinfo";
        string authorizationHeader = $"Bearer {accessToken}";

        StartCoroutine(MakeApiRequest(apiUrl, authorizationHeader));
    }

    private IEnumerator MakeApiRequest(string url, string authorizationHeader)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Authorization", authorizationHeader);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string responseJson = www.downloadHandler.text;
            Debug.Log(responseJson);
        }
    }

    // Define a data class for token response
    [System.Serializable]
    private class TokenResponse
    {
        public string access_token;
        // Add other token response fields as needed
    }
}
