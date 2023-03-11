using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdpTicketGetter : WebManager
{
    /// Adress of website for getting tickets from
    private static string OpdAdress = "";

    /// <summary>
    /// Sends web request to OpdAdress and request info about ticked based on inputed ticketUid.
    /// </summary>
    /// <param name="ticketUid">Unique identifier (UID) of ticket registration.</param>
    /// <returns>Json string containg information about ticket.</returns>
    public static string GetTicketOpdJsonString(string ticketUid)
    {
        HttpWebRequest requestConnection = CreateWebRequestConnection("POST", OpdAdress); // post should be methode of requesting

        SendWebRequest(ticketUid, requestConnection);
        Debug.Log($"Recieved string from opd web request: {requestConnection}");
        return GetWebResponseString(requestConnection);
    }
}
