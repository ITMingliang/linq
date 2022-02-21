using System;
using System.Collections.Generic;
using System.Linq;

namespace EF_Demo_05
{
    public class Program
    {
        static void Main(string[] args)
        {

            var master = new List<MartialArtsMaster>()
            {
                new MartialArtsMaster(){Id=1,Name="黄蓉",age=18,Menpai="丐帮",Kunfu="打狗棒法",Level=9},
                new MartialArtsMaster(){Id=2,Name="洪七公",age=70,Menpai="丐帮",Kunfu="打狗棒法",Level=10},
                new MartialArtsMaster(){Id=3,Name="郭靖",age=22,Menpai="丐帮",Kunfu="降龙十八掌",Level=10},
                new MartialArtsMaster(){Id=4,Name="任我行",age=50,Menpai="明教",Kunfu="吸星大法",Level=1},
                new MartialArtsMaster(){Id=5,Name="东方不败",age=35,Menpai="明教",Kunfu="葵花宝典",Level=10},
                new MartialArtsMaster(){Id=6,Name="林平之",age=25,Menpai="华山",Kunfu="葵花宝典",Level=7},
                new MartialArtsMaster(){Id=7,Name="岳不群",age=59,Menpai="华山",Kunfu="葵花宝典",Level=10}

            };

            var kongfu = new List<Kongfu>()
            {
                new Kongfu(){KongfuId=1,KongfuName="打狗棒法",Lethality=90},
                new Kongfu(){KongfuId=2,KongfuName="降龙十八掌",Lethality=95},
                new Kongfu(){KongfuId=3,KongfuName="葵花宝典",Lethality=90},
                new Kongfu(){KongfuId=4,KongfuName="吸星大法",Lethality=100}
            };

            //实例一：
            var GaiBangMaster = from m in master
                                where m.Level > 8 && m.Menpai == "丐帮"
                                select m;

            string GaiBangMasterResult = "查询丐帮中功夫等级高于8级的大侠：\n";
            Console.WriteLine(GaiBangMasterResult);
            foreach (var item in GaiBangMaster)
            {
                GaiBangMasterResult = item.Id + "\t" + item.Name + "\t" + item.age + "\t" + item.Menpai + "\t" + item.Kunfu;
                Console.WriteLine(GaiBangMasterResult);
            }

            //实例二：
            var masterKongfu = from m in master
                               from k in kongfu
                               where k.Lethality > 80 && m.Kunfu == k.KongfuName
                               orderby m.Level
                               select m.Id + "\t" + m.Name + "\t" + m.age + "\t" + m.Menpai + "\t" + m.Kunfu + "\t" + m.Level + "\t";


            string MasterKongfuResult = "查询杀伤力大于90的大侠：\n";
            Console.WriteLine(MasterKongfuResult);
            foreach (var item in masterKongfu)
            {
                MasterKongfuResult = item.ToString() + "\n";
                Console.WriteLine(MasterKongfuResult);
            }



            //实例三
            int i = 0;
            var topMaster1 = master.SelectMany(k => kongfu, (m, k) => new { mt = m, kf = k })
                .Where(x => x.mt.Kunfu == x.kf.KongfuName)
                .OrderByDescending(m => m.mt.Level * m.kf.Lethality)
                .ThenBy(m => m.mt.age)
                .ThenBy(m => m.mt.Name)
                .Select(m => m.mt.Id + "\t" + m.mt.Name + "\t" + m.mt.age + "\t" + m.mt.Menpai + "\t" + m.mt.Kunfu + "\t" + m.mt.Level + "\t" + m.mt.Level * m.kf.Lethality + "\t" + (++i));
            var topMaster = from m in master
                            from k in kongfu
                            where k.KongfuName == m.Kunfu
                            orderby m.Level * k.Lethality descending, m.age, m.Name
                            select m.Id + "\t" + m.Name + "\t" + m.age + "\t" + m.Menpai + "\t" + m.Level + "\t" + m.Level * k.Lethality + "\t" + (++i);
            string topMasterResult = "武林排行榜：\n";
            //Console.WriteLine(topMasterResult);
            foreach (var item in topMaster1)
            {
                topMasterResult+=item.ToString() + "\n";
            }

            Console.WriteLine(topMasterResult);

            //实例四：

            //通过对象关联，返对象的高手杀伤力
            i = 0;
            var masterTop = master.Where(x => x.Level > 8)
                .OrderByDescending(x => x.Level)
                .Select(x => new { Id = x.Id, Name = x.Name, masterKonngfu = x.Kunfu, Level = x.Level, Top = (++i) });
            i = 0;
            var kongfuTop = from k in kongfu
                            where k.Lethality > 90
                            orderby k.Lethality descending
                            select new { KongfuId = k.KongfuId, KongfuName = k.KongfuName, Lethality = k.Lethality, kongfuTop = (++i) };
           
            var masterLethalityTop = from m in masterTop
                                     join k in kongfuTop on m.masterKonngfu equals k.KongfuName
                                     orderby m.Level * k.Lethality descending
                                     select new { Id = m.Id, Name = m.Name, Kongfu = m.masterKonngfu, Level = m.Level, Kill = m.Level * k.Lethality };

            var masterLethalityTopMethod = masterTop.Join(kongfuTop, 
                m => m.masterKonngfu, k => k.KongfuName, 
                (m, k) => new { Id = m.Id, Name = m.Name, Kongfu = m.masterKonngfu, Level = m.Level, Kill = m.Level * k.Lethality })
                .OrderByDescending(m=>m.Kill);


            string s = "通过对象关联，返回新的对象的高手杀伤力:";
            Console.WriteLine(s);
            foreach (var ma in masterLethalityTop)
            {
                Console.WriteLine(ma.Id + "\t" + ma.Name + "\t" + ma.Kongfu + "\t" + ma.Level + "\t" + ma.Kill + "\n");
            }


            s = "通过对象关联，返回新的对象的高手杀伤力:";
            Console.WriteLine(s);
            foreach (var ma in masterLethalityTopMethod)
            {
                Console.WriteLine(ma.Id + "\t" + ma.Name + "\t" + ma.Kongfu + "\t" + ma.Level + "\t" + ma.Kill + "\n");
            }


            //实例五：
            master.Add(new MartialArtsMaster() { Id = 8, Name = "令狐冲", age = 18, Menpai = "华山", Kunfu = "孤独九剑", Level = 10 });
            master.Add(new MartialArtsMaster() { Id = 9, Name = "梅超风", age = 18, Menpai = "桃花岛", Kunfu = "九阴真经", Level = 8 });
            master.Add(new MartialArtsMaster() { Id = 10, Name = "黄药师", age = 18, Menpai = "丐帮", Kunfu = "弹指神功", Level = 10 });
            master.Add(new MartialArtsMaster() { Id = 11, Name = "风清扬", age = 18, Menpai = "华山", Kunfu = "孤独九剑", Level = 10});

            kongfu.Add(new Kongfu() { KongfuId = 4, KongfuName = "打狗棒法", Lethality = 100 });
            kongfu.Add(new Kongfu() { KongfuId = 5, KongfuName = "九阴真经", Lethality = 100 });
            kongfu.Add(new Kongfu() { KongfuId = 6, KongfuName = "弹指神功", Lethality = 100 });

            Console.WriteLine( "武学分组：");
            var masterItem = from k in kongfu
                             join m in master on k.KongfuName equals m.Kunfu
                             into groups
                             orderby groups.Count() descending
                             select new { KongfuId = k.KongfuId, KongfuName = k.KongfuName, Lethality = k.Lethality, Count = groups.Count() };
            var masterItem2 = kongfu.GroupJoin(master, k => k.KongfuName, m => m.Kunfu, (k, m) => new { k.KongfuId, k.KongfuName, k.Lethality, Count = m.Count() })
                .OrderByDescending(k=>k.Count);

            foreach (var item in masterItem)
            {
                Console.WriteLine(item.KongfuId + "\t" + item.KongfuName + "\t" + item.Lethality + "\t" + item.Count);
            }


            //使用join和GroupJoin的区别：
            Console.WriteLine("使用join和GroupJoin的区别:");
            var JoinOrGroupJoin = from k in kongfu
                                  join m in master on k.KongfuName equals m.Kunfu
                                  orderby k.KongfuId
                                  select new { KongfuId = k.KongfuId, KongfuName = k.KongfuName, Lethality = k.Lethality, master = m.Name };

            var JoinOrGroupJoin2 = kongfu.GroupJoin(master,k=>k.KongfuName,m=>m.Kunfu,(k,m)=>new {k.KongfuId,k.KongfuName,k.Lethality,Count=m.Count() }) 
                .OrderBy(k => k.KongfuId);

            foreach (var item in JoinOrGroupJoin)
            {
                Console.WriteLine(item.KongfuId + "\t" + item.KongfuName + "\t" + item.Lethality + "\t" + item.master);
            }


            //分组
            var GroupItems = from m in master
                             group m by m.Menpai into g
                             orderby g.Key
                             select new
                             {
                                 Menpai = g.Key,
                                 Count = g.Count()
                             };

            var  GroupItems1=master.GroupBy(m=>m.Menpai,(k,m)=>new { MenPai=k,Count=m.Count()});

            Console.WriteLine("分组表达式：");
            foreach (var item in GroupItems)
            {
                Console.WriteLine(item.Menpai + "\t" + item.Count);
            }

            //量词操作符
            var AnyItems = from m in master
                           where m.Kunfu == "葵花宝典"
                           select new { m.Name, m.Menpai, m.Kunfu };
            Console.WriteLine("练葵花宝典的大侠：");
            foreach (var item in AnyItems)
            {
                Console.WriteLine(item.Name + "\t" + item.Menpai + "\t" + item.Kunfu);
            }

            var  any=AnyItems.Any(m=>m.Menpai=="明教");

            Console.WriteLine("是否有练习葵花宝典中的明教中人："+any);

            var all = AnyItems.All(m => m.Kunfu == "明教");
            Console.WriteLine("是否有练习葵花宝典都是明教中人：" + all);

            //元素判断
            var OuyangFeng = new MartialArtsMaster { Id = 12 , Name = "欧阳锋", age = 58, Menpai = "白驼山庄", Kunfu = "蛤蟆功", Level = 10 };
            var HuangYaoShi = master[10];

            var IsOuYangFeng =master.Contains(OuyangFeng);
            var IsHunagYaoShi = master.Contains(HuangYaoShi as MartialArtsMaster);

            Console.WriteLine("大侠名单是否有欧阳锋：{0}",IsOuYangFeng);
            Console.WriteLine("大侠名单是否有黄药师：{0}",IsHunagYaoShi);


            //分页
            master.Add(new MartialArtsMaster() { Id = 12, Name = "萧峰", age = 33, Menpai = "丐帮", Kunfu = "降龙十八掌", Level = 9 });
            master.Add(new MartialArtsMaster() { Id = 13, Name = "段誉", age = 18, Menpai = "天龙寺", Kunfu = "六脉神剑", Level = 7});
            master.Add(new MartialArtsMaster() { Id = 14, Name = "虚竹", age = 18, Menpai = "逍遥派", Kunfu = "北冥神功", Level = 9 });
            master.Add(new MartialArtsMaster() { Id = 15, Name = "方正大师", age = 18, Menpai = "少林寺", Kunfu = "七十二绝技", Level = 10 });
            master.Add(new MartialArtsMaster() { Id = 16, Name = "杨过", age = 18, Menpai = "古墓派", Kunfu = "玉女心经", Level = 9 });
            master.Add(new MartialArtsMaster() { Id = 17, Name = "小龙女", age = 18, Menpai = "古墓派", Kunfu = "玉女心经", Level = 10 });

            kongfu.Add(new Kongfu() { KongfuId = 7, KongfuName = "六脉神剑", Lethality = 100 });
            kongfu.Add(new Kongfu() { KongfuId = 8, KongfuName = "北冥神功", Lethality = 100 });
            kongfu.Add(new Kongfu() { KongfuId = 9, KongfuName = "七十二绝技", Lethality = 100 });
            kongfu.Add(new Kongfu() { KongfuId = 10, KongfuName = "玉女心经", Lethality = 95 });

            int pageSizes = 5;
            int pageNumber = (int)Math.Ceiling(master.Count() / (double)pageSizes);
            Console.WriteLine("使用分区操作符分页：");

            Console.WriteLine("大侠总数："+master.Count()+" 总页数："+pageNumber+" 每页："+pageSizes);

            for (int n = 0; n < pageNumber; n++)
            {
                var pageMaster =(from  m in master
                                 join k in kongfu on m.Kunfu equals k.KongfuName
                                 orderby m.Level * k.Lethality descending
                                 select new { m.Name,m.Menpai,m.Kunfu,k.Lethality,m.Level,Kill=m.Level*k.Lethality}
                                 ).Skip(pageSizes * n).Take(pageSizes);
                Console.WriteLine("姓名   门派  武功  杀伤力 修炼级别    总功力");
                foreach (var item in pageMaster)
                {
                    Console.WriteLine(item.Name + "\t" + item.Menpai + "\t" + item.Kunfu + "\t" + item.Lethality + "\t" + item.Level + "\t" + item.Kill);
                }
            }


            //集合操作符

            //交集
            var  ItemInterSect=(from m in master
                              where m.Menpai=="华山" || m.Menpai=="明教"
                              select m).Intersect(from m in master where m.Kunfu=="葵花宝典" select m);

            foreach (var item in ItemInterSect)
            {
                Console.WriteLine(item.Name+"\t"+item.Menpai+"\t"+item.Kunfu);
            }

            //其他:
            //Union： 并集，没有重复元素
            //Contact: 返回两个并集
            //Except: 差级，返回只出现一个集合中元素


            //累加
            int[] numbers = { 1, 2, 3 };
            int x = numbers.Aggregate((prod, n) => prod + n);
            int y = numbers.Aggregate(0,(prod, n) => prod + n);
            int z = numbers.Aggregate(0, (prod, n) => prod + n, r => r * 2);


            //元素总量：
            //Count/LongCount
            //元素总和：
            //Sum
            //最大值
            //Max
            Console.WriteLine(x);


            Console.ReadKey();

        }
    }
}