using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestTask
{
    public class AuthOptions
    {
        public const string Issuer = "TestServer"; // издатель токена
        public const string Audience = "TestClient"; // потребитель токена
        const string Key = "3c7inyc3wx78n3c783x8yu8djvfmklslewiou837823iwessdajk";   // ключ для шифрации
        public const int Lifetime = 5; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
