using System;
using Backendless.Core;
using System.Collections.Generic;

namespace Backendless.Core.Test
{
	public class TestBackendlessBootstrap : BackendlessBootstrap
	{

		IDictionary<Type,Type> endpoints;
		IDictionary<Type,object> services;

		protected override IDictionary<Type, object> Services {
			get {
				if (services == null) {
					services = new Dictionary<Type,object> ();
					services [typeof(IUserService)] = new SimpleUserService ();
					services [typeof(IBackendlessJsonProvider)] = new SimpleBackendlessJsonProvider ();
				}
				return services;
			}
		}

		protected override IDictionary<Type, Type> Endpoints {
			get {
				if (endpoints == null) {
					endpoints = new Dictionary<Type,Type> ();
					endpoints [typeof(IBackendlessLocalEndPoint)] = typeof(SimpleBackendlessLocalEndPoint);
					endpoints [typeof(IBackendlessRestEndPoint)] = typeof(SimpleBackendlessRestEndPoint);
				}
				return endpoints;
			}
		}


		public static void Init(string applicationId, string secretKey, string apiVersion,string baseUrl = null, IConfigBackendless config = null){
			var bootstrap = new TestBackendlessBootstrap (applicationId, secretKey,apiVersion,baseUrl);
			Init (bootstrap,config);
		}









		internal TestBackendlessBootstrap (string applicationId, string secretKey, string apiVersion,string baseUrl = null):base(applicationId,secretKey,apiVersion,baseUrl)
		{
		}
	}
}

