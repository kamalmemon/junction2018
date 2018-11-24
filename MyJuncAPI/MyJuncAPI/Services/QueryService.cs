using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using MyJuncAPI.Models;

namespace MyJuncAPI.Services
{
	public class QueryService
	{
		public IEnumerable<Event> QueryEvents(IEnumerable<Event> eventList, string dateString, double? latitude, double? longitude, double range = 1000)
		{
			var responseList = new List<Event>();
			responseList = QueryEventsByDate(eventList, dateString).ToList();
			if (latitude.HasValue && longitude.HasValue)
			{
				responseList = QueryEventsByLocation(responseList, latitude.Value, longitude.Value, range).ToList();
			}
			return responseList;
		}

		public IEnumerable<Event> QueryEventsByDate(IEnumerable<Event> eventList, string dateString)
		{
			var isSuccess = DateTime.TryParse(dateString, out var dateTime);
			if (!isSuccess)
			{
				dateTime = DateTime.Now;
			}
			return eventList.Where(e =>
			{
				var startDateString = e.event_dates?.starting_day?.ToString();
				var endDateString = e.event_dates?.ending_day?.ToString();
				var startDate = !string.IsNullOrEmpty(startDateString) ? DateTime.Parse(startDateString) : DateTime.Now;
				var endDate = !string.IsNullOrEmpty(endDateString) ? DateTime.Parse(endDateString) : DateTime.Now;

				if (dateTime >= startDate && dateTime <= endDate)
				{
					return true;
				}
				return false;
			}).AsEnumerable();
		}

		public IEnumerable<Event> QueryEventsByLocation(IEnumerable<Event> eventList, double latitude, double longitude, double range = 1000)
		{
			var coord = new GeoCoordinate(latitude, longitude);
			return eventList.Where(e => {
				var eLat = (double) e.location.lat;
				var eLong = (double)e.location.lon;
				var dist = new GeoCoordinate(eLat, eLong).GetDistanceTo(coord);
				if (dist < range)
				{
					return true;
				}
				return false;
			}).AsEnumerable();
		}
	}
}
