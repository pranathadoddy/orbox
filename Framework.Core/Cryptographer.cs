using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Core
{
    public static class Cryptographer
    {
        #region Constants

        private const string Key = "a6653d2e-4d38-4cc5-af9b-91e91c5e9ee0";

        private const string Pad = "!@#oRDerb0x$%^";

        private const string RandomAlphaNumericPosition = "9PYKB8GMI3,7TX5VSDNH4,6UZLRJFC02,OW1AEQ";

        #endregion

        #region Public Methods

        public static string NumberToLetter(string numericString)
        {
            var randomAlphanumericPartition = RandomAlphaNumericPosition.Split(",");
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < numericString.Length; i++)
            {
                if (numericString[i] == '-') 
                {
                    stringBuilder.Append(numericString[i]);
                }
                else
                {
                    var number = int.Parse(numericString[i].ToString());
                    var partitionIndex = number % randomAlphanumericPartition.Length;
                    var partition = randomAlphanumericPartition[partitionIndex];
                    var alphanumericPosition = number % partition.Length;
                    var generatedAlphabet = partition[alphanumericPosition];
                    stringBuilder.Append(generatedAlphabet);
                }
            }

            return stringBuilder.ToString();
        }

        public static string Encrypt(string plainText)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(Key)));
        }

        public static string Decrypt(string encryptedText)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(Key)));
        }

        public static string Base64OTPEncrypt(string plainText)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var pad = Encoding.UTF8.GetBytes(Pad);

            var result = new byte[plainBytes.Length];
            for (int i = 0; i < plainBytes.Length; i++)
            {
                var sum = (int)plainBytes[i] + (int)pad[i];
                if (sum > 255)
                    sum -= 255;
                result[i] = (byte)sum;
            }

            return Convert.ToBase64String(result);
        }

        public static string Base64OTPDecrypt(string encryptedText)
        {
            try
            {
                var base64OfEncryptedTextBytes = Convert.FromBase64String(encryptedText);
                var pad = Encoding.UTF8.GetBytes(Pad);

                var result = new byte[base64OfEncryptedTextBytes.Length];
                for (int i = 0; i < base64OfEncryptedTextBytes.Length; i++)
                {
                    var dif = (int)base64OfEncryptedTextBytes[i] - (int)pad[i];
                    if (dif < 0)
                        dif += 255;
                    result[i] = (byte)dif;
                }

                return Encoding.UTF8.GetString(result);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string JsonWebTokenEncode(Dictionary<string, object> payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, Key);
        }

        /// <summary>
        ///     This method will need a try catch for safety since it may throw :
        ///     1. TokenExpiredException
        ///     2. SignatureVerificationException
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonWebTokenDecode(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            var json = decoder.Decode(token, Key, true);

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        #endregion

        #region Private Methods

        private static Aes GetRijndaelManaged(string secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));

            var aes = Aes.Create("AesManaged");
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyBytes;
            aes.IV = keyBytes;

            return aes;
        }

        private static byte[] Encrypt(byte[] plainBytes, Aes rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        private static byte[] Decrypt(byte[] encryptedData, Aes rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        #endregion
    }
}
