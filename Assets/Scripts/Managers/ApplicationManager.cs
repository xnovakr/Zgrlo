using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class ApplicationManager : MonoBehaviour
{
    private static string ITEM_CATEGORIES_PLAYERPREFS = "itemCategories";
    private static string SHOP_CATEGORIES_PLAYERPREFS = "shopCategories";
    private static string DEFAULT_FILE_PATH = @"C:\Users\R\Desktop\TestSaving";
    private static char PLAYERPREFS_STRING_SEPARATOR = '~';

    [SerializeField]
    private List<string> itemCategories;
    [SerializeField]
    private List<string> shopCategories;

    private Categories activeCategoryEdit; // temp

    public PanelManager panelManager;
    private void Awake()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) Debug.Log("Running on windows");
        else DEFAULT_FILE_PATH = Application.persistentDataPath;
        LoadSavedCategories();
        LoadSavedTickets();
        CategorySettingInitiate(Categories.Shop); // move to proper position
        panelManager.DeactivatePanels(MainPanels.BasicStructure.ToString(), MainPanels.CameraReader.ToString());
    }
    /// <summary>
    /// Recursive function that goes through directory where tickets are saved and initialize them.
    /// </summary>
    /// <param name="path">adition to path loaded in function (null for default path)</param>
    private void LoadSavedTickets(string path = null)
    {
        string[] direcotries = FileManager.GetFolderContent(DEFAULT_FILE_PATH, path);
        foreach (string directory in direcotries)
        {
            if (Directory.Exists(directory)) LoadSavedTickets(directory);
            else
            {
                if (!directory.Contains(FileManager.GetSavedTicketSuffix())) continue;

                string fileContent = FileManager.GetFileContent(DEFAULT_FILE_PATH, directory);
                TicketObjects.ReceiptInfo receiptInfo =  TicketLoader.ReceiptDeserializer(fileContent);

                Ticket ticket = new Ticket();
                ticket.Initiate(receiptInfo, fileContent);
            }
        }
    }

    public List<string> GetCategories(Categories categories)
    {
        if (categories == Categories.Items) return itemCategories;
        if (categories == Categories.Shop) return shopCategories;
        return null;
    }
    public void AddCategory(Categories category, string item)
    {
        // ToDo apply to category selector

        if (category == Categories.Items) 
        {
            itemCategories.Add(item);
            Helper.AddStringToPlayerPref(ITEM_CATEGORIES_PLAYERPREFS, PLAYERPREFS_STRING_SEPARATOR, item);
        }
        else if (category == Categories.Shop)
        {
            shopCategories.Add(item);
            Helper.AddStringToPlayerPref(SHOP_CATEGORIES_PLAYERPREFS, PLAYERPREFS_STRING_SEPARATOR, item);
        }
        else Debug.Log("Category wasn't found!");
    }
    public void RemoveCategory(Categories category, string item)
    {
        if (category == Categories.Items) itemCategories.Remove(item);
        if (category == Categories.Shop) shopCategories.Remove(item);
        else Debug.Log("Category wasn't found!");
    }
    public void LoadSavedCategories()
    {
        //load categories from player prefs
        string prefItems = PlayerPrefs.GetString(ITEM_CATEGORIES_PLAYERPREFS);
        string prefShops = PlayerPrefs.GetString(SHOP_CATEGORIES_PLAYERPREFS);
        // if player prefs are empty feed predefined categories (force coded in)
        if (prefItems == null || prefItems.Length <= 1) prefItems = LoadDefaulCategories(Categories.Items);
        if (prefShops == null || prefShops.Length <= 1) prefShops = LoadDefaulCategories(Categories.Shop);
        // separate categoris into array of strings
        string[] itemCategories = prefItems.Split(PLAYERPREFS_STRING_SEPARATOR);
        string[] shopCategories = prefShops.Split(PLAYERPREFS_STRING_SEPARATOR);
        // feed categoris to stored list
        InitiateCategories(Categories.Items, itemCategories);
        InitiateCategories(Categories.Shop, shopCategories);
    }
    private void InitiateCategories(Categories category, string[] categories)
    {
        foreach( string item in categories)
        {
            if (item.Length > 0)
            AddCategory(category, item);
        }
    }
    private string LoadDefaulCategories(Categories category)
    {
        char s = PLAYERPREFS_STRING_SEPARATOR;
        if (category == Categories.Items)
        {
            return "None" + s + "Food" + s + "Cloths" + s + "Hobby";
        }
        if (category == Categories.Shop)
        {
            return "None" + s + "Groceries" + s + "Clothing" + s + "HomeDepo";
        }
        else return null;
    }



    // move to proper spot
    public void CategorySettingsShop()
    {
        CategorySettingInitiate(Categories.Shop);
    }
    public void CategorySettingsItem()
    {
        CategorySettingInitiate(Categories.Items);
    }
    public void CategorySettingInitiate(Categories category)
    {
        activeCategoryEdit = category;
        Transform scrollableComponent = panelManager.GetPanel(CategoryManager.ScrollableComponent).transform;

        Helper.CleanScrollableComponent(scrollableComponent, CategoryManager.ButtonAddNewCategory.ToString());

        List<string> categories;
        if (category == Categories.Items) categories = itemCategories;
        else if  (category == Categories.Shop) categories = shopCategories;
        else categories = new List<string>();
        foreach (string item in categories)
        {
            InitiateCategory(item, scrollableComponent);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollableComponent.GetComponent<RectTransform>());
    }
    private GameObject InitiateCategory(string name, Transform parent)
    {
        GameObject category = GameObject.Instantiate(panelManager.GetPanel(Prefabs.CategoryPrefab), parent);

        category.name = name;
        category.transform.Find(CategoryPrefab.CategoryName.ToString()).GetComponent<Text>().text = name;

        category.transform.Find(CategoryPrefab.DeleteCategoryButton.ToString()).GetComponent<Button>().onClick.AddListener(() => DeleteCategory(name, activeCategoryEdit, category));
        category.transform.Find(CategoryPrefab.EditCategoryButton.ToString()).GetComponent<Button>().onClick.AddListener(() => EditCategory(name));

        return category;
    }
    public void CreateCategory()
    {
        Transform creationPanel = panelManager.GetPanel(OtherPanels.CreateCategoryPanel).transform;
        Dropdown categoryDropdown = creationPanel.Find(CreateCategoryPanel.CategoryTypeDropdown.ToString()).GetComponent<Dropdown>();
        string categoryName = creationPanel.Find(CreateCategoryPanel.CategoryName.ToString()).GetComponent<InputField>().text;
        // get info from category creation window
        if (!string.IsNullOrEmpty(categoryName))
        {
            Categories categoryType = (Categories)System.Enum.Parse(typeof(Categories), categoryDropdown.options[categoryDropdown.value].text);
            if((categoryType == Categories.Shop && !shopCategories.Contains(categoryName)) ||
                categoryType == Categories.Items && !itemCategories.Contains(categoryName))
            {
                AddCategory(categoryType, categoryName);
                CategorySettingInitiate(categoryType);
            }
        }
        //Add cateogry command
        // close creation panel
        creationPanel.Find(CreateCategoryPanel.CategoryName.ToString()).GetComponent<InputField>().text = null;
        panelManager.GetPanel(OtherPanels.CreateCategoryPanel).SetActive(false);


    }
    private void CategoryAddPredefinedShop()
    {
        //ToDo
    }
    private void CategoryRemovePredefinedShop()
    {
        //ToDo
    }
    private void EditCategory(string categoryName)
    {
        Debug.Log("Edito this categorito " + categoryName);
    }
    private void DeleteCategory(string categoryName, Categories categoryType, GameObject thisCategory)
    {
        Debug.Log("Deleto this fdfokin categorano ej " + categoryName);
        // delete, add safety check in case it does not contain it
        if (PlayerPrefs.GetString(categoryName) != null) PlayerPrefs.DeleteKey(categoryName);

        if (categoryType == Categories.Items)
        {
            itemCategories.Remove(categoryName);
            Helper.RemoveStringToPlayerPref(ITEM_CATEGORIES_PLAYERPREFS, PLAYERPREFS_STRING_SEPARATOR, categoryName);
        }

        else if (categoryType == Categories.Shop)
        {
            shopCategories.Remove(categoryName);
            Helper.RemoveStringToPlayerPref(SHOP_CATEGORIES_PLAYERPREFS, PLAYERPREFS_STRING_SEPARATOR, categoryName);
        }

        GameObject.Destroy(thisCategory);
        // update selections, possibly remove catery from assigned items
        UpdateCategorySelectionWindow();
    }
    private void UpdateCategorySelectionWindow()
    {

    }

    public string GetDefaultPaht() { return DEFAULT_FILE_PATH; }
    public void ResetCategories()
    {
        ResetItemCategories();
        ResetShopCategories();
    }
    public void ResetItemCategories()
    {
        Debug.Log("Resetting item categories");
        PlayerPrefs.DeleteKey(ITEM_CATEGORIES_PLAYERPREFS);
        string categories = LoadDefaulCategories(Categories.Items);
        PlayerPrefs.SetString(ITEM_CATEGORIES_PLAYERPREFS, categories);
        itemCategories = categories.Split(PLAYERPREFS_STRING_SEPARATOR).ToList();

        if (panelManager.GetPanel(SettingsPanel.CategoryManager).gameObject.activeSelf) CategorySettingsItem();
    }
    public void ResetShopCategories()
    {
        Debug.Log("Resetting shop categories");
        PlayerPrefs.DeleteKey(SHOP_CATEGORIES_PLAYERPREFS);
        string categories = LoadDefaulCategories(Categories.Shop);
        PlayerPrefs.SetString(SHOP_CATEGORIES_PLAYERPREFS, categories);
        shopCategories = categories.Split(PLAYERPREFS_STRING_SEPARATOR).ToList();

        if (panelManager.GetPanel(SettingsPanel.CategoryManager).gameObject.activeSelf) CategorySettingsShop();
    }
    public void DeleteSaveFiles()
    {
        FileManager.DeleteAllFiles(GetDefaultPaht());

        Transform loadedShowcases = panelManager.GetPanel(DataList.ScrollableComponent).transform;
        foreach (Transform showcase  in loadedShowcases)
        {
            GameObject.Destroy(showcase.gameObject);
        }
        panelManager.CleanLoadedTickets();
        LoadSavedTickets();
    }
}
