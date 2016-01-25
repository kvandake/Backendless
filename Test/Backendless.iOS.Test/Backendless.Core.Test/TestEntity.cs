using System;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace Backendless.Core.Test
{
	//[TestFixture ()]
	public class TestEntity
	{
		//[SetUp]
		public void Setup(){
			BackendlessBootstrap.Init (new TestBackendlessPlatform (), TestConstant.LocalApplicationId,TestConstant.LocalSecretKey, TestConstant.LocalApiVersion, TestConstant.LocalBaseAddress);
		}

		//[Test]
		public async void SavePermanentEntity(){
			var testEntity = new PermanentTestEntity ();
			testEntity.TestName ="first name";
			testEntity.TestCreated = DateTime.Now;
			var result = await testEntity.SaveAsync ();
			Console.WriteLine (result);
		}


		//[Test]
		public async void UpdatePermanentEntity(){
			var testEntity = new PermanentTestEntity ();
			testEntity.ObjectId = "22EF0344-90AA-67CB-FF0B-5BA19B5B3800";
			testEntity.TestName ="second 2 name";
			testEntity.TestCreated = DateTime.Now.AddYears (-2);
			var result = await testEntity.UpdateAsync ();
			Console.WriteLine (result);
		}

		//[Test]
		public async void DeletePermanentEntity(){
			var testEntity = new PermanentTestEntity ();
			testEntity.ObjectId = "4CA8BADF-78D1-CECB-FF6C-659101B57D00";
			testEntity.TestName ="second 2 name";
			testEntity.TestCreated = DateTime.Now.AddYears (-2);
			var result = await testEntity.DeleteAsync ();
			Console.WriteLine (result);
		}

		//[Test]
		public async void SearchPermanentEntity(){
			//var res = BackendlessEntity.ReadAsync<PermanentTestEntity>(x => x.IsDeleted = true && x.TestName == "tyty");
			var query = new BackendlessQuery<PermanentTestEntity> ();
			query.Where = x => x.TestName == "put 1" || x.TestCreated == DateTime.Now;
			var items = await BackendlessEntity.ReadAsync<PermanentTestEntity> (query);
			var g = 0;
		}

		//[Test]
		public async void BasicSearchPermanentEntity(){
			//var res = BackendlessEntity.ReadAsync<PermanentTestEntity>(x => x.IsDeleted = true && x.TestName == "tyty");
			var query = BackendlessQuery<PermanentTestEntity>.AllCollectionQuery;
			var items = await BackendlessEntity.ReadAsync<PermanentTestEntity> ("AF071F6A-CA8B-1D00-FF14-C35C8A623800");
			var g = 0;
		}


		#region Standart


		//[Test]
		public async void SaveEntity(){
			var testEntity = new StandartTestEntity ();
			testEntity.TestName ="first name";
			testEntity.TestCreated = DateTime.Now;
			var result = await testEntity.SaveAsync ();
			Console.WriteLine (result);
		}


		//[Test]
		public async void UpdateEntity(){
			var testEntity = new StandartTestEntity ();
			testEntity.ObjectId = "69ECF846-ED31-F0D6-FF53-9F77710F1300";
			testEntity.TestName ="second 2 name";
			testEntity.TestCreated = DateTime.Now.AddYears (-2);
			var result = await testEntity.UpdateAsync ();
			Console.WriteLine (result);
		}

		//[Test]
		public async void DeleteEntity(){
			var testEntity = new StandartTestEntity ();
			testEntity.ObjectId = "D59777AA-F67B-78AC-FF9D-F3071D706500";
			testEntity.TestName ="second 2 name";
			testEntity.TestCreated = DateTime.Now.AddYears (-2);
			var result = await testEntity.DeleteAsync ();
			Console.WriteLine (result);
		}

		#endregion

	}
}

