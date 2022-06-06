using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using democsbackendService.DataObjects;
using democsbackendService.Models;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.NotificationHubs;

namespace democsbackendService.Controllers
{
    public class ContactController : TableController<Contact>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            democsbackendContext context = new democsbackendContext();
            DomainManager = new EntityDomainManager<Contact>(context, Request);
        }

        // GET tables/Contact
        public IQueryable<Contact> GetAllContact()
        {
            return Query();
        }

        // GET tables/Contact/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Contact> GetContact(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Contact/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Contact> PatchContact(string id, Delta<Contact> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Contact
        [Authorize]
        public async Task<IHttpActionResult> PostContact(Contact item)
        {
            Contact current = await InsertAsync(item);

            // Get the settings for the server project.
            HttpConfiguration config = this.Configuration;
            MobileAppSettingsDictionary settings =
                this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            // Get the Notification Hubs credentials for the Mobile App.
            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings
                .Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            // Create a new Notification Hub client.
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

            // Sending the message so that all template registrations that contain "messageParam"
            // will receive the notifications. This includes APNS, GCM, WNS, and MPNS template registrations.
            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = item.Name + " es un nuevo contacto";

            try
            {
                // Send the push notification and log the results.
                var result = await hub.SendTemplateNotificationAsync(templateParams);

                // Write the success result to the logs.
                config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (System.Exception ex)
            {
                // Write the failure result to the logs.
                config.Services.GetTraceWriter()
                    .Error(ex.Message, null, "Push.SendAsync Error");
            }


            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Contact/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteContact(string id)
        {
            return DeleteAsync(id);
        }
    }
}
