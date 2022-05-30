using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsCrud.Models
{
	public class AccessTokenData
	{
		public const string TokenUrl = "https://api.smartsheet.com/2.0/token";
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string Token { get; set; }
		public string AuthToken { get; set; }
		public string ServiceUrl { get; set; }
		public string Code { get; set; }
		public string RefreshToken { get; set; }
		public string GrantType { get; set; }
	}
}
