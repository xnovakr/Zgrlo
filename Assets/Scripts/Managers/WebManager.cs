using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebManager
{
    protected static HttpWebRequest CreateWebRequestConnection(string requestMethod, string webAdress)
    {
        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(webAdress);

        httpRequest.Method = requestMethod;
        httpRequest.Accept = "application/json";
        httpRequest.ContentType = "application/json";

        return httpRequest;
    }

    protected static void SendWebRequest(string uidString, HttpWebRequest webRequest)
    {
        var requestData = @"{""receiptId"": """ + uidString + @"""}"; // JSON which will be send as request
        using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
        {
            streamWriter.Write(requestData);
        }
    }

    protected static string GetWebResponseString(HttpWebRequest webConnection)
    {
        HttpWebResponse httpResponse = (HttpWebResponse)webConnection.GetResponse();
        string result;

        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            result = streamReader.ReadToEnd();
        }

        Console.WriteLine(httpResponse.StatusCode);
        httpResponse.Close();
        return result;
    }
}
