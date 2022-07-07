using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace HRCentralDataToSharePoint
{
    public class ExcelHelper
    {

        public static List<Employee> GetEmployeeDataList()
        {
            using (HRDBContext context = new HRDBContext())
            {
                SqlParameter[] objParams = { new SqlParameter("@SYNCH_ACCOUNT", Configer.MACHINE_ID) };
                var sp = "EXEC dbo.SP_TRANSFER_HR_CENTRAL_DB_DATA @SYNCH_ACCOUNT";
                return context.Database.SqlQuery<Employee>(sp, objParams).ToList();
            }
        }

        public static DataTable GetEmployeeDataTable()
        {
            var conn = "HRDBContext";
            var sql = "EXEC dbo.SP_TRANSFER_HR_CENTRAL_DB_DATA @SYNCH_ACCOUNT";
            SqlParameter[] objParams = { new SqlParameter("@SYNCH_ACCOUNT", Configer.MACHINE_ID) };
            DataTable data = ExecuteToDataTable(conn, sql, objParams);
            return data;
        }


        public static DataTable ExecuteToDataTable(string con, string ssql, SqlParameter[] parameters = null)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[con].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(ssql, conn);
                command.Parameters.AddRange(parameters);
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.SelectCommand = command;
                adapter.Fill(table);
                conn.Close();
                conn.Dispose();
                return table;
            }
        }


        public static byte[] ExportWithTemp(DataTable source, string templateFile, string outputFile)
        {
            try
            {
                MemoryStream sw = new MemoryStream();
                using (FileStream file = new FileStream(templateFile, FileMode.Open, FileAccess.Read))
                {
                    var workbook = new XSSFWorkbook(file);
                    var sheet = workbook.GetSheetAt(0);

                    int rowIndex = 0;
                    foreach (DataRow item in source.Rows)
                    {
                        rowIndex++;
                        var row = sheet.CreateRow(rowIndex);
                        for (int colIndex = 0; colIndex < source.Columns.Count; colIndex++)
                        {
                            row.CreateCell(colIndex).SetCellValue(ObjToString(item[colIndex]));
                        }
                    }
                    workbook.Add(sheet);
                    FileStream file2007 = new FileStream(outputFile, FileMode.Create);
                    workbook.Write(file2007);
                    file2007.Close();
                    workbook.Write(sw);
                    return sw.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static byte[] ExportWithTemp<T>(IList<T> source, string templateFile, string outputFile)
        {
            try
            {
                MemoryStream sw = new MemoryStream();
                using (FileStream file = new FileStream(templateFile, FileMode.Open, FileAccess.Read))
                {
                    var workbook = new XSSFWorkbook(file);
                    var sheet = workbook.GetSheetAt(0);

                    int rowIndex = 0;
                    foreach (var item in source)
                    {
                        rowIndex++;
                        int colIndex = 0;
                        var row = sheet.CreateRow(rowIndex);
                        foreach (System.Reflection.PropertyInfo p in item.GetType().GetProperties())
                        {
                            row.CreateCell(colIndex).SetCellValue(ObjToString(p.GetValue(item)));
                            colIndex++;
                        }
                    }
                    workbook.Add(sheet);
                    FileStream file2007 = new FileStream(outputFile, FileMode.Create);
                    workbook.Write(file2007);
                    file2007.Close();
                    workbook.Write(sw);
                    return sw.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static byte[] GetDataBytesFromDB()
        {
            var tempFilePath = Configer.BaseDirectory + @"Template.xlsx";
            var outputFolderPath = Configer.BaseDirectory + @"TempData\";
            var outputFilePath = outputFolderPath + Configer.SharePointExcelName + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + @".xlsx";

            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            int days = 0;
            if (int.TryParse(Configer.DaysToKeepFiles, out days))
            {
                ClearFilesBeforeGivenDays(outputFolderPath, days);
            }

            var data = GetEmployeeDataTable();
            List<string> columns = new List<string> { "cwid", "bi_last_name", "bi_first_name", "cost_center", "staff_type" };
            foreach (string column in columns)
            {
                if (data.Columns.Contains(column))
                {
                    data.Columns[column].SetOrdinal(columns.IndexOf(column));
                }
            }
            return ExportWithTemp(data, tempFilePath, outputFilePath);
        }


        private static void ClearFilesBeforeGivenDays(string folderPath, int days)
        {

            DirectoryInfo folder = new DirectoryInfo(folderPath);
            foreach (var file in folder.GetFiles())
            {
                DateTime creatTime = file.LastWriteTime;
                if (DateTime.Now.AddDays(-days).Subtract(creatTime).TotalSeconds > 0)
                {
                    file.Delete();
                }
            }
        }


        public static string ObjToString(object obj)
        {
            return obj == null || obj == DBNull.Value ? "" : obj.ToString();
        }



        public static void ImportExcel(string filePath)
        {
            using (FileStream fsRead = System.IO.File.OpenRead(filePath))
            {
                MemoryStream ms = new MemoryStream();
                fsRead.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
            }
        }


        public static byte[] ConvertFileToBinary(string Path)
        {
            using (FileStream stream = new FileInfo(Path).OpenRead())
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                return buffer;
            }
        }
    }
}
