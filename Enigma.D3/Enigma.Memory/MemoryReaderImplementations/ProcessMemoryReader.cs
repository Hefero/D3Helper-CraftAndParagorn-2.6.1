﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace Enigma.Memory
{
    public class ProcessMemoryReader : MemoryReader, IHasMainModuleVersion
    {
        private readonly Process _process;
        private readonly MemoryAddress _minValidAddress;
        private readonly MemoryAddress _maxValidAddress;
        private readonly int _pointerSize;

        public ProcessMemoryReader(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");
            if (process.HasExited)
                throw new ArgumentException("process");

            _process = process;
            _minValidAddress = 0x00010000;
            if (process.Is64BitProcess())
            {
                _maxValidAddress = process.IsLargeAddressAware() ?
                    ((8UL << 40) - 1) : // 8TB - 1 byte
                    ((2UL << 30) - 1);  // 2GB - 1 byte
                _pointerSize = 8;
            }
            else
            {
                _maxValidAddress = process.IsLargeAddressAware() ? 0xBFFF0000 : 0x7FFEFFFF;
                _pointerSize = 4;
            }
        }

        public override MemoryAddress MinValidAddress { get { return _minValidAddress; } }

        public override MemoryAddress MaxValidAddress { get { return _maxValidAddress; } }

        public override bool IsValid { get { return !_process.HasExited; } }

        public override int PointerSize { get { return _pointerSize; } }

        public Version MainModuleVersion { get { return _process.GetFileVersion(); } }

        public Process Process { get { return _process; } }

        protected unsafe override void UnsafeReadBytesCore(MemoryAddress address, byte[] buffer, int offset, int count)
        {
            fixed (byte* pBuffer = &buffer[offset])
            {
                if (!Win32.ReadProcessMemory(_process.Handle, address, pBuffer, count, IntPtr.Zero))
                    throw new Win32Exception();
            }
        }

        public void CreateMiniDump(string path)
        {
            CreateMiniDump(path, false);
        }

        public void CreateMiniDump(string path, bool overwrite)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");
            if (!IsValid)
                throw new InvalidOperationException();

            var mode = overwrite ? FileMode.Create : FileMode.CreateNew;
            using (var file = new FileStream(path, mode, FileAccess.Write, FileShare.None, 64 * 1024, FileOptions.SequentialScan))
            {
                const int MiniDumpWithFullMemory = 0x00000002;
                if (!Win32.MiniDumpWriteDump(_process.Handle, _process.Id, file.SafeFileHandle, MiniDumpWithFullMemory, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero))
                    throw new Win32Exception();
            }
        }

        public override void Dispose()
        {
            _process.Dispose();
        }

        private static class Win32
        {
            private const string Kernel32 = "kernel32.dll";
            private const string DbgHelp = "DbgHelp.dll";

            [DllImport(Kernel32, SetLastError = true)]
            public unsafe static extern bool ReadProcessMemory(
                IntPtr processHandle,
                IntPtr baseAddress,
                void* buffer,
                int size,
                IntPtr pNumberOfBytesRead);

            [DllImport(DbgHelp, SetLastError = true)]
            public static extern bool MiniDumpWriteDump(
                IntPtr processHandle,
                int processId,
                SafeFileHandle fileHandle,
                int dumpType,
                IntPtr exceptionParam,
                IntPtr userStreamParam,
                IntPtr callbackParam);
        }
    }
}
