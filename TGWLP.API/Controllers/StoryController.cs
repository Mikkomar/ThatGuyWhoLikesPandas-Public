using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.API.Services;
using TGWLP.BLL.Models.API;
using TGWLP.DAL.Entities;

namespace TGWLP.API.Controllers
{
    /// <summary>
    /// A controller for stories.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly ILogger<StoryController> _logger;
        private readonly StoryService _storyService;

        public StoryController(ILogger<StoryController> logger, StoryService storyService) {
            _logger = logger;
            _storyService = storyService;
        }

        ///<summary>
        /// Gets stories published today or in the past.
        ///</summary>
        /// <response code="200">Returns published stories</response>
        /// <response code="404">If the server encounters an unhandled exception</response>
        [HttpGet("Published")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StoryAPIModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetPublished() {
            try {
                var publishedStories = await _storyService.GetPublishedStoriesAsync();
                return new JsonResult(publishedStories);
            }
            catch(Exception ex) {
                return new NotFoundResult();
            }
        }

        ///<summary>
        /// Gets published stories by keyword.
        ///</summary>
        ///<remarks>
        /// Searches the story database for titles and texts matching the keyword.
        ///</remarks>
        ///<param name="keyword">A string the database search is based on.</param>
        ///<response code="200">Returns published stories matching the keyword</response>
        ///<response code="404">If the server encounters an unhandled exception</response>
        [HttpGet("Search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StoryAPIModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetStoryByKeyword(string keyword = "") {
            try {
                var publishedStories = await _storyService.GetPublishedStoriesByKeywordAsync(keyword);
                return new JsonResult(publishedStories);
            }
            catch (Exception ex) {
                return new NotFoundResult();
            }
        }
    }
}
