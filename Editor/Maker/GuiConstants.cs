using UnityEngine;

namespace UnityPackage.Editor
{
    public static class GuiConstants
    {
        public const string PackageMakerName = "Package Maker";
        public const string PackageMakerMenuItemPath = "Tools/Yun/资源包管理";
        
        
        public const string MainPanelUxmlPath = "CS_PackageMakerCore";
        public const string IncludedPackageContentsUxmlPath = "CS_IncludedPackageContents";
        public const string PackageManifestUxmlPath = "CS_PackageManifest";
        public const string DependencyEntryUxmlPath = "CS_DependenciesEntry";
        public const string KeywordEntryUxmlPath = "CS_KeywordsEntry";
        public const string ReadmeUxmlPath = "CS_Readme";
        public const string ChangelogUxmlPath = "CS_Changelog";
        public const string LicenseUxmlPath = "CS_License";
        public const string ThirdPartyNoticesUxmlPath = "CS_ThirdPartyNotices";
        
        
        public const string LeftPanelName = "VE_PmLeftPanel";
        public const string CenterPanelName = "VE_PmCenterPanel";
        public const string RightPanelName = "VE_PmRightPanel";
        
        public const string LoadPackageButtonName = "BT_LoadPackage";
        public const string DisplayNameTextFieldName = "TF_PackageDisplayName";
        public const string UseDisplayNameAsRootFolderNameToggleName = "TG_UseDisplayNameAsRootFolderName";
        public const string RootFolderNameContentsVisualElementName =  "VE_RootFolderNameContents";
        public const string RootFolderNameTextFieldName = "TF_RootFolderNameContents";
        public const string ReadmeToggleName = "TG_Readme";
        public const string ReadmeTextFieldName = "TF_ReadmeContents";
        public const string ChangelogToggleName = "TG_Changelog";
        public const string ChangelogTextFieldName = "TF_ChangelogContents";
        public const string LicenceToggleName = "TG_License";
        public const string ThirdPartyNoticesToggleName = "TG_ThirdPartyNotices";
        public const string LicenseTextFieldName = "TF_LicenseContents";
        public const string ThirdPartyNoticesTextFieldName = "TF_ThirdPartyNotices";
        public const string EditorFolderToggleName = "TG_Editor";
        public const string RuntimeFolderToggleName = "TG_Runtime";
        public const string TestsFolderToggleName = "TG_Tests";
        public const string TestsEditorFolderToggleName = "TG_TestEditor";
        public const string TestsRuntimeFolderToggleName = "TG_TestRuntime";
        public const string DocumentationFolderToggleName = "TG_Documentation";
        public const string SamplesFolderToggleName = "TG_Samples";
        public const string ScreenshotsFolderToggleName = "TG_Screenshots";
        
        public const string PackageNameExtensionTextFieldName = "TF_PackageNameExtension";
        public const string PackageNameCompanyTextFieldName = "TF_PackageNameCompany";
        public const string PackageNameTextFieldName = "TF_PackageName";

        public const string PackageVersionMajorIntegerFieldName = "IF_PackageVersionMajor";
        public const string PackageVersionMinorIntegerFieldName = "IF_PackageVersionMinor";
        public const string PackageVersionPatchIntegerFieldName = "IF_PackageVersionPatch";

        public const string PackageUnityVersionMajorIntegerFieldName = "IF_PackageUnityVersionMajor";
        public const string PackageUnityVersionMinorIntegerFieldName = "IF_PackageUnityVersionMinor";

        public const string PackageDescriptionTextFieldName = "TF_PackageDescription";
        
        
        public const string AuthorNameTextFieldName = "TF_AuthorName";
        public const string AuthorEmailTextFieldName = "TF_AuthorEmail";
        public const string AuthorUrlTextFieldName = "TF_AuthorUrl";

        public const string UnityReleaseToggleName = "TG_UnityRelease";
        public const string UnityReleaseTextFieldName = "TF_UnityRelease";

        public const string DependenciesToggleName = "TG_Dependencies";
        public const string DependenciesContentsVisualElementName = "VE_DependenciesContents";
        public const string DependenciesListViewName = "LV_Dependencies";
        public const string AddDependencyButtonName = "BT_DependenciesAdd";
        public const string DependencyEntryNameTextFieldName = "TF_DependenciesEntryName";
        public const string DependencyEntryVersionTextFieldName = "TF_DependenciesEntryVersion";
        public const string DependencyEntryRemoveButtonName = "BT_DependenciesEntryRemove";

        public const string KeywordsToggleName = "TG_Keywords";
        public const string KeywordsContentsVisualElementName = "VE_KeywordsContents";
        public const string KeywordsListViewName = "LV_Keywords";
        public const string AddKeywordButtonName = "BT_KeywordsAdd";
        public const string KeywordEntryNameTextFieldName = "TF_KeywordsEntryName";
        public const string RemoveKeywordButtonName = "BT_KeywordsEntryRemove";

        public const string ClearAllButtonName = "BT_ClearAll";
        public const string UpdatePackageButtonName = "BT_UpdatePackage";
        public const string CreatePackageButtonName = "BT_CreatePackage";

        public const string LoadPackageWindowTitle = "Browse to the parent folder of the package you would like " +
                                                     "to load in the tool.";
        
        public const string CreatePackagesWindowTitle = "Browse to the parent folder where you would like to create " +
                                                        "the package folder.";

        public const string NoPackageManifestTitle = "No Package found";
        public const string NoPackageManifestMessage = "The selected folder does not contain a package manifest " +
                                                       "(package.json) and is hence invalid!";
        public const string NoPackageManifestOk = "OK";
        
        public const string InvalidPackageErrorTitle = "Invalid Package";
        public const string InvalidPackageErrorMessage = "One or more felds in the Package are invalid, please check " +
                                                         "the console debug log and fix them.";
        public const string InvalidPackageOk = "OK";
        public const char Period = '.';

        public const string OverridePackageTitle = "Override Package?";
        public const string OverridePackageMessage =
            "The package already exists at the path, do you want to override it?";
        public const string OverrideYes = "Yes";
        public const string OverrideCancel = "Cancel";

        public const string SuccessCreatePackageTitle = "Success!";
        public const string SuccessCreatePackageMessage = "Package has been successfully created.";
        public const string SuccessCreatePackageOk = "OK";
        
    }
}