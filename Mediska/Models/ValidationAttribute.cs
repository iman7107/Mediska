using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mediska.Models
{
    public class MobileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            string str = value.ToString();
            if (!str.StartsWith("09"))
                return false;

            try
            {
                long l = Convert.ToInt64(str);
            }
            catch (Exception)
            {
                return false;
            }

            if (str.Length != 11)
                return false;

            return true;
        }
    }

    public class StrongPasswordAttribute : ValidationAttribute
    {
        int minlen;
        bool hasspecialchar;
        bool hascapitalchar;
        bool hassmallchar;
        bool hasnumber;
        int minUnique;
       

        /// <summary>
        /// سازنده با پارامتر
        /// </summary>
        /// <param name="MinPasswordLength">حداقل تعداد کاراکتر برای رمز عبور</param>
        /// <param name="HasSpecialCharacter"></param>
        /// <param name="HasUpperCaseCharacter"></param>
        /// <param name="HasLowerCaseCharacter"></param>
        /// <param name="HasNumber"></param>
        /// <param name="MinimumUniqueChar"></param>
        public StrongPasswordAttribute(int MinPasswordLength=6, bool HasSpecialCharacter=true, bool HasUpperCaseCharacter=true, bool HasLowerCaseCharacter=true, bool HasDigit=true, int MinimumUniqueChar=3)
        {
            minlen = MinPasswordLength;
            hasspecialchar = HasSpecialCharacter;
            hascapitalchar = HasUpperCaseCharacter;
            hassmallchar = HasLowerCaseCharacter;
            hasnumber = HasDigit;
            minUnique = MinimumUniqueChar;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string pass = value.ToString();

            if (string.IsNullOrEmpty(pass) || string.IsNullOrWhiteSpace(pass))
                return false;

            if (pass.Length < minlen)
                return false;

            if (hasspecialchar && pass.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) == -1)
                return false;

            if (hascapitalchar && !pass.Any(c => char.IsUpper(c)))
                return false;

            if (hassmallchar && !pass.Any(c => char.IsLower(c)))
                return false;

            if (hasnumber && !pass.Any(c => char.IsDigit(c)))
                return false;

            if (pass.Distinct().Count() < minUnique)
                return false;

            return true;
        }
    }

}