using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyJuncAPI.Models;
using MyJuncAPI.Services;
using Newtonsoft.Json.Linq;

namespace MyJuncAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Events")]
    public class EventsController : Controller
    {
		private readonly QueryService _queryService;

		public EventsController(QueryService queryService)
		{
			_queryService = queryService;
		}

		[HttpGet()]
		public IActionResult GetEvents([FromQuery] string eventDateTime, [FromQuery] string lat, [FromQuery] string lon, [FromQuery] double range, string tags)
		{
			double? latitude = !string.IsNullOrEmpty(lat) ? Convert.ToDouble(lat) : default(double?);
			double? longitude = !string.IsNullOrEmpty(lon) ? Convert.ToDouble(lon) : default(double?);
			string[] tagsList = !string.IsNullOrEmpty(tags) ? tags.ToLower().Split(',') : null;
			var responseEvents = _queryService.QueryEvents(Startup.EventsList, eventDateTime, latitude, longitude, range);
			var res = responseEvents.Select(e =>
			{
				return new EventResponse()
				{
					name = !string.IsNullOrEmpty(e.name?.fi?.ToString()) ? e.name?.fi?.ToString() : e.name?.en?.ToString(),
					latitude = Convert.ToDouble(e.location.lat),
					longitude = Convert.ToDouble(e.location.lon),
					address = e.location?.address?.street_address?.ToString(),
					event_start = e.event_dates?.starting_day?.ToString(),
					event_end = e.event_dates?.ending_day?.ToString(),
					tags = e.tags.Select(t => t.name).ToList()
				};
			});

			if (tagsList != null && tagsList.Length > 0)
			{
				res = res.Where(e =>
				{
					var ts = e.tags;
					if (ts.Any(z => tagsList.Any(t => z.Contains(t))))
					{
						return true;
					}
					return false;
				});
			}

			return Ok(res);
		}
    }
}