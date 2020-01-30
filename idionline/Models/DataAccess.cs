using Idionline.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

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

        public Idiom GetIdiomById(string id, string openId)
        {
            Idiom raw = _idioms.Find(x => x.Id == id).FirstOrDefault();
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
            return raw;
        }

        public string CreateIdiom(JuheIdiomData dt)
        {
            Editor editor = _editors.Find(x => x.OpenId == dt.OpenId).FirstOrDefault();
            if (editor != null)
            {
                try
                {
                    if (Regex.IsMatch(dt.Name, "^[\u4e00-\u9fa5]+(，[\u4e00-\u9fa5]+)?$") && dt.DefText != null && dt.DefText != "")
                    {
                        Definition def = new Definition { Source = dt.Source, Text = dt.DefText.Replace("?", "？"), Examples = null, Addition = null, IsBold = false, Links = null };
                        List<Definition> defs = new List<Definition> { def };
                        long timeUT = DateTimeOffset.Now.ToUnixTimeSeconds();
                        char index = dt.Pinyin.ToUpper().ToCharArray()[0];
                        if (index == 'Ā' || index == 'Á' || index == 'Ǎ' || index == 'À')
                        {
                            index = 'A';
                        }
                        else if (index == 'Ē' || index == 'É' || index == 'Ě' || index == 'È')
                        {
                            index = 'E';
                        }
                        else if (index == 'Ō' || index == 'Ó' || index == 'Ǒ' || index == 'Ò')
                        {
                            index = 'O';
                        }
                        _idioms.InsertOne(new Idiom { Name = dt.Name, Index = index, Pinyin = dt.Pinyin.Replace(" ", ""), Origin = null, Definitions = defs, Creator = editor.NickName, CreateTimeUT = timeUT, LastEditor = editor.NickName, UpdateTimeUT = timeUT });
                        var filter = Builders<Editor>.Filter.Eq("_id", editor.Id);
                        var update = Builders<Editor>.Update.Inc("EditCount", 1);
                        _editors.UpdateOne(filter, update);
                        return "已自动收录！";
                    }

                }
                catch (Exception)
                {

                }
            }
            return "自动收录失败！";
        }

        public string UpdateIdiom(string id, UpdateData data)
        {
            Editor editor = _editors.Find(x => x.OpenId == data.OpenId).FirstOrDefault();
            List<DefinitionUpdate> updates = data.Updates;
            if (editor != null && !editor.IsLimited)
            {
                if (data.BsonMode)
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
                            {
                                return "无法进行更新操作！";
                            }
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
                            return "成语已更新！";
                        }

                    }
                    catch (Exception)
                    {
                        return "无法进行更新操作！";
                    }
                }
                else if (updates != null && updates.Count > 0)
                {
                    try
                    {
                        List<Definition> defs = _idioms.Find(x => x.Id == id).FirstOrDefault().Definitions;
                        for (int i = 0; i < updates.Count; i++)
                        {
                            if (updates[i].Source != null && updates[i].Text != null && updates[i].Source != "" && updates[i].Text != "" && defs.Count > 0)
                            {
                                if (i < defs.Count)
                                {
                                    defs[i].Source = updates[i].Source.Replace("?", "？");
                                    defs[i].Text = updates[i].Text.Replace("?", "？");
                                    if (updates[i].Addition != null && updates[i].Addition != "")
                                    {
                                        defs[i].Addition = updates[i].Addition.Replace("?", "？");
                                    }
                                    else
                                    {
                                        defs[i].Addition = null;
                                    }
                                    defs[i].IsBold = updates[i].IsBold;
                                }
                                else
                                {
                                    string tmp = updates[i].Addition;
                                    if (tmp != null && tmp != "")
                                    {
                                        tmp.Replace("?", "？");
                                    }
                                    defs.Add(new Definition { Source = updates[i].Source.Replace("?", "？"), Text = updates[i].Text.Replace("?", "？"), Examples = null, Addition = tmp, IsBold = updates[i].IsBold, Links = null });
                                }
                            }
                            else
                            {
                                return "无法进行更新操作！";
                            }
                        }
                        var filter = Builders<Idiom>.Filter.Eq("_id", new ObjectId(id));
                        var update = Builders<Idiom>.Update.Set("Definitions", defs).Set("LastEditor", editor.NickName).Set("UpdateTimeUT", DateTimeOffset.Now.ToUnixTimeSeconds());
                        _idioms.UpdateOne(filter, update);
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
                        return "释义已更新！";
                    }
                    catch (Exception)
                    {
                        return "无法进行更新操作！";
                    }
                }
            }
            return "无法进行更新操作！";
        }

        public string DeleteIdiom(string id, string openId)
        {
            Editor editor = _editors.Find(x => x.OpenId == openId).FirstOrDefault();
            if (editor != null && !editor.IsLimited)
            {
                _idioms.FindOneAndDelete(x => x.Id == id);
                var filter = Builders<Editor>.Filter.Eq("_id", editor.Id);
                var update = Builders<Editor>.Update.Inc("EditCount", 1);
                _editors.UpdateOne(filter, update);
                return "已删除！";
            }
            return "无法进行删除操作！";
        }
        public Dictionary<string, string> GetListByStr(string str)
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
            {
                items = _idioms.Aggregate().AppendStage<Idiom>("{$sample:{size:1}}").ToList();
            }
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
                            {
                                kv.Add(itemInf.DailyIdiom.Id + "_" + itemInf.DailyIdiom.Name, new List<long> { itemInf.DateUT });
                            }
                            else
                            {
                                kv[itemInf.DailyIdiom.Id + "_" + itemInf.DailyIdiom.Name].Add(itemInf.DateUT);
                            }
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
                foreach (Idiom item in items)
                {
                    dic.Add(item.Id.ToString(), item.Name);
                }
            }
            return dic;
        }
        public Dictionary<string, string> GetListById(string id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<Idiom> items = new List<Idiom>
            {
                _idioms.Find(x => x.Id == id).FirstOrDefault()
            };
            foreach (Idiom item in items)
            {
                dic.Add(item.Id.ToString(), item.Name);
            }
            return dic;
        }

        public Dictionary<string, string> GetListByIndex(char index)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<Idiom> items = _idioms.Find(x => x.Index == index).Sort(Builders<Idiom>.Sort.Ascending("Name")).ToList();
            foreach (Idiom item in items)
            {
                dic.Add(item.Id.ToString(), item.Name);
            }
            return dic;
        }

        public string Solitaire(string str)
        {
            List<Idiom> items = _idioms.Find(Builders<Idiom>.Filter.Regex("Name", new BsonRegularExpression("^" + str.Substring(str.Length - 1, 1) + "[\u4e00-\u9fa5]{3}$"))).ToList();
            if (items.Count - 1 >= 0)
            {
                Random rd = new Random();
                int index = rd.Next(0, items.Count - 1);
                return items[index].Name;
            }
            return null;
        }

        public LaunchInfo GetLaunchInf(long date, string openId)
        {
            bool proed = false;
            LaunchInfo deft = _launchInfo.Find(x => x.DateUT == DateTimeOffset.MinValue.ToUnixTimeSeconds()).FirstOrDefault();
            LaunchInfo current = _launchInfo.Find(x => x.DateUT == date).FirstOrDefault();
            if (_editors.Find(x => x.OpenId == openId).FirstOrDefault() == null)
                proed = true;
            return MergeLI(current, deft, proed);
        }

        public LaunchInfo MergeLI(LaunchInfo current, LaunchInfo deft, bool proed)
        {
            if (current == null)
            {
                current = new LaunchInfo { Version = null, ArgsDic = null, Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = null, IdiomsCount = 0, DateUT = 0 };
            }
            //将当前启动信息与默认启动信息合并并返回。
            current.Version = version.ToString();
            current.ArgsDic = deft.ArgsDic;
            if (current.IdiomsCount == 0 && deft.IdiomsCount == 0)
            {
                current.IdiomsCount = _idioms.CountDocuments(new BsonDocument());
            }
            else if (current.IdiomsCount == 0)
            {
                current.IdiomsCount = deft.IdiomsCount;
            }
            if (current.Text == null)
            {
                current.Text = deft.Text;
            }
            if (current.ThemeColor == null)
            {
                current.ThemeColor = deft.ThemeColor;
            }
            if (current.LogoUrl == null)
            {
                current.LogoUrl = deft.LogoUrl;
            }
            if (current.DisableAds == false)
            {
                current.DisableAds = deft.DisableAds;
            }
            if (current.DailyIdiom == null)
            {
                current.DailyIdiom = deft.DailyIdiom;
            }
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

        public string RegisterEdi(string nickName, string openId)
        {
            if (_editors.Find(x => x.OpenId == openId).FirstOrDefault() == null && _editors.Find(x => x.NickName == nickName).FirstOrDefault() == null)
            {
                if (openId != null && nickName != null && openId != "" && nickName != "")
                {
                    _editors.InsertOne(new Editor { OpenId = openId, NickName = nickName.Replace("?", "？"), RegisterTimeUT = DateTimeOffset.Now.ToUnixTimeSeconds() });
                    return "注册成功！";
                }
                else
                {
                    return "注册失败！";
                }
            }
            return "您已经注册过！";
        }
    }
}
