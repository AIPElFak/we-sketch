﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using WeSketch.App.Data.Shapes;
using Microsoft.AspNet.SignalR.Client;
using System.Net;
using WeSketch.App.Model;
using System.Windows.Threading;
using System.IO;

namespace WeSketch.App.Data.API
{
    public class ApiService : IAPI
    {
        private const string ServerUrl = "http://localhost:15000";
        private HubConnection connection;
        IHubProxy userHub, boardHub, groupsHub;
        IBoardContentObserver workspace;

        public ApiService()
        {
            connection = new HubConnection(ServerUrl);
            var writer = new StreamWriter("ClientLog.txt");
            writer.AutoFlush = true;
            connection.TraceLevel = TraceLevels.All;
            connection.TraceWriter = writer;
            ServicePointManager.DefaultConnectionLimit = 10;
            HubsSetup();
            connection.Start().Wait();
        }

        private void HubsSetup()
        {
            UserHubSetup();
            BoardHubSetup();
            GroupsHubSetup();
        }

        private void GroupsHubSetup()
        {
            groupsHub = connection.CreateHubProxy("GroupRegistrationHub");
        }

        private void UserHubSetup()
        {
            userHub = connection.CreateHubProxy("UserHub");
        }

        private void BoardHubSetup()
        {
            boardHub = connection.CreateHubProxy("BoardHub");
            boardHub.On("NotifyBoardUpdate", () => Dispatcher.CurrentDispatcher.InvokeAsync(() => Display()));
        }

        private void Display() => System.Windows.MessageBox.Show("OK");

        private void ResetConnection() // NEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
        {
            return;
            connection.Stop();
            connection.Start();
        }

        public List<Board> GetMyBoards(User user)
        {
            ResetConnection();
            //
            var id = user.Id;
            var boardsTask = boardHub.Invoke<List<Board>>("GetMyBoards", id);
            boardsTask.Wait();
            var boards = boardsTask.Result;
            return boards;
        }

        public Board GetBoardById(int id)
        {
            ResetConnection();
            //
            var task = boardHub.Invoke<Board>("GetBoard", id);
            task.Wait();
            var board = task.Result;
            return null;
        }

        public User Login(string email, string password)
        {
            ResetConnection();
            //
            var logged = userHub.Invoke<User>("Login", new { Email = email, Password = password });
            logged.Wait();
            return logged.Result;
        }

        public User GetUserByUsername(string username)
        {
            var task = userHub.Invoke<User>("GetUserByUsername", username);
            task.Wait();
            return task.Result;
        }

        public bool Register(UserRegistrationOptions options)
        {
            throw new NotImplementedException();
        }

        public bool CreateBoard(string title, bool isPublic, User user)
        {
            ResetConnection();
            //
            var boardTask = boardHub.Invoke<Board>("CreateBoard", new { PublicBoard = isPublic, Title = title, UserId = user.Id, DateCreated = DateTime.Now});
            boardTask.Wait();
            var board = boardTask.Result;
            return board.Title == title;
        }

        public bool DeleteBoard(Board board, User user)
        {
            ResetConnection();
            //
            var task = boardHub.Invoke("DeleteBoard", board.Id);
            task.Wait();
            return true;
        }

        public void UpdateBoardContent(Board board)
        {
            ResetConnection();
            //
            board.Content = Utilities.ExportShapes(board.Shapes);
            var boardObj = new { Id = board.Id, Content = board.Content };
            var task = boardHub.Invoke<Board>("UpdateBoardContent", boardObj);
            task.Wait();
        }

        public bool AddCollaborator(User user, Board board)
        {
            ResetConnection();
            //
            var task = boardHub.Invoke<Board>("AddCollaborator", new { UserId = user.Id, BoardId = board.Id });
            task.Wait();
            return true;
        }

        public bool RemoveCollaborator(User user, Board board)
        {
            throw new NotImplementedException();
        }

        public List<User> GetBoardCollaborators(Board board)
        {
            ResetConnection();
            //
            var task = boardHub.Invoke<List<User>>("GetCollaborators", board.Id);
            task.Wait();
            return task.Result;
        }

        public void SetObserver(IBoardContentObserver observer)
        {
            workspace = observer;
        }

        public void SubscribeToBoard(Board board)
        {
            var task = groupsHub.Invoke("RegisterToBoardGroup", board.Id);
            task.Wait();            
        }

        public void UnsubscribeFromBoard(Board board)
        {
            var task = groupsHub.Invoke("UnsubscribeFromBoardGroup", board.Id);
            task.Wait();
        }
    }
}