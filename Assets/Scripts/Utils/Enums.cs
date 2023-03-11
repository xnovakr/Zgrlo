using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums
{ // list of mostly panels used in application enum == panel name (mostly)
    /// <summary>
    /// Manager objects that are holding main app scripts, enum == object name
    /// </summary>
    public enum ManagerObjects
    {
        PanelManager,
        ApplicationManager
    }

    /// <summary>
    ///  Parent objects for structuring application.
    /// </summary>
    public enum MainPanels
    {
        CameraReader,
        DataPanel,
        SettingsPanel,
        BasicStructure,

        TicketInfo, // move to proper enum
        CreateCategoryPanel, // move this to proper enum
        CategoryPicker// move this to proper enum
    }

    /// <summary>
    /// Panels of parent object CameraReader.
    /// </summary>
    public enum CameraReader //rename to cameraReader but needs to be rewired first
    {
        CameraScreen,
        TicketNotification
    }
    /// <summary>
    /// Components of ticketNotification object under main cameraReader object
    /// </summary>
    public enum TicketNotification
    {
        Background,
        NotificationText,
        NotificationDetail
    }

    /// <summary>
    /// Panels of parent object dataPanel.
    /// </summary>
    public enum DataPanel
    {
        DataHeader,
        DataList
    }
    /// <summary>
    /// Panels of dataHeader under main DataPanel object
    /// </summary>
    public enum DataHeader
    {
        HeaderText,
        AddTicketsButton
    }
    /// <summary>
    /// Panels of dataList under main DataPanel object
    /// </summary>
    public enum DataList
    {
        ScrollableComponent,
        Scrollbar
    }

    /// <summary>
    /// Panles of parent object settingsPanel
    /// </summary>
    public enum SettingsPanel
    {
        SettingsHeader,
        SettingsBody,
        CategoryManager
    }
    /// <summary>
    /// Panels of settingsHeader under main settingsPanel object
    /// </summary>
    public enum SettingsHeader
    {
        HeaderText
    }
    /// <summary>
    /// Panels of settingsBody under main settingsPanel object
    /// </summary>
    public enum SettingsBody
    {
        ManageCategoriesButton,
        DeleteTicketsButton
    }
    /// <summary>
    /// Panels (and thier children) of settingsManager under main settingsPanel object
    /// </summary>
    public enum CategoryManager
    {
        CategoryHeader,
        HeaderText,
        HeaderCloseButton,

        CategorySwappingButtons,
        ButtonShopCategories,
        ButtonItemCategories,

        CategoryResetButtons,
        ButtonResetShopCategories,
        ButtonResetItemCategories,

        DataList,
        ScrollableComponent,
        ButtonAddNewCategory
    }

    //ToDo Basic structure after rework in gui

    /// <summary>
    /// Orher panles without main category object
    /// </summary>
    public enum OtherPanels
    {
        CreateCategoryPanel,
        TicketInfo,
        CategoryPicker
    }

    /// <summary>
    /// Panles of helper CreateCategoryPanel object that is placed under canvas
    /// </summary>
    public enum CreateCategoryPanel
    {
        BackgroundPanel,
        CategoryTypeText,
        CategoryTypeDropdown,
        CategoryNameText,
        CategoryName,
        ConfirmationButton
    }

    /// <summary>
    /// Panles of helper TicketInfo object that is placed under canvas
    /// </summary>
    public enum TicketInfo
    {
        Header,
        Body
    }
    /// <summary>
    /// Panles of header object that is child object of ticketInfo
    /// </summary>
    public enum TicketInfoHeader
    {
        ButtonClose,
        TextHeader
    }
    /// <summary>
    /// Panles of body object that is child object of ticketInfo
    /// </summary>
    public enum TicketInfoBody
    {
        TicketDetails,
        Scrollbar
    }
    /// <summary>
    /// Panles of body -> ticketDetails object that is child object of ticketInfo
    /// </summary>
    public enum TicketInfoDetails
    {
        TicketSellerInfo,
        SellerName,
        SellerAdress,

        TicketPrice,
        Price,

        TicketDate,
        Date,

        TicketItems,

        TicketOtherInfo,
        PointOfSale,
        DIC,
        ICDPH,
        ICO,
        CashierNumber,
        SerialNumber,
        UID,
        OKP,
    }

    /// <summary>
    /// Panles of helper CategoryPicker object that is placed under canvas
    /// </summary>
    public enum CategoryPicker
    {
        DarkOverlay,
        Background,
        TextName,
        DataList,
        ButtonClose,

        ScrollableComponent
    }


    // PREFABS SECTION


    /// <summary>
    /// List of prefab objects, enum == prefab object name
    /// </summary>
    public enum Prefabs
    {
        TicketPrefab,
        ItemPrefab,
        CategoryPrefab,
        CategoryBlockPrefab
    }

    /// <summary>
    /// Components of ticketPrefab object
    /// </summary>
    public enum TicketPrefab
    {
        TicketPrefabName,
        TicketPrefabDelete,
        TicketPrefabTime,
        TicketPrefabPrice,
        TicketPrefabTextCategory,
        TicketPrefabDetailedInfoButton,
        TicketPrefabCategory,

        CategoryText // child object of TicketPrefabCategory object
    }

    /// <summary>
    /// Components of itemPrefab object
    /// </summary>
    public enum ItemPrefab
    {
        Background,

        // components to fill based on your values
        ItemName,
        ItemQuantity,
        ItemVatRate,
        ItemPrice,
        ItemCategorySelection,

        CategoryText, //placed under ItemCategorySelection

        // text components that are placed infront of your values
        TextItemQuantity,
        TextItemVatRate,
        TextItemPrice,
        TextItemCategory
    }

    /// <summary>
    /// Components of CategoryPrefab object, this will possibly be removed in future updates
    /// </summary>
    public enum CategoryPrefab
    {
        CategoryName,
        DeleteCategoryButton,
        EditCategoryButton
    }

    /// <summary>
    /// Components of CategoryBlockPrefab object
    /// </summary>
    public enum CategoryBlockPrefab
    {
        CategoryName,
        Icon
    }


    // OTHER HELPER ENUMS


    /// <summary>
    /// Animations used to move panels, enum == animation name
    /// </summary>
    public enum PanelAnimations
    {
        MidToLeftAnim,
        MidToLeftAnimReversed,
        MidToRightAnim,
        MidToRightAnimReversed
    }

    /// <summary>
    /// Types of possible sorting categories
    /// </summary>
    public enum Categories
    {
        Shop,
        Items
    }
}