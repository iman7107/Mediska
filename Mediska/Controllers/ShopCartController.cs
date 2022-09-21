using Mediska.Models;
using Mediska.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Mediska.Controllers
{
    public class ShopCartController : Controller
    {
        repProduct RepP = new repProduct();

        #region Ajax
        public JsonResult myDiscountList(string offCode)
        {
            List<cmplxCheckOffCode> list = new repProduct().CheckOffCode(offCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        // GET: ShopCart
        public ActionResult Index()
        {
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
                var packages = RepP.GetPackagesByIDs(cart);
                count = cart.Count;
                if(packages.Count>0)
                    SumPrice = Convert.ToInt32(packages.Where(i=> productID==null || i.packageProductID== productID).Sum(i => i.packagePrice));
            }

            var JR= Json(new { Count = count, SumPrice = SumPrice, SumPriceText=String.Format("{0:N0}", SumPrice) });
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



    }
}