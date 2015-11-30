using CityQuest.Entities.GameModule.Keys;
using CityQuest.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Services.SafeGuidGenerationServices.KeyGenerationServices
{
    public class KeyGenerationService : IKeyGenerationService
    {
        private int _maxCountOfFails = 3;

        protected IKeyRepository KeyRepository { get; set; }

        public KeyGenerationService(IKeyRepository keyRepository)
        {
            KeyRepository = keyRepository;
        }

        public IList<string> Generate(int count)
        {
            List<string> newGuids = GenerateNewGuids(count).Distinct().ToList();
            SeparateBadGuids(newGuids);
            int failsCounter = 0;
            while (newGuids.Count != count && failsCounter < _maxCountOfFails)
            {
                failsCounter++;
                List<string> reGeneratedGuids = GenerateNewGuids(count - newGuids.Count).Distinct().ToList();
                SeparateBadGuids(reGeneratedGuids);
                newGuids.AddRange(reGeneratedGuids);
            }
            if (failsCounter >= _maxCountOfFails)
            {
                throw new SafeGuidGenerationServiceException();
            }
            return newGuids;
        }

        protected void SeparateBadGuids(List<string> newGuids)
        {
            var badGuids = KeyRepository.GetAll().Where(r => newGuids.Contains(r.KeyValue)).Select(r => r.KeyValue);
            newGuids.RemoveRange(badGuids);
        }

        private IList<string> GenerateNewGuids(int count)
        {
            IList<string> newGuids = new List<string>(count);

            for (var i = 0; i < count; i++)
                newGuids.Add(Guid.NewGuid().ToString());

            return newGuids;
        }
    }
}
