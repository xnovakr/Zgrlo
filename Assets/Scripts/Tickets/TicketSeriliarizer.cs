using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicketObjects;

public static class TicketSeriliarizer
{
    public static char JSON_SEPARATOR = '~';
    // Json output will be ReceiptShowcase | Receipt Info
    public static string SerializeTicket(Ticket ticket)
    {
        string receiptShowcase = JsonUtility.ToJson(ticket.GetReceiptShowcase());
        string receiptInfo = SerializeReceiptInfo(ticket.GetReceipt());
        return receiptShowcase + JSON_SEPARATOR + receiptInfo;
    }
    public static string SerializeReceiptInfo(ReceiptInfo receiptInfo)
    {
        string receipt = SerializeReceipt(receiptInfo.receipt);
        string searchIdentification = SerializeSearchIdentification(receiptInfo.searchIdentification);

        string jsonInfo = JsonUtility.ToJson(receiptInfo);
        jsonInfo = jsonInfo.Remove(jsonInfo.Length - 1);
        jsonInfo += receipt + searchIdentification + "}";

        return jsonInfo;
    }

    private static string SerializeReceipt(Receipt receipt)
    {
        string receiptJson = JsonUtility.ToJson(receipt);
        string items = SerializeItems(receipt.items);
        string organization = SerializeOrganization(receipt.organization);
        string unit = SerializeUnit(receipt.unit);

        receiptJson = receiptJson.Remove(receiptJson.Length - 1);
        receiptJson += items + organization + unit + ",\"exemption\":false}";

        return ",\"receipt\":" + receiptJson;
    }
    private static string SerializeSearchIdentification(SearchIdentification searchIdentification)
    {
        return ",\"searchIdentification\":" + JsonUtility.ToJson(searchIdentification);
    }

    private static string SerializeItems(Item[] items)
    {
        string itemsJson = ",\"items\":[";
        foreach(Item item in items)
        {
            if (itemsJson.Length > 12) itemsJson += ",";
            itemsJson += JsonUtility.ToJson(item);
        }
        return itemsJson + "]";
    }
    private static string SerializeOrganization(Organization organization)
    {
        return ",\"organization\":" + JsonUtility.ToJson(organization);
    }
    private static string SerializeUnit(Unit unit)
    {
        return ",\"unit\":" + JsonUtility.ToJson(unit);
    }
}
