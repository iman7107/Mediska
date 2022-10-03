﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mediska.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SupportContext : DbContext
    {
        public SupportContext()
            : base("name=SupportContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<cmplxProduct> MDSKGetProductList(Nullable<int> iD)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxProduct>("MDSKGetProductList", iDParameter);
        }
    
        public virtual ObjectResult<cmplxProductGroup> MDSKGetProductGroupList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxProductGroup>("MDSKGetProductGroupList");
        }
    
        public virtual ObjectResult<cmplxCustomerInfoByMobile> MDSKGetCustomerInfoByMobile(string mobileNo)
        {
            var mobileNoParameter = mobileNo != null ?
                new ObjectParameter("MobileNo", mobileNo) :
                new ObjectParameter("MobileNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxCustomerInfoByMobile>("MDSKGetCustomerInfoByMobile", mobileNoParameter);
        }
    
        public virtual ObjectResult<cmplxCustomerInfoByMobile> MDSKRegisterUser(string mobileNo, string password, string fName, string lName, Nullable<bool> gender)
        {
            var mobileNoParameter = mobileNo != null ?
                new ObjectParameter("MobileNo", mobileNo) :
                new ObjectParameter("MobileNo", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var fNameParameter = fName != null ?
                new ObjectParameter("FName", fName) :
                new ObjectParameter("FName", typeof(string));
    
            var lNameParameter = lName != null ?
                new ObjectParameter("LName", lName) :
                new ObjectParameter("LName", typeof(string));
    
            var genderParameter = gender.HasValue ?
                new ObjectParameter("Gender", gender) :
                new ObjectParameter("Gender", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxCustomerInfoByMobile>("MDSKRegisterUser", mobileNoParameter, passwordParameter, fNameParameter, lNameParameter, genderParameter);
        }
    
        public virtual ObjectResult<cmplxSMSSetting> MDSKGetSMSSetting()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxSMSSetting>("MDSKGetSMSSetting");
        }
    
        public virtual int MDSKUpdateCustomerUserAuthCode(Nullable<int> iD, string authCode, string gUID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            var authCodeParameter = authCode != null ?
                new ObjectParameter("AuthCode", authCode) :
                new ObjectParameter("AuthCode", typeof(string));
    
            var gUIDParameter = gUID != null ?
                new ObjectParameter("GUID", gUID) :
                new ObjectParameter("GUID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MDSKUpdateCustomerUserAuthCode", iDParameter, authCodeParameter, gUIDParameter);
        }
    
        public virtual ObjectResult<cmplxCustomerInfoByID> MDSKGetCustomerInfoByID(Nullable<int> iD)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxCustomerInfoByID>("MDSKGetCustomerInfoByID", iDParameter);
        }
    
        public virtual ObjectResult<cmplxCustomerInfoByID> MDSKGetCustomerInfoByAuthGUID(string gUID)
        {
            var gUIDParameter = gUID != null ?
                new ObjectParameter("GUID", gUID) :
                new ObjectParameter("GUID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxCustomerInfoByID>("MDSKGetCustomerInfoByAuthGUID", gUIDParameter);
        }
    
        public virtual int MDSKChangeUserActive(Nullable<int> iD, Nullable<bool> active)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            var activeParameter = active.HasValue ?
                new ObjectParameter("Active", active) :
                new ObjectParameter("Active", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MDSKChangeUserActive", iDParameter, activeParameter);
        }
    
        public virtual int MDSKChangePassword(Nullable<int> iD, string password, Nullable<bool> mustChangePass, string changePassGUID, Nullable<int> stopChangeSMSInMinute)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var mustChangePassParameter = mustChangePass.HasValue ?
                new ObjectParameter("MustChangePass", mustChangePass) :
                new ObjectParameter("MustChangePass", typeof(bool));
    
            var changePassGUIDParameter = changePassGUID != null ?
                new ObjectParameter("ChangePassGUID", changePassGUID) :
                new ObjectParameter("ChangePassGUID", typeof(string));
    
            var stopChangeSMSInMinuteParameter = stopChangeSMSInMinute.HasValue ?
                new ObjectParameter("StopChangeSMSInMinute", stopChangeSMSInMinute) :
                new ObjectParameter("StopChangeSMSInMinute", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MDSKChangePassword", iDParameter, passwordParameter, mustChangePassParameter, changePassGUIDParameter, stopChangeSMSInMinuteParameter);
        }
    
        public virtual ObjectResult<cmplxCustomerInfoByID> MDSKGetCustomerInfoByChangePassGUID(string gUID)
        {
            var gUIDParameter = gUID != null ?
                new ObjectParameter("GUID", gUID) :
                new ObjectParameter("GUID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxCustomerInfoByID>("MDSKGetCustomerInfoByChangePassGUID", gUIDParameter);
        }
    
        public virtual int MDSKUpdateCustomerUserOTPCode(Nullable<int> iD, string oTPCode, string gUID, Nullable<int> expireInMinute)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            var oTPCodeParameter = oTPCode != null ?
                new ObjectParameter("OTPCode", oTPCode) :
                new ObjectParameter("OTPCode", typeof(string));
    
            var gUIDParameter = gUID != null ?
                new ObjectParameter("GUID", gUID) :
                new ObjectParameter("GUID", typeof(string));
    
            var expireInMinuteParameter = expireInMinute.HasValue ?
                new ObjectParameter("ExpireInMinute", expireInMinute) :
                new ObjectParameter("ExpireInMinute", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MDSKUpdateCustomerUserOTPCode", iDParameter, oTPCodeParameter, gUIDParameter, expireInMinuteParameter);
        }
    
        public virtual ObjectResult<cmplxCustomerInfoByID> MDSKGetCustomerInfoByOTPGUID(string gUID)
        {
            var gUIDParameter = gUID != null ?
                new ObjectParameter("GUID", gUID) :
                new ObjectParameter("GUID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxCustomerInfoByID>("MDSKGetCustomerInfoByOTPGUID", gUIDParameter);
        }
    
        public virtual ObjectResult<cmplxPackage> MDSKGetProductPackages(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxPackage>("MDSKGetProductPackages", productIDParameter);
        }
    
        public virtual ObjectResult<cmplxPackage> MDSKGetPackageListByIDs(string iDs)
        {
            var iDsParameter = iDs != null ?
                new ObjectParameter("IDs", iDs) :
                new ObjectParameter("IDs", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxPackage>("MDSKGetPackageListByIDs", iDsParameter);
        }
    
        public virtual ObjectResult<cmplxPackageSpec> MDSKGetPackageSpecByProductID(Nullable<int> productID, Nullable<int> packageID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var packageIDParameter = packageID.HasValue ?
                new ObjectParameter("PackageID", packageID) :
                new ObjectParameter("PackageID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxPackageSpec>("MDSKGetPackageSpecByProductID", productIDParameter, packageIDParameter);
        }
    
        public virtual ObjectResult<cmplxProduct> MDSKGetProductListByIDs(string iDs)
        {
            var iDsParameter = iDs != null ?
                new ObjectParameter("IDs", iDs) :
                new ObjectParameter("IDs", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxProduct>("MDSKGetProductListByIDs", iDsParameter);
        }
    
        public virtual ObjectResult<cmplxProductPicture> MDSKGetProductPictures(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxProductPicture>("MDSKGetProductPictures", productIDParameter);
        }
    
        public virtual ObjectResult<cmplxPackagePicture> MDSKGetPackagePictures(Nullable<int> productID, Nullable<int> packageID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var packageIDParameter = packageID.HasValue ?
                new ObjectParameter("PackageID", packageID) :
                new ObjectParameter("PackageID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxPackagePicture>("MDSKGetPackagePictures", productIDParameter, packageIDParameter);
        }
    
        public virtual int spCheckOffCode(string offCode)
        {
            var offCodeParameter = offCode != null ?
                new ObjectParameter("OffCode", offCode) :
                new ObjectParameter("OffCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spCheckOffCode", offCodeParameter);
        }
    
        public virtual ObjectResult<cmplxCheckOffCode> MDSKCheckOffCode(string offCode)
        {
            var offCodeParameter = offCode != null ?
                new ObjectParameter("OffCode", offCode) :
                new ObjectParameter("OffCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxCheckOffCode>("MDSKCheckOffCode", offCodeParameter);
        }
    
        public virtual ObjectResult<spMDSKGetCustomerByUserID_Result> spMDSKGetCustomerByUserID(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spMDSKGetCustomerByUserID_Result>("spMDSKGetCustomerByUserID", userIDParameter);
        }
    
        public virtual ObjectResult<cmplxCustomerByUserID> MDSKGetCustomerByUserID(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxCustomerByUserID>("MDSKGetCustomerByUserID", userIDParameter);
        }
    
        public virtual int spMDSKInsertPayment(Nullable<int> customerID, string payReference, Nullable<decimal> price, Nullable<bool> registerSMSSent)
        {
            var customerIDParameter = customerID.HasValue ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(int));
    
            var payReferenceParameter = payReference != null ?
                new ObjectParameter("payReference", payReference) :
                new ObjectParameter("payReference", typeof(string));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var registerSMSSentParameter = registerSMSSent.HasValue ?
                new ObjectParameter("RegisterSMSSent", registerSMSSent) :
                new ObjectParameter("RegisterSMSSent", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spMDSKInsertPayment", customerIDParameter, payReferenceParameter, priceParameter, registerSMSSentParameter);
        }
    
        public virtual int spMDSKInsertContractAndPackage(Nullable<int> contractID, Nullable<int> customerID, string packageIDs, string offCode, Nullable<bool> isConfirm, string onlineLicense1, string onlineLicense2, Nullable<bool> customerAcceptLicense)
        {
            var contractIDParameter = contractID.HasValue ?
                new ObjectParameter("ContractID", contractID) :
                new ObjectParameter("ContractID", typeof(int));
    
            var customerIDParameter = customerID.HasValue ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(int));
    
            var packageIDsParameter = packageIDs != null ?
                new ObjectParameter("PackageIDs", packageIDs) :
                new ObjectParameter("PackageIDs", typeof(string));
    
            var offCodeParameter = offCode != null ?
                new ObjectParameter("OffCode", offCode) :
                new ObjectParameter("OffCode", typeof(string));
    
            var isConfirmParameter = isConfirm.HasValue ?
                new ObjectParameter("IsConfirm", isConfirm) :
                new ObjectParameter("IsConfirm", typeof(bool));
    
            var onlineLicense1Parameter = onlineLicense1 != null ?
                new ObjectParameter("OnlineLicense1", onlineLicense1) :
                new ObjectParameter("OnlineLicense1", typeof(string));
    
            var onlineLicense2Parameter = onlineLicense2 != null ?
                new ObjectParameter("OnlineLicense2", onlineLicense2) :
                new ObjectParameter("OnlineLicense2", typeof(string));
    
            var customerAcceptLicenseParameter = customerAcceptLicense.HasValue ?
                new ObjectParameter("CustomerAcceptLicense", customerAcceptLicense) :
                new ObjectParameter("CustomerAcceptLicense", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spMDSKInsertContractAndPackage", contractIDParameter, customerIDParameter, packageIDsParameter, offCodeParameter, isConfirmParameter, onlineLicense1Parameter, onlineLicense2Parameter, customerAcceptLicenseParameter);
        }
    
        public virtual int spMDSKConfirmContract(Nullable<int> contractID)
        {
            var contractIDParameter = contractID.HasValue ?
                new ObjectParameter("ContractID", contractID) :
                new ObjectParameter("ContractID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spMDSKConfirmContract", contractIDParameter);
        }
    
        public virtual ObjectResult<cmplxGetUnConfirmPackage> MDSKGetUnConfirmPackage(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxGetUnConfirmPackage>("MDSKGetUnConfirmPackage", userIDParameter);
        }
    
        public virtual int MDSKInsertCustomer(Nullable<int> userID, string customerCompanyName, string customerManagerName, string customerManagerFamily, string customerManagerMobileNo, string customerMelliNo, Nullable<System.DateTime> customerBirthDate, Nullable<int> customerCustomerGroupID, Nullable<bool> customerManagerGender, Nullable<int> customerAreaID, string customerAddress)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var customerCompanyNameParameter = customerCompanyName != null ?
                new ObjectParameter("customerCompanyName", customerCompanyName) :
                new ObjectParameter("customerCompanyName", typeof(string));
    
            var customerManagerNameParameter = customerManagerName != null ?
                new ObjectParameter("customerManagerName", customerManagerName) :
                new ObjectParameter("customerManagerName", typeof(string));
    
            var customerManagerFamilyParameter = customerManagerFamily != null ?
                new ObjectParameter("customerManagerFamily", customerManagerFamily) :
                new ObjectParameter("customerManagerFamily", typeof(string));
    
            var customerManagerMobileNoParameter = customerManagerMobileNo != null ?
                new ObjectParameter("customerManagerMobileNo", customerManagerMobileNo) :
                new ObjectParameter("customerManagerMobileNo", typeof(string));
    
            var customerMelliNoParameter = customerMelliNo != null ?
                new ObjectParameter("customerMelliNo", customerMelliNo) :
                new ObjectParameter("customerMelliNo", typeof(string));
    
            var customerBirthDateParameter = customerBirthDate.HasValue ?
                new ObjectParameter("customerBirthDate", customerBirthDate) :
                new ObjectParameter("customerBirthDate", typeof(System.DateTime));
    
            var customerCustomerGroupIDParameter = customerCustomerGroupID.HasValue ?
                new ObjectParameter("customerCustomerGroupID", customerCustomerGroupID) :
                new ObjectParameter("customerCustomerGroupID", typeof(int));
    
            var customerManagerGenderParameter = customerManagerGender.HasValue ?
                new ObjectParameter("customerManagerGender", customerManagerGender) :
                new ObjectParameter("customerManagerGender", typeof(bool));
    
            var customerAreaIDParameter = customerAreaID.HasValue ?
                new ObjectParameter("customerAreaID", customerAreaID) :
                new ObjectParameter("customerAreaID", typeof(int));
    
            var customerAddressParameter = customerAddress != null ?
                new ObjectParameter("CustomerAddress", customerAddress) :
                new ObjectParameter("CustomerAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MDSKInsertCustomer", userIDParameter, customerCompanyNameParameter, customerManagerNameParameter, customerManagerFamilyParameter, customerManagerMobileNoParameter, customerMelliNoParameter, customerBirthDateParameter, customerCustomerGroupIDParameter, customerManagerGenderParameter, customerAreaIDParameter, customerAddressParameter);
        }
    
        public virtual ObjectResult<cmplxGetAreaList> MDSKGetAreaList(string filter)
        {
            var filterParameter = filter != null ?
                new ObjectParameter("Filter", filter) :
                new ObjectParameter("Filter", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxGetAreaList>("MDSKGetAreaList", filterParameter);
        }
    
        public virtual int spMDSKGetCustomerGroup()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spMDSKGetCustomerGroup");
        }
    
        public virtual ObjectResult<cmplxGetCustomerGroup> MDSKGetCustomerGroup()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<cmplxGetCustomerGroup>("MDSKGetCustomerGroup");
        }
    
        public virtual ObjectResult<spMDSKCheckOffCode_Result> spMDSKCheckOffCode(string offCode)
        {
            var offCodeParameter = offCode != null ?
                new ObjectParameter("OffCode", offCode) :
                new ObjectParameter("OffCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spMDSKCheckOffCode_Result>("spMDSKCheckOffCode", offCodeParameter);
        }
    }
}
