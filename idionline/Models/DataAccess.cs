using Idionline.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Idionline
{
    public class DataAccess
    {
        readonly IMongoDatabase _db;
        readonly IMongoCollection<Idiom> _idioms;
        readonly IMongoCollection<LaunchInfo> _launchInfo;
        readonly IMongoCollection<Editor> _editors;
        public IdionlineSettings Config;
        readonly Version version = Assembly.GetEntryAssembly().GetName().Version;
        public DataAccess(IOptions<IdionlineSettings> option)
        {
            Config = option.Value;
            _db = new MongoClient("mongodb://localhost:27017").GetDatabase("IdionlineDB");
            _idioms = _db.GetCollection<Idiom>("Idioms");
            _launchInfo = _db.GetCollection<LaunchInfo>("LaunchInfo");
            _editors = _db.GetCollection<Editor>("Editors");
        }
        #region 测试用的生成代码，方便以后再瞎折腾就先不删，注释掉。

        //public void Test()
        //{
        //    List<Idiom> items = _idioms.Find(new BsonDocument()).ToList();
        //    foreach (var item in items)
        //    {
        //        if (item.Definitions[0].Links != null && item.Definitions[0].Source == "pwxcoo 的新华字典项目")
        //        {
        //            string target = item.Definitions[0].Links.Values.First();
        //            if (item.Definitions[0].Text.Contains(target + "”"))
        //            {
        //                item.Definitions[0].Text.Replace(target, "“" + target);
        //            }
        //            else
        //            {
        //                item.Definitions[0].Text = item.Definitions[0].Text.Replace(target, "“" + target + "”");
        //            }
        //            _idioms.FindOneAndReplace(x => x.Id == item.Id, item);
        //            Console.WriteLine(item.Name + ": Done.");
        //        }
        //    }



        //List<Idiom> items = _idioms.Find(new BsonDocument()).ToList();
        //foreach (var item in items)
        //{
        //    if (item.Name.Length < 4)
        //    {
        //        item.Name = item.Name + "，需要订正";
        //        _idioms.FindOneAndReplace(x => x.Id == item.Id, item);
        //        Console.WriteLine("已标注：" + item.Name);
        //    }
        //}
        //}

        //public string GenerateIdiom()
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>
        //    {
        //        { "5beghawgsagsaga7eb855e3e94", "66hhh" },
        //        { "5bebec2a2hgbhghhhhhhhh855e3e94", "6gggggh" }
        //    };
        //    Definition def = new Definition { Text = "hahaha", Addition = "666", IsBold = false, Source = "hhh", Links = dic };
        //    Definition def2 = new Definition { Text = "haa", Addition = "345sgsdgsdgc6", IsBold = false, Source = "hhh", Links = dic };
        //    List<Definition> defs = new List<Definition>
        //    {
        //        def,
        //        def2
        //    };
        //    _idioms.InsertOne(new Idiom { Name = "TEST", Definitions = defs, LastEditor = "fssssss", UpdateTimeUT = 666666, Index = 'C' });
        //    return "Done!";
        //}

        //public string GenerateLaunchInf()
        //{
        //    Dictionary<string, string> i = new Dictionary<string, string> {                { "aaaaaaaa", "6ggggh" },
        //        { "5bshedfhdfh4", "6gadfadah" }};
        //    _launchInf.InsertOne(new LaunchInf { Text = "23333", DailyIdiom = null, /*DailyIdiomName = "6666",*/  ThemeColor = null, LogoUrl = null, DisableAds = false, /*FloatEasterEggs = i, */DateUT = DateTimeOffset.MinValue.ToUnixTimeSeconds() });
        //    return "Done!";
        //}
        //public async Task<string> ToPinyin()
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        List<Idiom> items = _idioms.Find(new BsonDocument()).ToList();
        //        foreach (var item in items)
        //        {
        //            if (items.IndexOf(item) >= 9200)
        //            {
        //                var res = await httpClient.GetStringAsync("http://v1.alapi.cn/api/pinyin?word=" + item.Name + "&tone=1");
        //                try
        //                {
        //                    JObject json = JObject.Parse(res);
        //                    if (json["msg"].ToString() == "success")
        //                    {
        //                        var filter = Builders<Idiom>.Filter.Eq("_id", new ObjectId(item.Id));
        //                        var update = Builders<Idiom>.Update.Set("Pinyin", json["data"]["pinyin"]);
        //                        _idioms.UpdateOne(filter, update);
        //                    }
        //                }
        //                catch (Exception)
        //                {

        //                }
        //                Console.WriteLine("Progress: " + (100.0 / items.Count) * (items.IndexOf(item) + 1) + "%");
        //            }

        //        }

        //        System.Threading.Thread.Sleep(200);
        //    }
        //    return "Complete!";
        //}
        //public void Test()
        //{
        //    List<Idiom> list = _idioms.Find(x => x.Definitions[1].Text.Contains("③")).ToList();
        //    foreach (var item in list)
        //    {
        //        //if (item.Definitions[0].Text.Length >= 5)
        //        //{
        //        //    try
        //        //    {
        //        //        string name = item.Definitions[0].Text.Substring(item.Definitions[0].Text.IndexOf("亦作") + 2, 4);
        //        //        Console.WriteLine(name);
        //        //        Idiom target = _idioms.Find(x => x.Name == name).FirstOrDefault();
        //        //        if (target != null)
        //        //        {
        //        //            Dictionary<string, string> dic = new Dictionary<string, string>();
        //        //            dic.Add(target.Id, target.Name);
        //        //            item.Definitions[0].Links = dic;
        //        //            List<Definition> df = item.Definitions;
        //        //            UpdateDefinition<Idiom> upd = Builders<Idiom>.Update.Set("Definitions", df);
        //        //            _idioms.UpdateOne(x => x.Id == item.Id, upd);
        //        //        }
        //        //    }
        //        //    catch
        //        //    {
        //        //        continue;
        //        //    }

        //        //}

        //        try
        //        {
        //            //string[] def = item.Definitions[0].Text.Split("②");
        //            Console.WriteLine(item.Id+" "+item.Name);
        //            //Idiom target = _idioms.Find(x => x.Id == item.Id).FirstOrDefault();
        //            //if (target != null)
        //            //{
        //            //    item.Definitions[0].Text = def[0].Replace("①", "");
        //            //    item.Definitions.Add(new Definition { Text = def[1], Addition = null, Examples = null, IsBold = false, Links = null, Source = item.Definitions[0].Source });
        //            //    List<Definition> df = item.Definitions;
        //            //    UpdateDefinition<Idiom> upd = Builders<Idiom>.Update.Set("Definitions", df);
        //            //    _idioms.UpdateOne(x => x.Id == item.Id, upd);
        //            //    Console.WriteLine(item.Id + "：" + item.Name + " 已修正！");
        //            //}
        //        }
        //        catch
        //        {
        //            continue;
        //        }

        //    }
        //    Console.WriteLine("Done");
        //}
        #endregion
        //这里生成每日成语。
        public void GenLI()
        {
            DateTimeOffset dateUT = DateTimeOffset.Now;
            int hour = dateUT.Hour;
            int min = dateUT.Minute;
            int sec = dateUT.Second;
            long dateL = dateUT.AddSeconds(-sec).AddMinutes(-min).AddHours(-hour).ToUnixTimeSeconds();
            //默认的每日成语。
            Idiom deftIdiom = _launchInfo.Find(x => x.DateUT == DateTimeOffset.MinValue.ToUnixTimeSeconds()).FirstOrDefault().DailyIdiom;
            LaunchInfo info = _launchInfo.Find(x => x.DateUT == dateL).FirstOrDefault();
            //从数据库里随机抽取一条成语。
            Idiom idi = _idioms.Aggregate().AppendStage<Idiom>("{$sample:{size:1}}").FirstOrDefault();
            //当idi不为null才运行。
            if (idi != null)
            {
                if (info == null)
                {
                    //这种情况说明当天的info还没有生成。
                    if (deftIdiom == null)
                    {
                        //若默认成语为空，则生成每日成语。
                        LaunchInfo ins = new LaunchInfo { Version = null, ArgsDic = null, Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = idi, IdiomsCount = 0, DateUT = dateL };
                        _launchInfo.InsertOne(ins);
                    }
                    else
                    {
                        //不为空则将默认成语写入当天的启动信息，方便以后查询记录。
                        LaunchInfo ins = new LaunchInfo { Version = null, ArgsDic = null, Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = deftIdiom, IdiomsCount = 0, DateUT = dateL };
                        _launchInfo.InsertOne(ins);
                    }
                }
                else
                {
                    //这种情况说明当天的info已经提前编辑好了，根据需要补全。
                    if (info.DailyIdiom == null)
                    {
                        if (deftIdiom == null)
                        {
                            //若默认成语为空，则生成每日成语。
                            UpdateDefinition<LaunchInfo> upd = Builders<LaunchInfo>.Update.Set("DailyIdiom", idi);
                            _launchInfo.UpdateOne(x => x.DateUT == dateL, upd);
                        }
                        else
                        {
                            //不为空则将默认成语写入当天的启动信息，方便以后查询记录。
                            UpdateDefinition<LaunchInfo> upd = Builders<LaunchInfo>.Update.Set("DailyIdiom", deftIdiom);
                            _launchInfo.UpdateOne(x => x.DateUT == dateL, upd);
                        }
                    }
                }
            }
        }

        //public long GetIdiomsCount()
        //{
        //    return _idioms.CountDocuments(new BsonDocument());
        //}

        public StandardReturn GetIdiomById(string id, string openId, int bson)
        {
            Idiom raw = _idioms.Find(x => x.Id == id).FirstOrDefault();
            if (raw == null)
                return new StandardReturn(20001);
            if (Config.EnableProtection && _editors.Find(x => x.OpenId == openId).FirstOrDefault() == null)
            {
                List<Definition> defs = raw.Definitions;
                List<Definition> modified = new List<Definition>();
                foreach (Definition def in defs)
                {
                    def.Source = "网络";
                    modified.Add(def);
                }
                raw.Definitions = modified;
            }
            if (bson == 1)
                return new StandardReturn(result: raw.ToBsonDocument().ToString());
            return new StandardReturn(result: raw);
        }

        public async Task AutoCollect(string name)
        {
            using var httpClient = new HttpClient();
            var res = await httpClient.GetStringAsync("https://v.juhe.cn/chengyu/query?word=" + name + "&key=59a83fe5879d3ca2ce0eef7183db90ad");
            JObject json = JObject.Parse(res);
            if (json["reason"].ToString() == "success")
            {
                Editor editor = _editors.Find(x => x.OpenId == "Idionline").FirstOrDefault();
                if (editor != null && Regex.IsMatch(name, "^[\u4e00-\u9fa5]+(，[\u4e00-\u9fa5]+)?$") && json["result"]["chengyujs"] != null)
                {
                    Definition def = new Definition { Source = "聚合数据", Text = json["result"]["chengyujs"].ToString().Replace("?", "？"), Examples = null, Addition = null, IsBold = false, Links = null };
                    List<Definition> defs = new List<Definition> { def };
                    long timeUT = DateTimeOffset.Now.ToUnixTimeSeconds();
                    char index = json["result"]["pinyin"].ToString().ToUpper().ToCharArray()[0];
                    if (index == 'Ā' || index == 'Á' || index == 'Ǎ' || index == 'À')
                        index = 'A';
                    else if (index == 'Ē' || index == 'É' || index == 'Ě' || index == 'È')
                        index = 'E';
                    else if (index == 'Ō' || index == 'Ó' || index == 'Ǒ' || index == 'Ò')
                        index = 'O';
                    _idioms.InsertOne(new Idiom { Name = name, Index = index, Pinyin = json["result"]["pinyin"].ToString(), Origin = null, Definitions = defs, Creator = editor.NickName, CreationTimeUT = timeUT, LastEditor = editor.NickName, UpdateTimeUT = timeUT });
                    var filter = Builders<Editor>.Filter.Eq("_id", editor.Id);
                    var update = Builders<Editor>.Update.Inc("EditCount", 1);
                    _editors.UpdateOne(filter, update);
                }
            }
        }

        public StandardReturn UpdateIdiom(string id, UpdateData data)
        {
            Editor editor = _editors.Find(x => x.OpenId == data.OpenId).FirstOrDefault();
            List<DefinitionUpdate> definitionUpdates = data.DefinitionUpdates;
            if (editor != null && !editor.IsLimited)
            {
                if (data.BsonStr != null)
                {
                    try
                    {
                        BsonDocument doc = BsonDocument.Parse(data.BsonStr.Replace("?", "？"));//不应允许有英文问号出现，不然小程序解析Json时会抛异常。
                        Idiom idi = BsonSerializer.Deserialize<Idiom>(doc);
                        if (Regex.IsMatch(idi.Name, "^[\u4e00-\u9fa5]+(，[\u4e00-\u9fa5]+)?$"))
                        {
                            idi.LastEditor = editor.NickName;
                            idi.UpdateTimeUT = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (_idioms.FindOneAndReplace(x => x.Id == id, idi) == null)
                                return new StandardReturn(20001);
                            //更新启动信息中的每日成语。
                            DateTimeOffset dateUT = DateTimeOffset.Now;
                            int hour = dateUT.Hour;
                            int min = dateUT.Minute;
                            int sec = dateUT.Second;
                            long dateL = dateUT.AddSeconds(-sec).AddMinutes(-min).AddHours(-hour).ToUnixTimeSeconds();
                            LaunchInfo deft = _launchInfo.Find(x => x.DateUT == DateTimeOffset.MinValue.ToUnixTimeSeconds()).FirstOrDefault();
                            LaunchInfo today = _launchInfo.Find(x => x.DateUT == dateL).FirstOrDefault();
                            if (deft != null && deft.DailyIdiom != null && deft.DailyIdiom.Id == idi.Id)
                            {
                                LaunchInfo upd = deft;
                                upd.DailyIdiom = idi;
                                _launchInfo.FindOneAndReplace(x => x.Id == upd.Id, upd);
                            }
                            if (today != null && today.DailyIdiom != null && today.DailyIdiom.Id == idi.Id)
                            {
                                LaunchInfo upd = today;
                                upd.DailyIdiom = idi;
                                _launchInfo.FindOneAndReplace(x => x.Id == upd.Id, upd);
                            }
                            //更新编辑者编辑次数。
                            var filter = Builders<Editor>.Filter.Eq("_id", editor.Id);
                            var update = Builders<Editor>.Update.Inc("EditCount", 1);
                            _editors.UpdateOne(filter, update);
                            return new StandardReturn(result: "成语已更新！");
                        }
                        return new StandardReturn(20002);
                    }
                    catch
                    {
                        return new StandardReturn(20002);
                    }
                }
                else if (definitionUpdates != null && definitionUpdates.Count > 0 && data.Name != null && Regex.IsMatch(data.Name, "^[\u4e00-\u9fa5]+(，[\u4e00-\u9fa5]+)?$"))
                {
                    Idiom target = _idioms.Find(x => x.Id == id).FirstOrDefault();
                    if (target == null)
                        return new StandardReturn(20001);
                    List<Definition> defs = target.Definitions;
                    for (int i = 0; i < definitionUpdates.Count; i++)
                    {
                        if (definitionUpdates[i].Source != null && definitionUpdates[i].Text != null && definitionUpdates[i].Source.Replace(" ", "") != "" && definitionUpdates[i].Text.Replace(" ", "") != "" && defs.Count > 0)
                        {
                            if (i < defs.Count)
                            {
                                defs[i].Source = definitionUpdates[i].Source.Replace("?", "？");
                                defs[i].Text = definitionUpdates[i].Text.Replace("?", "？");
                                if (definitionUpdates[i].Addition != null && definitionUpdates[i].Addition.Replace(" ", "") != "")
                                    defs[i].Addition = definitionUpdates[i].Addition.Replace("?", "？");
                                else
                                    defs[i].Addition = null;
                                defs[i].IsBold = definitionUpdates[i].IsBold;
                            }
                            else
                            {
                                string tmp = definitionUpdates[i].Addition;
                                if (tmp != null && tmp.Replace(" ", "") != "")
                                    tmp.Replace("?", "？");
                                defs.Add(new Definition { Source = definitionUpdates[i].Source.Replace("?", "？"), Text = definitionUpdates[i].Text.Replace("?", "？"), Examples = null, Addition = tmp, IsBold = definitionUpdates[i].IsBold, Links = null });
                            }
                        }
                        else
                            return new StandardReturn(20002);
                    }
                    string tmpPinyin = data.Pinyin;
                    if (data.Pinyin == null || data.Pinyin.Replace(" ", "") == "")
                        tmpPinyin = null;
                    string tmpOrigin = data.Origin;
                    if (data.Origin == null || data.Origin.Replace(" ", "") == "")
                        tmpOrigin = null;
                    UpdateDefinition<Idiom> update = Builders<Idiom>.Update.Set("Definitions", defs)
                                                                           .Set("Name", data.Name)
                                                                           .Set("Index", data.Index)
                                                                           .Set("Pinyin", tmpPinyin)
                                                                           .Set("Origin", tmpOrigin)
                                                                           .Set("ToBeCorrected", data.ToBeCorrected)
                                                                           .Set("LastEditor", editor.NickName)
                                                                           .Set("UpdateTimeUT", DateTimeOffset.Now.ToUnixTimeSeconds());
                    _idioms.FindOneAndUpdate(x => x.Id == id, update);
                    //更新启动信息中的每日成语。
                    DateTimeOffset dateUT = DateTimeOffset.Now;
                    int hour = dateUT.Hour;
                    int min = dateUT.Minute;
                    int sec = dateUT.Second;
                    long dateL = dateUT.AddSeconds(-sec).AddMinutes(-min).AddHours(-hour).ToUnixTimeSeconds();
                    LaunchInfo deft = _launchInfo.Find(x => x.DateUT == DateTimeOffset.MinValue.ToUnixTimeSeconds()).FirstOrDefault();
                    LaunchInfo today = _launchInfo.Find(x => x.DateUT == dateL).FirstOrDefault();
                    Idiom idi = _idioms.Find(x => x.Id == id).FirstOrDefault();
                    if (deft != null && deft.DailyIdiom != null && deft.DailyIdiom.Id == id)
                    {
                        LaunchInfo upd = deft;
                        upd.DailyIdiom = idi;
                        _launchInfo.FindOneAndReplace(x => x.Id == upd.Id, upd);
                    }
                    if (today != null && today.DailyIdiom != null && today.DailyIdiom.Id == idi.Id)
                    {
                        LaunchInfo upd = today;
                        upd.DailyIdiom = idi;
                        _launchInfo.FindOneAndReplace(x => x.Id == upd.Id, upd);
                    }
                    //更新编辑者编辑次数。
                    var filter2 = Builders<Editor>.Filter.Eq("_id", editor.Id);
                    var update2 = Builders<Editor>.Update.Inc("EditCount", 1);
                    _editors.UpdateOne(filter2, update2);
                    return new StandardReturn(result: "成语已更新！");
                }
                return new StandardReturn(20002);
            }
            return new StandardReturn(20003);
        }

        public StandardReturn DeleteIdiom(string id, string openId)
        {
            Editor editor = _editors.Find(x => x.OpenId == openId).FirstOrDefault();
            if (editor != null && !editor.IsLimited)
            {
                _idioms.FindOneAndDelete(x => x.Id == id);
                var filter = Builders<Editor>.Filter.Eq("_id", editor.Id);
                var update = Builders<Editor>.Update.Inc("EditCount", 1);
                _editors.UpdateOne(filter, update);
                DateTimeOffset dateUT = DateTimeOffset.Now;
                int hour = dateUT.Hour;
                int min = dateUT.Minute;
                int sec = dateUT.Second;
                long dateL = dateUT.AddSeconds(-sec).AddMinutes(-min).AddHours(-hour).ToUnixTimeSeconds();
                LaunchInfo today = _launchInfo.Find(x => x.DateUT == dateL).FirstOrDefault();
                if (today != null && today.DailyIdiom != null && today.DailyIdiom.Id == id)
                {
                    LaunchInfo upd = today;
                    upd.DailyIdiom = null;
                    _launchInfo.FindOneAndReplace(x => x.Id == upd.Id, upd);
                }
                return new StandardReturn(result: "已删除！");
            }
            return new StandardReturn(20003);
        }
        public StandardReturn GetListByStr(string str)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<Idiom> items;
            bool queryPrevDailyIdioms = false;//查询往日成语
            Dictionary<string, List<long>> kv = new Dictionary<string, List<long>>();
            //if (str == "我全都要")
            //{
            //    items = _idioms.Find(new BsonDocument()).Sort(Builders<Idiom>.Sort.Ascending("Name")).ToList();
            //}
            if (str == "试试手气")
                items = _idioms.Aggregate().AppendStage<Idiom>("{$sample:{size:5}}").ToList();
            else if (str == "有待订正")
                items = _idioms.Find(x => x.ToBeCorrected == true).ToList();
            else if (str == "往日成语")
            {
                //除去deft
                List<LaunchInfo> info = _launchInfo.Find(Builders<LaunchInfo>.Filter.Ne("DateUT", DateTimeOffset.MinValue.ToUnixTimeSeconds())).Sort(Builders<LaunchInfo>.Sort.Ascending("DateUT")).ToList();
                items = new List<Idiom>();
                queryPrevDailyIdioms = true;
                if (info.Count > 1)
                {
                    foreach (LaunchInfo itemInf in info)
                    {
                        if (itemInf.DailyIdiom != null)
                        {
                            if (!kv.ContainsKey(itemInf.DailyIdiom.Id + "_" + itemInf.DailyIdiom.Name))
                                kv.Add(itemInf.DailyIdiom.Id + "_" + itemInf.DailyIdiom.Name, new List<long> { itemInf.DateUT });
                            else
                                kv[itemInf.DailyIdiom.Id + "_" + itemInf.DailyIdiom.Name].Add(itemInf.DateUT);
                        }
                    }
                }
            }
            else
            {
                items = _idioms.Find(Builders<Idiom>.Filter.Regex("Name", new BsonRegularExpression(str))).Sort(Builders<Idiom>.Sort.Ascending("Name")).ToList();
            }
            if (queryPrevDailyIdioms)
            {
                foreach (var it in kv)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (long i in it.Value)
                    {
                        DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(i);
                        sb.Append(string.Format("{0:D}", time.ToLocalTime()) + "、");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    string[] strArr = it.Key.Split("_");
                    dic.Add(strArr[0], sb + "：" + strArr[1]);
                }
            }
            else
            {
                if (items.Count == 0 && str != "试试手气" && str != "有待订正" && str != "往日成语")
                    _ = AutoCollect(str);
                foreach (Idiom item in items)
                {
                    dic.Add(item.Id.ToString(), item.Name);
                }
            }
            StandardReturn rtn;
            if (dic.Count == 0)
                rtn = new StandardReturn(20001);
            else
                rtn = new StandardReturn(result: dic);
            return rtn;
        }
        public StandardReturn GetListById(string id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<Idiom> items = new List<Idiom>
            {
                _idioms.Find(x => x.Id == id).FirstOrDefault()
            };
            if (items.Count == 0)
                return new StandardReturn(20001);
            foreach (Idiom item in items)
            {
                dic.Add(item.Id.ToString(), item.Name);
            }
            return new StandardReturn(result: dic);
        }

        public StandardReturn GetListByIndex(char index)
        {
            if (char.IsLetter(char.ToUpper(index)))
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                List<Idiom> items = _idioms.Find(x => x.Index == index).Sort(Builders<Idiom>.Sort.Ascending("Name")).ToList();
                if (items.Count == 0)
                    return new StandardReturn(20001);
                foreach (Idiom item in items)
                {
                    dic.Add(item.Id.ToString(), item.Name);
                }
                return new StandardReturn(result: dic);
            }
            return new StandardReturn(20001);
        }

        public string IgnoreTunes(string str)
        {
            return str.Replace("a", "(a|ā|á|ǎ|à)")
                      .Replace("ā", "(a|ā|á|ǎ|à)")
                      .Replace("á", "(a|ā|á|ǎ|à)")
                      .Replace("ǎ", "(a|ā|á|ǎ|à)")
                      .Replace("à", "(a|ā|á|ǎ|à)")
                      .Replace("o", "(o|ō|ó|ǒ|ò)")
                      .Replace("ō", "(o|ō|ó|ǒ|ò)")
                      .Replace("ó", "(o|ō|ó|ǒ|ò)")
                      .Replace("ǒ", "(o|ō|ó|ǒ|ò)")
                      .Replace("ò", "(o|ō|ó|ǒ|ò)")
                      .Replace("e", "(e|ē|é|ě|è)")
                      .Replace("ē", "(e|ē|é|ě|è)")
                      .Replace("é", "(e|ē|é|ě|è)")
                      .Replace("ě", "(e|ē|é|ě|è)")
                      .Replace("è", "(e|ē|é|ě|è)")
                      .Replace("i", "(i|ī|í|ǐ|ì)")
                      .Replace("ī", "(i|ī|í|ǐ|ì)")
                      .Replace("í", "(i|ī|í|ǐ|ì)")
                      .Replace("ǐ", "(i|ī|í|ǐ|ì)")
                      .Replace("ì", "(i|ī|í|ǐ|ì)")
                      .Replace("u", "(u|ū|ú|ǔ|ù)")
                      .Replace("ū", "(u|ū|ú|ǔ|ù)")
                      .Replace("ú", "(u|ū|ú|ǔ|ù)")
                      .Replace("ǔ", "(u|ū|ú|ǔ|ù)")
                      .Replace("ù", "(u|ū|ú|ǔ|ù)")
                      .Replace("ü", "(ü|ǖ|ǘ|ǚ|ǜ)")
                      .Replace("ǖ", "(ü|ǖ|ǘ|ǚ|ǜ)")
                      .Replace("ǘ", "(ü|ǖ|ǘ|ǚ|ǜ)")
                      .Replace("ǚ", "(ü|ǖ|ǘ|ǚ|ǜ)")
                      .Replace("ǜ", "(ü|ǖ|ǘ|ǚ|ǜ)");
        }

        public StandardReturn PlaySolitaire(string str)
        {
            Idiom target = _idioms.Find(x => x.Name == str).FirstOrDefault();
            if (target != null && target.Pinyin != null)
            {
                string last = IgnoreTunes(target.Pinyin.Split(" ").Last());
                List<Idiom> items = _idioms.Find(Builders<Idiom>.Filter.Regex("Pinyin", new BsonRegularExpression("^" + last + " "))).ToList();
                if (items.Count - 1 >= 0)
                {
                    Random rd = new Random();
                    int index = rd.Next(0, items.Count - 1);
                    return new StandardReturn(result: items[index].Name);
                }
            }
            else
            {
                List<Idiom> items = _idioms.Find(Builders<Idiom>.Filter.Regex("Name", new BsonRegularExpression("^" + str.Substring(str.Length - 1, 1) + "[\u4e00-\u9fa5]{3}$"))).ToList();
                if (items.Count - 1 >= 0)
                {
                    Random rd = new Random();
                    int index = rd.Next(0, items.Count - 1);
                    return new StandardReturn(result: items[index].Name);
                }
            }
            return new StandardReturn(20001);
        }

        public StandardReturn GetLaunchInf(long date, string openId)
        {
            bool proed = false;
            LaunchInfo deft = _launchInfo.Find(x => x.DateUT == DateTimeOffset.MinValue.ToUnixTimeSeconds()).FirstOrDefault();
            LaunchInfo current = _launchInfo.Find(x => x.DateUT == date).FirstOrDefault();
            if (deft == null)
            {
                deft = new LaunchInfo { Version = null, ArgsDic = null, Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = null, IdiomsCount = 0, DateUT = DateTimeOffset.MinValue.ToUnixTimeSeconds() };
                _launchInfo.InsertOne(deft);
            }
            if (_editors.Find(x => x.OpenId == openId).FirstOrDefault() == null)
                proed = true;
            return new StandardReturn(result: MergeLI(current, deft, proed));
        }

        public LaunchInfo MergeLI(LaunchInfo current, LaunchInfo deft, bool proed)
        {
            if (current == null)
                current = new LaunchInfo { Version = null, ArgsDic = null, Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = null, IdiomsCount = 0, DateUT = 0 };
            //将当前启动信息与默认启动信息合并并返回。
            current.Version = version.ToString();
            current.ArgsDic = deft.ArgsDic;
            if (current.IdiomsCount == 0 && deft.IdiomsCount == 0)
                current.IdiomsCount = _idioms.CountDocuments(new BsonDocument());
            else if (current.IdiomsCount == 0)
                current.IdiomsCount = deft.IdiomsCount;
            if (current.Text == null)
                current.Text = deft.Text;
            if (current.ThemeColor == null)
                current.ThemeColor = deft.ThemeColor;
            if (current.LogoUrl == null)
                current.LogoUrl = deft.LogoUrl;
            if (current.DisableAds == false)
                current.DisableAds = deft.DisableAds;
            if (current.DailyIdiom == null)
                current.DailyIdiom = deft.DailyIdiom;
            if (Config.EnableProtection && proed)
            {
                Idiom raw = current.DailyIdiom;
                List<Definition> defs = raw.Definitions;
                List<Definition> modified = new List<Definition>();
                foreach (Definition def in defs)
                {
                    def.Source = "网络";
                    modified.Add(def);
                }
                raw.Definitions = modified;
                current.DailyIdiom = raw;
            }
            return current;
        }

        public StandardReturn RegisterEdi(string nickName, string openId)
        {
            if (_editors.Find(x => x.OpenId == openId).FirstOrDefault() == null && _editors.Find(x => x.NickName == nickName).FirstOrDefault() == null)
            {
                if (openId != null && nickName != null && openId.Replace(" ", "") != "" && nickName.Replace(" ", "") != "")
                {
                    _editors.InsertOne(new Editor { OpenId = openId, NickName = nickName.Replace("?", "？"), RegistrationTimeUT = DateTimeOffset.Now.ToUnixTimeSeconds() });
                    return new StandardReturn(result: "注册成功！");
                }
                else
                    return new StandardReturn(20002);
            }
            return new StandardReturn(20004);
        }
    }
}