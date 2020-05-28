# Product Indexing

Product indexing is used to improve the search performance and speed.

The Index widget, located on Product details screen, compares the date of product indexing in the search engine with the current indexing date.

VirtoCommerce reindexes automatically whenever one or more items are changed (for example, price changes, catalog or shopping cart price rules are created, new categories added, and so on). Reindexing is performed as a background process. However, the indexing can be done also manually using the 'Build index' functionality.

## Indexing Widget

### Access Indexing Widget

1. In order to access the Indexing widget, the user should open the Product details screen; 
1. The Indexing widget displays the information about when the product was last indexed;
1. The user clicks on the Indexing widget;
1. The system will open the 'Search index details' of the product and the Index content (json) will be displayed.

![Fig. Indexing Widget](media/screen-index-widget.png)

### Build Index

1. The user navigates to the 'Product details' screen and make some changes to the product details;
1. The Indexing widget changes the color from light blue to rose, which means that changes to product details made, but not indexed;
1. The user clicks on the widget to open the 'Search index details' and clicks on the 'Build index' button;
1. The system starts the indexing process. Once the indexing is completed, the system will display the correspondent message on the screen.
1. The Indexing widget becomes light blue and new information about last indexing will be displayed.

![Fig. Change product details](media/screen-change-details-index.png)


![Fig. Indexaing completed](media/screen-indexation-completed.png)