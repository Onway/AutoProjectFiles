using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onway.AutoProjectFiles
{
    public partial class MyResources
    {
        public static string StartSnapshotTitle
        {
            get { return myRes.StartSnapshotTitle; }
        }

        public static string SnapshotDoneFmt
        {
            get { return myRes.SnapshotDoneFmt; }
        }

        public static string LastUpdate
        {
            get { return myRes.LastUpdate; }
        }

        public static string NotSpecifyFolder
        {
            get { return myRes.NotSpecifyFolder; }
        }

        public static string NotExistFolder
        {
            get { return myRes.NotExistFolder; }
        }

        public static string NotSupportNotation
        {
            get { return myRes.NotSupportNotation; }
        }
    }

    public partial class MyResources_en_US
    {
        public virtual string StartSnapshotTitle
        {
            get { return "Creating Project Snapshot"; }
        }

        public virtual string SnapshotDoneFmt
        {
            get { return "Snapshot {0} entries done!"; }
        }

        public virtual string LastUpdate
        {
            get { return "Last Update: "; }
        }

        public virtual string NotSpecifyFolder
        {
            get { return "Snapshot folders not specified!"; }
        }

        public virtual string NotExistFolder
        {
            get { return "Snapshot folder not exist: "; }
        }

        public virtual string NotSupportNotation
        {
            get { return "Directory notation '.' and '..' are not supported!"; }
        }
    }

    public partial class MyResources_zh_CN : MyResources_en_US
    {
        public override string StartSnapshotTitle
        {
            get { return "正在创建项目快照"; }
        }

        public override string SnapshotDoneFmt
        {
            get { return "完成快照{0}个条目！"; }
        }

        public override string LastUpdate
        {
            get { return "最近更新："; }
        }

        public override string NotSpecifyFolder
        {
            get { return "未指定快照目录！"; }
        }

        public override string NotExistFolder
        {
            get { return "快照目录不存在："; }
        }

        public override string NotSupportNotation
        {
            get { return "不支持目录符号 '.' 和 '..'！"; }
        }
    }
}
