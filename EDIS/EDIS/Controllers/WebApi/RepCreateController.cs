using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EDIS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDIS.Controllers.WebApi
{
    [Route("WebApi/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RepCreateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RepCreateController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class Root
        {
            [Required]
            public string UsrID { get; set; }
            /// <summary>
            /// Hash MD5 加密
            /// </summary>
            [Required]
            public string Passwd { get; set; }  //Hash MD5 加密
            /// <summary>
            /// 結果代碼  0: 成功
            /// </summary>
            public string Code { get; set; }    //結果代碼  0: 成功
            public string Msg { get; set; }     //結果訊息
            [Required]
            public string SerNo { get; set; }   //事件處理編號
            public string Mno { get; set; }     //維修單號
            [Required]
            public string Building { get; set; }//大樓
            [Required]
            public string Floor { get; set; }   //樓層
            [Required]
            public string Point { get; set; }   //故障點編號
            [Required]
            public string Name { get; set; }    //故障點名稱
            [Required]
            public string Area { get; set; }    //區域
            [Required]
            public string Des { get; set; }     //處理描述
        }

        //// GET: WebApi/RepCreate
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: WebApi/RepCreate/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: WebApi/RepCreate
        /// <summary>
        /// 工務請修系統提供之WebApi，驗證並新增工務請修單，回傳單號。
        /// </summary>
        /// <param name="root">客戶指定傳入之XML格式參數</param>
        [HttpPost]
        public IActionResult Post([FromBody] Root root)
        {
            var userName = root.UsrID;
            var user = _context.AppUsers.Where(u => u.UserName == userName).FirstOrDefault();

            if (user != null)   //Check UserName
            {
                var userMD5HashPW = MD5Hash(user.Password); //Get hashed result to compare.

                if (root.Passwd == userMD5HashPW)   //CheckPassWord
                {
                    Root testPast2 = new Root { Code = root.UsrID, Msg = root.Name, SerNo = root.SerNo, Mno = root.Passwd };
                    return Ok(testPast2);
                }
                else
                {
                    return BadRequest(userMD5HashPW);
                }
            }
            Root testPast = new Root { Code = root.UsrID, Msg = root.Name, SerNo = root.SerNo, Mno = root.Passwd };
            return BadRequest(testPast);
        }

        //// PUT: WebApi/RepCreate/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: WebApi/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                return Encoding.ASCII.GetString(result);
            }
        }
    }
}
