﻿' Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

' All help keywords for anything inside this assembly should be defined here,
'   so that it is easier for UE to find them.
'

Option Explicit On
Option Strict On
Option Compare Binary

'****************************************************
'*****  Property Page Help IDs
'****************************************************

Namespace Microsoft.VisualStudio.Editors.PropertyPages

    Friend NotInheritable Class HelpKeywords
        Friend Const VBProjPropVersion As String = "vb.ProjectPropertiesVersion"
        Friend Const VBProjPropSigning As String = "vb.ProjectPropertiesSigning"
        Friend Const VBProjPropAdvancedCompile As String = "vb.ProjectPropertiesAdvancedCompile"
        Friend Const VBProjPropAdvancedServices As String = "vb.ProjectPropertiesAdvancedServices"
        Friend Const VBProjPropApplication As String = "vb.ProjectPropertiesApplication"
        Friend Const VBProjPropApplicationWPF As String = "vb.ProjectPropertiesApplicationWPF"
        Friend Const VBProjPropAssemblyInfo As String = "vb.ProjectPropertiesAssemblyInfo"
        Friend Const VBProjPropCompile As String = "vb.ProjectPropertiesCompile"
        Friend Const VBProjPropDatabase As String = "vdt.ProjectProperties.SQL.Database"
        Friend Const VBProjPropDebug As String = "vb.ProjectPropertiesDebug"
        Friend Const VBProjPropDeploy As String = "vdt.ProjectProperties.SQL.Deploy"
        Friend Const VBProjPropReference As String = "vb.ProjectPropertiesReference"
        Friend Const VBProjPropReferencePaths As String = "vb.ProjectPropertiesReferencePaths"
        Friend Const VBProjPropSettingsLogin As String = "vb.ProjectPropertiesSettingsLogin"
        Friend Const VBProjPropImports As String = "vb.ProjectPropertiesImports"
        Friend Const VBProjPropUnusedReference As String = "vb.ProjectPropertiesUnusedReference"
        Friend Const VBProjPropSecurity As String = "vb.ProjectPropertiesSecurity"
        Friend Const VBProjPropSecurityLink As String = "vb.ProjectPropertiesSecurity.HowTo"
        Friend Const VBProjPropServices As String = "vb.ProjectPropertiesServices"
        Friend Const VBProjPropXBAPSecurity As String = "vb.XBAPProjectPropertiesSecurity"
        Friend Const VBProjPropXBAPSecurityLink As String = "vb.XBAPProjectPropertiesSecurity.HowTo"
        Friend Const VBProjPropXBAPAllowPartiallyTrustedCallers As String = "System.Security.AllowPartiallyTrustedCallersAttribute"
        Friend Const VBProjPropSigningNewCertDialog As String = "vb.ProjectPropertiesSigning.PfxPasswordDialog"
        Friend Const VBProjPropSigningPasswordNeededDialog As String = "vb.ProjectPropertiesSigning.PasswordNeededDialog"
        Friend Const VBProjPropSigningChangePasswordDialog As String = "vb.ProjectPropertiesSigning.ChangePasswordDialog"
        Friend Const VBProjPropBuildEvents As String = "vb.ProjectPropertiesBuildEvents"
        Friend Const VBProjPropBuildEventsBuilder As String = "vb.ProjectPropertiesBuildEventsBuilder"

        Friend Const CSProjPropApplication As String = "cs.ProjectPropertiesApplication"
        Friend Const CSProjPropBuild As String = "cs.ProjectPropertiesBuild"
        Friend Const CSProjPropBuildEvents As String = "cs.ProjectPropertiesBuildEvents"
        Friend Const CSProjPropBuildEventsBuilder As String = "cs.ProjectPropertiesBuildEventsBuilder"
        Friend Const CSProjPropReference As String = "cs.ProjectPropertiesReferencePaths"
        Friend Const CSProjPropReferencePaths As String = "cs.ProjectPropertiesReferencePaths"
        Friend Const CSProjPropAdvancedCompile As String = "cs.AdvancedBuildSettings"

        Friend Const JSProjPropApplication As String = "js.ProjectPropertiesApplication"
        Friend Const JSProjPropBuild As String = "js.ProjectPropertiesBuild"
        Friend Const JSProjPropAdvancedCompile As String = "js.AdvancedBuildSettings"
        Friend Const JSProjPropReferencePaths As String = "js.ProjectPropertiesReferencePaths"

        Friend Const VBProjPropWPFApp_CantOpenOrCreateAppXaml As String = "vb.ProjProp.WPFApp.CantOpenOrCreateAppXaml"
        Friend Const VBProjPropWPFApp_AppXamlOpenInUnsupportedEditor As String = "vb.ProjProp.WPFApp.AppXamlOpenInUnsupportedEditor"
        Friend Const VBProjPropWPFApp_CouldntCreateApplicationEventsFile As String = "vb.ProjProp.WPFApp.CouldntCreateApplicationEventsFile"
    End Class

End Namespace

'****************************************************
'*****  Settings Designer Help IDs
'****************************************************

Namespace Microsoft.VisualStudio.Editors.SettingsDesigner

    Friend NotInheritable Class HelpIDs

        'General errors
        Public Const Err_LoadingSettingsFile As String = "msvse_settingsdesigner.Err.LoadingSettingsFile"
        Public Const Err_LoadingAppConfigFile As String = "msvse_settingsdesigner.Err.LoadingAppConfigFile"
        Public Const Err_SavingAppConfigFile As String = "msvse_settingsdesigner.Err.SavingAppConfigFile"
        Public Const Err_NameBlank As String = "msvse_settingsdesigner.Err.NameBlank"
        Public Const Err_InvalidName As String = "msvse_settingsdesigner.Err.InvalidName"
        Public Const Err_DuplicateName As String = "msvse_settingsdesigner.Err.DuplicateName"
        Public Const Err_FormatValue As String = "msvse_settingsdesigner.Err.FormatValue"
        Public Const Err_ViewCode As String = "msvse_settingsdesigner.Err.ViewCode"

        ' Synchronize user config
        Public Const Err_SynchronizeUserConfig As String = "msvse_settingsdesigner.SynchronizeUserConfig"

        'Dialogs
        Public Const Dlg_PickType As String = "msvse_settingsdesigner.dlg.PickType"

        ' Help keyword for description link in settings designer
        Public Const SettingsDesignerDescription As String = "ApplicationSettingsOverview"

        'My.Settings help keyword (generated into the .settings.designer.vb file in VB)
        Public Const MySettingsHelpKeyword As String = "My.Settings"

        ' Can't create this!
        Private Sub New()
        End Sub
    End Class

End Namespace

'****************************************************
'*****  My Extensibility Design-Time Tools HelpIDs
'****************************************************
Namespace Microsoft.VisualStudio.Editors.MyExtensibility
    Friend NotInheritable Class HelpIDs
        Public Const Dlg_AddMyNamespaceExtensions As String = "vb.AddingMyExtensions"
        Public Const VBProjPropMyExtensions As String = "vb.ProjectPropertiesMyExtensions"

        Private Sub New()
        End Sub
    End Class
End Namespace
