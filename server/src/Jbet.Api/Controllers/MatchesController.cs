﻿using System;
using Jbet.Api.Hateoas.Resources.Base;
using Jbet.Api.Hateoas.Resources.Match;
using Jbet.Core.MatchContext.Queries;
using Jbet.Domain.Views.Match;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Jbet.Api.Hateoas.Resources.Team;
using Jbet.Core.TeamContext.Queries;
using Jbet.Domain;
using Jbet.Domain.Views.Team;
using Optional.Async.Extensions;

namespace Jbet.Api.Controllers
{
    /// <summary>
    /// A controller responsible for matches data.
    /// </summary>
    public class MatchesController : ApiController
    {
        public MatchesController(
            IMediator mediator,
            IResourceMapper resourceMapper)
            : base(mediator, resourceMapper)
        {
        }

        /// <summary>
        /// Display the matches, ordered by date with paging.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A collection of <see cref="MatchView"/></returns>
        /// <response code="200"></response>
        [AllowAnonymous]
        [HttpGet("index", Name = nameof(Index))]
        [ProducesResponseType(typeof(IEnumerable<MatchResource>), (int)HttpStatusCode.OK)]
        public Task<IActionResult> Index([FromQuery] GetAllMatches query) =>
            ResourceContainerResult<MatchView, MatchResource, MatchContainerResource>(query);

        /// <summary>
        /// Retrieves information about the team and its players.
        /// </summary>
        /// <response code="200">The team was found.</response>
        /// <response code="404">The team was not found.</response>
        [HttpGet("{id}", Name = nameof(MatchDetails))]
        [ProducesResponseType(typeof(MatchDetailsResource), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> MatchDetails([FromRoute] Guid id) =>
            (await Mediator.Send(new GetMatchDetails(id))
                .MapAsync(ToResourceAsync<MatchDetailsView, MatchDetailsResource>))
            .Match(Ok, Error);
    }
}