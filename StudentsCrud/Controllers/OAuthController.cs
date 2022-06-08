using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Smartsheet.Api.Models;
using Smartsheet.Api.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsCrud.Controllers
{
	[Route("oauth")]
	public class OAuthController : ControllerBase
	{
        IConfiguration _iconfiguration;
        public OAuthController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
     
        // GET oauth
        [HttpGet]
        public RedirectResult SignIn()
        {
            OAuthFlow oauth = new OAuthFlowBuilder()
                .SetTokenURL("https://api.smartsheet.com/2.0/token")
                .SetAuthorizationURL("https://www.smartsheet.com/b/authorize")
                .SetClientId(_iconfiguration["SmartsheetClientId"])
                .SetClientSecret(_iconfiguration["SmartsheetClientSecret"])
                .SetRedirectURL("https://localhost:44398/oauth/signin")
                .Build();
            string url = oauth.NewAuthorizationURL
            (
                new Smartsheet.Api.OAuth.AccessScope[]
                {
                Smartsheet.Api.OAuth.AccessScope.READ_SHEETS,
                Smartsheet.Api.OAuth.AccessScope.WRITE_SHEETS,
                Smartsheet.Api.OAuth.AccessScope.SHARE_SHEETS,
                Smartsheet.Api.OAuth.AccessScope.DELETE_SHEETS,
                Smartsheet.Api.OAuth.AccessScope.CREATE_SHEETS,
                Smartsheet.Api.OAuth.AccessScope.READ_USERS,
                Smartsheet.Api.OAuth.AccessScope.ADMIN_USERS,
                Smartsheet.Api.OAuth.AccessScope.ADMIN_SHEETS,
                Smartsheet.Api.OAuth.AccessScope.ADMIN_WORKSPACES,
                },
                "key=Test"
            );
            return RedirectPermanent(url);
        }

        // GET oauth/signin
        [HttpGet("oauth/signin")]
        public string SignInCallback(string code, int expires_in, string state)
        {
            OAuthFlow oauth = new OAuthFlowBuilder()
                .SetTokenURL("https://api.smartsheet.com/2.0/token")
                .SetAuthorizationURL("https://www.smartsheet.com/b/authorize")
                .SetClientId(_iconfiguration["SmartsheetClientId"])
                .SetClientSecret(_iconfiguration["SmartsheetClientSecret"])
                .SetRedirectURL("https://localhost:44398/oauth/signin")
                .Build();

            AuthorizationResult authResult = oauth.ExtractAuthorizationResult("https://localhost:44398/oauth/signin" + Request.QueryString.ToString());
            Token token = oauth.ObtainNewToken(authResult);

            return token.AccessToken;
        }
    }
}
