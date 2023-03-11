using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using TMPro;
using TicketObjects;

public class PanelManager : MonoBehaviour
{
    private PanelHolder panelHolder;
    [SerializeField]
    private GameObject currnetPanel;
    /// <summary>
    /// List of uid of loaded tickets.
    /// </summary>
    [SerializeField]
    private string[] loadedTickets;



    private void Awake()
    {
        panelHolder = GetComponent<PanelHolder>();
        currnetPanel = GetPanel(MainPanels.CameraReader);
    }

    // GETTERS FOR PANELS 

    /// <summary>
    /// Switch between lists of panels based on type of input enum type and calls search function with apropriate parameters.
    /// </summary>
    /// <typeparam name="T">Enum of panel type.</typeparam>
    /// <param name="panel">Enum of panel. Switch search array.</param>
    /// <returns>Returns panel GameObject based on input parameter panel.</returns>
    /// <exception cref="Exception">Throws exceptions if input paremeter wasn't found in switch statement.</exception>
    public GameObject GetPanel<TEnum>(TEnum panel)
    {
        if (panel == null) throw new ArgumentNullException(nameof(panel));

        switch (panel)
        {
            case Prefabs:
                return panelHolder.GetPrefab(panel.ToString());
            case ManagerObjects managerObjects:
                return GetPanel(panelHolder.managerObjects, panel.ToString());
            case MainPanels mainPanels:
                return GetPanel(panelHolder.mainObjects, panel.ToString());
            case CameraReader cameraSubpanels:
            case TicketInfo ticketInfo:
            case TicketInfoDetails ticketInfoDetails:
            case TicketInfoBody ticketInfoBody:
            case TicketNotification ticketNotification:
            case TicketInfoHeader ticketInfoHeader:
            case CategoryPicker categoryPicker:
            case CreateCategoryPanel createCategoryPanel:
            case DataPanel dataPanel:
            case DataHeader dataHeader:
            case DataList dataList:
            case SettingsPanel settingsPanel:
            case SettingsHeader settingsHeader:
            case SettingsBody settingsBody:
            case CategoryManager categoryManager:
            case OtherPanels otherPanels:
                return GetPanelFromParent(panel);
            default:
                throw new Exception($"Panel {panel} wans't found in panel types.");
        }
    }

    /// <summary>
    /// Returns gameobject of panel from list of panelList where panel name is equal to panelName.
    /// Thows exception if list of panels is empty or requested panel wasn't found.
    /// </summary>
    /// <param name="panelList">List of gameobjects where to look.</param>
    /// <param name="panelName">Name of panel that is being searched for.</param>
    private GameObject GetPanel(GameObject[] panelList, string panelName)
    {
        if (panelList.Length == 0) throw new ArgumentNullException(nameof(panelList));
        if (string.IsNullOrEmpty(panelName)) throw new ArgumentNullException(nameof(panelName));

        foreach (GameObject panel in panelList)
        {
            if (panelName == panel.name) return panel;
        }
        throw new Exception($"Panel {panelName} wasn't found in list of panels.");
    }

