using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using democsbackendService.DataObjects;
using democsbackendService.Models;
using Owin;

namespace democsbackendService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.IncludeErrorDetailPolicy =
                IncludeErrorDetailPolicy.Always;
            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            //config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            var migrator =
                new DbMigrator(new Migrations.Configuration());
            migrator.Update();
            
            
            
            app.UseWebApi(config);
        }
    }   
}

