using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Base.Extensions
{
    public static class Extension
    {
        // Verilen stringi MD5 ile şifreler.
        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).ToLower();

            }
        }


        public static int GetUserIdFromContext(HttpContext context)
        {
            var userId = int.Parse(context.Items["UserId"] as string);
            return userId;
        }

        // Sipariş Numarası Üretir.
        public static string GenerateOrderNumber()
        {
            // Sipariş numarası oluşturulacak karakter seti
            string charset = "0123456789";

            // Sipariş numarasının uzunluğu
            int length = 9;

            // Rastgele sayı üreteci
            Random random = new Random();

            // Sipariş numarasını oluştur
            StringBuilder orderNumberBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(0, charset.Length);
                char randomChar = charset[randomIndex];
                orderNumberBuilder.Append(randomChar);
            }

            string orderNumber = orderNumberBuilder.ToString();
            return orderNumber;
        }


    }
}
