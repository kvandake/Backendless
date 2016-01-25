using NUnit.Framework;
using System;
using System.Configuration;
using Backendless.Core.Test;
using System.Reflection;
using System.Runtime.InteropServices;
using Backendless.Core;


namespace Backendless.Core.Test
{
	[TestFixture ()]
	public class Test
	{

		[SetUp]
		public void Setup(){
			BackendlessBootstrap.Init (new TestBackendlessPlatform (), TestConstant.LocalApplicationId, TestConstant.LocalSecretKey, TestConstant.LocalApiVersion, TestConstant.LocalBaseAddress);
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

		//[Test]
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

