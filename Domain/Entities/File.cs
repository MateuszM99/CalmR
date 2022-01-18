namespace Domain.Entities
{
    public class File
    {
        public long Id { get; set; }   
        public string Filename { get; set; }
        public string FileExtension { get; set; }
        public byte[] FileContent { get; set; }
    }
}