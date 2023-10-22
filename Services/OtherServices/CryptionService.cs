using System.Security.Cryptography;
using System.Text;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.OtherServices
{
    public class CryptionService : ICryptionService
    {

        public string Decrypt(string key)
        {
            try
            {
                string _key = "abcdefgh";
                string privatekey = "hgfedcba";
                byte[] privatekeyByte = { };
                privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
                byte[] _keybyte = { };
                _keybyte = Encoding.UTF8.GetBytes(_key);
                byte[] inputtextbyteArray = new byte[key.Replace(" ", "+").Length];
                //This technique reverses base64 encoding when it is received over the Internet.
                inputtextbyteArray = Convert.FromBase64String(key.Replace(" ", "+"));
                using (DESCryptoServiceProvider dEsp = new DESCryptoServiceProvider())
                {
                    var memstr = new MemoryStream();
                    var crystr = new CryptoStream(memstr, dEsp.CreateDecryptor(_keybyte, privatekeyByte), CryptoStreamMode.Write);
                    crystr.Write(inputtextbyteArray, 0, inputtextbyteArray.Length);
                    crystr.FlushFinalBlock();
                    return Encoding.UTF8.GetString(memstr.ToArray());
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
