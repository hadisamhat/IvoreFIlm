using System.Collections.Generic;
using Newtonsoft.Json;

namespace IvoreFilm.Helpers.KeycloakHelpers
{
    public class UserInfo
    {
        public UserInfo()
        {
            ResourceAccess = new Dictionary<string, Access>();
            Group = new List<string>();
            Roles = new List<string>();
        }

        [JsonProperty("sub")] public string Sub { get; set; }

        [JsonProperty("resource_access")] public Dictionary<string, Access> ResourceAccess { get; set; }

        [JsonProperty("email_verified")] public bool EmailVerified { get; set; } = true;

        [JsonProperty("realm_access")] public Access RealmAccess { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("group")] public List<string> Group { get; set; }

        [JsonProperty("roles")] public List<string> Roles { get; set; }

        [JsonProperty("preferred_username")] public string PreferredUsername { get; set; }

        [JsonProperty("given_name")] public string GivenName { get; set; }

        [JsonProperty("family_name")] public string FamilyName { get; set; }

        [JsonProperty("email")] public string Email { get; set; }

        [JsonProperty("gender")] public string Gender { get; set; } = "";

        [JsonProperty("phone_number")] public string PhoneNumber { get; set; } = "";

        [JsonProperty("phone_number_verified")]
        public bool PhoneNumberVerified { get; set; } = true;

        [JsonProperty("birthdate")] public string Birthdate { get; set; }
    }

    public class Access
    {
        [JsonProperty("roles")] public string Roles { get; set; }
    }
}