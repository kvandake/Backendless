
namespace Backendless.Core
{
	public interface IBackendlessJsonConverter
	{

		T ReadObjectFromJson<T>(string json);

		T ReadBackendlessObjectFromJson<T>(string json) where T : BackendlessObject,new();

		string WriteBackendlessObjectToJson<T>(T obj) where T: BackendlessObject, new();

		string WriteObjectToJson(object obj);



	}
}

