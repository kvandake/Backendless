using System;
using NUnit.Framework;
using System.Text;

namespace Backendless.Core.Test
{
	[TestFixture ()]
	public class TestFile
	{
		[SetUp]
		public void Setup(){
			var platf = new TestBackendlessPlatform ();
			platf.GlobalHandler = (s, e) => {
				Console.WriteLine (e.Message);
			};
			BackendlessBootstrap.Init (platf, TestConstant.LocalApplicationId,TestConstant.LocalSecretKey, TestConstant.LocalApiVersion, TestConstant.LocalBaseAddress);
		}

		[Test]
		public async void UploadFileCase(){
			//string temp = "Hello world";
			string temp = "SGVsbG8gd29ybGQ=";
			var array = Encoding.UTF8.GetBytes (temp);
			BackendlessFile.UploadFile ("temp/test.txt",array,error=>{
				int f=0;
			});
		}
	}
}

