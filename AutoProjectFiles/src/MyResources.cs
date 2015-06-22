using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onway.AutoProjectFiles
{
    public class MyResources
    {
        public static string PackageTitle
        {
            get { return myRes.PackageTitle; }
        }

        public static string Error
        {
            get { return myRes.Error; }
        }

        public static string ExcludingTipFmt
        {
            get { return myRes.ExcludingTipFmt; }
        }

        public static string IncludingTipFmt
        {
            get { return myRes.IncludingTipFmt; }
        }

        public static string EntriesDoneFmt
        {
            get { return myRes.EntriesDoneFmt; }
        }

        public static string NoSnapshot
        {
            get { return myRes.NoSnapshot; }
        }

        static MyResources()
        {
            myRes = System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "zh-CN" ?
                new MyResources_zh_CN() : new MyResources_en_US();
        }

        private static MyResources_en_US myRes;
    }

    public class MyResources_en_US
    {
        public virtual string PackageTitle
        {
            get { return "Auto Project Files"; }
        }

        public virtual string Error
        {
            get { return "Error"; }
        }

        public virtual string ExcludingTipFmt
        {
            get { return "Excluding {0} entries from project"; }
        }

        public virtual string IncludingTipFmt
        {
            get { return "Including {0} entries to project"; }
        }

        public virtual string EntriesDoneFmt
        {
            get { return "{0} entries done"; }
        }

        public virtual string NoSnapshot
        {
            get { return "There is no snapshot for this project"; }
        }
    }

    public class MyResources_zh_CN : MyResources_en_US
    {
        public override string PackageTitle
        {
            get { return "项目文件自动更新"; }
        }

        public override string Error
        {
            get { return "错误"; }
        }

        public override string ExcludingTipFmt
        {
            get { return "正在从项目中排除{0}个条目"; }
        }

        public override string IncludingTipFmt
        {
            get { return "正在将{0}个条目包括到项目中"; }
        }

        public override string EntriesDoneFmt
        {
            get { return "完成{0}个条目"; }
        }

        public override string NoSnapshot
        {
            get { return "当前项目未建立快照"; }
        }
    }
}
