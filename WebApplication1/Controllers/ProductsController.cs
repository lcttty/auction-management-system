using System;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;
using System.Collections;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Ajax.Utilities;
using System.Security.Cryptography.X509Certificates;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductsController : ApiController
    {
        private static string rsa = "<RSAKeyValue><Modulus>m6dgEA9rh6IYV/xlgJ1M2nCZthsxTBkGeq5eRcvdJLcSwescuwA7nKm4nCz8Nq2xDNfSVpyHwtuSNWOjxsg09p1ovsArb0b56M/0I7q0WTt4ojIFFdXOtw7CDyRevgl+QsHFAIK4m5OctkaP/4U9Peohr/DsnQroauyuNiYjE/0=</Modulus><Exponent>AQAB</Exponent><P>wHo0uF/HE26BBffzBOMw+n6/XP1eU+9P7yEl7dScWAAbqpo+ON78qAszlr4oDXRgFA4GmDpdW3L813VhMuAWtw==</P><Q>zwYQt6476A2EHCpw1azpK/QCreN6KFiWinpTspQVR0MSaLOAy0uKV1QXD8wev+/ALwD/jEpSyr9EJwNRPNmW6w==</Q><DP>oTj71gAr8WPbYRN6lPp0eS4Xvp1gGLBY5TV/3sH7H18fzXwbaGmnel6/nKG1TOfQ3puM/I/OyR39GcBxZTr86w==</DP><DQ>xApOUmHfwMLr03AtIWp383NCBkBfMU2SYNet4nFwJOdSy2sQD3MdMc0jeYYlqEP0jY44cDUBaZTtiLQPfbveuQ==</DQ><InverseQ>X9arGYqWH78xjPEIhxMZXdB9bgjoBM0p9g0AbRPB5jisQ1botBI/mROyC9KUYhJx04JB9APTqDj2lBiiH+FSbQ==</InverseQ><D>PihRsTgnLauqYwB0nC60sQqEJ3emcQgRUU5ucRre/blp8SxtdbO7Gm2gPiWstj0YVdOduF6bzFGYil922Hu8whtCuacniMaAyDnyZ9gfQV2o0gFvdFxa/NKC7PQcfoqxeynMj8f0/vzPpjnp7wDj3/aG+m2ZFr2ijQ4GKsnePzU=</D></RSAKeyValue>";
        public IHttpActionResult GetProduct(int id)
        {
            string sql = "SELECT * FROM product WHERE id=" + id;
            Express ex = new Express();
            DataSet ds = ex.Query(sql);
            if (ds.Tables == null)
            {
                return NotFound();
            }
            return Ok(ds);
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetOne()
        {
            string sql = "SELECT * FROM product";
            Express ex = new Express();
            DataSet ds = ex.Query(sql);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, ds);
            CookieHeaderValue cookie = new CookieHeaderValue("UserInfo", "lcttty");
            cookie.Expires = DateTime.Now.AddDays(1);
            cookie.Path = "/";
            cookie.Domain = "localhost";
            cookie.HttpOnly = true;
            response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return response;
        }

        [HttpPost]
        [Route("yanzhengma")]
        public HttpResponseMessage yanzheng()
        {
            yanZheng yzm = new yanZheng();
            string code = yzm.GetCode();
            var imgByte = yzm.GetImage(code);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(imgByte)
            };
            HttpContext.Current.Session["code"] = code;

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return response;
        }

        [HttpPost]
        [Route("login/{username}/{password}/{code}")]
        public HttpResponseMessage login(string username, string password, string code)
        {
            string ycode = HttpContext.Current.Session["code"].ToString();
            if (ycode.ToLower() != code.ToLower())
            {
                Models.Message ms = new Models.Message();
                ms.Code = 400;
                ms.Mess = "验证码错误!";

                yanZheng yyzm = new yanZheng();
                string ccode = yyzm.GetCode();
                var iimgByte = yyzm.GetImage(ccode);
                HttpContext.Current.Session["code"] = ccode;
                HttpResponseMessage response1 = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(iimgByte)
                };
                response1.Headers.Add("Myself", ms.Code + " " + HttpUtility.UrlEncode(ms.Mess, Encoding.UTF8));
                response1.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return response1;
            }
            username = HttpUtility.UrlDecode(username, Encoding.UTF8);
            password = HttpUtility.UrlDecode(password, Encoding.UTF8);
            username = RSA.UnRsa(username, rsa);
            password = RSA.UnRsa(password, rsa);
            if (!jiance.login(username) || !jiance.login(password))
            {
                yanZheng yyzm = new yanZheng();
                string ccode = yyzm.GetCode();
                var iimgByte = yyzm.GetImage(ccode);
                HttpContext.Current.Session["code"] = ccode;
                HttpResponseMessage response2 = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(iimgByte)
                };
                Models.Message ms = new Models.Message();
                ms.Code = 400;
                ms.Mess = "账号或密码格式不正确";
                response2.Headers.Add("Myself", ms.Code + " " + HttpUtility.UrlEncode(ms.Mess, Encoding.UTF8));
                response2.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return response2;
            }
            Express ex = new Express();
            string sql = "SELECT * FROM users WHERE username = '" + username + "' AND password = '" + password + "'";
            DataSet ds = ex.Query(sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                yanZheng yyzm = new yanZheng();
                string ccode = yyzm.GetCode();
                var iimgByte = yyzm.GetImage(ccode);
                HttpContext.Current.Session["code"] = ccode;
                HttpResponseMessage response2 = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(iimgByte)
                };
                Models.Message ms = new Models.Message();
                ms.Code = 400;
                ms.Mess = "账号或者密码错误";
                response2.Headers.Add("Myself", ms.Code + " " + HttpUtility.UrlEncode(ms.Mess, Encoding.UTF8));
                response2.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return response2;
            }
            username = ds.Tables[0].Rows[0]["username"].ToString();
            password = ds.Tables[0].Rows[0]["password"].ToString();
            int state = int.Parse(ds.Tables[0].Rows[0]["state"].ToString());
            if (state == 4 || state == 5)
            {
                yanZheng yyzm = new yanZheng();
                string ccode = yyzm.GetCode();
                var iimgByte = yyzm.GetImage(ccode);
                HttpContext.Current.Session["code"] = ccode;
                HttpResponseMessage response2 = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(iimgByte)
                };
                Message ms = new Message();
                ms.Code = 400;
                ms.Mess = "账号被封禁";
                response2.Headers.Add("Myself", ms.Code + " " + HttpUtility.UrlEncode(ms.Mess, Encoding.UTF8));
                response2.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return response2;
            }
            Random rd = new Random(((int)DateTime.Now.Ticks));
            int rdd = rd.Next(100000, 999999);
            string st = username + DateTime.Now.ToFileTimeUtc().ToString() + rdd + password;
            string str = FormsAuthentication.HashPasswordForStoringInConfigFile(st, "MD5").ToLower();
            string sql1 = "UPDATE users SET cookie = '" + str + "' WHERE username = '" + username + "' AND password = '" + password + "'";
            Express ex1 = new Express();
            ex1.Execute(sql1);
            CookieHeaderValue cookie = new CookieHeaderValue("c1", str);
            cookie.Path = "/";
            cookie.Domain = "localhost";
            cookie.HttpOnly = true;
            yanZheng yzm = new yanZheng();
            string cc = yzm.GetCode();
            var imgByte = yzm.GetImage(cc);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(imgByte)
            };
            Models.Message mme = new Models.Message();
            mme.Code = 200;
            mme.Mess = "恭喜您,登陆成功!";
            response.Headers.Add("Myself", mme.Code + " " + HttpUtility.UrlEncode(mme.Mess, Encoding.UTF8));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return response;
        }

        [HttpPost]
        [Route("register/{username}/{password}/{repassword}")]
        public HttpResponseMessage register(string username, string password, string repassword)
        {
            username = HttpUtility.UrlDecode(username, Encoding.UTF8);
            password = HttpUtility.UrlDecode(password, Encoding.UTF8);
            repassword = HttpUtility.UrlDecode(repassword, Encoding.UTF8);
            username = RSA.UnRsa(username, rsa);
            password = RSA.UnRsa(password, rsa);
            repassword = RSA.UnRsa(repassword, rsa);
            if (password != repassword)
            {
                Models.Message mm = new Models.Message();
                mm.Code = 400;
                mm.Mess = "两次密码不同";
                HttpResponseMessage hrm = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(mm), Encoding.GetEncoding("UTF-8"), "application/json")
                };
                return hrm;
            }
            if (!jiance.login(username) || !jiance.login(password))
            {
                Models.Message mm = new Models.Message();
                mm.Code = 400;
                mm.Mess = "账号或密码格式不正确";
                HttpResponseMessage hrm = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(mm), Encoding.GetEncoding("UTF-8"), "application/json")
                };
                return hrm;
            }
            string sql1 = "SELECT * FROM users WHERE username = '" + username + "'";
            Express ex1 = new Express();
            DataSet ds = new DataSet();
            ds = ex1.Query(sql1);
            if (ds.Tables[0].Rows.Count != 0)
            {
                Models.Message mmm = new Models.Message();
                mmm.Code = 400;
                mmm.Mess = "账号已经存在";
                HttpResponseMessage hrmm = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(mmm), Encoding.GetEncoding("UTF-8"), "application/json")
                };
                return hrmm;
            }
            string sql = "INSERT INTO users (username,password,money,name,state) VALUES ('" + username + "','" + password + "',0, '" + username + "', 1)";
            Express ex = new Express();
            if (ex.Execute(sql) == 1)
            {
                Models.Message mm = new Models.Message();
                mm.Code = 200;
                mm.Mess = "注册成功";
                HttpResponseMessage hrm = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(mm), Encoding.GetEncoding("UTF-8"), "application/json")
                };
                return hrm;
            }
            else
            {
                Models.Message mm = new Models.Message();
                mm.Code = 400;
                mm.Mess = "注册失败";
                HttpResponseMessage hrm = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(mm), Encoding.GetEncoding("UTF-8"), "application/json")
                };
                return hrm;
            }
        }
        [HttpPost]
        [Route("pmpquery/{query}")]
        public HttpResponseMessage pmpquery(string query)
        {
            string[] qq = { "all", "zghh", "sfzk", "xhds", "gczx", "ddgy", "jp" };
            if (qq.Contains(query))
            {
                string sql = "SELECT * FROM pai_mai";
                switch (query)
                {
                    case "all":
                        sql = sql;
                        break;
                    case "zghh":
                        sql += " WHERE type = '中国绘画'";
                        break;
                    case "sfzk":
                        sql += " WHERE type = '书法篆刻'";
                        break;
                    case "xhds":
                        sql += " WHERE type = '西画雕塑'";
                        break;
                    case "gczx":
                        sql += " WHERE type = '古瓷杂项'";
                        break;
                    case "ddgy":
                        sql += " WHERE type = '当代工艺'";
                        break;
                    case "jp":
                        sql += " WHERE type = '酒品'";
                        break;
                    default:
                        sql = sql;
                        break;
                }
                Express ex = new Express();
                DataSet ds = ex.Query(sql);
                HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.OK, ds);
                return response1;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }


        [HttpGet]
        [Route("panduan")]
        public IHttpActionResult panduan()
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            if (hc == null)
            {
                return Ok("false");
            } 
            else
            {
                string cookie = hc.Value;
                Express ex = new Express();
                string sql = "SELECT * FROM users WHERE cookie = '" + cookie + "'";
                DataSet ds = ex.Query(sql);
                if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
                {
                    return Ok("false");
                }
            }
            return Ok("true");
        }
        [HttpGet]
        [Route("zhuanchang")]
        public IHttpActionResult zhuanchang()
        {
            Express ex = new Express();
            string sql = "SELECT distinct zname FROM zhuan_chang";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }
        [HttpPost]
        [Route("zhuanchang/{query}")]
        public IHttpActionResult zhuanchangquery(string query)
        {
            query = HttpUtility.UrlDecode(query, Encoding.UTF8);
            // zname = HttpUtility.UrlDecode(zname, Encoding.UTF8);
            Express ex = new Express();
            string sql = "SELECT * FROM zhuan_chang WHERE zname = '" + query + "'";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }
        [HttpPost]
        [Route("zhuanchang/{name}/{zname}")]
        public IHttpActionResult zhuanchangquery(string name, string zname)
        {
            name = HttpUtility.UrlDecode(name, Encoding.UTF8);
            zname = HttpUtility.UrlDecode(zname, Encoding.UTF8);
            Express ex = new Express();
            string sql = "SELECT * FROM zhuan_chang WHERE zname = '" + zname + "' AND name='" + name + "'";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }
        [HttpPost]
        [Route("fuqian/{id}/{money}")]
        public IHttpActionResult fuqian(int id, int money)
        {
            Models.Message ms = new Models.Message();
            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            if (hc == null)
            {
                ms.Code = 100;
                ms.Mess = "对不起,您没有登录!";
                return Ok(ms);
            }
            string sql = "SELECT * FROM users WHERE cookie = '" + hc.Value + "'";
            Express ex = new Express();
            DataSet ds = ex.Query(sql);
            if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
            {
                ms.Code = 100;
                ms.Mess = "对不起,您的登录信息有误,请重新登录!";
                return Ok(ms);
            }
            string name = ds.Tables[0].Rows[0]["name"].ToString();
            int nmoney = int.Parse(ds.Tables[0].Rows[0]["money"].ToString());
            if (nmoney < money)
            {
                ms.Code = 200;
                ms.Mess = "对不起,您的余额不足,请充值!";
                return Ok(ms);
            }
            Express ex1 = new Express();
            string sql1 = "SELECT * FROM pai_mai WHERE id=" + id;
            DataSet ds1 = ex1.Query(sql1);
            if (ds1 == null || ds1.Tables.Count == 0 || (ds1.Tables.Count == 1 && ds1.Tables[0].Rows.Count == 0))
            {
                ms.Code = 300;
                ms.Mess = "物品信息有误!";
                return Ok(ms);
            }
            int nowprice = int.Parse(ds1.Tables[0].Rows[0]["nowprice"].ToString());
            int gapprice = int.Parse(ds1.Tables[0].Rows[0]["gapprice"].ToString());
            int price = money - nowprice;
            string wupin_name = ds1.Tables[0].Rows[0]["name"].ToString();
            if (money <= nowprice || price % gapprice != 0)
            {
                ms.Code = 300;
                ms.Mess = "付款信息有误!";
                return Ok(ms);
            }
            string starttime = ds1.Tables[0].Rows[0]["starttime"].ToString();
            string endtime = ds1.Tables[0].Rows[0]["endtime"].ToString();
            string nowtime = DateTime.Now.ToLocalTime().ToString();
            string nname = ds1.Tables[0].Rows[0]["name"].ToString();
            string number = ds1.Tables[0].Rows[0]["number"].ToString();
            int idd = int.Parse(ds1.Tables[0].Rows[0]["id"].ToString());
            if (DateTime.Parse(nowtime) < DateTime.Parse(starttime))
            {
                ms.Code = 400;
                ms.Mess = "拍品还未开始";
                return Ok(ms);
            }
            if (DateTime.Parse(nowtime) > DateTime.Parse(endtime))
            {
                ms.Code = 400;
                ms.Mess = "物品拍卖已经结束";
                return Ok(ms);
            }
            // 减少用户的钱
            Express ex5 = new Express();
            string sql5 = "SELECT * FROM chengjiao WHERE name = '" + wupin_name + "' AND person = '" + name + "' ORDER BY price desc";
            DataSet ds5 = ex5.Query(sql5);
            if (!(ds5 == null || ds5.Tables.Count == 0 || (ds5.Tables.Count == 1 && ds5.Tables[0].Rows.Count == 0)))
            {
                int money1 = int.Parse(ds5.Tables[0].Rows[0]["price"].ToString());
                Express ex6 = new Express();
                string sql6 = "UPDATE users SET money = money + " + money1 + " WHERE name = '" + name + "'";
                ex6.Execute(sql6);
            }
            Express ex2 = new Express();
            string sql2 = "UPDATE users SET money = money - " + money + " WHERE name = '" + name + "'";
            ex2.Execute(sql2);
            Express ex3 = new Express();
            string sql3 = "UPDATE pai_mai SET cishu = cishu + 1,nowprice = " + money + ",person = '" + name + "' WHERE id = " + idd;
            ex3.Execute(sql3);
            string sql4 = "INSERT INTO chengjiao VALUES ('" + name + "'," + money + ",'" + nname + "','" + nowtime + "',0, '" + number + "')";
            ex3.Execute(sql4);
            ms.Code = 500;
            ms.Mess = "拍卖成功,请留意后续价格变动,祝您获取您中意之物!";
            return Ok(ms);
        }
        [HttpPost]
        [Route("guanli/zhuxiao")]
        public HttpResponseMessage guanli_zhuxiao()
        {
            HttpResponseMessage hrm = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("true", Encoding.GetEncoding("UTF-8"), "application/json")
            };
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (hc == null)
            {
                return hrm;
            }
            CookieHeaderValue cookie = new CookieHeaderValue(hc.Name, hc.Value);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Path = "/";
            cookie.Domain = "localhost";
            cookie.HttpOnly = true;
            hrm.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return hrm;
        }

        [HttpPost]
        [Route("zhuxiao")]
        public HttpResponseMessage zhuxiao()
        {
            HttpResponseMessage hrm = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("true", Encoding.GetEncoding("UTF-8"), "application/json")
            };
            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            if (hc == null)
            {
                return hrm;
            }
            CookieHeaderValue cookie = new CookieHeaderValue(hc.Name, hc.Value);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Path = "/";
            cookie.Domain = "localhost";
            cookie.HttpOnly = true;
            hrm.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return hrm;
        }

        [HttpPost]
        [Route("fuqian1/{zname}/{name}/{money}")]
        public IHttpActionResult zfuqian(string zname, string name, int money)
        {
            zname = HttpUtility.UrlDecode(zname, Encoding.UTF8);
            name = HttpUtility.UrlDecode(name, Encoding.UTF8);
            Models.Message ms = new Models.Message();
            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            if (hc == null)
            {
                ms.Code = 100;
                ms.Mess = "对不起,您没有登录!";
                return Ok(ms);
            }
            string sql = "SELECT * FROM users WHERE cookie = '" + hc.Value + "'";
            Express ex = new Express();
            DataSet ds = ex.Query(sql);
            if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
            {
                ms.Code = 100;
                ms.Mess = "对不起,您的登录信息有误,请重新登录!";
                return Ok(ms);
            }
            string username = ds.Tables[0].Rows[0]["name"].ToString();
            int nmoney = int.Parse(ds.Tables[0].Rows[0]["money"].ToString());
            if (nmoney < money)
            {
                ms.Code = 200;
                ms.Mess = "对不起,您的余额不足,请充值!";
                return Ok(ms);
            }
            Express ex1 = new Express();
            string sql1 = "SELECT * FROM zhuan_chang WHERE name='" + name + "' AND zname='" + zname + "'";
            DataSet ds1 = ex1.Query(sql1);
            if (ds1 == null || ds1.Tables.Count == 0 || (ds1.Tables.Count == 1 && ds1.Tables[0].Rows.Count == 0))
            {
                ms.Code = 300;
                ms.Mess = "物品信息有误!";
                return Ok(ms);
            }
            int nowprice = int.Parse(ds1.Tables[0].Rows[0]["nowprice"].ToString());
            int gapprice = int.Parse(ds1.Tables[0].Rows[0]["gapprice"].ToString());
            string number = ds1.Tables[0].Rows[0]["number"].ToString();
            int price = money - nowprice;
            if (price <= 0 || price % gapprice != 0)
            {
                ms.Code = 300;
                ms.Mess = "付款信息有误!";
                return Ok(ms);
            }
            string starttime = ds1.Tables[0].Rows[0]["starttime"].ToString();
            string endtime = ds1.Tables[0].Rows[0]["endtime"].ToString();
            string nowtime = DateTime.Now.ToLocalTime().ToString();
            // string nname = ds1.Tables[0].Rows[0]["name"].ToString();
            // int idd = int.Parse(ds1.Tables[0].Rows[0]["id"].ToString());
            if (DateTime.Parse(nowtime) < DateTime.Parse(starttime))
            {
                ms.Code = 400;
                ms.Mess = "拍品还未开始";
                return Ok(ms);
            }
            if (DateTime.Parse(nowtime) > DateTime.Parse(endtime))
            {
                ms.Code = 400;
                ms.Mess = "物品拍卖已经结束";
                return Ok(ms);
            }
            // 减少用户的钱
            Express ex5 = new Express();
            string sql5 = "SELECT * FROM chengjiao WHERE name = '" + name + "' AND person = '" + username + "' ORDER BY price desc";
            DataSet ds5 = ex5.Query(sql5);
            if (!(ds5 == null || ds5.Tables.Count == 0 || (ds5.Tables.Count == 1 && ds5.Tables[0].Rows.Count == 0)))
            {
                int money1 = int.Parse(ds5.Tables[0].Rows[0]["price"].ToString());
                Express ex6 = new Express();
                string sql6 = "UPDATE users SET money = money + " + money1 + " WHERE name = '" + username + "'";
                ex6.Execute(sql6);
            }

            Express ex2 = new Express();
            string sql2 = "UPDATE users SET money = money - " + money + " WHERE name = '" + username + "'";
            ex2.Execute(sql2);
            Express ex3 = new Express();
            string sql3 = "UPDATE zhuan_chang SET cishu = cishu + 1,nowprice = " + money + ",person = '" + username + "' WHERE name = '" + name + "' AND zname = '" + zname + "'";
            ex3.Execute(sql3);
            string sql4 = "INSERT INTO chengjiao VALUES ('" + username + "'," + money + ",'" + name + "','" + nowtime + "',0, " + number + ")";
            ex3.Execute(sql4);

            ms.Code = 500;
            ms.Mess = "拍卖成功,请留意后续价格变动,祝您获取您中意之物!";
            return Ok(ms);
        }

        [HttpPost]
        [Route("gaimi/{oldpass}/{newpass}")]
        public IHttpActionResult gaimi(string oldpass, string newpass)
        {
            oldpass = HttpUtility.UrlDecode(oldpass, Encoding.UTF8);
            newpass = HttpUtility.UrlDecode(newpass, Encoding.UTF8);
            oldpass = RSA.UnRsa(oldpass, rsa);
            newpass = RSA.UnRsa(newpass, rsa);
            if (!jiance.login(oldpass) || !jiance.login(newpass))
            {
                return Ok("error1");
            }
            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            string cookie = hc.Value;
            Express ex = new Express();
            string sql = "SELECT * FROM users WHERE cookie = '" + cookie + "'";
            DataSet ds = ex.Query(sql);
            if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
            {
                return Ok("error2");
            }
            if (oldpass != ds.Tables[0].Rows[0]["password"].ToString())
            {
                return Ok("error3");
            }
            Express ex1 = new Express();
            string sql1 = "UPDATE users SET password = '" + newpass + "' WHERE cookie = '" + cookie + "'";
            if (ex1.Execute(sql1) == 1)
            {
                return Ok("ok");
            }
            return Ok("error4");
        }

        [HttpPost]
        [Route("chongzhi/{xx}")]
        public IHttpActionResult chongzhi(string xx)
        {
            xx = HttpUtility.UrlDecode(xx, Encoding.UTF8);
            xx = RSA.UnRsa(xx, rsa);
            if (!jiance.chongzhi(xx))
            {
                return Ok(xx);
            }
            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            if (hc == null)
            {
                return Ok("state2");
            }
            string cookie = hc.Value;
            Express ex = new Express();
            string sql = "SELECT * FROM users WHERE cookie = '" + cookie + "'";
            DataSet ds = ex.Query(sql);
            if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
            {
                return Ok("state2");
            }
            Express ex1 = new Express();
            string sql1 = "SELECT * FROM chongzhi WHERE id = '" + xx + "'";
            DataSet ds1 = ex1.Query(sql1);
            if (ds1 == null || ds1.Tables.Count == 0 || (ds1.Tables.Count == 1 && ds1.Tables[0].Rows.Count == 0))
            {
                return Ok("state1");
            }
            int money = int.Parse(ds1.Tables[0].Rows[0]["money"].ToString());
            Express ex2 = new Express();
            string sql2 = "UPDATE users SET money = money+" + money + " WHERE cookie = '" + cookie + "'";
            if (ex2.Execute(sql2) == 1)
            {
                Express ex3 = new Express();
                string sql3 = "DELETE FROM chongzhi WHERE id = '" + xx + "'";
                ex3.Execute(sql3);
                return Ok("ok");
            }
            else
            {
                return Ok("state3");
            }
        }

        [HttpPost]
        [Route("myself")]
        public IHttpActionResult myself()
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            if (hc == null)
            {
                return Ok("state1");
            }
            string cookie = hc.Value;
            Express ex = new Express();
            string sql = "SELECT money,name FROM users WHERE cookie = '" + cookie + "'";
            DataSet ds = ex.Query(sql);
            if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
            {
                return Ok("state1");
            }
            return Ok(ds);
        }
        [HttpPost]
        [Route("changeid")]
        public IHttpActionResult changeid(JObject jsonObj)
        {

            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            if (hc == null)
            {
                return Ok("state1");
            }
            string cookie = hc.Value;
            Express ex = new Express();
            string sql = "SELECT * FROM users WHERE cookie = '" + cookie + "'";
            DataSet ds = ex.Query(sql);
            if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
            {
                return Ok("state1");
            }

            var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

            string id = jsonParams.id;

            if (!jiance.sql(id))
            {
                return Ok("state2");
            }

            Express ex1 = new Express();
            string sql1 = "UPDATE users SET name = '" + id + "' WHERE cookie = '" + cookie + "'";
            ex1.Execute(sql1);
            return Ok("state3");
        }
        [HttpPost]
        [Route("myself1")]
        public IHttpActionResult myself1()
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            if (hc == null)
            {
                return Ok("state1");
            }
            string cookie = hc.Value;
            Express ex = new Express();
            string sql = "SELECT username,money,name FROM users WHERE cookie = '" + cookie + "'";
            DataSet ds = ex.Query(sql);
            if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
            {
                return Ok("state1");
            }
            string username = ds.Tables[0].Rows[0]["username"].ToString();
            string name = ds.Tables[0].Rows[0]["name"].ToString();
            Express ex1 = new Express();
            string sql1 = "SELECT * from chengjiao WHERE person = '" + name + "'";
            DataSet ds1 = ex1.Query(sql1);
            if (ds1 == null || ds1.Tables.Count == 0 || (ds1.Tables.Count == 1 && ds1.Tables[0].Rows.Count == 0))
            {
                return Ok("state2");
            }
            return Ok(ds1);
        }

        [HttpPost]
        [Route("putong/{name}")]
        public IHttpActionResult putongquery(string name)
        {
            name = HttpUtility.UrlDecode(name, Encoding.UTF8);
            Express ex = new Express();
            string sql = "SELECT * FROM pai_mai WHERE name='" + name + "'";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }
        [HttpPost]
        [Route("getjj")]
        public IHttpActionResult getjj()
        {
            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Express ex = new Express();
            string sql = "SELECT top 10 name, starttime FROM pai_mai WHERE starttime>'" + dt + "' order by starttime";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }
        [HttpPost]
        [Route("getzx")]
        public IHttpActionResult getzx()
        {
        Express ex = new Express();
            string sql = "SELECT top 10 name,price FROM chengjiao order by time";
            DataSet ds = ex.Query(sql);

            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict["name"] = ds.Tables[0].Rows[i]["name"].ToString();
                dict["price"] = ds.Tables[0].Rows[i]["price"].ToString();
                dict["number"] = i.ToString();
                string sql1 = "SELECT name FROM pai_mai WHERE name = '" + dict["name"] + "'";
                Express ex1 = new Express();
                DataSet ds1 = ex1.Query(sql1);
                if (ds1.Tables[0].Rows.Count == 0)
                {
                    string sql2 = "SELECT name,zname FROM zhuan_chang WHERE name = '" + dict["name"] + "'";
                    Express ex2 = new Express();
                    DataSet ds2 = ex2.Query(sql2);
                    dict["state"] = "1";
                    dict["zname"] = ds2.Tables[0].Rows[0]["zname"].ToString();
                }
                else
                {
                    dict["state"] = "0";
                }
                list.Add(dict);
            }
            return Ok(list);
        }
        [HttpPost]
        [Route("gethj")]
        public IHttpActionResult gethj()
        {
            Express ex = new Express();
            string sql = "SELECT top 4 name,url FROM pai_mai WHERE type='酒品' order by starttime desc";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }
        [HttpPost]
        [Route("getgczx")]
        public IHttpActionResult getgczx()
        {
            Express ex = new Express();
            string sql = "SELECT top 4 name,url FROM pai_mai WHERE type='古瓷杂项' order by starttime desc";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }
        [HttpPost]
        [Route("getzghh")]
        public IHttpActionResult getzghh()
        {
            Express ex = new Express();
            string sql = "SELECT top 4 name,url FROM pai_mai WHERE type='中国绘画' order by starttime desc";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }

        [HttpPost]
        [Route("getjpzc")]
        public IHttpActionResult getjpzc()
        {
            string nowtime = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
            Express ex = new Express();
            string sql = "SELECT TOP 4 zname, url FROM zc WHERE url LIKE '%jpz%' ORDER BY starttime";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }
        [HttpPost]
        [Route("geturls")]
        public IHttpActionResult geturls()
        {
            string nowtime = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
            Express ex = new Express();
            string sql = "SELECT TOP 8 zname, url FROM zc WHERE url LIKE '%zmd%' ORDER BY starttime";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }
        [HttpPost]
        [Route("notice")]
        public IHttpActionResult notice()
        {
            Express ex = new Express();
            string sql = "SELECT * from notice";
            DataSet ds = ex.Query(sql);
            return Ok(ds);
        }
        [HttpPost]
        [Route("cwsj-upload-xh")]
        public IHttpActionResult cwsjuploadxh(JObject jsonObj)
        {
            var jsonStr = JsonConvert.SerializeObject(jsonObj);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);
            string name = jsonParams.name;
            if (HttpContext.Current.Session["prefix"] == null)
            {
                return Ok("false");
            }
            string rootpath = HttpContext.Current.Session["prefix"].ToString();
            if (!Directory.Exists(rootpath))
            {
                return Ok("false");
            }
            List<string> files = new List<string>();
            DirectoryInfo theFolder = new DirectoryInfo(rootpath);
            foreach (FileInfo file in  theFolder.GetFiles())
            {
                files.Add(file.Name);
                if (file.Name == name)
                {
                    File.Delete(file.FullName);
                    break;
                }
            }
            return Ok("true");
        }
        [HttpPost]
        [Route("cwsj-upload")]
        public async System.Threading.Tasks.Task<IHttpActionResult> cwsjuploadAsync()
        {
            string datetime = "";
            string rootpath = "";
            string url = "";
            if (HttpContext.Current.Session["prefix"] == null)
            {
                datetime = DateTime.Now.ToFileTimeUtc().ToString();
                rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/tmp/cwsj/" + datetime + "/";
            }
            else
            {
                rootpath = HttpContext.Current.Session["prefix"].ToString();
            }
            if (!Directory.Exists(rootpath))
            {
                Directory.CreateDirectory(rootpath);
            }
            string[] fileSystemEntries = Directory.GetFileSystemEntries(rootpath);
            if(fileSystemEntries.Length >=5)
            {
                return Json(new { code = "200", Message = "最多上传五个文件!" });
            }
            var provider = new MultipartFileWithExtensionStreamProvider(rootpath);
            List<string> files = new List<string>();
            List<string> urls = new List<string>();
            try
            {
                //执行完这条,文件就保存了
                await Request.Content.ReadAsMultipartAsync(provider);
                HttpContext.Current.Session["prefix"] = rootpath;
                url = rootpath.Substring(rootpath.IndexOf("/static"));
                foreach (var file in provider.FileData)
                {
                    files.Add(Path.GetFileName(file.LocalFileName));
                    urls.Add(url + Path.GetFileName(file.LocalFileName));
                }

                return Json(new { code = "200", name = files, url = urls});
            }
            catch (IOException innException)
            {
                if (innException.InnerException.InnerException.InnerException == null)
                {
                    return Json(new { code = "500", Message = "文件写入错误" });
                }
                return Json(new { code = "500", Message = "文件还是写入错误" });
            }
        }
        [HttpPost, Route("cwsj_destory")]
        public void cwsj_destory()
        {
            if (HttpContext.Current.Session["prefix"] == null)
            {
                return;
            }
            string prefix = HttpContext.Current.Session["prefix"].ToString();
            provider.DeleteFolder(prefix);
        }
        [HttpPost, Route("checkupload")]
        public IHttpActionResult checkupload()
        {
            if (HttpContext.Current.Session["prefix"] == null)
            {
                return Json(new { code = "500" });
            }
            string prefix = HttpContext.Current.Session["prefix"].ToString();
            if (!Directory.Exists(prefix))
            {
                return Json(new { code = "500" });
            }
            string[] fileSystemEntries = Directory.GetFileSystemEntries(prefix);
            if (fileSystemEntries.Length < 0 || fileSystemEntries.Length > 5)
            {
                return Json(new { code = "500" });
            }
            return Json(new { code = "200" });
        }
        [HttpPost, Route("cwsj_allsubmit")]
        public IHttpActionResult cwsj_allsubmit(JObject jsonObj)
        {
            var jsonStr = JsonConvert.SerializeObject(jsonObj);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);
            string gname = jsonParams.gname;
            string address = jsonParams.address;
            string phonenumber = jsonParams.phonenumber;

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("gname", gname);
            data.Add("address", address);
            data.Add("phonenumber", phonenumber);
            string d = JsonConvert.SerializeObject(data);


            string prefix = HttpContext.Current.Session["prefix"].ToString();

            string newfile = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/cwsj/";// + DateTime.Now.ToFileTimeUtc().ToString() + "/";

            //Directory.CreateDirectory(newfile);
            DirectoryInfo fi1 = new DirectoryInfo(prefix);

            foreach (FileInfo fileInfo in fi1.GetFiles())
            {
                if (fileInfo.Exists)
                {
                    string filename = fileInfo.FullName.Substring(fileInfo.FullName.LastIndexOf("\\"));
                    //fileInfo.CopyTo(newfile + filename, true);
                    File.Move(prefix + filename, newfile + filename);
                }
            }

            string[] fileSystemEntries = Directory.GetFileSystemEntries(newfile);



            List<string> urls = new List<string>();

            for (int i = 0; i < fileSystemEntries.Length; i++)
            {
                //fileSystemEntries[i]输出位全名
                urls.Add(fileSystemEntries[i].Substring(fileSystemEntries[i].IndexOf("/static")));
            }

            string s = string.Join(",", urls.ToArray());

            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            string sql1 = "UPDATE users SET state = '2' WHERE cookie = '" + hc.Value + "'";
            Express ex1 = new Express();
            ex1.Execute(sql1);

            string sql2 = "SELECT username FROM users WHERE cookie = '" + hc.Value + "'";
            Express ex2 = new Express();
            DataSet ds = ex2.Query(sql2);
            string username = ds.Tables[0].Rows[0]["username"].ToString();

            string sql = "INSERT INTO shenhee VALUES ('" + username + "', 'cwsj', '" + d + "', '" + s + "')";
            Express ex = new Express();
            if (ex.Execute(sql) != -1)
            {
                return Json(new {code = "200", message = "提交成功,请等待管理员审核!" });
            }
            return Json(new { code = "500", message = "提交失败!"});
        }
        [HttpPost, Route("issj")]
        public IHttpActionResult issj()
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["c1"];
            string sql = "SELECT state FROM users WHERE cookie = '" + hc.Value + "'";
            Express ex = new Express();
            DataSet ds =  ex.Query(sql);
            return Ok(ds);
        }

        [HttpPost, Route("shangjia/getxinxi")]
        public IHttpActionResult getxinxi()
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;
            if (jiance.isshangjia(cookie))
            {
                Express ex = new Express();
                string sql = "SELECT gname, address, phonenumber, money FROM users WHERE cookie = '" + cookie + "'";
                DataSet ds = ex.Query(sql);
                return Ok(ds);
            }
            return Json(new { message = "你不是商家" });
        }
        [HttpPost, Route("shangjia/changexinxi")]
        public IHttpActionResult changexinxi(JObject jsonObj)
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;
            if (jiance.isshangjia(cookie))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj);
                var jsonParam = JsonConvert.DeserializeObject<dynamic>(jsonStr);

                string gname = jsonParam.gname;
                string address = jsonParam.address;
                string phonenumber = jsonParam.phonenumber;

                string sql = "UPDATE users SET gname = '" + gname + "', address = '" + address + "', phonenumber = '" + phonenumber + "' WHERE cookie = '" + cookie + "'";
                Express ex = new Express();
                if (ex.Execute(sql) != -1)
                {
                    return Json(new { code = "200" });
                }
                else
                {
                    return Json(new { code = "500" });
                }
            }
            else
            {
                return Json(new { code = "500" });
            }
        }
        [HttpPost,Route("shangjia/tixian")]
        public IHttpActionResult tixian(JObject jsonObj)
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;

            if (jiance.isshangjia(cookie))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj);
                var jsonParam = JsonConvert.DeserializeObject<dynamic>(jsonStr);

                string money = jsonParam.money;
                string cardid = jsonParam.cardid;
                int moneyy = int.Parse(money);

                string sql1 = "SELECT money FROM users WHERE cookie = '" + cookie + "'";
                Express ex1 = new Express();
                DataSet ds = ex1.Query(sql1);

                int moneyyy = int.Parse(ds.Tables[0].Rows[0]["money"].ToString());

                if (moneyy > moneyyy)
                {
                    return Json(new { code = "300", message = "您没有这么多钱!" });
                }

                string sql = "UPDATE users SET money = money - " + moneyy + " WHERE cookie = '" + cookie + "'";
                Express ex = new Express();
                if(ex.Execute(sql) != -1)
                {
                    return Json(new { code = "200" });
                }
                else
                {
                    return Json(new { code = "300", message = "提现失败!" });
                }
            }
            return Json(new { code = "300", message = "您不是商家" });
        }
        [HttpPost, Route("shangjia/zhuanchang_upload")]
        public async System.Threading.Tasks.Task<IHttpActionResult> zhuanchang_upload()
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;
            if (jiance.isshangjia(cookie))
            {
                string sql = "SELECT username FROM users WHERE cookie = '" + cookie + "'";
                Express ex = new Express();
                DataSet ds = ex.Query(sql);
                string username = ds.Tables[0].Rows[0]["username"].ToString();
                string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/tmp/zhuanchang/" + username + "/";
                if (!Directory.Exists(rootpath))
                {
                    Directory.CreateDirectory(rootpath);
                }
                string[] fileSystemEntries = Directory.GetFileSystemEntries(rootpath);
                if (fileSystemEntries.Length >= 1)
                {
                    return Json(new { code = "200", Message = "只能上传一个文件" });
                }
                var provider = new MultipartFileWithExtensionStreamProvider(rootpath);
                List<string> files = new List<string>();
                try
                {
                    //执行完这条,文件就保存了
                    await Request.Content.ReadAsMultipartAsync(provider);
                    foreach (var file in provider.FileData)
                    {
                        files.Add(Path.GetFileName(file.LocalFileName));
                    }
                    return Json(new { code = "200", name = files });
                }
                catch (IOException innException)
                {
                    if (innException.InnerException.InnerException.InnerException == null)
                    {
                        return Json(new { code = "500", Message = "文件写入错误" });
                    }
                    return Json(new { code = "500", Message = "文件还是写入错误" });
                }
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("shangjia/zhuanchang_paimai_upload")]
        public async System.Threading.Tasks.Task<IHttpActionResult> zhuanchang_paimai_upload()
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;
            if (jiance.isshangjia(cookie))
            {
                string sql = "SELECT username FROM users WHERE cookie = '" + cookie + "'";
                Express ex = new Express();
                DataSet ds = ex.Query(sql);
                string username = ds.Tables[0].Rows[0]["username"].ToString();
                string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/tmp/zhuanchang_paimai/" + username + "/";
                if (!Directory.Exists(rootpath))
                {
                    Directory.CreateDirectory(rootpath);
                }
                string[] fileSystemEntries = Directory.GetFileSystemEntries(rootpath);
                if (fileSystemEntries.Length >= 1)
                {
                    return Json(new { code = "200", Message = "只能上传一个文件" });
                }
                var provider = new MultipartFileWithExtensionStreamProvider(rootpath);
                List<string> files = new List<string>();

                try
                {
                    //执行完这条,文件就保存了
                    await Request.Content.ReadAsMultipartAsync(provider);
                    foreach (var file in provider.FileData)
                    {
                        files.Add(Path.GetFileName(file.LocalFileName));
                    }
                    return Json(new { code = "200", name = files });
                }
                catch (IOException innException)
                {
                    if (innException.InnerException.InnerException.InnerException == null)
                    {
                        return Json(new { code = "500", Message = "文件写入错误" });
                    }
                    return Json(new { code = "500", Message = "文件还是写入错误" });
                }
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("shangjia/paimai_upload")]
        public async System.Threading.Tasks.Task<IHttpActionResult> paimai_upload()
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;
            if (jiance.isshangjia(cookie))
            {
                string sql1 = "SELECT username FROM users WHERE cookie = '" + cookie + "'";
                Express ex1 = new Express();
                DataSet ds = ex1.Query(sql1);
                string username = ds.Tables[0].Rows[0]["username"].ToString();
                string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/tmp/paimai/" + username + "/";
                if (!Directory.Exists(rootpath))
                {
                    Directory.CreateDirectory(rootpath);
                }
                string[] fileSystemEntries = Directory.GetFileSystemEntries(rootpath);
                if (fileSystemEntries.Length >= 1)
                {
                    return Json(new { code = "200", Message = "只能上传一个文件!" });
                }
                var provider = new MultipartFileWithExtensionStreamProvider(rootpath);
                List<string> files = new List<string>();
                try
                {
                    //执行完这条,文件就保存了
                    await Request.Content.ReadAsMultipartAsync(provider);
                    foreach (var file in provider.FileData)
                    {
                        files.Add(Path.GetFileName(file.LocalFileName));
                    }
                    return Json(new { code = "200", name = files});
                }
                catch (IOException innException)
                {
                    if (innException.InnerException.InnerException.InnerException == null)
                    {
                        return Json(new { code = "500", Message = "文件写入错误" });
                    }
                    return Json(new { code = "500", Message = "文件还是写入错误" });
                }
            }
            else
            {
                return Json(new { code = "500", Message = "你不是商家!" });
            }
        }
        [HttpPost, Route("shangjia/zhuanchang_paimai_xh")]
        public IHttpActionResult zhuanchang_paimai_xh()
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;

            if (jiance.isshangjia(cookie))
            {
                string sql1 = "SELECT username FROM users WHERE cookie = '" + cookie + "'";
                Express ex1 = new Express();
                DataSet ds = ex1.Query(sql1);
                string username = ds.Tables[0].Rows[0]["username"].ToString();
                string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/tmp/zhuanchang_paimai/" + username + "/";
                if (!Directory.Exists(rootpath))
                {
                    Directory.CreateDirectory(rootpath);
                }
                DirectoryInfo theFolder = new DirectoryInfo(rootpath);
                foreach (FileInfo file in theFolder.GetFiles())
                {
                    File.Delete(file.FullName);
                }
                return Json(new { code = "200" });
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("shangjia/zhuanchang_xh")]
        public IHttpActionResult zhuanchang_xh()
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;

            if (jiance.isshangjia(cookie))
            {
                string sql1 = "SELECT username FROM users WHERE cookie = '" + cookie + "'";
                Express ex1 = new Express();
                DataSet ds = ex1.Query(sql1);
                string username = ds.Tables[0].Rows[0]["username"].ToString();
                string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/tmp/zhuanchang/" + username + "/";
                if (!Directory.Exists(rootpath))
                {
                    Directory.CreateDirectory(rootpath);
                }
                DirectoryInfo theFolder = new DirectoryInfo(rootpath);
                foreach (FileInfo file in theFolder.GetFiles())
                {
                    File.Delete(file.FullName);
                }
                return Json(new { code = "200" });
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("shangjia/paimai_xh")]
        public IHttpActionResult paimai_xh()
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;

            if (jiance.isshangjia(cookie))
            {
                string sql1 = "SELECT username FROM users WHERE cookie = '" + cookie + "'";
                Express ex1 = new Express();
                DataSet ds = ex1.Query(sql1);
                string username = ds.Tables[0].Rows[0]["username"].ToString();
                string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/tmp/paimai/" + username + "/";
                if (!Directory.Exists(rootpath))
                {
                    Directory.CreateDirectory(rootpath);
                }
                DirectoryInfo theFolder = new DirectoryInfo(rootpath);
                foreach (FileInfo file in theFolder.GetFiles())
                {
                    File.Delete(file.FullName);
                }
                return Json(new { code = "200" });
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("shangjia/shangjia_zhuanchang")]
        public IHttpActionResult shangjia_zhuanchang(JObject jsonObj)
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;

            Express ex1 = new Express();
            string sql1 = "SELECT username, number FROM users WHERE cookie = '" + cookie + "'";
            DataSet ds = ex1.Query(sql1);
            string username = ds.Tables[0].Rows[0]["username"].ToString();
            string number = ds.Tables[0].Rows[0]["number"].ToString();

            // 检验是否上传了文件
            string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/tmp/zhuanchang_paimai/" + username + "/";
            string[] fileSystemEntries = Directory.GetFileSystemEntries(rootpath);
            if (fileSystemEntries.Length != 1)
            {
                return Json(new { code = "500", message = "请先上传文件!" });
            }

            string filepath = fileSystemEntries[0];

            var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

            string zname = jsonParams.zname;
            string name = jsonParams.name;
            int startprice = jsonParams.startprice;
            int gapprice = jsonParams.gapprice;

            Express exx = new Express();
            string sqll = "SELECT fzperson,starttime,endtime FROM zc WHERE number = '" + number + "' AND zname = '" + zname + "'";
            DataSet dss = exx.Query(sqll);
            string fzperson = dss.Tables[0].Rows[0]["fzperson"].ToString();
            string zc_starttime = dss.Tables[0].Rows[0]["starttime"].ToString();
            string zc_endtime = dss.Tables[0].Rows[0]["endtime"].ToString();

            string[] time = new string[2];
            time[0] = jsonParams.time[0];
            time[1] = jsonParams.time[1];

            if (zname.Length > 40 || zname.IndexOf('\'') != -1)
            {
                return Json(new { code = "500", message = "专场名格式不正确" });
            }
            if (name.Length > 40 || name.IndexOf('\'') != -1)
            {
                return Json(new { code = "500", message = "物品名格式不正确" });
            }
            
            if(startprice > 100000 || startprice < 500 || gapprice < 100 || gapprice > 10000)
            {
                return Json(new { code = "500", message = "价格区间不正确" });
            }

            DateTime startdt = Convert.ToDateTime(time[0]);
            DateTime enddt = Convert.ToDateTime(time[1]);
            DateTime zc_startdt = Convert.ToDateTime(zc_starttime);
            DateTime zc_enddt = Convert.ToDateTime(zc_endtime);

            if ((int)(zc_startdt - startdt).TotalSeconds >= 0 || (int)(zc_enddt - enddt).TotalSeconds <= 0)
            {
                return Json(new { code = "500", message = "拍卖品拍卖时间需要在专场开启时间之间" });
            }

            string filename = filepath.Substring(filepath.LastIndexOf("/"));
            string newdict = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/zhuanchang_paimai/" + username;

            if (!Directory.Exists(newdict))
            {
                Directory.CreateDirectory(newdict);
            }

            string newpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/zhuanchang_paimai/" + username + "/" + filename;

            File.Move(filepath, newpath);

            string url = newpath.Substring(newpath.IndexOf("/static"));

            Express ex2 = new Express();
            string sql2 = "INSERT INTO zhuan_chang (zname,number,name,fzperson,startprice,nowprice,gapprice,starttime,endtime,url,cishu) VALUES ('" + zname + "','" + number + "','" + name + "','" + fzperson + "'," + startprice + "," + startprice + "," + gapprice + ",'" + time[0] + "','" + time[1] + "','" + url + "',0)";
            if (ex2.Execute(sql2) == -1)
            {
                return Json(new { code = "500", message = "数据库异常,请稍后再试" });
            }
            return Json(new { code = "200" });

        }
        [HttpPost, Route("shangjia/zhuanchang_submit")]
        public IHttpActionResult zhuanchang_submit(JObject jsonObj)
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;

            Express ex1 = new Express();
            string sql1 = "SELECT username, number FROM users WHERE cookie = '" + cookie + "'";
            DataSet ds = ex1.Query(sql1);
            string username = ds.Tables[0].Rows[0]["username"].ToString();
            string number = ds.Tables[0].Rows[0]["number"].ToString();

            // 检验是否上传了文件
            string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/tmp/zhuanchang/" + username + "/";
            string[] fileSystemEntries = Directory.GetFileSystemEntries(rootpath);
            if (fileSystemEntries.Length != 1)
            {
                return Json(new { code = "500", message = "请先上传文件!" });
            }

            string filepath = fileSystemEntries[0];

            var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

            string zname = jsonParams.zname;
            string fzperson = jsonParams.fzperson;
            string[] time = new string[2];
            time[0] = jsonParams.time[0];
            time[1] = jsonParams.time[1];

            if (zname.Length > 40 || zname.IndexOf('\'') != -1)
            {
                return Json(new { code = "500", message = "专场名格式不正确" });
            }
            if (fzperson.Length > 20 || fzperson.IndexOf('\'') != -1)
            {
                return Json(new { code = "500", message = "人名格式不正确" });
            }

            string nowtime = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

            DateTime nowdt = Convert.ToDateTime(nowtime);
            DateTime startdt = Convert.ToDateTime(time[0]);
            DateTime enddt = Convert.ToDateTime(time[1]);

            if ((int)(startdt - nowdt).TotalSeconds < (60 * 60 * 24 * 2))
            {
                return Json(new { code = "500", message = "专场开启时间必须在当前时间两天后" });
            }

            if ((int)(enddt - startdt).TotalSeconds < (60 * 60 * 24 * 5) || (int)(enddt - startdt).TotalSeconds > (60 * 60 * 24 * 30))
            {
                return Json(new { code = "500", message = "专场持续时间为五天到三十天之间" });
            }

            string filename = filepath.Substring(filepath.LastIndexOf("/"));
            string newpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/zhuanchang/" + filename;
            File.Move(filepath, newpath);

            string url = newpath.Substring(newpath.IndexOf("/static"));

            Express ex2 = new Express();
            string sql2 = "INSERT INTO zc VALUES ('" + zname + "', '" + number + "', '" + fzperson + "', '" + time[0] + "', '" + time[1] + "', '" + url + "')";
            if (ex2.Execute(sql2) == -1)
            {
                return Json(new { code = "500", message = "数据库异常,请稍后再试" });
            }
            return Json(new { code = "200" });
        }
        [HttpPost, Route("shangjia/paimai_submit")]
        public IHttpActionResult paimai_submit(JObject jsonObj)
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;

            Express ex1 = new Express();
            string sql1 = "SELECT username, number FROM users WHERE cookie = '" + cookie + "'";
            DataSet ds = ex1.Query(sql1);
            string username = ds.Tables[0].Rows[0]["username"].ToString();
            string number = ds.Tables[0].Rows[0]["number"].ToString();

            // 检验是否上传了文件
            string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/tmp/paimai/" + username + "/";
            string[] fileSystemEntries = Directory.GetFileSystemEntries(rootpath);
            if (fileSystemEntries.Length != 1)
            {
                return Json(new { code = "500", message = "请先上传文件!" });
            }

            string filepath = fileSystemEntries[0];

            var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

            string name = jsonParams.name;
            string type = jsonParams.type;
            int startprice = jsonParams.startprice;
            int gapprice = jsonParams.gapprice;
            string[] time = new string[2];
            time[0] = jsonParams.time[0];
            time[1] = jsonParams.time[1];
            
            if (name.Length > 40 || name.IndexOf('\'') != -1)
            {
                return Json(new { code = "500", message = "拍品名格式不正确" });
            }
            string[] tt = { "中国绘画", "古瓷杂项", "酒品", "西画雕塑", "当代工艺", "书法篆刻" };
            if (Array.IndexOf(tt, type) == -1)
            {
                return Json(new { code = "500", message = "拍品类型不正确" });
            }

            if (startprice < 500 || startprice > 100000)
            {
                return Json(new { code = "500", message = "起始价格不在区间内" });
            }

            if (gapprice < 100 || gapprice > 10000)
            {
                return Json(new { code = "500", message = "竞价阶梯不在区间内" });
            }

            string nowtime = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

            DateTime nowdt = Convert.ToDateTime(nowtime);
            DateTime startdt = Convert.ToDateTime(time[0]);
            DateTime enddt = Convert.ToDateTime(time[1]);

            if ((int)(startdt - nowdt).TotalSeconds < (60 * 60 * 24 * 2))
            {
                return Json(new { code = "500", message = "起拍时间必须在当前时间两天后"});
            }

            if ((int)(enddt - startdt).TotalSeconds < (60 * 60 * 24 * 2) || (int)(enddt - startdt).TotalSeconds > (60 * 60 * 24 * 7))
            {
                return Json(new { code = "500", message = "竞拍时间为两天到七天之间"});
            }

            string filename = filepath.Substring(filepath.LastIndexOf("/"));
            string newpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/pai_mai" + filename;
            File.Move(filepath, newpath);

            string url = newpath.Substring(newpath.IndexOf("/static"));

            Express ex2 = new Express();
            string sql2 = "INSERT INTO pai_mai(name, type, startprice, nowprice, gapprice, starttime, endtime, url, number, cishu) VALUES ('" + name + "','" + type + "'," + startprice + "," + startprice + "," + gapprice + ",'" + time[0] + "','" + time[1] + "','" + url + "','" + number + "',0)";
            if (ex2.Execute(sql2) == -1)
            {
                return Json(new { code = "500", message = "数据库异常,请稍后再试"});
            }
            return Json(new { code = "200" });
        }
        [HttpPost, Route("shangjia/paimaipin")]
        public IHttpActionResult paimaipin()
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;
            if (jiance.isshangjia(cookie))
            {
                Express ex = new Express();
                string sql = "SELECT number FROM users WHERE cookie = '" + cookie + "'";
                DataSet ds = ex.Query(sql);
                string number = ds.Tables[0].Rows[0]["number"].ToString();

                Express ex1 = new Express();
                string sql1 = "SELECT * FROM pai_mai WHERE number = '" + number + "'";
                DataSet ds1 = ex1.Query(sql1);
                return Ok(ds1);
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("shangjia/zhuanchang_get")]
        public IHttpActionResult zhuanchang_get()
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;
            if (jiance.isshangjia(cookie))
            {
                Express ex = new Express();
                string sql = "SELECT number FROM users WHERE cookie = '" + cookie + "'";
                DataSet ds = ex.Query(sql);
                string number = ds.Tables[0].Rows[0]["number"].ToString();

                Express ex1 = new Express();
                string sql1 = "SELECT * FROM zc WHERE number = '" + number + "'";
                DataSet ds1 = ex1.Query(sql1);
                return Ok(ds1);
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("shangjia/getzcpm")]
        public IHttpActionResult getzcpm(JObject jsonObj)
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;
            if (jiance.isshangjia(cookie))
            {
                Express ex1 = new Express();
                string sql1 = "SELECT number FROM users WHERE cookie = '" + cookie + "'";
                DataSet ds = ex1.Query(sql1);
                string number = ds.Tables[0].Rows[0]["number"].ToString();

                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string zname = jsonParams.zname;

                Express ex = new Express();
                string sql = "SELECT * FROM zhuan_chang WHERE number = '" + number + "' AND zname = '" + zname + "'";

                DataSet ds1 = ex.Query(sql);

                return Ok(ds1);
            } else
            {
                return Ok();
            }
        }
        [HttpPost, Route("shangjia/jingpai")]
        public IHttpActionResult jingpai()
        {
            string cookie = HttpContext.Current.Request.Cookies["c1"].Value;
            if (jiance.isshangjia(cookie))
            {

                Express ex = new Express();
                string sql = "SELECT number FROM users WHERE cookie = '" + cookie + "'";
                DataSet ds = ex.Query(sql);
                string number = ds.Tables[0].Rows[0]["number"].ToString();

                Express ex1 = new Express();
                string sql1 = "SELECT * FROM chengjiao WHERE number = '" + number + "'";
                DataSet ds1 = ex1.Query(sql1);
                return Ok(ds1);
            }
            return Ok();
        }
        [HttpPost, Route("guanli/denglu")]
        public HttpResponseMessage guanli_denglu(JObject jsonObj)
        {
            var jsonStr = JsonConvert.SerializeObject(jsonObj);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            string username = jsonParams.username;
            string password = jsonParams.password;

            string pattern = @"\A[a-zA-Z0-9]{4,18}\z";

            HttpResponseMessage response1 = new HttpResponseMessage(HttpStatusCode.OK);

            if (Regex.IsMatch(username, pattern) && Regex.IsMatch(password, pattern))
            {
                Express ex = new Express();
                string sql = "SELECT * FROM guanliyuan WHERE username = '" + username + "' AND password = '" + password + "'";
                DataSet ds = ex.Query(sql);
                if (ds == null || ds.Tables.Count == 0 || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
                {

                    Message ms = new Message();
                    ms.Code = 300;
                    ms.Mess = "账号密码不正确";
                    string resultString = JsonConvert.SerializeObject(ms);
                    response1.Content = new StringContent(resultString);

                }
                else
                {
                    Random rd = new Random(((int)DateTime.Now.Ticks));
                    int rdd = rd.Next(100000, 999999);
                    string st = username + DateTime.Now.ToFileTimeUtc().ToString() + rdd + password;
                    string str = FormsAuthentication.HashPasswordForStoringInConfigFile(st, "MD5").ToLower();
                    string sql1 = "UPDATE guanliyuan SET cookie = '" + str + "' WHERE username = '" + username + "' AND password = '" + password + "'";
                    Express ex1 = new Express();
                    ex1.Execute(sql1);

                    CookieHeaderValue cookie = new CookieHeaderValue("g1", str);
                    cookie.Path = "/";
                    cookie.Domain = "localhost";
                    cookie.HttpOnly = true;
                    response1.Headers.AddCookies(new CookieHeaderValue[] { cookie });

                    Message ms = new Message();
                    ms.Code = 200;
                    ms.Mess = "登录成功";
                    response1.Content = new StringContent(JsonConvert.SerializeObject(ms));
                }
                return response1;
            }
            else
            {
                Message ms = new Message();
                ms.Code = 300;
                ms.Mess = "账号密码不符合规范";
                string resultString = JsonConvert.SerializeObject(ms);
                response1.Content = new StringContent(resultString);
                return response1;
            }
        }
        [HttpPost, Route("guanli/panduan")]
        public IHttpActionResult guanli_panduan()
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (guanli_yanzheng(hc))
            {
                string cookie = hc.Value;
                Express ex = new Express();
                string sql = "SELECT quanxian FROM guanliyuan WHERE cookie = '" + cookie + "'";
                DataSet ds =  ex.Query(sql);
                return Ok(ds);
            }
            else
            {
                return Ok("false");
            }
        }
        public static Boolean guanli_yanzheng(HttpCookie hc)
        {
            if (hc == null)
            {
                return false;
            }
            else
            {
                string cookie = hc.Value;
                Express ex1 = new Express();
                string sql1 = "SELECT * FROM guanliyuan WHERE cookie = '" + cookie + "'";
                DataSet ds1 = ex1.Query(sql1);
                if (ds1 == null || ds1.Tables.Count == 0 || (ds1.Tables.Count == 1 && ds1.Tables[0].Rows.Count == 0))
                {
                    return false;
                }
            }
            return true;
        }
        [HttpPost, Route("guanli/guanlidelete")]
        public IHttpActionResult guanlidelete(JObject jsonObj)
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (guanli_yanzheng(hc))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string username = jsonParams.username;
                string password = jsonParams.password;
                int quanxian = jsonParams.quanxian;

                if (jiance.login_guanli(username) && jiance.login_guanli(password))
                {
                    string cookie = hc.Value;
                    Express ex1 = new Express();
                    string sql = "SELECT quanxian FROM guanliyuan WHERE cookie = '" + cookie + "'";
                    DataSet ds1 = ex1.Query(sql);
                    int quanxian_wode = int.Parse(ds1.Tables[0].Rows[0]["quanxian"].ToString());

                    if (quanxian <= quanxian_wode || quanxian > 100)
                    {
                        return Json(new { code = "300", message = "权限不符合规则" });
                    }
                    Express ex2 = new Express();
                    string sql2 = "DELETE FROM guanliyuan WHERE username = '" + username + "'AND password = '" + password + "'";
                    if(ex2.Execute(sql2) != -1)
                    {
                        return Json(new { code = "200" });
                    }
                    return Json(new { code = "300", message = "数据库异常" });
                }
                return Json(new { code = "300", message = "账号或者密码不符合规则" });
            }
            return Ok();
        }
        [HttpPost, Route("guanli/quanxianchange")]
        public IHttpActionResult quanxianchange(JObject jsonObj)
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (guanli_yanzheng(hc))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string username = jsonParams.username;
                string password = jsonParams.password;
                int quanxian = jsonParams.num;

                if (jiance.login_guanli(username) && jiance.login_guanli(password))
                {
                    string cookie = hc.Value;
                    Express ex1 = new Express();
                    string sql = "SELECT quanxian FROM guanliyuan WHERE cookie = '" + cookie + "'";
                    DataSet ds1 = ex1.Query(sql);
                    int quanxian_wode = int.Parse(ds1.Tables[0].Rows[0]["quanxian"].ToString());

                    if (quanxian <= quanxian_wode || quanxian > 100)
                    {
                        return Json(new { code = "300", message = "权限不符合规则" });
                    }

                    Express ex2 = new Express();
                    string sql2 = "UPDATE guanliyuan SET quanxian = " + quanxian + " WHERE username = '" + username + "' AND password = '" + password + "'";
                    if (ex2.Execute(sql2) != -1)
                    {
                        return Json(new { code = "200" });
                    }
                    return Json(new { code = "300", message = "数据库异常" });
                }
                return Json(new { code = "300", message = "账号或者密码不符合规则" });
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("guanli/tianjia")]
        public IHttpActionResult guanli_tianjia(JObject jsonObj)
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (guanli_yanzheng(hc))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string username = jsonParams.username;
                string password = jsonParams.password;
                int quanxian = jsonParams.num;

                if(jiance.login_guanli(username) && jiance.login_guanli(password))
                {
                    string cookie = hc.Value;
                    Express ex1 = new Express();
                    string sql = "SELECT quanxian FROM guanliyuan WHERE cookie = '" + cookie + "'";
                    DataSet ds1 = ex1.Query(sql);
                    int quanxian_wode = int.Parse(ds1.Tables[0].Rows[0]["quanxian"].ToString());

                    if (quanxian <= quanxian_wode || quanxian > 100)
                    {
                        return Json(new { code = "300", message = "权限不符合规则" });
                    }

                    Express ex2 = new Express();
                    string sql2 = "INSERT INTO guanliyuan (username, password, quanxian) VALUES ('" + username + "','" + password + "'," + quanxian + ")";
                    if (ex2.Execute(sql2) != -1)
                    {
                        return Json(new { code = "200" });
                    }
                    return Json(new { code ="300", message = "数据库异常"});
                }
                return Json(new { code = "300", message = "账号或者密码不符合规则"});

            }
            else
            {
                return Ok("");
            }
        }
        [HttpPost, Route("guanli/tongyi")]
        public IHttpActionResult tongyi(JObject jsonObj)
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (ProductsController.guanli_yanzheng(hc))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string users = jsonParams.users;
                string gname = jsonParams.gname;
                string address = jsonParams.address;
                string phonenumber = jsonParams.phonenumber;
                string source = jsonParams.source[0];

                Express ex1 = new Express();
                string sql1 = "SELECT * FROM users WHERE username = '" + users + "'";
                DataSet ds1 = ex1.Query(sql1);
                if (ds1 == null || ds1.Tables.Count == 0 || (ds1.Tables.Count == 1 && ds1.Tables[0].Rows.Count == 0))
                {
                    return Json(new { code = "300", message = "账户不存在" });
                }
                if (int.Parse(ds1.Tables[0].Rows[0]["state"].ToString()) != 2)
                {
                    return Json(new { code = "300", message = "账户不符合规则" });
                }
                //删除文件
                string url = source;
                string[] urls = url.Split('/');
                string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/cwsj/";
                DirectoryInfo theFolder = new DirectoryInfo(rootpath);
                // 待改动
                foreach (FileInfo file in theFolder.GetFiles())
                {
                    File.Delete(file.FullName);
                }
                // Directory.Delete(rootpath);

                string number = "";
                while (true)
                {
                    number = sj_number();
                    Express ex3 = new Express();
                    string sql3 = "SELECT * FROM users WHERE number = '" + number + "'";
                    DataSet ds3 = ex3.Query(sql3);
                    if(ds3.Tables[0].Rows.Count == 0)
                    {
                        break;
                    } else
                    {
                        continue;
                    }
                }
                

                Express ex2 = new Express();
                string sql2 = "UPDATE users SET state = 3, number = '" + number + "',gname = '" + gname + "',address = '" + address + "',phonenumber = '" + phonenumber + "' WHERE username = '" + users + "'";

                Express ex4 = new Express();
                string sql4 = "DELETE FROM shenhee WHERE users = '" + users + "'";

                if (ex2.Execute(sql2) != -1 && ex4.Execute(sql4) != -1)
                {
                    return Json(new { code = "200" });
                }
                return Json(new { code = "300", message = "数据库异常" });
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("guanli/bohui")]
        public IHttpActionResult bohui(JObject jsonObj)
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (ProductsController.guanli_yanzheng(hc))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string users = jsonParams.users;
                string source = jsonParams.source[0];

                Express ex1 = new Express();
                string sql1 = "SELECT * FROM users WHERE username = '" + users + "'";
                DataSet ds1 = ex1.Query(sql1);
                if (ds1 == null || ds1.Tables.Count == 0 || (ds1.Tables.Count == 1 && ds1.Tables[0].Rows.Count == 0))
                {
                    return Json(new { code = "300", message = "账户不存在" });
                }
                if (int.Parse(ds1.Tables[0].Rows[0]["state"].ToString()) != 2)
                {
                    return Json(new { code = "300", message = "账户不符合规则" });
                }
                //删除文件
                string url = source;
                string[] urls = url.Split('/');
                string rootpath = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc/static/cwsj/";
                DirectoryInfo theFolder = new DirectoryInfo(rootpath);
                foreach (FileInfo file in theFolder.GetFiles())
                {
                    File.Delete(file.FullName);
                }
                //Directory.Delete(rootpath);


                Express ex2 = new Express();
                string sql2 = "UPDATE users SET state = 1 WHERE username = '" + users + "'";

                Express ex4 = new Express();
                string sql4 = "DELETE FROM shenhee WHERE users = '" + users + "'";

                if (ex2.Execute(sql2) != -1 && ex4.Execute(sql4) != -1)
                {
                    return Json(new { code = "200" });
                }
                return Json(new { code = "300", message = "数据库异常" });
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("guanli/chongzhi")]
        public IHttpActionResult chongzhi_guanli(JObject jsonObj)
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (ProductsController.guanli_yanzheng(hc))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string ma = jsonParams.ma;
                int money = jsonParams.money;

                if (jiance.chongzhi(ma))
                {
                    if(money < 100 || money > 1000 || money % 100 != 0)
                    {
                        return Json(new { code = "300", message = "充值金额不符合规则" });
                    }

                    Express ex = new Express();
                    string sql = "INSERT INTO chongzhi VALUES ('" + ma + "'," + money + ")";
                    if (ex.Execute(sql) == -1)
                    {
                        return Json(new { code = "300", message = "数据库异常" });
                    }
                    return Json(new { code = "200" });
                }
                return Json(new { code = "300", message = "充值码不符合规则" });

            }
            return Ok();
        }
        [HttpPost, Route("guanli/paimaipin")]
        public IHttpActionResult paimaipin_guanli()
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (guanli_yanzheng(hc))
            {
                Express ex = new Express();
                string sql = "SELECT * FROM pai_mai";
                DataSet ds = ex.Query(sql);
                return Ok(ds);
            } 
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("guanli/zhuanchangpin")]
        public IHttpActionResult zhuanchangpin_guanli()
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (guanli_yanzheng(hc))
            {
                /*Express ex = new Express();
                string sql = "SELECT * FROM zc";
                DataSet ds = ex.Query(sql);

                List<Dictionary<string, DataSet>> list = new List<Dictionary<string, DataSet>>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string zname = ds.Tables[0].Rows[i]["zname"].ToString();
                    string number = ds.Tables[0].Rows[i]["number"].ToString();

                    Express ex2 = new Express();
                    string sql2 = "SELECT * FROM zhuan_chang WHERE zname = '" + zname + "' AND number = '" + number + "'";
                    DataSet ds2 = ex2.Query(sql2);

                    Dictionary<string, DataSet> dict = new Dictionary<string, DataSet>();

                    dict.Add(zname, ds2);

                    list.Add(dict);
                }
                */
                Express ex = new Express();
                string sql = "SELECT * FROM zhuan_chang";
                DataSet ds = ex.Query(sql);

                List<Dictionary<string,string>> list = new List<Dictionary<string,string>>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();

                    dict.Add("zname", ds.Tables[0].Rows[i]["zname"].ToString());
                    dict.Add("number", ds.Tables[0].Rows[i]["number"].ToString());
                    dict.Add("name", ds.Tables[0].Rows[i]["name"].ToString());
                    dict.Add("fzperson", ds.Tables[0].Rows[i]["fzperson"].ToString());
                    dict.Add("startprice", ds.Tables[0].Rows[i]["startprice"].ToString());
                    dict.Add("nowprice", ds.Tables[0].Rows[i]["nowprice"].ToString());
                    dict.Add("gapprice", ds.Tables[0].Rows[i]["gapprice"].ToString());
                    dict.Add("starttime", ds.Tables[0].Rows[i]["starttime"].ToString());
                    dict.Add("endtime", ds.Tables[0].Rows[i]["endtime"].ToString());
                    dict.Add("person", ds.Tables[0].Rows[i]["person"].ToString());
                    dict.Add("url", ds.Tables[0].Rows[i]["url"].ToString());
                    dict.Add("cishu", ds.Tables[0].Rows[i]["cishu"].ToString());

                    list.Add(dict);
                }

                return Ok(list);

            }
            else
            {
                return Ok();
            }
        }
        public string sj_number()
        {
            string urls = "";
            Random rd = new Random();
            for (int i = 0; i < 6; i++)
            {
                urls = urls + rd.Next(1, 10).ToString();
            }
            return urls;
        }
        [HttpGet, Route("guanli/test")]
        public IHttpActionResult guanli_test()
        {
            string urls = "";
            Random rd = new Random();
            for (int i = 0; i < 6; i++)
            {
                urls = urls + rd.Next(1, 10).ToString();
            }
            return Ok(urls);
        }
        [HttpPost, Route("guanli/jiefeng")]
        public IHttpActionResult jiefeng(JObject jsonObj)
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (ProductsController.guanli_yanzheng(hc))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string username = jsonParams.username;
                string password = jsonParams.password;
                if (jiance.login_guanli(username) && jiance.login_guanli(password))
                {
                    Express ex1 = new Express();
                    string sql1 = "SELECT * FROM users WHERE username = '" + username + "' AND password = '" + password + "'";
                    DataSet ds1 = ex1.Query(sql1);
                    if (ds1 == null || ds1.Tables.Count == 0 || (ds1.Tables.Count == 1 && ds1.Tables[0].Rows.Count == 0))
                    {
                        return Json(new { code = "300", message = "要解封的账户不存在" });
                    }
                    int state = int.Parse(ds1.Tables[0].Rows[0]["state"].ToString());
                    int cstate = 0;
                    if (state == 4)
                    {
                        cstate = 1;
                    }
                    else if (state == 5)
                    {
                        cstate = 3;
                    }
                    else
                    {
                        return Json(new { code = "300", message = "此账户目前无法进行此操作" });
                    }
                    Express ex2 = new Express();
                    string sql2 = "UPDATE users SET state = " + cstate + " WHERE username = '" + username + "' AND password = '" + password + "'";
                    if (ex2.Execute(sql2) != -1)
                    {
                        return Json(new { code = "200" });
                    }
                    else
                    {
                        return Json(new { code = "300", message = "数据库异常" });
                    }
                }
                return Json(new { code = "300", message = "账号或者密码不符合规则" });

            }
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("guanli/fengjin")]
        public IHttpActionResult fengjin(JObject jsonObj)
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (ProductsController.guanli_yanzheng(hc))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string username = jsonParams.username;
                string password = jsonParams.password;
                if (jiance.login_guanli(username) && jiance.login_guanli(password))
                {
                    Express ex1 = new Express();
                    string sql1 = "SELECT * FROM users WHERE username = '" + username + "' AND password = '" + password + "'";
                    DataSet ds1 = ex1.Query(sql1);
                    if (ds1 == null || ds1.Tables.Count == 0 || (ds1.Tables.Count == 1 && ds1.Tables[0].Rows.Count == 0))
                    {
                        return Json(new { code = "300", message = "要封禁的账户不存在" });
                    }
                    int state = int.Parse(ds1.Tables[0].Rows[0]["state"].ToString());
                    int cstate = 0;
                    if (state == 1)
                    {
                        cstate = 4;
                    } 
                    else if (state == 3)
                    {
                        cstate = 5;
                    } 
                    else
                    {
                        return Json(new { code = "300", message = "此账户目前无法进行此操作" });
                    }
                    Express ex2 = new Express();
                    string sql2 = "UPDATE users SET state = " + cstate + ",cookie = null" + " WHERE username = '" + username + "' AND password = '" + password + "'";
                    if (ex2.Execute(sql2) != -1)
                    {
                        return Json(new { code = "200" });
                    }
                    else
                    {
                        return Json(new { code = "300", message = "数据库异常"});
                    }
                }
                return Json(new { code = "300", message = "账号或者密码不符合规则" });

            } 
            else
            {
                return Ok();
            }
        }
        [HttpPost, Route("guanli/cwsjhuoqu")]
        public IHttpActionResult cwsjhuoqu()
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (ProductsController.guanli_yanzheng(hc))
            {
                Express ex1 = new Express();
                string sql1 = "SELECT * FROM shenhee WHERE type = 'cwsj'";
                DataSet ds1 = ex1.Query(sql1);

                List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    string users = ds1.Tables[0].Rows[i]["users"].ToString();
                    string data = ds1.Tables[0].Rows[i]["data"].ToString();
                    string source = ds1.Tables[0].Rows[i]["source"].ToString();

                    JObject jsonText = (JObject)JsonConvert.DeserializeObject(data);
                    string gname = jsonText["gname"].ToString();
                    string address = jsonText["address"].ToString();
                    string phonenumber = jsonText["phonenumber"].ToString();

                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("users", users);
                    dict.Add("gname", gname);
                    dict.Add("address", address);
                    dict.Add("phonenumber", phonenumber);
                    dict.Add("source", source);
                    list.Add(dict);
                }
                return Ok(list);

            } else
            {
                return Ok();
            }
        }
        [HttpPost, Route("shangjia/zc_delete")]
        public IHttpActionResult zc_delete(JObject jsonObj)
        {
            var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

            string zname = jsonParams.zname;
            string url = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc" + jsonParams.url;

            Express ex = new Express();
            string sql = "DELETE FROM zc WHERE zname = '" + zname + "'";

            ex.Execute(sql);

            Express ex1 = new Express();
            string sql1 = "DELETE FROM zhuan_chang WHERE zname = '" + zname + "'";
            ex1.Execute(sql1);

            return Ok();
        }
        [HttpPost, Route("guanli/zcdelete")]
        public IHttpActionResult zcdelete(JObject jsonObj)
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (ProductsController.guanli_yanzheng(hc))
            {
                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string zname = jsonParams.zname;
                string number = jsonParams.number;
                string name = jsonParams.name;

                string url = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc" + jsonParams.url;

                Express ex = new Express();
                string sql = "SELECT * FROM zhuan_chang WHERE zname = '" + zname + "' AND number = '" + number + "' AND name = '" + name + "'";

                DataSet ds1 = ex.Query(sql);
                if (ds1 == null || ds1.Tables.Count == 0 || (ds1.Tables.Count == 1 && ds1.Tables[0].Rows.Count == 0))
                {
                    return Json(new { code = "300", message = "拍卖品不存在"});
                }

                Express ex2 = new Express();
                string sql2 = "DELETE FROM zhuan_chang WHERE zname = '" + zname + "' AND number = '" + number + "' AND name = '" + name + "'";
                if (ex2.Execute(sql2) != -1) 
                {
                    File.Delete(url);
                    return Json(new { code = "200" });
                }
                else
                {
                    return Json(new { code = "300", message = "数据库错误!"});
                }
            }
            return Ok();
        }
        [HttpPost, Route("guanli/paimaidelete")]
        public IHttpActionResult paimaidelete(JObject jsonObj)
        {
            // HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            // if (ProductsController.guanli_yanzheng(hc))
            // {
                var jsonStr = JsonConvert.SerializeObject(jsonObj); //序列化为json字符串
                var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);   //反序列化为动态jsob object

                string name = jsonParams.name;
                string number = jsonParams.number;
                string url = jsonParams.url;

                Express ex = new Express();
                string sql = "DELETE FROM pai_mai WHERE name = '" + name + "' AND number = '" + number + "'";
                if (ex.Execute(sql) == -1)
                {
                    return Json(new { code = "300", message = "数据库异常" });
                }
                else
                {
                    url = "C:/me/securitytool/IDE/Microsoft VS Code/workspace/vue-lc" + url;
                    File.Delete(url);
                    return Json(new { code = "200" });
                }


            // }
            // return Ok();
        }
        [HttpPost, Route("guanli/chengyuanhuoqu")]
        public IHttpActionResult chengyuanhuoqu_guanli()
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies["g1"];
            if (ProductsController.guanli_yanzheng(hc))
            {
                string cookie = hc.Value;
                Express ex1 = new Express();
                string sql1 = "SELECT quanxian FROM guanliyuan WHERE cookie = '" + cookie + "'";

                DataSet ds1 = ex1.Query(sql1);
                int quanxian = int.Parse(ds1.Tables[0].Rows[0]["quanxian"].ToString());

                Express ex2 = new Express();
                string sql2 = "SELECT username,password,quanxian FROM guanliyuan WHERE quanxian > " + quanxian;
                DataSet ds2 = ex2.Query(sql2);

                Express ex3 = new Express();
                string sql3 = "SELECT  username,password,name,money,state FROM users WHERE state = 1 OR state = 4";
                DataSet ds3 = ex3.Query(sql3);

                List<Dictionary<string,string>> list_cy = new List<Dictionary<string,string>>();

                for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                {
                    string username = ds3.Tables[0].Rows[i]["username"].ToString();
                    string password = ds3.Tables[0].Rows[i]["password"].ToString();
                    string name = ds3.Tables[0].Rows[i]["name"].ToString();
                    string money = ds3.Tables[0].Rows[i]["money"].ToString();
                    string state = ds3.Tables[0].Rows[i]["state"].ToString();

                    Express ex5 = new Express();
                    string sql5 = "SELECT count(*) AS count FROM chengjiao WHERE person = '" + name + "'";
                    DataSet ds5 = ex5.Query(sql5);
                    string count = ds5.Tables[0].Rows[0]["count"].ToString();

                    Dictionary<string,string> dict = new Dictionary<string, string>();

                    dict.Add("username",username);
                    dict.Add("password",password);
                    dict.Add("name", name);
                    dict.Add("money", money);
                    dict.Add("state", state);
                    dict.Add("count", count);
                    list_cy.Add(dict);
                }

                Express ex4 = new Express();
                string sql4 = "SELECT username,password,money,name,number,gname,address,phonenumber,state FROM users WHERE state = 3 OR state = 5";
                DataSet ds4 = ex4.Query(sql4);

                List<Dictionary<string, string>> list_sj = new List<Dictionary<string, string>>();

                for (int j = 0; j < ds4.Tables[0].Rows.Count; j++)
                {
                    string username = ds4.Tables[0].Rows[j]["username"].ToString();
                    string password = ds4.Tables[0].Rows[j]["password"].ToString();
                    string name = ds4.Tables[0].Rows[j]["name"].ToString();
                    string money = ds4.Tables[0].Rows[j]["money"].ToString();
                    string number = ds4.Tables[0].Rows[j]["number"].ToString();
                    string gname = ds4.Tables[0].Rows[j]["gname"].ToString();
                    string address = ds4.Tables[0].Rows[j]["address"].ToString();
                    string phonenumber = ds4.Tables[0].Rows[j]["phonenumber"].ToString();
                    string state = ds4.Tables[0].Rows[j]["state"].ToString();

                    Express ex6 = new Express();
                    string sql6 = "SELECT count(*) AS count FROM pai_mai WHERE number = '" + number + "'";
                    DataSet ds6 = ex6.Query(sql6);

                    string count_pai = ds6.Tables[0].Rows[0]["count"].ToString();

                    Express ex7 = new Express();
                    string sql7 = "SELECT count(*) AS count FROM zc WHERE number = '" + number + "'";
                    DataSet ds7 = ex7.Query(sql7);

                    string count_zhuan = ds7.Tables[0].Rows[0]["count"].ToString();

                    Dictionary<string, string> dict = new Dictionary<string, string>();

                    dict.Add("username", username);
                    dict.Add("password", password);
                    dict.Add("name",name);
                    dict.Add("money", money);
                    dict.Add("number",number);
                    dict.Add("gname", gname);
                    dict.Add("address", address);
                    dict.Add("phonenumber", phonenumber);
                    dict.Add("count_pai", count_pai);
                    dict.Add("count_zhuan", count_zhuan);
                    dict.Add("state", state);
                    list_sj.Add(dict);
                }

                return Json(new { gly = ds2, cy = list_cy, sj = list_sj});

            }
            else
            {
                return Ok();
            }
        }
        public class MultipartFileWithExtensionStreamProvider : MultipartFileStreamProvider
        {
            public MultipartFileWithExtensionStreamProvider(string rootPath)
                : base(rootPath) { }
            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                string Name = headers.ContentDisposition.FileName.Replace("\"", string.Empty);

                //这里做了一个判断，只有jpg,png,gif为后缀的，才给保存，否则抛出一个错误(写这个判断的原因是因为需求原因)
                if (Name.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
                    Name.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) ||
                    Name.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase))
                {
                    //以ContentDisposition的哈希值加上传的名字作为文件名
                    return $"{DateTime.Now.ToFileTimeUtc().ToString()}_{Name}";
                }
                throw new InvalidOperationException("上传格式错误");
            }
        }
    }
}
