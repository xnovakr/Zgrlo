using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TicketObjects;
using Enums;
using TMPro;

using Debug = UnityEngine.Debug;

public class Ticket 
{
    PanelManager panelManager;
    ReceiptInfo receipt;
    public Transform parent;
    [SerializeField]
    public ReceiptShowcase receiptShowcase;
    string uid, jsonString;
    public void Initiate (string UID)
    {
        panelManager = GameObject.Find("PanelManager").GetComponent<PanelManager>();
        uid = UID;
        jsonString = OdpTicketGetter.GetTicketOpdJsonString(uid);
        receipt = TicketLoader.ReceiptDeserializer(jsonString);
        CreateReceipitShowcase();
        SetupItemsCategory();
        jsonString = TicketSeriliarizer.SerializeTicket(this);
        InitiateShowcase();
    }
    public void Initiate (ReceiptInfo receiptInfo, string jsonString)
    {
        if (jsonString.Length <= 1 || jsonString == null) return;
        panelManager = GameObject.Find("PanelManager").GetComponent<PanelManager>();
        uid = receiptInfo.searchIdentification.internalReceiptId;
        this.jsonString = jsonString;
        receipt = receiptInfo;
        CreateReceipitShowcase();
        SetupItemsCategory();
        InitiateShowcase();
    }
    public ReceiptInfo GetReceipt() { return receipt; }
    public ReceiptShowcase GetReceiptShowcase() { return receiptShowcase; }
    public string GetUID() { return uid; }
    public string GetJsonString() { return jsonString; }
    public void SetJsonString(string newJson) { jsonString = newJson; }

