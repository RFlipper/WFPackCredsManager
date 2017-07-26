using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;

namespace WFPackCredsManager
{
    public class SPClient
    {
        private static readonly Guid _wfFeatureId = new Guid("d7891031-e7f5-4734-8077-9189dd35551c");
        private static readonly string _wfProperty = "PlumsailActionsPackSettings";

        private ClientContext _context;

        public Dictionary<string, WFPackSettings> ActionsPackSites { get; set; } = new Dictionary<string, WFPackSettings>();
        public string DefaultLogin { get; set; }
        public string DefaultPassword { get; set; }
        public bool RootSiteCredsExists => !string.IsNullOrEmpty(DefaultLogin) && !string.IsNullOrEmpty(DefaultPassword);

        public SPClient(string url, string login, string password)
        {
            _context = new ClientContext(url);
            var pass = new SecureString();
            foreach (var c in password)
            {
                pass.AppendChar(c);
            }

            _context.Credentials = new SharePointOnlineCredentials(login, pass);
            
            InitDefaultCreds();

            CheckWFPackEnabledSites();
        }

        private void InitDefaultCreds()
        {
            _context.Load(_context.Web);
            _context.Load(_context.Web.AllProperties);
            _context.ExecuteQuery();

            var defaultSettings = GetWFPackSettings(_context.Web);
            if (defaultSettings == null) return;

            DefaultLogin = defaultSettings.DefaultLogin;
            DefaultPassword = defaultSettings.DefaultEncPassword;
        }

        private void CheckWFPackEnabledSites()
        {
            _context.Load(_context.Web.Webs, r => r.Include(w => w.Url, w => w.Features, w => w.AllProperties));
            _context.ExecuteQuery();
            
            foreach (var web in _context.Web.Webs.Where(w => w.Features.ToList()
                                    .Exists(f => f.DefinitionId == _wfFeatureId)))
            {
                var settings = GetWFPackSettings(web);
                
                ActionsPackSites.Add(web.Url, settings);
            }
        }

        public void UpdateCredsOnChildSites()
        {
            foreach (var web in _context.Web.Webs.Where(w =>
                               w.Features.ToList().Exists(f => f.DefinitionId == _wfFeatureId)))
            {
                var settings = GetWFPackSettings(web) ?? new WFPackSettings();

                settings.DefaultLogin = DefaultLogin;
                settings.DefaultEncPassword = DefaultPassword;

                web.AllProperties[_wfProperty] = JsonConvert.SerializeObject(settings);
                web.Update();
                _context.ExecuteQuery();

                Console.WriteLine($"Site {web.Url} workflow actions pack credentials were updated");
            }
        }

        private WFPackSettings GetWFPackSettings(Web web)
        {
            if (!web.AllProperties.FieldValues.ContainsKey(_wfProperty))
            {
                return null;
            }

            var property = web.AllProperties[_wfProperty] as string;
            var settings = JsonConvert.DeserializeObject<WFPackSettings>(property);

            return settings;
        }
    }
}
