using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


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
            var PackageProduct= Context.MDSKGetProductPackages(productId).ToList();
            return PackageProduct;
        }

        //============================================================================================================================
        //============================================================================================================================
        public List<cmplxPackage> GetPackagesByIDs(List<int> packageIDs)
        {
            string str = "";
            try
            {
            if(packageIDs!=null)
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
            var packagefeatures=Context.MDSKGetPackageSpecByProductID(productID, packageID).ToList();
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

                var packagefeatures = Context.MDSKCheckOffCode(offCode).ToList();
                return packagefeatures;

        }
    }
}