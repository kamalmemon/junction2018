using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyJuncAPI.Models
{
	public class Name
	{
		public string fi { get; set; }
		public object en { get; set; }
		public object sv { get; set; }
		public object zh { get; set; }
	}

	public class SourceType
	{
		public int id { get; set; }
		public string name { get; set; }
	}

	public class Address
	{
		public string street_address { get; set; }
		public string postal_code { get; set; }
		public string locality { get; set; }
	}

	public class Location
	{
		public double lat { get; set; }
		public double lon { get; set; }
		public Address address { get; set; }
	}

	public class LicenseType
	{
		public int id { get; set; }
		public string name { get; set; }
	}

	public class Image
	{
		public string url { get; set; }
		public string copyright_holder { get; set; }
		public LicenseType license_type { get; set; }
	}

	public class Description
	{
		public string intro { get; set; }
		public string body { get; set; }
		public List<Image> images { get; set; }
	}

	public class Tag
	{
		public string id { get; set; }
		public string name {
			get { return _name; }
			set { _name = value.ToLower(); }
		}
		private string _name = null;
	}

	public class EventDates
	{
		public DateTime starting_day { get; set; }
		public DateTime ending_day { get; set; }
		public object additional_description { get; set; }
	}

	public class Event
	{
		public string id { get; set; }
		public dynamic name { get; set; }
		public dynamic source_type { get; set; }
		public dynamic info_url { get; set; }
		public dynamic modified_at { get; set; }
		public Location location { get; set; }
		public dynamic description { get; set; }
		public List<Tag> tags { get; set; }
		public dynamic event_dates { get; set; }
	}

	public class EventResponse
	{
		public string name { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string address { get; set; }
		public string event_start { get; set; }
		public string event_end { get; set; }
		public List<string> tags { get; set; }
	}
}
