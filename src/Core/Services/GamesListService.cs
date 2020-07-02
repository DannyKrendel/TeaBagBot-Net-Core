using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaBagBot.Models;

namespace TeaBagBot.Services
{
    public class GamesListService
    {
        private readonly string _spreadsheetId = "1JvyjC-BrHP7WMD9BKd6xSToQmQ5rZNj7la2jcbW_Pe0";
        private readonly string _sheet = "Список игр";
        private readonly GoogleSheetsService _googleSheetsService;

        public GamesListService(GoogleSheetsService googleSheetsService)
        {
            _googleSheetsService = googleSheetsService;
        }

        public async Task<GameInfo> FindGameAsync(string name)
        {
            GameInfo gameInfo = null;
            var sheet = await _googleSheetsService.GetSheetAsync(_spreadsheetId, _sheet);

            var data = sheet.Data.FirstOrDefault();

            if (data == null)
                return null;

            foreach (var rowData in data.RowData)
            {
                if (rowData.Values[0].FormattedValue.ToLower() == name.ToLower())
                {
                    gameInfo = new GameInfo
                    {
                        Name = rowData.Values[0].FormattedValue,
                        Status = rowData.Values[1].FormattedValue,
                        Description = rowData.Values[2].FormattedValue,
                        Url = string.IsNullOrEmpty(rowData.Values[0].Hyperlink) ? null : rowData.Values[0].Hyperlink
                    };
                    break;
                }
            }

            return gameInfo;
        }
    }
}
