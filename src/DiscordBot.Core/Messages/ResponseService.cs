using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeaBagBot.Core.Entities;
using TeaBagBot.Core.Storage;

namespace TeaBagBot.Core.Messages
{
    public class ResponseService
    {
        private readonly IDataStorage _storage;

        public ResponseService(IDataStorage storage)
        {
            _storage = storage;
        }

        public IReadOnlyCollection<TeaBagResponse> Load()
        {
            try
            {
                return _storage.RestoreObject<IEnumerable<TeaBagResponse>>("Responses").ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't load responses.", ex);
            }
        }

        public void Save(IEnumerable<TeaBagResponse> responses)
        {
            try
            {
                _storage.StoreObject(responses, "Responses");
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't save responses.", ex);
            }
        }
    }
}
