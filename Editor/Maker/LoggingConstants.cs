namespace UnityPackage.Editor
{
    public static class LoggingConstants
    {
        public const string NameExtensionErrorEmpty = "名称扩展不能为空。";
        public const string NameExtensionErrorContainsNumber = "名称扩展不能包含数字。";

        public const string NameCompanyErrorEmpty = "公司名称不能为空。";

        public const string NamePackageErrorEmpty = "包名称不能为空。";

        public const string DisplayNameErrorEmpty = "显示名称不能为空。";

        public const string RootFolderNameErrorEmpty = "根文件夹名称不能为空。";

        public const string UnityVersionMajorErrorMinimum = "Unity的主版本不能早于 ";

        public const string UnityVersionMinorErrorMinimum = "Unity的小版本不能小于：";
        public const string UnityVersionMinorErrorMaximum = "Unity的小版本不能大于：";

        public const string DescriptionErrorEmpty = "描述不能为空。";

        public const string AuthorNameErrorEmpty = "如果启用，作者名称不能为空。";
        public const string AuthorEmailErrorEmpty = "如果启用，作者电子邮件不能为空。";
        public const string AuthorEmailErrorSymbol = "作者电子邮件必须包含 @ 符号。";
        public const string AuthorUrlErrorEmpty = "如果启用，作者网址不能为空。";

        public const string UnityReleaseErrorEmpty = "如果启用，Unity发布版本不能为空。";
        public const string UnityReleaseErrorTooMany = "Unity发布版本的最大字符数为：";
        public const string UnityReleaseErrorTooFew = "Unity发布版本的最小字符数为：";
        public const string UnityReleaseErrorAllDigits = "Unity发布版本必须包含字母，不仅仅是数字 - 请参见示例。";
        public const string UnityReleaseErrorNoDigits = "Unity发布版本必须包含数字，不仅仅是字母 - 请参见示例。";
        public const string DependenciesErrorEmpty = "依赖项已启用，但没有任何一个处于激活状态。禁用它们或添加新字段。";
        public const string KeywordsErrorEmpty = "关键字已启用，但没有任何一个处于激活状态。禁用它们或添加新字段。";
        public const string ReadmeErrorEmpty = "如果启用，自述文件不能为空。";
        public const string ChangelogErrorEmpty = "如果启用，更新日志不能为空。";
        public const string LicenceErrorEmpty = "如果启用，许可证不能为空。";

        public const string ThirdPartyNoticesErrorEmpty = "如果启用，第三方通知不能为空。";
    }
}