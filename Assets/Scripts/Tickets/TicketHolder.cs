using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicketObjects;
using Enums;

public class TicketHolder : MonoBehaviour
{
    Ticket ticket;
    TicketInfoPanelHelper ticketInfoPanelHelper;

    public void SetTicket(Ticket ticket) { this.ticket = ticket; }
    public void SetTicketInfoPanelHelper(TicketInfoPanelHelper ticketInfoPanelHelper) { this.ticketInfoPanelHelper = ticketInfoPanelHelper; }
    public void UpdateTicketSave()
    {
        ticket.SetJsonString(TicketSeriliarizer.SerializeTicket(ticket));
        ApplicationManager applicationManager = GameObject.Find("PanelManager").GetComponent < PanelManager>().GetPanel(ManagerObjects.ApplicationManager).GetComponent<ApplicationManager>();
        TicketSaver.UpdateTicket(applicationManager.GetDefaultPaht(), ticket);
        Debug.Log(TicketSeriliarizer.SerializeTicket(ticket));
    }
    public void UpdateItem(Item oldItem, Item newItem)
    {
        Debug.Log("UpdateItemFor " + ticket.GetUID());
        bool changed = false;
        Receipt receipt = ticket.GetReceipt().receipt;
        
        for (int i = 0; i < receipt.items.Length; i++)
        {
            if (receipt.items[i].itemUid == oldItem.itemUid)
            {
                receipt.items[i] = newItem;
                changed = true;
                break;
            }
        }
        if (changed) UpdateTicketSave();
        else Debug.Log("Item wasn't found");

    }
    public string GetName()
    {
        return ticket.receiptShowcase.shopName;
    }
    public void UpdateCategory(string categoryName)
    {
        ticket.receiptShowcase.ticketCategory = categoryName;
        UpdateTicketSave();
        ticket.UpdateCategoryText(gameObject);
    }
}
