namespace TesterLandisGyrProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {

                #region variables
                EndpointsManager manager = new EndpointsManager(new LandisGyrProject.ServiceLog() { });
                Endpoint endpoint = new Endpoint() { endpointSerialNumber = "ABCD", meterFirmwareVersion = "X3", meterModelId = LandisGyrProject.Enum.MeterModelIds.NSX3P4W, meterNumber = 1, switchState = LandisGyrProject.Enum.States.Disconnected };
                #endregion
                #region actions
                DefaultResult<bool> firstEndpoint = manager.Insert(endpoint);
                DefaultResult<bool> secondEndpoint = manager.Insert(endpoint);
                DefaultResult<bool> firstEdit = manager.Edit(endpoint, LandisGyrProject.Enum.States.Armed);
                DefaultResult<Endpoint> firstSearch = manager.FindBySerialNumber(endpoint.endpointSerialNumber);
                DefaultResult<Endpoint> secondSearch = manager.FindBySerialNumber("A");
                #endregion
                #region validations
                Assert.IsTrue(firstEndpoint.content);
                Assert.IsFalse(secondEndpoint.content);
                Assert.IsTrue(firstEdit.content);
                Assert.IsNotNull(firstSearch);
                Assert.IsNull(secondSearch);
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}