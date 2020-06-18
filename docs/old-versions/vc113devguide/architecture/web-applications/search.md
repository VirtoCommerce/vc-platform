---
title: Search
description: Search
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 4
---
Out of the box Virto Commerce uses Elastic Search. Elastic search is installed as a windows service when running on-premises and as a worker role when running in azure environment. It run is a tomcat java container.

## Search Indexing

IndexingВ can be done for different types of documents, calledВ DocumentType's in theВ code. Each document type indexing in turn isВ split into 3 parts:

1. Create partitions - splits the task of indexing large data set into sets, by default of 100 items.
2. Create documents - creates name/valueВ documents that can be then submitted to the indexing server.
3. Publish document - publishes the documents to the indexing server.

The indexing process can be done in parallel on multiple servers akaВ WorkRoleВ orВ WebRoleВ in Azure Environment.

### Create Partitions

During this process we create partitions for the set of data to be indexed. It can be based from the full set of data like catalog products or a subset based on the last date the indexing ran. The partitions are then added to the Queue. The partition creator class must implementВ ISearchIndexPartitionCreatorВ interface.

### Create Documents

During this process we dequeue messages from the queue where partitions wereВ submitted to andВ createВ name/value documents. Class that implement this functionality must implementISearchIndexDocumentCreator<T> interface where T is typically "string[]". The class will be passed an array of strings which contains ID's of objects that will need to be converted into documents.

### PublishВ Documents

As soon as the first set of documents is created the publishing starts. The documents are submitted using the provider interface and are handed over to the default search provider which currently can be either Lucene or SOLR. The document publisher implementsВ ISearchIndexDocumentPublisherВ interface.

### Indexing Coordination

The overall process is controlled by the class implementingВ ISearchIndexController. For exampleВ AzureSearchIndexControllerВ does that for the azure environment. That class puts messages into the queue, dequeues items and saves the progress to theВ BuildSettingsВ table. It also makes sure the task of creating partitions is only executed once across all servers.

### Configuration

Each document type needs to have anВ IndexBuilderВ configured. For example Catalog has aВ CatalogItemIndexBuilderВ configured in search.config file. It needs to implementВ ISearchIndexBuilderinterface which simply contains methods to initiate 3 parts of the indexing process:В CreatePartitions,В CreateDocuments,В PublishDocumentsВ and is instantiated by theВ ISearchIndexController.
