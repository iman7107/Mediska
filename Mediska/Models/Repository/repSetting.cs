using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediska.Models.Repository
{
    public class repSetting
    {
        private SupportContext Context = new SupportContext();
        public cmplxSMSSetting GetSMSSetting()
        {
            var Product = Context.MDSKGetSMSSetting().DefaultIfEmpty(null).First();
            return Product;
        }

    }
}