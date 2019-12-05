using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebClinicGUI.Models.Users;
using System.IO;
using Syncfusion.XlsIO;
using System;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace WebClinicGUI.Services
{
    public interface IXlsService
    {
        public IActionResult CreateXlsList(IEnumerable<AppUser> list, string name);
    }
    public class XlsService : IXlsService
    {
        private readonly List<string> _cells = new List<string>()
        { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };
        private readonly IStringLocalizer<XlsService> _localizer;

        public XlsService(IStringLocalizer<XlsService> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult CreateXlsList(IEnumerable<AppUser> list, string name)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2013;

                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];

                var row = 0;
                var column = 0;
                if (list.Count() > 0)
                {
                    //first row

                    var properties = list.First().ToRow();
                    column = 0;
                    foreach (var property in properties)
                    {
                        worksheet.Range[GetCellName(column, row )].Text = _localizer[property.Key];
                        column++;
                    }
                    row++;

                    //other rows
                    foreach (var user in list)
                    {
                        properties = user.ToRow();
                        column = 0;
                        foreach (var property in properties)
                        {
                            worksheet.Range[GetCellName(column, row)].Text = _localizer[property.Value];
                            column++;
                        }
                        row++;

                    }
                }

                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                stream.Position = 0;

                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");
                var report_name = _localizer["Report"]+ "_" + _localizer[name] + "_" + DateTime.Now.ToString();
                fileStreamResult.FileDownloadName = $"{report_name}.xlsx";

                return fileStreamResult;
            }

        }
    private string GetCellName(int column, int row)
        {
            return _cells[column] + (row + 1).ToString();
        }
    }

}