    /// <summary>
    /// Create Receipt showcase with minimum necessary data.
    /// </summary>
    private void CreateReceipitShowcase()
    {
        receiptShowcase = TicketLoader.ReceiptGetShowcaseFromJson(jsonString);
        if (receipt == null || receiptShowcase != null) return;

        receiptShowcase = new ReceiptShowcase();

        receiptShowcase.shopName = receipt.receipt.organization.name;
        receiptShowcase.issueDate = receipt.receipt.issueDate;
        receiptShowcase.price = receipt.receipt.totalPrice.ToString();
        receiptShowcase.ticketCategory = panelManager.GetPanel(ManagerObjects.ApplicationManager).GetComponent<ApplicationManager>().GetCategories(Categories.Shop)[0];
    }
    public void InitiateShowcase()
    {
        if (panelManager == null) panelManager = GameObject.Find("PanelManager").GetComponent<PanelManager>();
        if (Array.Exists(panelManager.GetLoadedTickets(), val => val == uid)) return; // if ticket with this uid is already created then return
        Debug.Log("Initiating showcase into gameObject.");
        GameObject showcase = GameObject.Instantiate(panelManager.GetPanel(Prefabs.TicketPrefab), panelManager.GetPanel(DataList.ScrollableComponent).transform);
        parent = showcase.transform;
        //add to list of ticket UIDs
        panelManager.AddLoadedTicket(uid);
        //add to list of loaded objects
        panelManager.AddLoadedTicketObject(showcase);

        showcase.name = uid;
        showcase.transform.Find(TicketPrefab.TicketPrefabName.ToString()).GetComponent<Text>().text = receiptShowcase.shopName;
        showcase.transform.Find(TicketPrefab.TicketPrefabTime.ToString()).GetComponent<Text>().text = receiptShowcase.issueDate;
        showcase.transform.Find(TicketPrefab.TicketPrefabPrice.ToString()).GetComponent<Text>().text = receiptShowcase.price;
        showcase.transform.Find(TicketPrefab.TicketPrefabDelete.ToString()).GetComponent<Button>().onClick.AddListener(() => DeleteThisTicket());

        GameObject appManager = panelManager.GetPanel(ManagerObjects.ApplicationManager);
        ApplicationManager applicationManager = appManager.GetComponent<ApplicationManager>();

        //if doesn't work move it to this tickets creator.
        Action<TicketInfoPanelHelper> showTicketDetails = ticketInfoPanelHelper =>
        {
            if (!ticketInfoPanelHelper.gameObject.activeSelf) ticketInfoPanelHelper.gameObject.SetActive(true);
            ticketInfoPanelHelper.Cleanup();
            Debug.Log("Show ticket info started");
            ticketInfoPanelHelper.SetTicket(this);
            ticketInfoPanelHelper.Initialize();
            Debug.Log("Show ticket info finished");
            ticketInfoPanelHelper.transform.position = Vector3.zero;
            LayoutRebuilder.ForceRebuildLayoutImmediate(panelManager.GetPanel(TicketInfoDetails.TicketItems).GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(panelManager.GetPanel(TicketInfoBody.TicketDetails).GetComponent<RectTransform>());
        };
        TicketInfoPanelHelper ticketInfoHeplerScript = panelManager.GetPanel(MainPanels.TicketInfo).GetComponent<TicketInfoPanelHelper>();
        showcase.transform.Find(TicketPrefab.TicketPrefabDetailedInfoButton.ToString()).GetComponent<Button>().onClick.AddListener(() => showTicketDetails(ticketInfoHeplerScript));

        showcase.AddComponent<TicketHolder>();
        showcase.GetComponent<TicketHolder>().SetTicket(this);
        showcase.GetComponent<TicketHolder>().SetTicketInfoPanelHelper(ticketInfoHeplerScript);

        //setup categoty
        GameObject ticketCategory = showcase.transform.Find(TicketPrefab.TicketPrefabCategory.ToString()).gameObject;
        string showcaseCategory = receiptShowcase.ticketCategory;
        if (string.IsNullOrEmpty(showcaseCategory)) showcaseCategory = panelManager.GetPanel(ManagerObjects.ApplicationManager).GetComponent<ApplicationManager>().GetCategories(Categories.Shop)[0];

        //setting up category button
        ticketCategory.transform.Find(TicketPrefab.CategoryText.ToString()).GetComponent<TextMeshProUGUI>().SetText(showcaseCategory);

        Action setCategory = () =>
        {
            panelManager.SetupCategryPicker(showcase);
        };
        ticketCategory.GetComponent<Button>().onClick.AddListener(() => setCategory());

    }
    public void DeleteThisTicket()
    {
        Debug.Log("Deleting ticket: " + uid);
        if (panelManager == null) panelManager = GameObject.Find("PanelManager").GetComponent<PanelManager>();

        panelManager.RemoveLoadedTicket(uid);
        ApplicationManager applicationManager = panelManager.GetPanel(ManagerObjects.ApplicationManager).GetComponent<ApplicationManager>();
        TicketSaver.DeleteTicket(applicationManager.GetDefaultPaht(), uid, receiptShowcase.issueDate);
        foreach (Transform child in panelManager.GetPanel(DataList.ScrollableComponent).transform)
        {
            if(child.name == uid)
            {
                GameObject.Destroy(child.gameObject);
                return;
            }
        }
    }
    private void SetupItemsCategory()
    {
        string defaulCategory = panelManager.GetPanel(ManagerObjects.ApplicationManager).GetComponent<ApplicationManager>().GetCategories(Categories.Items)[0];
        foreach (Item item in receipt.receipt.items)
        {
            if (string.IsNullOrEmpty(item.itemCategory))
            {
                item.itemCategory = defaulCategory;
            }
            if (string.IsNullOrEmpty(item.itemUid))
            {
                item.itemUid = Guid.NewGuid().ToString();
            }
        }
    }
    public void UpdateCategoryText(GameObject showcase)
    {
        GameObject ticketCategory = showcase.transform.Find(TicketPrefab.TicketPrefabCategory.ToString()).gameObject;
        ticketCategory.transform.Find(TicketPrefab.CategoryText.ToString()).GetComponent<TextMeshProUGUI>().SetText(receiptShowcase.ticketCategory);
    }
}