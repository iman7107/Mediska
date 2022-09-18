using Mediska.Models.Repository;
using Mediska.SMSSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Mediska.Classes
{
    public class clsSMS
    {
        #region Fields
        private static Send SMSSender;
        private static repSetting RepSet;
        private static string UserName;
        private static string Password;
        private static string FromNumber;
        #endregion

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        static clsSMS()
        {
            RepSet = new repSetting();
            var Sett = RepSet.GetSMSSetting();

            SMSSender = new Send();
            SMSSender.Url = Sett.setSMSCenterWebServiceAddress;
        }

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        public static void RefreshSettings()
        {
            RepSet = new repSetting();
            var Sett = RepSet.GetSMSSetting();
            if (SMSSender != null)
                SMSSender.Url = Sett.setSMSCenterWebServiceAddress;

            UserName = Sett.setSMSCenterUserName;
            Password = Sett.setSMSCenterPassword;
            FromNumber = Sett.setSMSCenterFromNo;
        }

        #region Send

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        private static int SendSMS(string UserName, string Password, string[] To, string From, string Text, ref long[] RecID, ref byte[] Status)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            return SMSSender.SendSms(UserName, Password, To, From, Text, false, "", ref RecID, ref Status);
        }

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        private static string SendByKhadamatiSMS(string UserName, string Password, string To, string[] MessageParameters, int BodyID)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            return SMSSender.SendByBaseNumber(UserName, Password, MessageParameters, To, BodyID);
        }

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        public static int Send(string To, string Text, ref long RecID, ref byte Status)
        {
            long[] RecList = null;
            byte[] StatusList = null;
            string[] ToList = new string[1];

            if ((String.IsNullOrEmpty(UserName)) || (String.IsNullOrEmpty(Password)) || (String.IsNullOrEmpty(FromNumber)))
            {
                RepSet = new repSetting();
                var Sett = RepSet.GetSMSSetting();
                if (Sett != null)
                {
                    UserName = Sett.setSMSCenterUserName;
                    Password = Sett.setSMSCenterPassword;
                    FromNumber = Sett.setSMSCenterFromNo;
                }
            }

            ToList[0] = To;
            int RetVal = SendSMS(UserName, Password, ToList, FromNumber, Text, ref RecList, ref StatusList);
            switch (RetVal)
            {
                case 0:
                    throw new clsSMSException("نام کاربری یا رمز عبور اشتباه می باشد");
                case 1:
                    if (StatusList.Length > 0)
                    {
                        if (StatusList[0] == 0)
                        {
                            RecID = RecList[0];
                            return 1;
                        }
                        else
                            return 0;
                    }
                    //throw new clsSMSException("درخواست با موفقیت انجام شد");
                    break;

                case 2:
                    throw new clsSMSException("اعتبار کافی نمی باشد");
                case 3:
                    throw new clsSMSException("محدودیت در ارسال روزانه");
                case 4:
                    throw new clsSMSException("محدودیت در حجم ارسال");
                case 5:
                    throw new clsSMSException("شماره فرستنده معتبر نمی باشد");
                case 6:
                    throw new clsSMSException("سامانه در حال بروزرسانی می باشد");
                case 7:
                    throw new clsSMSException("متن حاوی کلمه فیلتر شده می باشد");
                case 9:
                    throw new clsSMSException("ارسال از خطوط عمومی از طریق وب سرویس امکان پذیر نمی باشد");
                case 10:
                    throw new clsSMSException("کاربر مورد نظر فعال نمی باشد");
                case 11:
                    throw new clsSMSException("ارسال نشده");
                case 12:
                    throw new clsSMSException("مدارک کاربر کامل نمی باشد");
            }

            return RetVal;
        }

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        public static bool SendBySharedKhadamatiLine(string To, string ParametricText, string Seprator, int PatternID, ref long RecID)
        {
            string[] ParameterList = ParametricText.Split(Seprator.ToCharArray()).Where(i => i.Length >= 1).ToArray();

            RefreshSettings();

            string RetVal = SendByKhadamatiSMS(UserName, Password, To, ParameterList, PatternID);
            if (RetVal.Length >= 14)
            {
                RecID = Convert.ToInt64(RetVal);
                return true;
            }
            else
            {
                switch (RetVal)
                {
                    case "-7":
                        throw new clsSMSException("خطایی در شماره فرستنده رخ داده است با پشتیبانی تماس بگیرید");
                    case "-6":
                        throw new clsSMSException("خطای داخلی رخ داده است با پشتیبانی تماس بگیرید");
                    case "-5":
                        throw new clsSMSException("متن ارسالی با توجه به متغیر های مشخص شده در متن پیشفرض همخوانی ندارد");
                    case "-4":
                        throw new clsSMSException("کد متن ارسالی صحیح نمی باشد و یا توسط مدیر سامانه تایید نشده است");
                    case "-3":
                        throw new clsSMSException("خط ارسالی در سیستم تعریف نشده است، با پشتیبانی سامانه تماس بگیرید");
                    case "-2":
                        throw new clsSMSException("محدودیت تعداد شماره، محدودیت هر بار ارسال 1 شماره موبایل می باشد");
                    case "-1":
                        throw new clsSMSException("دسترسی برای استفاده از این وبسرویس غیرفعال است، با پشتیبانی تماس بگیرید");
                    case "0":
                        throw new clsSMSException("نام کاربری یا رمز عبور صحیح نمی باشد");
                    case "2":
                        throw new clsSMSException("اعتبار کافی نمی باشد");
                    case "6":
                        throw new clsSMSException("سامانه در حال بروزرسانی می باشد");
                    case "7":
                        throw new clsSMSException("متن حاوی کلمه فیلتر شده می باشد، با واحد اداری تماس بگیرید");
                    case "10":
                        throw new clsSMSException("کاربر مورد نظر فعال نمی باشد");
                    case "11":
                        throw new clsSMSException("ارسال نشده (بلک لیست)");
                    case "12":
                        throw new clsSMSException("مدارک کاربر کامل نمی باشد");
                }

                return false;
            }
        }

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        #endregion

        #region GetCredit
        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        private static double GetCredit(string UserName, string Password)
        {
            if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password))
                return -1;

            System.Net.ServicePointManager.Expect100Continue = false;
            return SMSSender.GetCredit(UserName, Password);
        }

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        public static double GetCredit()
        {
            RefreshSettings();
            return GetCredit(UserName, Password);
        }
        #endregion

        #region GetDelivery
        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        private static int? ConvertDelivery(int InputDelivery)
        {
            int? Res = null;
            switch (InputDelivery)
            {
                case 0:
                    Res = 193;
                    break;

                case 1:
                    Res = 194;
                    break;

                case 2:
                    Res = 195;
                    break;

                case 3:
                    Res = 196;
                    break;

                case 5:
                    Res = 197;
                    break;

                case 8:
                    Res = 198;
                    break;

                case 16:
                    Res = 199;
                    break;

                case 100:
                    Res = 200;
                    break;
            }

            return Res;
        }

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        public static int? GetDelivery(long RecID)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            int Res = SMSSender.GetDelivery(RecID);
            int? RetValue = ConvertDelivery(Res);
            return RetValue;
        }


        #endregion
    }

    public class clsSMSException : Exception
    {
        public clsSMSException() : base()
        {
        }

        public clsSMSException(string Message)
            : base(Message)
        {
        }

        public clsSMSException(string Message, Exception InnerException)
            : base(Message, InnerException)
        {
        }

        public clsSMSException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}