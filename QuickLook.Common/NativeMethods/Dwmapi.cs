// Copyright © 2017-2026 QL-Win Contributors
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

using System.Runtime.InteropServices;

namespace QuickLook.Common.NativeMethods;

public static class Dwmapi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Margins(int cxLeftWidth, int cxRightWidth, int cyTopHeight, int cyBottomHeight)
    {
        public int cxLeftWidth = cxLeftWidth;
        public int cxRightWidth = cxRightWidth;
        public int cyTopHeight = cyTopHeight;
        public int cyBottomHeight = cyBottomHeight;
    }

    public enum WindowAttribute
    {
        UseImmersiveDarkMode = 20,
        SystembackdropType = 38,
        MicaEffect = 1029,
    }

    public enum SystembackdropType
    {
        Auto = 0,
        None = 1,
        Mica = 2,
        Acrylic = 3, // Automatically selects the best Acrylic effect available on the system (Acrylic11 > Acrylic10)
        Tabbed = 4,

        Acrylic10, // Windows 10 style, supported on Windows 10 and 11
        Acrylic11, // Windows 11 style, supported on Windows 11 22523+ (Insider) and 22621+ (Stable)
    }

    [DllImport("DwmApi.dll")]
    public static extern int DwmExtendFrameIntoClientArea(nint hwnd, ref Margins pMarInset);

    [DllImport("dwmapi.dll")]
    public static extern int DwmSetWindowAttribute(nint hwnd, uint dwAttribute, ref int pvAttribute, int cbAttribute);
}
