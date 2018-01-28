﻿using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using WeSketch.BusinessLogic.DTOs;
using WeSketch.BusinessLogic.DTOs.BoardDTOs;
using WeSketch.BusinessLogic.Services;

namespace WeSketch.Server.Communications.Hubs
{
    public class BoardHub : Hub
    {
        private DataService dataService = BusinessLogic.Utilities.ObjectFactory.GetDataService();

        public List<BoardDetailsDTO> GetMyBoards(int userId)
        {
            var message = $"Board list requested from user with id: {userId}";
            Logger.Log(message);
            return dataService.GetAllUserBoards(userId);
        }

        public CreateBoardDto CreateBoard(CreateBoardDto createBoardDto)
        {
            return dataService.CreateAndAttacheBoard(createBoardDto);
        }

        public BoardDetailsDTO GetBoard(int boardId)
        {
            var message = $"Board requested with id: {boardId}";
            Logger.Log(message);
            return dataService.GetBoard(boardId);
        }

        public BoardDetailsDTO UpdateBoardContent(BoardDetailsDTO boardDetailsDTO)
        {
            BoardDetailsDTO board = dataService.UpdateBoardContent(boardDetailsDTO);
            Clients.Others.GetBoardContent(boardDetailsDTO);
            return board;
        }

        public BoardDetailsDTO UpdateBoard(BoardDetailsDTO boardDetailsDTO)
        {
            BoardDetailsDTO board = dataService.UpdateBoard(boardDetailsDTO);
            Clients.Others.UpdateBoardInformation(boardDetailsDTO);
            return board;
        }

        public void DeleteBoard(int boardId)
        {
            dataService.DeleteBoard(boardId);
        }

        public List<UserDetailsDTO> GetCollaborators(int boardId)
        {
            return dataService.GetAllBoardCollaboratros(boardId);
        }

        public void AddCollaborator(CollaboratorDTO collaboratorDTO)
        {
            dataService.AddCollaborator(collaboratorDTO);
            Clients.Others.NotifyCollaboratorAddition(collaboratorDTO);
        }
    }
}
