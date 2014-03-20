using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace CodedUITests
{
	/// <summary>
	/// Summary description for CodedUITest1
	/// </summary>
	[CodedUITest]
	public class CatalogTests
	{
		public CatalogTests()
		{
            UiMap.OpenHomePageOnIEParams.Url = "http://localhost/store/en-us/electronics";
		}


		//[TestMethod]
		//public void Can_Admin_Login_As_John_Doe()
		//{
		//	// To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
		//	// For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
		//	UiMap.OpenHomePageOnIE();
		//	//UiMap.IENewSession();
		//	if (UiMap.UIHomeElectronicsStoreWindow.UIHomeElectronicsStoreDocument1.UILogInHyperlink.Exists)
		//	{
		//		UiMap.LoginAdmin();
		//		UiMap.IsUserLogedIn();
		//	}

		//}

        [TestMethod]
        public void Can_Open_TVVideoCategory()
        {
            UiMap.OpenHomePageOnIE();
            UiMap.OpenTVVideoPage();
            UiMap.IsTVVideoInBreadcrumb();
        }

		[TestMethod]
		public void Can_Open_AudioMP3Category()
		{
			UiMap.OpenHomePageOnIE();
			UiMap.OpenAudioMp3Page();
			UiMap.IsAudioMP3InBreadcrumb();
		}

		[TestMethod]
		public void Can_Open_CamerasCategory()
		{
			UiMap.OpenHomePageOnIE();
			UiMap.OpenCamerasCategory();
			UiMap.IsCamerasInBreadcrumb();
		}

		[TestMethod]
		public void Can_Open_ComputersAndTabletsCategory()
		{
			UiMap.OpenHomePageOnIE();
			UiMap.OpenComputersAndTabletsPage();
			UiMap.IsComputersAndTabletsInBreadcrumb();
		}

		[TestMethod]
		public void Can_Open_AccessoriesCategory()
		{
			UiMap.OpenHomePageOnIE();
			UiMap.OpenAccessoriesPage();
			UiMap.IsAccessoriesInBreadcrumb();
		}


		[TestMethod]
		public void Can_Edit_EditAccountInformation()
		{
			UiMap.OpenHomePageOnIE();
			if (UiMap.UIOnlineshoppingfromagWindow1.UIOnlineshoppingfromagDocument.UILayoutPane.UILoginHyperlink.Exists)
			{
				UiMap.LoginAdmin();
			}

			UiMap.OpenAccountPage();
			UiMap.IsMyDashboardOpened();

			UiMap.OpenAccountInformationPage();
			UiMap.IsEditAccountInformationVisible();

			UiMap.AccountRenameToJohnSmith();
			UiMap.IsJohnSmithWelcome();
		}

		[TestMethod]
		public void Can_Filter_Samsung_From_Menu()
		{
			UiMap.OpenHomePageOnIE();
			UiMap.FilterSamsungComputersFromMenu();
			UiMap.IsFilteredBySamsung();
		}

		[TestMethod]
		public void Can_Filter_Price_Over_1000()
		{
			UiMap.OpenHomePageOnIE();
			UiMap.OpenTVVideoPage();
			UiMap.SortByPriceAndFilterOver1000();
			UiMap.IsPriceGreaterThen1000();
		}

		[TestMethod]
		public void Can_Add_Samsung_UN19D4003_To_Cart()
		{
			UiMap.OpenHomePageOnIE();
			UiMap.AddToCartSamsungLN26D450();
			UiMap.IsSamsungLN26D450InCart();
		}

		[TestMethod]
		public void Can_Create_New_Order()
		{
			UiMap.OpenHomePageOnIE();
			UiMap.AddToCartSamsungLN26D450();
			UiMap.OpenCheckoutPage();
			UiMap.CheckoutUsingPaypal();
            //if (UiMap.UICheckoutElectronicsSWindow.UIPaywithaPayPalaccounDocument1.UILogInButton.WaitForControlExist(5000) == false)
            //{
            // This shoud work but doesnt!
            //    UiMap.ClickLoginToPaypal();
            //}
            UiMap.LoginToPaypal();
		    UiMap.UICheckoutElectronicsSWindow.UIReviewyourinformatioDocument.UIContinueButton.WaitForControlExist(5000);
            UiMap.FinishPaypal();
		    UiMap.UISuccessInternetExploWindow.UISuccessDocument.UIORDER2014032000000Hyperlink.WaitForControlExist(10000);
			UiMap.IsOrderTrackingNrValidFormat();
		}

		[TestMethod]
		public void Can_Apply_Test_Coupon()
		{
			UiMap.OpenHomePageOnIE();
			UiMap.AddToCartSamsungLN26D450();
            UiMap.OpenMyCartPage();
			UiMap.ApplyTestCoupon();
			UiMap.IsDiscountApplied();
		}

	    [TestMethod]
	    public void Can_Add_To_Wishlist()
	    {
            UiMap.OpenHomePageOnIE();
            UiMap.AddToWishListSdCard();
            UiMap.LoginAdmin();
            UiMap.OpenWishList();
            UiMap.IsSDCardInWishlist();
	    }

		#region Additional test attributes

		// You can use the following additional attributes as you write your tests:

		////Use TestInit.ialize to run code before running each test 
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{        
		//    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
		//    // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
		//}

		////Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{        
		//    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
		//    // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
		//}

		#endregion

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext { get; set; }

		public UIMap UiMap
		{
			get
			{
				if ((_map == null))
				{
					_map = new UIMap();
				}

				return _map;
			}
		}

		private UIMap _map;
	}
}
