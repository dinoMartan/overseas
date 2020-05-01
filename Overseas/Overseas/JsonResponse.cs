using Overseas;
using System;
using System.Collections.Generic;

namespace Overseas
{
    [Serializable]
    public class Sender
    {
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public object Address { get; set; }
        public object Name { get; set; }
        public object EmailAddress { get; set; }
        public object PhoneNumber { get; set; }
        public object GeoCoordY { get; set; }
        public object GeoCoordX { get; set; }
    }

    [Serializable]
    public class Consignee
    {
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public object Address { get; set; }
        public object Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public object GeoCoordY { get; set; }
        public object GeoCoordX { get; set; }
    }

    [Serializable]
    public class Errors
    {
    }

    [Serializable]
    public class LastShipmentTrace
    {
        public string ParcelNumber { get; set; }
        public DateTime ScanDateTime { get; set; }
        public string ScanDateString { get; set; }
        public string ScanTimeString { get; set; }
        public int StatusNumber { get; set; }
        public string StatusDescription { get; set; }
        public string CenterName { get; set; }
        public string Remark { get; set; }
        public Errors Errors { get; set; }
        public bool HasErrors { get; set; }
    }

    [Serializable]
    public class Message
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }

    [Serializable]
    public class Trace
    {
        public string ParcelNumber { get; set; }
        public DateTime ScanDateTime { get; set; }
        public string ScanDateString { get; set; }
        public string ScanTimeString { get; set; }
        public int StatusNumber { get; set; }
        public string StatusDescription { get; set; }
        public string CenterName { get; set; }
        public string Remark { get; set; }
        public bool HasErrors { get; set; }
    }

    [Serializable]
    public class Colly
    {
        public string CargonetColliId { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
        public string BarcodeCustomer { get; set; }
        public bool IgnoreStatusForShipment { get; set; }
        public IList<Trace> Traces { get; set; }
    }

    [Serializable]
    public class JsonResponse
    {
        public string CargonetCountryPrefix { get; set; }
        public string CargonetServerCode { get; set; }
        public string ShipmentNumber { get; set; }
        public object TrackingCode { get; set; }
        public DateTime SentOn { get; set; }
        public string SentOnDateString { get; set; }
        public string SentOnTimeString { get; set; }
        public Sender Sender { get; set; }
        public Consignee Consignee { get; set; }
        public string TnTRecieverName { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
        public string StatusDescription { get; set; }
        public object CanNotDeliverToPSDescription { get; set; }
        public object RadnoVrijeme { get; set; }
        public int PSID { get; set; }
        public bool CanChangeDelivery { get; set; }
        public bool HasChangesRemaining { get; set; }
        public bool DeliveryToRestrictCustomer { get; set; }
        public bool CanArrangeDeliveryDate { get; set; }
        public bool CanArrangeDeliveryToDoor { get; set; }
        public bool CanArrangeDeliveryToPS { get; set; }
        public bool CanContact { get; set; }
        public double CoD { get; set; }
        public LastShipmentTrace LastShipmentTrace { get; set; }
        public bool IsClaimed { get; set; }
        public bool CanClaim { get; set; }
        public IList<Message> Messages { get; set; }
        public int ClaimRetryCount { get; set; }
        public int ClaimMaxRetryCount { get; set; }
        public IList<Colly> Collies { get; set; }
        public object ExpectedDeliveryFrom { get; set; }
        public object ExpectedDeliveryTo { get; set; }
        public bool IsToParcelShop { get; set; }
        public bool IsFinalStatus { get; set; }
        public IList<object> Services { get; set; }
        public IList<object> ArrangeDeliveryRecords { get; set; }
        public object ExpectedDeliveryDateString { get; set; }
        public string ExpectedDeliveryIntervalString { get; set; }
        public bool HasErrors { get; set; }
    }


}
