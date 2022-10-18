using Mediska.Classes;
using Mediska.Models;
using Mediska.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Web.Helpers;
using System.Web.Mvc;


namespace Mediska.Controllers
{
    public class ShopCartController : BaseController
    {
        repProduct bll = new repProduct();

        #region Ajax
        public JsonResult myDiscountList(string offCode)
        {
            try
            {
                List<cmplxCheckOffCode> list = bll.CheckOffCode(offCode);
                return Json(new { Status = "Success", Message = "", Data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Message = myErrorMessage(ex), Data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult myAreaList(string query)
        {

            List<clsSelect> list = bll.AreaList(query);

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public string myGetProductLicenseAgreement(int? productID)
        {
            return bll.GetProductLicenseAgreement(productID);
        }
        public JsonResult myCustomerGroupList()
        {

            var list = bll.GetCustomerGroup();

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public JsonResult myCustomerList()
        {

            var list = new repUser().GetCustomerInfoByUserID(Session["CustomerID"] as int?);

            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion

        // GET: ShopCart
        public ActionResult Index()
        {
            Session["OffCodeList"] = null;
            return View();
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public int AddToCart(int id)
        {
            List<int> cart = new List<int>();

            if (Session["ShopCart"] != null)
            {
                cart = Session["ShopCart"] as List<int>;
            }

            if (cart.Any(p => p == id))
            {
                return -1;
            }
            else
            {
                cart.Add(id);
            }

            Session["ShopCart"] = cart;

            return cart.Count;
        }


        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public JsonResult ShopCartCount(int? productID)
        {
            int count = 0;
            int SumPrice = 0;
            if (Session["ShopCart"] != null)
            {
                List<int> cart = Session["ShopCart"] as List<int>;
                var packages = bll.GetPackagesByIDs(cart);
                count = cart.Count;
                if (packages.Count > 0)
                    SumPrice = Convert.ToInt32(packages.Where(i => productID == null || i.packageProductID == productID).Sum(i => i.packagePrice));
            }

            var JR = Json(new { Count = count, SumPrice = SumPrice, SumPriceText = String.Format("{0:N0}", SumPrice) });
            return JR;
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public int RemoveFromCart(int id)
        {
            List<int> cart = new List<int>();

            if (Session["ShopCart"] != null)
            {
                cart = Session["ShopCart"] as List<int>;
            }

            if (cart.Contains(id))
            {
                cart.Remove(id);
            }

            Session["ShopCart"] = cart;

            return cart.Count;
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public ActionResult RemovePackageFromCart(int id)
        {
            List<int> cart = Session["ShopCart"] as List<int>;
            int index = cart.FindIndex(p => p == id);
            cart.RemoveAt(index);
            Session["ShopCart"] = cart;
            return RedirectToAction("Index");
        }

        #region CompeletCart

        public ActionResult CompeletCart(List<clsCompeletCart> offCodeList)
        {
            if (Session["OffCodeList"] == null)
            {
                Session["OffCodeList"] = offCodeList;
            }

            if (Session["CustomerID"] == null)
            {

                return RedirectToAction("Login", "Account", new { ReturnUrl = "/ShopCart/CompeletCart" });
            }
            else
            {
                var unConfirmPackage = bll.UnConfirmPackage(Session["CustomerID"] as int?);
                if (unConfirmPackage != null)
                {
                    if (unConfirmPackage.Count > 0)
                    {
                        Session["ContractID"] = unConfirmPackage.Select(i => i.ContractID).FirstOrDefault();
                    }
                }
            }
            ViewBag.CustomerList = new repUser().GetCustomerInfoByUserID(Session["CustomerID"] as int?);

            return View();
        }

        [HttpPost]
        public ActionResult FinalCart(List<clsFinalCart> finalCartList)
        {
            if (Session["CustomerID"] == null)
            {
                return Json(new { Status = "Error", Message = "", Data = "" }, JsonRequestBehavior.AllowGet);
            }

            System.Net.ServicePointManager.Expect100Continue = false;
            com.zarinpal.sandbox.PaymentGatewayImplementationService client = new com.zarinpal.sandbox.PaymentGatewayImplementationService();

            string authority;


            string callbackUrl = "https://" + Request.Url.Authority + "/ShopCart/Verify/123";
            int status = client.PaymentRequest("MerchantID", 1000, "description", "4linecode@gmail.com", "09125365099", callbackUrl, out authority);

            if (status == 100)
            {
                ////For release mode
                //Response.Redirect("https://zarinpal.com/pg/StartPay/" + authority);

                var offCodeList = Session["OffCodeList"] as List<clsCompeletCart>;
                if (offCodeList != null && finalCartList != null)
                {
                    foreach (var item in offCodeList)
                    {
                        var finalCart = finalCartList.FirstOrDefault(i => i.ProductID == item.ProductID);
                        string packageIDs = string.Join(";", item.CompeletCartDetails.Select(j => j.PackageID));
                        try
                        {
                            bll.InsertContractAndPackage(Session["ContractID"] as int?, finalCart.CustomerID, packageIDs, item.OffCode, false, finalCart.OnlineLicense1, finalCart.OnlineLicense2, true);

                        }
                        catch (Exception ex)
                        {
                            var errorCode = ex.HResult;
                            var message = myErrorMessage(ex);   
                            throw;
                        }

                    }
                }

                ////For test mode
                var url = "https://sandbox.zarinpal.com/pg/StartPay/" + authority;
                Response.Redirect(url);
                return null;
            }
            TempData["Message"] = GetMessage(status);

            return View("CompeletCart", new { offCodeList = Session["OffCodeList"] });

        }
        #endregion

        public ActionResult Verify(int id)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Status"]) && !string.IsNullOrEmpty(Request.QueryString["Authority"]))
            {
                if (Request.QueryString["Status"].Equals("OK"))
                {
                    int amount = 1000;
                    long refId;
                    ServicePointManager.Expect100Continue = false;
                    com.zarinpal.sandbox.PaymentGatewayImplementationService client = new com.zarinpal.sandbox.PaymentGatewayImplementationService();
                    int status = client.PaymentVerification("MerchantID", Request.QueryString["Authority"], amount, out refId);
                    if (status == 100 || status == 101)
                    {
                        ViewBag.RefId = "کد پیگیری: " + refId + " - کد سفارش: " + id;
                        
                    }
                    else
                    {
                        ViewBag.Message = GetMessage(status);
                    }
                }
                else
                {
                    ViewBag.Message = "کد مرجع: " + Request.QueryString["Authority"] + " - وضعیت:" + Request.QueryString["Status"];
                }
            }
            else
            {
                ViewBag.Message = "ورودی نامعتبر است.";
            }
            return View();
        }

        public string GetMessage(int status)
        {
            switch (status)
            {
                case -1:
                    return "اطلاعات ارسال شده  ناقص است.";
                case -2:
                    return "IP و یا مرچنت کد پذیرنده صحیح نیست.";
                case -3:
                    return "با توجه به محدودیت های شاپرک امکان پرداخت با رقم درخواست شده میسر نمی باشد.";
                case -4:
                    return "سطح تایید پذیرنده پایین تر از سطح نقره ای است.";
                case -11:
                    return "درخواست مورد نظر یافت نشد.";
                case -12:
                    return "امکان ویرایش درخواست میسر نمی باشد.";
                case -21:
                    return "هیچ نوع عملیات مالی برای این تراکنش یافت نشد.";
                case -22:
                    return "تراکنش ناموفق می باشد.";
                case -33:
                    return "رقم تراکنش با رقم پرداخت شده مطابقت ندارد.";
                case -34:
                    return "سقف تقسیم تراکنش از لحاظ تعداد یا رقم عبور نموده است.";
                case -40:
                    return "اجازه دسترسی به متد مربوطه وجود ندارد.";
                case -41:
                    return "اطلاعات ارسال شده مربوط به AdditionalData غیر معتبر می باشد.";
                case -42:
                    return "مدت زمان معتبر طول عمر شناسه پرداخت باید بین 30 دقیقه تا 45 روز می باشد.";
                case -54:
                    return "درخواست مورد نظر آرشیو شده است.";
                case 100:
                    return "عملیات با موفقیت انجام گردیده است.";
                case 101:
                    return "عملیات پرداخت موفق بوده و قبلاً PaymentVerification تراکنش انجام شده است.";
                default:
                    return "کد تعریف نشده.";
            }
        }

        public JsonResult InsertCustomer(Nullable<int> userID, string customerCompanyName, string customerManagerName, string customerManagerFamily, string customerManagerMobileNo, string customerMelliNo, string customerBirthDate, Nullable<int> customerCustomerGroupID, string customerManagerGender, Nullable<int> customerAreaID, string customerAddress)
        {

            try
            {
                var birthDate = Utility.myConvertShamsiToMiladi(customerBirthDate);


                bll.InsertCustomer(Session["CustomerID"] as int?, customerCompanyName, customerManagerName, customerManagerFamily, customerManagerMobileNo, customerMelliNo, birthDate, customerCustomerGroupID, customerManagerGender == "1", customerAreaID, customerAddress);

                return Json(new { Status = "Success", Message = "", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return myErrorMessage(ex);
            }

        }

    }

}