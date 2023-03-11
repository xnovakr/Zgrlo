using System;
using UnityEngine;
/// <summary>
/// Contains lists of important loaded objects for usage in application
/// </summary>
public class PanelHolder : MonoBehaviour
{
    /// <summary>
    /// List of objects that are running most important scripts of application
    /// </summary>
    public GameObject[] managerObjects;
    /// <summary>
    /// List of parent objects deciding structure of canvas
    /// </summary>
    public GameObject[] mainObjects;

    /// <summary>
    /// All child objects of cameraReader object. They are placed in array based on their parent object.
    /// </summary>
    public ParentObject[] CameraReaderObjects;
    /// <summary>
    /// All child objects of dataPanel object. They are placed in array based on their parent object.
    /// </summary>
    public ParentObject[] DataPanelObjects;
    /// <summary>
    /// All child objects of SettingsPanel object. They are placed in array based on their parent object.
    /// </summary>
    public ParentObject[] SettingsPanelObjects;
    /// <summary>
    /// All child objects of BasicStructure object
    /// </summary>
    //public GameObject[] BasicStructureObjects;
    /// <summary>
    /// All child objects of Helper objects. Those objects are: TicketInfoDetailsObjects, CategoryPicker, CreateCategoryPanel
    /// </summary>
    public ParentObject[] OtherPanels;

    //if all good then delete bellow
    /// <summary>
    /// All child objects of TicketInfo object, and child objects of ticketInfoDetails
    /// </summary>
    //public GameObject[] TicketInfoDetailsObjects;
    /// <summary>
    /// All child objects of CategoryPicker object
    /// </summary>
    //public GameObject[] CategoryPicker;
    /// <summary>
    /// All child objects of CreateCategoryPanel object
    /// </summary>
    //public GameObject[] CreateCategoryPanel;


    /// <summary>
    /// List of all prefabs objects. They are listed in enum Prefabs.
    /// </summary>
    [SerializeField]
    private GameObject[] prefabObjects;

    /// <summary>
    /// List of uid of loaded tickets.
    /// </summary>
    [SerializeField]
    private GameObject[] loadedTicketShowcaseObjects;


    /// <summary>
    /// Getter for prefab object. Return prefab object based on given prefabName.
    /// If prefab wasn't found exception is thrown.
    /// </summary>
    /// <param name="prefabName">Name of prefab to be found.</param>
    /// <returns>Prefab object</returns>
    /// <exception cref="System.Exception">Prefab wasn't found.</exception>
    public GameObject GetPrefab(string prefabName)
    {
        foreach(GameObject prefab in prefabObjects)
        {
            if(prefab.name == prefabName) return prefab;
        }
        throw new System.Exception("Prefab with given name wasn't found. Prefab name: " + prefabName);
    }


    // GETTER AND SETTERS FOR LOADED TICKETS

    /// <summary>
    /// Returns all objcts for loaded showcases.
    /// </summary>
    /// <returns></returns>
    public GameObject[] GetLoadedShowcases() { return loadedTicketShowcaseObjects; }
    /// <summary>
    /// Return ticket showcase object for ticket based on given ticketUID.
    /// If showcase wasn't found exception is thrown.
    /// </summary>
    /// <param name="ticketUID">UID of ticket you want to find showcase for.</param>
    /// <returns>Gameobject of ticket showcase for ticket with given UID</returns>
    /// <exception cref="System.Exception">thrown if ticket was not found</exception>
    public GameObject GetLoadedTicket(string ticketUID)
    {
        foreach (GameObject obj in loadedTicketShowcaseObjects)
        {
            if (obj.name == ticketUID) return obj;
        }
        throw new System.Exception("Ticket object with given uid not found. UID: " + ticketUID);
    }
    /// <summary>
    /// Add ticket showcase to list of loaded showcase by using function from helper class.
    /// </summary>
    /// <param name="ticket">Object to be added to list.</param>
    public void AddLoadedTicketShowcase(GameObject ticket)
    { // rework helper function so it edit input array so you dont have to rewrite it when calling function
        loadedTicketShowcaseObjects =  Helper.AppendArray(loadedTicketShowcaseObjects, ticket);
    }
    /// <summary>
    /// Remove showcase of ticket from list loaded ticket showcases based on name of given ticket object (name is ticket UID).
    /// </summary>
    /// <param name="ticket">Ticket which showcase is to be deleted.</param>
    public void RemoveLoadedTicketShowcase(GameObject ticket)
    {
        loadedTicketShowcaseObjects = Array.FindAll(loadedTicketShowcaseObjects, val => val.name != ticket.name);
    }
    /// <summary>
    /// Remove showcase of ticket from list of loaded showcases based on given ticket UID
    /// </summary>
    /// <param name="ticketUID">UID of ticket that is to be delete</param>
    public void RemoveLoadedTicketShowcase(string ticketUID)
    {
        loadedTicketShowcaseObjects = Array.FindAll(loadedTicketShowcaseObjects, val => val.name != ticketUID);
    }

    public void CleanLoadedTicketShowcases()
    {
        loadedTicketShowcaseObjects = new GameObject[0];
    }
}
[System.Serializable]
public struct ParentObject
{
    public string categoryName;
    public GameObject[] children;
}