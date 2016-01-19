using System;


namespace Backendless.Core
{
	public interface IBackendlessJsonProvider : IBackendlessJsonConverter
	{

		void AddConverter (Type type, IBackendlessJsonConverter converter);

		void RemoveConverter(Type type);
	}
}

