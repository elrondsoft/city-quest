using CityQuest.ApplicationServices.GameModule.GamesLight.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Events.Messages
{
    public class KeyActivatedMessage
    {
        public GameLightDto Game { get; set; }
        public long? UserId { get; set; }
        public string Key { get; set; }

        public KeyActivatedMessage(GameLightDto game, long? userId, string key)
        {
            Game = game;
            UserId = userId;
            Key = key;
        }
    }
}
