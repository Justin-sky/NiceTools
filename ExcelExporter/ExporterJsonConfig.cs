﻿using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceTools
{
    public class ExporterJsonConfig
    {
        private const string ExcelPath = "../Excel";
        private const string JsonPath = "../output_json";
        private const string IdlPath = "../output_idl";
        private const string LuaPath = "../output_lua/fb";
        private const string TsPath = "../output_ts";
        private const string KotlinPath = "../output_kotlin";

        public static List<string> ignoreFiles = new List<string>()
        {

        };

        //变量以下划线开头
        private string underscope_field(string field)
        {
            string res = field;

            if (!field.StartsWith("_"))
            {
                res = $"_{field}";
            }
            return res;
        }

        public void ExportJsonConfig()
        {
            try
            {
                ExportAll(JsonPath);

                ExportAllIDLS(IdlPath);

                ExportData();

                GenerateLuaUtilsFiles.Generate(LuaPath, ExcelPath);

                GenerateTSMgr.Generate(TsPath, ExcelPath);

                GenerateTSData.Generate(TsPath, ExcelPath);

                GenerateKotlinMgr.Generate(KotlinPath, ExcelPath);

                Console.WriteLine($"导出服务端配置完成!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private void ExportData()
        {
            string exePath = Path.GetFullPath("../");

            foreach (string filePath in Directory.GetFiles(JsonPath))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string argvB      = $"-b -o output_bin output_idl/{fileName}.fbs  output_json/{fileName}.txt";
                string argvCsharp = $"-n --gen-onefile  -o output_csharp output_idl/{fileName}.fbs  output_json/{fileName}.txt";
                string argvLua    = $"-l -o output_lua output_idl/{fileName}.fbs  output_json/{fileName}.txt";
                string argvTS = $"--ts --no-fb-import --no-ts-reexport  -o output_fbts output_idl/{fileName}.fbs  output_json/{fileName}.txt";
                string argvCotlin = $"--kotlin -o output_kotlin output_idl/{fileName}.fbs  output_json/{fileName}.txt";

                ProcessHelper.Run( $"{exePath}/flatc.exe",  argvB,  exePath);
                ProcessHelper.Run( $"{exePath}/flatc.exe", argvCsharp,  exePath);
                ProcessHelper.Run( $"{exePath}/flatc.exe", argvLua,  exePath);
                ProcessHelper.Run( $"{exePath}/flatc.exe", argvTS,  exePath);
                ProcessHelper.Run( $"{exePath}/flatc.exe", argvCotlin,  exePath);

                Console.WriteLine(argvB);
            }
            
        }

        public void ExportAllIDLS(string exportDir)
        {
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

                if (ignoreFiles.Contains(Path.GetFileName(filePath)))
                {
                    continue;
                }

                ExportIDL(filePath, exportDir);
                Console.WriteLine($"生成{Path.GetFileName(filePath)}Schema");
            }
     
        }

        private void ExportIDL(string fileName, string exportDir)
        {
            XSSFWorkbook xssfWorkbook;
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                xssfWorkbook = new XSSFWorkbook(file);
            }

            string protoName = Path.GetFileNameWithoutExtension(fileName).ToLower();

            string exportPath = Path.Combine(exportDir, $"{protoName}.fbs");

            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(txt))
            {
                StringBuilder sb = new StringBuilder();
                ISheet sheet = xssfWorkbook.GetSheetAt(0);

                sb.Append("namespace fb; \n");

                //gen TB
                sb.Append($"table {protoName}TB\n");
                sb.Append("{\n");
                sb.Append($"\t {protoName}TRS:[{protoName}TR];\n");
                sb.Append("}\n\n"); //end TB

                //gen TR
                sb.Append($"table {protoName}TR\n");
                sb.Append("{\n");

                int cellCount = sheet.GetRow(0).LastCellNum;

                for (int i = 0; i < cellCount; i++)
                {
                    string fieldDesc = ExcelHelper.GetCellString(sheet, 0, i);

                    if (fieldDesc.StartsWith("#"))
                    {
                        continue;
                    }

                    string fieldName = ExcelHelper.GetCellString(sheet, 1, i).ToLower();


                    string fieldType = ExcelHelper.GetCellString(sheet, 2, i);
                    if (fieldType == "" || fieldName == "")
                    {
                        continue;
                    }
                    string idlType = Convert(fieldType);

                    //key
                    if (fieldName.Equals("_id"))
                    {
                        idlType += "(key)";
                    }

                    sb.Append($"\t {underscope_field(fieldName)}:{idlType};\n");
                }

                sb.Append("}\n"); //end TR

                sb.Append($"root_type {protoName}TB;");

                sw.Write(sb.ToString());
            }
        }

        private void ExportAll(string exportDir)
        {


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

                if (ignoreFiles.Contains(Path.GetFileName(filePath)))
                {
                    continue;
                }
           

                Export(filePath, exportDir);
            }


            Console.WriteLine("所有表导表完成");
      
        }

        private void Export(string fileName, string exportDir)
        {
            XSSFWorkbook xssfWorkbook;
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                xssfWorkbook = new XSSFWorkbook(file);
            }

            string protoName = Path.GetFileNameWithoutExtension(fileName).ToLower();

            Console.WriteLine($"{protoName}导表开始");
            string exportPath = Path.Combine(exportDir, $"{protoName}.txt");
            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(txt))
            {
                sw.WriteLine('{');
                sw.WriteLine($"\"{protoName}TRS\":[");
                for (int i = 0; i < xssfWorkbook.NumberOfSheets; ++i)
                {
                    ISheet sheet = xssfWorkbook.GetSheetAt(i);
                    ExportSheet(sheet, sw);
                }
                sw.WriteLine("]}");
            }

            Console.WriteLine($"{protoName}导表完成");
        }

        private void ExportSheet(ISheet sheet, StreamWriter sw)
        {
            int cellCount = sheet.GetRow(0).LastCellNum;

            CellInfo[] cellInfos = new CellInfo[cellCount];

            for (int i = 0; i < cellCount; i++)
            {
                string fieldDesc = ExcelHelper.GetCellString(sheet, 0, i);
                string fieldName = ExcelHelper.GetCellString(sheet, 1, i);
                string fieldType = ExcelHelper.GetCellString(sheet, 2, i);
                cellInfos[i] = new CellInfo() { Name = fieldName, Type = fieldType, Desc = fieldDesc };
            }

            for (int i = 3; i <= sheet.LastRowNum; ++i)
            {
                StringBuilder sb = new StringBuilder();
                if (ExcelHelper.GetCellString(sheet, i, 0) == "")
                {
                    
                    break; ;
                }
                if(i>3)
                    sb.Append(",");

                IRow row = sheet.GetRow(i);
                sb.Append("{");
                for (int j = 0; j < cellCount; ++j)
                {

                    string fieldValue = ExcelHelper.GetCellString(row, j);
                    if (fieldValue == "")
                    {
                        continue;
                    }

                    if (j > 0 )
                    {
                        sb.Append(",");
                    }

                    string fieldName = cellInfos[j].Name.ToLower();

                    string fieldType = cellInfos[j].Type;
                    sb.Append($"\"{underscope_field(fieldName)}\":{Convert(fieldType, fieldValue)}");
                }

                sb.Append("}");
          
                
                sw.WriteLine(sb.ToString());
            }
        }

        public static string Convert(string type, string value)
        {
            switch (type)
            {
                case "int[]":
                case "int32[]":
                case "long[]":
                    return $"[{value}]";
                case "string[]":
                    return $"[{value}]";
                case "int":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                    return value;
                case "string":
                    return $"\"{value}\"";
                default:
                    throw new Exception($"不支持此类型: {type}");
            }
        }

        public static string Convert(string type)
        {
            switch (type)
            {
                case "int[]":
                    return "[int]";
                case "int32[]":
                    return "[int32]";
                case "long[]":
                    return "[long]";
                case "string[]":
                    return "[string]";
                case "int":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                case "string":
                    return type;
                default:
                    throw new Exception($"不支持此类型: {type}");
            }
        }

    }
}
