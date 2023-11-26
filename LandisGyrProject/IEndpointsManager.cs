namespace LandisGyrProject
{
    interface IEndpointsManager
    {
        DefaultResult<bool> Insert(Endpoint endpoint);

        DefaultResult<bool> Edit(Endpoint endpoint, Enum.States newState);

        DefaultResult<bool> Delete(string serialNumber);

        DefaultResult<List<Endpoint>> ListAll();

        DefaultResult<Endpoint> FindBySerialNumber(string serialNumber);

        DefaultResult<bool> GetValuesForCreationEndpoint();

        DefaultResult<bool> EditingEndpoint();

        DefaultResult<bool> DeletingEndpoint();

        DefaultResult<List<Endpoint>> FindingEndpoints();

        DefaultResult<Endpoint> FindingEndpointBySerialNumber();
    }
}
