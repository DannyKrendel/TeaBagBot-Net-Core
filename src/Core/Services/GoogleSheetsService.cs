using Google.Apis.Auth.OAuth2;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaBagBot.Models;
using Data = Google.Apis.Sheets.v4.Data;

namespace TeaBagBot.Services
{
    public class GoogleSheetsService
    {
        private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string _appName = "Spreadsheet";
        private readonly string _path = @"C:\Users\Danny\Source\Repos\TeaBagBot\Config\GoogleSheetsApi.json";
        private SheetsService _service;

        public GoogleSheetsService()
        {
            GoogleCredential credential;
            using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(_scopes);
            }

            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _appName
            });
        }

        public async Task<Sheet> GetSheetAsync(string spreadsheetId, string sheet)
        {
            var dataList = new List<IList<string>>();
            var dataFilters = new List<DataFilter>();
            var requestBody = new GetSpreadsheetByDataFilterRequest();
            requestBody.DataFilters = dataFilters;
            requestBody.IncludeGridData = true;

            var request = _service.Spreadsheets.GetByDataFilter(requestBody, spreadsheetId);
            var response = await request.ExecuteAsync();

            return response.Sheets.FirstOrDefault(s => s.Properties.Title == sheet);
        }
    }
}
