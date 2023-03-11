using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using Enums;

public class CameraManager : MonoBehaviour
{
    private WebCamTexture camTexture;
    public Material camMaterial;
    public PanelManager _panelManager;
    public ApplicationManager _applicationManager;
    public Transform cameraImage;


    private bool isSearching = true;

    private void Awake()
    {
        UpdateImagePanel();
        InitializeCameraReader();
    }
    private void LateUpdate()
    {
        if (isSearching) LoadQR(camTexture);
    }
    public void InitializeCameraReader()
    {
        SetupCameraTexture();
        if (camTexture != null)
        {
            camTexture.Play();
        }
        camMaterial.mainTexture = camTexture;
    }
    public void LoadQR(WebCamTexture camTexture)
    {
        // do the reading — you might want to attempt to read less often than you draw on the screen for performance sake
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
            if (result != null)
            {
                if (Array.Exists(_panelManager.GetLoadedTickets(), val => val == result.Text))// if ticket with this uid is already created then return
                {
                    Debug.Log("Ticket with this UID is already created in showcases.");
                    TicketExistNotification(_panelManager.GetLoadedTicketName(result.Text));
                    return;
                }
                InitializeLoadingTicket(result.Text);
            }
        }
        catch (Exception ex) { Debug.LogWarning($"Exception while laoding QR code: {ex.Message}"); }
    }
    public void StartSearching()
    {
        isSearching = true;
        StartCamera();
    }
    public void StopSearching()
    {
        isSearching = false;
        StopCamera();
    }
    private void StopCamera() { camTexture.Stop(); }
    private void StartCamera() { camTexture.Play(); }
    private void SetupCameraTexture()
    {
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = //Screen.height;
        camTexture.requestedWidth = Screen.width;
    }
    /// <summary>
    /// Function that create ticket from uid and restart camera afterwards
    /// </summary>
    /// <param name="ticketUID">UID for ticket, loaded from camera</param>
    public void CreateTicket(string ticketUID)
    {
        InitializeLoadingTicket(ticketUID, true);
    }
    public void InitializeLoadingTicket(string uid, bool restartCamera = false)
    {
        Ticket currTicket = new Ticket();
        currTicket.Initiate(uid);

        if (restartCamera) _panelManager.GetPanel(CameraReader.CameraScreen).GetComponent<CameraManager>().StartSearching();
        if (currTicket == null) return;
        TicketSaver.SaveTicket(_applicationManager.GetDefaultPaht(), currTicket);
        TicketCreatedNotification(currTicket.receiptShowcase.shopName);
    }
    private void UpdateImagePanel()
    {
        //probbably need to figure out better solution but for now good enough
        if (Application.platform == RuntimePlatform.Android)
        {
            transform.rotation *= Quaternion.AngleAxis(-90, Vector3.forward);
            transform.localScale = new Vector3(2,(float)0.5,1);
        }
    }
    private void TicketCreatedNotification(string shopName)
    {
        string notificationText = "Ticket was created:";
        _panelManager.GetPanel(CameraReader.TicketNotification).GetComponent<Notification>().Initiate(shopName, notificationText);
    }
    private void TicketExistNotification(string shopName)
    {
        string notificationText = "Ticket is already saved:";
        _panelManager.GetPanel(CameraReader.TicketNotification).GetComponent<Notification>().Initiate(shopName, notificationText);
    }
}
