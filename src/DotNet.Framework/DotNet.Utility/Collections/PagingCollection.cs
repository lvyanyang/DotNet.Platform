// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Collections
{
    /// <summary>
    /// 分页集合
    /// </summary>
    public class PagingCollection
    {
        private int _pageIndex;
        private int _pageSize;
        private int _recordEndIndex;
        private int _recordStartIndex;
        private int _totalCount;
        private int _totalPages;

        /// <summary>
        /// 构造分页集合
        /// </summary>
        /// <param name="pageIndex">页码索引,从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="totalCount">总记录数</param>
        public PagingCollection(int pageIndex, int pageSize, int totalCount)
        {
            _pageIndex = pageIndex;
            _pageSize = pageSize;
            _totalCount = totalCount;
            _totalPages = (totalCount + pageSize - 1) / pageSize;
        }

        /// <summary>
        /// 页码索引
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
        }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get { return _totalPages; }
        }

        /// <summary>
        /// 当前记录开始数
        /// </summary>
        public int RecordStartIndex
        {
            get
            {
                if (_totalCount > 0)
                {
                    _recordStartIndex = (_pageIndex - 1) * PageSize + 1;
                }
                return _recordStartIndex;
            }
        }

        /// <summary>
        /// 当前记录结束数
        /// </summary>
        public int RecordEndIndex
        {
            get
            {
                _recordEndIndex = _recordStartIndex + _pageSize - 1;
                if (_recordEndIndex > _totalCount)
                {
                    _recordEndIndex = _totalCount;
                }
                return _recordEndIndex;
            }
        }

        /// <summary>
        /// 是否首页
        /// </summary>
        public bool IsFirst
        {
            get { return (_pageIndex == 0); }
        }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get { return (_pageIndex > 0); }
        }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get { return (_pageIndex + 1 < _totalPages); }
        }

        /// <summary>
        /// 是否最后一页
        /// </summary>
        public bool IsLast
        {
            get { return (_pageIndex == _totalPages - 1); }
        }
    }
}
