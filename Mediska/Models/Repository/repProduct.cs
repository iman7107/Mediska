using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Web.Http.Results;


namespace Mediska.Models.Repository
{
    public class repProduct
    {
        private SupportContext Context = new SupportContext();
        public List<cmplxProduct> GetProductList()
        {
            var ProductList = Context.MDSKGetProductList(null).ToList();
            return ProductList;
        }

        //============================================================================================================================
        //============================================================================================================================
        public cmplxProduct GetProductByID(int? id)
        {
            var Product = Context.MDSKGetProductList(id).DefaultIfEmpty(null).First();
            return Product;
        }

        public string GetProductLicenseAgreement(int? id)
        {
            var licenseAgreement = Context.MDSKGetProductList(id).Select(i => i.prdxtraLicenseAgreement).FirstOrDefault();
            return licenseAgreement;
        }
        //============================================================================================================================
        //============================================================================================================================
        public List<cmplxProductGroup> GetProductGroup()
        {
            var ProductGroup = Context.MDSKGetProductGroupList().ToList();
            return ProductGroup;
        }

        //============================================================================================================================
        //============================================================================================================================
        public List<cmplxPackage> GetProductPackages(int? productId)
        {
            var PackageProduct = Context.MDSKGetProductPackages(productId).ToList();
            return PackageProduct;
        }

        //============================================================================================================================
        //============================================================================================================================
        public List<cmplxPackage> GetPackagesByIDs(List<int> packageIDs)
        {
            string str = "";
            try
            {
                if (packageIDs != null)
                {
                    foreach (var ID in packageIDs)
                    {
                        str += ID.ToString() + ";";
                    }
                }
                var PackageList = Context.MDSKGetPackageListByIDs(str).ToList();
                return PackageList;

            }
            catch (Exception)
            {
                return new List<cmplxPackage>();
            }
        }

        //============================================================================================================================
        //============================================================================================================================
        public List<cmplxProduct> GetProductsByIDs(List<int> productIDs)
        {
            string str = "";
            try
            {
                if (productIDs != null)
                {
                    foreach (var ID in productIDs)
                    {
                        str += ID.ToString() + ";";
                    }
                }
                var ProductList = Context.MDSKGetProductListByIDs(str).ToList();
                return ProductList;

            }
            catch (Exception)
            {
                return new List<cmplxProduct>();
            }
        }

        //============================================================================================================================
        //============================================================================================================================
        public List<cmplxPackageSpec> GetPackageFeatureByProductID(int? productID, int? packageID)
        {
            var packagefeatures = Context.MDSKGetPackageSpecByProductID(productID, packageID).ToList();
            return packagefeatures;

        }

        //============================================================================================================================
        //============================================================================================================================
        public List<cmplxPackagePicture> GetPackagePicture(int? productID, int? packageID)
        {
            var packagepictures = Context.MDSKGetPackagePictures(productID, packageID).ToList();
            return packagepictures;

        }
        //============================================================================================================================
        //============================================================================================================================
        public List<cmplxProductPicture> GetProductPicture(int? productID)
        {
            var productpictures = Context.MDSKGetProductPictures(productID).ToList();
            return productpictures;

        }

        //===Marimi
        public List<cmplxCheckOffCode> CheckOffCode(string offCode)
        {
            
                var result = Context.MDSKCheckOffCode(offCode).ToList();
                return result;


        }

        public List<clsSelect> AreaList(string filter)
        {

            var result = Context.MDSKGetAreaList(filter).Select(i => new clsSelect { id = i.ID, text = i.areaFullTitle }).ToList();
            return result;

        }

        public int InsertContractAndPackage(Nullable<int> contractID, Nullable<int> customerID, string packageIDs, string offCode, Nullable<bool> isConfirm, string onlineLicense1, string onlineLicense2, Nullable<bool> customerAcceptLicense, Nullable<decimal> payPrice, string payReference)
        {

            return Context.spMDSKInsertContractAndPackage(contractID, customerID, packageIDs, offCode, isConfirm, onlineLicense1, onlineLicense2, customerAcceptLicense, payPrice, payReference);
        }

        public int CheckIsCustomerPackagesValid(Nullable<int> customerID, string packageIDs, string offCode)
        {

            return Context.MDSKCheckIsCustomerPackagesValid(customerID, packageIDs, offCode);
        }
        public List<cmplxGetUnConfirmPackage> UnConfirmPackage(int? userID)
        {

            var result = Context.MDSKGetUnConfirmPackage(userID).ToList();

            return result;

        }
        public int MDSKInsertPayment(Nullable<int> customerID, string payReference, Nullable<decimal> price, Nullable<bool> registerSMSSent)
        {

            var result =  Context.MDSKInsertPayment(customerID, payReference, price, registerSMSSent);
            return result;
        }

        public int InsertCustomer(Nullable<int> userID, string customerCompanyName, string customerManagerName, string customerManagerFamily, string customerManagerMobileNo, string customerMelliNo, Nullable<System.DateTime> customerBirthDate, Nullable<int> customerCustomerGroupID, Nullable<bool> customerManagerGender, Nullable<int> customerAreaID, string customerAddress)
        {

            var result = Context.MDSKInsertCustomer(userID, customerCompanyName, customerManagerName, customerManagerFamily, customerManagerMobileNo, customerMelliNo, customerBirthDate, customerCustomerGroupID, customerManagerGender, customerAreaID, customerAddress);

            return result;
        }

        public List<clsSelect> GetCustomerGroup()
        {
            var result = Context.MDSKGetCustomerGroup().Select(i => new clsSelect { id = i.ID, text = i.custgroupName }).ToList(); ;
            return result;
        }
    }
}