using System;

namespace Domain.Repository
{
    public class AddressJoin : System.IEquatable<AddressJoin>
    {
        public string Address { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public int? EparchyId { get; set; }
        public string PostalCode { get; set; }
        public string RegionName { get; set; }
        public string MyProperty { get; set; }
        public int StateId { get;  set; }
        public string EparchyName { get;  set; }

        public bool Equals(AddressJoin other)
        {
            if (this.CityName == other.CityName &&
                  this.Address == other.Address &&
                  this.CountryName == other.CountryName &&
                  this.EparchyId == other.EparchyId &&
                  this.PostalCode == other.PostalCode &&
                  this.RegionName == other.RegionName &&
                  this.StateId == other.StateId)
                return true;
            else
                return false;
        }
        public override int GetHashCode()
        {
            return (CityName == null ? 0 : CityName.GetHashCode()) ^
                (Address == null ? 0 : Address.GetHashCode()) ^
                (CountryName == null ? 0 : CountryName.GetHashCode()) ^
                (EparchyId == null ? 0 : EparchyId.GetHashCode()) ^
                (PostalCode == null ? 0 : PostalCode.GetHashCode()) ^
                (RegionName == null ? 0 : RegionName.GetHashCode()) ^
                (StateId.GetHashCode());

        }
    }
}