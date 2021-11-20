using Idionline.Models;
using MongoDB.Bson;
using MongoDB.Driver;

var db = new MongoClient("mongodb://localhost:27017").GetDatabase("db_idionline");
var idioms = db.GetCollection<Idiom>("idioms");
var launchInfo = db.GetCollection<LaunchInfo>("launchinfo");
var editors = db.GetCollection<Editor>("editors");

var idiomAll = idioms.Find(new BsonDocument()).ToList();

Console.WriteLine("Done.");