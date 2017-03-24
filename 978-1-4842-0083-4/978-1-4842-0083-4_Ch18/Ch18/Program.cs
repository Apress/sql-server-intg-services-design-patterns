using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.IntegrationServices;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Dts.Runtime;
using System.IO;

namespace MomDeployment
{
    class Program
    {
        const string ProjectFileLocation = @"C:\ETL\Project.ispac";

        static void Main(string[] args)
        {
            // Connect to the default instance on localhost
            var server = new Server("localhost");
            var store = new IntegrationServices(server);

            // Check that we have a catalog
            if (store.Catalogs.Count == 0)
            {
                Console.WriteLine("SSIS catalog not found on localhost.");
            }

            // Get the SSISDB catalog - note that there should only
            // be one, but the API may support multiple catalogs
            // in the future
            var catalog = store.Catalogs["SSISDB"];

            // Create a new folder
            var folder = new CatalogFolder(catalog, 
                                            "MyFolder", 
                                            "Folder that holds projects");
            folder.Create();

            // Make sure the project file exists
            if (!File.Exists(ProjectFileLocation))
            {
                Console.WriteLine("Project file not found at: {0}", 
                                    ProjectFileLocation);
            }

            // Load the project using the SSIS API
            var project = Project.OpenProject(ProjectFileLocation);

            // Deploy the project to the folder we just created
            folder.DeployProject(project);
        }
    }
}
