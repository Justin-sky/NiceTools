namespace NiceTools
{
    class Program
    {
        static void Main(string[] args)
        {

            //生成Json数据， 用于转换flatbuffer
            ExporterJsonConfig jsonExporter = new ExporterJsonConfig();
            jsonExporter.ExportJsonConfig();

        }
    }
}
