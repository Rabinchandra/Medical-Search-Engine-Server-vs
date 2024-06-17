using Google.Apis.Auth.OAuth2;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class FirebaseService
{
    private readonly HttpClient _httpClient;

    public FirebaseService()
    {
        _httpClient = new HttpClient();
    }

    public class FirebaseResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

    public async Task<FirebaseResponse> DeleteUser(string uid)
    {
        try
        {
            // Correctly reference the service account key file path
            var serviceAccountKeyPath = Path.Combine(AppContext.BaseDirectory, "ServiceFiles", "medical-search-engine-92f3b-cae392eb4c4d.json");

            // Load the service account credentials
            GoogleCredential credential;
            using (var stream = new FileStream(serviceAccountKeyPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped("https://www.googleapis.com/auth/identitytoolkit");
            }

            var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            // Replace with your actual Firebase Web API keyAIzaSyDc5018DHEWaMZmpVWwwaLshDRmFrb32yk
            var apiKey = "AIzaSyDc5018DHEWaMZmpVWwwaLshDRmFrb32yk";

            // Make the request to delete the user
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://identitytoolkit.googleapis.com/v1/accounts:delete?key={apiKey}"),
                Headers =
                {
                    { "Authorization", $"Bearer {token}" }
                },
                Content = new StringContent($"{{\"localId\": \"{uid}\"}}", Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return new FirebaseResponse
                {
                    StatusCode = (int)response.StatusCode,
                    Message = $"Successfully deleted user with UID: {uid}"
                };
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return new FirebaseResponse
            {
                StatusCode = (int)response.StatusCode,
                Message = responseContent
            };
        }
        catch (Exception ex)
        {
            return new FirebaseResponse
            {
                StatusCode = 500,
                Message = $"Internal server error: {ex.Message}"
            };
        }
    }
}
