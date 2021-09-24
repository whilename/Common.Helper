using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Common.Library.Utility
{
    /// <summary>Windows Credentials Management</summary>
    [SuppressUnmanagedCodeSecurity]
    public class CredentialManagement : IDisposable
    {
        /// <summary>Credential StructLayout</summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct CREDENTIAL
        {
            public int Flags;
            public int Type;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string TargetName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Comment;
            public long LastWritten;
            public int CredentialBlobSize;
            public IntPtr CredentialBlob;
            public int Persist;
            public int AttributeCount;
            public IntPtr Attributes;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string TargetAlias;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string UserName;
        }

        /// <summary>Read credential information.</summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <param name="reservedFlag"></param>
        /// <param name="CredentialPtr"></param>
        /// <returns></returns>
        [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CredRead(string target, CredentialType type, int reservedFlag, out IntPtr CredentialPtr);
        /// <summary>Add credential information</summary>
        /// <param name="userCredential"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("Advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CredWrite([In] ref CREDENTIAL userCredential, [In] UInt32 flags);

        /// <summary>Clear the handle cache.</summary>
        /// <param name="cred"></param>
        /// <returns></returns>
        [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
        internal static extern bool CredFree([In] IntPtr cred);

        /// <summary>Delete the specified reason.</summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode)]
        internal static extern bool CredDelete(string target, CredentialType type, int flags);

        /// <summary>A traversal collection enumerator that reads multiple credentials.</summary>
        /// <param name="filter"></param>
        /// <param name="flag"></param>
        /// <param name="count"></param>
        /// <param name="pCredentials"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool CredEnumerateW(string filter, int flag, out uint count, out IntPtr pCredentials);

        bool _disposed;

        /// <summary>disposed of this object.</summary>
        public void Dispose()
        {
            Dispose(true);
            // Prevent GC Collection since we have already disposed of this object
            GC.SuppressFinalize(this);
        }
        ~CredentialManagement()
        {
            Dispose(false);
        }

        /// <summary>disposed of this object.</summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    SecurePassword.Clear();
                    SecurePassword.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>Initialize a credential management object</summary>
        public CredentialManagement()
        {
        }

        /// <summary>Initialize the credential management object with the specified parameters.</summary>
        /// <param name="username"></param>
        public CredentialManagement(string username) : this(username, null)
        {
        }

        /// <summary>Initialize the credential management object with the specified parameters.</summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public CredentialManagement(string username, string password) : this(username, password, null)
        {
        }

        /// <summary>Initialize the credential management object with the specified parameters.</summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="target"></param>
        public CredentialManagement(string username, string password, string target)
            : this(username, password, target, CredentialType.Generic)
        {
        }

        /// <summary>Initialize the credential management object with the specified parameters.</summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public CredentialManagement(string username, string password, string target, CredentialType type)
        {
            Username = username;
            Password = password;
            Target = target;
            Type = type;
            PersistanceType = PersistanceType.Session;
            _lastWriteTime = DateTime.MinValue;
        }

        /// <summary>Check whether the current object has been disposed</summary>
        private void CheckNotDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Credential object is already disposed.");
            }
        }

        private string _username;
        /// <summary>Credentials User name.</summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>Credentials password.</summary>
        public string Password
        {
            get
            {
                string str;
                IntPtr zero = IntPtr.Zero;
                if ((SecurePassword == null) || (SecurePassword.Length == 0))
                {
                    return string.Empty;
                }
                try
                {
                    zero = Marshal.SecureStringToBSTR(SecurePassword);
                    str = Marshal.PtrToStringBSTR(zero);
                }
                finally
                {
                    if (zero != IntPtr.Zero)
                    {
                        Marshal.ZeroFreeBSTR(zero);
                    }
                }
                return str;
            }
            set
            {
                SecurePassword = CreateSecureString(string.IsNullOrEmpty(value) ? string.Empty : value);
            }
        }

        private SecureString _password;
        /// <summary>Secure password.</summary>
        public SecureString SecurePassword
        {
            get { return null == _password ? new SecureString() : _password.Copy(); }
            set
            {
                if (null != _password)
                {
                    _password.Clear();
                    _password.Dispose();
                }
                _password = null == value ? new SecureString() : value.Copy();
            }
        }

        private string _target;
        /// <summary>internet or network address.</summary>
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        private string _description;
        /// <summary>Description</summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>Last Modified</summary>
        public DateTime LastWriteTime
        {
            get { return LastWriteTimeUtc.ToLocalTime(); }
        }

        private DateTime _lastWriteTime;
        /// <summary>Last Modified Utc</summary>
        public DateTime LastWriteTimeUtc
        {
            get { return _lastWriteTime; }
            private set { _lastWriteTime = value; }
        }

        private CredentialType _type;
        /// <summary>Credential type</summary>
        public CredentialType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private PersistanceType _persistanceType;
        /// <summary>Persistence type</summary>
        public PersistanceType PersistanceType
        {
            get { return _persistanceType; }
            set { _persistanceType = value; }
        }

        /// <summary>Convert the string to a secure string.</summary>
        /// <param name="plainString"></param>
        /// <returns></returns>
        internal static unsafe SecureString CreateSecureString(string plainString)
        {
            SecureString str;
            if (string.IsNullOrEmpty(plainString))
            {
                return new SecureString();
            }
            fixed (char* str2 = plainString)
            {
                char* chPtr = str2;
                str = new SecureString(chPtr, plainString.Length);
                str.MakeReadOnly();
            }
            return str;
        }

        /// <summary>Save current credential information.</summary>
        /// <returns></returns>
        public bool Save()
        {
            CheckNotDisposed();
            //NOTE: Convert the password to bytecode, verify the length of the converted bytecode, the length cannot exceed 512 bytes.
            byte[] passwordBytes = Encoding.Unicode.GetBytes(Password);
            if (Password.Length > (512))
            {
                throw new ArgumentOutOfRangeException("The password has exceeded 512 bytes.");
            }

            CREDENTIAL credential = new CREDENTIAL();
            credential.TargetName = Target;
            credential.UserName = Username;
            credential.CredentialBlob = Marshal.StringToCoTaskMemUni(Password);
            credential.CredentialBlobSize = passwordBytes.Length;
            credential.Comment = Description;
            credential.Type = (int)Type;
            credential.Persist = (int)PersistanceType;

            bool result = CredWrite(ref credential, 0);
            if (!result)
            {
                return false;
            }
            LastWriteTimeUtc = DateTime.UtcNow;
            return true;
        }

        /// <summary>Delete current credential information.</summary>
        /// <returns></returns>
        public bool Delete()
        {
            CheckNotDisposed();
            if (string.IsNullOrEmpty(Target))
            {
                throw new InvalidOperationException("Target must be specified to delete a credential.");
            }

            bool result = CredDelete(Target, Type, 0);
            return result;
        }

        /// <summary>Load the credential information according to the provided Target.</summary>
        /// <returns></returns>
        public bool Load()
        {
            CheckNotDisposed();

            IntPtr credPointer;
            bool result = CredRead(Target, Type, 0, out credPointer);
            if (!result)
            {
                return false;
            }
            // Process the obtained credential handle.
            using (CriticalHandle credentialHandle = new CriticalHandle(credPointer))
            {
                LoadInternal(credentialHandle.GetCredential());
            }
            return true;
        }

        /// <summary>Deserialize the credential structure as the current object.</summary>
        /// <param name="credential"></param>
        internal void LoadInternal(CREDENTIAL credential)
        {
            Username = credential.UserName;
            if (credential.CredentialBlobSize > 0)
            {
                Password = Marshal.PtrToStringUni(credential.CredentialBlob, credential.CredentialBlobSize / 2);
            }
            Target = credential.TargetName;
            Type = (CredentialType)credential.Type;
            PersistanceType = (PersistanceType)credential.Persist;
            Description = credential.Comment;
            LastWriteTimeUtc = DateTime.FromFileTimeUtc(credential.LastWritten);
        }

        /// <summary>List of existing credentials.</summary>
        public IList<CredentialManagement> ExistingCredentials { get; private set; }

        /// <summary>
        /// Load all the credentials of the current machine to the existing credential list, 
        /// if the Target is not empty, it will be filtered according to the Target value.
        /// </summary>
        public bool LoadAll()
        {
            uint count;
            IntPtr pCredentials = IntPtr.Zero;
            bool result = CredEnumerateW(Target, 0, out count, out pCredentials);
            if (!result)
            {
                Trace.WriteLine(string.Format("Win32Exception: {0}", new Win32Exception(Marshal.GetLastWin32Error()).ToString()));
                return false;
            }

            // Read in all of the pointers first
            IntPtr[] ptrCredList = new IntPtr[count];
            for (int i = 0; i < count; i++)
            {
                ptrCredList[i] = Marshal.ReadIntPtr(pCredentials, IntPtr.Size * i);
            }

            // Now let's go through all of the pointers in the list and create our Credential object(s)
            List<CriticalHandle> credentialHandles =
                ptrCredList.Select(ptrCred => new CriticalHandle(ptrCred)).ToList();

            ExistingCredentials = credentialHandles
                .Select(handle => handle.GetCredential())
                .Select(nativeCredential =>
                {
                    CredentialManagement credential = new CredentialManagement();
                    credential.LoadInternal(nativeCredential);
                    return credential;
                }).ToList();

            // The individual credentials should not be free'd
            credentialHandles.ForEach(handle => handle.SetHandleAsInvalid());
            // Clean up memory to the Enumeration pointer
            CredFree(pCredentials);
            return true;
        }

        /// <summary>
        /// Provides a base class for Win32 critical handle implementations in which the value of either 0 or -1 indicates an invalid handle.
        /// </summary>
        internal sealed class CriticalHandle : CriticalHandleZeroOrMinusOneIsInvalid
        {
            // Set the handle.
            internal CriticalHandle(IntPtr preexistingHandle)
            {
                SetHandle(preexistingHandle);
            }

            // Get the Credential from the mem location
            internal CREDENTIAL GetCredential()
            {
                if (!IsInvalid)
                {
                    return (CREDENTIAL)Marshal.PtrToStructure(handle, typeof(CREDENTIAL));
                }
                else
                {
                    throw new InvalidOperationException("Invalid CriticalHandle!");
                }
            }

            /// <summary>
            /// Perform any specific actions to release the handle in the ReleaseHandle method.
            /// Often, you need to use Pinvoke to make a call into the Win32 API to release the handle. 
            /// In this case, however, we can use the Marshal class to release the unmanaged memory.
            /// </summary>
            /// <returns></returns>
            override protected bool ReleaseHandle()
            {
                // If the handle was set, free it. Return success.
                if (!IsInvalid)
                {
                    // NOTE: We should also ZERO out the memory allocated to the handle, before free'ing it
                    // so there are no traces of the sensitive data left in memory.
                    CredFree(handle);
                    // Mark the handle as invalid for future users.
                    SetHandleAsInvalid();
                    return true;
                }
                // Return false. 
                return false;
            }
        }
    }

    /// <summary>Credential type enumeration.</summary>
    public enum CredentialType : uint
    {
        None = 0,
        Generic = 1,
        DomainPassword = 2,
        DomainCertificate = 3,
        DomainVisiblePassword = 4
    }

    /// <summary>Persistence type enumeration</summary>
    public enum PersistanceType : uint
    {
        Session = 1,
        LocalComputer = 2,
        Enterprise = 3
    }

}
