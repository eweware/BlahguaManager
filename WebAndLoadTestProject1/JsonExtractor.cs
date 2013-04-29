using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Microsoft.VisualStudio.TestTools.WebTesting.Rules;
using WebTest.WebService.Plugin.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WebAndLoadTestProject1
{

    public class JsonExtractor : ExtractionRule
    {


        /// Specify a name for use in the user interface.
        /// The user sees this name in the Add Extraction dialog box.
        //---------------------------------------------------------------------
        [Obsolete]
        public override string RuleName
        {
            get { return "Custom Extract Input"; }
        }

        /// Specify a description for use in the user interface.
        /// The user sees this description in the Add Extraction dialog box.
        //---------------------------------------------------------------------
        [Obsolete]
        public override string RuleDescription
        {
            get { return "Extracts the value from a specified input field"; }
        }

        private string myContextParameter = "";
        public override string ContextParameterName
        {
            get { return myContextParameter; }
            set
            {
                if (value != string.Empty)
                    myContextParameter = value;
            }
        }

        // The name of the desired input field
        private string NameValue = "";
        public string Name
        {
            get { return NameValue; }
            set { NameValue = value; }
        }

        // The name of the desired input field
        private string KeyValue = "";
        public string Key
        {
            get { return KeyValue; }
            set { KeyValue = value; }
        }

        // The name of the desired input field
        private string KeyTestValue = "";
        public string KeyTest
        {
            get { return KeyTestValue; }
            set { KeyTestValue = value; }
        }

        // The Extract method.  The parameter e contains the Web test context.
        //---------------------------------------------------------------------
        public override void Extract(object sender, ExtractionEventArgs e)
        {
            JObject jObj = null;
            try
            {
                jObj = JObject.Parse(e.Response.BodyString);
            }
            catch (Exception exp)
            {
                jObj = null;
            }

            e.Success = false;
            if (jObj != null)
            {
                string propertyValue = (string)jObj[Name];
                if (propertyValue != null)
                {
                    e.WebTest.Context.Add(ContextParameterName, propertyValue);
                    e.Success = true;
                }
            }              
        }

        // The Extract method.  The parameter e contains the Web test context.
        //---------------------------------------------------------------------
        public void ExtractObject(object sender, ExtractionEventArgs e)
        {
            JObject jObj = null;
            try
            {
                jObj = JObject.Parse(e.Response.BodyString);
            }
            catch (Exception exp)
            {
                jObj = null;
            }

            if (jObj != null)
            {
                e.WebTest.Context.Add(ContextParameterName, jObj);
                e.Success = true;
            }
            else
                e.Success = false;
        }

        // The Extract method.  The parameter e contains the Web test context.
        //---------------------------------------------------------------------
        public void ExtractArray(object sender, ExtractionEventArgs e)
        {
            JArray jObj = null;
            try
            {
                jObj = JArray.Parse(e.Response.BodyString);
            }
            catch (Exception exp)
            {
                jObj = null;
            }

            if (jObj != null)
            {
                e.WebTest.Context.Add(ContextParameterName, jObj);
                e.Success = true;
            }
            else
                e.Success = false;

        }

        // The Extract method.  The parameter e contains the Web test context.
        //---------------------------------------------------------------------
        public void ExtractKeyedObject(object sender, ExtractionEventArgs e)
        {
            JArray jObj = null;
            try
            {
                jObj = JArray.Parse(e.Response.BodyString);
            }
            catch (Exception exp)
            {
                jObj = null;
            }

            e.Success = false;

            if (jObj != null)
            {
                foreach (JObject curObj in jObj.Children())
                {
                    if ((string)curObj[Key] == KeyTest)
                    {
                        e.WebTest.Context.Add(ContextParameterName, (string)curObj[Name]);
                        e.Success = true;
                        break;
                    }
                }
            }
        }

        
    }
}