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
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;


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
                return myErrorMessage(ex);
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

            var list = new repUser().GetCustomerInfoByUserID(Session["userID"] as int?);

            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion

        // GET: ShopCart
        public ActionResult Index()
        {
            if (Session["ShopCart"] == null)
                return RedirectToAction("Index", "Home");

            //Session["OffCodeList"] = null;
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

            var packages = bll.GetPackagesByIDs(cart);
            var productCount = packages.GroupBy(i => i.packageProductID).Count();

            if (productCount > 1)
            {
                if (cart.Contains(id))
                {
                    cart.Remove(id);
                }
                return -2;
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
                var productCount = packages.GroupBy(i => i.packageProductID).Count();
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
        //public ActionResult RemovePackageFromCart(int id)
        //{
        //    List<int> cart = Session["ShopCart"] as List<int>;
        //    int index = cart.FindIndex(p => p == id);
        //    cart.RemoveAt(index);
        //    Session["ShopCart"] = cart;
        //    return RedirectToAction("Index");
        //}

        public ActionResult RemovePackageFromCart(int id, string offCodes, List<clsCompeletCart> offCodeList)
        {
            List<int> cart = Session["ShopCart"] as List<int>;
            int index = cart.FindIndex(p => p == id);
            if (index > -1)
            {
                cart.RemoveAt(index);
            }
            Session["offCodes"] = offCodes;
            Session["ShopCart"] = cart;

            if (offCodeList != null)
            {

                try
                {
                    var details = offCodeList.Select(i => i.CompeletCartDetails).FirstOrDefault();
                    var detail = details.FirstOrDefault(i => i.PackageID == id);
                    details.Remove(detail);

                    if (offCodeList.Count > 0)
                    {
                        var offCodeCheckList = offCodeList.Where(i => i.OffCode != null).ToList();
                        List<cmplxCheckOffCode> list = null;
                        if (offCodeCheckList != null && offCodeCheckList.Count > 0)
                            list = bll.CheckOffCode(offCodeList.Select(i => i.OffCode).FirstOrDefault());

                        List<clsCompeletCart> offCodeListResult = new List<clsCompeletCart>();
                        List<clsCompeletCartDetail> compeletCartDetailList = new List<clsCompeletCartDetail>();
                        var offCode = offCodeList.FirstOrDefault();
                        clsCompeletCart compeletCart = new clsCompeletCart()
                        {
                            OffCode = offCode.OffCode,
                            ProductID = offCode.ProductID
                        };

                        foreach (var item in offCode.CompeletCartDetails)
                        {
                            var finalPrice = list?.Where(i => i.PackageID == item.PackageID).Select(i => i.FinalPrice).FirstOrDefault() ?? bll.GetPackagesByIDs(offCode.CompeletCartDetails.Select(i => i.PackageID).ToList()).Where(i => i.ID == item.PackageID).Select(i => i.packagePrice).FirstOrDefault();

                            clsCompeletCartDetail compeletCartDetail = new clsCompeletCartDetail()
                            {
                                FinalPrice = finalPrice,
                                PackageID = item.PackageID
                            };
                            compeletCartDetailList.Add(compeletCartDetail);
                        }
                        compeletCart.CompeletCartDetails = compeletCartDetailList;
                        offCodeListResult.Add(compeletCart);
                        Session["OffCodeList"] = offCodeListResult;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", myErrorMessageString(ex));
                }

            }


            return RedirectToAction("Index");
        }
        #region CompeletCart

        public ActionResult CompeletCart(List<clsCompeletCart> offCodeList, string errorMessage = default)
        {

            ViewBag.CustomerList = new repUser().GetCustomerInfoByUserID(Session["userID"] as int?);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError("", errorMessage);
                return View();
            }

            if (offCodeList != null && offCodeList.Count > 0)
            {

                try
                {
                    if (offCodeList.Count > 0)
                    {
                        var offCodeCheckList = offCodeList.Where(i => i.OffCode != null).ToList();
                        List<cmplxCheckOffCode> list = null;
                        if (offCodeCheckList != null && offCodeCheckList.Count > 0)
                            list = bll.CheckOffCode(offCodeList.Select(i => i.OffCode).FirstOrDefault());

                        List<clsCompeletCart> offCodeListResult = new List<clsCompeletCart>();
                        List<clsCompeletCartDetail> compeletCartDetailList = new List<clsCompeletCartDetail>();
                        var offCode = offCodeList.FirstOrDefault();
                        clsCompeletCart compeletCart = new clsCompeletCart()
                        {
                            OffCode = offCode.OffCode,
                            ProductID = offCode.ProductID
                        };

                        foreach (var item in offCode.CompeletCartDetails)
                        {
                            var finalPrice = list?.Where(i => i.PackageID == item.PackageID).Select(i => i.FinalPrice).FirstOrDefault() ?? bll.GetPackagesByIDs(offCode.CompeletCartDetails.Select(i => i.PackageID).ToList()).Where(i => i.ID == item.PackageID).Select(i => i.packagePrice).FirstOrDefault();

                            clsCompeletCartDetail compeletCartDetail = new clsCompeletCartDetail()
                            {
                                FinalPrice = finalPrice,
                                PackageID = item.PackageID
                            };
                            compeletCartDetailList.Add(compeletCartDetail);
                        }
                        compeletCart.CompeletCartDetails = compeletCartDetailList;
                        offCodeListResult.Add(compeletCart);
                        Session["OffCodeList"] = offCodeListResult;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", myErrorMessageString(ex));
                }

            }

            if (Session["userID"] == null)
                return RedirectToAction("Login", "Account", new { ReturnUrl = "/ShopCart/CompeletCart" });

            return View();
        }

        [HttpPost]
        public ActionResult FinalCart(List<clsFinalCart> finalCartList)
        {
            if (Session["userID"] == null)
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

                if (finalCartList != null)
                    Session["FinalCart"] = finalCartList;

                if (offCodeList != null && finalCartList != null)
                {
                    foreach (var item in offCodeList)
                    {
                        var finalCart = finalCartList.FirstOrDefault(i => i.ProductID == item.ProductID);
                        string packageIDs = string.Join(";", item.CompeletCartDetails.Select(j => j.PackageID));
                        try
                        {
                            if (finalCart.CustomerID < 1)
                                return RedirectToAction("CompeletCart", new { offCodeList = Session["OffCodeList"], errorMessage = "انتخاب مرکز الزامی است" });

                            bll.CheckIsCustomerPackagesValid(finalCart.CustomerID, packageIDs + ";", item.OffCode);

                            //var contractID = bll.InsertContractAndPackage(finalCart.ContractID == 0 ? null : finalCart.ContractID, finalCart.CustomerID, packageIDs, item.OffCode, false, finalCart.OnlineLicense1, finalCart.OnlineLicense2, true);

                        }
                        catch (Exception ex)
                        {

                            var errorCode = ex.HResult;
                            var errorMessage = myErrorMessageString(ex);
                            return RedirectToAction("CompeletCart", new { offCodeList = Session["OffCodeList"], errorMessage = errorMessage });
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
                        try
                        {
                            var offCodeList = Session["OffCodeList"] as List<clsCompeletCart>;
                            var finalCartList = Session["FinalCart"] as List<clsFinalCart>;

                            var finalCart = finalCartList.FirstOrDefault(i => i.ProductID == offCodeList?.Select(j => j.ProductID).FirstOrDefault());
                            var offCode = offCodeList.Select(i => i.OffCode).FirstOrDefault();
                            var compeletCartDetails = offCodeList?.Select(i => i.CompeletCartDetails).FirstOrDefault();
                            string packageIDs = string.Join(";", compeletCartDetails.Select(j => j.PackageID));
                            var finalPrice = compeletCartDetails.Select(e => e.FinalPrice).DefaultIfEmpty(0).Sum();

                            var contractID = bll.InsertContractAndPackage(finalCart.ContractID == 0 ? null : finalCart.ContractID, finalCart.CustomerID, packageIDs, offCode, true
                                , finalCart.OnlineLicense1, finalCart.OnlineLicense2, true, finalPrice, refId.ToString());

                            //bll.MDSKInsertPayment(finalCart.CustomerID, refId.ToString(), finalPrice, false);

                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", myErrorMessageString(ex));
                            return View();
                        }

                        ViewBag.RefId = "کد پیگیری: " + refId;
                        Session["offCodes"] = null;
                        Session["ShopCart"] = null;
                        Session["FinalCart"] = null;
                        Session["OffCodeList"] = null;

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


                bll.InsertCustomer(Session["userID"] as int?, customerCompanyName, customerManagerName, customerManagerFamily, customerManagerMobileNo, customerMelliNo, birthDate, customerCustomerGroupID, customerManagerGender == "1", customerAreaID, customerAddress);

                return Json(new { Status = "Success", Message = "", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return myErrorMessage(ex);
            }

        }

    }

}