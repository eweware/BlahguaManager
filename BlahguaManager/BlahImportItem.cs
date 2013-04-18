﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Data;

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

        private DataRow _dataRow;

        public BlahImportItem(DataRow curRow)
        {
            Channel = curRow[0].ToString();
            Username = curRow[1].ToString();
            BlahType = curRow[2].ToString();
            Title = curRow[3].ToString();
            Body = curRow[4].ToString();
            Image = curRow[5].ToString();
            ImagePath = curRow[6].ToString();
            _dataRow = curRow;
        }

        public string GetData(int dataIndex)
        {
            return _dataRow[7 + dataIndex].ToString();
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
            paramStr += createJsonParameter("Y", GetBlahTypeId()) + ", ";
            if (Body != "")
                paramStr += createJsonParameter("F", GetBlahTypeId()) + ", ";

            // handle the blah types
            if (this.BlahType == "polls")
            {
                int pollCount = 0;
                ArrayList pollItems = new ArrayList();
                string curPollText = "";

                while ((pollCount < 10) && (GetData(pollCount) != ""))
                {
                    if (curPollText != "")
                        curPollText += ", ";
                    curPollText += "{";
                    curPollText += createJsonParameter("G", GetData(pollCount));
                    curPollText += createJsonParameter("T", "");
                    curPollText += "}";
                    pollCount++;
                }
                paramStr += createJsonParameter("I", curPollText);
            }
            else if (this.BlahType == "predicts")
            {
                DateTime theDate = DateTime.Parse(GetData(0));
                paramStr += createJsonParameter("E", theDate.ToShortDateString());
            }

            paramStr += "}";

            string theBlah = App.Blahgua.CreateBlah(paramStr);

            if (theBlah != "")
            {

            }

            return resultStr;
        }

        private string createJsonParameter(string paramName, string paramVal)
        {
            string resultStr = "";

            resultStr += "\"" + paramName + "\":\"";
            resultStr += paramVal;
            resultStr += "\" ";

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
            App.Blahgua.JoinGroup(Channel);
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