using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;




namespace WebLoadTester
{
    class JsonExtractor : ExtractionRule
    {
         public override string RuleName
        {
            get { return "Extract JSON properties from response"; }
        }

        /// Specify a description for use in the user interface.
        /// The user sees this description in the Add Extraction dialog box.
        //---------------------------------------------------------------------
        public override string RuleDescription
        {
            get { return "Extracts the value from a specified JSON parameter"; }
        }

        // The name of the desired input field
        private string NameValue;
        public string JSonPropertyName
        {
            get { return NameValue; }
            set { NameValue = value; }
        }

        // The Extract method.  The parameter e contains the web performance test context.
        //---------------------------------------------------------------------
        public override void Extract(object sender, Microsoft.VisualStudio.TestTools.WebTesting.ExtractionEventArgs e)
        {
            //JObject jsonResult = JObject.Parse(e.Response.BodyString);
            
            try 
            {
                string propertyValue = "test";// jsonResult[JSonPropertyName].ToString();
                e.WebTest.Context.Add(ContextParameterName, propertyValue);
                e.Success = true;
            }
            catch (Exception exp) 
            {
                e.Success = false;
            }
        }
        
    }

}