    /// <summary>
    /// This function will find object that is stored in array of parents objects.
    /// </summary>
    /// <typeparam name="TEnum">Enum of panel type.</typeparam>
    /// <param name="panel">Enum of panel. Switch search array.</param>
    /// <returns>Returns panel GameObject based on input parameter panel.</returns>
    /// <exception cref="Exception">Throws exception based on search fail.</exception>
    private GameObject GetPanelFromParent<TEnum>(TEnum panel)
    {
        if (panel == null) throw new ArgumentNullException(nameof(panel));

        ParentObject[] parentObject;

        // this switch choose apropriate parentObject array
        switch (panel)
        {
            case CameraReader cameraReader:
            case TicketNotification ticketNotification:
                parentObject = panelHolder.CameraReaderObjects;
                break;
            case DataPanel dataPanel:
            case DataHeader dataHeader:
            case DataList dataList:
                parentObject = panelHolder.DataPanelObjects;
                break ;
            case SettingsPanel settingsPanel:
            case SettingsHeader settingsHeader:
            case SettingsBody settingsBody:
            case CategoryManager categoryManager:
                parentObject = panelHolder.SettingsPanelObjects;
                break;
            case OtherPanels otherPanels:
            case CreateCategoryPanel createCategoryPanel:
            case TicketInfo ticketInfo:
            case TicketInfoHeader ticketInfoHeader:
            case TicketInfoBody ticketInfoBody:
            case TicketInfoDetails ticketInfoDetails:
            case CategoryPicker categoryPicker:
                parentObject = panelHolder.OtherPanels;
                break;
            default:
                throw new Exception($"Parent object for {panel} wasn't found.");
        }

        // search through arrays on parent object and pick wanted object
        foreach (ParentObject parent in parentObject)
        {
            foreach (GameObject obj in parent.children)
            {
                if (panel.ToString() == obj.name) return obj;
            }
        }

        throw new Exception($"Panel {panel} wasn't found in parent panels.");
    }

    //********************


    // GETTERS AND SETTERS FOR TICKETS (needed here for easy access)
    
    /// <summary>
    /// This function will find object of ticket showcase based on given ticket UID.
    /// </summary>
    /// <param name="ticketUid">UID of ticket that should be found.</param>
    /// <returns>Gameobject for ticket showcase.</returns>
    /// <exception cref="ArgumentNullException">If given UID is null throws exception.</exception>
    public GameObject GetTicketObject(string ticketUid)
    {
        if (string.IsNullOrEmpty(ticketUid)) throw new ArgumentNullException(nameof(ticketUid));

        return panelHolder.GetLoadedTicket(ticketUid);
    }

    /// <summary>
    /// Get array of UID of currently loaded tickets
    /// </summary>
    /// <returns>Array of strings.</returns>
    public string[] GetLoadedTickets() { return loadedTickets; }

    /// <summary>
    /// Add given UID to list of loaded tickets
    /// </summary>
    /// <param name="newUid">Ticket of newly loaded ticket.</param>
    /// <exception cref="ArgumentNullException">If given UID is null or empty exception is thrown.</exception>
    public void AddLoadedTicket(string newUid)
    {
        if (string.IsNullOrEmpty(newUid)) throw new ArgumentNullException(nameof(newUid));

        loadedTickets = Helper.AppendArray(loadedTickets, newUid);
    }

    /// <summary>
    /// Add ticket showcase object to list of loaded ticket showcase objects
    /// </summary>
    /// <param name="ticket">GameObject of ticket showcase that is loaded.</param>
    /// <exception cref="ArgumentNullException">If ticket is null throws exception.</exception>
    public void AddLoadedTicketObject(GameObject ticket)
    {
        if (ticket == null) throw new ArgumentNullException(nameof(ticket));

        panelHolder.AddLoadedTicketShowcase(ticket);
    }

    /// <summary>
    /// This function removes tickets UID from list of loaded tickets.
    /// </summary>
    /// <param name="ticketUid">UID of ticket to be removed from loaded tickets.</param>
    /// <exception cref="ArgumentNullException">If given UID is null or empty exception is thrown.</exception>
    public void RemoveLoadedTicket(string ticketUid)
    {
        if (string.IsNullOrEmpty(ticketUid)) throw new ArgumentNullException(nameof(ticketUid));

        loadedTickets = Array.FindAll(loadedTickets, val => val != ticketUid);
    }

    /// <summary>
    /// Set array of ticket UIDs and loaded Showcases to 0 lenght.
    /// </summary>
    public void CleanLoadedTickets()
    {
        loadedTickets = new string[0];
        panelHolder.CleanLoadedTicketShowcases();
    }

