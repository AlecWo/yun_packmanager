﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityPackage.Editor.GuiConstants;

namespace UnityPackage.Editor
{
    public class PackageMakerWindow : EditorWindow
    {
        private static readonly Vector2 PackageMakerWindowSize = new Vector2(1400, 900);

        private VisualElement _root;
        private PackageManifest _packageManifest;

        private ListView _dependenciesListView;
        private VisualTreeAsset _dependencyEntryVisualTreeAsset;

        private ListView _keywordsListView;
        private VisualTreeAsset _keywordsEntryVisualTreeAsset;

        [MenuItem(PackageMakerMenuItemPath)]
        public static void ShowWindow()
        {
            var window = GetWindow<PackageMakerWindow>();
            window.titleContent = new GUIContent(PackageMakerName);
            window.minSize = PackageMakerWindowSize;
        }

        private void OnEnable()
        {
            _root = rootVisualElement;

            #region INITIALIZATION AND QUERY

            // Main Package
            var mainPackageVisualTree = Resources.Load<VisualTreeAsset>(MainPanelUxmlPath);
            mainPackageVisualTree.CloneTree(_root);
            var leftPanel = _root.Q<VisualElement>(LeftPanelName);
            var centerPanel = _root.Q<VisualElement>(CenterPanelName);
            var rightPanel = _root.Q<VisualElement>(RightPanelName);
            var contentsView = _root.Q<ScrollView>("CenterScrView");
            var rightScrollView = _root.Q<ScrollView>("RightScrView");

            var loadPackageButton = _root.Q<Button>(LoadPackageButtonName);

            // Included Package Contents
            var includedPackageContentsVisualTree = Resources.Load<VisualTreeAsset>(IncludedPackageContentsUxmlPath);
            includedPackageContentsVisualTree.CloneTree(leftPanel);

            // Package Manifest
            var packageManifestVisualTree = Resources.Load<VisualTreeAsset>(PackageManifestUxmlPath);
            var packageManifestVisualElement = new VisualElement();
            packageManifestVisualTree.CloneTree(packageManifestVisualElement);
            contentsView.Add(packageManifestVisualElement);

            // Display Name
            var displayNameTextField = leftPanel.Q<TextField>(DisplayNameTextFieldName);
            var useDisplayNameAsRootFolderNameToggle = leftPanel.Q<Toggle>(UseDisplayNameAsRootFolderNameToggleName);

            // Root Folder Name
            var rootFolderNameContentsVisualElement =
                    leftPanel.Q<VisualElement>(RootFolderNameContentsVisualElementName);
            var rootFolderNameTextField = leftPanel.Q<TextField>(RootFolderNameTextFieldName);

            // Readme
            var readmeToggle = leftPanel.Q<Toggle>(ReadmeToggleName);

            // Changelog
            var changelogToggle = leftPanel.Q<Toggle>(ChangelogToggleName);

            // License
            var licenseToggle = leftPanel.Q<Toggle>(LicenceToggleName);

            // Third Party Notices
            var thirdPartyNoticesToggle = leftPanel.Q<Toggle>(ThirdPartyNoticesToggleName);

            // Editor Folder
            var editorFolderToggle = leftPanel.Q<Toggle>(EditorFolderToggleName);

            // Runtime Folder
            var runtimeFolderToggle = leftPanel.Q<Toggle>(RuntimeFolderToggleName);

            // Tests Folder
            var testsFolderToggle = leftPanel.Q<Toggle>(TestsFolderToggleName);

            // Tests Editor Folder
            var testsEditorFolderToggle = leftPanel.Q<Toggle>(TestsEditorFolderToggleName);

            // Tests Runtime Folder
            var testsRuntimeFolderToggle = leftPanel.Q<Toggle>(TestsRuntimeFolderToggleName);

            // Documentation
            var documentationFolderToggle = leftPanel.Q<Toggle>(DocumentationFolderToggleName);

            // Samples
            var samplesFolderToggle = leftPanel.Q<Toggle>(SamplesFolderToggleName);

            // Screenshots
            var screenshotsFolderToggle = leftPanel.Q<Toggle>(ScreenshotsFolderToggleName);

            // Name
            var packageNameExtensionTextField =
                    packageManifestVisualElement.Q<TextField>(PackageNameExtensionTextFieldName);
            var packageNameCompanyTextField =
                    packageManifestVisualElement.Q<TextField>(PackageNameCompanyTextFieldName);
            var packageNameTextField =
                    packageManifestVisualElement.Q<TextField>(PackageNameTextFieldName);

            // Version
            var packageVersionMajorIntegerField =
                    packageManifestVisualElement.Q<IntegerField>(PackageVersionMajorIntegerFieldName);
            var packageVersionMinorIntegerField =
                    packageManifestVisualElement.Q<IntegerField>(PackageVersionMinorIntegerFieldName);
            var packageVersionPatchIntegerField =
                    packageManifestVisualElement.Q<IntegerField>(PackageVersionPatchIntegerFieldName);

            // Unity 
            var packageUnityVersionMajorIntegerField =
                    packageManifestVisualElement.Q<IntegerField>(PackageUnityVersionMajorIntegerFieldName);
            var packageUnityVersionMinorIntegerField =
                    packageManifestVisualElement.Q<IntegerField>(PackageUnityVersionMinorIntegerFieldName);

            // Description
            var packageUnityDescriptionTextField =
                    packageManifestVisualElement.Q<TextField>(PackageDescriptionTextFieldName);

            // Author Name
            var authorNameTextField = packageManifestVisualElement.Q<TextField>(AuthorNameTextFieldName);

            // Author Email
            var authorEmailTextField = packageManifestVisualElement.Q<TextField>(AuthorEmailTextFieldName);

            // Author Url
            var authorUrlTextField = packageManifestVisualElement.Q<TextField>(AuthorUrlTextFieldName);

            // Unity Release
            var unityReleaseToggle = packageManifestVisualElement.Q<Toggle>(UnityReleaseToggleName);
            var unityReleaseTextField = packageManifestVisualElement.Q<TextField>(UnityReleaseTextFieldName);

            // Dependencies
            var dependenciesToggle = packageManifestVisualElement.Q<Toggle>(DependenciesToggleName);
            var dependenciesContent =
                    packageManifestVisualElement.Q<VisualElement>(DependenciesContentsVisualElementName);
            _dependenciesListView = packageManifestVisualElement.Q<ListView>(DependenciesListViewName);
            _dependencyEntryVisualTreeAsset = Resources.Load<VisualTreeAsset>(DependencyEntryUxmlPath);
            var addDependencyButton = packageManifestVisualElement.Q<Button>(AddDependencyButtonName);

            // Keywords
            var keywordsToggle = packageManifestVisualElement.Q<Toggle>(KeywordsToggleName);
            var keywordsContent = packageManifestVisualElement.Q<VisualElement>(KeywordsContentsVisualElementName);
            _keywordsListView = packageManifestVisualElement.Q<ListView>(KeywordsListViewName);
            _keywordsEntryVisualTreeAsset = Resources.Load<VisualTreeAsset>(KeywordEntryUxmlPath);
            var addKeywordButton = packageManifestVisualElement.Q<Button>(AddKeywordButtonName);

            // Readme
            var readmeVisualTree = Resources.Load<VisualTreeAsset>(ReadmeUxmlPath);
            var readmeVisualElement = new VisualElement();
            readmeVisualTree.CloneTree(readmeVisualElement);
            readmeVisualElement.style.flexGrow = 1;
            rightScrollView.Add(readmeVisualElement);
            var readmeTextField = readmeVisualElement.Q<TextField>(ReadmeTextFieldName);

            // Changelog
            var changelogVisualTree = Resources.Load<VisualTreeAsset>(ChangelogUxmlPath);
            var changelogVisualElement = new VisualElement();
            changelogVisualTree.CloneTree(changelogVisualElement);
            changelogVisualElement.style.flexGrow = 1;
            rightScrollView.Add(changelogVisualElement);
            var changelogTextField = changelogVisualElement.Q<TextField>(ChangelogTextFieldName);

            // License
            var licenseVisualTree = Resources.Load<VisualTreeAsset>(LicenseUxmlPath);
            var licenseVisualElement = new VisualElement();
            licenseVisualTree.CloneTree(licenseVisualElement);
            licenseVisualElement.style.flexGrow = 1;
            rightScrollView.Add(licenseVisualElement);
            var licenseTextField = licenseVisualElement.Q<TextField>(LicenseTextFieldName);

            // Third Party Notices
            var thirdPartyNoticesVisualTree = Resources.Load<VisualTreeAsset>(ThirdPartyNoticesUxmlPath);
            var thirdPartyNoticesVisualElement = new VisualElement();
            thirdPartyNoticesVisualTree.CloneTree(thirdPartyNoticesVisualElement);
            thirdPartyNoticesVisualElement.style.flexGrow = 1;
            rightScrollView.Add(thirdPartyNoticesVisualElement);
            var thirdPartyNoticesTextField =
                    thirdPartyNoticesVisualElement.Q<TextField>(ThirdPartyNoticesTextFieldName);

            var clearAllButton = _root.Q<Button>(ClearAllButtonName);

            // Update Package Button
            var updatePackageButton = _root.Q<Button>(UpdatePackageButtonName);
            updatePackageButton.SetEnabled(false);

            var saveToAssetButton = _root.Q<Button>("BT_SaveToAssets");
            saveToAssetButton.SetEnabled(false);

            // Create Package Button
            var createPackageButton = _root.Q<Button>(CreatePackageButtonName);

            #endregion

            #region BINDINGS

            // Setup
            _packageManifest = ScriptableObject.CreateInstance<PackageManifest>();
            var pmSerObj = new UnityEditor.SerializedObject(_packageManifest);

            // Display Name
            var displayNameProperty = pmSerObj.FindProperty(PackageManifestConstants.DisplayNamePropName);
            if (displayNameProperty != null)
            {
                displayNameTextField.BindProperty(displayNameProperty);
            }

            var isUseDisplayNameAsRootFolderNameProperty =
                    pmSerObj.FindProperty(PackageManifestConstants.IsUseDisplayNameAsRootFolderNamePropName);
            if (isUseDisplayNameAsRootFolderNameProperty != null)
            {
                useDisplayNameAsRootFolderNameToggle.BindProperty(isUseDisplayNameAsRootFolderNameProperty);
            }

            // Root Folder Name
            var rootFolderNameProperty = pmSerObj.FindProperty(PackageManifestConstants.RootFolderNamePropName);
            if (rootFolderNameProperty != null)
            {
                rootFolderNameTextField.BindProperty(rootFolderNameProperty);
            }

            // Readme
            var hasReadmeProperty = pmSerObj.FindProperty(PackageManifestConstants.HasReadmePropName);
            if (hasReadmeProperty != null)
            {
                readmeToggle.BindProperty(hasReadmeProperty);
            }

            // Changelog
            var hasChangelogProperty = pmSerObj.FindProperty(PackageManifestConstants.HasChangelogPropName);
            if (hasChangelogProperty != null)
            {
                changelogToggle.BindProperty(hasChangelogProperty);
            }

            // License
            var hasLicenseProperty = pmSerObj.FindProperty(PackageManifestConstants.HasLicensePropName);
            if (hasLicenseProperty != null)
            {
                licenseToggle.BindProperty(hasLicenseProperty);
            }

            // Third Party Notices
            var hasThirdPartyNoticesProperty =
                    pmSerObj.FindProperty(PackageManifestConstants.HasThirdPartyNoticesPropName);
            if (hasThirdPartyNoticesProperty != null)
            {
                thirdPartyNoticesToggle.BindProperty(hasThirdPartyNoticesProperty);
            }

            // Editor Folder
            var hasEditorFolderProperty = pmSerObj.FindProperty(PackageManifestConstants.HasEditorFolderPropName);
            if (hasEditorFolderProperty != null)
            {
                editorFolderToggle.BindProperty(hasEditorFolderProperty);
            }

            // Runtime Folder
            var hasRuntimeFolderProperty = pmSerObj.FindProperty(PackageManifestConstants.HasRuntimeFolderPropName);
            if (hasRuntimeFolderProperty != null)
            {
                runtimeFolderToggle.BindProperty(hasRuntimeFolderProperty);
            }

            // Tests Folder
            var hasTestsFolderProperty = pmSerObj.FindProperty(PackageManifestConstants.HasTestsFolderPropName);
            if (hasTestsFolderProperty != null)
            {
                testsFolderToggle.BindProperty(hasTestsFolderProperty);
            }

            // Tests Editor Folder
            var hasTestsEditorFolderProperty =
                    pmSerObj.FindProperty(PackageManifestConstants.HasTestsEditorFolderPropName);
            if (hasTestsEditorFolderProperty != null)
            {
                testsEditorFolderToggle.BindProperty(hasTestsEditorFolderProperty);
            }

            // Tests Runtime Folder
            var hasTestsRuntimeFolderProperty =
                    pmSerObj.FindProperty(PackageManifestConstants.HasTestsRuntimeFolderPropName);
            if (hasTestsRuntimeFolderProperty != null)
            {
                testsRuntimeFolderToggle.BindProperty(hasTestsRuntimeFolderProperty);
            }

            // Documentation Folder
            var hasDocumentationFolderProperty =
                    pmSerObj.FindProperty(PackageManifestConstants.HasDocumentationFolderPropName);
            if (hasDocumentationFolderProperty != null)
            {
                documentationFolderToggle.BindProperty(hasDocumentationFolderProperty);
            }

            // Samples Folder
            var hasSamplesFolderProperty = pmSerObj.FindProperty(PackageManifestConstants.HasSamplesFolderPropName);
            if (hasSamplesFolderProperty != null)
            {
                samplesFolderToggle.BindProperty(hasSamplesFolderProperty);
            }

            // Screenshots Folder
            var hasScreenshotsFolderProperty =
                    pmSerObj.FindProperty(PackageManifestConstants.HasScreenshotsFolderPropName);
            if (hasScreenshotsFolderProperty != null)
            {
                screenshotsFolderToggle.BindProperty(hasScreenshotsFolderProperty);
            }

            // Name
            var nameExtensionProperty = pmSerObj.FindProperty(PackageManifestConstants.NameExtensionPropName);
            if (nameExtensionProperty != null)
            {
                packageNameExtensionTextField.BindProperty(nameExtensionProperty);
            }

            var nameCompanyProperty = pmSerObj.FindProperty(PackageManifestConstants.NameCompanyPropName);
            if (nameCompanyProperty != null)
            {
                packageNameCompanyTextField.BindProperty(nameCompanyProperty);
            }

            var namePackageProperty = pmSerObj.FindProperty(PackageManifestConstants.NamePackagePropName);
            if (namePackageProperty != null)
            {
                packageNameTextField.BindProperty(namePackageProperty);
            }

            // Version
            var versionMajorProperty = pmSerObj.FindProperty(PackageManifestConstants.VersionMajorPropName);
            if (versionMajorProperty != null)
            {
                packageVersionMajorIntegerField.BindProperty(versionMajorProperty);
            }

            var versionMinorProperty = pmSerObj.FindProperty(PackageManifestConstants.VersionMinorPropName);
            if (versionMinorProperty != null)
            {
                packageVersionMinorIntegerField.BindProperty(versionMinorProperty);
            }

            var versionPatchProperty = pmSerObj.FindProperty(PackageManifestConstants.VersionPatchPropName);
            if (versionPatchProperty != null)
            {
                packageVersionPatchIntegerField.BindProperty(versionPatchProperty);
            }

            // Unity Version
            var unityVersionMajorProperty = pmSerObj.FindProperty(PackageManifestConstants.UnityVersionMajorPropName);
            if (unityVersionMajorProperty != null)
            {
                packageUnityVersionMajorIntegerField.BindProperty(unityVersionMajorProperty);
            }

            var unityVersionMinorProperty = pmSerObj.FindProperty(PackageManifestConstants.UnityVersionMinorPropName);
            if (unityVersionMinorProperty != null)
            {
                packageUnityVersionMinorIntegerField.BindProperty(unityVersionMinorProperty);
            }

            // Description
            var descriptionProperty = pmSerObj.FindProperty(PackageManifestConstants.DescriptionPropName);
            if (descriptionProperty != null)
            {
                packageUnityDescriptionTextField.BindProperty(descriptionProperty);
            }

            var authorNameProperty = pmSerObj.FindProperty(PackageManifestConstants.AuthorNamePropName);
            if (authorNameProperty != null)
            {
                authorNameTextField.BindProperty(authorNameProperty);
            }

            var authorEmailProperty = pmSerObj.FindProperty(PackageManifestConstants.AuthorEmailPropName);
            if (authorEmailProperty != null)
            {
                authorEmailTextField.BindProperty(authorEmailProperty);
            }

            var authorUrlProperty = pmSerObj.FindProperty(PackageManifestConstants.AuthorUrlPropName);
            if (authorUrlProperty != null)
            {
                authorUrlTextField.BindProperty(authorUrlProperty);
            }

            // Unity Release
            var hasUnityReleaseProperty = pmSerObj.FindProperty(PackageManifestConstants.HasUnityReleasePropName);
            if (hasUnityReleaseProperty != null)
            {
                unityReleaseToggle.BindProperty(hasUnityReleaseProperty);
            }

            var unityReleaseProperty = pmSerObj.FindProperty(PackageManifestConstants.UnityReleasePropName);
            if (unityReleaseProperty != null)
            {
                unityReleaseTextField.BindProperty(unityReleaseProperty);
            }

            // Dependencies
            var hasDependenciesProperty = pmSerObj.FindProperty(PackageManifestConstants.HasDependenciesPropName);
            if (hasDependenciesProperty != null)
            {
                dependenciesToggle.BindProperty(hasDependenciesProperty);
            }

            // Keywords
            var hasKeywordsProperty = pmSerObj.FindProperty(PackageManifestConstants.HasKeywordsPropName);
            if (hasKeywordsProperty != null)
            {
                keywordsToggle.BindProperty(hasKeywordsProperty);
            }

            // Readme Contents
            var readmeProperty = pmSerObj.FindProperty(PackageManifestConstants.ReadmePropName);
            if (readmeProperty != null)
            {
                readmeTextField.BindProperty(readmeProperty);
            }

            // Changelog Contents
            var changelogProperty = pmSerObj.FindProperty(PackageManifestConstants.ChangelogPropName);
            if (changelogProperty != null)
            {
                changelogTextField.BindProperty(changelogProperty);
            }

            // Licence Contents
            var licenseProperty = pmSerObj.FindProperty(PackageManifestConstants.LicensePropName);
            if (licenseProperty != null)
            {
                licenseTextField.BindProperty(licenseProperty);
            }

            // Third Party Notices
            var thirdPartyNoticesProperty = pmSerObj.FindProperty(PackageManifestConstants.ThirdPartyNoticesPropName);
            if (thirdPartyNoticesProperty != null)
            {
                thirdPartyNoticesTextField.BindProperty(thirdPartyNoticesProperty);
            }

            #endregion

            #region BEHAVIOR

            // Load Button
            loadPackageButton.clickable.clicked += () =>
                    LoadPackage(updatePackageButton, saveToAssetButton);

            // Tests Folder
            testsFolderToggle.RegisterValueChangedCallback(evt =>
            {
                if (testsFolderToggle.value != false)
                {
                    return;
                }

                testsEditorFolderToggle.value = false;
                testsRuntimeFolderToggle.value = false;
            });

            // Tests Editor Folder
            testsEditorFolderToggle.RegisterValueChangedCallback(evt =>
            {
                if (testsEditorFolderToggle.value != true)
                {
                    return;
                }

                testsFolderToggle.value = true;
            });

            // Tests Runtime Folder
            testsRuntimeFolderToggle.RegisterValueChangedCallback(evt =>
            {
                if (testsRuntimeFolderToggle.value == true)
                {
                    testsFolderToggle.value = true;
                }
            });

            // Display Name
            rootFolderNameContentsVisualElement.SetEnabled(!useDisplayNameAsRootFolderNameToggle.value);
            useDisplayNameAsRootFolderNameToggle.RegisterValueChangedCallback(evt =>
            {
                if (useDisplayNameAsRootFolderNameToggle.value)
                {
                    rootFolderNameTextField.value = displayNameTextField.value;
                }
                else
                {
                    rootFolderNameTextField.value = String.Empty;
                }

                rootFolderNameContentsVisualElement.SetEnabled(!useDisplayNameAsRootFolderNameToggle.value);
            });

            displayNameTextField.RegisterValueChangedCallback(evt =>
            {
                if (useDisplayNameAsRootFolderNameToggle.value)
                {
                    rootFolderNameTextField.value = displayNameTextField.value;
                }
            });

            // Unity Release
            unityReleaseTextField.SetEnabled(unityReleaseToggle.value);
            unityReleaseToggle.RegisterValueChangedCallback(evt => { unityReleaseTextField.SetEnabled(unityReleaseToggle.value); });

            // Dependencies
            dependenciesContent.SetEnabled(dependenciesToggle.value);
            dependenciesToggle.RegisterValueChangedCallback(evt => { dependenciesContent.SetEnabled(dependenciesToggle.value); });
            addDependencyButton.clickable.clicked += () => AddEntryToDependencies();

            // Keywords
            keywordsContent.SetEnabled(keywordsToggle.value);
            keywordsToggle.RegisterValueChangedCallback(evt => { keywordsContent.SetEnabled(keywordsToggle.value); });
            addKeywordButton.clickable.clicked += () => AddEntryToKeywords();

            // Root Folder Name
            // The user can't change the folder name in the Update Mode - they have to click Create New instead.
            rootFolderNameTextField.RegisterValueChangedCallback(evt =>
            {
                if (!Directory.Exists(_packageManifest.PackageAbsolutePath))
                {
                    return;
                }

                var directoryName = new DirectoryInfo(_packageManifest.PackageAbsolutePath).Name;
                updatePackageButton.SetEnabled(rootFolderNameTextField.value == directoryName);
                saveToAssetButton.SetEnabled(rootFolderNameTextField.value == directoryName);
            });

            // Readme
            readmeVisualElement.SetEnabled(readmeToggle.value);
            readmeToggle.RegisterValueChangedCallback(evt => { readmeVisualElement.SetEnabled(readmeToggle.value); });

            // Third Party Notices
            thirdPartyNoticesVisualElement.SetEnabled(thirdPartyNoticesToggle.value);
            thirdPartyNoticesToggle.RegisterValueChangedCallback(evt =>
            {
                thirdPartyNoticesVisualElement.SetEnabled(thirdPartyNoticesToggle.value);
            });

            // Changelog
            changelogVisualElement.SetEnabled(changelogToggle.value);
            changelogToggle.RegisterValueChangedCallback(evt => { changelogVisualElement.SetEnabled(changelogToggle.value); });

            // Licence
            licenseVisualElement.SetEnabled(licenseToggle.value);
            licenseToggle.RegisterValueChangedCallback(evt => { licenseVisualElement.SetEnabled(licenseToggle.value); });

            // Clear All Button
            clearAllButton.clickable.clicked += () =>
            {
                _packageManifest.ResetToDefault();
                _dependenciesListView.Clear();
                _keywordsListView.Clear();
            };

            // Update Package Button
            updatePackageButton.clickable.clicked += () =>
                    TryCreateNewUnityPackage(_packageManifest,
                        _packageManifest.PackageAbsolutePath);
            updatePackageButton.clickable.clicked += SaveToAssets;

            // Create Package Button
            createPackageButton.clickable.clicked += () =>
                    TryCreateNewUnityPackage(_packageManifest);
            saveToAssetButton.clickable.clicked += SaveToAssets;

            #endregion
        }

