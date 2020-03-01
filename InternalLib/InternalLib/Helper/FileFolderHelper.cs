/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

//3rd party
using NLog;

namespace InternalLib
{
    internal static class Win32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetDiskFreeSpaceEx(string drive, out long freeBytesForUser, out long totalBytes, out long freeBytes);
    }

    static public class FileFolderHelper
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// this method will return all the files in the patch
        /// </summary>
        /// <param name="path">where to look for the files</param>
        /// <returns>list of file names</returns>
        static public List<string> GetFilesInfolder(string path)
        {
            List<string> result = new List<string>();
            //ensure the path is a directory
            if (Directory.Exists(path))
            {
                string[] tmp = Directory.GetFiles(path);
                result.AddRange(tmp);
            }
            return result;
        }

        /// <summary>
        /// this method will return all the files in the patch, ordered by creation date
        /// </summary>
        /// <param name="path">where to look for the files</param>
        /// <returns>list of file names</returns>
        static public List<string> GetFilesInfolderByDate(string path)
        {
            List<string> result = new List<string>();
            DirectoryInfo di = new DirectoryInfo(path);
            FileSystemInfo[] files = di.GetFileSystemInfos();
            var orderedFiles = files.OrderBy(f => f.CreationTime);
            var t = from a in orderedFiles
                    where a.Attributes != FileAttributes.Directory
                    select a.FullName;
            result = t.ToList();
            return result;
        }

        /// <summary>
        /// this method will return all the directories in the patch
        /// </summary>
        /// <param name="path">where to look for the directories</param>
        /// <returns>list of directory names</returns>
        static public List<string> GetDirectoriesInfolder(string path)
        {
            List<string> result = new List<string>();
            //ensure the path is a directory
            if (Directory.Exists(path))
            {
                string[] tmp = Directory.GetDirectories(path);
                result.AddRange(tmp);
            }
            return result;
        }

        /// <summary>
        /// This method will look to see if the directory exists and if forced,
        /// will create the directory if the directory is not there
        /// </summary>
        /// <param name="directoryName">name of the directory to look up</param>
        /// <param name="force">if true, will create the folder if it does not exists</param>
        /// <returns></returns>
        static public bool DirectoryExists(string directoryName, bool force)
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            return true;
        }

        /// <summary>
        /// this method will force a file copy.  if the file already exists in the
        /// destination location, it will be over written
        /// </summary>
        /// <param name="oldName">Source file</param>
        /// <param name="newname">destination file</param>
        /// <returns>true if it was able to move it, else false</returns>
        static public bool ForceFileCopy(string oldName, string newname)
        {
            bool result = true;
            try
            {
                //copy the file with the new name
                if (File.Exists(newname))
                {
                    //delete the old one
                    File.Delete(newname);
                }
                File.Copy(oldName, newname);
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// this method will force a file move.  if the file already exists in the
        /// destination location, it will be over written
        /// </summary>
        /// <param name="oldName">Source file</param>
        /// <param name="newname">destination file</param>
        /// <returns>true if it was able to move it, else false</returns>
        static public bool ForceFileMove(string oldName, string newname)
        {
            bool result = true;
            try
            {
                //copy the file with the new name
                if (File.Exists(newname))
                {
                    //delete the old one
                    File.Delete(newname);
                }
                File.Move(oldName, newname);
            }
            catch (Exception ex)
            {
                logger.Error("Unable to move file: " + oldName + " because : " + ex);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// this method will rename a file with a GUID
        /// </summary>
        /// <param name="oldName">source file</param>
        /// <returns>new file name</returns>
        static public string RenameFileWithGuid(string oldName)
        {
            string result = "";
            string dir = Path.GetDirectoryName(oldName);
            if (dir == null)
            {
                dir = string.Empty;
            }
            result = Path.Combine(dir, Guid.NewGuid() + Path.GetExtension(oldName));
            try
            {
                ForceFileMove(oldName, result);
            }
            catch
            {
                result = "";
            }
            return result;
        }

        static public void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                //create it
                DirectoryInfo di = new DirectoryInfo(directory);
                di.Create();
                logger.Debug("Created directory : " + directory);
            }
        }

        public static void CreateCleanTempFolder(string newDir)
        {
            if (Directory.Exists(newDir))
            {
                FileFolderHelper.CleanFolder(newDir);
            }
            else
            {
                FileFolderHelper.CreateDirectory(newDir);
            }
        }

        /// <summary>
        /// this method will transverse a folder and delete all files and sub folders
        /// </summary>
        /// <param name="directory">root folder</param>
        /// <returns>true if successful, else false</returns>
        static public bool CleanFolder(string directory)
        {
            bool result = true;
            try
            {
                if (Directory.Exists(directory))
                {
                    //clean it
                    DirectoryInfo downloadedMessageInfo = new DirectoryInfo(directory);

                    foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        static public bool CleanFolderAndRemove(string directory)
        {
            bool result = true;
            try
            {
                if (Directory.Exists(directory))
                {
                    //clean it
                    DirectoryInfo downloadedMessageInfo = new DirectoryInfo(directory);

                    foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    Directory.Delete(directory);
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// this method will return the folder last written to
        /// </summary>
        /// <param name="path">root folder</param>
        /// <returns>name of the last folder written to</returns>
        public static string GetLatestFolder(string path)
        {
            string result = "";
            DateTime lastHigh = new DateTime(1900, 1, 1);
            foreach (string subdir in Directory.GetDirectories(path))
            {
                DirectoryInfo fi1 = new DirectoryInfo(subdir);
                DateTime created = fi1.LastWriteTime;

                if (created > lastHigh)
                {
                    result = subdir;
                    lastHigh = created;
                }
            }
            return result;
        }

        public static long GetAvailableDiskSpace(string path)
        {
            long result = 0;

            if (Win32.GetDiskFreeSpaceEx(Path.GetPathRoot(path), out long freeBytesForUser, out long totalBytes, out long freeBytes))
            {
                    result = freeBytes;
            }
            return result;
        }

        //Will determine the mime type of a file
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static UInt32 FindMimeFromData(UInt32 pBC,
                    [MarshalAs(UnmanagedType.LPStr)] String pwzUrl,
                    [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
                    UInt32 cbSize,
                    [MarshalAs(UnmanagedType.LPStr)] String pwzMimeProposed,
                    UInt32 dwMimeFlags,
                    out UInt32 ppwzMimeOut,
                    UInt32 dwReserverd
        );

        /// <summary>
        /// will determine the mime time of a source file
        /// </summary>
        /// <param name="filename">source file</param>
        /// <returns>the mime type</returns>
        static public string GetMimeFromFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(filename + " not found");
            byte[] buffer = new byte[256];
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                if (fs.Length >= 256)
                    fs.Read(buffer, 0, 256);
                else
                    fs.Read(buffer, 0, (int)fs.Length);
            }

            try
            {
                FindMimeFromData(0, null, buffer, 256, null, 0, out UInt32 mimetype, 0);
                IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch
            {
                return "unknown/unknown";
            }
        }

        static public void EncryptFile(string inputFile, string outputFile)
        {
            const string skey = "B@tM@n9898";
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = Encoding.UTF8.GetBytes(skey);

                    byte[] iv = Encoding.UTF8.GetBytes(skey);

                    using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform encryptor = aes.CreateEncryptor(key, iv))
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                                {
                                    int data;
                                    while ((data = fsIn.ReadByte()) != -1)
                                    {
                                        cs.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error("Encryption failed - " + ex);
            }
        }

        static public void DecryptFile(string inputFile, string outputFile)
        {
            const string password = @"B@tM@n9898";

            UnicodeEncoding ue = new UnicodeEncoding();
            byte[] key = ue.GetBytes(password);

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            RijndaelManaged rmCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                rmCrypto.CreateDecryptor(key, key),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();
        }
    }
}