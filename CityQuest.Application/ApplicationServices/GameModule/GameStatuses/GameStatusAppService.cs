using Abp.Application.Services.Dto;
using CityQuest.ApplicationServices.GameModule.GameStatuses.Dtos;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.Entities.GameModule.Games.GameStatuses;
using CityQuest.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GameStatuses
{
    public class GameStatusAppService : IGameStatusAppService
    {
        private IGameStatusRepository GameStatusRepository { get; set; }

        public GameStatusAppService(IGameStatusRepository gameStatusRepository)
        {
            GameStatusRepository = gameStatusRepository;
        }

        public RetrieveAllOutput<GameStatusDto, long> RetrieveAll(RetrieveAllGameStatusesInput input)
        {
            GameStatusRepository.Includes.Add(r => r.LastModifierUser);
            GameStatusRepository.Includes.Add(r => r.CreatorUser);

            IList<GameStatus> gameStatusEntities = GameStatusRepository.GetAll().OrderBy(r => r.Name).ToList();

            IList<GameStatusDto> result = gameStatusEntities.MapIList<GameStatus, GameStatusDto>();

            GameStatusRepository.Includes.Clear();

            return new RetrieveAllOutput<GameStatusDto, long>()
            {
                RetrievedEntities = result
            };
        }

        public RetrieveAllGameStatusesLikeComboBoxesOutput RetrieveAllGameStatusesLikeComboBoxes(RetrieveAllGameStatusesLikeComboBoxesInput input)
        {
            IReadOnlyList<ComboboxItemDto> gameStatusesLikeComboBoxes = GameStatusRepository.GetAll().ToList()
                .Select(r => new ComboboxItemDto(r.Id.ToString(), r.Name))
                .OrderBy(r => r.DisplayText)
                .ToList();

            return new RetrieveAllGameStatusesLikeComboBoxesOutput()
            {
                Items = gameStatusesLikeComboBoxes
            };
        }
    }
}
