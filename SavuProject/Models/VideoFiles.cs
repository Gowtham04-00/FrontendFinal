namespace SavuProject.Models
{
    public class VideoFiles
    {
        public int ID { get; set; }
        public int IsDeleted { get; set; }
        public string Name { get; set; }
        public Nullable<int> FileSize { get; set; }
        public string FilePath { get; set; }
    }
}
