﻿// Copyright © 2017-2025 QL-Win Contributors
//
// This file is part of QuickLook program.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using QuickLook.Common.ExtensionMethods;
using QuickLook.Common.Helpers;
using QuickLook.Common.Plugin;
using QuickLook.Plugin.AppViewer.PackageParsers.Dmg;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QuickLook.Plugin.AppViewer.InfoPanels;

public partial class DmgInfoPanel : UserControl, IAppInfoPanel
{
    private readonly ContextObject _context;

    public DmgInfoPanel(ContextObject context)
    {
        _context = context;

        DataContext = this;
        InitializeComponent();

        string translationFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Translations.config");
        applicationNameTitle.Text = TranslationHelper.Get("APP_NAME", translationFile);
        versionNameTitle.Text = TranslationHelper.Get("APP_VERSION_NAME", translationFile);
        versionCodeTitle.Text = TranslationHelper.Get("APP_VERSION_CODE", translationFile);
        packageNameTitle.Text = TranslationHelper.Get("PACKAGE_NAME", translationFile);
        minimumOSVersionTitle.Text = TranslationHelper.Get("APP_MIN_OS_VERSION", translationFile);
        deviceFamilyTitle.Text = TranslationHelper.Get("DEVICE_FAMILY", translationFile);
        platformVersionTitle.Text = TranslationHelper.Get("APP_TARGET_OS_VERSION", translationFile);
        totalSizeTitle.Text = TranslationHelper.Get("TOTAL_SIZE", translationFile);
        modDateTitle.Text = TranslationHelper.Get("LAST_MODIFIED", translationFile);
        permissionsGroupBox.Header = TranslationHelper.Get("PERMISSIONS", translationFile);
    }

    public void DisplayInfo(string path)
    {
        var name = Path.GetFileName(path);
        filename.Text = string.IsNullOrEmpty(name) ? path : name;

        _ = Task.Run(() =>
        {
            if (File.Exists(path))
            {
                var size = new FileInfo(path).Length;
                DmgInfo dmgInfo = DmgParser.Parse(path);
                var last = File.GetLastWriteTime(path);

                Dispatcher.Invoke(() =>
                {
                    applicationName.Text = dmgInfo.DisplayName;
                    versionName.Text = dmgInfo.VersionName;
                    versionCode.Text = dmgInfo.VersionCode;
                    packageName.Text = dmgInfo.Identifier;
                    minimumOSVersion.Text = dmgInfo.MinimumOSVersion;
                    platformVersion.Text = dmgInfo.PlatformVersion;
                    deviceFamily.Text = dmgInfo.SupportedPlatforms;
                    totalSize.Text = size.ToPrettySize(2);
                    modDate.Text = last.ToString(CultureInfo.CurrentCulture);
                    permissions.ItemsSource = dmgInfo.Permissions;

                    if (dmgInfo.HasIcon)
                    {
                        image.Source = dmgInfo.Logo.ToBitmapSource();
                    }
                    else
                    {
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/QuickLook.Plugin.AppViewer;component/Resources/ios.png"));
                    }

                    _context.IsBusy = false;
                });
            }
        });
    }
}
