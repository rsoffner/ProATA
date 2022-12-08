using ApiManager.Models.Api;
using ApiManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiManager.Controllers.Api
{
    [ApiController]
    [AllowAnonymous]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpPost]
        [Route("api/log")]
        public ActionResult GetLog([FromForm] int period, [FromForm] int draw, [FromForm] IList<RequestOrderDto> order,
            [FromForm] RequestSearchDto search, [FromForm] int userId, [FromForm] Guid taskId)
        {
            //var searchString = search;
            string[] columnNames =
            {
                "timeStamp",
                "priorityName",
                "message",
                "source",
                "url"
            };

            IList<string> sortOrders = new List<string>();
            foreach (var orderItem in order)
            {
                sortOrders.Add(columnNames[orderItem.Column] + "." + orderItem.Dir);
            }

            var items = _logService.ListByPeriod(period, draw, sortOrders.First(), search.Value, userId, taskId);

            return Ok(items);
        }

        [HttpGet]
        [Route("api/log/{id}")]
        public ActionResult GetLogById(int id)
        {
            var logItem = _logService.GetLogItem(id);
            if (logItem == null)
            {
                return NotFound("LogItem with id: " + id + " not found");
            }
            else
            {
                return Ok(logItem);
            }
        }
    }
}
