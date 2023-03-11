using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using TMPro;
using TicketObjects;


public class TicketInfoPanelHelper : MonoBehaviour
{
    private Ticket currTicket;
    private Receipt receipt;
    [SerializeField]
    PanelManager panelManager;

    public void SetTicket(Ticket ticket) { currTicket = ticket; }
    public Ticket GetTicket() { return currTicket; }

    public void Initialize()
    {
        Debug.Log("Initializing detailed info for ticket: " + currTicket.GetUID());
        receipt = currTicket.GetReceipt().receipt;
        SetupTicketInfo();
    }
    public void Cleanup()
    {
        Debug.Log("Doing cleanup for ticket info panel");

        CleanSellerInfo();
        CleanTicketPrice();
        CleanTicketDate();
        CleanTicketItems();
        CleanTicketOthers();
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
    }

    private void SetupTicketInfo()
    {
        SetupSellerInfo();
        SetupTicketPrice();
        SetupTicketDate();
        SetupTicketItems();
        SetupTicketOthers();
        //SetupTicketDPH();

        //LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
    }
    private void SetupSellerInfo()
    {
        panelManager.GetPanel(TicketInfoDetails.SellerName).GetComponent<Text>().text = receipt.organization.name;
        panelManager.GetPanel(TicketInfoDetails.SellerAdress).GetComponent<Text>().text = receipt.organization.streetName;
    }
    private void CleanSellerInfo()
    {
        panelManager.GetPanel(TicketInfoDetails.SellerName).GetComponent<Text>().text = "TempVal";
        panelManager.GetPanel(TicketInfoDetails.SellerAdress).GetComponent<Text>().text = "TempVal";
    }
    private void SetupTicketPrice()
    {
        panelManager.GetPanel(TicketInfoDetails.Price).GetComponent<Text>().text = receipt.totalPrice.ToString();
    }
    private void CleanTicketPrice()
    {
        panelManager.GetPanel(TicketInfoDetails.Price).GetComponent<Text>().text = "TempVal";
    }
    private void SetupTicketDate()
    {
        panelManager.GetPanel(TicketInfoDetails.Date).GetComponent<Text>().text = receipt.issueDate;
    }
    private void CleanTicketDate()
    {
        panelManager.GetPanel(TicketInfoDetails.Date).GetComponent<Text>().text = "TempVal";
    }
    private void SetupTicketItems()
    {
        GameObject itemPrefab = panelManager.GetPanel(Prefabs.ItemPrefab);
        GameObject itemCategories = itemPrefab.transform.Find(ItemPrefab.ItemCategorySelection.ToString()).gameObject;
        List<string> categories = panelManager.GetPanel(ManagerObjects.ApplicationManager).GetComponent<ApplicationManager>().GetCategories(Categories.Items);
        //Helper.SetupDropdown(itemCategories, categories);
        foreach (Item item in receipt.items)
        {
            SetupTicketItem(item);
        }
    }
    private void CleanTicketItems()
    {
        foreach (Transform item in panelManager.GetPanel(TicketInfoDetails.TicketItems).transform)
        {
            Destroy(item.gameObject);
        }
    }
    private void SetupTicketItem(Item item)
    {
        GameObject itemPrefab = Instantiate(panelManager.GetPanel(Prefabs.ItemPrefab), panelManager.GetPanel(TicketInfoDetails.TicketItems).transform);

        itemPrefab.name = item.name;
        itemPrefab.transform.Find(ItemPrefab.ItemName.ToString()).GetComponent<Text>().text = item.name;
        itemPrefab.transform.Find(ItemPrefab.ItemQuantity.ToString()).GetComponent<Text>().text = item.quantity.ToString();
        itemPrefab.transform.Find(ItemPrefab.ItemVatRate.ToString()).GetComponent<Text>().text = item.vatRate.ToString();
        itemPrefab.transform.Find(ItemPrefab.ItemPrice.ToString()).GetComponent<Text>().text = item.price.ToString();
        
        //setup categories
        Transform buttonCategory = itemPrefab.transform.Find(ItemPrefab.ItemCategorySelection.ToString());
        Transform buttonText = buttonCategory.Find(ItemPrefab.CategoryText.ToString());

        buttonText.GetComponent<TextMeshProUGUI>().SetText(item.itemCategory);

        Action setCategory = () =>
        {
            panelManager.SetupCategryPicker(buttonText, item, currTicket.GetUID());
        };
        buttonCategory.GetComponent<Button>().onClick.AddListener(() => setCategory());

        //Dropdown dropdown = itemPrefab.transform.Find(ItemPrefab.ItemCategorySelection.ToString()).GetComponent<Dropdown>();

        //foreach (Dropdown.OptionData option in dropdown.options)
        //{
        //    if (option.text == item.itemCategory) dropdown.value = dropdown.options.IndexOf(option);
        //}

        //Action updateTicketCategory = () =>
        //{
        //    string category = dropdown.options[dropdown.value].text;
        //    Item newItem = item;
        //    newItem.itemCategory = category;
        //    //GameObject ticket = panelManager.GetTicketObject(currTicket.GetUID());
        //    if (currTicket.parent != null) currTicket.parent.GetComponent<TicketHolder>().UpdateItem(item, newItem);
        //    else Debug.Log("Ticket " + currTicket.GetUID() + "wasn't found");
        //};
        //dropdown.onValueChanged.AddListener(x => updateTicketCategory());
    }
    private void SetupTicketOthers()
    {
        panelManager.GetPanel(TicketInfoDetails.PointOfSale).GetComponent<Text>().text = receipt.unit.streetName;
        panelManager.GetPanel(TicketInfoDetails.DIC).GetComponent<Text>().text = receipt.dic.ToString();
        panelManager.GetPanel(TicketInfoDetails.ICDPH).GetComponent<Text>().text = receipt.icDph;
        panelManager.GetPanel(TicketInfoDetails.ICO).GetComponent<Text>().text = receipt.ico.ToString();
        panelManager.GetPanel(TicketInfoDetails.CashierNumber).GetComponent<Text>().text = receipt.cashRegisterCode.ToString();
        panelManager.GetPanel(TicketInfoDetails.SerialNumber).GetComponent<Text>().text = receipt.receiptNumber.ToString();
        panelManager.GetPanel(TicketInfoDetails.UID).GetComponent<Text>().text = currTicket.GetUID();
        panelManager.GetPanel(TicketInfoDetails.OKP).GetComponent<Text>().text = receipt.okp;

    }
    private void CleanTicketOthers()
    {
        panelManager.GetPanel(TicketInfoDetails.PointOfSale).GetComponent<Text>().text = "TempVal";
        panelManager.GetPanel(TicketInfoDetails.DIC).GetComponent<Text>().text = "TempVal";
        panelManager.GetPanel(TicketInfoDetails.ICDPH).GetComponent<Text>().text = "TempVal";
        panelManager.GetPanel(TicketInfoDetails.ICO).GetComponent<Text>().text = "TempVal";
        panelManager.GetPanel(TicketInfoDetails.CashierNumber).GetComponent<Text>().text = "TempVal";
        panelManager.GetPanel(TicketInfoDetails.SerialNumber).GetComponent<Text>().text = "TempVal";
        panelManager.GetPanel(TicketInfoDetails.UID).GetComponent<Text>().text = "TempVal";
        panelManager.GetPanel(TicketInfoDetails.OKP).GetComponent<Text>().text = "TempVal";

    }
    private void SetupTicketDPH()
    {

    }
    private void CleanTicketDPH()
    {

    }
}
