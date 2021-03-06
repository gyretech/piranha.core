/*
 * Copyright (c) 2019 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *
 * https://github.com/piranhacms/piranha.core
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Piranha.Manager.Models;
using Piranha.Manager.Services;
using Piranha.Models;

namespace Piranha.Manager.Controllers
{
    /// <summary>
    /// Api controller for site management.
    /// </summary>
    [Area("Manager")]
    [Route("manager/api/site")]
    [Authorize(Policy = Permission.Admin)]
    [ApiController]
    public class SiteApiController : Controller
    {
        private readonly SiteService _service;
        private readonly ManagerLocalizer _localizer;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SiteApiController(SiteService service, ManagerLocalizer localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        /// <summary>
        /// Gets the site with the given id.
        /// </summary>
        /// <param name="id">The unique id</param>
        /// <returns>The page edit model</returns>
        [Route("{id:Guid}")]
        [HttpGet]
        [Authorize(Policy = Permission.SitesEdit)]
        public async Task<SiteEditModel> Get(Guid id)
        {
            return await _service.GetById(id);
        }

        /// <summary>
        /// Gets the site with the given id.
        /// </summary>
        /// <param name="id">The unique id</param>
        /// <returns>The page edit model</returns>
        [Route("save")]
        [HttpPost]
        [Authorize(Policy = Permission.SitesEdit)]
        public async Task<StatusMessage> Save(SiteEditModel model)
        {
            try
            {
                await _service.Save(model);
            }
            catch (ValidationException e)
            {
                // Validation did not succeed
                return new StatusMessage
                {
                    Type = StatusMessage.Error,
                    Body = e.Message
                };
            }
            catch
            {
                return new StatusMessage
                {
                    Type = StatusMessage.Error,
                    Body = _localizer.Site["An error occured while saving the site"]
                };
            }

            return new StatusMessage
            {
                Type = StatusMessage.Success,
                Body = _localizer.Site["The site was successfully saved"]
            };
        }
    }
}