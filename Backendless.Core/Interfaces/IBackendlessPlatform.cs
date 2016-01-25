namespace Backendless.Core
{
	public interface IBackendlessPlatform
	{
		IBackendlessRestEndPoint CreatorRestPoint { get; }

		IBackendlessCacheTableProvider CreatorDefaultCacheTableProvider {get;}

		IBackendlessConnectivity Connectivity {get;}

	}
}

