﻿using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Events.HandlerFactory;
using BusinessLogic.Logic.PlayerAchievements;
using BusinessLogic.Logic.Players;
using BusinessLogic.Models;
using BusinessLogic.Models.Achievements;
using BusinessLogic.Models.User;
using UI.Attributes.Filters;
using UI.Controllers.Helpers;
using UI.Mappers;
using UI.Models.Achievements;

namespace UI.Controllers
{
    [RoutePrefix("achievements")]
    public partial class AchievementController : BaseController
    {

        private readonly IPlayerAchievementRetriever _playerAchievementRetriever;
        private readonly IPlayerRetriever _playerRetriever;
        private readonly PlayerAchievementToPlayerAchievementViewModelMapper _playerAchievementToPlayerAchievementViewModelMapper;
        private readonly AchievementToAchievementViewModelMapper _achievementToAchievementViewModelMapper;

        public AchievementController(IPlayerAchievementRetriever playerAchievementRetriever,
            IPlayerRetriever playerRetriever,
            PlayerAchievementToPlayerAchievementViewModelMapper playerAchievementToPlayerAchievementViewModelMapper,
            AchievementToAchievementViewModelMapper achievementToAchievementViewModelMapper)
        {
            _playerAchievementRetriever = playerAchievementRetriever;
            _playerRetriever = playerRetriever;
            _playerAchievementToPlayerAchievementViewModelMapper = playerAchievementToPlayerAchievementViewModelMapper;
            _achievementToAchievementViewModelMapper = achievementToAchievementViewModelMapper;
        }

        [Route("")]
        public virtual ActionResult Index()
        {
            var achievements = AchievementFactory.GetAchievements();
            var model = new AchievementListViewModel
            {
                Achievements = achievements.Select(a => _achievementToAchievementViewModelMapper.Map(a)).OrderByDescending(a => a.Winners.Count).ThenBy(a=>a.Name).ToList()
            };

            return View(MVC.Achievement.Views.Index, model);

        }

        [Route("{achievementId}/currentplayer")]
        [UserContext]
        public virtual ActionResult DetailsForCurrentUser(AchievementId achievementId, ApplicationUser currentUser)
        {
            var playerId = _playerRetriever.GetPlayerIdForCurrentUser(currentUser.Id, currentUser.CurrentGamingGroupId);

            var playerAchievement = _playerAchievementRetriever.GetPlayerAchievement(playerId, achievementId);

            if (playerAchievement != null)
            {
                return PlayerAchievement(achievementId,playerId);
            }

            playerAchievement = new PlayerAchievement
            {
                AchievementId = achievementId,
                PlayerId = playerId
            };
            var model = _playerAchievementToPlayerAchievementViewModelMapper.Map(playerAchievement);
            return View(MVC.Achievement.Views.Details, model);
        }

        [Route("{achievementId}")]
        [UserContext(RequiresGamingGroup = false)]
        public virtual ActionResult Details(AchievementId achievementId, ApplicationUser currentUser)
        {
            if (currentUser != null && currentUser.CurrentGamingGroupId > 0)
            {
                return DetailsForCurrentUser(achievementId, currentUser);
            }

            var playerAchievement = new PlayerAchievement
            {
                AchievementId = achievementId,
                PlayerId = 0
            };
            var model = _playerAchievementToPlayerAchievementViewModelMapper.Map(playerAchievement);
            return View(MVC.Achievement.Views.Details, model);
        }

        [Route("{achievementId}/player/{playerId}")]
        public virtual ActionResult PlayerAchievement(AchievementId achievementId, int playerId)
        {
            var playerAchievement = _playerAchievementRetriever.GetPlayerAchievement(playerId, achievementId);
            var model = _playerAchievementToPlayerAchievementViewModelMapper.Map(playerAchievement);
            return View(MVC.Achievement.Views.Details, model);
        }
    }
}