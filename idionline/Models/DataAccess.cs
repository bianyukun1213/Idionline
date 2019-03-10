using Idionline.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Idionline
{
    public class DataAccess
    {
        IMongoDatabase _db;
        IMongoCollection<Idiom> _idioms;
        IMongoCollection<LaunchInf> _launchInf;
        IMongoCollection<Editor> _editors;
        Version version = Assembly.GetEntryAssembly().GetName().Version;
        public DataAccess()
        {
            _db = new MongoClient("mongodb://localhost:27017").GetDatabase("IdionlineDB");
            _idioms = _db.GetCollection<Idiom>("Idioms");
            _launchInf = _db.GetCollection<LaunchInf>("LaunchInf");
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
        public void AddIdiom2Db()
        {
            DateTimeOffset dateUT = DateTimeOffset.Now;
            int hour = dateUT.Hour;
            int min = dateUT.Minute;
            int sec = dateUT.Second;
            long dateL = dateUT.AddSeconds(-sec).AddMinutes(-min).AddHours(-hour).ToUnixTimeSeconds();
            //默认的每日成语。
            Idiom deftIdiom = _launchInf.Find(x => x.DateUT == DateTimeOffset.MinValue.ToUnixTimeSeconds()).FirstOrDefault().DailyIdiom;
            LaunchInf inf = _launchInf.Find(x => x.DateUT == dateL).FirstOrDefault();
            //从数据库里随机抽取一条成语。
            Idiom idi = _idioms.Aggregate().AppendStage<Idiom>("{$sample:{size:1}}").FirstOrDefault();
            //当idi不为null才运行。
            if (idi != null)
            {
                if (inf == null)
                {
                    //这种情况说明当天的inf还没有生成。
                    if (deftIdiom == null)
                    {
                        //若默认成语为空，则生成每日成语。
                        LaunchInf ins = new LaunchInf { Version = null, ArgsDic = null, Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = idi, IdiomsCount = 0, DateUT = dateL };
                        _launchInf.InsertOne(ins);
                    }
                    else
                    {
                        //不为空则将默认成语写入当天的启动信息，方便以后查询记录。
                        LaunchInf ins = new LaunchInf { Version = null, ArgsDic = null, Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = deftIdiom, IdiomsCount = 0, DateUT = dateL };
                        _launchInf.InsertOne(ins);
                    }
                }
                else
                {
                    //这种情况说明当天的inf已经提前编辑好了，根据需要补全。
                    if (inf.DailyIdiom == null)
                    {
                        if (deftIdiom == null)
                        {
                            //若默认成语为空，则生成每日成语。
                            UpdateDefinition<LaunchInf> upd = Builders<LaunchInf>.Update.Set("DailyIdiom", idi);
                            _launchInf.UpdateOne(x => x.DateUT == dateL, upd);
                        }
                        else
                        {
                            //不为空则将默认成语写入当天的启动信息，方便以后查询记录。
                            UpdateDefinition<LaunchInf> upd = Builders<LaunchInf>.Update.Set("DailyIdiom", deftIdiom);
                            _launchInf.UpdateOne(x => x.DateUT == dateL, upd);
                        }
                    }
                }
            }
        }

        public long GetIdiomsCount()
        {
            return _idioms.CountDocuments(new BsonDocument());
        }

        public Idiom GetIdiomById(ObjectId id)
        {
            return _idioms.Find(x => x.Id == id).FirstOrDefault();
        }

        public string UpdateIdiom(ObjectId id, UpdateData data)
        {
            Editor editor = _editors.Find(x => x.OpenId == data.OpenId).FirstOrDefault();
            List<DefinitionUpdate> updates = data.Updates;
            if (editor != null && updates != null && updates.Count > 0)
            {
                List<Definition> defs = _idioms.Find(x => x.Id == id).FirstOrDefault().Definitions;
                //for (int i = 0; i < defs.Count; i++)
                //{
                //    if (i < updates.Count)
                //    {
                //        defs[i].Text = updates[i];
                //    }
                //}
                for (int i = 0; i < updates.Count; i++)
                {
                    if (updates[i].Source != null && updates[i].Text != null && updates[i].Source != "" && updates[i].Text != "")
                    {
                        if (i < defs.Count)
                        {
                            defs[i].Source = updates[i].Source;
                            defs[i].Text = updates[i].Text;
                        }
                        else
                        {
                            defs.Add(new Definition { Source = updates[i].Source, Text = updates[i].Text, Examples = null, Addition = null, IsBold = false, Links = null });
                        }
                    }
                    else
                    {
                        return "无法进行更新操作！";
                    }
                }
                var filter = Builders<Idiom>.Filter.Eq("_id", id);
                var update = Builders<Idiom>.Update.Set("Definitions", defs).Set("LastEditor", editor.NickName).Set("UpdateTimeUT", DateTimeOffset.Now.ToUnixTimeSeconds());
                _idioms.UpdateOne(filter, update);
                return "释义已更新！";
            }
            else
            {
                return "无法进行更新操作！";
            }
        }

        public string DeleteIdiom(ObjectId id, string openId)
        {
            Editor editor = _editors.Find(x => x.OpenId == openId).FirstOrDefault();
            if (editor != null)
            {
                _idioms.FindOneAndDelete(x => x.Id == id);
                return "已删除！";
            }
            return "无法进行删除操作！";
        }
        public Dictionary<string, string> GetListByStr(string str)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<Idiom> items;
            //if (str == "我全都要")
            //{
            //    items = _idioms.Find(new BsonDocument()).Sort(Builders<Idiom>.Sort.Ascending("Name")).ToList();
            //}
            if (str == "试试手气")
            {
                items = _idioms.Aggregate().AppendStage<Idiom>("{$sample:{size:1}}").ToList();
            }
            else
            {
                items = _idioms.Find(Builders<Idiom>.Filter.Regex("Name", new BsonRegularExpression(str))).Sort(Builders<Idiom>.Sort.Ascending("Name")).ToList();
            }
            foreach (var item in items)
            {
                dic.Add(item.Id.ToString(), item.Name);
            }
            return dic;
        }

        public Dictionary<string, string> GetListByIndex(char index)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<Idiom> items = _idioms.Find(x => x.Index == index).Sort(Builders<Idiom>.Sort.Ascending("Name")).ToList();
            foreach (var item in items)
            {
                dic.Add(item.Id.ToString(), item.Name);
            }
            return dic;
        }

        public string Solitaire(string str)
        {
            List<Idiom> items = _idioms.Find(Builders<Idiom>.Filter.Regex("Name", new BsonRegularExpression("^" + str.Substring(str.Length - 1, 1)))).ToList();
            if (items.Count - 1 >= 0)
            {
                Random rd = new Random();
                int index = rd.Next(0, items.Count - 1);
                return items[index].Name;
            }
            return null;
        }

        public LaunchInf GetLaunchInf(long date)
        {
            LaunchInf deft = _launchInf.Find(x => x.DateUT == DateTimeOffset.MinValue.ToUnixTimeSeconds()).FirstOrDefault();
            LaunchInf current = _launchInf.Find(x => x.DateUT == date).FirstOrDefault();
            return MergeLI(current, deft);
        }

        LaunchInf MergeLI(LaunchInf current, LaunchInf deft)
        {
            if (current == null)
            {
                current = new LaunchInf { Version = null, ArgsDic = null, Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = null, IdiomsCount = 0, DateUT = 0 };
            }
            //将当前启动信息与默认启动信息合并并返回。
            current.Version = version.ToString();
            current.ArgsDic = deft.ArgsDic;
            current.IdiomsCount = _idioms.CountDocuments(new BsonDocument());
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
            return current;
        }

        public string RegisterEdi(EditorRegisterData ediDt)
        {
            if (_editors.Find(x => x.OpenId == ediDt.OpenId).FirstOrDefault() == null && _editors.Find(x => x.NickName == ediDt.NickName).FirstOrDefault() == null)
            {
                if (ediDt.OpenId != null && ediDt.NickName != null && ediDt.OpenId != "" && ediDt.NickName != "")
                {
                    _editors.InsertOne(new Editor { OpenId = ediDt.OpenId, NickName = ediDt.NickName, RegisterTimeUT = DateTimeOffset.Now.ToUnixTimeSeconds() });
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
