namespace CMLabExtim
{
    public class LinkOption
    {
        public LinkOption(int id, string description, string navigateUrl)
        {
            Id = id;
            Description = description;
            NavigateUrl = navigateUrl;
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string NavigateUrl { get; set; }
    }
}