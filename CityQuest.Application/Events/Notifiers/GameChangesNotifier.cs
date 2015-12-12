using Abp.Dependency;
using CityQuest.Events.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Notifiers
{
    public class GameChangesNotifier : IGameChangesNotifier, ISingletonDependency
    {
        #region Events

        public event EventHandler<KeyActivatedMessage> OnKeyActivated;

        public event EventHandler<GameAddedMessage> OnGameAdded;

        public event EventHandler<GameUpdatedMessage> OnGameUpdated;

        public event EventHandler<GameStatusChangedMessage> OnGameStatusChanged;

        public event EventHandler<GameDeletedMessage> OnGameDeleted;

        public event EventHandler<GameDeactivatedMessage> OnGameDeactivated;

        public event EventHandler<GameActivatedMessage> OnGameActivated;

        #endregion

        #region Methods to raise events

        public virtual void RaiseOnKeyActivated(KeyActivatedMessage msg)
        {
            EventHandler<KeyActivatedMessage> handler = OnKeyActivated;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        public virtual void RaiseOnGameAdded(GameAddedMessage msg)
        {
            EventHandler<GameAddedMessage> handler = OnGameAdded;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        public virtual void RaiseOnGameUpdated(GameUpdatedMessage msg)
        {
            EventHandler<GameUpdatedMessage> handler = OnGameUpdated;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        public virtual void RaiseOnGameStatusChanged(GameStatusChangedMessage msg)
        {
            EventHandler<GameStatusChangedMessage> handler = OnGameStatusChanged;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        public virtual void RaiseOnGameDeleted(GameDeletedMessage msg)
        {
            EventHandler<GameDeletedMessage> handler = OnGameDeleted;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        public virtual void RaiseOnGameDeactivated(GameDeactivatedMessage msg)
        {
            EventHandler<GameDeactivatedMessage> handler = OnGameDeactivated;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        public virtual void RaiseOnGameActivated(GameActivatedMessage msg)
        {
            EventHandler<GameActivatedMessage> handler = OnGameActivated;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        #endregion
    }
}
