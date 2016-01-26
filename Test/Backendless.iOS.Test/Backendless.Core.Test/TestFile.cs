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
			string temp = "Hello world";
			var array = Encoding.UTF8.GetBytes (temp);
			BackendlessFile.UploadFile ("/temp/test.txt",array,BackendlessConstant.TextContentTypeValue, error=>{
				int f=0;
			});
		}

		//[Test]
		public async void DownloadFileCase(){
			//string temp = "Hello world";
			var file = await BackendlessFile.DownloadFile ("/temp/test.txt", error=>{
				int f=0;
			});
			int h = 0;
		}
	}
}

