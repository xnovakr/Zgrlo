using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace TicketObjects
{
    public class ReceiptShowcase
    {
        [SerializeReference]
        public string shopName;
            public string issueDate, price, ticketCategory;
    }
    public class ReceiptInfo//ints are in "" so they are probably saved as strings
    {
        public int returnValue;
        public Receipt receipt;
        public SearchIdentification searchIdentification;
    }
    public class Receipt
    {
        public string receiptId, issueDate, createDate, icDph, okp, pkp, type;
        public int ico, customerId, dic, invoiceNumber, paragonNumber, receiptNumber;//was null ceheck variable type with non null for invoiceNumber and paragonNumber
        public long cashRegisterCode;
        public bool paragon, exemption;
        public double taxBaseBasic, taxBaseReduced, totalPrice, freeTaxAmount, vatAmountBasic, vatAmountReduced, vatRateBasic, vatRateReduced;
        public Item[] items;
        public Organization organization;
        public Unit unit;
    }

    public class Item
    {
        public string name, itemType, itemCategory, itemUid;
        public double quantity, vatRate, price;
    }

    public class Organization
    {
        public string country, icDph, municipality, name, streetName;
        public int buildingNumber, dic, ico, postalCode, propertyRegistrationNumber;      ///////////////was null ceheck variable type with non null buildingNumber
        public bool vatPayer;
    }

    public class Unit
    {
        public string country, unitType, name, streetName, municipality, postalCode;
        public int propertyRegistrationNumber, buildingNumber;
        public long cashRegisterCode;
    }

    public class SearchIdentification
    {
        public string internalReceiptId, searchUuid;
        public int bucket;
        public long createDate;
    }
}
