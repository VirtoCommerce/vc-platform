using System.Collections.Generic;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;

namespace VirtoCommerce.SearchModule.Data.Providers.ElasticSearch
{
    public class ElasticClient<T>
    {
        private readonly IElasticConnection connection;
        private readonly IJsonSerializer serializer;


        public ElasticClient(IJsonSerializer serializer, string defaultHost = null, int defaultPort = 9200)
        {
            connection = new ElasticConnection(defaultHost, defaultPort);
            this.serializer = serializer;
        }


        public ElasticClient(string defaultHost, int defaultPort) :
            this(new JsonNetSerializer(), defaultHost, defaultPort)
        {
        }


        public IJsonSerializer Serializer
        {
            get { return serializer; }
        }

        public IElasticConnection Connection
        {
            get { return connection; }
        }

        public GetResult<T> Get(GetCommand getCommand)
        {
            var result = connection.Get(getCommand);
            return Serializer.ToGetResult<T>(result);
        }

        public bool IndexExists(IndexExistsCommand indexExistsCommand)
        {
            try
            {
                connection.Head(indexExistsCommand);
                return true;
            }
            catch (OperationException ex)
            {
                if (ex.HttpStatusCode == 404)
                    return false;
                throw;
            }
        }

        public IndexResult CreateIndex(IndexCommand indexCommand, string indexSettings = null)
        {
            var result = connection.Put(indexCommand, indexSettings);
            return Serializer.ToIndexResult(result);
        }

        public IndexResult Index(IndexCommand indexCommand, object document = null)
        {
            string data = Serializer.ToJson(document);
            var result = connection.Put(indexCommand, data);
            return Serializer.ToIndexResult(result);
        }

        public BulkResult IndexBulk(BulkCommand bulkCommand, IEnumerable<ESDocument> documents = null)
        {
            string bulkJson =
                new BulkBuilder(serializer)
                   .BuildCollection(documents,
                        (builder, document) => builder.Index(data: document, id: document.Id.ToString())
                // You can apply any custom logic here
                // to generate Indexes, Creates or Deletes.
            );

            string result = connection.Post(bulkCommand, bulkJson);

            //Parse bulk result;
            var bulkResult = serializer.ToBulkResult(result);

            return bulkResult;
        }

        /*
        public string IndexBulk(BulkCommand bulkCommand, IEnumerable<ESDocument> documents = null)
        {
            string data = Serializer.ToJson(documents);

            string command = "index";
            string index = bulkCommand.Index;
            string typeName = bulkCommand.Type;

            var sb = new StringBuilder();
            var action = String.Format("{{ \"{0}\" : {{ \"_index\" : \"{1}\", \"_type\" : \"{2}\"", command, index, typeName);

            foreach (var document in documents)
            {
                var objectAction = action;
                objectAction += String.Format(", \"_id\" : \"{0}\" ", document["__key"]);

                objectAction += "} }\n";

                sb.Append(objectAction);
                if (command == "index")
                {
                    string jsonCommand = Serializer.ToJson(document);
                    sb.Append(jsonCommand + "\n");
                }
            }
            var json = sb.ToString();

            var result = connection.Put(bulkCommand, json);
            return result;
            //return Serializer.ToIndexResult(result);
        }
         * */

        public DeleteResult Delete(DeleteCommand deleteCommand)
        {
            var result = connection.Delete(deleteCommand);
            return Serializer.ToDeleteResult(result);
        }

        public SearchResult<T> Search(SearchCommand searchCommand, QueryBuilder<T> query)
        {
            string jsonData = query.Build();
            var results = connection.Post(searchCommand, jsonData);
            return Serializer.ToSearchResult<T>(results);
        }

        public string GetMapping(GetMappingCommand getMappingCommand)
        {
            return connection.Get(getMappingCommand);
        }

        public CommandResult PutMapping(PutMappingCommand putMappingCommand, string mapping)
        {
            string result = connection.Put(putMappingCommand, mapping);
            return Serializer.ToCommandResult(result);
        }

        public DeleteResult DeleteMapping(DeleteMappingCommand deleteMappingCommand)
        {
            var result = connection.Delete(deleteMappingCommand);
            return Serializer.ToDeleteResult(result);
        }


        public CommandResult Flush(FlushCommand flushCommand)
        {
            string result = connection.Post(flushCommand);
            return Serializer.ToCommandResult(result);
        }

        public CommandResult Close(CloseCommand closeCommand)
        {
            string result = connection.Post(closeCommand);
            return Serializer.ToCommandResult(result);
        }

        public CommandResult Open(OpenCommand openCommand)
        {
            string result = connection.Post(openCommand);
            return Serializer.ToCommandResult(result);
        }

        public CommandResult UpdateSettings(UpdateSettingsCommand updateSettingsCommand, string settings)
        {
            string result = connection.Put(updateSettingsCommand, settings);
            return Serializer.ToCommandResult(result);
        }

    }
}
