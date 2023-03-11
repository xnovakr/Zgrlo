using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TicketSaver
{
    //ToDo replace debug logs with trace logs
    public static void SaveTicket(string defaultPath, Ticket ticket)
    {
        Debug.Log("Saving ticket.");

        string ticketUid = ticket.GetUID();
        string ticketJson = ticket.GetJsonString();
        string[] ticketDate = SeparateDates(ticket.GetReceiptShowcase().issueDate);
        //inverting date format so it starts with year
        ticketDate = Helper.InvertArray(ticketDate);

        //creating file for saving file, needs rework
        string[] tempPath = new string[0];
        foreach (string date in ticketDate)
        {
            tempPath = tempPath.AppendArray(date);
            FileManager.CreateFolder(defaultPath, tempPath);
        }
        //saving file
        FileManager.CreateFile(defaultPath, ticketJson, ticketDate.AppendArray(ticketUid));
        Debug.Log("Tticket saved.");
    }
    public static void DeleteTicket(string defaultPath, string ticketUid, string ticketDate)
    {
        Debug.Log("Deleting ticket.");
        string[] ticketDateArray = SeparateDates(ticketDate);
        ticketDateArray = Helper.InvertArray(ticketDateArray);

        //deleting ticket
        FileManager.DeleteFile(defaultPath, ticketDateArray.AppendArray(ticketUid));

        //deleting empty folders
        do
        {
            if (FileManager.FolderIsEmpty(defaultPath, ticketDateArray)) FileManager.DeleteFolder(defaultPath, ticketDateArray);
            Array.Resize(ref ticketDateArray, ticketDateArray.Length - 1);
        }while (ticketDateArray.Length > 0);
    }
    public static void UpdateTicket(string defaultPath, Ticket ticket)
    {
        //rework to just rewriting file content
        Debug.Log("Updating ticktet" + ticket.GetUID());
        DeleteTicket(defaultPath, ticket.GetUID(), ticket.GetReceipt().receipt.issueDate);
        SaveTicket(defaultPath, ticket);
    }
    public static void SaveTicketShowcase()
    {
        Debug.Log("Saving ticket showcase.");
    }
    public static void RemoveTicketFromShowcase()
    {
        Debug.Log("Removing ticket showcase.");
    }
    public static void DeleteTicketShowcases()
    {
        Debug.Log("Deleting ticket showcase.");
    }

    public static string[] SeparateDates(string date)
    {
        Debug.Log("Separating dates");
        var split = date.Split(' ');
        return split[0].Split('.');
    }

}
