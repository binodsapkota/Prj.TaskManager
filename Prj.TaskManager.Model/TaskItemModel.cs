using System.ComponentModel.DataAnnotations;

namespace Prj.TaskManager.Models
{
    public class TaskItemModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// store relative path for file uploaded
        /// </summary>
        public string? FilePath { get; set; }
    }
}
