using System;
using System.Web.Security;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Security.Accounts;
using Sitecore.Security.Domains;

namespace Sitecore.Support.Commerce.Connect.CommerceServer.Profiles.Events
{
    public class CommerceUserEventHandler : Sitecore.Commerce.Connect.CommerceServer.Profiles.Events.
        CommerceUserEventHandler
    {
        public override void OnUserUpdated(object sender, EventArgs e)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(e, "args");
            MembershipUser user = Event.ExtractParameter(e, 0) as MembershipUser;
            if (user != null && User.Exists(user.UserName))
            {
                string[] commerceDomains = MainUtil.RemoveEmptyStrings(Sitecore.Configuration.Settings.GetSetting("CommerceDomains").ToLowerInvariant().Split(new char[] {'|'}));
                if (commerceDomains != null)
                {
                    foreach (string domain in commerceDomains)
                    {
                        if (Domain.GetAccountDomain(user.UserName).Name.ToLowerInvariant() == domain)
                        {
                            base.OnUserUpdated(sender, e);
                            return;
                        }
                    }
                }
            }
        }
    }
}