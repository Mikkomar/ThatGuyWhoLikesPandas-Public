using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.BLL.Models.API;
using TGWLP.DAL.Entities;
using TGWLP.DAL.Interfaces;
using TGWLP.DAL.Repositories;

namespace TGWLP.API.Services
{
    public class StoryService : IService
    {
        private readonly StoryRepository _storyRepository;

        public StoryService(StoryRepository storyRepository) {
            _storyRepository = storyRepository;
        }

        private IEnumerable<StoryAPIModel> MapDataTableToStoryModel(DataTable dataTable) {
            List<StoryAPIModel> storyResults = new List<StoryAPIModel>();

            if (dataTable != null) {
                foreach (DataRow row in dataTable.Rows) {
                    var tmp = new StoryAPIModel();

                    tmp.Id = Guid.Parse(row["Id"].ToString());

                    if (row["Title"] != DBNull.Value) {
                        tmp.Title = row["Title"].ToString();
                    }

                    if (row["Text"] != DBNull.Value) {
                        tmp.Text = row["Text"].ToString();
                    }

                    if (row["PublishDate"] != DBNull.Value) {
                        tmp.PublishDate = DateTime.Parse(row["PublishDate"].ToString());
                    }


                    storyResults.Add(tmp);
                }
            }

            return storyResults;
        }

        /// <summary>
        /// Gets published stories.
        /// </summary>
        /// <returns>A list of published stories.</returns>
        public async Task<IEnumerable<StoryAPIModel>> GetPublishedStoriesAsync() {
            DataTable storyTable = await _storyRepository.GetPublishedStoriesAsync();
            return MapDataTableToStoryModel(storyTable);
        }

        /// <summary>
        /// Gets published stories that match the keyword by Title or Text.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>A list of published stories matching the keyword.</returns>
        public async Task<IEnumerable<StoryAPIModel>> GetPublishedStoriesByKeywordAsync(string keyword) {
            DataTable storyTable = await _storyRepository.GetPublishedStoriesByKeywordAsync(keyword);
            return MapDataTableToStoryModel(storyTable);
        }
    }
}
