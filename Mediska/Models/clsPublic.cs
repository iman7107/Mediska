using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Mediska.Models
{
    public static class clsPublic
    {
        public static async Task<bool> IsCaptchaValid(HttpRequestBase Request, string response, string ActionName)
        {
            try
            {
                var secret = "6LcXnQAhAAAAAE4HwY_j3VDkJ80bFTy5txZwkqAE";
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        {"secret", secret},
                        {"response", response},
                        {"remoteip", Request.UserHostAddress}
                    };

                    var content = new FormUrlEncodedContent(values);
                    var verify = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                    var captchaResponseJson = await verify.Content.ReadAsStringAsync();
                    var captchaResult = JsonConvert.DeserializeObject<CaptchaResponseViewModel>(captchaResponseJson);
                    return captchaResult.Success
                           && captchaResult.Action == ActionName
                           && captchaResult.Score > 0.5;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public static string GetEnglishNumber(string persianNumber)
        {
            if (string.IsNullOrEmpty(persianNumber))
                return persianNumber;

            string englishNumber = "";
            foreach (char ch in persianNumber)
            {
                if (((int)ch) >= 1776 && ((int)ch) <= 1785)
                    englishNumber += char.GetNumericValue(ch);
                else
                    englishNumber += ch;
            }
            return englishNumber;
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public static string GenerateRandomCode(int CodeLength)
        {
            Random rd = new Random();
            const string allowedChars = "0123456789";
            char[] chars = new char[CodeLength];

            for (int i = 0; i < CodeLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public static string GetStandardCharacter(string inputStr)
        {
            return inputStr;
        }

    }
}