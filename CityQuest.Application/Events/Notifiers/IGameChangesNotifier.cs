using Abp.Dependency;
using CityQuest.ApplicationServices.GameModule.Games.Dtos;
using CityQuest.ApplicationServices.GameModule.GameStatuses.Dtos;
using CityQuest.Events.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Notifiers
{
    public interface IGameChangesNotifier : ITransientDependency
    {
        event EventHandler<KeyActivatedMessage> OnKeyActivated;
        event EventHandler<GameAddedMessage> OnGameAdded;
        event EventHandler<GameUpdatedMessage> OnGameUpdated;
        event EventHandler<GameStatusChangedMessage> OnGameStatusChanged;
        event EventHandler<GameDeletedMessage> OnGameDeleted;
        event EventHandler<GameDeactivatedMessage> OnGameDeactivated;
        event EventHandler<GameActivatedMessage> OnGameActivated;

        void RaiseOnKeyActivated(KeyActivatedMessage msg);
        void RaiseOnGameAdded(GameAddedMessage msg);
        void RaiseOnGameUpdated(GameUpdatedMessage msg);
        void RaiseOnGameStatusChanged(GameStatusChangedMessage msg);
        void RaiseOnGameDeleted(GameDeletedMessage msg);
        void RaiseOnGameDeactivated(GameDeactivatedMessage msg);
        void RaiseOnGameActivated(GameActivatedMessage msg);
    }
}
