using System;
using System.Collections.Generic;
using System.Text;

namespace NetChina.Common
{
    public class PageNavigator
    {
        /// <param name="total">总记录数</param>
        /// <param name="per">每页记录数</param>
        /// <param name="page">当前页数</param>
        /// <param name="query_string">Url参数</param>
        /// 返回一个带HTML代码的分页样式（字符串）
        public static string Pagination(int total, int per, int page, string query_string, string otherParams)
        {            
            int allpage = 0;
            int next = 0;//上一页
            int pre = 0;//下一页
            int startcount = 0;
            int endcount = 0;
            string pagestr = "";

            if (page < 1) { page = 1; }
            //计算总页数
            if (per != 0)
            {
                allpage = (total / per);
                allpage = ((total % per) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = page + 1;
            pre = page - 1;
            startcount = (page + 5) > allpage ? allpage - 9 : page - 4;//中间页起始序号
            //中间页终止序号
            endcount = page < 5 ? 10 : page + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; }//页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            pagestr = "共" + allpage + "页      当前第" + page + "页  共"+total+"条记录   ";

            pagestr += page > 1 ? "<a href=\"" + query_string + "?page=1"+otherParams+"\">首页</a>  <a href=\"" + query_string + "?page=" + pre + otherParams+"\">上一页</a>" : "首页 上一页";
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                pagestr += page == i ? "  <font color=\"#ff0000\">" + i + "</font>" : "  <a href=\"" + query_string + "?page=" + i +otherParams+ "\">" + i + "</a>";
            }
            pagestr += page != allpage ? "  <a href=\"" + query_string + "?page=" + next + otherParams + "\">下一页</a>  <a href=\"" + query_string + "?page=" + allpage + otherParams+"\">末页</a>" : " 下一页 末页";

            return pagestr;
        }
    }
}
