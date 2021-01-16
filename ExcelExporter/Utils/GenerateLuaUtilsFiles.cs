using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceTools
{
    class GenerateLuaUtilsFiles
    {
        public static void Generate(String LuaPath, String ExcelPath)
        {
            string luaMgrPath = Path.Combine(LuaPath, "FBMapManager.lua");
            StringBuilder sbMgr = new StringBuilder();
            sbMgr.Append("local FBMapManager = BaseClass(\"FBMapManager\", Singleton) \n");
            sbMgr.Append("local function __init(self)\n");

            foreach (string filePath in Directory.GetFiles(ExcelPath))
            {
                if (Path.GetExtension(filePath) != ".xlsx")
                {
                    continue;
                }

                if (Path.GetFileName(filePath).StartsWith("~"))
                {
                    continue;
                }

                if (ExporterJsonConfig.ignoreFiles.Contains(Path.GetFileName(filePath)))
                {
                    continue;
                }

                string filename = Path.GetFileNameWithoutExtension(filePath).ToLower();
                string filenameUpperFirst = filename.Substring(0, 1).ToUpper() + filename.Substring(1);

                string luaUtilPath = Path.Combine(LuaPath, $"{filename}Util.lua");

                sbMgr.Append($"\trequire(\"fb.{filename}Util\"):GetInstance()\n");


                using (FileStream txt = new FileStream(luaUtilPath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(txt))
                    {
                        StringBuilder sbUtil = new StringBuilder();
                        sbUtil.Append($"local {filename}Util = BaseClass(\"{filename}Util\", Singleton)\n");
                        sbUtil.Append("local function __init(self)\n");
                        sbUtil.Append("\tself.map = {}\n");
                        sbUtil.Append($"\tlocal {filename}TB = require(\"fb.{filename}TB\")\n");
                        sbUtil.Append($"\tlocal tbbuf = FBUtil:GetInstance():GetFB(\"{filename}\")\n");
                        sbUtil.Append($"\tlocal tbObj = {filename}TB.GetRootAs{filename}TB(tbbuf, 0)\n");

                        sbUtil.Append($"\tfor i = 1, tbObj:{filenameUpperFirst}TRSLength() do\n");
                        sbUtil.Append($"\t\tlocal trObj = tbObj:{filenameUpperFirst}TRS(i);\n");
                        sbUtil.Append($"\t\tself.map[trObj:_id()] = trObj\n");
                        sbUtil.Append("\tend\n");//end for

                        sbUtil.Append("end\n"); //end init

                        sbUtil.Append("local function GetByID(self, id)\n");
                        sbUtil.Append("\treturn self.map[id]\n");
                        sbUtil.Append("end\n");

                        sbUtil.Append("local function GetAll(self)\n");
                        sbUtil.Append("\treturn self.map\n");
                        sbUtil.Append("end\n");

                        sbUtil.Append($"{filename}Util.__init = __init\n");
                        sbUtil.Append($"{filename}Util.GetByID = GetByID\n");
                        sbUtil.Append($"{filename}Util.GetAll = GetAll\n");
                        sbUtil.Append($"return {filename}Util\n");

                        sw.Write(sbUtil);
                        sw.Flush();
                    }
                }

            }

            sbMgr.Append("end\n");
            sbMgr.Append("FBMapManager.__init = __init\n");
            sbMgr.Append("return FBMapManager\n");

            using (FileStream txt = new FileStream(luaMgrPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(txt))
                {
                    sw.Write(sbMgr.ToString());
                    sw.Flush();
                }
            }
        }
    }
}
