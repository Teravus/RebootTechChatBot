using RebootTechBotLib;
using RebootTechBotLib.Config;
using RebootTechBotLib.Infrastructure;
using RebootTechBotLib.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RebootTechBot
{
    public class OAuthRequestHandler
    {
        private string m_Client_Secret = string.Empty;
        private string m_Client_Id = string.Empty;
        private string m_RedirectURI = string.Empty;
        private string m_scope = string.Empty;
        private BotConfig m_Config = null;

        public delegate void OAuthCallback(object o, OAuthSuccessEventArgs result);

        public event OAuthCallback OnOAuthSuccess;

        public void RegisterHandler(string pclientsecret, BotConfig config)
        {
            m_Client_Secret = pclientsecret;
            m_Config = config;
            MainServer.Instance.AddHTTPHandler("/OAuthCallback", new GenericHTTPMethod(OAuthCallbackHandler));
            MainServer.Instance.AddHTTPHandler("/OAuthInitiationHandler", new GenericHTTPMethod(OAuthInitiationHandler));
        }
        private Hashtable OAuthCallbackHandler(Hashtable input)
        {

            string code = input["code"].ToString();
            string scope = input["scope"].ToString();
            string poststring = string.Format("client_id={0}&client_secret={1}&code={2}&grant_type=authorization_code&redirect_uri={3}", m_Client_Id, m_Client_Secret, code, HttpUtility.UrlEncode(m_RedirectURI));
            BasicAuthenticatedHttpRequestResponseParams parms = new BasicAuthenticatedHttpRequestResponseParams()
            {
                HTTP_Method = "POST",
                ContentType = "application/x-www-form-urlencoded",
                Data = poststring,
                URL = "https://id.twitch.tv/oauth2/token"
                 
            };
            var result = Utilities.AuthenticatedURLRequest(parms);

            if (result.ResponseCode == 200)
            {
                string responsestring = result.ResponseString;
                TwitchOAuthCodePostResponse resp = 
                    Newtonsoft.Json.JsonConvert.DeserializeObject<TwitchOAuthCodePostResponse>(responsestring);
                m_Config.credentials.TwitchOAuth = resp.access_token;
                m_Config.credentials.TwitchRefreshToken = resp.refresh_token;
                m_Config.credentials.TwitchClientID = m_Client_Id;
                m_Config.credentials.ClientSecret = m_Client_Secret;

                var evtdel = OnOAuthSuccess;
                if (evtdel != null)
                {
                    evtdel(this, new OAuthSuccessEventArgs() { Config = m_Config });
                }

            }
            else
            {
                int randomvar = 0;

            }

            Hashtable response = new Hashtable();
            response["int_response_code"] = 200;
            response["str_response_string"] = "<html><head><title>Congratulations!</title></head><body><H1>monkaS</H1><p>You Connected your bot.</p></body></html>";
            response["content_type"] = "text/html";

            return response;
        }
        private Hashtable OAuthInitiationHandler(Hashtable input)
        {
            m_Client_Id = input["ClientId"].ToString();
            m_RedirectURI = input["redirecturi"].ToString();
            m_scope = input["scope"].ToString();

            Hashtable response = new Hashtable();
            response["int_response_code"] = 302;
            response["redirect_location"] = string.Format("https://id.twitch.tv/oauth2/authorize?client_id={0}&redirect_uri={1}&response_type=code&scope={2}", 
                input["ClientId"], input["redirecturi"], input["scope"]);
            response["str_response_string"] = "";
            //response["content_type"] = "text/html";

            return response;
        }
    }
    public class TwitchOAuthCodePostResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string[] scope { get; set; }
        public string token_type { get; set; }

    }
    public class OAuthSuccessEventArgs: EventArgs
    {
        public BotConfig Config { get; set; }

    }
}
