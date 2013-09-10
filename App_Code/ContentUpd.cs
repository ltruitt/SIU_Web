using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.Web.Administration;


public class ContentUpd
{



    public static string FindVirtualDirectory(string DirName)
    {
        System.IO.DirectoryInfo RootDirInfo = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(@"~" + DirName));
        if (RootDirInfo.Exists == true)
            return RootDirInfo.FullName;
        return "";

        throw( new Exception("Failed To Find Virtual Directory " + DirName) );
    }

    private List<SIU_File> AllFiles;


    /////////////////////////////////////////////////////////////////////
    // Search Out And Build A Complete Unorder List Of Sub Directories //
    /////////////////////////////////////////////////////////////////////
    public void ContentScan(string VirtPath, string PhyDirPath="", bool SkipRecursion = false)
    {
        string RootPhyPath = FindVirtualDirectory(VirtPath);
        string PhyPath =  ((PhyDirPath.Length > 0) ? PhyDirPath : RootPhyPath);

        if (Environment.MachineName.ToLower() == "tsdc-dev")
            return;
        return;

        AllFiles = SqlServer_Impl.GetFiles().ToList();
        AddNewFiles(PhyPath, @"//" + System.Environment.MachineName + VirtPath, RootPhyPath, SkipRecursion);
        RemoveDeletedFiles();
    }

    private void RemoveDeletedFiles()
    {
        if (Environment.MachineName.ToLower() == "tsdc-dev")
            return;
        return;

        foreach( SIU_File FileChk in AllFiles )
        {
            if ( ! File.Exists(FileChk.PhyDir + "/" + FileChk.FileName) )
                SqlServer_Impl.ContentRemoveFile(FileChk);
        }
    }


    private void AddNewFiles(string PhyPath, string LogRoot, string RootPhyPath, bool SkipRecursion)
    {
        if (Environment.MachineName.ToLower() == "tsdc-dev")
            return;
        return;

        System.IO.DirectoryInfo RootDirInfo = new System.IO.DirectoryInfo(PhyPath);

        try
        {
            /////////////////////////////////////////////////////////////////////////
            // Walk Through Each File In Directory And Either Add Or Update Record //
            /////////////////////////////////////////////////////////////////////////
            foreach (FileInfo DiskFile in RootDirInfo.GetFiles())
            {

                SIU_File ThisFile = new SIU_File();

                ThisFile.PhyDir = RootDirInfo.FullName;
                ThisFile.FileName = DiskFile.Name;

                string[] FileParts = DiskFile.Name.Split('.');
                ThisFile.FileType = FileParts[FileParts.Length - 1];

                ThisFile.LastTouch = DiskFile.LastWriteTime.ToFileTime();


                string VirtDirPath = "";
                if (RootPhyPath != RootDirInfo.FullName)
                    VirtDirPath = RootDirInfo.FullName.Substring(RootPhyPath.Length).Replace(@"\", @"/");
                ThisFile.VirDir = LogRoot + "/" + VirtDirPath;

                if (ThisFile.FileName == "Appendix B")
                    System.Diagnostics.Debugger.Break();

                if (ThisFile.FileType.Length < 5 && !ThisFile.FileName.EndsWith(".db"))
                {
                    var ExistingFile = (from isItThere in AllFiles
                                        where isItThere.FileName == ThisFile.FileName && isItThere.PhyDir == ThisFile.PhyDir
                                        select isItThere).FirstOrDefault();

                    if (ExistingFile == null)
                    {
                        ThisFile.RefID = System.Guid.NewGuid();
                        SqlServer_Impl.ContentRecordFile(ThisFile);
                    }
                    else
                    {
                        if (ExistingFile.FileType != ThisFile.FileType ||
                            ExistingFile.LastTouch != ThisFile.LastTouch ||
                            ExistingFile.PhyDir != ThisFile.PhyDir ||
                            ExistingFile.VirDir != ThisFile.VirDir)
                            SqlServer_Impl.ContentUpdateFile(ThisFile);
                    }
                }
            }



            /////////////////////////////
            // Scan Each Sub Directory //
            /////////////////////////////
            if (!SkipRecursion)
            {
                foreach (DirectoryInfo Dir in RootDirInfo.GetDirectories())
                {
                    System.Diagnostics.Debug.WriteLine(Dir.FullName);
                    AddNewFiles(Dir.FullName, LogRoot, RootPhyPath, false);

                }
            }

        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

}