using Mediska.Models;
using Mediska.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
                string ErrorMessage = "";
                if (ex.ToString().Contains("[TSA050]"))
                    ErrorMessage = "این کد تخفیف وجود ندارد";
                else if (ex.ToString().Contains("[TSA051]"))
                    ErrorMessage = "این کد تخفیف فعال نمی باشد";
                else if (ex.ToString().Contains("[TSA052]"))
                    ErrorMessage = "زمان استفاده از این کد تخفیف به پایان رسیده است";
                else if (ex.ToString().Contains("[TSA053]"))
                    ErrorMessage = "این کد تخفیف قبلا استفاده شده است";
                else
                    ErrorMessage = "در بررسی کد تخفیف خطایی رخ داده است";

                return Json(new { Status = "Error", Message = ErrorMessage, Data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult myAreaList(string query)
        {

            List<clsSelect> list = bll.AreaList(query);

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        //public JsonResult myAreaList(string query)
        //{

        //    List<clsSelect> list = bll.AreaList(query);

        //    return Json(list, JsonRequestBehavior.AllowGet);

        //}
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

        public JsonResult FinalCart(List<clsFinalCart> finalCartList)
        {
            if (Session["CustomerID"] == null)
            {
                return Json(new { Status = "Error", Message = "", Data = "" }, JsonRequestBehavior.AllowGet);
            }


            try
            {
                var offCodeList = Session["OffCodeList"] as List<clsCompeletCart>;
                if (offCodeList != null)
                {
                    foreach (var item in offCodeList)
                    {
                        var customerID = finalCartList.Where(i => i.ProductID == item.ProductID).Select(i => i.CustomerID).FirstOrDefault();
                        string packageIDs = string.Join(";", item.CompeletCartDetails.Select(j => j.PackageID));


                        bll.InsertContractAndPackage(Session["ContractID"] as int?, customerID, packageIDs, item.OffCode, false, "0", string.Empty, true);

                    }
                }
                return Json(new { Status = "Success", Message = "", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return myErrorMessage(ex);
            }

        }
        #endregion

        public JsonResult InsertCustomer(Nullable<int> userID, string customerCompanyName, string customerManagerName, string customerManagerFamily, string customerManagerMobileNo, string customerMelliNo, Nullable<System.DateTime> customerBirthDate, Nullable<int> customerCustomerGroupID, Nullable<bool> customerManagerGender, Nullable<int> customerAreaID, string customerAddress)
        {

            try
            {
                bll.InsertCustomer(Session["CustomerID"] as int?, customerCompanyName, customerManagerName, customerManagerFamily, customerManagerMobileNo, customerMelliNo, customerBirthDate, customerCustomerGroupID, customerManagerGender, customerAreaID, customerAddress);

                return Json(new { Status = "Success", Message = "", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return myErrorMessage(ex);
            }

        }

    }

}