        private void LoadPackage(Button updatePackageButton, Button SaveBtn)
        {
            // Clear the package manifest
            _packageManifest.ResetToDefault();

            // Get path
            var parentDirectoryPath = EditorUtility.OpenFolderPanel(LoadPackageWindowTitle, "", "");
            if (String.IsNullOrWhiteSpace(parentDirectoryPath))
            {
                return;
            }

            _packageManifest.PackageAbsolutePath = parentDirectoryPath;

            // package.json

            #region package.json

            // Package Reading
            var packageJsonPath = Path.Combine(parentDirectoryPath, PackageManifestConstants.JsonFileName);

            if (!File.Exists(packageJsonPath))
            {
                EditorUtility.DisplayDialog(NoPackageManifestTitle, NoPackageManifestMessage, NoPackageManifestOk);
                return;
            }

            var dictionary = JsonUtilities.GetData(packageJsonPath);
            if (dictionary == null)
            {
                return;
            }

            // Display Name
            var displayName = string.Empty;
            if (dictionary.ContainsKey(PackageManifestConstants.JsonDisplayName))
            {
                displayName = (string)dictionary[PackageManifestConstants.JsonDisplayName];
            }

            _packageManifest.DisplayName = displayName;

            // Root Folder Name
            var rootFolderName = new DirectoryInfo(parentDirectoryPath).Name;
            _packageManifest.RootFolderName = rootFolderName;
            _packageManifest.IsUseDisplayNameAsRootFolderName = displayName == rootFolderName;

            // Name
            var fullName = string.Empty;
            var nameExtension = string.Empty;
            var nameCompany = string.Empty;
            var namePackage = string.Empty;

            if (dictionary.ContainsKey(PackageManifestConstants.JsonName))
            {
                fullName = (string)dictionary[PackageManifestConstants.JsonName];
            }

            if (!string.IsNullOrWhiteSpace(fullName))
            {
                var namePieces = fullName.Split(Period);

                // TODO: Validate contents

                nameExtension = namePieces[0];
                nameCompany = namePieces[1];
                namePackage = namePieces[2];
            }

            _packageManifest.NameExtension = nameExtension;
            _packageManifest.NameCompany = nameCompany;
            _packageManifest.NamePackage = namePackage;

            // Version
            var fullVersion = string.Empty;
            var versionMajor = string.Empty;
            var versionMinor = string.Empty;
            var versionPatch = string.Empty;

            if (dictionary.ContainsKey(PackageManifestConstants.JsonVersion))
            {
                fullVersion = (string)dictionary[PackageManifestConstants.JsonVersion];
            }

            if (!string.IsNullOrWhiteSpace(fullVersion))
            {
                var versionPieces = fullVersion.Split(Period);

                // TODO: Validate contents

                versionMajor = versionPieces[0];
                versionMinor = versionPieces[1];
                versionPatch = versionPieces[2];
            }

            _packageManifest.VersionMajor = int.Parse(versionMajor);
            _packageManifest.VersionMinor = int.Parse(versionMinor);
            _packageManifest.VersionPatch = int.Parse(versionPatch);

            // Unity Version
            var fullUnityVersion = string.Empty;
            var unityVersionMajor = string.Empty;
            var unityVersionMinor = string.Empty;

            if (dictionary.ContainsKey(PackageManifestConstants.JsonUnity))
            {
                fullUnityVersion = (string)dictionary[PackageManifestConstants.JsonUnity];
            }

            if (!string.IsNullOrWhiteSpace(fullUnityVersion))
            {
                var unityVersionPieces = fullUnityVersion.Split(Period);

                // TODO: Validate contents

                unityVersionMajor = unityVersionPieces[0];
                unityVersionMinor = unityVersionPieces[1];
            }

            _packageManifest.UnityVersionMajor = int.Parse(unityVersionMajor);
            _packageManifest.UnityVersionMinor = int.Parse(unityVersionMinor);

            // Description
            var description = string.Empty;
            if (dictionary.ContainsKey(PackageManifestConstants.JsonDescription))
            {
                description = (string)dictionary[PackageManifestConstants.JsonDescription];
            }

            _packageManifest.Description = description;

            // Author
            if (dictionary.ContainsKey(PackageManifestConstants.JsonAuthor))
            {
                var authorContents = dictionary[PackageManifestConstants.JsonAuthor];
                var authorDictionary = new Dictionary<string, string>();
                try
                {
                    var serializedAuthorContents = JsonConvert.SerializeObject(authorContents);
                    authorDictionary =
                            JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedAuthorContents);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }

                if (authorDictionary != null)
                {
                    if (authorDictionary.TryGetValue(PackageManifestConstants.JsonAuthorName, out var authorName))
                    {
                        _packageManifest.AuthorName = authorName;
                    }

                    // Author Email
                    if (authorDictionary.TryGetValue(PackageManifestConstants.JsonAuthorEmail, out var authorEmail))
                    {
                        _packageManifest.AuthorEmail = authorEmail;
                    }

                    // Author Url
                    if (authorDictionary.TryGetValue(PackageManifestConstants.JsonAuthorUrl, out var authorUrl))
                    {
                        _packageManifest.AuthorUrl = authorUrl;
                    }
                }
            }

