using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using HospitalSimulator.Models;

namespace HospitalSimulator
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase());
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseStaticFiles();

            // Seed database on Startup
            Seed(app.ApplicationServices.GetService<ApplicationDbContext>());

            app.UseMvc();
        }

        // This method seeds the database based on a json file
        private static void Seed(ApplicationDbContext context)
        {
            JObject dbSeed = JObject.Parse(File.ReadAllText("dbSeed.json"));
            
            // Get the data to seed
            var roles = dbSeed.GetValue("roles").ToList().Select(r => r.ToString()).ToList();
            var doctors = dbSeed.GetValue("doctors").ToList();
            var machines = dbSeed.GetValue("treatmentMachines").ToList();
            var rooms = dbSeed.GetValue("treatmentRooms").ToList();

            // Add roles
            foreach (var roleName in roles)
            {                
                context.Roles.Add( new Role { Name = roleName });
            }

            // Add doctors
            foreach (var doctor in doctors)
            {
                var name = ((JObject) doctor).GetValue("name").ToString();
                var doctorRoles = ((JObject) doctor).GetValue("roles").ToList().Select(r => r.ToString());
                var newDoctor = new Doctor { Name = name };
                newDoctor.Roles = new List<DoctorRole>();
                foreach (var dr in doctorRoles)
                {
                    newDoctor.Roles.Add(new DoctorRole { RoleName = dr });
                }
                context.Doctors.Add(newDoctor);
            }

            // Add treatment machines
            foreach (var machine in machines)
            {
                var name = ((JObject) machine).GetValue("name").ToString();
                var capability = ((JObject) machine).GetValue("capability").ToString();
                context.TreatmentMachines.Add(new TreatmentMachine 
                {
                    Name = name,
                    Capability = capability
                });
            }

            // Add treatment rooms
            foreach (var room in rooms)
            {
                var name = ((JObject) room).GetValue("name").ToString();
                var machine = ((JObject) room).GetValue("treatmentMachine");
                var machineName = (machine == null) ? "" : machine.ToString();
                context.TreatmentRooms.Add(new TreatmentRoom 
                {
                    Name = name,
                    MachineName = machineName
                });
            }

            context.SaveChanges();
        }
    }
}
