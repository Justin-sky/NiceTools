using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceTools
{
    class GenerateTSData
    {

        public static void Generate(String TsPath, String ExcelPath)
        {
            foreach (string filePath in Directory.GetFiles(ExcelPath))
            {
                if (Path.GetExtension(filePath) != ".xlsx") continue;

                if (Path.GetFileName(filePath).StartsWith("~")) continue;

                if (ExporterJsonConfig.ignoreFiles.Contains(Path.GetFileName(filePath))) continue;


                string filename = Path.GetFileNameWithoutExtension(filePath);
                string filenameUpperFirst = filename.Substring(0, 1).ToUpper() + filename.Substring(1);



                XSSFWorkbook xssfWorkbook;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    xssfWorkbook = new XSSFWorkbook(file);


                    string tsdataPath = Path.Combine(TsPath, $"{filename}.ts");
                    StringBuilder sbTR = new StringBuilder();
                    StringBuilder sbTRProp = new StringBuilder();
                    StringBuilder sbTRParams = new StringBuilder();
                    StringBuilder sbTRInit = new StringBuilder();

                    StringBuilder sbTB = new StringBuilder();

                    //生成TR
                    sbTR.Append("import { Singleton } from \"../../framework/common/Singleton\";\n");
                    sbTR.Append($"export class {filenameUpperFirst}TR{{");


                    //生成TB
                    sbTB.Append($"export class {filenameUpperFirst}TB extends Singleton<{filenameUpperFirst}TB>{{ \n");
                    sbTB.Append($"\tpublic trs:Map<number, {filenameUpperFirst}TR> = new Map<number, {filenameUpperFirst}TR>();\n");
                    sbTB.Append("\tconstructor(){\n");
                    sbTB.Append("\t\tsuper();\n");

                    // for (int k = 0; k < xssfWorkbook.NumberOfSheets; ++k)
                    {
                        int k = 0; //只支持一个sheet
                        ISheet sheet = xssfWorkbook.GetSheetAt(k);

                        int cellCount = sheet.GetRow(0).LastCellNum;

                        CellInfo[] cellInfos = new CellInfo[cellCount];

                        for (int i = 0; i < cellCount; i++)
                        {
                            string fieldDesc = ExcelHelper.GetCellString(sheet, 0, i);
                            string fieldName = ExcelHelper.GetCellString(sheet, 1, i);
                            string fieldType = ExcelHelper.GetCellString(sheet, 2, i);
                            cellInfos[i] = new CellInfo() { Name = fieldName, Type = fieldType, Desc = fieldDesc };

                            sbTRProp.Append($"\t public {fieldName}:{GetTSType(fieldType)} ;\n");

                            if (i == 0)
                            {
                                sbTRParams.Append($"{fieldName}:{GetTSType(fieldType)}");
                            }
                            else
                            {
                                sbTRParams.Append($", {fieldName}:{GetTSType(fieldType)}");
                            }
                            sbTRInit.Append($"\t\tthis.{fieldName} = {fieldName};\n");
                        }

                        for (int i = 3; i <= sheet.LastRowNum; ++i)
                        {

                            if (ExcelHelper.GetCellString(sheet, i, 0) == "")
                            {
                                break; ;
                            }

                            IRow row = sheet.GetRow(i);
                            string id = ExcelHelper.GetCellString(row, 0);
                            sbTB.Append($"\t\tthis.trs.set({id}, new {filenameUpperFirst}TR(");

                            for (int j = 0; j < cellCount; ++j)
                            {
                                string fieldValue = ExcelHelper.GetCellString(row, j);
                                string fieldType = cellInfos[j].Type;
                                if (fieldValue == null || fieldValue.Trim() == "")
                                {
                                    fieldValue = GetTSDefaultValue(fieldType);
                                }
                                fieldValue = ExporterJsonConfig.Convert(fieldType, fieldValue);
                                if (j == 0)
                                {
                                    sbTB.Append($"{fieldValue}");

                                }
                                else
                                {
                                    sbTB.Append($", {fieldValue}");
                                }


                            }
                            sbTB.Append($"));\n");

                        }


                        using (FileStream txt = new FileStream(tsdataPath, FileMode.Create))
                        using (StreamWriter sw = new StreamWriter(txt))
                        {
                            //生成TR类
                            sw.WriteLine(sbTR.ToString());
                            sw.WriteLine(sbTRProp.ToString());
                            sw.Write("\tconstructor(");
                            sw.Write(sbTRParams.ToString());
                            sw.WriteLine("){");
                            sw.WriteLine(sbTRInit.ToString());
                            sw.WriteLine("\t}");
                            sw.WriteLine("}\n");



                            //生成TB类
                            sbTB.Append("\t }\n");
                            sbTB.Append("}\n");
                            sw.WriteLine(sbTB.ToString());
                            sw.Flush();
                        }
                    }




                }



            }
        }


        private static string GetTSType(string type)
        {
            switch (type)
            {
                case "int[]":
                case "int32[]":
                case "long[]":
                    return "Array<number>";
                case "string[]":
                    return "Array<string>";
                case "int":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                    return "number";
                case "string":
                    return "string";
                default:
                    throw new Exception($"不支持此类型: {type}");
            }
        }

        private static string GetTSDefaultValue(string type)
        {
            switch (type)
            {
                case "int[]":
                case "int32[]":
                case "long[]":
                case "string[]":
                    return null;
                case "int":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                    return "0";
                case "string":
                    return "";
                default:
                    throw new Exception($"不支持此类型: {type}");
            }
        }



    }
}
