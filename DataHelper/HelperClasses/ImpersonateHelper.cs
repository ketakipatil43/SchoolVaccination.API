using DataHelper.Entities;
using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace DataHelper.HelperClasses
{
    public class ImpersonateHelper
    {
        private readonly string _impersonateUser;
        private readonly string _impersonatePassword;

        /// <summary>
        /// Class custructor
        /// </summary>
        /// <param name="impersonateUser">Impersonate user name</param>
        /// <param name="impersonatePassword">Impersonate user password</param>
        public ImpersonateHelper(string impersonateUser, string impersonatePassword)
        {
            _impersonateUser = impersonateUser;
            _impersonatePassword = impersonatePassword;
        }

        /// <summary>
        /// This function is helping to check directory exists, directory create, file exists, file create, file copy, file delete etc.
        /// </summary>
        /// <param name="actionType">Action type is define the what action will perform</param>
        /// <param name="sourcePath">Directory/File source path</param>
        /// <param name="destinationPath">Directory/File destination path</param>
        /// <param name="fileUpload">File object</param>
        /// <returns></returns>
        public bool ImpersonateUser(DirectoryOrFileActionType actionType, IFormFile fileUpload, string sourcePath, string destinationPath)
        {
            bool result = false;
            if (actionType == DirectoryOrFileActionType.None)
                return false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using WindowsLogin wi = new(_impersonateUser, "", _impersonatePassword);
#if NET461
                    using (user.Impersonate())
#else

                WindowsIdentity.RunImpersonated(wi.Identity.AccessToken, () =>
#endif
                {
                    switch (actionType)
                    {
                        case DirectoryOrFileActionType.DirectoryExists:
                            result = Directory.Exists(sourcePath);
                            break;
                        case DirectoryOrFileActionType.DirectoryCreate:
                            if (!Directory.Exists(sourcePath))
                            {
                                Directory.CreateDirectory(sourcePath);
                                result = true;
                                break;
                            }
                            result = false;
                            break;

                        case DirectoryOrFileActionType.FileExists:
                            result = File.Exists(sourcePath);
                            break;

                        case DirectoryOrFileActionType.FileCreate:
                            if (fileUpload != null)
                            {
                                using var stream = new FileStream(sourcePath, FileMode.CreateNew, FileAccess.Write);
                                fileUpload.CopyTo(stream);
                                result = true;
                                break;
                            }
                            result = false;
                            break;

                        case DirectoryOrFileActionType.FileCopy:
                            if (File.Exists(sourcePath))
                            {
                                File.Copy(sourcePath, destinationPath);
                                result = true;
                                break;
                            }
                            result = false;
                            break;

                        case DirectoryOrFileActionType.FileDelete:
                            if (File.Exists(sourcePath))
                            {
                                File.Delete(sourcePath);
                                result = true;
                                break;
                            }
                            result = false;
                            break;

                        case DirectoryOrFileActionType.FileStreamWrite:
                            try
                            {
                                using StreamWriter writer = new(sourcePath + @"\Lets-Talk\Dialouge_Attachments.txt", true);
                                writer.WriteLine(destinationPath);
                                result = true;
                                break;
                            }
                            catch
                            {
                                result = false;
                                break;
                            }

                        default:
                            result = false;
                            break;
                    }
                }
#if !NET461
               );
#endif
            }
            return result;
        }
    }
}
