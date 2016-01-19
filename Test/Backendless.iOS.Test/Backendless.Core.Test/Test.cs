using NUnit.Framework;
using System;
using System.Configuration;

namespace Backendless.Core.Test
{
	[TestFixture ()]
	public class Test
	{
		//local id
		readonly static string LocalApplicationId = "B76AD7FA-DBE9-4AB3-FF3D-550A432A9900";

		readonly static string LocalSecretKey = "92A8D8FE-FBCE-66C4-FFAF-535658BE3D00";

		readonly static string LocalApiVersion = "v1";

		readonly static string LocalBaseAddress = "http://192.168.1.165:8080";

//		readonly static string LocalBaseAddress = "http://birthdayslist.com";

		[SetUp]
		public void Setup(){
			TestBackendlessBootstrap.Init (LocalApplicationId, LocalSecretKey, LocalApiVersion, LocalBaseAddress);
			BackendlessHadlerException.InvokeError += (sender, e) => {
				Console.WriteLine (e.ToString ());
			};

		}




		[Test]
		public async void TestLoginCase ()
		{
			try {
				var result = await BackendlessBootstrap.FromContext<IUserService> ().LoginAsync ("test@test.ru", "testtest", new BackendlessCallback<BackendlessUser> (
					             user => {
						int h =0;
					},
					             error => {
						int g = 0;
					}));
				int hs = 0;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				throw ex;
			}
		}

		//[Test ()]
		public async void TestRegistrCase ()
		{
			//user.Email = ""
//			try {
//				var result = await BackendlessBootstrap.FromContext<IUserService> ().SignUpAsync ("test1@test.ru", "testtest", new BackendlessCallback<BackendlessUser> (
//					user => {
//						int h =0;
//					},
//					error => {
//						int g = 0;
//					}));
//				int hs = 0;
//			} catch (Exception ex) {
//				Console.WriteLine (ex);
//				throw ex;
//			}
		}
	}
}

