using System.Security.Principal;

namespace DataHelper.HelperClasses
{
    public class WindowsLogin : System.IDisposable
    {
        protected const int LOGON32_PROVIDER_DEFAULT = 0;
        protected const int LOGON32_LOGON_INTERACTIVE = 2;

        public WindowsIdentity Identity = null;
        private System.IntPtr m_accessToken;

        /// <summary>
        /// Import the advance API32 dll 
        /// </summary>
        /// <param name="lpszUsername">impersonate user name</param>
        /// <param name="lpszDomain">impersonate user domain</param>
        /// <param name="lpszPassword">impersonate user password</param>
        /// <param name="dwLogonType">login type</param>
        /// <param name="dwLogonProvider">login provider</param>
        /// <param name="phToken">token</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain,
        string lpszPassword, int dwLogonType, int dwLogonProvider, ref System.IntPtr phToken);

        /// <summary>
        /// kernel32 dll import
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private extern static bool CloseHandle(System.IntPtr handle);

        /// <summary>
        /// Login into advance api32 dll for impersonate user
        /// </summary>
        public WindowsLogin()
        {
            this.Identity = WindowsIdentity.GetCurrent();
        }

        /// <summary>
        /// windows login from inpersonate
        /// </summary>
        /// <param name="username">impersonate user name</param>
        /// <param name="domain">impersonate user domain</param>
        /// <param name="password">impersonate user password</param>
        public WindowsLogin(string username, string domain, string password)
        {
            Login(username, domain, password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">impersonate user name</param>
        /// <param name="domain">impersonate user domain</param>
        /// <param name="password">impersonate user password</param>
        public void Login(string username, string domain, string password)
        {
            if (this.Identity != null)
            {
                this.Identity.Dispose();
                this.Identity = null;
            }

            try
            {
                this.m_accessToken = new System.IntPtr(0);
                Logout();

                this.m_accessToken = System.IntPtr.Zero;
                bool logonSuccessfull = LogonUser(
                   username,
                   domain,
                   password,
                   LOGON32_LOGON_INTERACTIVE,
                   LOGON32_PROVIDER_DEFAULT,
                   ref this.m_accessToken);

                if (!logonSuccessfull)
                {
                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(error);
                }
                Identity = new WindowsIdentity(this.m_accessToken);
            }
            catch
            {
                throw;
            }

        } // End Sub Login 

        /// <summary>
        /// Log out from windows
        /// </summary>
        public void Logout()
        {
            if (this.m_accessToken != System.IntPtr.Zero)
                CloseHandle(m_accessToken);

            this.m_accessToken = System.IntPtr.Zero;

            if (this.Identity != null)
            {
                this.Identity.Dispose();
                this.Identity = null;
            }

        } // End Sub Logout 

        /// <summary>
        /// Log out from windows
        /// </summary>
        void System.IDisposable.Dispose()
        {
            Logout();
        } // End Sub Dispose 
    }
}
