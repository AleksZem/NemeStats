﻿using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Logic.BoardGameGeekGameDefinitions;
using X.Web.Sitemap;

namespace BusinessLogic.Jobs.SitemapGenerator
{
    public class UniversalGameSitemapGenerator : IUniversalGameSitemapGenerator
    {
        private readonly IUniversalGameRetriever _universalGameRetriever;
        private readonly ISitemapGenerator _sitemapGenerator;

        public UniversalGameSitemapGenerator(IUniversalGameRetriever universalGameRetriever, ISitemapGenerator sitemapGenerator)
        {
            _universalGameRetriever = universalGameRetriever;
            _sitemapGenerator = sitemapGenerator;
        }

        public List<SitemapInfo> BuildUniversalGamesSitemaps()
        {
            var boardGameGeekGameDefinitionIds = _universalGameRetriever.GetAllActiveBoardGameGeekGameDefinitionIds();

            var urls = boardGameGeekGameDefinitionIds.Select(id => new Url
            {
                ChangeFrequency = ChangeFrequency.Daily,
                Location = $"https://nemestats.com/UniversalGame/Details/{id}",
                Priority = .7,
                TimeStamp = DateTime.UtcNow
            }).ToList();

            _sitemapGenerator.GenerateSitemaps(urls);

            return null;
        }
    }
}