    /// <summary>
    /// Get name of saved ticket based on given UID.
    /// </summary>
    /// <param name="UID">UID of ticket, which name you want</param>
    /// <returns>Name of ticket</returns>
    /// <exception cref="ArgumentNullException">If UID is null or ticket wasn't found exception is thrown.</exception>
    public string GetLoadedTicketName(string UID)
    {
        if (string.IsNullOrEmpty(UID)) throw new ArgumentNullException(nameof(UID));

        foreach (GameObject ticket in panelHolder.GetLoadedShowcases())
        {
            if (ticket.name == UID)
            {
                return ticket.GetComponent<TicketHolder>().GetName();
            }
        }
        throw new Exception($"Ticket with givent UID wasn't found when searching for name. Given UID: {UID}");
    }

    //********************


    // PANEL CONTROL

    /// <summary>
    /// Turn off all panels except given exceptions.
    /// </summary>
    /// <param name="exceptions">Panels that should stay on.</param>
    public void DeactivatePanels(params string[] exceptions)
    {
        foreach (GameObject panel in panelHolder.mainObjects)
        {
            if (!exceptions.Contains(panel.name)) panel.SetActive(false);
        }
    }
    
    /// <summary>
    /// Function for closing panels. Mostly used for buttons.
    /// </summary>
    /// <param name="panel">Panel to be closed.</param>
    /// <exception cref="ArgumentNullException">If panel is null throws exception</exception>
    public void ClosePanel(GameObject panel)
    {
        if (panel == null) throw new ArgumentNullException(nameof(panel));

        panel.SetActive(false);

        if (panel.name == MainPanels.TicketInfo.ToString()) panel.GetComponent<TicketInfoPanelHelper>().Cleanup();
    }

    /// <summary>
    /// Swap panels between saved current and new given panel.
    /// Run animations for panel swap.
    /// </summary>
    /// <param name="newPanel">panel that should be shown.</param>
    /// <exception cref="ArgumentNullException">If one of panels is null, exception is thrown.</exception>
    /// <exception cref="Exception">If transition for given case isn't found exception is thrown.</exception>
    public void SwapPanels(GameObject newPanel)
    {
        if (newPanel == null) throw new ArgumentNullException(nameof(newPanel));
        if (currnetPanel == null) throw new ArgumentNullException(nameof(newPanel));

        string newPanelName = newPanel.name;
        string currPanelName = currnetPanel.name;

        if (currPanelName == newPanelName) return;


        Animator currPanel = currnetPanel.GetComponent<Animator>();
        Animator newAnimator = newPanel.GetComponent<Animator>();

        newPanel.SetActive(true);
        // end of setup
        // setting up animations
        if (currPanelName == MainPanels.CameraReader.ToString())
        { // if current panel is camera reader same anymations are always played.
            currPanel.Play(PanelAnimations.MidToLeftAnim.ToString(), 0);
            newAnimator.Play(PanelAnimations.MidToRightAnimReversed.ToString(), 0);

            GetPanel(CameraReader.CameraScreen).GetComponent<CameraManager>().StopSearching();
        }
        else if (currPanelName == MainPanels.DataPanel.ToString())
        { //if current panel is data panel
            if (newPanelName == MainPanels.CameraReader.ToString())
            { // if new panel is camera reader
                GetPanel(CameraReader.CameraScreen).GetComponent<CameraManager>().StartSearching(); // maybe start on animation finish

                currPanel.Play(PanelAnimations.MidToRightAnim.ToString(), 0);
                newAnimator.Play(PanelAnimations.MidToLeftAnimReversed.ToString(), 0);
            }
            else if (newPanelName == MainPanels.SettingsPanel.ToString())
            { // if new panel is settings panel
                currPanel.Play(PanelAnimations.MidToLeftAnim.ToString(), 0);
                newAnimator.Play(PanelAnimations.MidToRightAnimReversed.ToString(), 0);
            }
            else
            { //if no transition is found
                throw new Exception($"Transition from panel {currPanelName} to {newPanelName} wasn't found.");
            }
        }
        else if (currPanelName == MainPanels.SettingsPanel.ToString())
        {// if current panel is settings same anymations are always played.
            if (newPanelName == MainPanels.CameraReader.ToString()) GetPanel(CameraReader.CameraScreen).GetComponent<CameraManager>().StartSearching(); // maybe start on animation finish

            currPanel.Play(PanelAnimations.MidToRightAnim.ToString(), 0);
            newAnimator.Play(PanelAnimations.MidToLeftAnimReversed.ToString(), 0);
        }
        currnetPanel = newPanel;
    }

