using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceTools
{
    class GenerateTSMgr
    {

        public static void Generate(String TsPath, String ExcelPath)
        {
            string tsMgrPath = Path.Combine(TsPath, "ExcelManager.ts");
            StringBuilder sbMgrHeader = new StringBuilder();
            sbMgrHeader.Append("import { Singleton } from \"../../framework/common/Singleton\";\n");


            StringBuilder sbMgrBody = new StringBuilder();
            sbMgrBody.Append("export class ExcelManager extends Singleton<ExcelManager>{\n");
            sbMgrBody.Append("\tconstructor(){\n");
            sbMgrBody.Append("\t\tsuper();\n");

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

                string filename = Path.GetFileNameWithoutExtension(filePath);
                string filenameUpperFirst = filename.Substring(0, 1).ToUpper() + filename.Substring(1);

                sbMgrHeader.Append($"import {{ {filenameUpperFirst}TB }} from \"./{filename}\";\n");
                sbMgrBody.Append($"\t\t{filenameUpperFirst}TB.Instance({filenameUpperFirst}TB);\n");
            }


            sbMgrBody.Append("\t}\n");
            sbMgrBody.Append(" }\n");

            using (FileStream txt = new FileStream(tsMgrPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(txt))
                {
                    sw.Write(sbMgrHeader.ToString());
                    sw.Write(sbMgrBody.ToString());
                    sw.Flush();
                }
            }
        }

    }
}
