﻿using Idionline.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Idionline
{
    public class DataAccess
    {
        IMongoDatabase _db;
        IMongoCollection<Idiom> _idioms;
        IMongoCollection<LaunchInf> _launchInf;
        public DataAccess()
        {
            _db = new MongoClient("mongodb://localhost:27017").GetDatabase("IdionlineDB");
            _idioms = _db.GetCollection<Idiom>("Idioms");
            _launchInf = _db.GetCollection<LaunchInf>("LaunchInf");
        }
        #region 测试用的生成代码，方便以后再瞎折腾就先不删，注释掉。
        //public string GenerateIdiom()
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>
        //    {
        //        { "5beghawgsagsaga7eb855e3e94", "66hhh" },
        //        { "5bebec2a2hgbhghhhhhhhh855e3e94", "6gggggh" }
        //    };
        //    Definition def = new Definition { Text = "hahaha", Addition = "666", IsEmphasis = false, Source = "hhh", Links = dic };
        //    Definition def2 = new Definition { Text = "haa", Addition = "345sgsdgsdgc6", IsEmphasis = false, Source = "hhh", Links = dic };
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
        //    _launchInf.InsertOne(new LaunchInf { Text = "23333", DailyIdiomId = "h3h3", /*DailyIdiomName = "6666",*/  MainColor = null, LogoUrl = null, DisableAds = false, /*FloatEasterEggs = i, */DateUT = DateTimeOffset.MinValue.ToUnixTimeSeconds() });
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
            Idiom idi = _idioms.Aggregate().AppendStage<Idiom>("{ $sample: { size: 1 } }").FirstOrDefault();
            //当idi不为null才运行。
            if (idi != null)
            {
                if (inf == null)
                {
                    //这种情况说明当天的inf还没有生成。
                    if (deftIdiom == null)
                    {
                        //若默认成语为空，则生成每日成语。
                        LaunchInf ins = new LaunchInf { Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = idi, IdiomsCount = 0, DateUT = dateL };
                        _launchInf.InsertOne(ins);
                    }
                    else
                    {
                        //不为空则将默认成语写入当天的启动信息，方便以后查询记录。
                        LaunchInf ins = new LaunchInf { Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = deftIdiom, IdiomsCount = 0, DateUT = dateL };
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

        public Dictionary<string, string> GetListByStr(string str)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<Idiom> items;
            if (str == "我全都要")
            {
                items = _idioms.Find(new BsonDocument()).Sort(Builders<Idiom>.Sort.Ascending("Name")).ToList();
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
                current = new LaunchInf { Text = null, ThemeColor = null, LogoUrl = null, DisableAds = false, DailyIdiom = null, IdiomsCount = _idioms.CountDocuments(new BsonDocument()), DateUT = 0 };
            }
            //将当前启动信息与默认启动信息合并并返回。
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
            if (current.IdiomsCount <= 0)
            {
                if (deft.IdiomsCount <= 0)
                {
                    current.IdiomsCount = _idioms.CountDocuments(new BsonDocument());
                }
                else
                {
                    current.IdiomsCount = deft.IdiomsCount;
                }
            }
            return current;
        }
    }
}