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

        // The name of the desired input field
        private string NameValue;
        public string Name
        {
            get { return NameValue; }
            set { NameValue = value; }
        }

        // The Extract method.  The parameter e contains the Web test context.
        //---------------------------------------------------------------------
        public override void Extract(object sender, ExtractionEventArgs e)
        {
               JObject jObj = JObject.Parse(e.Response.BodyString);

              
              string propertyValue = "";
              if (propertyValue != null) {
                e.WebTest.Context.Add(ContextParameterName, propertyValue);
                e.Success = true;
              } else
                e.Success = false;
        }
    }
}