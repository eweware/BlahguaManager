using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Text;



/// <summary>
/// Summary description for WebServiceHelper
/// </summary>
public class WebServiceHelper
{
    public Dictionary<string, string> groupNames = null;
    public Dictionary<string, string> blahTypes = null;


    public WebServiceHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    // helper functions
    // RESTful helping methods // ec2-50-17-23-5.compute-1.amazonaws.com
    public static string serverURL = @"beta.blahgua.come"; // production
    //public static string serverURL = @"ec2-107-22-125-68.compute-1.amazonaws.com"; // QA

    private string subURL = "v2";
    private bool _usingDefaultUser = true;

    public bool IsDefaultUser()
    {
        return _usingDefaultUser;
    }





    public Uri CreateURIfromBaseURL(string path, string query = "")
    {
        UriBuilder builder = new UriBuilder();
        builder.Host = serverURL;
        builder.Path = path;

        builder.Query = query;
        Uri result = builder.Uri;

        return result;

    }

    private string CreateRESTBaseURL(string command)
    {
        return "/" + subURL + "/" + command;
    }



    private string GetDataFromService(Uri commandURI)
    {
        return DoWebServiceRequest("GET", commandURI);
    }

    private string PostDataToService(Uri commandURI, string JSONdata)
    {
        return DoWebServiceRequest("POST", commandURI, JSONdata);
    }

    private string PutDataToService(Uri commandURI, string JSONdata)
    {
        return DoWebServiceRequest("PUT", commandURI, JSONdata);
    }

    private string DeleteDataFromService(Uri commandURI)
    {
        return DoWebServiceRequest("DELETE", commandURI);
    }

    private string DoWebServiceRequest(string method, Uri commandURI, string JSONdata = "")
    {
        WebRequest request = WebRequest.Create(commandURI);
        Stream dataStream;
        bool failed;

        request.Method = method;

        if (JSONdata != "")
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(JSONdata);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
        }

