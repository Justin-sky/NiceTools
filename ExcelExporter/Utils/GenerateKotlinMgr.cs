using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceTools
{
    class GenerateKotlinMgr
    {

        public static void Generate(String KotPath, String ExcelPath)
        {
            string kotMgrPath = Path.Combine(KotPath, "FBManager.kt");


            StringBuilder sbMgrHeader = new StringBuilder();
            sbMgrHeader.Append("package fb\n");
            sbMgrHeader.Append("import jodd.util.ClassLoaderUtil \n");
            sbMgrHeader.Append("import kt.scaffold.Application \n");
            sbMgrHeader.Append("import kt.scaffold.ext.filePathJoin \n");
            sbMgrHeader.Append("import java.io.File \n");
            sbMgrHeader.Append("import java.nio.ByteBuffer \n");


            StringBuilder sbMgrBody = new StringBuilder();
            sbMgrBody.Append("object FBManager { \n");


            //生成变量
            foreach (string filePath in Directory.GetFiles(ExcelPath))
            {
                if (Path.GetExtension(filePath) != ".xlsx"){ continue;}
                if (Path.GetFileName(filePath).StartsWith("~")){continue;}
                if (ExporterJsonConfig.ignoreFiles.Contains(Path.GetFileName(filePath))){ continue; }

                string filename = Path.GetFileNameWithoutExtension(filePath).ToLower();
                sbMgrBody.Append($"\tlateinit var {filename}:{filename}TB \n");
            }
            //生成initialize方法
            sbMgrBody.Append("\tfun initialize(){ \n");
            foreach (string filePath in Directory.GetFiles(ExcelPath))
            {
                if (Path.GetExtension(filePath) != ".xlsx") { continue; }
                if (Path.GetFileName(filePath).StartsWith("~")) { continue; }
                if (ExporterJsonConfig.ignoreFiles.Contains(Path.GetFileName(filePath))) { continue; }

                string filename = Path.GetFileNameWithoutExtension(filePath).ToLower();
                sbMgrBody.Append($"\t\t{filename} = {filename}TB.getRootAs{filename}TB(ByteBuffer.wrap(File(getPath(\"{filename}.bin\")).readBytes()))\n");
            }
            sbMgrBody.Append("\t} \n"); //end initialize

            //生成getPath方法
            sbMgrBody.Append("\tprivate fun getPath(name:String):String{ \n");
            sbMgrBody.Append("\t\tvar fbPath = filePathJoin(Application.appHome, \"fb\",name) \n");
            sbMgrBody.Append("\t\tif (File(fbPath).exists().not()){ \n");
            sbMgrBody.Append("\t\t\tfbPath = ClassLoaderUtil.getDefaultClassLoader().getResource(\"fb/$name\")!!.path \n");
            sbMgrBody.Append("\t\t}\n");
            sbMgrBody.Append("\t\t return fbPath\n");
            sbMgrBody.Append("\t} \n"); //edn get path

            sbMgrBody.Append("}\n"); //end class

            using (FileStream txt = new FileStream(kotMgrPath, FileMode.Create))
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
