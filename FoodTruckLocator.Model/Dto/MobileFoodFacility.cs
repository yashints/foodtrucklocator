using System;

namespace FoodTruckLocator.Model
{
    public class MobileFoodFacility
    {
        public int LocationId { get; set; }
        public string Applicant { get; set; }
        public string FacilityType { get; set; }
        public int CNN { get; set; }
        public string LocationDescription { get; set; }
        public string Address { get; set; }
        public string BlockLot { get; set; }
        public string Block { get; set; }
        public string Lot { get; set; }
        public string Permit { get; set; }
        public string Status { get; set; }
        public string FoodItems { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Schedule { get; set; }
        public string DaysHours { get; set; }
        public string NOISent { get; set; }
        public string Approved { get; set; }
        public int Received { get; set; }
        public int PriorPermit { get; set; }
        public string ExpirationDate { get; set; }
        public string Location { get; set; }
        public int? FirePreventionDistricts { get; set; }
        public int? PoliceDistricts { get; set; }
        public int? SupervisorDistricts { get; set; }
        public int? ZipCodes { get; set; }
        public int? Neighborhoodsold { get; set; }
    }
}
