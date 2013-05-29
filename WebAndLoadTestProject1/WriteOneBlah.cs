﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAndLoadTestProject1
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;
    using WebTest.WebService.Plugin.Runtime;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;



    public class WriteOneBlah : WebTest
    {
        static int counter = 0;
        static Random rndBase = new Random();
        static string rndCounter = DateTime.Now.Ticks.ToString();
        string headerTestName = "Visual_Studio; Coded_Web_Test;";
        static int instanceCounter = rndBase.Next(1000);
        

        private WebServicePlugin testPlugin0 = new WebServicePlugin();

        public WriteOneBlah()
        {
            counter++;
            if (counter > 100)
                counter = 0;
            this.Context.Add("DefaultGroup", "");
            this.Context.Add("SayBlahType", "");
            this.Context.Add("DefaultBlahId", "");
            this.Context.Add("UserExists", "");
            this.Context.Add("UserLoggedIn", "");
            this.PreAuthenticate = true;
            this.PreWebTest += new EventHandler<PreWebTestEventArgs>(this.testPlugin0.PreWebTest);
            this.PostWebTest += new EventHandler<PostWebTestEventArgs>(this.testPlugin0.PostWebTest);
            this.PreTransaction += new EventHandler<PreTransactionEventArgs>(this.testPlugin0.PreTransaction);
            this.PostTransaction += new EventHandler<PostTransactionEventArgs>(this.testPlugin0.PostTransaction);
            this.PrePage += new EventHandler<PrePageEventArgs>(this.testPlugin0.PrePage);
            this.PostPage += new EventHandler<PostPageEventArgs>(this.testPlugin0.PostPage);
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            //string UserName = "loadtest2_" + rndCounter + "_" + counter.ToString();
            string UserName = "writer5_" + counter.ToString();
            //string UserName = "writer1_" + instanceCounter.ToString();


            // Initialize validation rules that apply to all requests in the WebTest
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidateResponseUrl validationRule1 = new ValidateResponseUrl();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidationRuleResponseTimeGoal validationRule2 = new ValidationRuleResponseTimeGoal();
                validationRule2.Tolerance = 0D;
                this.ValidateResponseOnPageComplete += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }
            this.PreRequestDataBinding += new EventHandler<PreRequestDataBindingEventArgs>(this.testPlugin0.PreRequestDataBinding);
            this.PreRequest += new EventHandler<PreRequestEventArgs>(this.testPlugin0.PreRequest);
            this.PostRequest += new EventHandler<PostRequestEventArgs>(this.testPlugin0.PostRequest);


            WebTestRequest request1Dependent1 = new WebTestRequest("http://beta.blahgua.com/v2/groups/featured");
            request1Dependent1.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request1Dependent1"));
            request1Dependent1.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request1Dependent1.QueryStringParameters.Add("", "{}", false, false);
            JsonExtractor groupExtractor = new JsonExtractor();
            groupExtractor.ContextParameterName = "DefaultGroup";
            groupExtractor.Key = "N";
            groupExtractor.KeyTest = "The Now Network";
            groupExtractor.Name = "_id";
            request1Dependent1.ExtractValues += new EventHandler<ExtractionEventArgs>(groupExtractor.ExtractKeyedObject);
            yield return request1Dependent1;
            request1Dependent1 = null;

            WebTestRequest request1Dependent2 = new WebTestRequest("http://beta.blahgua.com/v2/blahs/types");
            request1Dependent2.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request1Dependent2"));
            request1Dependent2.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request1Dependent2.QueryStringParameters.Add("", "{}", false, false);
            JsonExtractor sayExtractor = new JsonExtractor();
            sayExtractor.ContextParameterName = "SayBlahType";
            sayExtractor.Key = "N";
            sayExtractor.KeyTest = "says";
            sayExtractor.Name = "_id";
            request1Dependent2.ExtractValues += new EventHandler<ExtractionEventArgs>(sayExtractor.ExtractKeyedObject);
            yield return request1Dependent2;
            request1Dependent2 = null;

            WebTestRequest request2 = new WebTestRequest("http://beta.blahgua.com/v2/users/inbox");
            request2.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request2"));
            request2.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request2.QueryStringParameters.Add("start", "0", false, false);
            request2.QueryStringParameters.Add("count", "100", false, false);
            request2.QueryStringParameters.Add("groupId", this.Context["DefaultGroup"].ToString(), false, false);
            yield return request2;
            request2 = null;

            WebTestRequest request3 = new WebTestRequest(("http://beta.blahgua.com/v2/groups/"
                            + (this.Context["DefaultGroup"].ToString() + "/viewerCount")));
            request3.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request3.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request3"));
            yield return request3;
            request3 = null;

            WebTestRequest request4 = new WebTestRequest("http://beta.blahgua.com/v2/users/inbox");
            request4.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request4.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request4"));
            request4.QueryStringParameters.Add("start", "0", false, false);
            request4.QueryStringParameters.Add("count", "100", false, false);
            request4.QueryStringParameters.Add("groupId", this.Context["DefaultGroup"].ToString(), false, false);
            yield return request4;
            request4 = null;

            ContextParameterExistenceRule loginInRule = new ContextParameterExistenceRule();
            loginInRule.ContextParameterName = "UserReady";
            loginInRule.CheckForExistence = false;

            int maxIterationsLogin = 3;
            bool advanceDataCursorsLogIn = false;
            this.BeginLoop(loginInRule, maxIterationsLogin, advanceDataCursorsLogIn);

            for (; this.ExecuteConditionalRule(loginInRule); )
            {

                // check if the user exists
                WebTestRequest request1A = new WebTestRequest("http://beta.blahgua.com/v2/users/check/username/" + UserName);
                request1A.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request1A"));
                request1A.Method = "POST";
                request1A.Encoding = System.Text.Encoding.GetEncoding("utf-8");
                StringHttpBody request1ABody = new StringHttpBody();
                request1ABody.ContentType = "application/json; charset=utf-8";
                request1ABody.InsertByteOrderMark = false;
                request1ABody.BodyString = "";
                request1A.Body = request1ABody;
                XPathExtractionRule extractionRule1a = new XPathExtractionRule();
                extractionRule1a.XPathToSearch = "//*[local-name()=\'ok\']";
                extractionRule1a.Index = 0;
                extractionRule1a.ExtractRandomMatch = false;
                extractionRule1a.ContextParameterName = "UserExists";
                request1A.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1a.Extract);
                yield return request1A;

                // make the conditioner
                StringComparisonRule conditionalRule2 = new StringComparisonRule();
                conditionalRule2.ContextParameterName = "UserExists";
                conditionalRule2.ComparisonOperator = StringComparisonOperator.Equality;
                conditionalRule2.Value = "false";
                conditionalRule2.IgnoreCase = true;
                conditionalRule2.UseRegularExpression = false;

                this.BeginCondition(conditionalRule2);

                if (this.ExecuteConditionalRule(conditionalRule2))
                {
                    WebTestRequest requestA = new WebTestRequest("http://beta.blahgua.com/v2/users");
                    requestA.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " requestA"));
                    requestA.Method = "POST";
                    StringHttpBody requestABody = new StringHttpBody();
                    requestABody.ContentType = "application/json; charset=utf-8";
                    requestABody.InsertByteOrderMark = false;
                    requestABody.BodyString = "{\"N\":\"" + UserName + "\",\"pwd\":\"Sheep\"}";
                    //requestA.IgnoreHttpStatusCode = true;
                    requestA.Body = requestABody;
                    yield return requestA;

                    requestA = null;
                    LastResponseCodeRule createdOkRule = new LastResponseCodeRule();
                    createdOkRule.ComparisonOperator = StringComparisonOperator.Equality;
                    createdOkRule.ResponseCode = WebTestResponseCode.Created;

                    this.BeginCondition(createdOkRule);
                    bool didIt = this.ExecuteConditionalRule(createdOkRule);

                
                    WebTestRequest request5 = new WebTestRequest("http://beta.blahgua.com/v2/users/login");
                    request5.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request5"));
                    request5.Method = "POST";
                    StringHttpBody request5Body = new StringHttpBody();
                    request5Body.ContentType = "application/json; charset=utf-8";
                    request5Body.InsertByteOrderMark = false;
                    request5Body.BodyString = "{\"N\":\"" + UserName + "\", \"pwd\":\"Sheep\"}";
                    request5.Body = request5Body;
                    yield return request5;
                    request5 = null;

                    if (didIt)
                    {
                        WebTestRequest request8c = new WebTestRequest("http://beta.blahgua.com/v2/userGroups");
                        request8c.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request8c"));
                        request8c.Method = "POST";
                        StringHttpBody request8cBody = new StringHttpBody();
                        request8cBody.ContentType = "application/json; charset=utf-8";
                        request8cBody.InsertByteOrderMark = false;
                        request8cBody.BodyString = "{\"G\":\"" + this.Context["DefaultGroup"].ToString() + "\"}";
                        request8c.Body = request8cBody;
                        yield return request8c;
                        request8c = null;
                        this.Context.Add("UserReady", "true");
                    }
                    else
                        this.Context.Add("UserReady", "true");

                    this.EndCondition(createdOkRule);
                }
                else
                {
                    WebTestRequest request5 = new WebTestRequest("http://beta.blahgua.com/v2/users/login");
                    request5.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request5"));
                    request5.Method = "POST";
                    StringHttpBody request5Body = new StringHttpBody();
                    request5Body.ContentType = "application/json; charset=utf-8";
                    request5Body.InsertByteOrderMark = false;
                    request5Body.BodyString = "{\"N\":\"" + UserName + "\", \"pwd\":\"Sheep\"}";
                    request5.Body = request5Body;
                    yield return request5;
                    request5 = null;
                    this.Context.Add("UserReady", "true");
                }

                this.EndCondition(conditionalRule2);

            }

            this.EndLoop(loginInRule);

            

            WebTestRequest request6 = new WebTestRequest("http://beta.blahgua.com/v2/users/profile/schema");
            request6.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request6"));
            request6.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            yield return request6;
            request6 = null;

            WebTestRequest request7 = new WebTestRequest("http://beta.blahgua.com/v2/users/info");
            request7.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request7.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request7"));
            request7.QueryStringParameters.Add("", "{}", false, false);
            yield return request7;
            request7 = null;


            WebTestRequest request8 = new WebTestRequest("http://beta.blahgua.com/v2/userGroups");
            request8.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request8.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request8"));
            request8.QueryStringParameters.Add("", "{}", false, false);
            XPathExtractionRule extractionRule3 = new XPathExtractionRule();
            extractionRule3.XPathToSearch = "//*[local-name()=\'item\']/*[local-name()=\'_id\']";
            extractionRule3.Index = 0;
            extractionRule3.ExtractRandomMatch = false;
            extractionRule3.ContextParameterName = "DefaultGroup";
            request8.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule3.Extract);
            yield return request8;
            request8 = null;


            WebTestRequest request9 = new WebTestRequest("http://beta.blahgua.com/v2/users/inbox");
            request9.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request9.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request9"));
            request9.QueryStringParameters.Add("start", "0", false, false);
            request9.QueryStringParameters.Add("count", "100", false, false);
            request9.QueryStringParameters.Add("groupId", this.Context["DefaultGroup"].ToString(), false, false);
            yield return request9;
            request9 = null;

            WebTestRequest request10 = new WebTestRequest(("http://beta.blahgua.com/v2/groups/"
                            + (this.Context["DefaultGroup"].ToString() + "/viewerCount")));
            request10.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request10.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request10"));
            yield return request10;
            request10 = null;

            CountingLoopRule conditionalRule1 = new CountingLoopRule();
            conditionalRule1.ContextParameterName = "CreateCount";
            conditionalRule1.IterationsCount = 1D;

            int maxIterations1 = -1;
            bool advanceDataCursors1 = false;
            this.BeginLoop(conditionalRule1, maxIterations1, advanceDataCursors1);

            for (; this.ExecuteConditionalRule(conditionalRule1); )
            {
                WebTestRequest request11 = new WebTestRequest("http://beta.blahgua.com/v2/blahs");
                request11.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request11"));
                request11.Method = "POST";
                StringHttpBody request11Body = new StringHttpBody();
                request11Body.ContentType = "application/json; charset=utf-8";
                request11Body.InsertByteOrderMark = false;
                request11Body.BodyString = ("{\"G\":\""
                            + (this.Context["DefaultGroup"].ToString()
                            + ("\",\"T\":\"Blah # "
                            + (this.Context["CreateCount"].ToString() + " from " + UserName + " from run " + rndCounter.ToString()
                            + ("\",\"Y\":\""
                            + (this.Context["SayBlahType"].ToString() + "\",\"F\":\"Body Text of the Test Blah\"}"))))));
                request11.Body = request11Body;
                XPathExtractionRule extractionRule4 = new XPathExtractionRule();
                extractionRule4.XPathToSearch = "//*[local-name()=\'_id\']";
                extractionRule4.Index = 0;
                extractionRule4.ExtractRandomMatch = false;
                extractionRule4.ContextParameterName = "DefaultBlahId";
                request11.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule4.Extract);
                yield return request11;

                request11 = null;

                WebTestRequest request12 = new WebTestRequest("http://beta.blahgua.com/v2/blahs/" + (this.Context["DefaultBlahId"].ToString()));
                request12.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
                request12.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request12"));
                request12.QueryStringParameters.Add("stats", "true", false, false);
                yield return request12;
                request12 = null;

                WebTestRequest request13 = new WebTestRequest("http://beta.blahgua.com/v2/users/descriptor");
                request13.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request13"));
                request13.Method = "POST";
                StringHttpBody request13Body = new StringHttpBody();
                request13Body.ContentType = "application/json; charset=utf-8";
                request13Body.InsertByteOrderMark = false;
                request13Body.BodyString = "{}";
                request13.Body = request13Body;
                yield return request13;
                request13 = null;
            }

            this.EndLoop(conditionalRule1);

            WebTestRequest request14 = new WebTestRequest("http://beta.blahgua.com/v2/users/logout");
            request14.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request14"));
            request14.Method = "POST";
            StringHttpBody request14Body = new StringHttpBody();
            request14Body.ContentType = "application/json; charset=utf-8";
            request14Body.InsertByteOrderMark = false;
            request14Body.BodyString = "{}";
            request14.Body = request14Body;
            yield return request14;
            request14 = null;
        }
    }
}