            // Unity Release
            if (dictionary.ContainsKey(PackageManifestConstants.JsonUnityRelease))
            {
                var unityRelease = (string)dictionary[PackageManifestConstants.JsonUnityRelease];
                _packageManifest.HasUnityRelease = true;
                _packageManifest.UnityRelease = unityRelease;
            }

            // Dependencies
            if (dictionary.ContainsKey(PackageManifestConstants.JsonDependencies))
            {
                _packageManifest.HasDependencies = true;

                var dependenciesContents = dictionary[PackageManifestConstants.JsonDependencies];

                var dependenciesDictionary = new Dictionary<string, string>();
                try
                {
                    var serializedDependenciesContents = JsonConvert.SerializeObject(dependenciesContents);
                    dependenciesDictionary =
                            JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedDependenciesContents);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }

                foreach (var dependencyRelationship in dependenciesDictionary)
                {
                    var dependency = ScriptableObject.CreateInstance<PackageDependency>();
                    dependency.DependencyName = dependencyRelationship.Key;
                    dependency.DependencyVersion = dependencyRelationship.Value;
                    AddEntryToDependencies(dependency.DependencyName, dependency.DependencyVersion);
                }
            }

            // Keywords
            if (dictionary.ContainsKey(PackageManifestConstants.JsonKeywords))
            {
                _packageManifest.HasKeywords = true;

                var keywordsContents = dictionary[PackageManifestConstants.JsonKeywords];

                var keywordsList = new List<string>();
                try
                {
                    var serializedKeywordsContents = JsonConvert.SerializeObject(keywordsContents);
                    keywordsList =
                            JsonConvert.DeserializeObject<List<string>>(serializedKeywordsContents);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }

                foreach (var keywordValue in keywordsList)
                {
                    AddEntryToKeywords(keywordValue);
                }
            }

