using System;
using System.Collections.Generic;
using System.Text;

namespace NetChina.JournalModel
{
    public class FileManager
    {
        /// <summary>
        /// 文件编号
        /// </summary>
        private int _fileId;
        /// <summary>
        /// 用户编号
        /// </summary>
        private int _userId;
        /// <summary>
        /// 文件标题
        /// </summary>
        private string _fileTitle;
        /// <summary>
        /// 文件描述
        /// </summary>
        private string _fileDesc;
        /// <summary>
        /// 文件类型
        /// </summary>
        private string _fileType;
        /// <summary>
        /// 文件路径
        /// </summary>
        private string _filePath;
        /// <summary>
        /// 上传时间
        /// </summary>
        private DateTime _writeDate;

        /// <summary>
        /// 文件编号
        /// </summary>
        public int FileId
        {
            get { return _fileId; }
            set { _fileId = value; }
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        /// <summary>
        /// 文件标题
        /// </summary>
        public string FileTitle
        {
            get { return _fileTitle; }
            set { _fileTitle = value; }
        }
        /// <summary>
        /// 文件描述
        /// </summary>
        public string FileDesc
        {
            get { return _fileDesc; }
            set { _fileDesc = value; }
        }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime WriteDate
        {
            get { return _writeDate; }
            set { _writeDate = value; }
        }

        public FileManager()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_fileIdX">文件编号</param>
        /// <param name="_userIdX">用户编号</param>
        /// <param name="_fileTitleX">文件标题</param>
        /// <param name="_fileDescX">文件描述</param>
        /// <param name="_fileTypeX">文件类型</param>
        /// <param name="_filePathX">文件路径</param>
        /// <param name="_writeDateX">上传日期</param>
        public FileManager(int _fileIdX, int _userIdX, string _fileTitleX, string _fileDescX, string _fileTypeX, string _filePathX, DateTime _writeDateX)
        {
            _fileId = _fileIdX;
            _userId = _userIdX;
            _fileTitle = _fileTitleX;
            _fileDesc = _fileDescX;
            _fileType = _fileTypeX;
            _filePath = _filePathX;
            _writeDate = _writeDateX;
        }
    }
}