    public void ShowHelperPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.position = Vector3.zero;
    }
    //**************

    // SETTING UP PANELS

    /// <summary>
    /// Setup category picket for item categories
    /// </summary>
    /// <param name="itemObject">Object that holds item to be updated.</param>
    /// <param name="item">item to be updated</param>
    /// <param name="ticketUID">UID of parent ticket of item</param>
    public void SetupCategryPicker(Transform itemObject, Item item, string ticketUID)
    {
        if (itemObject == null) throw new ArgumentNullException(nameof(itemObject));
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (ticketUID == null) throw new ArgumentNullException(nameof(ticketUID));

        GameObject categoryPicker = GetPanel(OtherPanels.CategoryPicker);
        GameObject categoryBlock = GetPanel(Prefabs.CategoryBlockPrefab);
        GameObject parentTicket = GetTicketObject(ticketUID);
        Transform scrollableComponent = GetPanel(CategoryPicker.ScrollableComponent).transform;
        List<string> categories = GetPanel(ManagerObjects.ApplicationManager).GetComponent<ApplicationManager>().GetCategories(Categories.Items);

        Helper.CleanScrollableComponent(scrollableComponent);

        Action<string> UpdateCategory = (category) =>
        {
            Item newItem = item;
            newItem.itemCategory = category;
            parentTicket.GetComponent<TicketHolder>().UpdateItem(item, newItem);
            itemObject.GetComponent<TextMeshProUGUI>().SetText(category);

            categoryPicker.SetActive(false);
        };

        SetupCategoties(categories, UpdateCategory, scrollableComponent);
        categoryPicker.SetActive(true);
    }

    /// <summary>
    /// Setup category picker for shop categories
    /// </summary>
    /// <param name="ticketShowcase">Ticket showcase that should be categorized</param>
    public void SetupCategryPicker(GameObject ticketShowcase)
    {
        if (ticketShowcase == null) throw new ArgumentNullException(nameof(ticketShowcase));

        GameObject categoryPicker = GetPanel(OtherPanels.CategoryPicker);
        Transform scrollableComponent = GetPanel(CategoryPicker.ScrollableComponent).transform;
        List<string> categories = GetPanel(ManagerObjects.ApplicationManager).GetComponent<ApplicationManager>().GetCategories(Categories.Shop);

        Helper.CleanScrollableComponent(scrollableComponent);

        Action<string> UpdateCategory = (category) =>
        {
            ticketShowcase.GetComponent<TicketHolder>().UpdateCategory(category);
            categoryPicker.SetActive(false);
        };

        SetupCategoties(categories, UpdateCategory, scrollableComponent);
        categoryPicker.SetActive(true);
    }

    /// <summary>
    /// Setup given categories under given parent that will call given buttonAction on category selection.
    /// </summary>
    /// <param name="categories">List of categories to be setup.</param>
    /// <param name="buttonAction">Function to be called on category selection.</param>
    /// <param name="parent">Parent of category objects.</param>
    public void SetupCategoties(List<string> categories, Action<string> buttonAction, Transform parent)
    {
        GameObject categoryBlock = GetPanel(Prefabs.CategoryBlockPrefab);

        foreach (string category in categories)
        {
            GameObject categoryObj = GameObject.Instantiate(categoryBlock, parent);
            categoryObj.name = category;
            categoryObj.transform.Find(CategoryBlockPrefab.CategoryName.ToString()).GetComponent<TextMeshProUGUI>().SetText(category);

            categoryObj.GetComponent<Button>().onClick.AddListener(() => buttonAction(category));
        }
    }
    //******************
}