            #endregion

            // README.MD
            var readmeMdPath = Path.Combine(parentDirectoryPath, PackageManifestConstants.ReadmeMdFileName);
            if (File.Exists(readmeMdPath))
            {
                var streamReader = new StreamReader(readmeMdPath);
                var readme = streamReader.ReadToEnd();
                _packageManifest.HasReadme = true;
                _packageManifest.Readme = readme;
                streamReader.Close();
            }
            else
            {
                _packageManifest.HasReadme = false;
            }

            // CHANGELOG.MD
            var changelogMdPath = Path.Combine(parentDirectoryPath, PackageManifestConstants.ChangelogMdFileName);
            if (File.Exists(changelogMdPath))
            {
                var streamReader = new StreamReader(changelogMdPath);
                var changelog = streamReader.ReadToEnd();
                _packageManifest.HasChangelog = true;
                _packageManifest.Changelog = changelog;
                streamReader.Close();
            }
            else
            {
                _packageManifest.HasChangelog = false;
            }

            // LICENSE
            var licensePath = Path.Combine(parentDirectoryPath, PackageManifestConstants.LicenseFileName);
            if (File.Exists(licensePath))
            {
                var streamReader = new StreamReader(licensePath);
                var license = streamReader.ReadToEnd();
                _packageManifest.HasLicense = true;
                _packageManifest.License = license;
                streamReader.Close();
            }
            else
            {
                _packageManifest.HasLicense = false;
            }

