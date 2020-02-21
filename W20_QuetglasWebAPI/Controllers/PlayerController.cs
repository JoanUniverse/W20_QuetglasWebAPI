using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using W20_QuetglasWebAPI.Models;
using Microsoft.AspNet.Identity;

namespace W20_QuetglasWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Player")]
    public class PlayerController : ApiController
    {
        // POST api/Player/RegisterPlayer
        [HttpPost]
        [Route("RegisterPlayer")]
        public IHttpActionResult RegisterPlayer(PlayerModel player)
        {
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = "INSERT INTO dbo.Players(Id, Name, Email, BirthDay) " +
                    $"VALUES ('{player.Id}', '{player.Name}', '{player.Email}', '{player.BirthDay}')";
                try
                {
                    cnn.Execute(sql);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error inserting player in database: " + ex.Message);
                }

                return Ok();
            }
        }

        // GET api/Player/Info
        [HttpGet]
        [Route("Info")]
        public PlayerModel GetPlayerInfo()
        {
            string authenticatedAspNetUserId = RequestContext.Principal.Identity.GetUserId();
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"SELECT Id, Name, Email, BirthDay FROM dbo.Players " +
                    $"WHERE Id LIKE '{authenticatedAspNetUserId}'";
                var player = cnn.Query<PlayerModel>(sql).FirstOrDefault();
                return player;
            }
        }

        // GET api/Player/PlayersOnline
        [HttpGet]
        [Route("PlayersOnline")]
        public List<string> GetPlayersOnline()
        {
            string authenticatedAspNetUserId = RequestContext.Principal.Identity.GetUserId();
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"SELECT UserName FROM dbo.PlayersOnline";
                var players = cnn.Query<string>(sql).ToList();
                return players;
            }
        }

        // GET api/Player/NewPlayerOnline
        [HttpPost]
        [Route("NewPlayerOnline")]
        public IHttpActionResult NewPlayerOnline(PlayerModel player)
        {
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"INSERT INTO dbo.PlayersOnline(UserEmail, UserName) " +
                    $"VALUES ('{player.Email}', '{player.Name}')";
                try
                {
                    cnn.Execute(sql);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error inserting player in database: " + ex.Message);
                }

                return Ok();
            }
        }
    }
}
