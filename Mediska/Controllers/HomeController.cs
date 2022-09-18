﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mediska.Models.Repository;

namespace Mediska.Controllers
{
    public class HomeController : Controller
    {
        repProduct repP = new repProduct();

        //===========   =================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public ActionResult Index()
        {
            //TempData["WellcomeMessage"] = "به سایت مدیســکا خوش آمدید";
            return View();
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public ActionResult ContactUs()
        {
            return View();
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        public ActionResult ShowProduct(int? id)
        {
            var p = repP.GetProductByID(id);
            return View(p);
        }

        //============================================================================================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpPost]
        public ActionResult GetProductDetail(int? productID)
        {
            var p=repP.GetProductByID(productID);
            return PartialView("_pwBuyPackage", p);
        }
    }
}