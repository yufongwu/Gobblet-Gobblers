﻿using Wsa.Gaas.GobbletGobblers.Application.Interfaces;
using Wsa.Gaas.GobbletGobblers.Domain;

namespace Wsa.Gaas.GobbletGobblers.Application.UseCases
{
    public class CreateGameUseCase
    {
        public async Task<GameModel> ExecuteAsync(CreateGameRequest request, IRepository repository)
        {
            var gameId = Guid.NewGuid();

            // 查
            var game = repository.Find(gameId);

            if (game != null)
                throw new Exception();
            // 改
            var player = new Player();
            player.Nameself(request.PlayerName);

            game = new Game();
            game.JoinPlayer(player);

            // 存
            var players = game.GetPlayers().Select(x => new PlayerModel
            {
                Id = x.Id,
                Name = x.Name,
                Cocks = x.GetHandAllCock(),
            }).ToList();

            var gameModel = new GameModel
            {
                Id = gameId,
                Board = game.GetBoard(),
                Players = players,
                Lines = game.GetLines(),
            };

            repository.Add(gameId, game);

            // 推

            return await Task.FromResult(gameModel);
        }
    }
}
