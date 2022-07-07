﻿using MegaTravelAPI.IRepository;
using MegaTravelAPI.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Text;

namespace MegaTravelAPI.Data
{
    public class AgentDAL : IAgent
    {
        //context for the database connection
        private readonly MegaTravelContext context;

        public AgentDAL(MegaTravelContext Context)
        {
            context = Context;
        }


        #region Login Agent Method
        public async Task<LoginResponse> LoginAgent(LoginModel tokenData)
        {
            LoginResponse res = new LoginResponse();
            try
            {
                if(tokenData != null)
                {
                    //look for the agent in the database
                    var query = context.Agents
                    .Where(x => x.LoginInfo.UserName == tokenData.Username && x.LoginInfo.Password == tokenData.Password)
                    .FirstOrDefault<Agent>();

                    //if query has a result then we have a match
                    if(query != null)
                    {
                        res.Status = true;
                        res.StatusCode = 200;
                        res.Message = "Login Success";
                        return res;
                    }
                    else
                    {
                        //the user wasn't found or wasn't a match
                        res.Status = false;
                        res.StatusCode = 500;
                        res.Message = "Login Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginAgent --- " + ex.Message);
                res.Status = false;
                res.StatusCode = 500;
            }

            return res;
        }
        #endregion

        #region Find Agent By Name Method

        public async Task<Agent> FindByName(string username)
        {
            Agent agent = null;

            try
            {
                if (username != null)
                {
                    //query the database to find the agent who has this username
                    var query = context.Agents
                        .Where(x => x.LoginInfo.UserName == username)
                        .FirstOrDefault<Agent>();

                    if (query != null)
                    {
                        //set up the object so we can return it
                        agent = new Agent
                        {
                            AgentId = query.AgentId,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            OfficeLocation = query.OfficeLocation,
                            Phone = query.Phone
                            
                        };
                    }

                }

                return agent;


            }
            catch (Exception ex)
            {
                Console.WriteLine("FindByName --- " + ex.Message);
            }

            return agent;
        }
        #endregion



    }
}
