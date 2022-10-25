using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediska.Models.Repository
{
    public class repMyContracts
    {
        private readonly SupportContext context ;

        public repMyContracts()
        {
            context = new SupportContext();
        }
        public List<cmplxGetMyContracts> ContractList(int? userID)
        {

            var result = context.MDSKGetMyContracts(userID).ToList();
            return result;

        }

    }
}