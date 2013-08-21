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
    public class BlahImportItem
    {
        public static string CurrentUserName = "";
 
        public string Channel;
        public string Username;
        public string BlahType;
        public string Title;
        public string Body;
        public string Image;
        public string ImagePath;
        public DateTime PredictionDate;
        public string Data1;
        public string Data2;
        public string Data3;
        public string Data4;
        public string Data5;
        public string Data6;
        public string Data7;
        public string Data8;
        public string Data9;
        public string Data10;
        public string Badge;

        private DataRow _dataRow;

        public BlahImportItem(DataRow curRow)
        {
            Channel = curRow["Channel"].ToString().ToLower();
            Username = curRow["Username"].ToString();
            BlahType = curRow["Blah Type"].ToString().ToLower();
            Badge = curRow["Badge"].ToString();
            Title = curRow["Title"].ToString();
            Body = curRow["Body"].ToString();
            Image = curRow["Image"].ToString();
            ImagePath = curRow["Image path"].ToString();
            if (curRow["Date"] is System.DBNull)
            {
                PredictionDate = DateTime.Now;
            }
            else PredictionDate = (DateTime)curRow["Date"];

            _dataRow = curRow;
        }

        public string GetData(int dataIndex)
        {
            string itemName = "Data " + (dataIndex + 1).ToString();
            return _dataRow[itemName].ToString();
        }

        public string ImportBlah()
        {
            string resultStr = "failed.";

            try
            {
                SetCurrentUser(Username);

                if (!UserHasChannel())
                {
                    JoinUserToChannel();
                }

                resultStr = CreateBlah();
            }
            catch (Exception exp)
            {
                // to do - something
                resultStr = exp.Message;
            }

            return resultStr;
        }

        private string CreateBlah()
        {
            string resultStr = "failed.";

            string paramStr = "{";
            paramStr += createJsonParameter("G", GetChannelId()) + ", ";
            paramStr += createJsonParameter("T", Title) + ", ";
            paramStr += createJsonParameter("Y", GetBlahTypeId());
            if (Body != "")
                paramStr += ", " + createJsonParameter("F", Body);
            if (Badge != "")
            {
                string badgeId = App.Blahgua.GetDefaultBadges();

                paramStr += ", \"B\":[" + badgeId + "] ";
            }

            // handle the blah types
            if (this.BlahType == "polls")
            {
                int pollCount = 0;
                ArrayList pollItems = new ArrayList();
                string curPollText = "";

                while (pollCount < 10)
                {
                    if (GetData(pollCount) != "")
                    {
                        if (curPollText != "")
                            curPollText += ", ";
                        curPollText += "{";
                        curPollText += createJsonParameter("G", GetData(pollCount));
                        curPollText += ", " + createJsonParameter("T", "");
                        curPollText += "}";
                    }
                    pollCount++;
                }
                curPollText = "[" + curPollText + "]";
                paramStr += ", " + createJsonParameter("I", curPollText, false);
            }
            else if (this.BlahType == "predicts")
            {
                paramStr += ", " + createJsonParameter("E", PredictionDate.ToString("o"));
            }

            paramStr += "}";
            try
            {
                string theBlah = App.Blahgua.CreateBlah(paramStr);

                if (theBlah != "")
                {
                    string blahId = WebServiceHelper.GetJSONProperty(theBlah, "_id");

                    if (Image != "")
                    {
                        string curPath =  Image;
                        App.Blahgua.AddFileToBlah(blahId, curPath);
                    }

                    resultStr = "ok";
                }

            }
            catch (Exception exp)
            {
                resultStr = exp.Message;
            }

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

        private void JoinUserToChannel()
        {
            App.Blahgua.JoinGroup(GetChannelId());
        }


        // utilities
        private string GetChannelId()
        {
            return App.Blahgua.GetChannelId(Channel);
        }

        private bool UserHasChannel()
        {
            return App.Blahgua.UserHasChannel(Channel);
        }

        private string GetBlahTypeId()
        {
            return App.Blahgua.GetBlahTypeID(BlahType);
        }
    }
}
