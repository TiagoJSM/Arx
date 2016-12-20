using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

public static class GoogleApi
{
    private static readonly string GetCodeUrl = "https://accounts.google.com/o/oauth2/v2/auth?response_type=code&scope=https://www.googleapis.com/auth/drive.readonly&redirect_uri=urn:ietf:wg:oauth:2.0:oob&client_id={0}";
    private static readonly string GetTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
    private static readonly string GetFilesUrl = "https://www.googleapis.com/drive/v2/files?access_token={0}";
    private static readonly string GetSheetsUrl = "https://sheets.googleapis.com/v4/spreadsheets/{0}?access_token={1}";
    private static readonly string GetValuesInSheetUrl = "https://sheets.googleapis.com/v4/spreadsheets/{0}/values/{1}?access_token={2}";

    public static void RequestCodeOnBrowser(string clientId)
    {
        Process.Start(
            string.Format(GetCodeUrl, clientId));
    }

    public static string GetToken(string code, string clientId, string clientSecret)
    {
        var values = new NameValueCollection();
        values["client_id"] = clientId;
        values["client_secret"] = clientSecret;
        values["code"] = code;
        values["grant_type"] = "authorization_code";
        values["redirect_uri"] = "urn:ietf:wg:oauth:2.0:oob";

        var model =
            Post<TokenServiceModel>(
                GetTokenUrl,
                values,
                new Tuple<HttpRequestHeader, string>(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded"));
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

        return model.access_token;
    }

    public static ListFilesServiceModel GetFileList(string token)
    {
        return Get<ListFilesServiceModel>(string.Format(GetFilesUrl, token));
    }

    public static GetSheetsServiceModel GetSheets(string token, string fileId)
    {
        return Get<GetSheetsServiceModel>(string.Format(GetSheetsUrl, fileId, token));
    }

    public static string GetUrlForValuesInSheet(string token, string fileId, string sheetId)
    {
        return string.Format(GetValuesInSheetUrl, fileId, sheetId, token);
    }

    public static string[][] GetValuesInSheet(string token, string fileId, string sheetId)
    {
        return GetValuesInSheet(GetUrlForValuesInSheet(token, fileId, sheetId));
    }

    public static string[][] GetValuesInSheet(string url)
    {
        var data = GetData(url);
        var jObject = JSONObject.Create(data);
        var fieldObject = jObject.GetField("values");
        return fieldObject.list.Select(jObj =>
        {
            return jObj.list.Select(val => val.str).ToArray();
        })
        .ToArray();
    }

    private static TData Get<TData>(
        string address,
        params Tuple<HttpRequestHeader, string>[] headers)
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

        var data = GetData(address, headers);
        return JsonUtility.FromJson<TData>(data);
    }

    private static string GetData(
        string address,
        params Tuple<HttpRequestHeader, string>[] headers)
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

        using (var client = new WebClient())
        {
            for (var idx = 0; idx < headers.Length; idx++)
            {
                client.Headers.Add(headers[idx].First, headers[idx].Second);
            }

            return client.DownloadString(address);
        }
    }

    private static TData Post<TData>(
        string address,
        NameValueCollection data,
        params Tuple<HttpRequestHeader, string>[] headers)
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

        using (var client = new WebClient())
        {
            for(var idx = 0; idx < headers.Length; idx++)
            {
                client.Headers.Add(headers[idx].First, headers[idx].Second);
            }

            var response = client.UploadValues(GetTokenUrl, data);
            var responseString = Encoding.Default.GetString(response);
            return JsonUtility.FromJson<TData>(responseString);
        }
    }

    private static bool MyRemoteCertificateValidationCallback(
        System.Object sender, 
        X509Certificate certificate, 
        X509Chain chain, 
        SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }
}
