using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityPackage.Editor
{
    public class PackageManifest : ScriptableObject
    {
        #region PRIVATE FIELDS
        [SerializeField] private string packageAbsolutePath;
        [SerializeField] private string displayName;
        [SerializeField] private bool isUseDisplayNameAsRootFolderName;
        [SerializeField] private string rootFolderName;
        [SerializeField] private bool hasReadme;
        [SerializeField] private bool hasChangelog;
        [SerializeField] private bool hasLicense;
        [SerializeField] private bool hasThirdPartyNotices;
        [SerializeField] private bool hasEditorFolder;
        [SerializeField] private bool hasRuntimeFolder;
        [SerializeField] private bool hasTestsFolder;
        [SerializeField] private bool hasTestsEditorFolder;
        [SerializeField] private bool hasTestsRuntimeFolder;
        [SerializeField] private bool hasDocumentationFolder;
        [SerializeField] private bool hasSamplesFolder;
        [SerializeField] private bool hasScreenshotsFolder;
        [SerializeField] private string nameExtension;
        [SerializeField] private string nameCompany;
        [SerializeField] private string namePackage;
        [SerializeField] private int versionMajor;
        [SerializeField] private int versionMinor;
        [SerializeField] private int versionPatch;
        [SerializeField] private int unityVersionMajor;
        [SerializeField] private int unityVersionMinor;
        [SerializeField] private string description;
        [SerializeField] private bool hasAuthorName;
        [SerializeField] private string authorName;
        [SerializeField] private bool hasAuthorEmail;
        [SerializeField] private string authorEmail;
        [SerializeField] private bool hasAuthorUrl;
        [SerializeField] private string authorUrl;
        [SerializeField] private bool hasUnityRelease;
        [SerializeField] private string unityRelease;
        [SerializeField] private bool hasDependencies;
        [SerializeField] private List<PackageDependency> dependencies;
        [SerializeField] private bool hasKeywords;
        [SerializeField] private List<PackageKeyword> keywords;
        [SerializeField] private string readme;
        [SerializeField] private string changelog;
        [SerializeField] private string license;
        [SerializeField] private string thirdPartyNotices;
        #endregion
        
        #region PUBLIC PROPERTIES
        public string PackageAbsolutePath
        {
            get => packageAbsolutePath;
            set => packageAbsolutePath = value;
        }

        public string DisplayName 
        {
            get => displayName;
            set => displayName = value;
        }
        
        public bool IsUseDisplayNameAsRootFolderName
        {
            get => isUseDisplayNameAsRootFolderName;
            set => isUseDisplayNameAsRootFolderName = value;
        }
        
        public string RootFolderName  
        {
            get => rootFolderName;
            set => rootFolderName = value;
        }

        public bool HasReadme 
        {
            get => hasReadme;
            set => hasReadme = value;
        }
        
        public bool HasChangelog
        {
            get => hasChangelog;
            set => hasChangelog = value;
        }
        
        public bool HasLicense
        {
            get => hasLicense;
            set => hasLicense = value;
        }

        public bool HasThirdPartyNotices
        {
            get => hasThirdPartyNotices;
            set => hasThirdPartyNotices = value;
        }

        public bool HasEditorFolder
        {
            get => hasEditorFolder;
            set => hasEditorFolder = value;
        }
        
        public bool HasRuntimeFolder
        {
            get => hasRuntimeFolder;
            set => hasRuntimeFolder = value;
        }
        
        public bool HasTestsFolder
        {
            get => hasTestsFolder;
            set => hasTestsFolder = value;
        }
        
        public bool HasTestsEditorFolder
        {
            get => hasTestsEditorFolder;
            set => hasTestsEditorFolder = value;
        }
        
        public bool HasTestsRuntimeFolder
        {
            get => hasTestsRuntimeFolder;
            set => hasTestsRuntimeFolder = value;
        }
        
        public bool HasDocumentationFolder
        {
            get => hasDocumentationFolder;
            set => hasDocumentationFolder = value;
        }
        
        public bool HasSamplesFolder
        {
            get => hasSamplesFolder;
            set => hasSamplesFolder = value;
        }
        
        public bool HasScreenshotsFolder
        {
            get => hasScreenshotsFolder;
            set => hasScreenshotsFolder = value;
        }
        
        public string NameExtension
        {
            get => nameExtension;
            set => nameExtension = value;
        }
        
        public string NameCompany
        {
            get => nameCompany;
            set => nameCompany = value;
        }
        
        public string NamePackage
        {
            get => namePackage;
            set => namePackage = value;
        }
        
        public int VersionMajor
        {
            get => versionMajor;
            set => versionMajor = value;
        }
        
        public int VersionMinor
        {
            get => versionMinor;
            set => versionMinor = value;
        }
        
        public int VersionPatch
        {
            get => versionPatch;
            set => versionPatch = value;
        }
        
        public int UnityVersionMajor
        {
            get => unityVersionMajor;
            set => unityVersionMajor = value;
        }
        
        public int UnityVersionMinor
        {
            get => unityVersionMinor;
            set => unityVersionMinor = value;
        }
        
        public string Description
        {
            get => description;
            set => description = value;
        }
        
        public bool HasAuthorName
        {
            get => hasAuthorName;
            set => hasAuthorName = value;
        }
        
        public string AuthorName
        {
            get => authorName;
            set => authorName = value;
        }
        
        public bool HasAuthorEmail
        {
            get => hasAuthorEmail;
            set => hasAuthorEmail = value;
        }
        
        public string AuthorEmail
        {
            get => authorEmail;
            set => authorEmail = value;
        }
        
        public bool HasAuthorUrl
        {
            get => hasAuthorUrl;
            set => hasAuthorUrl = value;
        }
        
        public string AuthorUrl
        {
            get => authorUrl;
            set => authorUrl = value;
        }
        
        public bool HasUnityRelease
        {
            get => hasUnityRelease;
            set => hasUnityRelease = value;
        }
        
        public string UnityRelease
        {
            get => unityRelease;
            set => unityRelease = value;
        }
        
        public bool HasDependencies
        {
            get => hasDependencies;
            set => hasDependencies = value;
        }
        
        public List<PackageDependency> Dependencies
        {
            get => dependencies;
            set => dependencies = value;
        }
        
        public bool HasKeywords
        {
            get => hasKeywords;
            set => hasKeywords = value;
        }
        
        public List<PackageKeyword> Keywords
        {
            get => keywords;
            set => keywords = value;
        }
        
        public string Readme
        {
            get => readme;
            set => readme = value;
        }
        
        public string Changelog
        {
            get => changelog;
            set => changelog = value;
        }
        
        public string License
        {
            get => license;
            set => license = value;
        }
        
        public string ThirdPartyNotices
        {
            get => thirdPartyNotices;
            set => thirdPartyNotices = value;
        }
        #endregion

        // 默认值
        //文件夹设定
        private const string DisplayNameDefault = "Yun_";
        private const bool IsUseDisplayNameAsRootFolderNameDefault = false;
        private const string RootFolderNameDefault = "";
        private const bool HasReadmeDefault = false;
        private const bool HasChangelogDefault = false;
        private const bool HasLicenseDefault = false;
        private const bool HasThirdPartyNoticesDefault = false;
        private const bool HasEditorFolderDefault = false;
        private const bool HasRuntimeFolderDefault = false;
        private const bool HasTestsFolderDefault = false;
        private const bool HasTestsEditorFolderDefault = false;
        private const bool HasTestsRuntimeFolderDefault = false;
        private const bool HasDocumentationFolderDefault = false;
        private const bool HasSamplesFolderDefault = false;
        private const bool HasScreenshotsDefault = false;

        //包信息
        private const string NameExtensionDefault = "et";
        private const string NameCompanyDefault = "yun";
        private const string NameDefault = "";
        private const int VersionMajorDefault = 0;
        private const int VersionMinorDefault = 1;
        private const int VersionPathDefault = 0;
        
        private const int UnityVersionMajorDefault = 2022;
        private const int UnityVersionMinorDefault = 3;
        private const string DescriptionDefault = "";

        private const bool HasAuthorNameDefault = true;
        private const string AuthorNameDefault = "YUNLO";
        private const bool HasAuthorEmailDefault = false;
        private const string AuthorEmailDefault = "";
        private const bool HasAuthorUrlDefault = false;
        private const string AuthorUrlDefault = "";
        private const bool HasUnityReleaseDefault = true;
        private const string UnityReleaseDefault = "42f1c1";

        private const bool HasDependenciesDefault = false;
        private static readonly List<PackageDependency> DependenciesDefault = new List<PackageDependency>();
        private const bool HasKeywordsDefault = false;
        private static readonly List<PackageKeyword> KeywordsDefault = new List<PackageKeyword>();

        private const string ReadmeDefault = "";
        private const string ChangelogDefault = "";
        private const string LicenseDefault = "";
        private const string ThirdPartyNoticesDefault = "";

        // Validation Defaults
        private const int MinimumUnityVersionMajor = 2017;
        private const int MinimumUnityVersionMinor = 1;
        private const int MaximumUnityVersionMinor = 4;
        private const int MinUnityReleaseCharCount = 3;
        private const int MaxUnityReleaseCharCount = 5;

        public PackageManifest()
        {
            ResetToDefault();
        }

        public void ResetToDefault()
        {
            DisplayName = DisplayNameDefault;
            IsUseDisplayNameAsRootFolderName = IsUseDisplayNameAsRootFolderNameDefault;
            RootFolderName = RootFolderNameDefault;
            HasReadme = HasReadmeDefault;
            HasChangelog = HasChangelogDefault;
            HasLicense = HasLicenseDefault;
            HasThirdPartyNotices = HasThirdPartyNoticesDefault;
            HasEditorFolder = HasEditorFolderDefault;
            HasRuntimeFolder = HasRuntimeFolderDefault;
            HasTestsFolder = HasTestsFolderDefault;
            HasTestsEditorFolder = HasTestsEditorFolderDefault;
            HasTestsRuntimeFolder = HasTestsRuntimeFolderDefault;
            HasDocumentationFolder = HasDocumentationFolderDefault;
            HasSamplesFolder = HasSamplesFolderDefault;
            HasScreenshotsFolder = HasScreenshotsDefault;
            
            NameExtension = NameExtensionDefault;
            NameCompany = NameCompanyDefault;
            NamePackage = NameDefault;

            VersionMajor = VersionMajorDefault;
            VersionMinor = VersionMinorDefault;
            VersionPatch = VersionPathDefault;
            
            UnityVersionMajor = UnityVersionMajorDefault;
            UnityVersionMinor = UnityVersionMinorDefault;
            Description = DescriptionDefault;

            HasAuthorName = HasAuthorNameDefault;
            AuthorName = AuthorNameDefault;
            HasAuthorEmail = HasAuthorEmailDefault;
            AuthorEmail = AuthorEmailDefault;
            HasAuthorUrl = HasAuthorUrlDefault;
            AuthorUrl = AuthorUrlDefault;

            HasUnityRelease = HasUnityReleaseDefault;
            UnityRelease = UnityReleaseDefault;
            HasDependencies = HasDependenciesDefault;
            Dependencies = DependenciesDefault;
            HasKeywords = HasKeywordsDefault;
            Keywords = KeywordsDefault;

            HasReadme = HasReadmeDefault;
            Readme = ReadmeDefault;
            HasChangelog = HasChangelogDefault;
            Changelog = ChangelogDefault;
            License = LicenseDefault;
            ThirdPartyNotices = ThirdPartyNoticesDefault;
        }

     

        public bool IsValidPackageManifest()
        {
            var isValidPackageManifest = true;
            
            // Name Extension
            if (string.IsNullOrWhiteSpace(NameExtension))
            {
                Debug.LogError(LoggingConstants.NameExtensionErrorEmpty);
                isValidPackageManifest = false;
            }
            else
            {
                var containsNumbers = NameExtension.Any(char.IsDigit);
                if (containsNumbers)
                {
                    Debug.LogError(LoggingConstants.NameExtensionErrorContainsNumber);
                    isValidPackageManifest = false;
                }
            }

            // Name Company
            if (string.IsNullOrWhiteSpace(NameCompany))
            {
                Debug.LogError(LoggingConstants.NameCompanyErrorEmpty);
                isValidPackageManifest = false;
            }
            
            // Name 
            if (string.IsNullOrWhiteSpace(NamePackage))
            {
                Debug.LogError(LoggingConstants.NamePackageErrorEmpty);
                isValidPackageManifest = false;
            }
            
            // Version
            // Version is always valid because it resets to 0
            
            // Display Name
            if (string.IsNullOrWhiteSpace(DisplayName))
            {
                Debug.LogError(LoggingConstants.DisplayNameErrorEmpty);
                isValidPackageManifest = false;
            }
            
            // Root Folder Name
            if (string.IsNullOrWhiteSpace(RootFolderName))
            {
                Debug.LogError(LoggingConstants.RootFolderNameErrorEmpty);
                isValidPackageManifest = false;
            }
            
            // Unity Version Major
            if (UnityVersionMajor < MinimumUnityVersionMajor)
            {
                Debug.LogError(LoggingConstants.UnityVersionMajorErrorMinimum + MinimumUnityVersionMajor);
                isValidPackageManifest = false;
            }
            
            // Unity Version Minor
            if (UnityVersionMinor < MinimumUnityVersionMinor)
            {
                Debug.LogError(LoggingConstants.UnityVersionMinorErrorMinimum + MinimumUnityVersionMinor);
                isValidPackageManifest = false;
            }

            if (UnityVersionMinor > MaximumUnityVersionMinor)
            {
                Debug.LogError(LoggingConstants.UnityVersionMinorErrorMaximum + MaximumUnityVersionMinor);
                isValidPackageManifest = false;
            }
            
            // Description
            if (String.IsNullOrWhiteSpace(Description))
            {
                Debug.LogError(LoggingConstants.DescriptionErrorEmpty);
                isValidPackageManifest = false;
            }
            
            // Author Name
            if (HasAuthorName && string.IsNullOrWhiteSpace(AuthorName))
            {
                Debug.LogError(LoggingConstants.AuthorNameErrorEmpty);
                isValidPackageManifest = false;
            }
            
            // Author Email
            if (HasAuthorEmail && string.IsNullOrWhiteSpace(AuthorEmail))
            {
                Debug.LogError(LoggingConstants.AuthorEmailErrorEmpty);
                isValidPackageManifest = false;
            }
            else
            {
                if (HasAuthorEmail)
                {
                    if (!AuthorEmail.Contains(PackageManifestConstants.EmailAtSymbol))
                    {
                        Debug.LogError(LoggingConstants.AuthorEmailErrorSymbol);
                        isValidPackageManifest = false;
                    }
                }
            }
            
            // Author Url
            if (HasAuthorUrl && string.IsNullOrWhiteSpace(AuthorUrl))
            {
                Debug.LogError(LoggingConstants.AuthorUrlErrorEmpty);
                isValidPackageManifest = false;
            }
            
            // Unity Release
            if (HasUnityRelease && string.IsNullOrWhiteSpace(UnityRelease))
            {
                Debug.LogError(LoggingConstants.UnityReleaseErrorEmpty);
                isValidPackageManifest = false;
            }
            else
            {
                if (HasUnityRelease)
                {
                    var unityReleaseCharCount = UnityRelease.Length;

                    if (unityReleaseCharCount < MinUnityReleaseCharCount)
                    {
                        Debug.LogError(LoggingConstants.UnityReleaseErrorTooFew + MinUnityReleaseCharCount);   
                        isValidPackageManifest = false;
                    }
                
                    if (unityReleaseCharCount > MaxUnityReleaseCharCount)
                    {
                        Debug.LogError(LoggingConstants.UnityReleaseErrorTooMany + MaxUnityReleaseCharCount);
                        isValidPackageManifest = false;
                    }

                    if (UnityRelease.All(char.IsDigit))
                    {
                        Debug.LogError("Unity Release must contains letters, not just numbers - see example.");
                        isValidPackageManifest = false;
                    }

                    if (!UnityRelease.Any(char.IsDigit))
                    {
                        Debug.LogError("Unity Release must contain numbers, not just letters - see example.");
                        isValidPackageManifest = false;
                    }
                }
            }
            
            // Dependencies
            if (HasDependencies && Dependencies.Count == 0)
            {
                Debug.LogError("Dependencies are enabled, but none are active. Disable them " +
                                  "or add new fields");
                isValidPackageManifest = false;
            }
            
            // Keywords
            if (HasKeywords && Keywords.Count == 0)
            {
                Debug.LogError("Keywords are enabled, but none are active. Disable them or add " +
                                  "new fields");
                isValidPackageManifest = false;
            }
            
            // Readme
            if (HasReadme && string.IsNullOrWhiteSpace(Readme))
            {
                Debug.LogError("Readme cannot be empty if enabled.");
                isValidPackageManifest = false;
            }
            
            // Changelog
            if (HasChangelog && string.IsNullOrWhiteSpace(Changelog))
            {
                Debug.LogError("Changelog cannot be empty if enabled.");
                isValidPackageManifest = false;
            }
            
            // License
            if (HasLicense && string.IsNullOrWhiteSpace(License))
            {
                Debug.LogError(LoggingConstants.LicenceErrorEmpty);
                isValidPackageManifest = false;
            }
            
            // Third Party Notices
            if (HasThirdPartyNotices && string.IsNullOrWhiteSpace(ThirdPartyNotices))
            {
                Debug.LogError(LoggingConstants.ThirdPartyNoticesErrorEmpty);
                isValidPackageManifest = false;
            }

            return isValidPackageManifest;
        }
    }
}