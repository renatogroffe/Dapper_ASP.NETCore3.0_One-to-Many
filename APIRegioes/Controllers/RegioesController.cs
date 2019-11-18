﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using Slapper;
using APIRegioes.Models;

namespace APIRegioes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegioesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Regiao> Get(
             [FromServices]IConfiguration config)
        {
            using (SqlConnection conexao = new SqlConnection(
                config.GetConnectionString("DadosGeograficos")))
            {
                var dados = conexao.Query<dynamic>(
                    "SELECT R.IdRegiao, " +
                           "R.NomeRegiao, " +
                           "E.SiglaEstado AS Estados_SiglaEstado, " +
                           "E.NomeEstado AS Estados_NomeEstado, " +
                           "E.NomeCapital AS Estados_NomeCapital " +
                    "FROM dbo.Regioes R " +
                    "INNER JOIN dbo.Estados E " +
                        "ON E.IdRegiao = R.IdRegiao " +
                    "ORDER BY R.NomeRegiao, E.NomeEstado");

                AutoMapper.Configuration.AddIdentifier(
                    typeof(Regiao), "IdRegiao");
                AutoMapper.Configuration.AddIdentifier(
                    typeof(Estado), "SiglaEstado");

                List<Regiao> regioes = (AutoMapper.MapDynamic<Regiao>(dados)
                    as IEnumerable<Regiao>).ToList();

                return regioes;
            }
        }
    }
}