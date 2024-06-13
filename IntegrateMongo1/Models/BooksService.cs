
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Text.Json.Nodes;

namespace IntegrateMongo1.Models
{
    public class BooksService : IBooksService
    {
        private readonly IMongoCollection<AppBook> collection;

        public BooksService(IOptions<AppDbSettings> dbSetting)
        {
            var mongoClient = new MongoClient(dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSetting.Value.DatabaseName);
            collection = mongoDatabase.GetCollection<AppBook>(dbSetting.Value.BooksCollectionName);
        }

        public async Task<List<AppBook>> GetAll(GetAllParams _params)
        {
            FilterDefinition<AppBook> filter = Builders<AppBook>.Filter.Empty;
            SortDefinition<AppBook> sort = Builders<AppBook>.Sort.Ascending(x => x.Id);
            
            if (_params?.filterField?.ToLower() == "bookname" && !string.IsNullOrWhiteSpace(_params.filterValue))
            {
                filter = Builders<AppBook>.Filter.Text(_params.filterValue);
            }
            if (_params?.sortField?.ToLower() == "bookname")
            {
                if (_params.sortAsc) sort = Builders<AppBook>.Sort.Ascending(x => x.BookName);
                else sort = Builders<AppBook>.Sort.Descending(x => x.BookName);
            } else if (string.IsNullOrWhiteSpace(_params?.sortField) && _params?.sortAsc == false)
            {
                sort = Builders<AppBook>.Sort.Descending(x => x.Id);
            }

            int skipResult = (_params.page - 1) * _params.pageSize;
            return await collection.Find(filter).Sort(sort).Skip(skipResult).Limit(_params.pageSize).ToListAsync();
        }

        public async Task<AppBook> GetById(string id)
        {
            //string match = "{$match: {_id: ObjectId(\"" + id + "\")}}";
            //string lookup = "{$lookup: {from: \"categories\", localField: \"categories\", foreignField: \"_id\", as: \"category_docs\"}}";
            //var aggregatePipline = new BsonDocument[]
            //{
            //    BsonDocument.Parse(match),
            //    BsonDocument.Parse(lookup)
            //};
            //var bsonDoc = collection.Aggregate<BsonDocument>(aggregatePipline).FirstOrDefault();
            //var result = BsonSerializer.Deserialize<AppBook>(bsonDoc);
            //return result;
            //return await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

            BsonDocument pipelineStage1 = new BsonDocument
            {
                {
                    "$match", new BsonDocument {
                        { "_id", ObjectId.Parse(id) }
                    }
                }
            };

            BsonDocument pipelineStage2 = new BsonDocument
            {
                {
                    "$lookup", new BsonDocument
                    {
                        { "from", "categories" },
                        { "localField", "categories" },
                        { "foreignField", "_id" },
                        { "as", "category_docs" }
                    }
                }
            };

            BsonDocument pipelineStage3 = new BsonDocument
            {
                {
                    "$project", new BsonDocument
                    {
                        { "_id", 1 },
                        { "name", 1 },
                        { "price", 1 },
                        { "author", 1 },
                        { "category_docs", 1 },
                    }
                }
            };

            BsonDocument[] pipepline = { pipelineStage1, pipelineStage2, pipelineStage3 };
            var bsonDoc = await collection.Aggregate<BsonDocument>(pipepline).FirstOrDefaultAsync();
            var result = BsonSerializer.Deserialize<AppBook>(bsonDoc);
            return result;
        }

        public async Task<AppBook> Insert(AppBookDto bookDto)
        {
            var book = new AppBook
            {
                BookName = bookDto.BookName,
                Author = bookDto.Author,
                Category = bookDto.Category,
                Price = bookDto.Price
            };
            await collection.InsertOneAsync(book);
            return book;
        }

        public async Task<AppBook> Update(string id, AppBookDto bookDto)
        {
            var book = new AppBook
            {
                Id = id,
                BookName = bookDto.BookName,
                Author = bookDto.Author,
                Category = bookDto.Category,
                Price = bookDto.Price
            };
            await collection.ReplaceOneAsync(x => x.Id == id, book);
            return book;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await collection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
