using System;

namespace TGWLP.BLL.Models.API
{
    public class StoryAPIModel
    {
        /// <summary>
        /// The unique identifier
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The story title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The story text including stylings
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Date the story was published
        /// </summary>
        public DateTime? PublishDate { get; set; }
    }
}
