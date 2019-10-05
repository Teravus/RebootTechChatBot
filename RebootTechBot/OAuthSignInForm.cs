using RebootTechBotLib;
using RebootTechBotLib.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace RebootTechBot
{
    public partial class OAuthSignInForm : Form
    {
        public BotConfig botConfig { get; set; }
        public Bot bot { get; set; }
        public OAuthSignInForm()
        {
            InitializeComponent();
        }

        private void OAuthSignInForm_Load(object sender, EventArgs e)
        {
            //clbScopes.Items.Count
            for (int i=0;i<clbScopes.Items.Count;i++)
                clbScopes.SetItemChecked(i, true);

            //clbScopes
            oAuthBrowser.Navigate("http://localhost:" + botConfig.httpserver.Port.ToString() + "/oauthinfo.html");
        }

        private void btnStartOAuth_Click(object sender, EventArgs e)
        {
            var Handlers = new OAuthRequestHandler();
            Handlers.RegisterHandler(tbClientSecret.Text, botConfig);
            Handlers.OnOAuthSuccess += Handler_OnOAuthSuccess;

            string port = botConfig.httpserver.Port.ToString();
            string clientId = tbClientId.Text;
            string RedirectUri = string.Format("http://localhost:{0}/OAuthCallback", port);
            string scopes = string.Empty;
            
            

            foreach (var item in clbScopes.CheckedItems)
            {
                if (scopes.Length != 0)
                    scopes += " ";

                scopes += item.ToString();
            }
            scopes = HttpUtility.UrlEncode(scopes);
            RedirectUri = HttpUtility.UrlEncode(RedirectUri);

            oAuthBrowser.Navigate(string.Format("http://localhost:{0}/OAuthInitiationHandler?ClientId={1}&redirecturi={2}&scope={3}", port, clientId, RedirectUri, scopes));
        }
        private void Handler_OnOAuthSuccess(object o, OAuthSuccessEventArgs args)
        {
            bot.SaveConfig(args.Config);
        }
    }
}
