using System;
using System.Linq;
using System.Threading;
using CommerceFoundation.UI.FunctionalTests.Catalogs.Extensions;
using CommerceFoundation.UI.FunctionalTests.TestHelpers;
using FunctionalTests.Catalogs;
using FunctionalTests.TestHelpers;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using UI.FunctionalTests.Helpers.Common;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using Xunit;

namespace CommerceFoundation.UI.FunctionalTests.Catalogs
{
	[Variant(RepositoryProvider.DataService)]
	public class CatalogViewModelScenarios : FunctionalUITestBase
	{
		#region Infrastructure/ setup

		public override void DefService()
		{
			ServManager.AddService(ServiceNameEnum.AppConfig);
			ServManager.AddService(ServiceNameEnum.Catalog);
		}

		#endregion

		[Fact]
		public void Can_create_catalog_propertysets_and_acceptchanges()
		{
			var catalogBuilder = CatalogBuilder.BuildCatalog("Test catalog").WithCategory("category").WithProducts(2);
			var catalog = catalogBuilder.GetCatalog() as Catalog;

			var propertySet = catalog.PropertySets[0];
			dynamic copy = propertySet.Local();

			copy.Name = "new name";
			copy.AcceptChanges();

			Assert.Equal(propertySet.Name, "new name");
		}

		[RepositoryTheory]
		public void Can_create_catalogviewmodel_in_wizardmode()
		{
			var vmFactory = new TestCatalogViewModelFactory<ICatalogOverviewStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var repositoryFactory =
				new DSRepositoryFactory<ICatalogRepository, DSCatalogClient, CatalogEntityFactory>(
					ServManager.GetUri(ServiceNameEnum.Catalog));

			//create item using entity factory
			var entityFactory = new CatalogEntityFactory();
			var item = entityFactory.CreateEntity<Catalog>();

			var createViewModel = new CreateCatalogViewModel(vmFactory, item);
			var overviewViewModel = createViewModel.AllRegisteredSteps[0] as ViewModelDetailAndWizardBase<Catalog>;
			overviewViewModel.InitializeForOpen();

			//check the default values in stepViewModel
			Assert.False(createViewModel.AllRegisteredSteps[0].IsValid);

			// step 1
			//fill the properties for the first step
			overviewViewModel.InnerItem.CatalogId = "TestCatalog";
			overviewViewModel.InnerItem.Name = "TestName";
			overviewViewModel.InnerItem.CatalogLanguages.Add(new CatalogLanguage()
			{
				Language = "ru-ru",
				CatalogId = overviewViewModel.InnerItem.CatalogId
			});
			overviewViewModel.InnerItem.DefaultLanguage = "ru-ru";

			Assert.True(createViewModel.AllRegisteredSteps[0].IsValid);

			// final actions: save
			createViewModel.PrepareAndSave();

			using (var repository = repositoryFactory.GetRepositoryInstance())
			{
				var itemFromDb = repository.Catalogs.Where(s => s.CatalogId == item.CatalogId).OfType<Catalog>().Expand(x => x.CatalogLanguages).SingleOrDefault();

				Assert.NotNull(itemFromDb);
				Assert.True(itemFromDb.Name == "TestName");
				Assert.True(itemFromDb.DefaultLanguage == "ru-ru");
				Assert.True(itemFromDb.CatalogLanguages.Any(x => x.Language == "ru-ru"));
			}
		}

