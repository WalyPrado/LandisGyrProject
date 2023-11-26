using System;
using System.Net;

namespace LandisGyrProject
{
    public class EndpointsManager : IEndpointsManager
    {
        #region Variables
        private readonly IServiceLog _serviceLog;
        private static List<Endpoint> endpoints { get; set; } = new();
        #endregion

        #region Initiator
        public EndpointsManager(IServiceLog serviceLog)
        {
            _serviceLog = serviceLog;
        }
        #endregion

        #region Methods
        public DefaultResult<bool> Insert(Endpoint endpoint)
        {
            try
            {
                if (endpoint is null)
                {
                    _serviceLog.Log("The endpoint informed is without content.");
                    return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "The endpoint informed is without content.", content = false };
                }

                if (endpoints.Any(x => x.endpointSerialNumber == endpoint.endpointSerialNumber))
                {
                    _serviceLog.Log(string.Format("Serial number {0} is already registered!", endpoint.endpointSerialNumber));
                    return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = string.Format("Serial number {0} is already registered!", endpoint.endpointSerialNumber), content = false };
                }

                _serviceLog.Log(string.Format("Endpoint saved with success! Serial {0}", endpoint.endpointSerialNumber));
                endpoints.Add(endpoint);

                return new() { statusCode = System.Net.HttpStatusCode.OK, message = string.Format("Endpoint saved with success! Serial {0}", endpoint.endpointSerialNumber), content = true };
            }
            catch (Exception exception)
            {
                _serviceLog.Log(string.Format("An error was found executing the Method Insert: {0} \nInformed Serial: {1}", exception.Message, endpoint.endpointSerialNumber));
                return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "An internal error occuried while your search was beeing processed. Please, try again later.", content = false };
            }
        }

        public DefaultResult<bool> Edit(Endpoint endpoint, Enum.States newState)
        {
            try
            {
                if (endpoint is null)
                {
                    _serviceLog.Log("Any endpoint was not found");
                    return new() { statusCode = System.Net.HttpStatusCode.NotFound, message = "Any endpoint was not found" };
                }

                if (endpoints.Any(x => x.endpointSerialNumber == endpoint.endpointSerialNumber))
                {
                    var index = endpoints.IndexOf(endpoint);

                    _serviceLog.Log(string.Format("The serial's endpoint {0} is now {1}!", endpoint.endpointSerialNumber, endpoint.switchState));
                    endpoints[index].switchState = newState;
                    return new() { statusCode = System.Net.HttpStatusCode.OK, message = string.Format("The serial's endpoint {0} is now {1}!", endpoint.endpointSerialNumber, endpoint.switchState), content = true };
                }
                else
                {
                    _serviceLog.Log(string.Format("The serial's endpoint {0} wasn't finded!", endpoint.endpointSerialNumber));
                    return new() { statusCode = System.Net.HttpStatusCode.NotFound, message = string.Format("None endpoint with serial number {0} was finded!", endpoint.endpointSerialNumber), content = false };
                }
            }
            catch (Exception exception)
            {
                _serviceLog.Log(string.Format("An error was found executing the Method Edit: {0} \nInformed Serial: {1} State: {2}", exception.Message, endpoint.endpointSerialNumber, newState));
                return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "An internal error occuried while your search was beeing processed. Please, try again later.", content = false };
            }
        }

        public DefaultResult<bool> Delete(string serialNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(serialNumber))
                {
                    _serviceLog.Log("Serial number not informed!");
                    return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "Serial number not informed!", content = false };
                }

                if (endpoints.Any(x => x.endpointSerialNumber == serialNumber))
                {
                    _serviceLog.Log(string.Format("The serial's endpoint {0} was excluded!", serialNumber));
                    endpoints.Remove(endpoints.Where(x => x.endpointSerialNumber == serialNumber).First());
                    return new() { statusCode = System.Net.HttpStatusCode.OK, message = string.Format("The serial's endpoint {0} was excluded!", serialNumber), content = true };
                }
                else
                {
                    _serviceLog.Log(string.Format("The serial's endpoint {0} wasn't finded!", serialNumber));
                    return new() { statusCode = System.Net.HttpStatusCode.NotFound, message = string.Format("The serial's endpoint {0} wasn't finded!", serialNumber), content = false };
                }
            }
            catch (Exception exception)
            {
                _serviceLog.Log(string.Format("An error was found executing the Method Delete: {0} \nInformed Serial: {1}", exception.Message, serialNumber));
                return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "An internal error occuried while your search was beeing processed. Please, try again later.", content = false };
            }
        }

        public DefaultResult<List<Endpoint>> ListAll()
        {
            try
            {
                if (endpoints is null || endpoints.Count < 1)
                {
                    _serviceLog.Log("None endpoint was found!");
                    return new() { statusCode = System.Net.HttpStatusCode.NotFound, message = "None endpoint was found!" };
                }

                string result = string.Format("A total of {0} endpoint(s) was found!", endpoints.Count);

                for (int i = 0; i < endpoints.Count; i++)
                {
                    result += $"\n\nENDPOINT {i + 1}\n" +
                                $"Endpoint Serial Number: {endpoints[i].endpointSerialNumber}\n" +
                                $"Meter Model Identifier: {endpoints[i].meterModelId}\n" +
                                $"Meter Number:           {endpoints[i].meterNumber}\n" +
                                $"Meter Firmware Version: {endpoints[i].meterFirmwareVersion}\n" +
                                $"Actual State:           {endpoints[i].switchState}\n";
                }

                _serviceLog.Log(string.Format("A total of {0} endpoints was found!", endpoints.Count));
                return new() { statusCode = System.Net.HttpStatusCode.OK, message = result, content = endpoints };
            }
            catch (Exception exception)
            {
                _serviceLog.Log(string.Format("An error was found executing the Method ListAll: {0}", exception.Message));
                return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "An internal error occuried while your search was beeing processed. Please, try again later." };
            }
        }

        public DefaultResult<Endpoint> FindBySerialNumber(string serialNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(serialNumber))
                {
                    _serviceLog.Log("Serial number not informed!");
                    return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "Serial number not informed!" };
                }

                if (endpoints.Any(x => x.endpointSerialNumber == serialNumber))
                {
                    Endpoint endpoint = endpoints.Where(x => x.endpointSerialNumber == serialNumber).First();

                    string result = string.Format("The endpoint {0} was finded!\n\n", serialNumber) +
                                $"Endpoint Serial Number: {endpoint.endpointSerialNumber}\n" +
                                $"Meter Model Identifier: {endpoint.meterModelId}\n" +
                                $"Meter Number:           {endpoint.meterNumber}\n" +
                                $"Meter Firmware Version: {endpoint.meterFirmwareVersion}\n" +
                                $"Actual State:           {endpoint.switchState}\n";

                    return new() { statusCode = System.Net.HttpStatusCode.OK, message = result, content = endpoints.Where(x => x.endpointSerialNumber == serialNumber).First() };
                }
                else
                {
                    _serviceLog.Log(string.Format("The endpoint {0} was not finded!", serialNumber));
                    return new() { statusCode = System.Net.HttpStatusCode.NotFound, message = string.Format("The serial's endpoint {0} wasn't finded!", serialNumber) };
                }
            }
            catch (Exception exception)
            {
                _serviceLog.Log(string.Format("An error was found executing the Method FindBySerialNumber: {0} \nInformed Serial: {1}", exception.Message, serialNumber));
                return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "An internal error occuried while your search was beeing processed. Please, try again later." };
            }
        }

        public DefaultResult<bool> GetValuesForCreationEndpoint()
        {
            Console.WriteLine("To cancel this action, type 'Cancel'.");
            try
            {
                Endpoint endpoint = new Endpoint();
                string value = string.Empty;
                DefaultResult<bool> result = new DefaultResult<bool>() { };
                #region Serial Number
                while (value.ToLower() != "cancel")
                {
                    Console.WriteLine("Please, enter a valid Serial Number:");
                    value = Console.ReadLine() ?? string.Empty;
                    if (value.ToLower() == "cancel") break;

                    if (string.IsNullOrEmpty(value))
                        Console.WriteLine("Invalid serial number!");
                    else if (endpoints.Any(x => x.endpointSerialNumber == value))
                        Console.WriteLine("Serial Number is already in use!");
                    else
                    {
                        endpoint.endpointSerialNumber = value;
                        break;
                    }
                }
                #endregion
                #region Meter Model Id
                while (value.ToLower() != "cancel")
                {
                    Console.WriteLine("Please, enter the code of a valid State:\n"
                                    + "Valid values: 16 - NSX1P2W, 17 - NSX1P3W, 18 - NSX2P3W, 19 - NSX3P4W");

                    value = Console.ReadLine() ?? string.Empty;
                    if (value.ToLower() == "cancel") break;

                    if (string.IsNullOrEmpty(value) || !new string[] { "16", "17", "18", "19" }.Contains(value))
                        Console.WriteLine("Invalid Meter Model Id!");
                    else
                    {
                        endpoint.meterModelId = (Enum.MeterModelIds)Convert.ToInt32(value);
                        break;
                    }
                }
                #endregion
                #region Meter Number
                while (value.ToLower() != "cancel")
                {
                    Console.WriteLine("Please, enter a valid Meter Number:");
                    value = Console.ReadLine() ?? string.Empty;
                    if (value.ToLower() == "cancel") break;

                    if (Int32.TryParse(value, out int intValue))
                    {
                        endpoint.meterNumber = intValue;
                        break;
                    }
                    else
                        Console.WriteLine("Invalid Meter Number!");
                }
                #endregion
                #region Meter Firmware Version
                while (value.ToLower() != "cancel")
                {
                    Console.WriteLine("Please, enter a valid Meter Firmware Version:");
                    value = Console.ReadLine() ?? string.Empty;
                    if (value.ToLower() == "cancel") break;

                    if (string.IsNullOrEmpty(value))
                        Console.WriteLine("Invalid Meter Firmware Version!");
                    else
                    {
                        endpoint.meterFirmwareVersion = value;
                        break;
                    }
                }
                #endregion
                #region Switch State
                while (value.ToLower() != "cancel")
                {
                    Console.WriteLine("Please, enter the code of a valid State:\n"
                                       + "Valid values: 0 - Disconnected, 1 - Connected, 2 - Armed");
                    value = Console.ReadLine() ?? string.Empty;
                    if (value.ToLower() == "cancel") break;

                    if (string.IsNullOrEmpty(value) || !new string[] { "0", "1", "2" }.Contains(value))
                        Console.WriteLine("Invalid Meter Firmware Version!");
                    else
                    {
                        endpoint.switchState = (Enum.States)Convert.ToInt32(value);
                        break;
                    }
                }
                #endregion

                if (value.ToLower() == "cancel")
                    return new() { statusCode = System.Net.HttpStatusCode.OK, content = false, message = "Action Canceled!" };

                return Insert(endpoint);
            }
            catch (Exception exception)
            {
                _serviceLog.Log(string.Format("An error was found executing the Method GetValuesForCreationEndpoint: {0}", exception.Message));
                return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "An internal error occuried while your search was beeing processed. Please, try again later.", content = false };
            }
        }

        public DefaultResult<bool> EditingEndpoint()
        {
            Console.WriteLine("To cancel this action, type 'Cancel'.");
            try
            {
                #region Variables
                Endpoint endpoint = new Endpoint();
                Enum.States newState = new Enum.States();
                string value = string.Empty;
                DefaultResult<Endpoint> result = new DefaultResult<Endpoint>() { };
                #endregion
                #region Serial Number
                while (value.ToLower() != "cancel")
                {
                    Console.WriteLine("Please, enter a valid Serial Number:");
                    value = Console.ReadLine() ?? string.Empty;
                    if (value.ToLower() == "cancel") break;

                    if (string.IsNullOrEmpty(value))
                        Console.WriteLine("Invalid serial number!");
                    else
                    {
                        result = FindBySerialNumber(value);
                        if (result.statusCode == System.Net.HttpStatusCode.OK)
                        {
                            endpoint = result.content;
                            break;
                        }
                        else
                            Console.WriteLine(result.message);
                    }
                }
                #endregion
                #region Switch State
                while (value.ToLower() != "cancel")
                {
                    Console.WriteLine("Please, enter the code of a valid State:\n"
                                       + "Valid values: 0 - Disconnected, 1 - Connected, 2 - Armed");
                    value = Console.ReadLine() ?? string.Empty;
                    if (value.ToLower() == "cancel") break;

                    if (string.IsNullOrEmpty(value) || !new string[] { "0", "1", "2" }.Contains(value))
                        Console.WriteLine("Invalid Meter Firmware Version!");
                    else
                    {
                        newState = (Enum.States)Convert.ToInt32(value);
                        break;
                    }
                }
                #endregion

                if (value.ToLower() == "cancel")
                    return new() { statusCode = System.Net.HttpStatusCode.OK, content = false, message = "Action Canceled!" };

                return Edit(endpoint, newState);
            }
            catch (Exception exception)
            {
                _serviceLog.Log(string.Format("An error was found executing the Method EditingEndpoint: {0}", exception.Message));
                return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "An internal error occuried while your search was beeing processed. Please, try again later.", content = false };
            }
        }

        public DefaultResult<bool> DeletingEndpoint()
        {
            Console.WriteLine("To cancel this action, type 'Cancel'.");
            try
            {
                #region Variables
                string value = string.Empty;
                DefaultResult<Endpoint> result = new DefaultResult<Endpoint>() { };
                #endregion
                #region Serial Number
                while (value.ToLower() != "cancel")
                {
                    Console.WriteLine("Please, enter a valid Serial Number:");
                    value = Console.ReadLine() ?? string.Empty;
                    if (value.ToLower() == "cancel") break;

                    if (string.IsNullOrEmpty(value))
                        Console.WriteLine("Invalid serial number!");
                    else
                    {
                        result = FindBySerialNumber(value);
                        if (result.statusCode == System.Net.HttpStatusCode.OK)
                        {
                            Console.WriteLine(string.Format("Are you sure that you want to delete the endpoint {0}? This action cannot be undone. (Type 'YES' to confirm)", result.content.endpointSerialNumber));
                            value = Console.ReadLine() ?? string.Empty;
                            if (value == "YES")
                            {
                                value = result.content.endpointSerialNumber;
                                break;
                            }
                            else
                                value = string.Empty;
                        }
                        else
                            Console.WriteLine(result.message);
                    }
                }
                #endregion

                if (value.ToLower() == "cancel")
                    return new() { statusCode = System.Net.HttpStatusCode.OK, content = false, message = "Action Canceled!" };

                return Delete(value);
            }
            catch (Exception exception)
            {
                _serviceLog.Log(string.Format("An error was found executing the Method DeletingEndpoint: {0}", exception.Message));
                return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "An internal error occuried while your search was beeing processed. Please, try again later.", content = false };
            }
        }

        public DefaultResult<List<Endpoint>> FindingEndpoints()
        {
            try
            {
                return ListAll();
            }
            catch (Exception exception)
            {
                _serviceLog.Log(string.Format("An error was found executing the Method FindingEndpoints: {0}", exception.Message));
                return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "An internal error occuried while your search was beeing processed. Please, try again later." };
            }
        }

        public DefaultResult<Endpoint> FindingEndpointBySerialNumber()
        {
            Console.WriteLine("To cancel this action, type 'Cancel'.");

            try
            {
                #region Variables
                string value = string.Empty;
                #endregion
                #region Serial Number
                while (value.ToLower() != "cancel")
                {
                    Console.WriteLine("Please, enter a valid Serial Number:");
                    value = Console.ReadLine() ?? string.Empty;
                    if (value.ToLower() == "cancel") break;

                    if (string.IsNullOrEmpty(value))
                        Console.WriteLine("Invalid serial number!");
                    else
                        break;
                }
                #endregion

                if (value.ToLower() == "cancel")
                    return new() { statusCode = System.Net.HttpStatusCode.OK, message = "Action Canceled!" };

                return FindBySerialNumber(value);
            }
            catch (Exception exception)
            {
                _serviceLog.Log(string.Format("An error was found executing the Method FindingEndpointBySerialNumber: {0}", exception.Message));
                return new() { statusCode = System.Net.HttpStatusCode.BadRequest, message = "An internal error occuried while your search was beeing processed. Please, try again later." };
            }
        }
        #endregion
    }
}
