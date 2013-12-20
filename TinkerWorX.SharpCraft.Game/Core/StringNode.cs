﻿using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinkerWorX.SharpCraft.Game.Core
{
    [StructLayout(LayoutKind.Sequential, Size = 0x18)]
    unsafe public struct StringNode
    {
        public IntPtr field0000;
        public IntPtr field0004;
        public IntPtr field0008;
        public IntPtr field0010;
        public IntPtr field0014;

        public IntPtr ValuePtr;
        public String Value { get { return Marshal.PtrToStringAnsi(this.ValuePtr); } }
    }
}
