using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices.ComTypes;
using System.Data;

namespace NetChina.Common
{
    public class ExportData
    {
        /**/
        /// <summary>
        /// 将Web控件或页面信息导出(带文件名参数)
        /// </summary>
        /// <param name="source">控件实例</param>        
        /// <param name="DocumentType">导出类型:Excel或Word</param>
        /// <param name="filename">保存文件名</param>
        public static void ExportWebData(string DocumentType, string filename, StringBuilder sbXml)
        {
            //设置Http的头信息,编码格式
            if (DocumentType == "Excel")
            {
                //Excel            
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename + ".xls", System.Text.Encoding.UTF8));
                HttpContext.Current.Response.ContentType = "application/ms-excel";
            }

            else if (DocumentType == "Word")
            {
                //Word
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename + ".doc", System.Text.Encoding.UTF8));
                HttpContext.Current.Response.ContentType = "application/ms-word";
            }

            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;


            //输出
            HttpContext.Current.Response.Write(sbXml.ToString());
            HttpContext.Current.Response.End();
        }

        public static string CreateWordFile(string title,DataTable dt)
        {
            string message = "";
            try
            {
                Object Nothing = System.Reflection.Missing.Value;
                //Directory.CreateDirectory("C:/CNSI");  //创建文件所在目录 
                string name = NetChina.Common.commonFun.uploadFile + "temp/已经完成任务列表.doc";
                object filename = HttpContext.Current.Server.MapPath(name);  //文件保存路径

                //创建Word文档
                Application WordApp = new ApplicationClass();
                Document WordDoc = WordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

                WordDoc.PageSetup.LeftMargin = 57;//设置左侧页边距
                WordDoc.PageSetup.RightMargin = 57;//设置右侧页边距

                WordApp.Selection.ParagraphFormat.LineSpacing = 15f;//设置文档的行间距               
                WordApp.Selection.TypeParagraph();//插入段落

                //设置文档大标题内容
                WordApp.Selection.Font.Size = 12;
                WordApp.Selection.Font.Bold = 2;
                WordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                WordApp.Selection.TypeText(title);

                WordApp.Selection.TypeParagraph();//插入段落
                WordApp.Selection.TypeParagraph();//插入段落


                int count = dt.Rows.Count;
                //文档中创建表格
                Table newTable = WordDoc.Tables.Add(WordApp.Selection.Range, count+1, 2, ref Nothing, ref Nothing);
                
                //设置表格每列的宽度              
                newTable.Columns[1].Width = 100f;
                newTable.Columns[2].Width = 400f;


                newTable.Cell(1, 1).Range.Text = "编号";
                newTable.Cell(1, 1).Range.Bold = 2;//设置单元格中字体为粗体 第七城市 www.th7.cn 
                newTable.Cell(1, 1).Range.Font.Size = 10;
                

                newTable.Cell(1, 2).Range.Text = "修改内容";
                newTable.Cell(1, 2).Range.Bold = 2;
                newTable.Cell(1, 2).Range.Font.Size = 10;                

                for (int i = 1; i <= count; i++)
                {
                    //填充表格内容
                    newTable.Cell(i + 1, 1).Range.Text = i.ToString();
                    newTable.Cell(i + 1, 1).Range.Bold = 0;//设置单元格中字体为粗体 第七城市 www.th7.cn 
                    newTable.Cell(i + 1, 1).Range.Font.Size = 10;
                   

                    newTable.Cell(i + 1, 2).Range.Text = dt.Rows[i-1]["MissionDesc"].ToString();
                    newTable.Cell(i + 1, 2).Range.Bold = 0;
                    newTable.Cell(i + 1, 2).Range.Font.Size = 10;
                    newTable.Cell(i + 1, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                }

                //单元格属性设置                
                WordApp.Selection.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;//垂直居中               
                WordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;//水平居中
                

                //在表格中增加行
                WordDoc.Content.Tables[1].Rows.Add(ref Nothing);  
         

                //设置底部信息
                WordDoc.Paragraphs.Last.Range.Text = "文档创建时间：" + DateTime.Now.ToString();//“落款”               
                WordDoc.Paragraphs.Last.Range.Font.Size = 10;
                
                WordDoc.Paragraphs.Last.Alignment = WdParagraphAlignment.wdAlignParagraphRight;


                //文件保存 
                WordDoc.ActiveWindow.Selection.WholeStory();
                WordDoc.ActiveWindow.Selection.Copy(); 

                WordDoc.SaveAs(ref filename, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                WordDoc.Close(ref Nothing, ref Nothing, ref Nothing);
                WordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
                //message = name + "文档生成成功，以保存到C:CNSI下";
            }
            catch
            {
                message = "文件导出异常！";
            }
            return message;
        }

        public static void DoadLoadFile(string FilePath)
        {
            System.IO.FileInfo file = new FileInfo(FilePath);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(file.Name));
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/ms-word";
            HttpContext.Current.Response.WriteFile(file.FullName);
            HttpContext.Current.Response.End();
        }
    }
}
