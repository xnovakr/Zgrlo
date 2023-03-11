using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Helper
{
    /// <summary>
    /// Gets input field attached to game object and set its text value to null.
    /// </summary>
    /// <param name="inputField">GameObject containg InputField to be cleared.</param>
    public static void ClearInputField(GameObject inputField) { ClearInputField(inputField.GetComponent<InputField>()); }

    /// <summary>
    /// Set value of inputField to null.
    /// </summary>
    /// <param name="inputField">InputField to be cleared.</param>
    public static void ClearInputField(InputField inputField) { inputField.text = null; }
    /// <summary>
    /// Append one item into the array, if array is null return new array with item.
    /// </summary>
    /// <typeparam name="T">Type of array that is used.</typeparam>
    /// <param name="array">Array that you already have.</param>
    /// <param name="item">Item you want to add to array.</param>
    /// <returns></returns>
    public static T[] AppendArray<T>(this T[] array, T item)
    {
        if (array == null) return new T[] { item };

        T[] result = new T[array.Length + 1];
        array.CopyTo(result, 0);
        result[array.Length] = item;
        return result;
    }
    /// <summary>
    /// Inverts array.
    /// </summary>
    /// <typeparam name="T">Type of array.</typeparam>
    /// <param name="array">Array to invert.</param>
    /// <returns></returns>
    public static T[] InvertArray<T>(this T[] array)
    {
        if (array == null || array.Length < 2) return null;
        T[] result = new T[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            result[array.Length - i - 1] = array[i];
        }
        return result;
    }
    public static T[] MergeArrays<T>(this T[] firstArray, T[] secondArray)
    {
        var combinedArray = new T[firstArray.Length + secondArray.Length];
        
        Array.Copy(firstArray, combinedArray, firstArray.Length);
        Array.Copy(secondArray, 0, combinedArray, firstArray.Length, secondArray.Length);

        return combinedArray;
    }
    //change separator to load from global class, maybe create new file for separators and misc
    public static void AddStringToPlayerPref(string playerPrefName, char separator, string item)
    {
        string savedCategories = PlayerPrefs.GetString(playerPrefName);
        if (!savedCategories.Contains(item))
        {
            savedCategories += separator + item;
            PlayerPrefs.SetString(playerPrefName, savedCategories);
        }
    }
    //possibly buggy
    public static void RemoveStringToPlayerPref(string playerPrefName, char separator, string item)
    {
        string savedCategories = PlayerPrefs.GetString(playerPrefName);

        if (!savedCategories.Contains(item)) return;

        string[] categories = savedCategories.Split(separator);
        savedCategories = "";
        foreach (string category in categories)
        {
            if (category != item) savedCategories += category + separator;
        }
        if (savedCategories.EndsWith(separator.ToString())) savedCategories = savedCategories.Remove(savedCategories.Length - 1);
        PlayerPrefs.SetString(playerPrefName, savedCategories);
    }
    public static void SetupDropdown(GameObject dropdown, List<string> options)
    {
        List<Dropdown.OptionData> optionsData = new List<Dropdown.OptionData>();
        Dropdown dropdownComponent = dropdown.GetComponent<Dropdown>();

        dropdownComponent.ClearOptions();
        foreach(string option in options)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = option;
            optionsData.Add(optionData);
        }

        foreach(Dropdown.OptionData optionData in optionsData)
        {
            dropdownComponent.options.Add(optionData);
        }
    }

    public static void CleanScrollableComponent(Transform scrollableComponent, params string[] exceptions)
    {
        foreach(Transform child in scrollableComponent)
        {
            if (IsInArray(child.gameObject.name, exceptions)) continue;
            GameObject.Destroy(child.gameObject);
        }
    }
    public static bool IsInArray<T>(T item, T[] array)
    {
        foreach(T arrayItem in array)
        {
            if (arrayItem.ToString() == item.ToString()) return true;
        }
        return false;
    }
}