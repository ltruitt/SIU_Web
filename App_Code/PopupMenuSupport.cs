using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using ShermcoYou.DataTypes;

/// <summary>
/// Summary description for PopupMenuSupport
/// </summary>
public static class PopupMenuSupport
{

    //private static readonly string RootVideoPath = HttpContext.Current.Server.MapPath("/Videos");
    private static readonly string RootFilesPath = HttpContext.Current.Server.MapPath("/Files");

#region Web Methods
    ///////////////////////////////////////////////
    // List Directories                          //
    // Called To Build Menu Based On Directories //
    ///////////////////////////////////////////////
    public static List<DirectoryList> ListDirectories(string Root, string Path, string Icon, string HoverIcon)
    {
        const string method = "PopupMenu.ListDirectories";
        List<DirectoryList> dirList = new List<DirectoryList>();
        List<string> addedDirectoryNames = new List<string>();

        DirectoryInfo virtRootDirInfo = new DirectoryInfo(Root);
        DirectoryInfo phyRootDirInfo = new DirectoryInfo(virtRootDirInfo.FullName + @"\" + Path);

        if (phyRootDirInfo.Exists == false)
        {
            SqlServer_Impl.LogDebug(method, Root + "/" + Path + " Is Missing");
            return null;                
        }

        //////////////////////////////////////////////////////
        // Create A Folder Icon For Each Physical Directory //
        //////////////////////////////////////////////////////
        foreach (DirectoryInfo dir in phyRootDirInfo.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore")))
        {
            //////////////////////////////
            // Ignore Enpty Directories //
            //////////////////////////////
            if (dir.GetFiles().Length > 0 || dir.GetDirectories().Length > 0)
            {
                string subPath = dir.FullName.Replace(dir.Name, "").Replace(virtRootDirInfo.FullName, "").Replace(@"\", @"/").TrimStart('/');
                DirectoryList thisDirList = new DirectoryList { LongName = dir.FullName, ShortName = dir.Name, SubPath = subPath, DisplayName = dir.Name };
                dirList.Add((thisDirList));
                addedDirectoryNames.Add(dir.Name);
            }
        }



        //////////////////////////////////////////////
        // Create A Folder Icon For Directory Links //
        //////////////////////////////////////////////
        foreach (FileInfo diskFile in phyRootDirInfo.GetFiles())
        {
            if (diskFile.Extension == ".lnk")
            {
                string shortName = diskFile.Name.Replace(diskFile.Extension, "");
                /////////////////////////////////////////////////////////////////////
                // Only If We Have Not Already Created A Folder With The Same Name //
                /////////////////////////////////////////////////////////////////////
                if (!addedDirectoryNames.Contains(diskFile.Name))
                {
                    /////////////////////////////////////////////////////
                    // Write Out A List Item Element For The Directory //
                    /////////////////////////////////////////////////////
                    string targetName = GetShortcutTarget(diskFile.FullName);
                    if (Directory.Exists(targetName))
                    {
                        DirectoryInfo dir = new DirectoryInfo(targetName);

                        //////////////////////////////
                        // Ignore Enpty Directories //
                        //////////////////////////////
                        if (dir.GetFiles().Length > 0 || dir.GetDirectories().Length > 0)
                        {
                            string subPath = dir.FullName.Replace(dir.Name, "").Replace(virtRootDirInfo.FullName, "").Replace(@"\", @"/").TrimStart('/');

                            DirectoryList thisDirList;
                            if (File.Exists(targetName + "\\" + shortName))
                                thisDirList = new DirectoryList { LongName = dir.FullName, ShortName = shortName, SubPath = subPath, DisplayName = shortName };
                            else
                            {
                                thisDirList = new DirectoryList { LongName = dir.FullName, SubPath = subPath, ShortName = dir.Name, DisplayName = shortName };
                            }
                            dirList.Add((thisDirList));
                        }
                    }

                    else
                    {
                        SqlServer_Impl.LogDebug(method, diskFile.FullName + " link has invalid path " + targetName);
                    }
                }
            }
        }

        return dirList;

    }

    public static List<PhoneDirectoryList> PhoneDocumentList(string Root, string Path, string VirtualDirectory)
    {
        const string method = "PopupMenu.PhoneDocumentList";
        List<PhoneDirectoryList> dirList = new List<PhoneDirectoryList>();

        DirectoryInfo virtRootDirInfo = new DirectoryInfo(Root);
        DirectoryInfo phyRootDirInfo = new DirectoryInfo(virtRootDirInfo.FullName + @"\" + Path.Replace("%20", " "));

        if (phyRootDirInfo.Exists == false)
        {
            SqlServer_Impl.LogDebug(method, Root + "/" + Path + " Is Missing");
            return null;
        }

        /////////////////////////////////////////////
        // Add List Of Files In The Root Directory //
        /////////////////////////////////////////////
        PhoneFileList(phyRootDirInfo, VirtualDirectory, ref dirList);


        foreach (DirectoryInfo dir in phyRootDirInfo.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore"))  ) 
        {

            /////////////////////////////
            // Add Root Directory Info //
            /////////////////////////////
            //string SubPath = Dir.FullName.Replace(Dir.Name, "").Replace(VirtRootDirInfo.FullName, "").Replace(@"\", @"/").TrimStart('/');
            int numDb = dir.GetFiles("*.db", SearchOption.TopDirectoryOnly).Length;
            PhoneDirectoryList thisDirList = new PhoneDirectoryList { RootDirName = dir.Name, RootSubDirCnt = dir.GetDirectories().Length.ToString(CultureInfo.InvariantCulture), RootFileCnt = (dir.GetFiles().Length - numDb).ToString(CultureInfo.InvariantCulture) };
            dirList.Add((thisDirList));

            /////////////////////////////
            // Now Add Sub Directories //
            /////////////////////////////
            PhoneDirList(dir, virtRootDirInfo, ref dirList);

            /////////////////////////////////////////////
            // And Then List Of Files In The Directory //
            /////////////////////////////////////////////
            PhoneFileList(dir, VirtualDirectory, ref dirList);
        }

        /////////////////////////////////////////////////////
        // Dont Forget List Of Files In The Root Directory //
        /////////////////////////////////////////////////////
        //PhoneFileList(phyRootDirInfo, VirtualDirectory, ref dirList);

        return dirList;
    }

    private static void PhoneDirList(DirectoryInfo Dir, DirectoryInfo VirtRootDirInfo, ref List<PhoneDirectoryList> DirList)
    {
        foreach (DirectoryInfo subDir in Dir.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore")))
        {
            int numDb = subDir.GetFiles("*.db", SearchOption.TopDirectoryOnly).Length;

            PhoneDirectoryList thisSubDirList = new PhoneDirectoryList { RootDirName = Dir.Name, RootSubDirCnt = Dir.GetDirectories().Length.ToString(CultureInfo.InvariantCulture), RootFileCnt = (Dir.GetFiles().Length - numDb).ToString(CultureInfo.InvariantCulture) };
            string subPath = Dir.FullName.Replace(Dir.Name, "").Replace(VirtRootDirInfo.FullName, "").Replace(@"\", @"/").TrimStart('/');
            thisSubDirList.SubDirName = subDir.Name;
            thisSubDirList.SubDirDirCnt = subDir.GetDirectories().Length.ToString(CultureInfo.InvariantCulture);
            thisSubDirList.SubFileCnt = subDir.GetFiles().Length.ToString(CultureInfo.InvariantCulture);
            thisSubDirList.SubDirPath = subPath + Dir.Name + "/" + subDir.Name;
            DirList.Add((thisSubDirList));
        }        
    }


    /////////////////////////////////////////
    // Add List Of Files To FileSystemEnum //
    /////////////////////////////////////////
    private static void PhoneFileList(DirectoryInfo Dir, string VirtualDirectory, ref List<PhoneDirectoryList> DirList)
    {
        foreach (FileInfo subFile in Dir.GetFiles().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore")))
        {
            PhoneDirectoryList thisFileList = new PhoneDirectoryList
            {
                RootDirName = Dir.Name,
                RootSubDirCnt = Dir.GetDirectories().Length.ToString(CultureInfo.InvariantCulture),
                RootFileCnt = Dir.GetFiles().Length.ToString(CultureInfo.InvariantCulture),
                FileName = subFile.Name,
                FileType = subFile.Extension.Replace(".", ""),
                FilePath = toVirtual(subFile.FullName, VirtualDirectory)
            };

            if ( thisFileList.FileType == "url")
            {
                thisFileList.FilePath =  GetShortcutTarget(subFile.FullName);
                thisFileList.FileName = thisFileList.FileName.Replace(".url", "");
            }

            if (thisFileList.FileType != "db" && thisFileList.FileType != "dat")
                DirList.Add((thisFileList));
        }        
    }




    /////////////////////////////////////////
    // List Files                          //
    // Called To Build Menu Based On Files //
    /////////////////////////////////////////
    public static List<DirectoryList> ListFiles(string Root, string Path, string VirtualDirectory)
    {
        const string method = "PopupMenu.ListFiles";
        List<DirectoryList> dirList = new List<DirectoryList>();

        DirectoryInfo virtRootDirInfo = new DirectoryInfo(Root);
        DirectoryInfo phyRootDirInfo = new DirectoryInfo(virtRootDirInfo.FullName + @"\" + Path);


        if (phyRootDirInfo.Exists == false)
        {
            SqlServer_Impl.LogDebug(method, Root + "/" + Path + " Is Missing");
            return null;
        }

        //////////////////////////////////////////////
        // Create A Folder Icon For Directory Links //
        //////////////////////////////////////////////
        foreach (FileInfo subFiles in phyRootDirInfo.GetFiles())
        {
            if (subFiles.Name != "Folder.png" && subFiles.Extension != ".lnk")
            {
                string fullPath = Root + "/" + Path.Replace("\\", "/") + "/" + subFiles.Name;
                string displayName = subFiles.Name;
                if (displayName.Length > 5)
                    displayName = displayName.Remove(displayName.Length - 4);

                /////////////////////////////////
                // Add Specific File eferences //
                /////////////////////////////////
                if ( ! subFiles.Name.EndsWith(".db"))
                {
                    DirectoryList thisDirList = new DirectoryList
                    {
                                                        //HoverIcon = ImageIcon,
                                                        Icon = SetIcon(subFiles.Extension),
                                                        LongName = fullPath,
                                                        ShortName = displayName,
                                                        Href = toVirtual(fullPath, VirtualDirectory),
                                                        Ext = subFiles.Extension
                                                    };
                    dirList.Add((thisDirList));
                }
            }
        }

        return dirList;

    }

    //////////////////////////////////////////
    // List Files                           //
    // Called To Build Menu Based On Videos //
    //////////////////////////////////////////
    public static List<DirectoryList> ListVideos(string Root, string Path, string VirtualDirectory)
    {
        const string method = "PopupMenu.ListVideos";
        List<DirectoryList> dirList = new List<DirectoryList>();

        DirectoryInfo virtRootDirInfo = new DirectoryInfo(Root);
        DirectoryInfo phyRootDirInfo = new DirectoryInfo(virtRootDirInfo.FullName + @"\" + Path);


        if (phyRootDirInfo.Exists == false)
        {
            SqlServer_Impl.LogDebug(method, Root + "/" + Path + " Is Missing");
            return null;
        }

        //////////////////////////////////////////////
        // Create A Folder Icon For Directory Links //
        //////////////////////////////////////////////
        foreach (FileInfo subFiles in phyRootDirInfo.GetFiles())
        {
            if (subFiles.Name != "Folder.png")
            {
                string fullPath = Root + "/" + Path.Replace("\\", "/") + "/" + subFiles.Name;
                string displayName = subFiles.Name;
                if (displayName.Length > 5)
                    displayName = displayName.Remove(displayName.Length - subFiles.Extension.Length);

                /////////////////////////////////
                // Add Specific File references //
                /////////////////////////////////
                if ( IsVideo(subFiles.Extension) )
                {
                    DirectoryList thisDirList = new DirectoryList
                    {
                        Icon = SetIcon(subFiles.Extension),
                        LongName = fullPath,
                        ShortName = displayName,
                        Href = toVirtual(fullPath, VirtualDirectory),
                        Ext = subFiles.Extension
                    };
                    dirList.Add((thisDirList));
                }
            }
        }

        return dirList;

    }
#endregion

#region Document Section
    //////////////////////////////////////////////////////////////////////
    // Search Out And Build A Complete Unorder List Of Sub Directories  //
    // That Conform To Popup Menu JS Naming Expectations                //
    // NON INCLUSIVE OF VirtualDirectry                                 //
    // *** THESE ARE THE FOLDER THAT SHOW ON THE PAGE ***               //
    //////////////////////////////////////////////////////////////////////
    //public static string Create_Injectable_Directory_Folders(string VirtualDirectory, bool liOnly = false)
    //{
    //    const string method = "Create_Injectable_Directory_Folders";
    //    int menuItemNo = 1;
    //    List<string> addedDirectoryNames = new List<string>();

    //    DirectoryInfo virtRootDirInfo = new DirectoryInfo(RootFilesPath);
    //    DirectoryInfo phyRootDirInfo = new DirectoryInfo(virtRootDirInfo.FullName + @"\" + VirtualDirectory);


    //    /////////////////////////////////////////////////////////////////////////////
    //    // Look For Cached Markup Code   ((( Cached For An Hour Only In SQL )))    //
    //    /////////////////////////////////////////////////////////////////////////////
    //    //string menuList = SqlServer_Impl.GetCache("DocFolders", phyRootDirInfo.FullName, VirtualDirectory, ListOnly: liOnly);
    //    //if (menuList.Length > 0)
    //    //    return menuList;

    //    string menuList = "";
    //    //////////////////////////////////////
    //    // Verify Physical Directroy Exists //
    //    //////////////////////////////////////
    //    if (phyRootDirInfo.Exists == false)
    //    {
    //        SqlServer_Impl.LogDebug(method, VirtualDirectory + " Is Missing");
    //        return "";
    //    }

    //    ///////////////////////////////////////////////
    //    // For Every Object In Gallery, Make An Icon //
    //    ///////////////////////////////////////////////
    //    if (!liOnly)
    //    {
    //        menuList = "\n    <ul id=\"menulist\">\n";
    //    }

    //    /////////////////////////////////////////////
    //    // Create A Folder Icon For Each Directory //
    //    /////////////////////////////////////////////
    //    foreach (DirectoryInfo dir in phyRootDirInfo.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore"))    )
    //    {
    //        menuList += "        <li>\n";
    //        menuList += "            <a>\n";
    //        menuList += "                <img id=\"mbi_mb73g0_" + menuItemNo++ + "\" alt=\"" + dir.Name + "\" src=\"/Images/LibraryFolder.png\"/><br/>\n";
    //        menuList += "                <span>" + dir.Name + "</span>\n";
    //        menuList += "            </a>\n";
    //        menuList += "        </li>\n";

    //        /////////////////////////////////////////////////////////////////
    //        // Record that we added this name so we dont do it again later //
    //        // Add .lnk because thats what we will be expecting            //
    //        /////////////////////////////////////////////////////////////////
    //        addedDirectoryNames.Add(dir.Name + ".lnk");
    //    }



    //    //////////////////////////////////////////////
    //    // Create A Folder Icon For Directory Links //
    //    //////////////////////////////////////////////
    //    foreach (FileInfo diskFile in phyRootDirInfo.GetFiles())
    //    {
    //        if (diskFile.Extension == ".lnk")
    //        {
    //            /////////////////////////////////////////////////////////////////////
    //            // Only If We Have Not Already Created A Folder With The Same Name //
    //            /////////////////////////////////////////////////////////////////////
    //            if (!addedDirectoryNames.Contains(diskFile.Name))
    //            {
    //                /////////////////////////////////////////////////////
    //                // Write Out A List Item Element For The Directory //
    //                /////////////////////////////////////////////////////
    //                string targetName = GetShortcutTarget(diskFile.FullName);
    //                if (Directory.Exists(targetName))
    //                {
    //                    DirectoryInfo dir = new DirectoryInfo(targetName);
    //                    menuList += "        <li>\n";
    //                    menuList += "            <a>\n";
    //                    menuList += "                <img id=\"mbi_mb73g0_" + menuItemNo++ + "\" alt=\"" + dir.Name + "\" src=\"/Images/LibraryFolder.png\"/><br/>\n";
    //                    menuList += "                <span>" + diskFile.Name.Remove(diskFile.Name.Length - 4) + "</span>\n";
    //                    menuList += "            </a>\n";
    //                    menuList += "        </li>\n";
    //                }
    //                else
    //                {
    //                    SqlServer_Impl.LogDebug(method, diskFile.FullName + " link has invalid path " + targetName);
    //                }
    //            }
    //        }
    //    }

    //    if (!liOnly)
    //    {
    //        menuList += "    </ul>\n";
    //    }

    //    /////////////////////////
    //    // Record Markup Cache //
    //    /////////////////////////
    //    //SqlServer_Impl.RecordCache(phyRootDirInfo.FullName, menuList, "DocFolders", VirtualDirectory, ListOnly: liOnly);

    //    return menuList;
    //}

    ///////////////////////////////////////////////////////////////////////////////
    // Search Out And Build A Complete Unorder List Of Sub Directories and Files //
    // Recursively Searchs Sub Directories                                       //
    // NON INCLUSIVE OF VirtualDirectry                                          //
    // *** THESE ARE THE POPUP FILE LISTS THAT SHOW WHEN YOU HOVER OVER A FOLDER //
    ///////////////////////////////////////////////////////////////////////////////
    //public static string Create_Injectable_Directory_Folders_Popup_Menu(string VirtualDirectory)
    //{
    //    const string method = "Create_Injectable_Directory_Folders_Popup_Menu";
    //    Dictionary<string, DirectoryListing> addedDirectoryNames = new Dictionary<string, DirectoryListing>();

    //    DirectoryInfo virtRootDirInfo = new DirectoryInfo(RootFilesPath);
    //    DirectoryInfo phyRootDirInfo = new DirectoryInfo(virtRootDirInfo.FullName + @"\" + VirtualDirectory);

    //    /////////////////////////////////////////////////////////////////////////////
    //    // Look For Cached Markup Code   ((( Cached For An Hour Only In SQL )))    //
    //    /////////////////////////////////////////////////////////////////////////////
    //    //string popupMenus = SqlServer_Impl.GetCache("DocFiles", phyRootDirInfo.FullName, VirtualDirectory);
    //    //if (popupMenus.Length > 0)
    //    //    return popupMenus;

    //    string popupMenus = "";
    //    /////////////////////////////////
    //    // Verify Physical Path Exists //
    //    /////////////////////////////////
    //    if (phyRootDirInfo.Exists == false)
    //    {
    //        SqlServer_Impl.LogDebug(method, VirtualDirectory + " Is Missing");
    //        return "";
    //    }


    //    ///////////////////////////////////////////////////
    //    // Build Popup Menu File List For Each Directory //
    //    ///////////////////////////////////////////////////
    //    int menuNo = 1;
    //    foreach (DirectoryInfo dir in phyRootDirInfo.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore")))
    //    {
    //        string popupMenu = Create_Injectable_File_List_Popup(menuNo++, dir.FullName, "");

    //        /////////////////////////////////////////////////////////////////
    //        // Record that we added this name so we dont do it again later //
    //        // Add .lnk because thats what we will be expecting            //
    //        /////////////////////////////////////////////////////////////////
    //        DirectoryListing item = new DirectoryListing {DirectoryName = dir.Name, Html = popupMenu, MenuItemNo = menuNo - 1};

    //        addedDirectoryNames.Add(dir.Name + ".lnk", item);
    //    }



    //    //////////////////////////////////////////////////////////////
    //    // Build A Popup Menu Document List For Each Directory Link //
    //    //////////////////////////////////////////////////////////////
    //    foreach (FileInfo diskFile in phyRootDirInfo.GetFiles())
    //    {
    //        if (diskFile.Extension == ".lnk")
    //        {
    //            DirectoryInfo linkDirInfo = new DirectoryInfo(GetShortcutTarget(diskFile.FullName));
    //            string popupMenu;

    //            /////////////////////////////////////////////////////////////////
    //            // Record that we added this name so we dont do it again later //
    //            // Add .lnk because thats what we will be expecting            //
    //            /////////////////////////////////////////////////////////////////
    //            DirectoryListing item;

    //            ///////////////////////////////////////////////////////
    //            // See If We Have A Directory That Duplicates A .lnk //
    //            ///////////////////////////////////////////////////////
    //            addedDirectoryNames.TryGetValue(diskFile.Name, out item);

    //            ////////////////////
    //            // No Duplication //
    //            ////////////////////
    //            if (item == null)
    //            {
    //                popupMenu = Create_Injectable_File_List_Popup(menuNo++, linkDirInfo.FullName, "");
    //                item = new DirectoryListing {DirectoryName = linkDirInfo.Name, Html = popupMenu, MenuItemNo = menuNo - 1};
    //            }

    //            /////////////////////////////////////////////////////////
    //            // Folder and .lnk Require We Splice The List Together //
    //            /////////////////////////////////////////////////////////
    //            else
    //            {
    //                popupMenu = Create_Injectable_File_List_Popup(item.MenuItemNo, linkDirInfo.FullName, "");
    //                popupMenu = popupMenu.Substring(popupMenu.IndexOf("<li>", StringComparison.Ordinal));
    //                item.Html = item.Html.Replace("</ul>",  popupMenu);
    //                addedDirectoryNames.Remove(diskFile.Name);
    //            }

    //            /////////////////////////////////
    //            // Add / Update The Dictionary //
    //            /////////////////////////////////
    //            addedDirectoryNames.Add(linkDirInfo.Name + ".lnk", item);
    //        }
    //    }

    //    /////////////////////////////////////
    //    // Join All Html Elements Together //
    //    /////////////////////////////////////
    //    foreach (var addedDirectoryName in addedDirectoryNames)
    //        popupMenus += (addedDirectoryName.Value).Html;

    //    //////////////////
    //    // Cache Markup //
    //    //////////////////
    //    //SqlServer_Impl.RecordCache(phyRootDirInfo.FullName, popupMenus, "DocFiles", VirtualDirectory);

    //    ///////////////////
    //    // REturn Markup //
    //    ///////////////////
    //    return popupMenus;
    //}

    /////////////////////////////////////////////////////////////////////////
    // Create A Directory Popup Menu That Will Attached To A Specific Icon //
    // This Is Called When An ICON is provided by the Web Page.  This call //
    // Provides The File List Popup Html That Gets Injected Into The Page  //
    /////////////////////////////////////////////////////////////////////////
    public static string Create_Icon_Attached_Injectable_Directory_Popup_Menu(string DirectoryName)
    {
        const string method = "Create_Icon_Attached_Injectable_Directory_Popup_Menu";

        //////////////////////////////////////////////////////////////////
        // Test If THe Passed In DirectoryName Is A Link To A Directory //
        //////////////////////////////////////////////////////////////////
        DirectoryInfo virtRootDirInfo = new DirectoryInfo(RootFilesPath);
        //string FullPath = VirtRootDirInfo.FullName + @"\" + DirectoryName;

        ///////////////////////////////////////////////////
        // Check If We Pointing To A Link To A Directory //
        ///////////////////////////////////////////////////
        FileInfo diskFile = new FileInfo(virtRootDirInfo.FullName + @"\" + DirectoryName + ".lnk");
        if (diskFile.Exists)
            DirectoryName = GetShortcutTarget(diskFile.FullName);


        /////////////////////////////////////
        // Otherwise It's A Real DIrectory //
        /////////////////////////////////////
        else
            DirectoryName = RootFilesPath + @"\" + DirectoryName;

        //////////////////////////////////////////
        // Try And Get Cached Version Of Markup //
        //////////////////////////////////////////
        //string popupMenus = SqlServer_Impl.GetCache("DocFiles", DirectoryName, RootFilesPath);
        //if (popupMenus.Length > 0)
        //    return popupMenus;

        string popupMenus = "";

        /////////////////////////////////
        // Verify The Directory Exists //
        /////////////////////////////////
        if (!Directory.Exists(DirectoryName))
        {
            SqlServer_Impl.LogDebug(method, DirectoryName + " Is Missing");
            return "";
        }

        //////////////////////////////////////////
        // Build A Popup Menu For The Directory //
        //////////////////////////////////////////
        popupMenus = Create_Injectable_File_List_Popup(1, DirectoryName, "");

        //////////////////
        // Cache Markup //
        //////////////////
       // SqlServer_Impl.RecordCache(DirectoryName, popupMenus, "DocFiles", RootFilesPath);


        return popupMenus;
    }

    //////////////////////////////////////////////////////////////////////////////
    // Build A Popup Menu Of Files Located And All Sub Folder Files (Recursive) //
    // Creates A <UL><LI>File</LI><LI>...</LI></UL> For Each Folder             //
    //////////////////////////////////////////////////////////////////////////////
    private static string Create_Injectable_File_List_Popup(int MenuNo, string FullDirectoryPath, string SubMenuPrefix /*, string VirtualDirectory */)
    {
        const string method = "Create_Injectable_File_List_Popup";
        string menuId;
        string popupMenu = String.Empty;
        DirectoryInfo dirInfo = new DirectoryInfo(FullDirectoryPath);

        if (dirInfo.Exists == false)
        {
            SqlServer_Impl.LogDebug(method, FullDirectoryPath + " Is Missing");
            return "";
        }


        ///////////////////
        // Build Menu Id //
        ///////////////////
        if (SubMenuPrefix.Length == 0)
            menuId = "ebul_mb73g0_" + MenuNo;
        else
            menuId = SubMenuPrefix + MenuNo;

        ////////////////////////
        // Start A Popup Menu //
        ////////////////////////
        if (SubMenuPrefix.Length == 0)
        {
            popupMenu += "\n    <!-- File Galery Popup Menu   " + menuId + "  For Directory  " + FullDirectoryPath + "  -->\n";
            popupMenu += "    <ul style=\"display: none;\" id=\"" + menuId + "\" class=\"ebul_mb73g0\">\n";
        }
        else
        {
            popupMenu += "\n        <!-- File Galery SubMenu Popup   " + menuId + "  For Directory  " + FullDirectoryPath + "  -->\n";
            popupMenu += "        <ul id=\"" + menuId + "_table\" class=\"ebul_mb73g0\">\n";
        }

        ////////////////////////////////////////////////////////
        // Check For Sub Directories                          //
        // For Each Sub Directory, Add A Link To A Popup Menu //
        // Then Generate The Sub PopMenu                      //
        ////////////////////////////////////////////////////////
        int subMenuNo = 1;
        foreach (DirectoryInfo subDir in dirInfo.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore") && dirname.Extension != "db" ))
        {
            popupMenu += "        <li>\n";
            popupMenu += "            <span style='background-image: url(\"/Images/folder_32.gif\");' class=\"ebul_imgmb73g032x32\"></span>\n";
            popupMenu += "            <a title=\"\">" + subDir.Name + "</a>\n";

            ///////////////////////////////////////////////////////
            //                Note The Recursion                 //
            // This Will Insert A Unorder List Paragraph Under   //
            // The Current List Item, Which Becomes The Sub Menu //
            ///////////////////////////////////////////////////////
            //string SubVirtualDirectory = VirtualDirectory + "/" + SubDir.Name;
            popupMenu += Create_Injectable_File_List_Popup(subMenuNo, subDir.FullName, menuId + "_");
            popupMenu += "        </li>\n";

            subMenuNo++;
        }


        //////////////////////////////////////////////
        // Make Sure Content Database Is Up To Date //
        //////////////////////////////////////////////
        //ContentUpd Updater = new ContentUpd();
        //Updater.ContentScan("/Files", FullDirectoryPath, true);


        //////////////////////////////////////////
        // Add Links To Files In This Directory //
        //////////////////////////////////////////
        foreach (FileInfo diskFile in dirInfo.GetFiles())
        {
            if (diskFile.Extension != ".db" && diskFile.Extension != ".ignore")
            {
                string vPath = GetShortcutTarget(diskFile.FullName);
                string imageIcon;

                if (!(vPath.StartsWith("http:") || vPath.StartsWith("https:")))
                {
                    FileInfo linkInfo = new FileInfo(vPath);
                    vPath = toVirtual(linkInfo.FullName, "/Files").Replace("localhost", Environment.MachineName);
                    imageIcon = SetIcon(linkInfo.Extension.ToLower());
                }
                else
                {
                    imageIcon = SetIcon("href");
                }

                popupMenu += "        <li>\n";
                popupMenu += "            <span style='background-image: url(\"" + imageIcon + "\");' class=\"ebul_imgmb73g032x32\"></span>\n";
                popupMenu += "            <a  target=\"_blank\" href=\"" + vPath + "\">" + diskFile.Name.Replace(".lnk", "").Replace(".url", "") + "    </a>\n";
                popupMenu += "        </li>\n";
            }
        }

        //////////////////////////
        // Close The Popup Menu //
        //////////////////////////
        popupMenu += "    </ul>\n";

        return popupMenu;
    }

    public static string FileNameSearch(string searchParam)
    {
        List<DirectoryList> dirList = new List<DirectoryList>();
        try
        {
            foreach (string file in Directory.EnumerateFiles(RootFilesPath, "*" + searchParam + "*", SearchOption.AllDirectories ))
            {
                

                FileInfo subFile = new FileInfo(file); ;

                if (subFile.Name != "Folder.png" && subFile.Extension != ".lnk")
                {
                    string fullPath = file;
                    string displayName = subFile.Name;
                    if (displayName.Length > 5)
                        displayName = displayName.Remove(displayName.Length - 4);

                    /////////////////////////////////
                    // Add Specific File eferences //
                    /////////////////////////////////
                    if (!subFile.Name.EndsWith(".db"))
                    {
                        DirectoryList thisDirList = new DirectoryList
                        {
                            DisplayName = displayName,
                            Icon = SetIcon(subFile.Extension),
                            LongName = fullPath,
                            ShortName = displayName,
                            Href = toVirtual(fullPath, @"/Files"),
                            Ext = subFile.Extension
                        };
                        dirList.Add((thisDirList));

                    }
                }


            }

        }
        catch (System.Exception excpt)
        {
            Console.WriteLine(excpt.Message);
        }


        return "";
    }
#endregion

#region Video Section
    //public static string Create_Injectable_Video_Folders(string DirName, bool liOnly = false)
    //{
    //    const string method = "Create_Injectable_Video_Folders";
    //    int menuListNo = 100;
    //    int popupMenuListNo = 100;

    //    try
    //    {
    //        bool hasOther = false;
    //        string phyGalleryPath = RootVideoPath + "\\" + DirName;
    //        string logGalleryPath = @"//" + Environment.MachineName + @"/videos/" + DirName;

    //        /////////////////////////////////////////////////////////////////////////////
    //        // Look For Cached Markup Code   ((( Cached For An Hour Only In SQL )))    //
    //        /////////////////////////////////////////////////////////////////////////////
    //        string imageGalleries = SqlServer_Impl.GetCache("VideoFolders", phyGalleryPath, logGalleryPath, ListOnly: liOnly);
    //        if (imageGalleries.Length > 0)
    //            return imageGalleries;

    //        DirectoryInfo rootDirInfo = new DirectoryInfo(phyGalleryPath);

    //        if (rootDirInfo.Exists == false)
    //        {
    //            SqlServer_Impl.LogDebug(method, DirName + " Is Missing");
    //            return "";
    //        }

    //        foreach (DirectoryInfo dir in rootDirInfo.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore")))
    //        {
    //            string galleryName = dir.Name;

    //            ///////////////////////////////////////////////////////////////////////////
    //            // If There Are Sub Directories, Build Folders Containing Videos With An //
    //            // Associated Popup Menu Listing Specific Videos                         //
    //            ///////////////////////////////////////////////////////////////////////////
    //            if (dir.EnumerateDirectories().Any())
    //            {
    //                /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                // Build Div To Hold   Video Icons       Or                                                            //
    //                // Folders Containing Videos With an Associated Popup Menu Listing Specific Videos //
    //                /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                if (! liOnly)
    //                {
    //                    imageGalleries += "<div id=\"" + galleryName + "\"  >\n\n";
    //                    imageGalleries += "    <h2>" + galleryName + "</h2>\n";
    //                    imageGalleries += "        <ul id=\"MenuList" + menuListNo + "\"  class=\"menu_gallery\" >\n";
    //                }
                    

    //                //////////////////////////////////////////////////////////////
    //                // First We Add A Folder Icon Representing The Video Folder //
    //                //////////////////////////////////////////////////////////////
    //                foreach (DirectoryInfo subDir in dir.EnumerateDirectories())
    //                {
    //                    const string folderPath = @"/images/VideoFolder.png";

    //                    imageGalleries += "            <li>";
    //                    imageGalleries += "  <a>";
    //                    imageGalleries += "  <img  id=\"mbi_mb73g0_" + popupMenuListNo + "\"  src=\"" +
    //                                      folderPath + "\" alt=\"" + subDir.Name + "\" /><span class='VideoFolderName'>" + subDir.Name + "</span>";
    //                    imageGalleries += "  </a>";
    //                    imageGalleries += "  </li>\n";

    //                    popupMenuListNo++;
    //                }

                    
    //                if (! liOnly)
    //                {
    //                    imageGalleries += "    </ul>\n";
    //                    imageGalleries += "</div>\n";
    //                    imageGalleries += "\n<br/>\n\n";
    //                }

    //                menuListNo++;
    //            }
    //            else
    //            {
    //                hasOther = true;
    //            }
    //        }


    //        ///////////////////////////////////////////////////////////////////
    //        // Now Build A List Of Gallery Folders That Don't Have Sub Menus //
    //        // Assuming That That Means The List Is Files Is Fairly Short    //
    //        ///////////////////////////////////////////////////////////////////
    //        if (hasOther)
    //        {
    //            const string galleryName = "Other Galleries";

    //            /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //            // Build Div To Hold   Video Icons       Or                                                            //
    //            //                     Folders Containing Videos With an Associated Popup Menu Listing Specific Videos //
    //            /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //            if (! liOnly)
    //            {
    //                imageGalleries += "<div id=\"" + galleryName + "\"  >\n\n";
    //                imageGalleries += "        <ul id=\"MenuList" + menuListNo + "\"  class=\"menu_gallery\" >\n";
    //            }
                

    //            foreach (DirectoryInfo dir in rootDirInfo.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore")))
    //            {
    //                /////////////////////////////////////////////////////////////////////////////////////////////////
    //                // Build Folders Icons Containing Videos With An Associated Popup Menu Listing Specific Videos //
    //                /////////////////////////////////////////////////////////////////////////////////////////////////
    //                if (! dir.EnumerateDirectories().Any())
    //                {
    //                    const string folderPath = @"/images/VideoFolder.png";

    //                    imageGalleries += "            <li>";
    //                    imageGalleries += "  <a>";
    //                    imageGalleries += "  <img  id=\"mbi_mb73g0_" + popupMenuListNo + "\"  src=\"" +
    //                                        folderPath + "\" alt=\"" + dir.Name + "\" /><span class='VideoFolderName'>" + dir.Name + "</span>";
    //                    imageGalleries += "  </a>";
    //                    imageGalleries += "  </li>\n";


    //                    popupMenuListNo++;
    //                }

    //            }

                
    //            if (! liOnly)
    //            {
    //                imageGalleries += "    </ul>\n";
    //                imageGalleries += "</div>\n";
    //                imageGalleries += "\n<br/>\n\n";
    //            }
    //        }


    //        /////////////////////////////////////////////////////
    //        // Inject The Gallery Folders ANd Popup File Lists //
    //        /////////////////////////////////////////////////////
    //        //SqlServer_Impl.RecordCache(phyGalleryPath, imageGalleries, "VideoFolders", logGalleryPath, ListOnly: liOnly);

    //        return imageGalleries;
    //    }


    //    catch (Exception ex)
    //    {
    //        return ex.InnerException.Message;
    //    }
    //}
    //public static string Create_Injectable_Video_Folder_Popup_Menu(string DirName)
    //{
    //    const string method = "Create_Injectable_Video_Folder_Popup_Menu";
    //    //int MenuListNo = 100;
    //    int popupMenuListNo = 100;

    //    try
    //    {
    //        bool hasOther = false;
    //        string phyGalleryPath = RootVideoPath + "//" + DirName;
    //        string logGalleryPath = @"/videos/" + DirName;

    //        /////////////////////////////////////////////////////////////////////////////
    //        // Look For Cached Markup Code   ((( Cached For An Hour Only In SQL )))    //
    //        /////////////////////////////////////////////////////////////////////////////
    //        string popupMenus = SqlServer_Impl.GetCache("VideoFiles", phyGalleryPath, logGalleryPath);
    //        if (popupMenus.Length > 0)
    //            return popupMenus;


    //        DirectoryInfo rootDirInfo = new DirectoryInfo(phyGalleryPath);

    //        if (rootDirInfo.Exists == false)
    //        {
    //            SqlServer_Impl.LogDebug(method, DirName + " Is Missing");
    //            return string.Empty;
    //        }

    //        foreach (DirectoryInfo dir in rootDirInfo.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore")))
    //        {
    //            //string GalleryName = Dir.Name;

    //            ///////////////////////////////////////////////////////////////////////////
    //            // If There Are Sub Directories, Build Folders Containing Videos With An //
    //            // Associated Popup Menu Listing Specific Videos                         //
    //            ///////////////////////////////////////////////////////////////////////////
    //            if (dir.EnumerateDirectories().Any())
    //            {

    //                //////////////////////////////////////////////////////////////
    //                // First We Add A Folder Icon Representing The Video Folder //
    //                //////////////////////////////////////////////////////////////
    //                foreach (DirectoryInfo subDir in dir.EnumerateDirectories())
    //                {
    //                    /////////////////////////////////////////////
    //                    // Build A Popup Menu For This Folder Icon //
    //                    /////////////////////////////////////////////
    //                    popupMenus += Create_Video_Popup_Menu(subDir, "ebul_mb73g0_" + popupMenuListNo,
    //                                                     logGalleryPath + "/" + subDir.Parent + "/" + subDir.Name);

    //                    popupMenuListNo++;
    //                }

    //                //MenuListNo++;
    //            }
    //            else
    //            {
    //                hasOther = true;
    //            }
    //        }


    //        ///////////////////////////////////////////////////////////////////
    //        // Now Build A List Of Gallery Folders That Don't Have Sub Menus //
    //        // Assuming That That Means The List Is Files Is Fairly Short    //
    //        ///////////////////////////////////////////////////////////////////
    //        if (hasOther)
    //        {
    //            foreach (DirectoryInfo dir in rootDirInfo.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore")))
    //            {
    //                /////////////////////////////////////////////////////////////////////////////////////////////////
    //                // Build Folders Icons Containing Videos With An Associated Popup Menu Listing Specific Videos //
    //                /////////////////////////////////////////////////////////////////////////////////////////////////
    //                if (!dir.EnumerateDirectories().Any())
    //                {
    //                    /////////////////////////////////////////////
    //                    // Build A Popup Menu For This Folder Icon //
    //                    /////////////////////////////////////////////
    //                    popupMenus += Create_Video_Popup_Menu(dir, "ebul_mb73g0_" + popupMenuListNo,
    //                                                     logGalleryPath + "/" + dir.Name);
    //                    popupMenuListNo++;
    //                }

    //            }
    //        }


    //        /////////////////////////////////////////////////////
    //        // Inject The Gallery Folders ANd Popup File Lists //
    //        /////////////////////////////////////////////////////
    //        //SqlServer_Impl.RecordCache(phyGalleryPath, popupMenus, "VideoFiles", logGalleryPath);

    //        return popupMenus;
    //    }


    //    catch (Exception ex)
    //    {
    //        return ex.InnerException.Message;
    //    }

    //}
    //public static string Create_Icon_Attached_Injectable_Video_Popup_Menu(string DirName, int PopupMenuListNo = 100)
    //{
    //    const string logGalleryPath = "/videos";
    //    //string LogGalleryPath = @"//" + System.Environment.MachineName + @"/videos";
    //    string phyGalleryPath = RootVideoPath + "//" + DirName;
    //    DirectoryInfo rootDirInfo = new DirectoryInfo(phyGalleryPath);

    //    /////////////////////////////////////////////////////////////////////////////
    //    // Look For Cached Markup Code   ((( Cached For An Hour Only In SQL )))    //
    //    /////////////////////////////////////////////////////////////////////////////
    //    //string popupMenus = SqlServer_Impl.GetCache("VideoFiles", phyGalleryPath, logGalleryPath, MenuID: PopupMenuListNo);
    //    //if (popupMenus.Length > 0)
    //    //    return popupMenus;

    //    string popupMenus = "";
    //    /////////////////////////////////////////////////////
    //    // Create Popup Menus That Will Be Tied To An ICON //
    //    /////////////////////////////////////////////////////
    //    popupMenus = Create_Video_Popup_Menu(rootDirInfo, "ebul_mb73g0_" + PopupMenuListNo, logGalleryPath + "/" + DirName.Replace('\\', '/')   );

    //    //////////////////////
    //    // Cache The Markup //
    //    //////////////////////
    //    //SqlServer_Impl.RecordCache(phyGalleryPath, popupMenus, "VideoFiles", logGalleryPath, PopupMenuListNo);

    //    ///////////////////////
    //    // Return The Markup //
    //    ///////////////////////
    //    return popupMenus;
    //}
    //private static string Create_Video_Popup_Menu(DirectoryInfo SubDir, string MenuID, string LogGalleryPath)
    //{
    //    string popupMenu = string.Empty;

    //    //////////////////////////////////////////////////
    //    // Build A Popup Menu That List Specific Videos //
    //    //////////////////////////////////////////////////
    //    popupMenu += "    <!-- Video Folder Popup Menu " + MenuID + "   " + LogGalleryPath + "/" + SubDir.Name + "  -->\n\n";
    //    popupMenu += "    <ul style=\"display: none;\" id=\"" + MenuID + "\" class=\"ebul_mb73g0\">\n";




    //    ////////////////////////////////////////////////////////
    //    // Check For Sub Directories                          //
    //    // For Each Sub Directory, Add A Link To A Popup Menu //
    //    // Then Generate The Sub PopMenu                      //
    //    ////////////////////////////////////////////////////////
    //    int subMenuNo = 1;
    //    foreach (DirectoryInfo subDirFolder in SubDir.GetDirectories().Where(dirname => !dirname.Name.ToLower().EndsWith("ignore") && dirname.Extension != "db"))
    //    {
    //        popupMenu += "        <li>\n";
    //        popupMenu += "            <span style='background-image: url(\"/Images/folder_32.gif\");' class=\"ebul_imgmb73g032x32\"></span>\n";
    //        popupMenu += "            <a title=\"\">" + subDirFolder.Name + "</a>\n";

    //        ///////////////////////////////////////////////////////
    //        //                Note The Recursion                 //
    //        // This Will Insert A Unorder List Paragraph Under   //
    //        // The Current List Item, Which Becomes The Sub Menu //
    //        ///////////////////////////////////////////////////////
    //        popupMenu += Create_Video_Popup_Menu(subDirFolder, MenuID + "_" + subMenuNo, LogGalleryPath + @"/" + subDirFolder.Name);
    //        popupMenu += "        </li>\n";

    //        subMenuNo++;
    //    }






    //    ////////////////////////////
    //    // Create Links To Videos //
    //    ////////////////////////////
    //    foreach (FileInfo subFiles in SubDir.GetFiles())
    //    {
    //        if (subFiles.Name != "Folder.png" && subFiles.Extension != ".db" )
    //        {
    //            if (IsVideo(subFiles.Extension))
    //            {
    //                string imageIcon = SetIcon(subFiles.Extension);

    //                //string FullPath = LogGalleryPath + "/" + SubFiles.Name;
    //                string displayName = subFiles.Name;
    //                if (displayName.Length > 13)
    //                {
    //                    //DisplayName = SubFiles.Name.Substring(9);
    //                    displayName = displayName.Remove(displayName.Length - 4);
    //                }

    //                /////////////////////////////////
    //                // Add Specific File eferences //
    //                /////////////////////////////////
    //                popupMenu += "        <li>  <span style='background-image: url(\"" + imageIcon +
    //                             "\");' class=\"ebul_imgmb73g032x32\"></span>\n";
    //                popupMenu += "            <a target='_flovid' href=/video/FlowPlayerVideo.aspx?Video=" +
    //                             subFiles.Name.Replace(" ", "%20") + "&EID=" + BusinessLayer.UserEmpID + "&Path=" + LogGalleryPath.Replace(" ", "%20") + ">" + displayName + "</a>\n";
    //                popupMenu += "        </li>\n\n";
    //            }
    //        }
    //    }

    //    popupMenu += "    </ul>\n";

    //    return popupMenu;
    //}
    public static string Create_Injectable_Video_Support_Documents(string Path, string FileName)
    {
        string logGalleryPath = @"//" + Environment.MachineName + Path;
        string phyGalleryPath = HttpContext.Current.Server.MapPath(Path);
        string host = HttpContext.Current.Request.Url.DnsSafeHost;

        if (Path == "null" || FileName == "null" )
            return "";

        FileName = FileName.Substring(0, FileName.LastIndexOf('.'));

        string supportDocuments = string.Empty;
        DirectoryInfo pathInfo = new DirectoryInfo(phyGalleryPath);

        //////////////////////////////////////////////////
        // Build A Popup Menu That List Specific Videos //
        //////////////////////////////////////////////////
        foreach (FileInfo subFiles in pathInfo.GetFiles())
        {
            if (subFiles.Name.Contains(FileName))
            {
                if ( ! IsVideo(subFiles.Extension))
                {
                    //string ImageIcon = SetIcon(SubFiles.Extension);
// LogonRcd URL CHANGE
                    string fullPath = logGalleryPath + "/" + subFiles.Name;
                    fullPath = "http://" + host + "/" + Path + "/" + subFiles.Name;
                    string displayName = subFiles.Name;

                    if (displayName.Length > 13)
                        displayName = displayName.Replace(subFiles.Extension, "");

                    supportDocuments += "            <a class='aButton' target='_viddoc' href='" +    fullPath             + "' >" + displayName + "</a><br/>\n";
                }
            }
        }

        return supportDocuments;
    }
#endregion



    public static string SetIcon(string FileType)
    {
        if (FileType.EndsWith("pdf")) return "/Images/adobe_icon.gif";
        if (FileType.EndsWith(".pdf")) return "/Images/adobe_icon.gif";

        if (FileType.EndsWith("swf")) return "/Images/flash_icon.gif";
        if (FileType.EndsWith(".swf")) return "/Images/flash_icon.gif";

        if (FileType.EndsWith("flv")) return "/Images/flash_icon.gif";
        if (FileType.EndsWith(".flv")) return "/Images/flash_icon.gif";

        if (FileType.EndsWith("flash")) return "/Images/flash_icon.gif";
        if (FileType.EndsWith(".flash")) return "/Images/flash_icon.gif";

        if (FileType.EndsWith("mp4")) return "/Images/video_mp4.gif";
        if (FileType.EndsWith(".mp4")) return "/Images/video_mp4.gif";

        //if (FileType.EndsWith("wmv")) return "/Images/video_mp4.gif";
        //if (FileType.EndsWith(".wmv")) return "/Images/video_mp4.gif";

        if (FileType.EndsWith("href")) return "/Images/href.png";
        if (FileType.EndsWith(".href")) return "/Images/href.png";

        if (FileType.EndsWith("doc")) return "/Images/word_32.gif";
        if (FileType.EndsWith(".doc")) return "/Images/word_32.gif";

        if (FileType.EndsWith("docx")) return "/Images/word_32.gif";
        if (FileType.EndsWith(".docx")) return "/Images/word_32.gif";

        if (FileType.EndsWith("xls")) return "/Images/excel_32.gif";
        if (FileType.EndsWith(".xls")) return "/Images/excel_32.gif";

        if (FileType.EndsWith("xlsx")) return "/Images/excel_32.gif";
        if (FileType.EndsWith(".xlsx")) return "/Images/excel_32.gif";

        return "/Images/Unknown.gif";
    }
    public static bool IsLink(string shortcutFilename)
    {
        string pathOnly = Path.GetDirectoryName(shortcutFilename);
        string filenameOnly = Path.GetFileName(shortcutFilename);

        Shell32.Shell shell = new Shell32.ShellClass();
        Shell32.Folder folder = shell.NameSpace(pathOnly);
        Shell32.FolderItem folderItem = folder.ParseName(filenameOnly);
        if (folderItem != null)
        {
            return folderItem.IsLink;
        }
        return false; // not found
    }
    public static bool IsVideo(string Extension)
    {
        if (Extension == ".swf") return true;
        if (Extension == ".flv") return true;
        if (Extension == ".flash") return true;
        if (Extension == ".mp4") return true;
        if (Extension == ".mpeg") return true;
        //if (Extension == ".wmv") return true;
        return false;
    }
    public static string GetShortcutTarget(string shortcutFilename)
    {
        string pathOnly = Path.GetDirectoryName(shortcutFilename);
        string filenameOnly = Path.GetFileName(shortcutFilename);

        Shell32.Shell shell = new Shell32.ShellClass();
        Shell32.Folder folder = shell.NameSpace(pathOnly);
        Shell32.FolderItem folderItem = folder.ParseName(filenameOnly);
        if (folderItem != null)
        {
            if (folderItem.IsLink)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }
            return shortcutFilename;
        }
        return "";  // not found
    }
    public static string toVirtual(string physicalPath, string VirtualRoot)
    {
        string[] xxx = HttpContext.Current.Request.Url.AbsoluteUri.Split('/');


        string rootPath = HttpContext.Current.Server.MapPath(VirtualRoot);
        string virtRootDir = new DirectoryInfo(rootPath).FullName;
        string url = physicalPath.Replace(@"//", "/").Substring(virtRootDir.Length).Replace('\\', '/').Insert(0, @"http://" + xxx[2] + VirtualRoot);
        return (url);
    }
}