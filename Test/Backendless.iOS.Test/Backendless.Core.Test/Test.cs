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




		//[Test]
		public async void TestLoginCase ()
		{
			try {
				var result = await BackendlessUser.LoginAsync<CustomBackendlessUser>("test@test.ru", "testtest", error=>{
					Console.WriteLine (error.Message);
				});
				int f =0;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				throw ex;
			}
		}

		//[Test ()]
		public async void TestRegistrCase ()
		{
			var user = new CustomBackendlessUser ();
			user.Email = "test@test.ru";
			user.Username  = "test";
			user.AvatarSource = "fdfdf";
			try {
				var result = await user.SignUpAsync ("testtest", error=>{
					Console.WriteLine (error.Message);
				});
				int hs = 0;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				throw ex;
			}
		}

		//[Test]
		public async void TestUpdateCase(){
			var result = await BackendlessUser.LoginAsync<CustomBackendlessUser>("test@test.ru", "testtest", error=>{
				Console.WriteLine (error.Message);
			});
			result.AvatarSource = "change avatar";
			await result.UpdateAsync (error => {
				Console.WriteLine (error.Message);
			});
			int g = 0;
		}

		//[Test]
		public async void TestLogoutCase(){
			var result = await BackendlessUser.LoginAsync<CustomBackendlessUser>("test@test.ru", "testtest", error=>{
				Console.WriteLine (error.Message);
			});
			await result.LogoutAsync (error => {
				Console.WriteLine (error.Message);
			});
			int g = 0;
		}

		[Test]
		public async void TestRestorePasswordCase(){
			var result = await BackendlessUser.LoginAsync<CustomBackendlessUser>("test@test.ru", "testtest", error=>{
				Console.WriteLine (error.Message);
			});
			var res = await BackendlessUser.PasswordResetAsync (result.Email,error=>{
				Console.WriteLine (error.Message);
			});
			int g = 0;
		}
	}
}

