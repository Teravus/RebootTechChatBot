using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Infrastructure
{
    public class FileRequest
    {
        private Config.BotConfig.HttpServer httpserverConfig = null;

        private DirectoryInfo RootDirectory = null;

        private int LongestPath = short.MaxValue;

        public FileRequest(Config.BotConfig.HttpServer config)
        {
            
            httpserverConfig = config;
            string root = httpserverConfig.DocumentRoot;

            if (string.IsNullOrEmpty(root) || string.IsNullOrWhiteSpace(root))
                root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inetpub");

            bool absolutepath = false;
            if (root.Length > 2)
            {
                // check for drive
                if (root.Substring(1,1)==":")
                {
                    absolutepath = true;
                }
            }
            if (absolutepath)
                RootDirectory = new DirectoryInfo(CleanPath(root));
            else
            {

                RootDirectory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CleanPath(root)));
            }
        }
        public bool HasFile(string fulluri)
        {
            return false;
        }

        public bool TryGetFile(OSHttpRequest request, out FileResponse FileResult, out Exception ex)
        {
            string uri = request.Url.AbsolutePath;
            string pathrelativetoroot = uri.Replace("/", "\\");

            //string[] dirsegments = pathrelativetoroot.Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            if (pathrelativetoroot.StartsWith("\\"))
            {
                pathrelativetoroot = pathrelativetoroot.Substring(1, pathrelativetoroot.Length - 1);
            }
            string absolutefile = Path.Combine(RootDirectory.FullName, pathrelativetoroot);
            ex = null;
            bool Found = false;
            Found = File.Exists(absolutefile);
            FileResult = null;
            if (Found)
            {

                try
                {
                    DirectoryInfo drinfo = new DirectoryInfo(absolutefile);
                    string extension = drinfo.Extension;
                    MimeType outtype = null;
                    if (MainServer.Instance.TryGetRegisteredMIMETypeByExtension(extension, out outtype))
                    {

                        try
                        {
                            byte[] filedata = File.ReadAllBytes(absolutefile);
                            FileResult = new FileResponse()
                            {
                                Data = filedata,
                                MimeType = outtype,
                                PhysicalPath = absolutefile,
                                Uri = uri
                            };
                        }
                        catch (ArgumentNullException ex2)
                        {
                            ex = ex2;
                        }
                        catch (System.Security.SecurityException ex2)
                        {
                            ex = ex2;
                        }
                        catch (ArgumentException ex2)
                        {
                            ex = ex2;
                        }
                        catch (PathTooLongException ex2)
                        {
                            ex = ex2;
                        }
                        catch (DirectoryNotFoundException ex2)
                        {
                            ex = ex2;
                        }
                        catch (FileNotFoundException ex2)
                        {
                            ex = ex2;
                        }
                        catch (NotSupportedException ex2)
                        {
                            ex = ex2;
                        }
                        catch (IOException ex2)
                        {
                            ex = ex2;
                        }
                        catch (UnauthorizedAccessException ex2)
                        {
                            ex = ex2;
                        }

                    }
                    else
                        Found = false;
                }
                catch (ArgumentNullException ex1)
                {
                    ex = ex1;
                }
                catch (System.Security.SecurityException ex1)
                {
                    ex = ex1;
                }
                catch (ArgumentException ex1)
                {
                    ex = ex1;
                }
                catch (PathTooLongException ex1)
                {
                    ex = ex1;
                }

            }
            return Found;
        }
        private static readonly char[] InvalidPathCars =
        {
            '\"', '<','>','|','\0',
            (char)1,(char)2,(char)3,(char)4,(char)5,(char)6,(char)7,(char)8,(char)9,(char)10,
            (char)11,(char)12,(char)13,(char)14,(char)15,(char)16,(char)17,(char)18,(char)19,(char)20,
            (char)21,(char)22,(char)23,(char)24,(char)25,(char)26,(char)27,(char)28,(char)29,(char)30,(char)31,
        };

        // cleans path.  Removes extra separators etc.   Does not validate path exists
        internal static string CleanPath(string s)
        {
            s = string.Join("", s.Split(InvalidPathCars)); // remove invalid path characters
            int l = s.Length;
            int sub = 0;
            int alt = 0;
            int start = 0;

            char s0 = s[0];
            if (l > 2 && s0 == '\\' && s[1] == '\\')
            {
                start = 2;
            }

            if (l == 1 && (s0 == Path.DirectorySeparatorChar || s0 == Path.AltDirectorySeparatorChar))
                return s;

            for (int i = start; i < l; i++)
            {
                char c = s[i];

                if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar)
                    continue;
                if (Path.DirectorySeparatorChar != Path.AltDirectorySeparatorChar && c == Path.AltDirectorySeparatorChar)
                    alt++;
                if (i + 1 == l)
                    sub++;
                else
                {
                    c = s[i + 1];
                    if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
                        sub++;
                }
            }

            if (sub == 0 && alt == 0)
                return s;

            char[] copy = new char[l - sub];
            if (start != 0)
            {
                copy[0] = '\\';
                copy[1] = '\\';

            }
            for (int i = start, j=start; i < l && j < copy.Length; i++)
            {
                char c = s[i];
                if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar)
                {
                    copy[j++] = c;
                    continue;
                }

                if (j + 1 != copy.Length)
                {
                    copy[j++] = Path.DirectorySeparatorChar;
                    for (; i < l - 1; i++)
                    {
                        c = s[i + 1];
                        if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar)
                            break;
                    }
                }
            }
            return new string(copy);
        }
        public class FileResponse
        {
            public string Uri { get; set; }
            public string PhysicalPath { get; set; }
            public byte[] Data { get; set; }
            public MimeType MimeType { get; set; }
            
        }
       
    }
}
