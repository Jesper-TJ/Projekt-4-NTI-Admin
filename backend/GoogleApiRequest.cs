using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System.Text.Json;

namespace AdminApi.GoogleApi
{
    static class GoogleUsersApiRequest
    {
        private const string SERVICE_ACCOUNT_FILE = "googleToken.json";
        // SCOPES
        private static readonly string[] SCOPES =
        {
            DirectoryService.Scope.AdminDirectoryUserReadonly
        };

        public static JsonDocument Main()
        {
            var service = Authorize();
            string domain = "ga.ntig.se";
            var users = ListUsers(service, domain);
            var allUsers = new List<User>(users.UsersValue);

            while (users.NextPageToken != null)
            {
                users = ListNextUsers(service, users.NextPageToken, domain);
                allUsers.AddRange(users.UsersValue);
            }

            domain = "elev.ga.ntig.se";
            users = ListUsers(service, domain);
            allUsers.AddRange(users.UsersValue);

            while (users.NextPageToken != null)
            {
                users = ListNextUsers(service, users.NextPageToken, domain);
                allUsers.AddRange(users.UsersValue);
            }

            var jsonString = JsonConvert.SerializeObject(allUsers);
            return JsonDocument.Parse(jsonString);
        }

        static DirectoryService Authorize()
        {
            // Authorize with the service account credentials
            var credential = GoogleCredential.FromFile(SERVICE_ACCOUNT_FILE)
                .CreateScoped(SCOPES)
                .CreateWithUser(Environment.GetEnvironmentVariable("GOOGLE_API_SERVICE_ACCOUNT"));

            var service = new DirectoryService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Nti Admin API"
            });

            return service;
        }

        static Users ListUsers(DirectoryService service, string domain)
        {
            var request = service.Users.List();
            request.Domain = domain;
            request.Query = $"orgUnitPath='{Environment.GetEnvironmentVariable("GOOGLE_API_ORGUNITPATH")}'";

            return request.Execute();
        }

        static Users ListNextUsers(DirectoryService service, string nextPageToken, string domain)
        {
            var request = service.Users.List();
            request.Domain = domain;
            request.Query = $"orgUnitPath='{Environment.GetEnvironmentVariable("GOOGLE_API_ORGUNITPATH")}'";
            request.PageToken = nextPageToken;

            return request.Execute();
        }
    }
}