            // Third Party Notices.md
            var thirdPartyNoticesPath =
                    Path.Combine(parentDirectoryPath, PackageManifestConstants.ThirdPartyNoticesFileName);
            if (File.Exists(thirdPartyNoticesPath))
            {
                var streamReader = new StreamReader(thirdPartyNoticesPath);
                var thirdPartyNotices = streamReader.ReadToEnd();
                _packageManifest.HasThirdPartyNotices = true;
                _packageManifest.ThirdPartyNotices = thirdPartyNotices;
                streamReader.Close();
            }
            else
            {
                _packageManifest.HasThirdPartyNotices = false;
            }

            // Editor Folder
            var editorFolderPath = Path.Combine(parentDirectoryPath, PackageManifestConstants.EditorFolderName);
            _packageManifest.HasEditorFolder = Directory.Exists(editorFolderPath);

            // Runtime Folder
            var runtimeFolderPath = Path.Combine(parentDirectoryPath, PackageManifestConstants.RuntimeFolderName);
            _packageManifest.HasRuntimeFolder = Directory.Exists(runtimeFolderPath);

            // Tests Folder
            var testsFolderPath = Path.Combine(parentDirectoryPath, PackageManifestConstants.TestsFolderName);
            if (Directory.Exists(testsFolderPath))
            {
                _packageManifest.HasTestsFolder = true;

                // Tests Editor Folder
                var testsEditorFolderPath =
                        Path.Combine(testsFolderPath, PackageManifestConstants.TestsEditorFolderName);
                _packageManifest.HasTestsEditorFolder = Directory.Exists(testsEditorFolderPath);

                // Tests Runtime Folder
                var testsRuntimeFolderPath =
                        Path.Combine(testsFolderPath, PackageManifestConstants.TestsRuntimeFolderName);
                _packageManifest.HasTestsRuntimeFolder = Directory.Exists(testsRuntimeFolderPath);
            }

