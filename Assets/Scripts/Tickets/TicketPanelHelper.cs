using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.UI;

public class TicketPanelHelper : MonoBehaviour
{
    //[SerializeField]
    //private PanelManager _panelManager;

    //bool restartCamera = false;
    //private void Awake()
    //{
    //    ClearTicketPanel();
    //}
    //public void ClearTicketPanel()
    //{
    //    ClearTicketHeader();
    //    ClearTicketItems();
    //}

    ///// <summary>
    ///// Clear all inputfields int ticket header
    ///// </summary>
    //private void ClearTicketHeader()
    //{
    //    Helper.ClearInputField(_panelManager.GetPanel(TicketPanel.TicketName));
    //    Helper.ClearInputField(_panelManager.GetPanel(TicketPanel.TicketUID));
    //    Helper.ClearInputField(_panelManager.GetPanel(TicketPanel.ShopName));
    //    Helper.ClearInputField(_panelManager.GetPanel(TicketPanel.ShopAdress));
    //    Helper.ClearInputField(_panelManager.GetPanel(TicketPanel.TicketPrice));
    //}

    ///// <summary>
    ///// Delete all items in ticket.
    ///// </summary>
    //private void ClearTicketItems()
    //{
    //    GameObject ticketItemsParent = _panelManager.GetPanel(TicketPanel.ScrollableComponent);
    //    foreach (Transform item in ticketItemsParent.transform)
    //    {
    //        Destroy(item.gameObject);
    //    }
    //}

    //public void InitializeCreatingTicket()
    //{// ToDo currently its place holder, after creating ticket objects complete rework is needed
    //    ClearTicketPanel();
    //}
    //public void InitializeLoadingTicket(string uid, bool restartCamera = false)
    //{
    //    this.restartCamera = restartCamera;

    //    //gameObject.SetActive(true);

    //    Ticket currTicket = new Ticket();
    //    currTicket.Initiate(uid);

    //    ////temp showcase in panel
    //    //_panelManager.GetPanel(TicketPanel.TicketUID).GetComponent<InputField>().text = uid;
    //    //_panelManager.GetPanel(TicketPanel.ShopName).GetComponent<InputField>().text = currTicket.GetReceiptShowcase().shopName;
    //    //_panelManager.GetPanel(TicketPanel.TicketName).GetComponent<InputField>().text = currTicket.GetReceiptShowcase().issueDate;
    //    //_panelManager.GetPanel(TicketPanel.TicketPrice).GetComponent<InputField>().text = currTicket.GetReceiptShowcase().price;
    //    //end of temp
    //    if (restartCamera) _panelManager.GetPanel(CameraSubpanels.CameraScreen).GetComponent<CameraManager>().StartSearching();
    //    if (currTicket == null) return;
    //    //SaveTicket(currTicket);
    //    TicketSaver.SaveTicket(currTicket);
    //}

    //    public void SaveTicket(Ticket ticket)
    //    {
    //        //clearing of Temp panel delete in next faze
    //        //ClearTicketPanel();
    //        //gameObject.SetActive(false);
    //        //if (restartCamera) _panelManager.GetPanel(CameraSubpanels.CameraScreen).GetComponent<CameraManager>().StartSearching();
    //        // maybe move restarting camera to panelManager

    //        //check ticket before saving
    //        //ToDo maybe better check
    //        if (ticket == null) return;

    //        //saving ticket into file
    //        //TicketSaver.SaveTicket(ticket);
    //    }
}
