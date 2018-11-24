using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MyJuncAPI.Test
{
	public class FetchEventsTest
	{
		private readonly ITestOutputHelper output;
		public FetchEventsTest(ITestOutputHelper outputter)
		{
			output = outputter;
		}

		[Fact]
		public void LoadEvents()
		{
			string jsonEvents = File.ReadAllText("C:/temp/open-api-events.json");
			List<dynamic> eventsList = JsonConvert.DeserializeObject<List<dynamic>>(jsonEvents);
			// starting_day
			// ending_day

			var dateEvents = eventsList.Where(e => {
				var dateString = e.event_dates?.starting_day.ToString();
				if (!string.IsNullOrEmpty(dateString))
				{
					var actualDate = DateTime.Parse(dateString);
					return actualDate == DateTime.Parse("2018-12-12T15:30:00.000Z");
				}
				return false;
			}).ToList();
		}
	}
}
