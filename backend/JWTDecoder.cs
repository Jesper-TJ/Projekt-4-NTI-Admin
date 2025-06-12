using System.Text;

namespace AdminApi.JWTDecoder
{
    public static class JwtDecoder
    {
        
        public static string main(string token)
        {
            var parts = token.Split('.');
            var payload = parts[1];

            var payloadJson = Base64UrlDecode(payload);

            return payloadJson;
        }

        private static string Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+');
            output = output.Replace('_', '/');
            switch (output.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    output += "==";
                    break;
                case 3:
                    output += "=";
                    break;
                default:
                    throw new FormatException("Illegal base64url string!");
            }

            var converted = Convert.FromBase64String(output);
            return Encoding.UTF8.GetString(converted);
        }
    }
}