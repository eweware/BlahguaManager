using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Data;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace BlahguaManager
{
    class ProfileImportItem
    {
        public static string CurrentUserName = "";
        public string Username;
        public string Nickname;
        public bool Nickname_public;
        public string City;
        public bool City_public;
        public string State;
        public bool State_public;
        public string Zipcode;
        public bool Zipcode_public;
        public string Country;
        public bool Country_public;
        public string Gender;
        public bool Gender_public;
        public string DOB;
        public bool DOB_public;
        public string Income;
        public bool Income_public;
        public string Ethnicity;
        public bool Ethnicity_public;
        public string Image;
        public string ImagePath;


        public ProfileImportItem(DataRow curRow)
        {
            Username = curRow["Username"].ToString();
            Nickname = curRow["Nickname"].ToString();
            City = curRow["City"].ToString();
            State = curRow["State"].ToString();
            Zipcode = curRow["Zipcode"].ToString();
            Country = curRow["Country"].ToString();
            Gender = curRow["Sex"].ToString();
            DOB = curRow["DOB"].ToString();
            Income = curRow["Income"].ToString();
            Ethnicity = curRow["Ethnicity"].ToString();
            Image = curRow["User Image"].ToString();
            ImagePath = curRow["Image Path"].ToString();

            // now the permissions
            City_public = curRow["Citypublic"].ToString() == "1";
            State_public = curRow["Statepublic"].ToString() == "1";
            Zipcode_public = curRow["Zipcodepublic"].ToString() == "1";
            Country_public = curRow["Countrypublic"].ToString() == "1";
            Gender_public = curRow["Sexpublic"].ToString() == "1";
            DOB_public = curRow["DOBpublic"].ToString() == "1";
            Income_public = curRow["Incomepublic"].ToString() == "1";
            Ethnicity_public = curRow["Ethnicitypublic"].ToString() == "1";

        }

       
        public string ImportProfile()
        {
            string resultStr = "failed.";

            try
            {
                SetCurrentUser(Username);

                resultStr = UpdateUserProfile();

            }
            catch (Exception exp)
            {
                // to do - something
                resultStr = exp.Message;
            }

            return resultStr;
        
        }


        private string UpdateUserProfile()
        {
            string resultStr = "failure";
            string paramStr = "{";
            if (Nickname != "")
            {
                paramStr += createJsonParameter("A", Nickname) + ", ";
            }

            if (City != "")
            {
                paramStr += createJsonParameter("G", City) + ", ";
                paramStr += createJsonParameter("6", City_public ? "2" : "0") + ", ";
            }

            if (State != "")
            {
                paramStr += createJsonParameter("H", State) + ", ";
                paramStr += createJsonParameter("7", State_public ? "2" : "0") + ", ";
            }

            if (Zipcode != "")
            {
                paramStr += createJsonParameter("I", Zipcode) + ", ";
                paramStr += createJsonParameter("8", Zipcode_public ? "2" : "0") + ", ";
            }

            if (Country != "")
            {
                paramStr += createJsonParameter("J", FindCountryCode(Country)) + ", ";
                paramStr += createJsonParameter("9", Country_public ? "2" : "0") + ", ";
            }

            if (Gender != "")
            {
                paramStr += createJsonParameter("B", FindGenderCode(Gender)) + ", ";
                paramStr += createJsonParameter("1", Gender_public ? "2" : "0") + ", ";
            }

            if (DOB != "")
            {
                paramStr += createJsonParameter("C", MakeDOB(DOB)) + ", ";
                paramStr += createJsonParameter("2", DOB_public ? "2" : "0") + ", ";
            }

            if (Income != "")
            {
                paramStr += createJsonParameter("E", FindIncomeCode(Income)) + ", ";
                paramStr += createJsonParameter("4", Income_public ? "2" : "0") + ", ";
            }

            if (Ethnicity != "")
            {
                paramStr += createJsonParameter("D", FindEthnicityCode(Ethnicity)) + ", ";
                paramStr += createJsonParameter("3", Ethnicity_public ? "2" : "0") + ", ";
            }

            paramStr = paramStr.Trim();
            paramStr = paramStr.TrimEnd(',');
            paramStr += "}";

            try
            {
                App.Blahgua.UpdateUserProfile(paramStr);

                if (Image != "")
                {
                    string curPath = ImagePath + "\\" + Image;
                    //App.Blahgua.AddFileToPerson(blahId, curPath);
                }

                resultStr = "";
            }
            catch (Exception exp)
            {
                resultStr = exp.Message;
            }

            return resultStr;

        }

        private string FindCountryCode(string nameStr)
        {
            string resultStr = "";

            switch (nameStr)
            {
                case "United States":
                case "USA":
                case "US":
                    resultStr = "US";
                    break;
                case "United Kingdom":
                    resultStr = "GB";
                    break;
                case "South Korea":
                    resultStr = "KR";
                    break;
                case "Thailand":
                    resultStr = "TH";
                    break;
                case "Australia":
                    resultStr = "AU";
                    break;
                case "Brazil":
                    resultStr = "BR";
                    break;
                case "Taiwan":
                    resultStr = "TW";
                    break;
                case "Canada":
                    resultStr = "CA";
                    break;
                case "Singapore":
                    resultStr = "SG";
                    break;
                case "Japan":
                    resultStr = "JP";
                    break;
                case "China":
                case "China (PRC)":
                    resultStr = "CN";
                    break;


            }

            return resultStr;
        }

        private string FindIncomeCode(string nameStr)
        {
            string resultStr = "";
            int incomeQty = int.Parse(nameStr);

            if (incomeQty < 25000)
                resultStr = "0";
            else if (incomeQty < 50000)
                resultStr = "1";
            else if (incomeQty < 75000)
                resultStr = "2";
            else if (incomeQty < 100000)
                resultStr = "3";
            else if (incomeQty < 150000)
                resultStr = "4";
            else if (incomeQty < 200000)
                resultStr = "5";
            else resultStr = "6";

            return resultStr;
        }

        private string FindGenderCode(string nameStr)
        {
            string resultStr = "";

            switch (nameStr)
            {
                case "Male":
                case "M":
                    resultStr = "0";
                    break;
                case "Female":
                case "F":
                    resultStr = "1";
                    break;

            }

            return resultStr;
        }

        private string FindEthnicityCode(string nameStr)
        {
            string resultStr = "";

            switch (nameStr)
            {
                case "Asian":
                    resultStr = "0";
                    break;
                case "Black":
                    resultStr = "1";
                    break;
                case "Hispanic":
                    resultStr = "2";
                    break;
                case "White":
                    resultStr = "3";
                    break;
                case "Other":
                    resultStr = "4";
                    break;

            }

            return resultStr;
        }

        private string MakeDOB(string dateStr)
        {
            string resultStr = "";
            DateTime newDate = DateTime.Parse(dateStr);
            resultStr += newDate.Year;
            resultStr += "-" + newDate.Month;
            resultStr += "-" + newDate.Day;

            return resultStr;
        }

        

        private string FormatJSONString(string inputStr)
        {
            string newStr;

            inputStr = inputStr.Replace("\r\n", "[_r;");
            inputStr = inputStr.Replace("\n", "[_r;");
            inputStr = inputStr.Replace("\r", "[_r;");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            newStr = serializer.Serialize(inputStr);

            return newStr;
        }


        private string createJsonParameter(string paramName, string paramVal, bool quoteIt = true)
        {
            string resultStr = "";

            resultStr += "\"" + paramName + "\":";
            if (quoteIt)
                paramVal = FormatJSONString(paramVal);
            
           resultStr += paramVal;

            return resultStr;
        }

        private void SetCurrentUser(string userName)
        {
            if (userName != CurrentUserName)
            {
                if (CurrentUserName != "")
                {
                    App.Blahgua.LogoutUser();
                    CurrentUserName = "";
                }

                if (!UserExists())
                {
                    CreateUser();
                }

                SignInUser();
            }
        }


        private void SignInUser()
        {
            App.Blahgua.SignInUser(Username, "secret");
            CurrentUserName = Username;
        }

        private void CreateUser()
        {
            App.Blahgua.CreateUser(Username, "secret");
        }



        private Boolean UserExists()
        {
            Boolean exists = false;

            exists = App.Blahgua.CheckUserExists(Username);

            return exists;
        }

       
    }
}
