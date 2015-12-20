using Abp.Domain.Uow;
using CityQuest.ApplicationServices.GameModule.Games.Dtos;
using CityQuest.ApplicationServices.GameModule.GamesLight.Dtos;
using CityQuest.ApplicationServices.GameModule.Keys.Dtos;
using CityQuest.CityQuestPolicy.GameModule.Games;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Keys;
using CityQuest.Events.Messages;
using CityQuest.Events.Notifiers;
using CityQuest.Exceptions;
using CityQuest.Mapping;
using CityQuest.Runtime.Sessions;
using CityQuest.Services.SafeGuidGenerationServices.KeyGenerationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Keys
{
    public class KeyAppService : IKeyAppService
    {
        #region Injected Dependencies

        private IUnitOfWorkManager UowManager { get; set; }
        private ICityQuestSession Session { get; set; }
        private IKeyRepository KeyRepository { get; set; }
        private IGameRepository GameRepository { get; set; }
        private IGamePolicy GamePolicy { get; set; }
        private IKeyGenerationService KeyGenerationService { get; set; }
        private IGameChangesNotifier GameChangesNotifier { get; set; }

        #endregion

        #region ctors

        public KeyAppService(
            IUnitOfWorkManager uowManager,
            ICityQuestSession cityQuestSession,
            IKeyRepository keyRepository, 
            IGameRepository gameRepository, 
            IGamePolicy gamePolicy, 
            IKeyGenerationService keyGenerationService,
            IGameChangesNotifier gameChangesNotifier)
        {
            UowManager = uowManager;
            Session = cityQuestSession;
            KeyRepository = keyRepository;
            GameRepository = gameRepository;
            GamePolicy = gamePolicy;
            KeyGenerationService = keyGenerationService;
            GameChangesNotifier = gameChangesNotifier;
        }

        #endregion

        public GenerateKeysForGameOutput GenerateKeysForGame(GenerateKeysForGameInput input)
        {
            Game gameEntity = GameRepository.Get(input.GameId);

            if (gameEntity == null)
                throw new CityQuestItemNotFoundException(CityQuestConsts.CityQuestItemNotFoundExceptionMessageBody, "\"Game\"");

            if (!GamePolicy.CanGenerateKeysForEntity(gameEntity) || 
                !(input.Count > 0 && input.Count <= CityQuestConsts.MaxCountForKeyGeneration))
                throw new CityQuestPolicyException(CityQuestConsts.CQPolicyExceptionCreateDenied, "\"Key\"");

            IList<string> newKeys = KeyGenerationService.Generate(input.Count);

            IList<Key> newKeyEntities = new List<Key>(input.Count);
            foreach (string newKey in newKeys)
                newKeyEntities.Add(new Key() { KeyValue = newKey, GameId = input.GameId });

            KeyRepository.AddRange(newKeyEntities);

            return new GenerateKeysForGameOutput()
            {
                Keys = newKeys
            };
        }

        public ActivateKeyOutput ActivateKey(ActivateKeyInput input)
        {
            IList<Key> keys = KeyRepository.GetAll().Where(r => r.KeyValue == input.Key && r.OwnerUserId == null).ToList();

            if (keys.Count != 1)
                throw new KeyActivationException(input.Key);

            Key keyEntity = keys.Single();
            keyEntity.OwnerUserId = Session.UserId;
            KeyRepository.Update(keyEntity);

            GameRepository.Includes.Add(r => r.Location);
            GameRepository.Includes.Add(r => r.GameStatus);
            GameRepository.Includes.Add(r => r.GameTasks);
            GameRepository.Includes.Add(r => r.LastModifierUser);
            GameRepository.Includes.Add(r => r.CreatorUser);

            GameLightDto activatedGame = GameRepository.Get(keyEntity.GameId).MapTo<GameLightDto>();

            GameRepository.Includes.Clear();

            UowManager.Current.Completed += (sender, e) =>
            {
                GameChangesNotifier.RaiseOnKeyActivated(new KeyActivatedMessage(activatedGame, Session.UserId, input.Key));
            };

            return new ActivateKeyOutput() { ActivatedGame = activatedGame };
        }
    }
}