		[RepositoryTheory]
		public void Can_create_categoryviewmodel_in_wizardmode()
		{
			var repositoryFactory =
				new DSRepositoryFactory<ICatalogRepository, DSCatalogClient, CatalogEntityFactory>(ServManager.GetUri(ServiceNameEnum.Catalog));

			const string catalogId = "testcatalog";
			var catalogBuilder = CatalogBuilder.BuildCatalog(catalogId);
			var catalog = catalogBuilder.GetCatalog() as Catalog;

			using (var repository = repositoryFactory.GetRepositoryInstance())
			{
				repository.Add(catalog);
				repository.UnitOfWork.Commit();
			}

			var propertiesVmFactory = new TestCatalogViewModelFactory<ICategoryPropertiesStepViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));
			var overviewVmFactory = new TestCatalogViewModelFactory<ICategoryOverviewStepViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));

			//create item using entity factory
			var entityFactory = new CatalogEntityFactory();
			var item = entityFactory.CreateEntity<Category>();
			item.CatalogId = catalogId;
			item.Catalog = catalog;

			var createViewModel = new CreateCategoryViewModel(propertiesVmFactory, overviewVmFactory, item);
			var overviewViewModel = createViewModel.AllRegisteredSteps[0] as CategoryViewModel;
			overviewViewModel.InitializeForOpen();
			var propertyValuesViewModel = createViewModel.AllRegisteredSteps[1] as CategoryViewModel;
			propertyValuesViewModel.InitializeForOpen();

			//check the default values in stepViewModel
			Assert.False(createViewModel.AllRegisteredSteps[0].IsValid);
			Assert.True(createViewModel.AllRegisteredSteps[1].IsValid);

			// step 1
			//fill the properties for the first step
			overviewViewModel.InnerItem.Name = "TestName";
			overviewViewModel.InnerItem.Code = "TestCode";
			var propertySet = overviewViewModel.AvailableCategoryTypes.First();
			overviewViewModel.InnerItem.PropertySet = propertySet;
			overviewViewModel.InnerItem.PropertySetId = propertySet.PropertySetId;

			Assert.True(createViewModel.AllRegisteredSteps[0].IsValid);

			// step 2
			//fill the values for the property values step
			propertyValuesViewModel.PropertiesAndValues[0].Value = new CategoryPropertyValue()
				{
					ShortTextValue = "short text",
					Name = propertyValuesViewModel.PropertiesAndValues[0].Property.Name,
					ValueType = propertyValuesViewModel.PropertiesAndValues[0].Property.PropertyValueType
				};
			propertyValuesViewModel.InnerItem.CategoryPropertyValues.Add((CategoryPropertyValue)propertyValuesViewModel.PropertiesAndValues[0].Value);

			Assert.True(createViewModel.AllRegisteredSteps[1].IsValid);

			// final actions: save
			propertyValuesViewModel.InnerItem.Catalog = null;
			createViewModel.PrepareAndSave();

			using (var repository = repositoryFactory.GetRepositoryInstance())
			{
				var itemFromDb = repository.Categories.Where(s => s.CategoryId == item.CategoryId).OfType<Category>().ExpandAll().SingleOrDefault();

				Assert.NotNull(itemFromDb);
				Assert.True(itemFromDb.Name == "TestName");
				Assert.True(itemFromDb.CategoryPropertyValues.Any(x => x.ShortTextValue == "short text"));
			}
		}

		[RepositoryTheory]
		public void Can_add_update_delete_item_property_values()
		{
			var catalogName = "Test catalog";
			var catalogBuilder = CatalogBuilder.BuildCatalog(catalogName).WithCategory("category").WithProducts(1);
			var catalog = catalogBuilder.GetCatalog() as Catalog;
			var item = catalogBuilder.GetItems()[0];

			var property1 = new Property { Name = "bool", PropertyValueType = PropertyValueType.Boolean.GetHashCode() };
			var property2 = new Property { Name = "datetime", PropertyValueType = PropertyValueType.DateTime.GetHashCode() };
			var property3 = new Property { Name = "Decimal", PropertyValueType = PropertyValueType.Decimal.GetHashCode() };
			var property4 = new Property { Name = "int", PropertyValueType = PropertyValueType.Integer.GetHashCode() };
			var property5 = new Property { Name = "longstr", PropertyValueType = PropertyValueType.LongString.GetHashCode() };
			var property6 = new Property { Name = "shorttext", PropertyValueType = PropertyValueType.ShortString.GetHashCode() };

			var propertySet = catalog.PropertySets[0];
			propertySet.PropertySetProperties.Add(new PropertySetProperty { Property = property1 });
			propertySet.PropertySetProperties.Add(new PropertySetProperty { Property = property2 });
			propertySet.PropertySetProperties.Add(new PropertySetProperty { Property = property3 });
			propertySet.PropertySetProperties.Add(new PropertySetProperty { Property = property4 });
			propertySet.PropertySetProperties.Add(new PropertySetProperty { Property = property5 });
			propertySet.PropertySetProperties.Add(new PropertySetProperty { Property = property6 });
			propertySet.PropertySetProperties.ToList().ForEach(x =>
			{
				x.Property.IsRequired = true;
				x.Property.CatalogId = catalogName;
			});

			var repositoryFactory = new DSRepositoryFactory<ICatalogRepository, DSCatalogClient, CatalogEntityFactory>(ServManager.GetUri(ServiceNameEnum.Catalog));
			using (var repository = repositoryFactory.GetRepositoryInstance())
			{
				repository.Add(catalog);
				repository.Add(item);
				repository.UnitOfWork.Commit();
			}

			IRepositoryFactory<IPricelistRepository> pricelistRepositoryFactory = new DSRepositoryFactory<IPricelistRepository, DSCatalogClient, CatalogEntityFactory>(ServManager.GetUri(ServiceNameEnum.Catalog));
			IViewModelsFactory<IPropertyValueBaseViewModel> propertyValueVmFactory = new TestCatalogViewModelFactory<IPropertyValueBaseViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));
			IViewModelsFactory<IPriceViewModel> priceVmFactory = new TestCatalogViewModelFactory<IPriceViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));
			IViewModelsFactory<IItemAssetViewModel> assetVmFactory = new TestCatalogViewModelFactory<IItemAssetViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));
			IViewModelsFactory<IAssociationGroupEditViewModel> associationGroupEditVmFactory = new TestCatalogViewModelFactory<IAssociationGroupEditViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));
			IViewModelsFactory<IAssociationGroupViewModel> associationGroupVmFactory = new TestCatalogViewModelFactory<IAssociationGroupViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));
			IViewModelsFactory<IItemRelationViewModel> itemRelationVmFactory = new TestCatalogViewModelFactory<IItemRelationViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));
			IViewModelsFactory<IEditorialReviewViewModel> reviewVmFactory = new TestCatalogViewModelFactory<IEditorialReviewViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));
			IViewModelsFactory<ICategoryItemRelationViewModel> categoryVmFactory = new TestCatalogViewModelFactory<ICategoryItemRelationViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));
			ICatalogEntityFactory entityFactory = new CatalogEntityFactory();
			IAuthenticationContext authContext = new TestAuthenticationContext();
			INavigationManager navManager = new TestNavigationManager();

			var itemViewModel = new ItemViewModel(null, null, repositoryFactory, pricelistRepositoryFactory, propertyValueVmFactory, priceVmFactory, assetVmFactory, associationGroupEditVmFactory, associationGroupVmFactory, itemRelationVmFactory, reviewVmFactory, categoryVmFactory, entityFactory, item, authContext, navManager);
			itemViewModel.InitializeForOpen();

			// property change should set IsModified to true
			itemViewModel.InnerItem.EndDate = DateTime.UtcNow;
			Assert.True(itemViewModel.IsModified);

			Assert.False(itemViewModel.PropertyValueEditCommand.CanExecute(null));
			Assert.True(itemViewModel.PropertyValueEditCommand.CanExecute(itemViewModel.PropertiesAndValues[0]));

			itemViewModel.CommonConfirmRequest.Raised += EditValueSetConfirmation;

			foreach (var propItem in itemViewModel.PropertiesAndValues)
			{
				itemViewModel.PropertyValueEditCommand.Execute(propItem);
			}

			itemViewModel.SaveChangesCommand.Execute(null);
			Thread.Sleep(1000);// waiting for SaveChangesCommand to finish in background thread

			using (var repository = repositoryFactory.GetRepositoryInstance())
			{
				var itemFromDb = repository.Items.Expand(x => x.ItemPropertyValues).Single();

				Assert.True(itemFromDb.ItemPropertyValues.Count > 0);
				Assert.Equal(itemViewModel.PropertiesAndValues.Count, itemFromDb.ItemPropertyValues.Count);
			}

			// test if values are saved when updated in UI
			DecimalValue = 123123m;
			var valueToEdit =
				itemViewModel.PropertiesAndValues.First(x => x.Property.PropertyValueType == PropertyValueType.Decimal.GetHashCode());
			itemViewModel.PropertyValueEditCommand.Execute(valueToEdit);

			LongTextValue = "other long text";
			valueToEdit = itemViewModel.PropertiesAndValues.First(x => x.Property.PropertyValueType == PropertyValueType.LongString.GetHashCode());
			itemViewModel.PropertyValueEditCommand.Execute(valueToEdit);

			itemViewModel.SaveChangesCommand.Execute(null);
			Thread.Sleep(1000);// waiting for SaveChangesCommand to finish in background thread

			using (var repository = repositoryFactory.GetRepositoryInstance())
			{
				var itemFromDb = repository.Items.Expand(x => x.ItemPropertyValues).Single();

				Assert.Equal(DecimalValue, itemFromDb.ItemPropertyValues.First(x => x.ValueType == PropertyValueType.Decimal.GetHashCode()).DecimalValue);
				Assert.Equal(LongTextValue, itemFromDb.ItemPropertyValues.First(x => x.ValueType == PropertyValueType.LongString.GetHashCode()).LongTextValue);
			}

			// check if item can be saved without required property value
			var valueToDelete =
				itemViewModel.PropertiesAndValues.First(x => x.Property.PropertyValueType == PropertyValueType.Decimal.GetHashCode());
			itemViewModel.PropertyValueDeleteCommand.Execute(valueToDelete);

			itemViewModel.SaveChangesCommand.CanExecute(null);
			Thread.Sleep(1000);// waiting for SaveChangesCommand to finish in background thread

			//Assert True as the last Save command execution failed as the validation failed
			Assert.True(itemViewModel.IsModified);
		}

		private decimal DecimalValue = 123m;
		private string LongTextValue = "some long value text";

		private void EditValueSetConfirmation(object o, InteractionRequestedEventArgs args)
		{
			var confirmation = (Confirmation)args.Context;
			var propertyvalueVM = (PropertyValueBaseViewModel)confirmation.Content;

			if (propertyvalueVM.IsBooleanValue)
			{
				propertyvalueVM.InnerItem.Value.BooleanValue = true;
			}
			else if (propertyvalueVM.IsDateTimeValue)
			{
				propertyvalueVM.InnerItem.Value.DateTimeValue = DateTime.UtcNow;
			}
			else if (propertyvalueVM.IsDecimalValue)
			{
				propertyvalueVM.InnerItem.Value.DecimalValue = DecimalValue;
			}
			else if (propertyvalueVM.IsIntegerValue)
			{
				propertyvalueVM.InnerItem.Value.IntegerValue = 1234;
			}
			else if (propertyvalueVM.IsLongStringValue)
			{
				propertyvalueVM.InnerItem.Value.LongTextValue = LongTextValue;
			}
			else if (propertyvalueVM.IsShortStringValue)
			{
				propertyvalueVM.InnerItem.Value.ShortTextValue = "shrt";
			}

			confirmation.Confirmed = true;
			args.Callback.Invoke();
		}
	}
}
