﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WeSketch.BusinessLogic.DTOs.BoardDTOs;
using WeSketch.BusinessLogic.Services;
using WeSketch.BusinessLogic.Utilities;
using WeSketch.DataLayer.Model;
using WeSketch.DataLayer.UnitOfWork;
using WeSketch.BusinessLogic.DTOs;

namespace WeSketch.BackEndTests.Controllers
{
    [RoutePrefix("api/boards")]
    public class BoardsController : ApiController
    {
        #region CRUD tests
        [HttpGet]
        [Route("crud/get/{id:int}")]
        public BoardDetailsDTO Get(int id)
        {
            DataService service = ObjectFactory.GetDataService();
            return service.GetBoard(id);
        }

        [HttpGet]
        [Route("crud/get/all")]
        public List<BoardDetailsDTO> GetAll()
        {
            DataService service = ObjectFactory.GetDataService();
            return service.AllBoards();
        }

        [HttpPost]
        [Route("crud/create")]
        public BoardDetailsDTO Create(CreateBoardDto boardDetails)
        {
            DataService service = ObjectFactory.GetDataService();
            return service.CreateBoard(boardDetails);
        }

        [HttpPut]
        [Route("crud/update")]
        public BoardDetailsDTO Update([FromBody]BoardDetailsDTO board)
        {
            DataService service = ObjectFactory.GetDataService();
            return service.UpdateBoard(board);
        }

        [HttpDelete]
        [Route("crud/delete/{id:int}")]
        public void Delete(int id)
        {
            DataService service = ObjectFactory.GetDataService();
            service.DeleteBoard(id);
        }
        #endregion
        #region Business logic tests
        //TODO make DataService property of controller
        [HttpGet]
        [Route("logic/get/{id:int}")]
        public BoardDetailsDTO GetBoard(int id)
        {
            DataService service = ObjectFactory.GetDataService();
            return service.GetBoard(id);
        }

        [HttpGet]
        [Route("logic/get/all")]
        public List<BoardDetailsDTO> GetAllBoards()
        {
            DataService service = ObjectFactory.GetDataService();
            return service.AllBoards();
        }

        [HttpGet]
        [Route("logic/get/alluserboards/{id:int}")]
        public BoardListDTO GetAllUserBoards(int id)
        {
            DataService service = ObjectFactory.GetDataService();
            BoardListDTO boards = new BoardListDTO();
            boards.Boards = service.GetAllUserBoards(id);
            return boards;
        }

        [HttpPost]
        [Route("logic/create")]
        public CreateBoardDto CreateBoard([FromBody]CreateBoardDto userBoard)
        {
            DataService service = ObjectFactory.GetDataService();
            return service.CreateAndAttacheBoard(userBoard);
        }

        [HttpPost]
        [Route("logic/addcollaborator")]
        public void AddCollaborator([FromBody]CollaboratorDTO collaboratorDTO)
        {
            DataService service = ObjectFactory.GetDataService();
            service.AddCollaborator(collaboratorDTO);
        }

        [HttpPut]
        [Route("logic/update")]
        public BoardDetailsDTO UpdateBoard([FromBody]BoardDetailsDTO board)
        {
            DataService service = ObjectFactory.GetDataService();
            return service.UpdateBoard(board);
        }

        [HttpPut]
        [Route("logic/delete/{id:int}")]
        public void DeleteBoard(int id)
        {
            DataService service = ObjectFactory.GetDataService();
            service.DeleteBoard(id);
        }

        [HttpPut]
        [Route("logic/setpreference")]
        public BoardDetailsDTO SetBoardPreference([FromBody]BoardPreferenceDTO preference)
        {
            DataService service = ObjectFactory.GetDataService();
            return service.SetBoardPreference(preference.UserId, preference.BoardId);
        }
        
        [HttpGet]
        [Route("crud/userboards/{id:int}")]
        public List<UserBoards> UserBoards(int id)
        {
            UnitOfWork unitOfWork = ObjectFactory.GetUnitOfWork();
            return unitOfWork.UserRepository.GetById(id).UserBoards.ToList();            
        }

        [HttpGet]
        [Route("logic/boardcollaborators/{id:int}")]
        public CollaboratorListDTO GetAllBoardCollaboratros(int id)
        {
            DataService service = ObjectFactory.GetDataService();
            var users = service.GetAllBoardCollaboratros(id);
            var list = new CollaboratorListDTO() { Collaborators = users };
            return list;
        }

        [HttpPut]
        [Route("logic/removecollaborator")]
        public void RemoveCollaborator([FromBody]CollaboratorDTO collaboratorDTO)
        {
            DataService service = ObjectFactory.GetDataService();
            service.RemoveCollaboratro(collaboratorDTO);
        }
        #endregion

    }
}
