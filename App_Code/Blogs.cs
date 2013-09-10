using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


public static  class Blogs
{

    public static string FindVirtualDirectory(string DirName)
    {
        System.IO.DirectoryInfo RootDirInfo = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(@"~" + DirName));
        if (RootDirInfo.Exists == true)
            return RootDirInfo.FullName;
        return "";

        throw (new Exception("Failed To FInd Virtual Directory " + DirName));
    }


    public static string Create_Injectable_Blog_Advertisements(string BlogName)
    {
        string InjectableListItems = string.Empty;

        try
        {
            foreach (SIU_Blog anAdvBlog in SqlServer_Impl.GetBlogsAdvertised(BlogName))
            {
                InjectableListItems += Environment.NewLine;
                InjectableListItems += "<li>" + Environment.NewLine;
                InjectableListItems += "    <div class=\"BlogText\" >" + Environment.NewLine;

                if (anAdvBlog.BlogTitleURL == null) anAdvBlog.BlogTitleURL = "";
                if (anAdvBlog.BlogTitleURL.Length > 0)
                    InjectableListItems += "        <a href=\"" + anAdvBlog.BlogTitleURL + "\">" + anAdvBlog.BlogTitle +
                                           "</a>" + Environment.NewLine;
                else
                    InjectableListItems += "        <p>" + anAdvBlog.BlogTitle + "</p>" + Environment.NewLine;


                InjectableListItems += "    </div>" + Environment.NewLine;
                InjectableListItems += "</li>" + Environment.NewLine;
            }

            
        }

        catch
        {
            InjectableListItems += Environment.NewLine;
            InjectableListItems += "<li>" + Environment.NewLine;
            InjectableListItems += "    <div class=\"BlogText\" >" + Environment.NewLine;
            InjectableListItems += "        <p>DATABASE CONECTION FAILED</p>" + Environment.NewLine;
            InjectableListItems += "    </div>" + Environment.NewLine;
            InjectableListItems += "</li>" + Environment.NewLine;
        }
        return InjectableListItems;

    }



    internal class RotatorImageMeta
    {
        public string ImageName = string.Empty;
        public string ImagePath = string.Empty;
        public string ImageText = string.Empty;
        public string ImageLink = string.Empty;

        public string BuildListItem()
        {
            string ThisItem = string.Empty;

            ThisItem += "<li>" + Environment.NewLine;

            if (ImagePath.Length > 0)
                ThisItem +=  "    <div class=\"SliderImage\">" + Environment.NewLine;

            if ( ImageLink.Length > 0 )
                ThisItem += "        <a href=\"" + ImageLink + "\">" + Environment.NewLine;

            if (ImagePath.Length > 0)
            ThisItem += "           <img src=\"" + ImagePath + "\"   alt=\"" + ImageName + "\" style=\"border: none;\"/>" + Environment.NewLine;

            if (ImageText.Length > 0)
            {
                ThisItem += "           <div class=\"SliderText\">" + Environment.NewLine;
                ThisItem += "               <p>" + ImageText + "</p>" + Environment.NewLine;
                ThisItem += "           </div>" + Environment.NewLine;
            }


            if ( ImageLink.Length > 0 )
                ThisItem += "        </a>" + Environment.NewLine;

            if (ImagePath.Length > 0)
                ThisItem +=  "    </div>" + Environment.NewLine;

            ThisItem += "</li>" + Environment.NewLine + Environment.NewLine + Environment.NewLine;

            return ThisItem;
        }
    }

    public static string Create_Injectable_Ads(string LogRoot, string LogPath)
    {
        string RootPhyPath = FindVirtualDirectory(LogRoot);
        string PhyPath = RootPhyPath + @"\" + LogPath;

        ///////////////////////
        // Directory Missing //
        ///////////////////////
        if (RootPhyPath.Length == 0)
            return "";

        DirectoryInfo RootDirInfo = new System.IO.DirectoryInfo(PhyPath);

        Dictionary<string, RotatorImageMeta> ImagesDict = new Dictionary<string, RotatorImageMeta>();

        try
        {
            ///////////////////////////////////////////////////////////////
            // Walk Through Each File In Directory And Add To Image List //
            ///////////////////////////////////////////////////////////////
            foreach (FileInfo DiskFile in RootDirInfo.GetFiles())
            {
                //////////////////////////////////////
                // Get Disk File Type and Base Name //
                //////////////////////////////////////
                string[] FileParts = DiskFile.Name.Split('.');
                string FileType = FileParts[FileParts.Length - 1];
                string FileName = FileParts[0];

                ///////////////////////////////////////////////////////
                // Get Image Info Class From Dictionary If It Exists //
                // Otherwise Make A New One                          //
                ///////////////////////////////////////////////////////
                RotatorImageMeta thisImage;
                if (ImagesDict.ContainsKey(FileName))
                    thisImage = ImagesDict[FileName];
                else
                    thisImage = new RotatorImageMeta();


                ////////////////////////////////////////////////
                // Update Image Info Class Based On File Type //
                ////////////////////////////////////////////////
                thisImage.ImageName = FileName;
                switch (FileType)
                {
                    case "txt":
                        thisImage.ImageText = GetFileText(DiskFile.FullName);
                        break;

                    case "htm":
                        thisImage.ImageText = GetFileText(DiskFile.FullName);
                        break;

                    case "html":
                        thisImage.ImageText = GetFileText(DiskFile.FullName);
                        break;

                    case "ilk":
                        thisImage.ImagePath = GetFileText(DiskFile.FullName);
                        break;

                    case "ref":
                        thisImage.ImageLink = GetFileText(DiskFile.FullName);
                        break;

                    case "docx": break;
                    case "dat": break;
                    case "db": break;

                    default:
                        thisImage.ImagePath = LogRoot + "/" + LogPath + "/" + DiskFile.Name;
                        break;
                }

                //////////////////////////////////////////////////////////
                // Add or Update Image Dictionary With Image Info Class //
                //////////////////////////////////////////////////////////
                //if (DiskFile.Extension != ".db")
                if (thisImage.ImageLink != "" || thisImage.ImagePath != "" || thisImage.ImageText != "")
                {
                    if (ImagesDict.ContainsKey(FileName))
                        ImagesDict[FileName] = thisImage;
                    else
                        ImagesDict.Add(FileName, thisImage);
                }

            }

            /////////////////////////////////////////////////
            // Build Injectable List Items From Dictionary //
            // Image Info Class Does The Hard Work........ //
            /////////////////////////////////////////////////
            string RotatorListItems = string.Empty;
            foreach (var ImageItem in ImagesDict)
                RotatorListItems += (ImageItem.Value).BuildListItem();

            return RotatorListItems;
        }

        catch (Exception ex)
        {
            return ex.Message;
            //throw ex;
        }

    }

    private static string GetFileText(string FileName)
    {
        string Content = string.Empty;
        try
        {
            using (StreamReader sr = new StreamReader(FileName))
            {
                Content = sr.ReadToEnd();
            }
        }

        catch
        {
            Content = "The file could not be read";
        }

        return Content;

    }
}