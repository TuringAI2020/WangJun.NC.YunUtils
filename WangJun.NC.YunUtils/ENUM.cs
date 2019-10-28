namespace WangJun.NC.YunUtils
{
    public static class ENUM
    {

        /// <summary>
        /// 服务类型
        /// </summary>
        public enum ServiceType
        {
            云收藏 = 0x0101,
            云商品 = 0x0201,
            云头条 = 0x0301,
            云应用 = 0x0401,
            云菜单 = 0x0501,
            云账号 = 0x0601,
            云订单 = 0x0701,
            云权限 = 0x0801,
            云评论 = 0x0901
        }

        public enum TaskStatus
        {
            待处理 = 0,


            正常处理中 = 2,

            超时处理中 = 3,

            首次处理出错 = -1,

            多次处理出错 = -2,
            处理失败 = -10000,

            处理完毕 = 1
        }

        /// <summary>
        /// 实体状态
        /// </summary>
        public enum 实体状态
        {
            正常 = 0x0001,
            草稿 = 0x0002,
            隐藏 =0x0003,
            已删除 = -0x0001,
            不可用 =-0x0004,
            已完成 = -0x0005,
            未初始化=0x0
            
        }

        public enum 排序 {
            按创建时间正序=0x0001,
            按创建时间倒序 = -0x0001,
        }

        /// <summary>
        /// 实体状态
        /// </summary>
        public enum CommodityStatus
        {
            上架 = 0x0010,
            待售 = 0x0020,
            下架 = -0x0010
        }


        public enum 队列分组名称
        {
            YunForm待处理 = 0x0001,
            YunFav待处理 = 0x0002,
        }


        /// <summary>
        /// 
        /// </summary>
        public enum 订单及消费项状态
        {
            异常 = -0x10000,
            未支付 = 0x10000,
            未使用 = 0x10001,
            部分使用 = 0x10002,
            无可用项目 = 0x10003,///全部被用,部分被用,部分退款
            全部已退款 = 0x10004,
            已取消 = 0x10005,
            退款处理中 = 0x10006,
            已退款 = 0x10007,
        }

        public enum 客户行为
        {
            打开文档 = 0x101,
            表浅阅读 = 0x102,
            深度阅读 = 0x103,
            收藏 = 0x104,
            取消收藏 = -0x104,
            评论 = 0x105,
            删除评论 = -0x105,
            评论他人 = 0x106,
            删除评论他人 = -0x106,
            点赞=0x107,
            取消点赞=-0x107
        }

        public enum 数据类型
        {
            PlainText=0x001,
            Json = 0x002,
            Html = 0x003,
            ByteArray=0x004
        }


    }
}
