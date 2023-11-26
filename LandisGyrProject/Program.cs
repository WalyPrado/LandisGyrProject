using LandisGyrProject;

#region Variables
ServiceLog logs = new ServiceLog();
IEndpointsManager manager = new EndpointsManager(logs);
string option = string.Empty;
#endregion

do
{
    Console.Clear();
    Console.WriteLine(@$"Hello! Please, type the number's option that you want!
1) Insert a new endpoint.
2) Edit an existing endpoint.
3) Delete an existing endpoint.
4) List all endpoints.
5) Find an endpoint by a serial number.
6) Exit.");
    option = Console.ReadLine() ?? string.Empty;

    Console.Write("\n\n");
    switch (option)
    {
        case "1":
            Console.WriteLine(manager.GetValuesForCreationEndpoint().message);
            Console.WriteLine("Type any key to continue.");
            Console.ReadLine();
            break;
        case "2":
            Console.WriteLine(manager.EditingEndpoint().message);
            Console.WriteLine("Type any key to continue.");
            Console.ReadLine();
            break;
        case "3":
            Console.WriteLine(manager.DeletingEndpoint().message);
            Console.WriteLine("Type any key to continue.");
            Console.ReadLine();
            break;
        case "4":
            Console.WriteLine(manager.FindingEndpoints().message);
            Console.WriteLine("Type any key to continue.");
            Console.ReadLine();
            break;
        case "5":
            Console.WriteLine(manager.FindingEndpointBySerialNumber().message);
            Console.WriteLine("Type any key to continue.");
            Console.ReadLine();
            break;
        case "6":
            Console.WriteLine("Are you sure that you want to exit? (Y/N)");
            option = Console.ReadLine() ?? string.Empty;
            if (option.ToLower() == "y")
                return;
            break;
        default:
            Console.WriteLine("\nInvalid option!");
            Console.ReadLine();
            break;
    }
} while (option != "6");
