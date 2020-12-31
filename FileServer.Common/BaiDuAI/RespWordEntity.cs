using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.Common.BaiDuAI
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-29 17:19:20 
    /// </summary>
    public class RespWordEntity<T> where T : class
    {
        public string Log_id { get; set; }
        public int Words_result_num { get; set; }
        public List<T> Words_result { get; set; }

        public string Where()
        {
            throw new NotImplementedException();
        }
    }

    public class WordEntity
    {
        public string Words { get; set; }
    }
}
