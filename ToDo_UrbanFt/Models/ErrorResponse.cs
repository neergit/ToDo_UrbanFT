using System.Net;
using Newtonsoft.Json;

namespace ToDo_UrbanFt.Models
{
    public class ErrorResponse
	{
		public int StatusCode { get; set; }
		public required string Message { get; set; }
		public required string Path { get; set; }
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}

