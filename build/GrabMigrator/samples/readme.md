# Grab migrator utility quickstart
Purposes of this tool:
1. Grab EF-migrations from platform and modules as SQL idempotent scripts (safe to apply repeatedly over early applied);
1. Apply migrations accordingly to specific data sources and in specific order without installed platform and source codes;
1. Two grabbing modes: v2->v3 upgrade scripts, all scripts;
1. Apply scripts in one transaction per each module.

## 1. How to run
```
vc-build GrabMigrator --grab-migrator-config <configfile>
```

## 2. Grabbing migrations from platform and modules
### 1. Checkout platform/modules source codes
### 2. Ensure you have dotnet-ef tool installed. If not, reference to https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet  (dotnet tool install --global dotnet-ef)
### 3. Prepare grab config file accordingly to this scratch:
``` json
{
  "MigrationDirectories": [
    "D:\\AK\\Code\\Projects\\VC3-DEV-CORE3\\modules",
    "D:\\AK\\Code\\Projects\\VC3-DEV-CORE3\\vc-platform\\src\\VirtoCommerce.Platform.Data",
    "D:\\AK\\Code\\Projects\\VC3-DEV-CORE3\\vc-platform\\src\\VirtoCommerce.Platform.Security"
  ],
  "StatementsDirectory": "Statements"
}
```
Nodes explanation:
| Node | Description  |
| - | -- |
| MigrationDirectories | Directories where tool will search for migrations. One or more paths. |
| StatementsDirectory | There the tool will store grabbed sql statements. One file for every module. Default is 'Statements'. |
| Mode | 'V2V3' or 'All'. Upgrade platform v2 to v3 scripts or all scripts should be grabbed. Default is 'V2V3'. |
### 4. Run the tool, wait a lot, sql files appearing in StatementsDirectory.
### 5. Look into config file: ConnectionStringsRefs node appeared. 


## 3. Applying migrations to a different databases

### 1. Have prepared statements to apply
### 2. Prepare apply config file accordingly to this scratch:
``` json
{
  "ApplyingOrder": [
    "VirtoCommerce.Platform",
    "VirtoCommerce.Platform.Security",
    "VirtoCommerce.CoreModule",
    "VirtoCommerce.TaxModule",
    "VirtoCommerce.InventoryModule",
    "VirtoCommerce.ImageToolsModule",
    "VirtoCommerce.NotificationsModule",
    "VirtoCommerce.ContentModule",
    "VirtoCommerce.Payment",
    "VirtoCommerce.StoreModule",
    "VirtoCommerce.CustomerModule",
    "VirtoCommerce.CatalogModule",
    "VirtoCommerce.ShippingModule",
    "VirtoCommerce.SitemapsModule",
    "VirtoCommerce.PricingModule",
    "VirtoCommerce.CartModule",
    "VirtoCommerce.OrdersModule",
    "VirtoCommerce.MarketingModule",
    "VirtoCommerce.SubscriptionModule",
    "VirtoCommerce.CustomerReviews",
    "VirtoCommerce.CatalogPersonalizationModule",
    "VirtoCommerce.CatalogPublishingModule",
    "VirtoCommerce.DynamicAssociationsModule",
    "VirtoCommerce.QuoteModule"
  ],
  "PlatformConfigFile": "D:\\AK\\Code\\Projects\\VC3-DEV-CORE3\\vc-platform\\src\\VirtoCommerce.Platform.Web\\appsettings.json",
  "StatementsDirectory": "Statements",
  "CommandTimeout": 30,
  "Grab": false,
  "Apply": true
}
```
Nodes explanation:
| Node | Description  |
| - | -- |
| ApplyingOrder | An order the tool will use to apply scripts sequentally |
| PlatformConfigFile | A place where platform config is. This used to discover connection strings for every module |
| StatementsDirectory | There stored previously grabbed sql statements. One file for every module. |
| CommandTimeout | Command timeout in seconds |
| Grab | Switches the tool to grab mode (if true) |
| Apply | Switches the tool to apply mode (if true) |

### 3 Copy node ConnectionStringsRefs from grab config file to apply config file.
### 4. Run the tool