        // Get the response.
        string responseFromServer = "";
        WebResponse response;
        try
        {
            response = request.GetResponse();
            failed = false;

        }
        catch (WebException exp)
        {
            response = exp.Response;
            failed = true;
        }

        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);

        // Read the content.
        responseFromServer = reader.ReadToEnd();


        // Clean up the streams.
        reader.Close();
        dataStream.Close();
        response.Close();

        if (failed)
        {
            WebException exp = new WebException(responseFromServer);
            throw exp;
        }
        return responseFromServer;
    }

    


    /// <summary>
    /// Creates a new user with the given name
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string CreateUser(string userName)
    {
        string UserStr = CreateRESTBaseURL("users");
        Uri commandURI;
        commandURI = CreateURIfromBaseURL(UserStr);
        string json = "{\"displayName\":\"" + userName + "\"}";

        string newUserStr = PostDataToService(commandURI, json);

        string newID = GetJSONProperty(newUserStr, "_id");

        return newUserStr;
    }




    /// <summary>
    /// Given a userID, returns the user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public string GetUser(string userId)
    {
        Uri commandURI;
        string UserStr = CreateRESTBaseURL("users");
        UserStr += "/" + userId;
        commandURI = CreateURIfromBaseURL(UserStr);

        return GetDataFromService(commandURI);
    }


    /// <summary>
    /// Returns a list of all blah types
    /// </summary>
    /// <returns></returns>
    public string GetBlahTypes()
    {
        string TypesString = CreateRESTBaseURL("blahs/types");
        Uri commandURI;
        commandURI = CreateURIfromBaseURL(TypesString);

        return GetDataFromService(commandURI);
    }



    protected List<string> GetUserGroupIDs()
    {
        string userGroupsStr = GetUserGroups("A");
        string[] split = new string[] { "},{" };
        string[] groups = userGroupsStr.Split(split, StringSplitOptions.RemoveEmptyEntries);
        List<string> newList = new List<string>();
        groups[0] = groups[0].Substring(groups[0].IndexOf('{') + 1);

        foreach (string curGroup in groups)
        {
            string idString = GetJSONProperty(curGroup, "groupId", "");
            if (idString == "")
                idString = GetJSONProperty(curGroup, "_id", "");

            newList.Add(idString);
        }
        return newList;
    }

    







    /// <summary>
    /// Creates a new Blah, given the text string, the ID, and the group
    /// </summary>
    /// <param name="blahText"></param>
    /// <param name="blahTypeID"></param>
    /// <param name="blahGroupID"></param>
    /// <returns></returns>
    public string CreateBlah(string blahText, string blahTypeID, string blahGroupID, string body)
    {

        string BlahStr = CreateRESTBaseURL("blahs");
        string jsonData = "\"groupId\":\"" + blahGroupID + "\", \"text\":\"" + blahText + "\", \"typeId\":\"" + blahTypeID + "\"";

        if (body != "")
            jsonData += ", \"b\":\"" + body + "\"";
        jsonData = "{" + jsonData + "}";

        Uri commandURI = CreateURIfromBaseURL(BlahStr);

        return PostDataToService(commandURI, jsonData);
    }




 
    /// <summary>
    /// AddBlahComment
    /// </summary>
    /// <param name="newComment"></param>
    /// <returns></returns>
    public string AddBlahComment(string newComment, string currentBlahID)
    {
        string BlahStr = CreateRESTBaseURL("comments");
        Uri commandURI = CreateURIfromBaseURL(BlahStr);
        //'{"authorId": authorId, "blahId": blahId, "text": text, "blahVote": blahVote}' 

        string jsonData = "{\"blahVote\":0, \"text\":\"" + newComment + "\"}";


        return PostDataToService(commandURI, jsonData);

    }




    /// <summary>
    /// Helper function to return the value of a property from a JSON string
    /// </summary>
    /// <param name="json"></param>
    /// <param name="propName"></param>
    /// <param name="defaultVal"></param>
    /// <returns></returns>
    public static string GetJSONProperty(string json, string propName, string defaultVal = "missing")
    {
        string resultStr = defaultVal;
        string searchStr = "\"" + propName + "\":";
        int startIndex = json.IndexOf(searchStr);

        if (startIndex != -1)
        {
            startIndex += propName.Length + 4;

            int endIndex = json.IndexOf('\"', startIndex + 1);
            resultStr = json.Substring(startIndex, endIndex - startIndex);
        }

        return resultStr;

    }


    public string GetAllGroups()
    {
        string queryStr = CreateRESTBaseURL("groups");

        Uri commandURI = CreateURIfromBaseURL(queryStr);

        return GetDataFromService(commandURI);

    }




    /// <summary>
    /// returns the groups of the current user.
    /// </summary>
    /// <param name="statusStr"></param>
    /// <returns></returns>
    public string GetUserGroups(string statusStr = "")
    {
        string UserStr = CreateRESTBaseURL("userGroups");
        string queryArgs = "";

        if (statusStr != "")
            queryArgs = "state=" + statusStr;

        Uri commandURI = CreateURIfromBaseURL(UserStr, queryArgs);

        string resultStr = GetDataFromService(commandURI);

        return resultStr; // AddNameAndDescToGroupList(resultStr);
    }



    /// <summary>
    /// Joins a group
    /// </summary>
    /// <param name="groupID"></param>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    public string JoinGroup(string groupID)
    {
        string GroupStr = CreateRESTBaseURL("userGroups");

        Uri commandURI = CreateURIfromBaseURL(GroupStr);
        string jsonData;

        jsonData = "{\"groupId\":\"" + groupID + "\"}";

        return PostDataToService(commandURI, jsonData);

    }



    /// <summary>
    /// Returns the ID for a blah type given the name
    /// </summary>
    /// <param name="blahType"></param>
    /// <returns></returns>
    public string GetBlahTypeID(string blahType)
    {
        if (blahTypes == null)
            blahTypes = new Dictionary<string, string>();

        string typeID = "";

        if (groupNames.Keys.Contains(blahType))
            typeID = groupNames[blahType];
        else
        {
            string resultStr = GetBlahTypes();
            resultStr = resultStr.Substring(1, resultStr.Length - 2);
            string[] split = new string[] { "},{" };
            string[] groups = resultStr.Split(split, StringSplitOptions.RemoveEmptyEntries);
            List<string> newList = new List<string>();
            groups[0] = groups[0].Substring(groups[0].IndexOf('{') + 1);

            foreach (string curGroup in groups)
            {
                string idString = GetJSONProperty(curGroup, "_id");
                string nameStr = GetJSONProperty(curGroup, "name");
                blahTypes[nameStr] = idString;
                if (nameStr == blahType)
                    typeID = idString;
            }
        }

        return typeID;
    }

    public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
    {
        string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

        HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
        wr.ContentType = "multipart/form-data; boundary=" + boundary;
        wr.Method = "POST";
        wr.KeepAlive = true;
        wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

        Stream rs = wr.GetRequestStream();

        string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
        foreach (string key in nvc.Keys)
        {
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            string formitem = string.Format(formdataTemplate, key, nvc[key]);
            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
            rs.Write(formitembytes, 0, formitembytes.Length);
        }
        rs.Write(boundarybytes, 0, boundarybytes.Length);

        string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
        string header = string.Format(headerTemplate, paramName, file, contentType);
        byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
        rs.Write(headerbytes, 0, headerbytes.Length);

        FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[4096];
        int bytesRead = 0;
        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
        {
            rs.Write(buffer, 0, bytesRead);
        }
        fileStream.Close();

        byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
        rs.Write(trailer, 0, trailer.Length);
        rs.Close();

        WebResponse wresp = null;
        try
        {
            wresp = wr.GetResponse();
            Stream stream2 = wresp.GetResponseStream();
            StreamReader reader2 = new StreamReader(stream2);
            //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
        }
        catch (Exception ex)
        {
            //log.Error("Error uploading file", ex);
            if (wresp != null)
            {
                wresp.Close();
                wresp = null;
            }
        }
        finally
        {
            wr = null;
        }
    }

}
