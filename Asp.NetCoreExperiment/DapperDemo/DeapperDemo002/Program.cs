﻿using Dapper;
using Npgsql;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Collections.Generic;
namespace DeapperDemo002
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;

            Test3();


        }

        static void Test3()
        {
            var connString = "Server=127.0.0.1;Port=5432;UserId=postgres;Password=postgres2018;Database=abc;";
            using (var conn = new NpgsqlConnection(connString))
            {
                var sql = @"select b,a,d,c from test3";
                var list = conn.Query<dynamic>(sql);
                var itemString = new StringBuilder();
                foreach (IDictionary<string, object> item in list)
                {
                    itemString.AppendJoin(',', item?.Keys);
                    itemString.AppendLine();
                    break;
                }
                foreach (IDictionary<string, object> item in list)
                {
                    itemString.AppendJoin(',', item?.Values);
                    itemString.AppendLine();
                }
                Console.WriteLine(itemString.ToString());
            }
        }

        static void Test2()
        {
            var connString = "Server=127.0.0.1;Port=5432;UserId=postgres;Password=postgres2018;Database=abc;";
            using (var conn = new NpgsqlConnection(connString))
            {

                #region 错误做法
                //var sql = @"select * from ""T_Test"" where  date(createtime)=date(@date)";
                //var date = DateTimeOffset.Parse("2019-09-03 09:27:00 +09:00");
                //var list = conn.Query<dynamic>(sql, new { date });
                #endregion

                #region 正确做法
                var sql = @"select * from ""T_Test"" where  createtime>=@beginTime and createtime<=@endtime";
                var beginTime = DateTimeOffset.Parse("2019-09-03 00:00:00");
                var endTime = DateTimeOffset.Parse("2019-09-03 23:59:59.9999994");
                var list = conn.Query<dynamic>(sql, new { beginTime, endTime });
                #endregion
                foreach (var item in list)
                {
                    Console.WriteLine($"ID:{item.id}  Name:{item.name}  Time:{item.createtime} ");
                }
            }
        }
        static void Test1()
        {
            //var file = System.IO.Directory.GetCurrentDirectory() + "/sql.txt";

            //var content = System.IO.File.ReadAllText(file).ToLower();

            //return;
            var connString = "Server=127.0.0.1;Port=5432;UserId=postgres;Password=postgre2018;Database=TestDB;";
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // Insert some data
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO Roles(RoleName) VALUES (@rolename)";
                    cmd.Parameters.AddWithValue("rolename", "aaa");
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new NpgsqlCommand("SELECT id FROM Roles", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        Console.WriteLine(reader.GetString(0));
            }
        }


    }
}
