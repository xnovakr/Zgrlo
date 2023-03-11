using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicketObjects;

public static class TicketLoader
{
    public static ReceiptInfo GetReciptFromUid(string uid)
    {
        return ReceiptDeserializer(OdpTicketGetter.GetTicketOpdJsonString(uid));
    }
    public static ReceiptShowcase ReceiptGetShowcaseFromJson(string JsonTicket)
    {
        if (string.IsNullOrEmpty(JsonTicket)) return null;
        string[] jsonString = JsonTicket.Split(TicketSeriliarizer.JSON_SEPARATOR);
        if (jsonString.Length <= 1)
        {
            Debug.Log("Json string didn't contain receipt showcase!");
            return null;
        }
        return JsonUtility.FromJson<ReceiptShowcase>(jsonString[0]);

    }
    public static ReceiptInfo ReceiptDeserializer(string JsonTicket)
    {
        if (string.IsNullOrEmpty(JsonTicket)) return null;
        if (JsonTicket.Split(TicketSeriliarizer.JSON_SEPARATOR).Length > 1) JsonTicket = JsonTicket.Split(TicketSeriliarizer.JSON_SEPARATOR)[1];
        ReceiptInfo returnInfo = JsonUtility.FromJson<ReceiptInfo>(JsonTicket);

        returnInfo.searchIdentification = GetSearchIdentificationFromTicketJson(JsonTicket);
        returnInfo.receipt = GetReceiptFromTicket(JsonTicket);

        return returnInfo;
    }
    private static SearchIdentification GetSearchIdentificationFromTicketJson(string JsonTicket)
    {
        return JsonUtility.FromJson<SearchIdentification>(GetJsonPart(JsonTicket, "searchIdentification"));
    }
    private static Organization GetOrganizationFromTicketJson(string JsonTicket)
    {
        return JsonUtility.FromJson<Organization>(GetJsonPart(JsonTicket, "organization"));
    }
    private static Unit GetUnitFromTicketJson(string JsonTicket)
    {
        return JsonUtility.FromJson<Unit>(GetJsonPart(JsonTicket, "unit"));
    }
    private static Item GetItemFromJson(string Json)
    {
        return JsonUtility.FromJson<Item>(Json);
    }
    private static Item[] GetItemFieldFromJson(string Json)
    {
        string jsonItemField = GetJsonField(Json, "items");
        List<Item> jsonItems = new List<Item>();
        foreach (string item in jsonItemField.Split('}'))
        {
            if (item.Length > 2) jsonItems.Add(GetItemFromJson(item.Remove(0, 1) + "}")); // check if string is long enough to be item, reconstrut missing part and then get it from JSON and add it to list
        }
        return jsonItems.ToArray();
    }
    private static Receipt GetReceiptFromTicketJson(string JsonTicket)
    {
        return JsonUtility.FromJson<Receipt>(GetJsonPart(JsonTicket, "receipt"));
    }
    private static Receipt GetReceiptFromTicket(string JsonTicket)
    {
        Receipt receipt = GetReceiptFromTicketJson(JsonTicket);
        receipt.organization = GetOrganizationFromTicketJson(JsonTicket);
        receipt.unit = GetUnitFromTicketJson(JsonTicket);
        receipt.items = GetItemFieldFromJson(JsonTicket);

        return receipt;
    }
    private static string GetJsonPart(string Json, string partName)
    {
        return GetJson(Json, partName, new char[] { '{', '}' });
    }
    private static string GetJsonField(string Json, string partName)
    {
        return GetJson(Json, partName, new char[] { '[', ']' });
    }
    private static string GetJson(string Json, string partName, char[] splittingChar)
    {
        if (Json.Length <= 1) return null;
        bool started = false;
        int bracketCounter = 1;
        string[] tempText = Json.Split(new string[] { "\"" + partName + "\"" }, System.StringSplitOptions.None);
        string returnString = "";
        int indexCounter = 0;


        while (bracketCounter > 0)
        {
            char c = tempText[1][indexCounter];
            indexCounter++;
            if (c == splittingChar[0])
            {
                if (started) bracketCounter++;
                started = true;
            }
            if (c == splittingChar[1])
            {
                bracketCounter--;
            }
            if (started) returnString += c;
            if (indexCounter >= tempText[1].Length) break;
        }
        return returnString;
    }
}
