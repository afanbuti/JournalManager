using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace NetChina.Common
{
    
    public class commonFun
    {
        /// <summary>
        /// 上传文件路径
        /// </summary>
        public static string uploadFile = "/upload/";
        /// <summary>
        /// 任务文档路径
        /// </summary>
        public static string uploadMission="/upload/MissionDoc/";

        #region 提取摘要，是否清除HTML代码
        /// <summary>
        /// 提取摘要，是否清除HTML代码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <param name="StripHTML"></param>
        /// <returns></returns>
        /// 

        public static string GetContentSummary(string content, int length, bool StripHTML)
        {
            if (string.IsNullOrEmpty(content) || length == 0)
                return "";
            if (StripHTML)
            {
                Regex re = new Regex("<[^>]*>");
                content = re.Replace(content, "");
                content = content.Replace("　", "").Replace(" ", "");
                if (content.Length <= length)
                    return content;
                else
                    return content.Substring(0, length) + "...";
                //return content.Substring(0, length);
            }
            else
            {
                if (content.Length <= length)
                    return content;

                int pos = 0, npos = 0, size = 0;
                bool firststop = false, notr = false, noli = false;
                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    if (pos >= content.Length)
                        break;
                    string cur = content.Substring(pos, 1);
                    if (cur == "<")
                    {
                        string next = content.Substring(pos + 1, 3).ToLower();
                        if (next.IndexOf("p") == 0 && next.IndexOf("pre") != 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                        }
                        else if (next.IndexOf("/p") == 0 && next.IndexOf("/pr") != 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                                sb.Append("<br/>");
                        }
                        else if (next.IndexOf("br") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                                sb.Append("<br/>");
                        }
                        else if (next.IndexOf("img") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                                size += npos - pos + 1;
                            }
                        }
                        else if (next.IndexOf("li") == 0 || next.IndexOf("/li") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                            }
                            else
                            {
                                if (!noli && next.IndexOf("/li") == 0)
                                {
                                    sb.Append(content.Substring(pos, npos - pos));
                                    noli = true;
                                }
                            }
                        }
                        else if (next.IndexOf("tr") == 0 || next.IndexOf("/tr") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                            }
                            else
                            {
                                if (!notr && next.IndexOf("/tr") == 0)
                                {
                                    sb.Append(content.Substring(pos, npos - pos));
                                    notr = true;
                                }
                            }
                        }
                        else if (next.IndexOf("td") == 0 || next.IndexOf("/td") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                            }
                            else
                            {
                                if (!notr)
                                {
                                    sb.Append(content.Substring(pos, npos - pos));
                                }
                            }
                        }
                        else
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            sb.Append(content.Substring(pos, npos - pos));
                        }
                        if (npos <= pos)
                            npos = pos + 1;
                        pos = npos;
                    }
                    else
                    {
                        if (size < length)
                        {
                            sb.Append(cur);
                            size++;
                        }
                        else
                        {
                            if (!firststop)
                            {
                                sb.Append("...");
                                firststop = true;
                            }
                        }
                        pos++;
                    }

                }
                return sb.ToString();
            }
        }
        #endregion

        /// <summary>
        /// 替换换行和回车
        /// </summary>
        /// <param name="tempStr"></param>
        /// <returns></returns>
        public static string replaceStr(string tempStr)
        {
            tempStr = tempStr.Replace("\r\n", "<br/>");
            tempStr = tempStr.Replace("\r", "<br/>");
            tempStr = tempStr.Replace("\n", "<br/>");
            return tempStr;
        }

        /// <summary>
        /// 获取星期几
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static string  getDate(string dateStr)
        {
            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(dateStr);
            }
            catch
            {
                dt = System.DateTime.Now;
            }
            string Temp = "";
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    Temp = "星期天";
                    break;
                case DayOfWeek.Monday:
                    Temp = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    Temp = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    Temp = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    Temp = "星期四";
                    break;
                case DayOfWeek.Friday:
                    Temp = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    Temp = "星期六";
                    break;
            }
            return Temp;
        }


        public static string EnReplaceStr(string tempStr)
        {
            tempStr = tempStr.Replace("<br/>", "\r\n");
            return tempStr;
        }
    }
}
