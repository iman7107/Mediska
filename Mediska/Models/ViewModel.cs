using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mediska.Models
{
    public class clsLoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "موبایل خود را وارد نمایید")]
        [Mobile(ErrorMessage = "شماره موبایل صحیح نمی باشد")]
        [Display(Name = "موبایل")]
        public string userMobile { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور را وارد نمایید")]
        [Display(Name = "رمز عبور")]
        public string userPassword { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool rememberMe { get; set; }

        public string GoogleCaptchaToken { get; set; }
    }

    //============================================================================================================================
    //============================================================================================================================
    //============================================================================================================================
    public class ChangePasswordViewModel
    {
        public int UserID { get; set; }

        [Display(Name = "رمز عبور فعلی")]
        [Required(ErrorMessage = "{0} وارد نشده است")]
        public string OldPassword { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "{0} وارد نشده است")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "{0} وارد نشده است")]
        public string ConfirmNewPassword { get; set; }
    }

    //============================================================================================================================
    //============================================================================================================================
    //============================================================================================================================
    public class CaptchaResponseViewModel
    {
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }

        [JsonProperty(PropertyName = "challenge_ts")]
        public DateTime ChallengeTime { get; set; }

        public string HostName { get; set; }
        public double Score { get; set; }
        public string Action { get; set; }
    }

    //============================================================================================================================
    //============================================================================================================================
    //============================================================================================================================
    public class clsRegisterModel
    {
        [Mobile(ErrorMessage = "شماره موبایل صحیح نمی باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "موبایل خود را وارد نمایید")]
        [Display(Name = "موبایل")]
        public string Mobile { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور را وارد نمایید")]
        [StrongPassword(ErrorMessage ="رمز عبور انتخاب شده ضعیف می باشد.")]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "{0} با رمز عبور برابر نمی باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "تکرار رمز عبور را وارد نمایید")]
        [Display(Name = "تکرار رمز عبور")]
        public string ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "نام خود را وارد نمایید")]
        [Display(Name = "نام")]
        public string FName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "نام خانوادگی خود را وارد نمایید")]
        [Display(Name = "نام خانوادگی")]
        public string LName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "جنسیت خود را وارد نمایید")]
        [Display(Name = "جنسیت")]
        public bool Gender { get; set; }

        public string ReturnUrl { get; set; }

        public string GoogleCaptchaToken { get; set; }
    }
    //============================================================================================================================
    //============================================================================================================================
    //============================================================================================================================

    public class clsGuidCode
    {
        public string Guid{ get; set;}

        [Required(AllowEmptyStrings = false, ErrorMessage = "کد وارد نشده است")]
        [Display(Name = "کد تایید")]
        public string Code{ get; set;}

        public string GoogleCaptchaToken { get; set; }
    }

    //============================================================================================================================
    //============================================================================================================================
    //============================================================================================================================
    public class clsMobile
    {
        [Mobile(ErrorMessage = "شماره موبایل صحیح نمی باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "موبایل خود را وارد نمایید")]
        [Display(Name = "موبایل")]
        public string Mobile { get; set; }
        public string GoogleCaptchaToken { get; set; }
    }

    //============================================================================================================================
    //============================================================================================================================
    //============================================================================================================================
    public class clsChangePassword
    {
        public int? CustomerID { get; set; }
        public string Guid { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور فعلی را وارد نمایید")]
        [Display(Name = "رمز عبور فعلی")]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "رمز عبور را وارد نمایید")]
        [StrongPassword(ErrorMessage = "رمز عبور انتخاب شده ضعیف می باشد.")]
        [Display(Name = "رمز عبور جدید")]
        public string NewPassword { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "{0} با رمز عبور برابر نمی باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "تکرار رمز عبور را وارد نمایید")]
        [Display(Name = "تکرار رمز عبور")]
        public string ConfirmPassword { get; set; }

        public string GoogleCaptchaToken { get; set; }
        public bool HasSMSPassword { get; set; }

    }
    //============================================================================================================================
    //============================================================================================================================
    //============================================================================================================================

    public class clsShopCartItem
    {
        public int PackageID { get; set; }
        public int ProductID { get; set; }
        public string PackageName { get; set; }
        public string ProductName { get; set; }
        public Decimal Price { get; set; }

    }



}