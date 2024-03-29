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
    using System.Collections;
    using Newtonsoft.Json.Linq;


    public class ReadAndVoteTest : WebTest
    {
        static int counter = 0;
        static Random rndBase = new Random();
        static string rndCounter = DateTime.Now.Ticks.ToString();
        string headerTestName = "Visual_Studio; Coded_Web_Test;";
        static int instanceCounter = rndBase.Next(1000);

        private WebServicePlugin testPlugin0 = new WebServicePlugin();

        public ReadAndVoteTest()
        {
            counter++;
            this.Context.Add("DefaultGroup", "");
            this.Context.Add("SayBlahType", "");
            this.Context.Add("DefaultBlahId", "");
            this.Context.Add("UserExists", "");
            this.Context.Add("BlahList", "");
            this.Context.Add("CurrentUserId", "");
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
            string UserName = "reader4_" + counter.ToString();
            //string UserName = "reader1_" + instanceCounter.ToString();


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


            WebTestRequest request1Dependent1 = new WebTestRequest("https://beta.blahgua.com/v2/groups/featured");
            request1Dependent1.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request1Dependent1"));
            request1Dependent1.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request1Dependent1.QueryStringParameters.Add("", "{}", false, false);
            JsonExtractor groupExtractor = new JsonExtractor();
            groupExtractor.ContextParameterName = "DefaultGroup";
            groupExtractor.Key = "N";
            groupExtractor.KeyTest = "General Discussion";
            groupExtractor.Name = "_id";
            request1Dependent1.ExtractValues += new EventHandler<ExtractionEventArgs>(groupExtractor.ExtractKeyedObject);
            yield return request1Dependent1;
            request1Dependent1 = null;

            WebTestRequest request1Dependent2 = new WebTestRequest("https://beta.blahgua.com/v2/blahs/types");
            request1Dependent2.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request1Dependent2.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request1Dependent2"));
            request1Dependent2.QueryStringParameters.Add("", "{}", false, false);
            JsonExtractor sayExtractor = new JsonExtractor();
            sayExtractor.Name = "_id";
            sayExtractor.Key = "N";
            sayExtractor.KeyTest = "says";
            sayExtractor.ContextParameterName = "SayBlahType";
            request1Dependent2.ExtractValues += new EventHandler<ExtractionEventArgs>(sayExtractor.ExtractKeyedObject);
            yield return request1Dependent2;
            request1Dependent2 = null;

            WebTestRequest request2 = new WebTestRequest("https://beta.blahgua.com/v2/users/inbox");
            request2.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request2.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request2"));
            request2.QueryStringParameters.Add("start", "0", false, false);
            request2.QueryStringParameters.Add("count", "100", false, false);
            request2.QueryStringParameters.Add("groupId", this.Context["DefaultGroup"].ToString(), false, false);
            JsonExtractor blahsExtractor = new JsonExtractor();
            blahsExtractor.ContextParameterName = "BlahList";
            request2.ExtractValues += new EventHandler<ExtractionEventArgs>(blahsExtractor.ExtractArray);
            
            yield return request2;
            request2 = null;

            WebTestRequest request3 = new WebTestRequest(("https://beta.blahgua.com/v2/groups/"
                            + (this.Context["DefaultGroup"].ToString() + "/viewerCount")));
            request3.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request3.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request3"));
            yield return request3;
            request3 = null;


            // check if the user exists
            WebTestRequest request1A = new WebTestRequest("https://beta.blahgua.com/v2/users/check/username/" + UserName);
            request1A.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request1A"));
            request1A.Method = "POST";
            request1A.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            StringHttpBody request1ABody = new StringHttpBody();
            request1ABody.ContentType = "application/json; charset=utf-8";
            request1ABody.InsertByteOrderMark = false;
            request1ABody.BodyString = "";
            request1A.Body = request1ABody;
            JsonExtractor userExistsRule = new JsonExtractor();
            userExistsRule.ContextParameterName = "UserExists";
            userExistsRule.Name = "ok";
            request1A.ExtractValues += new EventHandler<ExtractionEventArgs>(userExistsRule.Extract);

            yield return request1A;

            // make the conditioner
            StringComparisonRule conditionalRule2 = new StringComparisonRule();
            conditionalRule2.ContextParameterName = "UserExists";
            conditionalRule2.ComparisonOperator = StringComparisonOperator.Equality;
            conditionalRule2.Value = "false";
            conditionalRule2.IgnoreCase = true;
            conditionalRule2.UseRegularExpression = false;

            this.BeginCondition(conditionalRule2);
            JsonExtractor userIdExtractor = new JsonExtractor();
            userIdExtractor.Name = "_id";
            userIdExtractor.ContextParameterName = "CurrentUserId";

            if (this.ExecuteConditionalRule(conditionalRule2))
            {
                WebTestRequest requestA = new WebTestRequest("https://beta.blahgua.com/v2/users");
                requestA.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " requestA"));
                requestA.ThinkTime = 1;
                requestA.Method = "POST";
                StringHttpBody requestABody = new StringHttpBody();
                requestABody.ContentType = "application/json; charset=utf-8";
                requestABody.InsertByteOrderMark = false;
                requestABody.BodyString = "{\"N\":\"" + UserName + "\",\"pwd\":\"Sheep\"}";
                requestA.Body = requestABody;
                yield return requestA;
                requestA = null;
                LastResponseCodeRule createdOkRule = new LastResponseCodeRule();
                createdOkRule.ComparisonOperator = StringComparisonOperator.Equality;
                createdOkRule.ResponseCode = WebTestResponseCode.Created;

                this.BeginCondition(createdOkRule);
                bool didIt = this.ExecuteConditionalRule(createdOkRule);


                WebTestRequest request5 = new WebTestRequest("https://beta.blahgua.com/v2/users/login");
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
                    WebTestRequest request8c = new WebTestRequest("https://beta.blahgua.com/v2/userGroups");
                    request8c.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request8c"));
                    request8c.ThinkTime = 1;
                    request8c.Method = "POST";
                    StringHttpBody request8cBody = new StringHttpBody();
                    request8cBody.ContentType = "application/json; charset=utf-8";
                    request8cBody.InsertByteOrderMark = false;
                    request8cBody.BodyString = "{\"G\":\"" + this.Context["DefaultGroup"].ToString() + "\"}";
                    request8c.Body = request8cBody;
                    yield return request8c;
                    request8c = null;
                }

                this.EndCondition(createdOkRule);
            }
            else
            {
                WebTestRequest request5 = new WebTestRequest("https://beta.blahgua.com/v2/users/login");
                request5.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request5"));
                request5.Method = "POST";
                StringHttpBody request5Body = new StringHttpBody();
                request5Body.ContentType = "application/json; charset=utf-8";
                request5Body.InsertByteOrderMark = false;
                request5Body.BodyString = "{\"N\":\"" + UserName + "\", \"pwd\":\"Sheep\"}";
                request5.Body = request5Body;
                yield return request5;
                request5 = null;
            }

            this.EndCondition(conditionalRule2);



            WebTestRequest request6 = new WebTestRequest("https://beta.blahgua.com/v2/users/profile/schema");
            request6.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request6"));
            request6.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            yield return request6;
            request6 = null;

            WebTestRequest request7 = new WebTestRequest("https://beta.blahgua.com/v2/users/info");
            request7.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request7"));
            request7.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request7.QueryStringParameters.Add("", "{}", false, false);
            request7.ExtractValues += new EventHandler<ExtractionEventArgs>(userIdExtractor.Extract);
            yield return request7;
            request7 = null;


            WebTestRequest request8 = new WebTestRequest("https://beta.blahgua.com/v2/userGroups");
            request8.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request8"));
            request8.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request8.QueryStringParameters.Add("", "{}", false, false);
            XPathExtractionRule extractionRule3 = new XPathExtractionRule();
            extractionRule3.XPathToSearch = "//*[local-name()=\'item\']/*[local-name()=\'_id\']";
            extractionRule3.Index = 0;
            extractionRule3.ExtractRandomMatch = false;
            extractionRule3.ContextParameterName = "DefaultGroup";
            request8.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule3.Extract);
            yield return request8;
            request8 = null;


            WebTestRequest request10 = new WebTestRequest(("https://beta.blahgua.com/v2/groups/"
                            + (this.Context["DefaultGroup"].ToString() + "/viewerCount")));
            request10.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
            request10.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request10"));
            yield return request10;
            request10 = null;

            CountingLoopRule conditionalRule1 = new CountingLoopRule();
            conditionalRule1.ContextParameterName = "OpenCount";
            conditionalRule1.IterationsCount = ((JArray)this.Context["BlahList"]).Count;

            int maxIterations1 = -1;
            bool advanceDataCursors1 = false;
            this.BeginLoop(conditionalRule1, maxIterations1, advanceDataCursors1);

            for (; this.ExecuteConditionalRule(conditionalRule1); )
            {
                int curIndex = (int)this.Context["OpenCount"];
                string curBlahID = (string)((JArray)this.Context["BlahList"])[curIndex-1]["I"];
                this.Context["DefaultBlahId"] = curBlahID;

                WebTestRequest request12 = new WebTestRequest(("https://beta.blahgua.com/v2/blahs/" + this.Context["DefaultBlahId"].ToString()));
                request12.Headers.Add(new WebTestRequestHeader("Content-Type", "application/json; charset=utf-8"));
                request12.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request12"));
                request12.QueryStringParameters.Add("stats", "true", false, false);
                JsonExtractor authorExtractor = new JsonExtractor();
                authorExtractor.ContextParameterName = "CurrentBlah";
                request12.ExtractValues += new EventHandler<ExtractionEventArgs>(authorExtractor.ExtractObject);
                yield return request12;
                request12 = null;

                JObject curObject = (JObject)this.Context["CurrentBlah"];


                // add a view and an open
                WebTestRequest viewOpenRequest = new WebTestRequest(("https://beta.blahgua.com/v2/blahs/" + this.Context["DefaultBlahId"].ToString()));
                viewOpenRequest.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " viewOpenRequest"));
                viewOpenRequest.Method = "PUT";
                StringHttpBody viewOpenBody = new StringHttpBody();
                viewOpenBody.ContentType = "application/json; charset=utf-8";
                viewOpenBody.InsertByteOrderMark = false;
                viewOpenBody.BodyString = "{\"V\":1, \"O\":1}";
                viewOpenRequest.Body = viewOpenBody;
                yield return viewOpenRequest;

                // potentially vote
                // TODO: Check if this is the user's own blah
                if ((string)curObject["A"] != (string)this.Context["CurrentUserId"])
                {
                    // TO DO:  Check if the user has voted
                    int newVote = 0;
                    int voteChance = rndBase.Next(10);

                    if (voteChance < 3)
                        newVote = 1;
                    else if (voteChance < 6)
                        newVote = -1;

                    if (newVote != 0)
                    {
                        string bodyString;
                        if (newVote == 1)
                            bodyString = "{\"P\":1}";
                        else
                            bodyString = "{\"D\":1}";

                        WebTestRequest promoteDemoteRequest = new WebTestRequest(("https://beta.blahgua.com/v2/blahs/" + this.Context["DefaultBlahId"].ToString()));
                        promoteDemoteRequest.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " promoteDemoteRequest"));
                        promoteDemoteRequest.Method = "PUT";
                        StringHttpBody promoteBody = new StringHttpBody();
                        promoteBody.ContentType = "application/json; charset=utf-8";
                        promoteBody.InsertByteOrderMark = false;
                        promoteBody.BodyString = bodyString;
                        promoteDemoteRequest.Body = promoteBody;
                        yield return promoteDemoteRequest;
                    }
                }

                   
            }

            this.EndLoop(conditionalRule1);

            WebTestRequest request14 = new WebTestRequest("https://beta.blahgua.com/v2/users/logout");
            request14.Headers.Add(new WebTestRequestHeader("JEWS", headerTestName + " request14"));
            request14.ThinkTime = 1;
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
