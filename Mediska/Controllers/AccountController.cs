using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mediska.Classes;
using Mediska.Models;
using Mediska.Models.Repository;
namespace Mediska.Controllers
{
    public class AccountController : Controller
    {
        repUser RepU = new repUser();
        repSetting RepSet = new repSetting();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        // GET: Account
        public ActionResult Login(string ReturnUrl)
        {
            if (Session["User"] != null)
                return RedirectToAction("Index", "Home");

            clsLoginModel UObj = new clsLoginModel();

            if (Request.Cookies["Login"] != null)
            {
                try
                {
                    UObj.userMobile = Crypto.Decrypt(Request.Cookies["Login"].Values["UserName"], "!Ts#uI98PoK9A';s");
                }
                catch (Exception)
                {
                }

                try
                {
                    UObj.userPassword = Crypto.Decrypt(Request.Cookies["Login"].Values["Password"], "!Ts#uI98PoK9A';s");
                }
                catch (Exception)
                {
                }
                UObj.rememberMe = true;
            }

            UObj.ReturnUrl = ReturnUrl;
            UObj.rememberMe = true;
            return View(UObj);
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public async Task<ActionResult> Login(clsLoginModel user)
        {
            user.userMobile = clsPublic.GetEnglishNumber(user.userMobile);
            user.userPassword = clsPublic.GetEnglishNumber(user.userPassword);
            if (!ModelState.IsValid)
            {
                //ViewBag.ReturnUrl = user.ReturnUrl;
                return View(user);
            }
            else
            {
                var isCaptchaValid = await clsPublic.IsCaptchaValid(Request, user.GoogleCaptchaToken, "Login");
                if (!isCaptchaValid)
                {
                    ModelState.AddModelError("GoogleCaptcha", "گزینه امنیتی تایید نشده است");
                    return View(user);
                }

                var U = RepU.GetCustomerInfoByMobile(user.userMobile);

                if (U != null)
                {
                    _ = Hash.ComputeHash(user.userPassword, Hash.enmHashAlgorithm.SHA256, null);
                    bool IsCorrect = Hash.VerifyHash(user.userPassword, Hash.enmHashAlgorithm.SHA256, U.custusrPassword);
                    if (IsCorrect)
                    {
                        if (U.custusrActivated)
                        {
                            if (!U.custusrPasswordMustChange)
                            {
                                //userID
                                Session["userID"] = U.ID;
                                Session["CustomerFullName"] = U.custusrManagerName + " " + U.custusrManagerFamily;
                                if (string.IsNullOrEmpty(user.ReturnUrl))
                                    return RedirectToAction("Index", "Home");
                                else
                                    return Redirect(user.ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("ChangePassword", "Account", new { guid = U.custusrChangePasswordGUID });
                            }
                        }
                        else
                        {
                            return RedirectToAction("VerifySMSCode", "Account", new { guid = U.custusrAuthCodeGUID });
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "نام کاربری یا رمز عبور صحیح نمی باشد";
                        ViewBag.ReturnUrl = user.ReturnUrl;
                        return View(user);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "نام کاربری یا رمز عبور صحیح نمی باشد";
                    ViewBag.ReturnUrl = user.ReturnUrl;
                    return View(user);
                }

                //var tokenBased = string.Empty;
                //using (var client = new HttpClient())
                //{
                //    client.BaseAddress = new Uri(HttpContext.Request.Url.ToString().Replace(HttpContext.Request.RawUrl.ToString(), ""));
                //    client.DefaultRequestHeaders.Clear();
                //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //    var responseMessage = await client.PostAsJsonAsync("api/Account/ValidateLogin", new clsLoginModel { userName = user.userName, userPassword = user.userPassword });
                //    if (responseMessage.IsSuccessStatusCode)
                //    {
                //        var resultMessage = responseMessage.Content.ReadAsStringAsync().Result;
                //        tokenBased = JsonConvert.DeserializeObject<string>(resultMessage);

                //        FormsAuthentication.SetAuthCookie(user.userName, user.rememberMe);

                //        if (user.rememberMe)
                //        {
                //            try
                //            {
                //                HttpCookie cookie = new HttpCookie("Login");
                //                cookie.Values.Add("UserName", Crypto.Encrypt(user.userName, "!Ts#uI98PoK9A';s"));
                //                cookie.Values.Add("Password", Crypto.Encrypt(user.userPassword, "!Ts#uI98PoK9A';s"));
                //                cookie.Expires = DateTime.Now.AddDays(15);
                //                Response.Cookies.Add(cookie);
                //            }
                //            catch (Exception)
                //            {
                //            }
                //        }

                //        try
                //        {
                //            HttpCookie Tokencookie = new HttpCookie("Auth");
                //            Tokencookie.Values.Add("Token", Crypto.Encrypt(tokenBased, "!Ts#uI98PoK9A';s"));
                //            Tokencookie.Expires = DateTime.Now.AddHours(2);
                //            Response.Cookies.Add(Tokencookie);
                //            //var U = clsTokenManager.ValidateToken(tokenBased);
                //        }
                //        catch (Exception)
                //        {
                //        }

                //        if (string.IsNullOrEmpty(user.ReturnUrl))
                //            return RedirectToAction("Index", "Home");
                //        else
                //            return Redirect(user.ReturnUrl);
                //    }
                //    else
                //    {
                //        ViewBag.ErrorMessage = "نام کاربری یا رمز عبور صحیح نمی باشد";
                //        ViewBag.ReturnUrl = user.ReturnUrl;
                //        return View(user);
                //    }
                //}


            }
        }

        public ActionResult Logout()
        {
            Session["userID"] = null;
            Session["CustomerFullName"] = null;
            return RedirectToAction("Index", "Home");
        }
        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================

        public ActionResult Register(string ReturnUrl)
        {
            if (Session["userID"] != null)
                return RedirectToAction("Index", "Home");

            clsRegisterModel user = new clsRegisterModel();
            user.Gender = true;
            user.ReturnUrl = ReturnUrl;
            return View(user);
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public async Task<ActionResult> Register(clsRegisterModel user)
        {
            if (Session["userID"] != null)
                return RedirectToAction("Index", "Home");

            var isCaptchaValid = await clsPublic.IsCaptchaValid(Request, user.GoogleCaptchaToken, "Register");
            if (!isCaptchaValid)
            {
                ModelState.AddModelError("GoogleCaptcha", "گزینه امنیتی تایید نشده است");
                return View(user);
            }
            user.Mobile = clsPublic.GetEnglishNumber(user.Mobile);
            user.Password = clsPublic.GetEnglishNumber(user.Password);
            user.Password = clsPublic.GetStandardCharacter(user.Password);

            user.ConfirmPassword = clsPublic.GetEnglishNumber(user.ConfirmPassword);
            user.ConfirmPassword = clsPublic.GetStandardCharacter(user.ConfirmPassword);

            user.FName = clsPublic.GetEnglishNumber(user.FName);
            user.FName = clsPublic.GetStandardCharacter(user.FName);

            user.LName = clsPublic.GetEnglishNumber(user.LName);
            user.LName = clsPublic.GetStandardCharacter(user.LName);

            if (ModelState.IsValid)
            {
                string errmsg = "";
                var s = Hash.ComputeHash(user.Password, Hash.enmHashAlgorithm.SHA256, null);
                var RU = RepU.RegisterUser(user.Mobile, s, user.FName, user.LName, user.Gender, ref errmsg);
                if (RU != null)
                {
                    var RandomCode = clsPublic.GenerateRandomCode(6);
                    var MyGUID = Guid.NewGuid().ToString().Replace("-", "");

                    RepU.UpdateUserAuthCode(RU.ID, RandomCode, MyGUID);
                    _ = RepSet.GetSMSSetting();
                    long RecID = 0;
                    byte Status = 0;
                    try
                    {
                        int Res = clsSMS.Send(RU.custusrMobileNo, "کد فعالسازی شما : " + RandomCode + Environment.NewLine + "وبسایت مدیسکا", ref RecID, ref Status);
                    }
                    catch (Exception)
                    {
                        ViewBag.ErrorMessage = "ارسال کد فعالسازی موفقیت آمیز نبود";
                    }
                    return RedirectToAction("VerifySMSCode", "Account", new { guid = MyGUID });
                }
                else
                {
                    ViewBag.ErrorMessage = errmsg;
                    return View(user);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده صحیح نمی باشد";
                return View(user);
            }


        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public ActionResult VerifySMSCode(string guid)
        {
            clsGuidCode gc = new clsGuidCode();
            gc.Guid = guid;
            return View(gc);
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public async Task<ActionResult> VerifySMSCode(clsGuidCode gc)
        {
            if (ModelState.IsValid)
            {
                var isCaptchaValid = await clsPublic.IsCaptchaValid(Request, gc.GoogleCaptchaToken, "Verify");
                if (!isCaptchaValid)
                {
                    ModelState.AddModelError("GoogleCaptcha", "گزینه امنیتی تایید نشده است");
                    return View(gc);
                }


                var U = RepU.GetCustomerInfoByAuthGUID(gc.Guid);
                if (U != null)
                {
                    if (!U.custusrActivated)
                    {
                        var Ecode = clsPublic.GetEnglishNumber(gc.Code);
                        if (U.custusrAuthCode == Ecode || U.custusrAuthCode == gc.Code)
                        {
                            RepU.ActiveUser(U.ID, true);
                            Session["userID"] = U.ID;
                            Session["CustomerFullName"] = U.custusrManagerName + " " + U.custusrManagerFamily;
                            ViewBag.SuccessMessage = "حساب کاربری شما با موفقیت فعال شد";
                            TempData["SuccessMessage"] = "حساب کاربری شما با موفقیت فعال شد";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "کد وارد شده صحیح نمی باشد";
                            return View(gc);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "این حساب کاربری قبلا فعال شده است";
                        return View(gc);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "کاربر مورد نظر یافت نشد";
                    return View(gc);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده صحیح نیست";
                return View(gc);
            }

        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public JsonResult ResendAuthCodeSMS(string guid)
        {
            var U = RepU.GetCustomerInfoByAuthGUID(guid);
            if (U != null)
            {
                if (!U.custusrActivated)
                {
                    var RandomCode = clsPublic.GenerateRandomCode(6);

                    RepU.UpdateUserAuthCode(U.ID, RandomCode, guid);
                    _ = RepSet.GetSMSSetting();
                    long RecID = 0;
                    byte Status = 0;
                    try
                    {
                        int Res = clsSMS.Send(U.custusrMobileNo, "کد فعالسازی شما : " + RandomCode + Environment.NewLine + "وبسایت مدیسکا", ref RecID, ref Status);
                    }
                    catch (Exception)
                    {
                        return Json(new { Status = "Error", Message = "ارسال کد فعالسازی موفقیت آمیز نبود" });
                    }

                    return Json(new { Status = "Success", Message = "کد فعالسازی جدید، مجددا به شماره " + U.custusrMobileNo + " ارسال شد." });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "این حساب کاربری قبلا فعال شده است" });
                }
            }
            else
            {
                return Json(new { Status = "Error", Message = "کاربر مورد نظر یافت نشد" });
            }
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public ActionResult ForgetPassword()
        {
            return View();
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public async Task<ActionResult> ForgetPassword(clsMobile mobileObj)
        {
            mobileObj.Mobile = clsPublic.GetEnglishNumber(mobileObj.Mobile);
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده صحیح نمی باشد";
                return View(mobileObj);
            }
            else
            {
                var isCaptchaValid = await clsPublic.IsCaptchaValid(Request, mobileObj.GoogleCaptchaToken, "ForgetPass");
                if (!isCaptchaValid)
                {
                    ViewBag.ErrorMessage = "گزینه امنیتی تایید نشده است";
                    return View(mobileObj);
                }

                var U = RepU.GetCustomerInfoByMobile(mobileObj.Mobile);
                if (U != null)
                {
                    if (U.custusrChangePasswordDateTime == null || U.custusrChangePasswordDateTime < U.NowDate)
                    {
                        var NewPassword = clsPublic.GenerateRandomCode(6);
                        var MyGUID = Guid.NewGuid().ToString().Replace("-", "");
                        var HashPass = Hash.ComputeHash(NewPassword, Hash.enmHashAlgorithm.SHA256, null);
                        RepU.ChangePassword(U.ID, HashPass, true, MyGUID, 3);
                        long RecID = 0;
                        byte Status = 0;
                        try
                        {
                            int Res = clsSMS.Send(U.custusrMobileNo, "رمز عبور جدید شما : " + NewPassword + Environment.NewLine + "باید بعد از ورود به سایت، رمز خود را تغییر دهید" + Environment.NewLine + "وبسایت مدیسکا", ref RecID, ref Status);
                        }
                        catch (Exception)
                        {
                            ViewBag.ErrorMessage = "ارسال کد فعالسازی موفقیت آمیز نبود";
                            return View(mobileObj);
                        }
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "امکان ارسال مجدد پیامک، زودتر از 3 دقیقه وجود ندارد ";
                        return View(mobileObj);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "کاربر مورد نظر یافت نشد";
                    return View(mobileObj);
                }
            }
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public ActionResult ChangePassword(string guid)
        {
            if (string.IsNullOrEmpty(guid) && Session["userID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            clsChangePassword cp = new clsChangePassword();

            cp.Guid = guid;
            if (string.IsNullOrEmpty(guid))
                cp.HasSMSPassword = false;
            else
            {
                if (Session["userID"] == null)
                    cp.HasSMSPassword = true;
                else
                {
                    cp.HasSMSPassword = false;
                    cp.CustomerID = Convert.ToInt32(Session["userID"]);
                }
            }
            return View(cp);
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public async Task<ActionResult> ChangePassword(clsChangePassword CP)
        {
            CP.OldPassword = clsPublic.GetEnglishNumber(CP.OldPassword);
            CP.NewPassword = clsPublic.GetEnglishNumber(CP.NewPassword);
            CP.ConfirmPassword = clsPublic.GetEnglishNumber(CP.ConfirmPassword);

            if (!ModelState.IsValid)
            {
                //ViewBag.ReturnUrl = user.ReturnUrl;
                ViewBag.ErrorMessage = "در اطلاعات وارد شده خطا وجود دارد";
                return View(CP);
            }

            var isCaptchaValid = await clsPublic.IsCaptchaValid(Request, CP.GoogleCaptchaToken, "ChangePassword");
            if (!isCaptchaValid)
            {
                ViewBag.ErrorMessage = "گزینه امنیتی تایید نشده است";
                return View(CP);
            }

            //    ModelState.AddModelError("GoogleCaptcha", "گزینه امنیتی تایید نشده است");
            var result = false;
            cmplxCustomerInfoByID U = null;

            if (CP.HasSMSPassword)
            {
                U = RepU.GetCustomerInfoByChangePasswordGUID(CP.Guid);
            }
            else
            {
                int CustomerID = Convert.ToInt32(Session["userID"]);
                U = RepU.GetCustomerInfoByID(CustomerID);
            }

            if (U == null)
            {
                ViewBag.ErrorMessage = "کاربر پیدا نشد";
                return View(CP);
            }

            var CurPass = U.custusrPassword;


            if (!Hash.VerifyHash(CP.OldPassword, Hash.enmHashAlgorithm.SHA256, CurPass))
            {
                ModelState.AddModelError("OldPassword", "رمز عبور فعلی اشتباه است");
                ViewBag.ErrorMessage = "در اطلاعات وارد شده خطا وجود دارد";
                return View(CP);
            }

            var HashPass = Hash.ComputeHash(CP.NewPassword, Hash.enmHashAlgorithm.SHA256, null);
            result = RepU.ChangePassword(U.ID, HashPass, false, CP.Guid, 3);

            Session["userID"] = U.ID;
            Session["CustomerFullName"] = U.custusrManagerName + " " + U.custusrManagerFamily;

            if (result == true)
            {
                TempData["SuccessMessage"] = "رمز با موفقیت تغییر یافت";
                return RedirectToAction("index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "ذخیره رمز عبور با خطا مواجه شد";
                return View(CP);
            }

        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public ActionResult LoginOTP()
        {
            return View();
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public async Task<ActionResult> LoginOTP(clsMobile mobileObj)
        {
            mobileObj.Mobile = clsPublic.GetEnglishNumber(mobileObj.Mobile);
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده صحیح نمی باشد";
                return View(mobileObj);
            }
            else
            {
                var isCaptchaValid = await clsPublic.IsCaptchaValid(Request, mobileObj.GoogleCaptchaToken, "LoginOTP");
                if (!isCaptchaValid)
                {
                    ViewBag.ErrorMessage = "گزینه امنیتی تایید نشده است";
                    return View(mobileObj);
                }

                var U = RepU.GetCustomerInfoByMobile(mobileObj.Mobile);
                if (U != null)
                {
                    if (U.custusrOTPDateTime == null || U.custusrOTPDateTime < U.NowDate)
                    {
                        var OTP = clsPublic.GenerateRandomCode(4);
                        var MyGUID = Guid.NewGuid().ToString().Replace("-", "");
                        RepU.UpdateUserOTPCode(U.ID, OTP, MyGUID, 2);
                        long RecID = 0;
                        byte Status = 0;
                        try
                        {
                            int Res = clsSMS.Send(U.custusrMobileNo, "رمز یکبار مصرف شما : " + OTP + Environment.NewLine + "وبسایت مدیسکا", ref RecID, ref Status);
                        }
                        catch (Exception)
                        {
                            ViewBag.ErrorMessage = "ارسال کد فعالسازی موفقیت آمیز نبود";
                            return View(mobileObj);
                        }

                        return RedirectToAction("VerifyOTP", "Account", new { guid = MyGUID });
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "حداقل زمان برای ارسال مجدد پیامک، 2 دقیقه است";
                        return View(mobileObj);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "کاربر یافت نشد";
                    return View(mobileObj);
                }
            }

        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public ActionResult VerifyOTP(string guid)
        {
            clsGuidCode gc = new clsGuidCode();
            gc.Guid = guid;
            return View(gc);
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public async Task<ActionResult> VerifyOTP(clsGuidCode gc)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "اطلاعات وارد شده صحیح نمی باشد";
                return View(gc);
            }
            else
            {
                var isCaptchaValid = await clsPublic.IsCaptchaValid(Request, gc.GoogleCaptchaToken, "VerifyOTP");
                if (!isCaptchaValid)
                {
                    ModelState.AddModelError("GoogleCaptcha", "گزینه امنیتی تایید نشده است");
                    return View(gc);
                }


                var U = RepU.GetCustomerInfoByOTPGUID(gc.Guid);
                if (U == null)
                {
                    ViewBag.ErrorMessage = "کاربر یافت نشد";
                    return View(gc);
                }
                else
                {
                    gc.Code = clsPublic.GetEnglishNumber(gc.Code);
                    var OTP_pass = U.custusrOTP;
                    if (OTP_pass == gc.Code)
                    {
                        Session["userID"] = U.ID;
                        Session["CustomerFullName"] = U.custusrManagerName + " " + U.custusrManagerFamily;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "رمز وارد شده صحیح نمی باشد";
                        return View(gc);
                    }
                }

            }
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public JsonResult ResendOTPSMS(string guid)
        {
            
            var U = RepU.GetCustomerInfoByOTPGUID(guid);

            if (U != null)
            {
                if (U.custusrOTPDateTime == null || U.custusrOTPDateTime < U.NowDate)
                {
                    var RandomCode = clsPublic.GenerateRandomCode(4);
                    RepU.UpdateUserOTPCode(U.ID, RandomCode, guid, 2);
                    long RecID = 0;
                    byte Status = 0;
                    try
                    {
                        int Res = clsSMS.Send(U.custusrMobileNo, "رمز یکبار مصرف شما : " + RandomCode + Environment.NewLine + "وبسایت مدیسکا", ref RecID, ref Status);
                    }
                    catch (Exception)
                    {
                        return Json(new { Status = "Error", Message = "ارسال رمز یکبار مصرف موفقیت آمیز نبود" });
                    }

                    return Json(new { Status = "Success", Message = "رمز یکبار مصرف شما، مجددا به شماره " + U.custusrMobileNo + " ارسال شد." });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "حداقل زمان برای ارسال مجدد پیامک، 2 دقیقه است" });
                }
            }
            else
            {
                return Json(new { Status = "Error", Message = "کاربر مورد نظر یافت نشد" });
            }
        }


    }
}

