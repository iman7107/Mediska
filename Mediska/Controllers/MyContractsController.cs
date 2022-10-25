using Mediska.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mediska.Controllers
{
    public class MyContractsController : Controller
    {
        private readonly repMyContracts bll ;
        public MyContractsController()
        {
            bll = new repMyContracts();
        }
        public ActionResult Index()
        {
            var result = bll.ContractList(Session["userID"] as int?);
            return View(result);
        }
    }
}