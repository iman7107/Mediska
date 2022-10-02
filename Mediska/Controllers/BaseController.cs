using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mediska.Controllers
{
    public class BaseController:Controller
    {
        protected JsonResult myErrorMessage(Exception ex)
        {
            string ErrorMessage = "";
            if (ex.ToString().Contains("[TSA050]"))
                ErrorMessage = "این کد تخفیف وجود ندارد";
            else if (ex.ToString().Contains("[TSA051]"))
                ErrorMessage = "این کد تخفیف فعال نمی باشد";
            else if (ex.ToString().Contains("[TSA052]"))
                ErrorMessage = "زمان استفاده از این کد تخفیف به پایان رسیده است";
            else if (ex.ToString().Contains("[TSA053]"))
                ErrorMessage = "این کد تخفیف قبلا استفاده شده است";
            else if (ex.ToString().Contains("[TSA054]"))
                ErrorMessage = "هیچ پکیجی انتخاب نشده است";
            else if (ex.ToString().Contains("[TSA055]"))
                ErrorMessage = "پکیجهای انتخاب شده از یک محصول نیستند";
            else if (ex.ToString().Contains("[TSA056]"))
                ErrorMessage = "این کد تخفیف متعلق به ین محصول نمیباشد";
            else if (ex.ToString().Contains("[TSA057]"))
                ErrorMessage = "توافقنامه توسط مشتری تایید نشده است";
            else if (ex.ToString().Contains("[TSA058]"))
                ErrorMessage = "این مشتری قبلا این محصول را خریداری کرده است";
            else
                ErrorMessage = "یک خطای غیر منتظره رخ داده است";


            return Json(new { Status = "Error", Message = ErrorMessage, Data = "" }, JsonRequestBehavior.AllowGet);
        }
    }
}