using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class FileManager
{
    private static string SAVING_TICKET_SUFFIX = ".zgrlo";
    public static bool CheckFile(string defaultPath, params string[] name)
    {
        return File.Exists(GetPath(defaultPath, true, name));
    }
    public static void CreateFile(string defaultPath, string content, params string[] name)
    {
        Debug.Log("Saving ticket file. File manager");
        if (CheckFile(defaultPath, name)) return; // file exists so return
        string filePath = GetPath(defaultPath, true, name);

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.Write(content);
        }
    }
    public static void UpdateFile(string defaultPath, string content, params string[] name)
    {
        //ToDo ticket Updater
        Debug.Log("Updating ticket file.");
        DeleteFile(defaultPath, name);
        CreateFile(defaultPath, content, name);
    }
    public static string GetFileContent(string defaultPath, params string[] name)
    {
        Debug.Log("Geting content of file.");
        if (!CheckFile(defaultPath, name)) throw new Exception("No file found while trying to get its content: " + name); // file desn't exists so return
        string filePath = GetPath(defaultPath, true, name);

        return File.ReadAllText(filePath);
    }
    public static void DeleteFile(string defaultPath, params string[] name)
    {
        if (!CheckFile(defaultPath, name)) return; // file desn't exists so return
        string filePath = GetPath(defaultPath, true, name);

        File.Delete(GetPath(defaultPath, true, name));
    }
    public static void DeleteAllFiles(string defaultPath)
    {
        Debug.Log("Deleting all saved files. " + defaultPath);
        string[] folders = Directory.GetDirectories(defaultPath);
        foreach (string folder in folders)
        {
            string currfolder = folder.Split(GetFileSeparator())[folder.Split(GetFileSeparator()).Length - 1];
            Debug.Log(currfolder);
            if (currfolder.StartsWith("20") && currfolder.Length == 4) DeleteFolder(defaultPath, currfolder);
        }

    }


    public static bool CheckFolder(string defaultPath, params string[] name)
    {
        Debug.Log("Checking if folder exists.");
        return Directory.Exists(GetPath(defaultPath, false, name));
    }
    public static void CreateFolder(string defaultPath, params string[] name)
    {
        if (CheckFolder(defaultPath, name)) return; // file exists so return

        Directory.CreateDirectory(GetPath(defaultPath, false, name));
    }
    public static string[] GetFolderContent(string defaultPath, params string[] name)
    {
        Debug.Log("Getting content of folder.");
        //if (name[0] == null) name[0] = ""; 
        if (!CheckFolder(defaultPath, name)) throw new Exception("Folder not found while trying to check its content: " + name.ToString());
        string folderPath;
        if (name[0] != null && name[0].StartsWith("C:")) folderPath = name[0];
        else folderPath = GetPath(defaultPath, false, name);
        //if (folderPath == null) folderPath = GetPath(false, name);
        string[] result = Directory.GetFiles(folderPath);
        result = Helper.MergeArrays(result, Directory.GetDirectories(folderPath));
        return result;
    }
    public static void DeleteFolder(string defaultPath, params string[] name)
    {
        if (!CheckFolder(defaultPath, name)) return; // directory doesn't exist so return

        Directory.Delete(GetPath(defaultPath, false, name), true);
    }
    public static bool FolderIsEmpty(string defaultPath, params string[] name)
    {
        string[] folderContent = GetFolderContent(defaultPath, name);

        foreach(string item in folderContent)
        {
            if (item != null) return false;
        }

        return true;
    }



    private static string GetPath(string defaultPath, bool file, params string[] name)
    {
        if (name[0] != null)
        {
            if (name.Length == 1 && name[0].StartsWith(defaultPath)) return name[0];
        }
        Debug.Log("Getting path for file.");
        string filePath = defaultPath;
        foreach (string s in name)
        {
            if (s == null) continue;
            filePath += GetFileSeparator() + s;
        }

        if (file)
        {
            filePath += SAVING_TICKET_SUFFIX;
        }
        Debug.Log("Returned path: " + filePath);
        return filePath;
    }
    

    public static char GetFileSeparator()
    {
        if (Application.platform == RuntimePlatform.Android) return '/';
        else return '\\';
    }
    public static string GetSavedTicketSuffix() { return SAVING_TICKET_SUFFIX; }
}