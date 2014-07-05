﻿using BusinessLogic.DataAccess;
using BusinessLogic.Models;
using BusinessLogic.Models.Games;
using BusinessLogic.Models.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Tests.IntegrationTests.LogicTests
{
    public class IntegrationTestBase
    {
        protected List<PlayedGame> testPlayedGames = new List<PlayedGame>();
        protected GameDefinition testGameDefinition;
        protected Player testPlayer1;
        protected string testPlayer1Name = "testPlayer1";
        protected Player testPlayer2;
        protected string testPlayer2Name = "testPlayer2";
        protected Player testPlayer3;
        protected string testPlayer3Name = "testPlayer3";
        protected Player testPlayer4;
        protected string testPlayer4Name = "testPlayer4";
        protected Player testPlayer5;
        protected string testPlayer5Name = "testPlayer5";
        protected Player testPlayer6;
        protected string testPlayer6Name = "testPlayer6";
        protected string testGameName = "this is a test game name 123abc";
        protected string testGameDescription = "this is a test game description 123abc";
        protected string testApplicationUserNameForUserWithDefaultGamingGroup = "username with default gaming group";
        protected string testApplicationUserNameForUserWithoutDefaultGamingGroup = "username without default gaming group";
        protected string testGamingGroupName = "this is a test gaming group name 123abc";
        protected ApplicationUser testApplicationUserWithDefaultGamingGroup;
        protected ApplicationUser testApplicationUserWithoutDefaultGamingGroup;
        protected GamingGroup gamingGroup;
        
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            using(NemeStatsDbContext dbContext = new NemeStatsDbContext())
            {
                CleanUpTestData();

                int gamingGroupId = SaveDefaultGamingGroup(dbContext);
                SaveApplicationUsers(dbContext, gamingGroupId);
                SaveGameDefinitions(dbContext, gamingGroupId);
                SavePlayers(dbContext, gamingGroupId);
                CreatePlayedGames(dbContext);
            }
        }

        private void CreatePlayedGames(NemeStatsDbContext dbContext)
        {
            PlayedGameLogic playedGameLogic = new PlayedGameRepository(dbContext);

            List<Player> players = new List<Player>() { testPlayer1, testPlayer2 };
            List<int> playerRanks = new List<int>() { 1, 1 };
            PlayedGame playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);

            players = new List<Player>() { testPlayer1, testPlayer2, testPlayer3 };
            playerRanks = new List<int>() { 1, 2, 3 };
            playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);

            players = new List<Player>() { testPlayer1, testPlayer3, testPlayer2 };
            playerRanks = new List<int>() { 1, 2, 3 };
            playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);

            players = new List<Player>() { testPlayer3, testPlayer1 };
            playerRanks = new List<int>() { 1, 2 };
            playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);

            //make player4 beat player 1 three times
            players = new List<Player>() { testPlayer4, testPlayer1, testPlayer2, testPlayer3 };
            playerRanks = new List<int>() { 1, 2, 3, 4 };
            playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);

            players = new List<Player>() { testPlayer4, testPlayer1 };
            playerRanks = new List<int>() { 1, 2 };
            playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);

            players = new List<Player>() { testPlayer4, testPlayer1 };
            playerRanks = new List<int>() { 1, 2 };
            playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);

            //--make the inactive player5 beat player1 3 times
            players = new List<Player>() { testPlayer5, testPlayer1 };
            playerRanks = new List<int>() { 1, 2 };
            playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);

            players = new List<Player>() { testPlayer5, testPlayer1 };
            playerRanks = new List<int>() { 1, 2 };
            playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);

            players = new List<Player>() { testPlayer5, testPlayer1 };
            playerRanks = new List<int>() { 1, 2 };
            playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);

            //make player 2 be the only one who beat player 5
            players = new List<Player>() { testPlayer2, testPlayer5 };
            playerRanks = new List<int>() { 1, 2 };
            playedGame = CreateTestPlayedGame(players, playerRanks, playedGameLogic);
            testPlayedGames.Add(playedGame);
        }

        private void SavePlayers(NemeStatsDbContext dbContext, int gamingGroupId)
        {
            testPlayer1 = new Player() { Name = testPlayer1Name, Active = true, GamingGroupId = gamingGroupId };
            dbContext.Players.Add(testPlayer1);
            testPlayer2 = new Player() { Name = testPlayer2Name, Active = true, GamingGroupId = gamingGroupId };
            dbContext.Players.Add(testPlayer2);
            testPlayer3 = new Player() { Name = testPlayer3Name, Active = true, GamingGroupId = gamingGroupId };
            dbContext.Players.Add(testPlayer3);
            testPlayer4 = new Player() { Name = testPlayer4Name, Active = true, GamingGroupId = gamingGroupId };
            dbContext.Players.Add(testPlayer4);
            testPlayer5 = new Player() { Name = testPlayer5Name, Active = false, GamingGroupId = gamingGroupId };
            dbContext.Players.Add(testPlayer5);
            testPlayer6 = new Player() { Name = testPlayer6Name, Active = true, GamingGroupId = gamingGroupId };
            dbContext.Players.Add(testPlayer6);

            dbContext.SaveChanges();
        }

        private void SaveGameDefinitions(NemeStatsDbContext dbContext, int gamingGroupId)
        {
            testGameDefinition = new GameDefinition() { Name = testGameName, Description = testGameDescription, GamingGroupId = gamingGroupId };
            dbContext.GameDefinitions.Add(testGameDefinition);
        }

        private void SaveApplicationUsers(NemeStatsDbContext dbContext, int gamingGroupId)
        {
            testApplicationUserWithDefaultGamingGroup = new ApplicationUser()
            {
                Email = "testemail@mailinator.com",
                UserName = testApplicationUserNameForUserWithDefaultGamingGroup,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                CurrentGamingGroupId = gamingGroupId
            };
            dbContext.Users.Add(testApplicationUserWithDefaultGamingGroup);
            dbContext.SaveChanges();

            testApplicationUserWithoutDefaultGamingGroup = new ApplicationUser()
            {
                Email = "otheremail@mailinator.com",
                UserName = testApplicationUserNameForUserWithoutDefaultGamingGroup,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                CurrentGamingGroupId = null
            };
            dbContext.Users.Add(testApplicationUserWithoutDefaultGamingGroup);
            dbContext.SaveChanges();
        }

        private int SaveDefaultGamingGroup(NemeStatsDbContext dbContext)
        {
            gamingGroup = new GamingGroup() { Name = testGamingGroupName };
            dbContext.GamingGroups.Add(gamingGroup);
            dbContext.SaveChanges();
            int gamingGroupId = gamingGroup.Id;
            return gamingGroupId;
        }

        private PlayedGame CreateTestPlayedGame(List<Player> players, List<int> correspondingPlayerRanks, PlayedGameLogic playedGameLogic)
        {
            List<PlayerRank> playerRanks = new List<PlayerRank>();

            for(int i = 0; i < players.Count(); i++)
            {
                playerRanks.Add(new PlayerRank()
                    {
                        PlayerId = players[i].Id,
                        GameRank = correspondingPlayerRanks[i]
                    });
            }

            NewlyCompletedGame newlyCompletedGame = new NewlyCompletedGame() 
                { 
                    GameDefinitionId = testGameDefinition.Id, 
                    PlayerRanks = playerRanks 
                };
            UserContext userContext = new UserContext()
            {
                ApplicationUserId = testApplicationUserWithDefaultGamingGroup.Id,
                GamingGroupId = gamingGroup.Id
            };

            return playedGameLogic.CreatePlayedGame(newlyCompletedGame, userContext);
        }

        private void CleanUpTestData()
        {
            using (NemeStatsDbContext dbContext = new NemeStatsDbContext())
            {
                CleanUpPlayerGameResults(dbContext);
                CleanUpPlayedGames(dbContext);
                CleanUpGameDefinitions(dbContext);
                CleanUpPlayerByPlayerName(testPlayer1Name, dbContext);
                CleanUpPlayerByPlayerName(testPlayer2Name, dbContext);
                CleanUpPlayerByPlayerName(testPlayer3Name, dbContext);
                CleanUpPlayerByPlayerName(testPlayer4Name, dbContext);
                CleanUpPlayerByPlayerName(testPlayer5Name, dbContext);
                CleanUpPlayerByPlayerName(testPlayer6Name, dbContext);
                CleanUpGamingGroup(testGamingGroupName, dbContext);
                CleanUpApplicationUser(testApplicationUserNameForUserWithDefaultGamingGroup, dbContext);
                CleanUpApplicationUser(testApplicationUserNameForUserWithoutDefaultGamingGroup, dbContext);

                dbContext.SaveChanges();
            }
        }

        private void CleanUpApplicationUser(string testApplicationUserName, NemeStatsDbContext dbContext)
        {
            ApplicationUser applicationUserToDelete = (from applicationUser in dbContext.Users
                                               where applicationUser.UserName == testApplicationUserName
                                               select applicationUser).FirstOrDefault();

            if (applicationUserToDelete != null)
            {
                try
                {
                    dbContext.Users.Remove(applicationUserToDelete);
                }
                catch (Exception) { }
            }
        }

        private void CleanUpGamingGroup(string testGamingGroupName, NemeStatsDbContext dbContext)
        {
            GamingGroup gamingGroupToDelete = (from gamingGroup in dbContext.GamingGroups
                                               where gamingGroup.Name == testGamingGroupName
                                               select gamingGroup).FirstOrDefault();

            if(gamingGroupToDelete != null)
            {
                try
                {
                    dbContext.GamingGroups.Remove(gamingGroupToDelete);
                }
                catch (Exception) { }
            }
        }

        private void CleanUpPlayedGames(NemeStatsDbContext dbContext)
        {
            List<PlayedGame> playedGamesToDelete = (from playedGame in dbContext.PlayedGames
                                                    where playedGame.GameDefinition.Name == testGameName
                                                    select playedGame).ToList();

            foreach (PlayedGame playedGame in playedGamesToDelete)
            {
                try
                {
                    dbContext.PlayedGames.Remove(playedGame);
                }
                catch (Exception) { }
            }
        }

        private void CleanUpPlayerGameResults(NemeStatsDbContext dbContext)
        {
            List<PlayerGameResult> playerGameResultsToDelete = (from playerGameResult in dbContext.PlayerGameResults
                                                                where playerGameResult.PlayedGame.GameDefinition.Name == testGameName
                                                                select playerGameResult).ToList();

            foreach (PlayerGameResult playerGameResult in playerGameResultsToDelete)
            {
                try
                {
                    dbContext.PlayerGameResults.Remove(playerGameResult);
                }
                catch (Exception) { }
            }
        }

        private void CleanUpGameDefinitions(NemeStatsDbContext dbContext)
        {
            List<GameDefinition> gameDefinitionsToDelete = (from game in dbContext.GameDefinitions
                                                            where game.Name == testGameName
                                                            select game).ToList();

            foreach (GameDefinition game in gameDefinitionsToDelete)
            {
                try
                {
                    dbContext.GameDefinitions.Remove(game);
                }
                catch (Exception) { }
            }
        }

        private static void CleanUpPlayerByPlayerName(string playerName, NemeStatsDbContext nemeStatsDbContext)
        {
            Player playerToDelete = nemeStatsDbContext.Players.FirstOrDefault(player => player.Name == playerName);

            if (playerToDelete != null)
            {
                try
                {
                    nemeStatsDbContext.Players.Remove(playerToDelete);
                }
                catch (Exception) { }
            }
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            CleanUpTestData();
        }
    }
}