            // Documentation Folder
            var documentationFolderPath =
                    Path.Combine(parentDirectoryPath, PackageManifestConstants.DocumentationFolderName);
            _packageManifest.HasDocumentationFolder = Directory.Exists(documentationFolderPath);

            // Samples Folder
            var samplesFolderPath = Path.Combine(parentDirectoryPath, PackageManifestConstants.SamplesFolderName);
            _packageManifest.HasSamplesFolder = Directory.Exists(samplesFolderPath);

            // Screenshots Folder
            var screenshotsFolderPath =
                    Path.Combine(parentDirectoryPath, PackageManifestConstants.ScreenshotsFolderName);
            _packageManifest.HasScreenshotsFolder = Directory.Exists(screenshotsFolderPath);

            // Enable Update Button
            updatePackageButton.SetEnabled(true);

            SaveBtn.SetEnabled(true);
        }

        private void AddEntryToDependencies(string dependencyName = "", string dependencyVersion = "")
        {
            var customVisualElement = new VisualElement();
            _dependencyEntryVisualTreeAsset.CloneTree(customVisualElement);
            _dependenciesListView.hierarchy.Add(customVisualElement);

            var packageDependency = ScriptableObject.CreateInstance<PackageDependency>();
            var pmSerObjPackageDependency = new UnityEditor.SerializedObject(packageDependency);
            //依赖数据
            var dependencyNameProperty =
                    pmSerObjPackageDependency.FindProperty("dependencyName");
            var dependencyVersionProperty =
                    pmSerObjPackageDependency.FindProperty("dependencyVersion");
            _packageManifest.Dependencies.Add(packageDependency);

            var entryNameTextField = customVisualElement.Q<TextField>(DependencyEntryNameTextFieldName);
            entryNameTextField.BindProperty(dependencyNameProperty);
            entryNameTextField.value = dependencyName;

            var entryVersionTextField = customVisualElement.Q<TextField>(DependencyEntryVersionTextFieldName);
            entryVersionTextField.BindProperty(dependencyVersionProperty);
            entryVersionTextField.value = dependencyVersion;

            var removeButton = customVisualElement.Q<Button>(DependencyEntryRemoveButtonName);
            removeButton.clickable.clicked += () =>
            {
                _dependenciesListView.hierarchy.Remove(customVisualElement);
                _packageManifest.Dependencies.Remove(packageDependency);
            };

            _dependenciesListView.RefreshItems();
        }

        private void AddEntryToKeywords(string keywordName = "")
        {
            var customVisualElement = new VisualElement();
            _keywordsEntryVisualTreeAsset.CloneTree(customVisualElement);
            _keywordsListView.hierarchy.Add(customVisualElement);

            var packageKeyword = ScriptableObject.CreateInstance<PackageKeyword>();
            var pmSerObjPackageKeyword = new UnityEditor.SerializedObject(packageKeyword);
            var keywordValueProperty =
                    pmSerObjPackageKeyword.FindProperty("keywordValue");
            _packageManifest.Keywords.Add(packageKeyword);

            var entryNameTextField = customVisualElement.Q<TextField>(KeywordEntryNameTextFieldName);
            entryNameTextField.BindProperty(keywordValueProperty);
            entryNameTextField.value = keywordName;
            var removeButton = customVisualElement.Q<Button>(RemoveKeywordButtonName);
            removeButton.clickable.clicked += () =>
            {
                _keywordsListView.hierarchy.Remove(customVisualElement);
                _packageManifest.Keywords.Remove(packageKeyword);
            };

            _keywordsListView.RefreshItems();
        }

        private static void TryCreateNewUnityPackage(PackageManifest packageManifest, string rootFolderPath = "")
        {
            // Validate
            if (!packageManifest.IsValidPackageManifest())
            {
                EditorUtility.DisplayDialog(InvalidPackageErrorTitle, InvalidPackageErrorMessage, InvalidPackageOk);
                return;
            }

            // Get path if it doesn't exists
            if (string.IsNullOrWhiteSpace(rootFolderPath))
            {
                var parentDirectoryPath = EditorUtility.OpenFolderPanel(CreatePackagesWindowTitle, "", "");
                if (String.IsNullOrWhiteSpace(parentDirectoryPath))
                {
                    return;
                }

                // Create Root Folder
                rootFolderPath = Path.GetFullPath(Path.Combine(parentDirectoryPath, packageManifest.RootFolderName));
                if (Directory.Exists(rootFolderPath))
                {
                    var isOverride = EditorUtility.DisplayDialog(OverridePackageTitle, OverridePackageMessage,
                        OverrideYes,
                        OverrideCancel);
                    if (!isOverride)
                    {
                        return;
                    }
                }

                Directory.CreateDirectory(rootFolderPath);
            }

            // package.json
            var packageJsonFilePath = Path.Combine(rootFolderPath, PackageManifestConstants.JsonFileName);
            var packageDictionary = new Dictionary<string, object>();

            var packageName =
                    packageManifest.NameExtension + Period +
                    packageManifest.NameCompany + Period +
                    packageManifest.NamePackage;
            packageDictionary[PackageManifestConstants.JsonName] = packageName;

            var packageVersion =
                    packageManifest.VersionMajor.ToString() + Period +
                    packageManifest.VersionMinor.ToString() + Period +
                    packageManifest.VersionPatch.ToString();
            packageDictionary[PackageManifestConstants.JsonVersion] = packageVersion;

            var packageDisplayName = packageManifest.DisplayName;
            packageDictionary[PackageManifestConstants.JsonDisplayName] = packageDisplayName;

            var packageDescription = packageManifest.Description;
            packageDictionary[PackageManifestConstants.JsonDescription] = packageDescription;

            var packageUnity =
                    packageManifest.UnityVersionMajor.ToString() + Period +
                    packageManifest.UnityVersionMinor.ToString();
            packageDictionary[PackageManifestConstants.JsonUnity] = packageUnity;

            if (packageManifest.HasUnityRelease)
            {
                var packageUnityRelease = packageManifest.UnityRelease;
                packageDictionary[PackageManifestConstants.JsonUnityRelease] = packageUnityRelease;
            }

            if (packageManifest.HasDependencies)
            {
                var dependencies = packageManifest.Dependencies;
                var dependenciesDictionary = new Dictionary<string, string>();

                foreach (var dependency in dependencies)
                {
                    dependenciesDictionary[dependency.DependencyName]
                            = dependency.DependencyVersion;
                }

                packageDictionary[PackageManifestConstants.JsonDependencies] = dependenciesDictionary;
            }

            if (packageManifest.HasKeywords)
            {
                var keywords = packageManifest.Keywords;
                var keywordsList = new List<string>();

                foreach (var t in keywords)
                {
                    keywordsList.Add(t.KeywordValue);
                }

                packageDictionary[PackageManifestConstants.JsonKeywords] = keywordsList;
            }

            if (packageManifest.AuthorName != "" || packageManifest.AuthorEmail != "" || packageManifest.AuthorUrl != "")
            {
                var author = new Dictionary<string, string>
                {
                    [PackageManifestConstants.JsonAuthorName] = packageManifest.AuthorName,
                    [PackageManifestConstants.JsonAuthorEmail] = packageManifest.AuthorEmail,
                    [PackageManifestConstants.JsonAuthorUrl] = packageManifest.AuthorUrl
                };
                packageDictionary[PackageManifestConstants.JsonAuthor] = author;
            }

            JsonUtilities.SetData(packageDictionary, packageJsonFilePath);

            // README.MD
            if (packageManifest.HasReadme)
            {
                var readmeMdFilePath = Path.Combine(rootFolderPath, PackageManifestConstants.ReadmeMdFileName);
                var readmeWriter = File.CreateText(readmeMdFilePath);
                readmeWriter.Write(packageManifest.Readme);
                readmeWriter.Close();
            }

            // CHANGELOG.MD
            if (packageManifest.HasChangelog)
            {
                var changelogMdFilePath = Path.Combine(rootFolderPath, PackageManifestConstants.ChangelogMdFileName);
                var changelogWriter = File.CreateText(changelogMdFilePath);
                changelogWriter.Write(packageManifest.Changelog);
                changelogWriter.Close();
            }

            // LICENSE
            if (packageManifest.HasLicense)
            {
                var licenseFilePath = Path.Combine(rootFolderPath, PackageManifestConstants.LicenseFileName);
                var licenseWriter = File.CreateText(licenseFilePath);
                licenseWriter.Write(packageManifest.License);
                licenseWriter.Close();
            }

            // Third Party Notices.md
            if (packageManifest.HasThirdPartyNotices)
            {
                var thirdPartyNoticesFilePath =
                        Path.Combine(rootFolderPath, PackageManifestConstants.ThirdPartyNoticesFileName);
                var thirdPartyNoticesWriter = File.CreateText(thirdPartyNoticesFilePath);
                thirdPartyNoticesWriter.Write(packageManifest.ThirdPartyNotices);
                thirdPartyNoticesWriter.Close();
            }

            // Folders
            // Editor Folder
            if (packageManifest.HasEditorFolder)
            {
                Directory.CreateDirectory(Path.Combine(rootFolderPath, PackageManifestConstants.EditorFolderName));
                var editorFolderNullFile = File.Create(Path.Combine(rootFolderPath,
                    PackageManifestConstants.EditorFolderName,
                    PackageManifestConstants.EmptyFileName));
                editorFolderNullFile.Close();
            }

            // Runtime Folder
            if (packageManifest.HasRuntimeFolder)
            {
                Directory.CreateDirectory(Path.Combine(rootFolderPath, PackageManifestConstants.RuntimeFolderName));
                var runtimeFolderNullFile = File.Create(Path.Combine(rootFolderPath,
                    PackageManifestConstants.RuntimeFolderName,
                    PackageManifestConstants.EmptyFileName));
                runtimeFolderNullFile.Close();
            }

            // Tests Folder
            if (packageManifest.HasTestsFolder)
            {
                var testsFolderPath = Path.Combine(rootFolderPath, PackageManifestConstants.TestsFolderName);
                Directory.CreateDirectory(testsFolderPath);
                var testsFolderNullFile =
                        File.Create(Path.Combine(testsFolderPath, PackageManifestConstants.EmptyFileName));
                testsFolderNullFile.Close();

                // Tests Editor Folder
                if (packageManifest.HasTestsEditorFolder)
                {
                    var testsEditorFolderPath =
                            Path.Combine(testsFolderPath, PackageManifestConstants.TestsEditorFolderName);
                    Directory.CreateDirectory(testsEditorFolderPath);
                    var testsEditorFolderNullFile =
                            File.Create(Path.Combine(testsEditorFolderPath, PackageManifestConstants.EmptyFileName));
                    testsEditorFolderNullFile.Close();
                }

                // Tests Runtime Folder
                if (packageManifest.HasTestsRuntimeFolder)
                {
                    var testsRuntimeFolderPath =
                            Path.Combine(rootFolderPath, PackageManifestConstants.TestsRuntimeFolderName);
                    Directory.CreateDirectory(testsRuntimeFolderPath);
                    var testsRuntimeFolderNullFile =
                            File.Create(Path.Combine(testsRuntimeFolderPath, PackageManifestConstants.EmptyFileName));
                    testsRuntimeFolderNullFile.Close();
                }
            }

            // Documentation Folder
            if (packageManifest.HasDocumentationFolder)
            {
                var documentationFolderPath =
                        Path.Combine(rootFolderPath, PackageManifestConstants.DocumentationFolderName);
                Directory.CreateDirectory(documentationFolderPath);
                var documentationFolderNullFile =
                        File.Create(Path.Combine(documentationFolderPath, PackageManifestConstants.EmptyFileName));
                documentationFolderNullFile.Close();
            }

            // Samples Folder
            if (packageManifest.HasSamplesFolder)
            {
                var samplesFolderPath = Path.Combine(rootFolderPath, PackageManifestConstants.SamplesFolderName);
                Directory.CreateDirectory(samplesFolderPath);
                var samplesFolderNullFile =
                        File.Create(Path.Combine(samplesFolderPath, PackageManifestConstants.EmptyFileName));
                samplesFolderNullFile.Close();
            }

            // Screenshots Folder
            if (packageManifest.HasScreenshotsFolder)
            {
                Directory.CreateDirectory(Path.Combine(rootFolderPath, PackageManifestConstants.ScreenshotsFolderName));
            }

            // Success prompt
            EditorUtility.DisplayDialog(SuccessCreatePackageTitle, SuccessCreatePackageMessage, SuccessCreatePackageOk);
        }

        private void SaveToAssets()
        {
            if (_packageManifest == null)
            {
                Debug.LogError("没有数据");
                return;
            }

            var _packages = AssetDatabase.LoadAssetAtPath<ExpansionPackages>
            ("Assets/Config/ExpansionPackages.asset");

            if (_packages == null)
            {
                Debug.LogError("找不到数据集");
                return;
            }

            if ( _packages.Get(_packageManifest.NamePackage)!=null)
            {
                Debug.LogError("已有相同的名称数据存在");
                return;
            }
            
            ExpansionPackageInfo packageInfo = new ExpansionPackageInfo();
            packageInfo.name = _packageManifest.NamePackage;
            packageInfo.displayName = _packageManifest.DisplayName;
            packageInfo.description = _packageManifest.Description;
            packageInfo.gitUrl = _packageManifest.AuthorUrl;
            packageInfo.version = _packageManifest.VersionMajor
                    + "." + _packageManifest.VersionMinor
                    + "." + _packageManifest.VersionPatch;
            packageInfo.quick_start = _packageManifest.Readme;
            packageInfo.change_log = _packageManifest.Changelog;
            packageInfo.author = _packageManifest.AuthorName;
            packageInfo.dependencies = new List<string>();
            if (_packageManifest.Dependencies is { Count: > 0 })
            {
                foreach (var VARIABLE in _packageManifest.Dependencies)
                {
                    packageInfo.dependencies.Add(VARIABLE.DependencyName);
                }
            }

            _packages.packages.Add(packageInfo);
            _packages.Save();
            Debug.Log("成功保存到私人库中");
        }
    }
}